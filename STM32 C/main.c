/*
 *   Cable connections:
 *
 * +=========+=========+           +----------------+
 * |  STM32  | ESP8266 |           | +-+ +-+ +-+ +- |
 * +=========+=========+           | | | | | | | |  |
 * | VCC 3.3V|   VCC   |           | | +-+ +-+ +-+  |
 * |   GND   |   GND   |           | +------------- |
 * | PC10 Tx |   Rx    |           | |  ESP8266-01  |
 * | PC11 Rx |   Tx    |         .-|-----.    .-----|-.
 * | VCC 3.3V|  CH_PD  | GPIO2 o-  |GND GPI2 GPI0 RX|  -o GPIO0
 * +---------+---------+ GND   o---|--O   O   O   O-|---o Rx
 *                       Tx    o---|--O   O   O   O-|---o VCC
 *                       CH_PD o-  |TX CH_PD RST VCC| .-o RST
 *                               \ +----------------+ |
 *                                `-------'    `------'
 *
 *  +=========+=========+
 *  |  STM32  |RelModule| SRD-05VDC-SL-C
 *  +=========+=========+ ON on LOW / OFF on HIGH
 *  |  VCC 5V |   VCC   |
 *  |   GND   |   GND   |
 *  |   PE12  |   IN1   |
 *  |   PE13  |   IN2   |
 *  +---------+---------+
 *
 *
 *   Important information:
 *   1. All messages must be ended with Line Feed (a.k.a. '\n') sign.
 *
 *   To be done:
 *   3) Change ints to uint_xs
 */


/* ----includes&defines---- */
#include "misc.h"
#include "stm32f4xx_exti.h"
#include "stm32f4xx_gpio.h"
#include "stm32f4xx_rcc.h"
#include "stm32f4xx_syscfg.h"
#include "stm32f4xx_usart.h"
#include "stm32f4xx_conf.h"

#include <string.h> //strstr(), strcpy(), strcat()...

#define MaxMsgSize 64

/* ----global variables---- */
char recvUARTmsg[MaxMsgSize] = {0};
uint8_t recvUARTmsgPos = 0;

/* ------declarations------ */
int main(void);
void configureUART(void);
void configureRelays(void);
void USART3_IRQHandler(void);
void handleMessage(char *);
void recvUARTmsgFlush(void);
void initESP(void);
void delay(uint32_t);
void sendUARTmsg(char*);
uint16_t lengthOfString(char *);
void getStatus(void);
void setStatus(uint8_t, uint8_t);

/* ------definitions------- */

/* Main function */
int main(void){
	SystemInit(); //STM32F4 initialization

	configureUART();
	configureRelays();
	initESP();

	while(1){
		
	}
}

/* Configures UART 3 for ESP8266 connectivity */
void configureUART(void){
	//Interrupt (NVIC) priority
	NVIC_PriorityGroupConfig(NVIC_PriorityGroup_1);

	//GPIO C peripheral - enabling clock
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOC, ENABLE);
	//UART 3 peripheral - enabling clock
	RCC_APB1PeriphClockCmd(RCC_APB1Periph_USART3, ENABLE);

	//GPIO C init structure - filling with data
	GPIO_InitTypeDef GPIO_InitStructure;
	GPIO_InitStructure.GPIO_Pin = GPIO_Pin_10 | GPIO_Pin_11;
	GPIO_InitStructure.GPIO_Mode = GPIO_Mode_AF;
	GPIO_InitStructure.GPIO_OType = GPIO_OType_PP;
	GPIO_InitStructure.GPIO_PuPd = GPIO_PuPd_UP;
	GPIO_InitStructure.GPIO_Speed = GPIO_Speed_50MHz;
	GPIO_Init(GPIOC, &GPIO_InitStructure);

	//UART setting pins as alternate function
	GPIO_PinAFConfig(GPIOC, GPIO_PinSource10, GPIO_AF_USART3);
	GPIO_PinAFConfig(GPIOC, GPIO_PinSource11, GPIO_AF_USART3);

	//UART 3 init structure - filling with data
	USART_InitTypeDef USART_InitStructure;
	USART_InitStructure.USART_BaudRate = 115200;
	USART_InitStructure.USART_WordLength = USART_WordLength_8b;
	USART_InitStructure.USART_StopBits = USART_StopBits_1;
	USART_InitStructure.USART_Parity = USART_Parity_No;
	USART_InitStructure.USART_HardwareFlowControl = USART_HardwareFlowControl_None;
	USART_InitStructure.USART_Mode = USART_Mode_Rx | USART_Mode_Tx;
	USART_Init(USART3, &USART_InitStructure);

	//NVIC init structure - filling with data
	NVIC_InitTypeDef NVIC_InitStructure;
	USART_ITConfig(USART3, USART_IT_RXNE, ENABLE);
	NVIC_InitStructure.NVIC_IRQChannel = USART3_IRQn;
	NVIC_InitStructure.NVIC_IRQChannelPreemptionPriority = 0x01;
	NVIC_InitStructure.NVIC_IRQChannelSubPriority = 0x01;
	NVIC_InitStructure.NVIC_IRQChannelCmd = ENABLE;
	NVIC_Init(&NVIC_InitStructure);

	//UART interrupt
	NVIC_EnableIRQ(USART3_IRQn);

	//UART start
	USART_Cmd(USART3, ENABLE);
}

