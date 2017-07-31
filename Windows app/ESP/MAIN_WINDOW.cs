using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ESP
{
	public partial class MainWindow : Form
	{
		private TCP _tcp = new TCP();

		private string Ip { get; set; } = "192.168.4.1";
		private int Port { get; set; } = 80;
		private bool IsConnected { get; set; }

		private bool[] IsSocket { get; } = new bool[2];

		private const int BufferSize = 100;

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

		public MainWindow()
		{
			InitializeComponent();
		}

		private void SendMsg(string text)
		{
			text += "\r\n";

			var msg = _tcp.Ascii.GetBytes(text);

			_tcp.StreamData.Write(msg, 0, msg.Length);

			Invoke(new Action(delegate { CreateLog(text, DateTime.Now, SENT_MSG_TEXTBOX); }));

			SENT_MSG_TEXTBOX.Invoke(new Action(delegate
			{
				// Make sure the last item is made visible
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
			}
			catch(Exception e)
			{
				ExceptionHandler(e);
			}
		}

		/**********************************************************************************************************************************************/

		private void SOCKET1_BUTTON_Click(object sender, EventArgs e)
		{
			SendMsg(@"#");

			SOCKET1_STATE_LABEL.Text = (!IsSocket[0] ? "ON" : "OFF");
			IsSocket[0] = !IsSocket[0];

		}

		private void SOCKET2_BUTTON_Click(object sender, EventArgs e)
		{
			SendMsg(@"$");

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
					MSG_READER_WORKER.RunWorkerAsync(0);

					IsConnected = true;

					ESP_CONNECT_BUTTON.Enabled = true;
					ESP_IP.Enabled = false;
					ESP_PORT.Enabled = false;
				}
				else
				{
					MSG_READER_WORKER.CancelAsync();

					ESP_CONNECT_BUTTON.Text = @"Disconnecting...";
					ESP_CONNECT_BUTTON.Enabled = false;

					_tcp.Tcp.Close();

					IsConnected = false;

					ESP_CONNECT_BUTTON.Enabled = true;
					ESP_IP.Enabled = true;
					ESP_PORT.Enabled = true;
				}
			}
			catch(Exception ex)
			{
				ExceptionHandler(ex);
				ESP_IP.Text = oldValue;
				IsConnected = false;
				ESP_CONNECT_BUTTON.Enabled = true;
			}

			ESP_CONNECT_BUTTON.Text = IsConnected ? "Disconnect" : "Connect";


			if (!IsConnected) return;
			Thread.Sleep(1000);
			SendMsg(@"~");
		}

		private void ESP_IP_TextChanged(object sender, EventArgs e)
		{

		}

		private void ESP_PORT_TextChanged(object sender, EventArgs e)
		{
			var oldValue = Port;
			var newValueSTRING = sender.ToString().Split(' ')[2];
			var newValueINT = Convert.ToInt32(newValueSTRING);

			Regex reg = new Regex(@"[0-9]{1,5}");
			Match mat = reg.Match(newValueSTRING);


			try
			{
				if (newValueSTRING != "" && newValueINT < 65535 && mat.Success)
				{
					Port = newValueINT;
				}
				else
				{
					throw new FormatException("Port must be in range 0 through 65535");
				}
			}
			catch (Exception ex)
			{
				ExceptionHandler(ex);
				ESP_PORT.Text = oldValue.ToString();
			}
		}

		private void SOCKET1_STATE_LABEL_Click(object sender, EventArgs e)
		{
		}

		private void SOCKET2_STATE_LABEL_Click(object sender, EventArgs e)
		{
		}

		private void MAIN_WINDOW_Load(object sender, EventArgs e)
		{
		}

		private void Label_Adres_IP_Click(object sender, EventArgs e)
		{
		}

		private void TabPage1_Click(object sender, EventArgs e)
		{
		}

		private void TAB_SOCKETS_Click(object sender, EventArgs e)
		{
		}

		private void RECIVED_MESSAGE_LABEL_Click(object sender, EventArgs e)
		{
		}

		private void RECIVED_MSG_TEXTBOX_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void MSG_READER_WORKER_DoWork(object sender, DoWorkEventArgs e)
		{
			_tcp.StreamData = _tcp.Tcp.GetStream();

			while(IsConnected && _tcp.StreamData != Stream.Null)
			{
				var byteBuffer = new byte[BufferSize];
				var message = "";

				try
				{
					if (MSG_READER_WORKER.IsBusy && !MSG_READER_WORKER.CancellationPending)
					{
						var numberOfRecived = _tcp.StreamData.Read(byteBuffer, 0, BufferSize);

						for (var i = 0; i < numberOfRecived; i++)
						{
							var character = Convert.ToChar(byteBuffer[i]);
							message += character;
						}
						Invoke(new Action(delegate { CreateLog(message, DateTime.Now, RECIVED_MSG_TEXTBOX); }));

						if (message.Length == 4)
						{
							switch (message[0])
							{
								case '1':
									IsSocket[0] = true;
									break;
								case '0':
									IsSocket[0] = false;
									break;
							}

							switch (message[1])
							{
								case '1':
									IsSocket[1] = true;
									break;
								case '0':
									IsSocket[1] = false;
									break;
							}
						}

						Invoke(new Action(delegate { SOCKET1_STATE_LABEL.Text = (IsSocket[0] == true ? "ON" : "OFF"); }));
						Invoke(new Action(delegate { SOCKET2_STATE_LABEL.Text = (IsSocket[1] == true ? "ON" : "OFF"); }));

						// Make sure the last item is made visible
						Invoke(new Action(delegate
						{
							RECIVED_MSG_TEXTBOX.SelectedIndex = RECIVED_MSG_TEXTBOX.Items.Count - 1;
							RECIVED_MSG_TEXTBOX.ClearSelected();
						}));
					}

				}
				catch(Exception ex)
				{
					ExceptionHandler(ex);
				}
			}
		}

		private void SENT_MESSAGE_LABEL_Click(object sender, EventArgs e)
		{
		}
	}
}