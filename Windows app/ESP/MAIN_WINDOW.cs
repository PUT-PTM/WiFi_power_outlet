﻿using System;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace ESP
{
	public partial class MainWindow : Form
	{
		/**********************************************************************************************************************************************/
		/* INITIALIZATION OF PROGRAM	*/

		public MainWindow()
		{
			InitializeComponent();
		}

		/**********************************************************************************************************************************************/
		/*	GLOBAB VARIABLES	*/

		private TCP _tcp = new TCP();

		private string Ip { get; set; } = "192.168.4.1";
		private int Port { get; set; } = 80;
		private bool IsConnected { get; set; }

		private bool[] IsSocket { get; } = new bool[2];

		private string ToBeSend { get; set; } = "";

		private string ToBeSendOldValue { get; set; } = "";

		private const int BufferSize = 100;

		/**********************************************************************************************************************************************/
		/*	USEFUL METHODS	*/

		private static void ExceptionHandler(Exception e)
		{
			MessageBox.Show
			(
				e.Message
				+ Environment.NewLine
				+ Environment.NewLine

				+ @"Source:"
				+ Environment.NewLine
				+ e.Source
				+ Environment.NewLine
				+ Environment.NewLine

				+ @"StackTrace:"
				+ Environment.NewLine
				+ e.StackTrace

				, (@"Error - " + DateTime.Now)
			);
		}

		private void SendMsg(string text)
		{
			text += "\r\n";

			var msg = _tcp.Ascii.GetBytes(text);

			_tcp.StreamData.Write(msg, 0, msg.Length);

			Invoke(new Action(delegate { CreateLog(text, DateTime.Now, SENT_MSG_TEXTBOX); }));

			SENT_MSG_TEXTBOX.Invoke(new Action(delegate
			{
				SENT_MSG_TEXTBOX.SelectedIndex = SENT_MSG_TEXTBOX.Items.Count - 1;
				SENT_MSG_TEXTBOX.ClearSelected();
			}));
		}

		private void CreateLog(string msg, DateTime time, ListBox worker)
		{
			try
			{
				_tcp.Log.Add(time.ToString("HH:m:s tt") + ": " + msg + Environment.NewLine);
				worker.Items.Add(_tcp.CreateLog());

				RECIVED_MSG_TEXTBOX.SelectedIndex = RECIVED_MSG_TEXTBOX.Items.Count - 1;
				RECIVED_MSG_TEXTBOX.ClearSelected();
			}
			catch(Exception e)
			{
				ExceptionHandler(e);
			}
		}

		private void SetElementsAvaible(bool connection)
		{
			if(connection)
			{
				ESP_CONNECT_BUTTON.Enabled = true;
				ESP_IP.Enabled = false;
				ESP_PORT.Enabled = false;
				SEND_MSG_RICHBOX.Enabled = true;
				SEND_TO_SERV_BUTTON_CLICK.Enabled = true;
				SOCKET1_BUTTON.Enabled = true;
				SOCKET2_BUTTON.Enabled = true;
			}
			else
			{
				ESP_CONNECT_BUTTON.Enabled = true;
				ESP_IP.Enabled = true;
				ESP_PORT.Enabled = true;
				SEND_MSG_RICHBOX.Enabled = false;
				SEND_TO_SERV_BUTTON_CLICK.Enabled = false;
				SOCKET1_BUTTON.Enabled = false;
				SOCKET2_BUTTON.Enabled = false;
			}
		}

		private void SetSocet(string txt)
		{
			switch(txt[0])
			{
				case '1':
					IsSocket[0] = true;
					break;

				case '0':
					IsSocket[0] = false;
					break;
			}

			switch(txt[1])
			{
				case '1':
					IsSocket[1] = true;
					break;

				case '0':
					IsSocket[1] = false;
					break;
			}
		}

		/**********************************************************************************************************************************************/
		/*	WORKERS	*/

		private void MSG_READER_WORKER_DoWork(object sender, DoWorkEventArgs e)
		{
			while(IsConnected)
			{
				var byteBuffer = new byte[BufferSize];
				var message = "";

				try
				{
					if(MSG_READER_WORKER == null || (!MSG_READER_WORKER.IsBusy || MSG_READER_WORKER.CancellationPending))
					{
						return;
					}

					_tcp.StreamData = _tcp.Tcp.GetStream();
					var numberOfRecived = _tcp.StreamData.Read(byteBuffer, 0, BufferSize);

					for(var i = 0; i < numberOfRecived; i++)
					{
						var character = Convert.ToChar(byteBuffer[i]);
						message += character;
					}

					if(message == "" || message == "\r\n" || message == "\n" || message == Environment.NewLine)
					{
						return;
					}

					Invoke(new Action(delegate { CreateLog(message, DateTime.Now, RECIVED_MSG_TEXTBOX); }));

					if(message.Length == 3)
					{
						SetSocet(message);
					}

					Invoke(new Action(delegate { SOCKET1_STATE_LABEL.Text = (IsSocket[0] ? "ON" : "OFF"); }));
					Invoke(new Action(delegate { SOCKET2_STATE_LABEL.Text = (IsSocket[1] ? "ON" : "OFF"); }));
				}
				catch(Exception ex)
				{
					//Actual do nothing because of threads
					//ExceptionHandler(ex);
				}
			}
		}

		/**********************************************************************************************************************************************/
		/*	BUTTONS	*/

		private void SOCKET1_BUTTON_Click(object sender, EventArgs e)
		{
			if(!IsConnected) return;
			SendMsg(@"setStatus(10)");

			SOCKET1_STATE_LABEL.Text = (!IsSocket[0] ? "ON" : "OFF");
			IsSocket[0] = !IsSocket[0];
		}

		private void SOCKET2_BUTTON_Click(object sender, EventArgs e)
		{
			if(!IsConnected) return;
			SendMsg(@"setStatus(01)");

			SOCKET2_STATE_LABEL.Text = (!IsSocket[1] ? "ON" : "OFF");
			IsSocket[1] = !IsSocket[1];
		}

		private void ESP_CONNECT_BUTTON_Click(object sender, EventArgs e)
		{
			var oldValue = Ip;

			try
			{
				if(ESP_IP.Text != "" && ESP_IP.Text.Length >= 7)
				{
					Ip = IPAddress.Parse(ESP_IP.Text).ToString();
				}

				if(!IsConnected)
				{
					ESP_CONNECT_BUTTON.Text = @"Connecting...";
					ESP_CONNECT_BUTTON.Enabled = false;

					_tcp.Tcp.Connect(Ip, Port);

					IsConnected = true;

					MSG_READER_WORKER.RunWorkerAsync();

					SEND_MSG_RICHBOX.Text = ToBeSendOldValue;

					SetElementsAvaible(IsConnected);
				}
				else
				{
					MSG_READER_WORKER.CancelAsync();

					ESP_CONNECT_BUTTON.Text = @"Disconnecting...";
					ESP_CONNECT_BUTTON.Enabled = false;

					SOCKET1_STATE_LABEL.Text = "-";
					SOCKET2_STATE_LABEL.Text = "-";

					for(var i = 0; i < IsSocket.Length; i++)
					{
						IsSocket[i] = false;
					}

					_tcp.Tcp.Close();

					IsConnected = false;

					_tcp = new TCP();

					ToBeSendOldValue = SEND_MSG_RICHBOX.Text;
					SEND_MSG_RICHBOX.Text = "";

					SetElementsAvaible(IsConnected);
				}
			}
			catch(Exception ex)
			{
				ExceptionHandler(ex);
				ESP_IP.Text = oldValue;
				IsConnected = false;
				SetElementsAvaible(IsConnected);
				SEND_MSG_RICHBOX.Text = "";
			}

			ESP_CONNECT_BUTTON.Text = IsConnected ? "Disconnect" : "Connect";

			if(!IsConnected) return;
			Thread.Sleep(1000);
			SendMsg(@"getStatus()");
		}

		private void SEND_TO_SERV_BUTTON_CLICK_Click(object sender, EventArgs e)
		{
			try
			{
				SendMsg(SEND_MSG_RICHBOX.Text);
			}
			catch (Exception ex)
			{
				ExceptionHandler(ex);
			}
		}

		/**********************************************************************************************************************************************/
		/*	ON TEXT CHANGE DEDICATED METHODS	*/

		private void ESP_PORT_TextChanged(object sender, EventArgs e)
		{
			var oldValue = Port;

			var newValueString = sender.ToString().Split(' ')[2];
			var newValueInt = Convert.ToInt32(newValueString);

			var reg = new Regex(@"[0-9]{1,5}");
			var mat = reg.Match(newValueString);

			try
			{
				if(newValueString != "" && newValueInt < 65535 && mat.Success)
				{
					Port = newValueInt;
				}
				else
				{
					throw new FormatException("Port must be in range 0 through 65535");
				}
			}
			catch(Exception ex)
			{
				ExceptionHandler(ex);
				ESP_PORT.Text = oldValue.ToString();
			}
		}

		private void SEND_MSG_RICHBOX_TextChanged(object sender, EventArgs e)
		{
			ToBeSend = "";
			ToBeSend = SEND_MSG_RICHBOX.Text;
		}

		/**********************************************************************************************************************************************/
		/*	LIST CLICK EVENT HANDLERS	*/

		private void RECIVED_MSG_TEXTBOX_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(RECIVED_MSG_TEXTBOX.SelectedItem.ToString());
		}

		private void SENT_MSG_TEXTBOX_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(RECIVED_MSG_TEXTBOX.SelectedItem.ToString());
		}

		/**********************************************************************************************************************************************/
	}
}