/* Configures GPIO E for relays */
void configureRelays(){
	//GPIOE Peripheral Clock Enable
	RCC_AHB1PeriphClockCmd(RCC_AHB1Periph_GPIOE, ENABLE);

	GPIO_InitTypeDef GPIO_Rel_IS;
	GPIO_Rel_IS.GPIO_Pin = GPIO_Pin_12 | GPIO_Pin_13;
	GPIO_Rel_IS.GPIO_Mode = GPIO_Mode_OUT;
	GPIO_Rel_IS.GPIO_OType = GPIO_OType_OD;
	GPIO_Rel_IS.GPIO_Speed = GPIO_Speed_100MHz;
	GPIO_Rel_IS.GPIO_PuPd = GPIO_PuPd_DOWN;
	GPIO_Init(GPIOE, &GPIO_Rel_IS);

	//reset both sockets to LOW
	GPIO_SetBits(GPIOE, GPIO_Pin_12);
	GPIO_SetBits(GPIOE, GPIO_Pin_13);
}

/* UART interrupt handler (message receiving) */
void USART3_IRQHandler(void){

	uint16_t buff = 0;

	if(USART_GetITStatus(USART3, USART_IT_RXNE) != RESET){

		//put received UART sign to recvUARTmsg
		buff = USART3->DR;
		recvUARTmsg[recvUARTmsgPos] = (char)buff;

		//if last read char is \n then handle whole message
		if(buff == '\n'){
			//message handler
			handleMessage(recvUARTmsg);

			//message buffer flasher
			recvUARTmsgFlush();
		} else {
			//increase recvUARTmsg position
			recvUARTmsgPos++;
		}
	}
}

/* UART received message handler */
void handleMessage(char *msg){
	char tempInp1[1] = {0};
	char tempInp2[1] = {0};
	int inp1 = 0;
	int inp2 = 0;
	uint16_t lenOfMsg = lengthOfString(msg);

	delay(5);
	if(strstr(msg, "getStatus()") != NULL){
		getStatus();
	}
	else if(strstr(msg, "setStatus(") != NULL){
		//learning the states from msg
		tempInp1[0] = msg[lenOfMsg-5];
		tempInp2[0] = msg[lenOfMsg-4];
		inp1 = atoi(tempInp1);
		inp2 = atoi(tempInp2);

		setStatus(inp1, inp2);
	}
}

/* Flushing UART receiving buffer [NOT USED IN THIS VERSION] */
void recvUARTmsgFlush(){
	//clear recvUARTmsg[]
	for(uint8_t i = 0; i<MaxMsgSize; i++){
		recvUARTmsg[i] = 0;
	}

	//moving cursor back to position zero
	recvUARTmsgPos = 0;
}

/* Connect to WiFi network and starts server */
void initESP(void){
	delay(10);

	//Quit AP if connected to any
	sendUARTmsg("AT+CWQAP\r\n");
	delay(100);

	//Switch to Station Mode (creates own AI-Thinker WiFi network)
	sendUARTmsg("AT+CWMODE=2\r\n");
	delay(100);

	//Checks if mode changed (debug only)
	//sendUARTmsg("AT+CWMODE?\r\n");
	//delay(100);

	//Allows multiple TCP/UDP connections at once
	sendUARTmsg("AT+CIPMUX=1\r\n");
	delay(100);

	//Checks if multiple connections availabled (debug only)
	//sendUARTmsg("AT+CIPMUX?\r\n");
	//delay(100);

	//Initiates a webserver on port 80
	sendUARTmsg("AT+CIPSERVER=1,80\r\n");
	delay(100);

	//Returns IP & MAC addresses via USART (debug only)
	//sendUARTmsg("AT+CIFSR\r\n");
	//delay(100);
}

/* Delays inside */
void delay(volatile uint32_t s){
	s *= 24;
	while(s--);
}

/* Sends message via UART (last sign must be NULL sign, which is not send) */
void sendUARTmsg(char msg[MaxMsgSize]){
	for(uint8_t i=0; msg[i] != '\0'; i++){
		USART_SendData(USART3, msg[i]);
		delay(500);
	}
}

/* Returns length of given string in chars [NOT USED IN THIS VERSION]*/
uint16_t lengthOfString(char *str){
	uint16_t NOsigns = 0;
	while(str[NOsigns] != '\n'){
		NOsigns++;
	}
	return (NOsigns+1);
}

/* Sends current sockets' status via WiFi */
void getStatus(){
	if(!GPIO_ReadOutputDataBit(GPIOE, GPIO_Pin_12)){
		if(!GPIO_ReadOutputDataBit(GPIOE, GPIO_Pin_13)){
			sendUARTmsg("AT+CIPSEND=0,3\r\n");
			delay(10);
			sendUARTmsg("11\n");
		}
		else{
			sendUARTmsg("AT+CIPSEND=0,3\r\n");
			delay(10);
			sendUARTmsg("10\n");
		}
	}
	else{
		if(!GPIO_ReadOutputDataBit(GPIOE, GPIO_Pin_13)){
			sendUARTmsg("AT+CIPSEND=0,3\r\n");
			delay(10);
			sendUARTmsg("01\n");
		}
		else{
			sendUARTmsg("AT+CIPSEND=0,3\r\n");
			delay(10);
			sendUARTmsg("00\n");
		}
	}
}

/* Something does not work here (minor error with casting probably) */
void setStatus(uint8_t input1, uint8_t input2){
	if(input1){
		sendUARTmsg("AT+CIPSEND=0,19\r\n");
		delay(10);
		sendUARTmsg("1st socket toggled\n");

		//toggle bit
		GPIO_ToggleBits(GPIOE, GPIO_Pin_12);
	}
	if(input2){
		sendUARTmsg("AT+CIPSEND=0,19\r\n");
		delay(10);
		sendUARTmsg("2nd socket toggled\n");

		//toggle bit
		GPIO_ToggleBits(GPIOE, GPIO_Pin_13);
	}

	//Send status after switching
//	delay(100);
//	getStatus();

}

/*                  --------======BONUS======--------

    +--------------|^|--------------+   bb  = breadboard
    |  :           |_|           :  |   -E- = one cable in front of the other
    |              ___              |
    |  :     A    |   |   :         |
bb  |  :     V    |___|   :         |
GND |  :                            |
|   |                               |
+-GND*::          <<>>           :: |
    | ::   .     _______         :: |
    | ::   ^    |       |        :: |
    | ::        | STM32 |        :: |           [bb GND]
    | ::        |_______|        :: |              |            for debug only
    | ::                         :: |              |            disconnect Tx
    | ::    =o=           =o=    :: |             GND           when using ESP8266
    | ::_PE12  __                :: |               \   +-----ooooo------+
+PE13*::    | |__|        +--------------[bb]---TxD---.=:     _____  .  +----+
|   | ::     \            +-PC10_::_PC11--E--[bb]-RxD-*=:    |_____| :  |  : |
|   | ::   __ \                  :: |     |    |       =:     FT232     +----+
|   | ::  |  | |                 :: |     |    |        +-----ooooo------+
|   | ::  |  | |     _           :: |     +----E--.
|   +-----|  |-|----|_|-------------+          |  (2)  +--[bb 3V]
|          ||  |                           +---+    \ (3)
|              |  GND--[bb GND]            |         \ |
 \             |   \ +----------------+    |        |oooo| < 1)GPIO0; 2)Rx; 3)VCC; 4)RST
  \            |    \|    +-------+   |    |  +--------+ |
   \           +-IN1_|:   | RELAY |8  |    |  ||| 82 ::| |
    *------------IN2*|:   +=======+8  |    |  |== 66 ::| |
                    /|8   |MODULES|8  |    |  +--------+ |
                   / |o   +-------+   |    |        |oooo| < 1)GPIO2; 2)GND; 3)Tx; 4) CH_PD
                 VCC +----------------+    |          /|\
                  |      SRD-05VDC-SL-C    +----(3)--E-+ \_(4)_[bb 3V]
               [bb 3V]                               |
                                                    (2)---[bb GND]

*/
