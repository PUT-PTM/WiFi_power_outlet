namespace ESP
{
	partial class MainWindow
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ESP_IP = new System.Windows.Forms.TextBox();
			this.ESP_PORT = new System.Windows.Forms.TextBox();
			this.SOCKET1_BUTTON = new System.Windows.Forms.Button();
			this.SOCKET2_BUTTON = new System.Windows.Forms.Button();
			this.SOCKET1_STATE_LABEL = new System.Windows.Forms.Label();
			this.SOCKET2_STATE_LABEL = new System.Windows.Forms.Label();
			this.ESP_CONNECT_BUTTON = new System.Windows.Forms.Button();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.TAB_CONNECTION = new System.Windows.Forms.TabPage();
			this.Label_Port = new System.Windows.Forms.Label();
			this.Label_Adres_IP = new System.Windows.Forms.Label();
			this.TAB_SOCKETS = new System.Windows.Forms.TabPage();
			this.RECIVED_MESSAGE_LABEL = new System.Windows.Forms.Label();
			this.MSG_READER_WORKER = new System.ComponentModel.BackgroundWorker();
			this.RECIVED_MSG_TEXTBOX = new System.Windows.Forms.ListBox();
			this.SENT_MESSAGE_LABEL = new System.Windows.Forms.Label();
			this.SENT_MSG_TEXTBOX = new System.Windows.Forms.ListBox();
			this.SEND_TO_SERV_BUTTON_CLICK = new System.Windows.Forms.Button();
			this.SEND_MSG_RICHBOX = new System.Windows.Forms.RichTextBox();
			this.tabControl1.SuspendLayout();
			this.TAB_CONNECTION.SuspendLayout();
			this.TAB_SOCKETS.SuspendLayout();
			this.SuspendLayout();
			// 
			// ESP_IP
			// 
			this.ESP_IP.Location = new System.Drawing.Point(10, 20);
			this.ESP_IP.Name = "ESP_IP";
			this.ESP_IP.Size = new System.Drawing.Size(95, 20);
			this.ESP_IP.TabIndex = 0;
			this.ESP_IP.Text = "192.168.4.1";
			// 
			// ESP_PORT
			// 
			this.ESP_PORT.Location = new System.Drawing.Point(136, 21);
			this.ESP_PORT.MaxLength = 5;
			this.ESP_PORT.Name = "ESP_PORT";
			this.ESP_PORT.Size = new System.Drawing.Size(70, 20);
			this.ESP_PORT.TabIndex = 1;
			this.ESP_PORT.Text = "80";
			this.ESP_PORT.TextChanged += new System.EventHandler(this.ESP_PORT_TextChanged);
			// 
			// SOCKET1_BUTTON
			// 
			this.SOCKET1_BUTTON.Enabled = false;
			this.SOCKET1_BUTTON.Location = new System.Drawing.Point(10, 10);
			this.SOCKET1_BUTTON.Name = "SOCKET1_BUTTON";
			this.SOCKET1_BUTTON.Size = new System.Drawing.Size(73, 26);
			this.SOCKET1_BUTTON.TabIndex = 2;
			this.SOCKET1_BUTTON.Text = "SOCKET1";
			this.SOCKET1_BUTTON.UseVisualStyleBackColor = true;
			this.SOCKET1_BUTTON.Click += new System.EventHandler(this.SOCKET1_BUTTON_Click);
			// 
			// SOCKET2_BUTTON
			// 
			this.SOCKET2_BUTTON.Enabled = false;
			this.SOCKET2_BUTTON.Location = new System.Drawing.Point(10, 42);
			this.SOCKET2_BUTTON.Name = "SOCKET2_BUTTON";
			this.SOCKET2_BUTTON.Size = new System.Drawing.Size(73, 26);
			this.SOCKET2_BUTTON.TabIndex = 3;
			this.SOCKET2_BUTTON.Text = "SOCKET2";
			this.SOCKET2_BUTTON.UseVisualStyleBackColor = true;
			this.SOCKET2_BUTTON.Click += new System.EventHandler(this.SOCKET2_BUTTON_Click);
			// 
			// SOCKET1_STATE_LABEL
			// 
			this.SOCKET1_STATE_LABEL.AutoSize = true;
			this.SOCKET1_STATE_LABEL.Location = new System.Drawing.Point(90, 17);
			this.SOCKET1_STATE_LABEL.Name = "SOCKET1_STATE_LABEL";
			this.SOCKET1_STATE_LABEL.Size = new System.Drawing.Size(10, 13);
			this.SOCKET1_STATE_LABEL.TabIndex = 4;
			this.SOCKET1_STATE_LABEL.Text = "-";
			// 
			// SOCKET2_STATE_LABEL
			// 
			this.SOCKET2_STATE_LABEL.AutoSize = true;
			this.SOCKET2_STATE_LABEL.Location = new System.Drawing.Point(90, 49);
			this.SOCKET2_STATE_LABEL.Name = "SOCKET2_STATE_LABEL";
			this.SOCKET2_STATE_LABEL.Size = new System.Drawing.Size(10, 13);
			this.SOCKET2_STATE_LABEL.TabIndex = 5;
			this.SOCKET2_STATE_LABEL.Text = "-";
			// 
			// ESP_CONNECT_BUTTON
			// 
			this.ESP_CONNECT_BUTTON.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.ESP_CONNECT_BUTTON.Location = new System.Drawing.Point(6, 46);
			this.ESP_CONNECT_BUTTON.Name = "ESP_CONNECT_BUTTON";
			this.ESP_CONNECT_BUTTON.Size = new System.Drawing.Size(200, 30);
			this.ESP_CONNECT_BUTTON.TabIndex = 6;
			this.ESP_CONNECT_BUTTON.Text = "Connect";
			this.ESP_CONNECT_BUTTON.UseVisualStyleBackColor = true;
			this.ESP_CONNECT_BUTTON.Click += new System.EventHandler(this.ESP_CONNECT_BUTTON_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.TAB_CONNECTION);
			this.tabControl1.Controls.Add(this.TAB_SOCKETS);
			this.tabControl1.Location = new System.Drawing.Point(13, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(225, 110);
			this.tabControl1.TabIndex = 8;
			// 
			// TAB_CONNECTION
			// 
			this.TAB_CONNECTION.Controls.Add(this.Label_Port);
			this.TAB_CONNECTION.Controls.Add(this.Label_Adres_IP);
			this.TAB_CONNECTION.Controls.Add(this.ESP_CONNECT_BUTTON);
			this.TAB_CONNECTION.Controls.Add(this.ESP_IP);
			this.TAB_CONNECTION.Controls.Add(this.ESP_PORT);
			this.TAB_CONNECTION.Location = new System.Drawing.Point(4, 22);
			this.TAB_CONNECTION.Name = "TAB_CONNECTION";
			this.TAB_CONNECTION.Padding = new System.Windows.Forms.Padding(3);
			this.TAB_CONNECTION.Size = new System.Drawing.Size(217, 84);
			this.TAB_CONNECTION.TabIndex = 0;
			this.TAB_CONNECTION.Text = "Connection";
			this.TAB_CONNECTION.UseVisualStyleBackColor = true;
			// 
			// Label_Port
			// 
			this.Label_Port.AutoSize = true;
			this.Label_Port.Location = new System.Drawing.Point(133, 3);
			this.Label_Port.Name = "Label_Port";
			this.Label_Port.Size = new System.Drawing.Size(29, 13);
			this.Label_Port.TabIndex = 8;
			this.Label_Port.Text = "Port:";
			// 
			// Label_Adres_IP
			// 
			this.Label_Adres_IP.AutoSize = true;
			this.Label_Adres_IP.Dock = System.Windows.Forms.DockStyle.Top;
			this.Label_Adres_IP.Location = new System.Drawing.Point(3, 3);
			this.Label_Adres_IP.Name = "Label_Adres_IP";
			this.Label_Adres_IP.Size = new System.Drawing.Size(60, 13);
			this.Label_Adres_IP.TabIndex = 7;
			this.Label_Adres_IP.Text = "IP address:";
			// 
			// TAB_SOCKETS
			// 
			this.TAB_SOCKETS.Controls.Add(this.SOCKET1_BUTTON);
			this.TAB_SOCKETS.Controls.Add(this.SOCKET2_STATE_LABEL);
			this.TAB_SOCKETS.Controls.Add(this.SOCKET2_BUTTON);
			this.TAB_SOCKETS.Controls.Add(this.SOCKET1_STATE_LABEL);
			this.TAB_SOCKETS.Location = new System.Drawing.Point(4, 22);
			this.TAB_SOCKETS.Name = "TAB_SOCKETS";
			this.TAB_SOCKETS.Padding = new System.Windows.Forms.Padding(3);
			this.TAB_SOCKETS.Size = new System.Drawing.Size(217, 84);
			this.TAB_SOCKETS.TabIndex = 1;
			this.TAB_SOCKETS.Text = "Sockets";
			this.TAB_SOCKETS.UseVisualStyleBackColor = true;
			// 
			// RECIVED_MESSAGE_LABEL
			// 
			this.RECIVED_MESSAGE_LABEL.AutoSize = true;
			this.RECIVED_MESSAGE_LABEL.Location = new System.Drawing.Point(492, 9);
			this.RECIVED_MESSAGE_LABEL.Name = "RECIVED_MESSAGE_LABEL";
			this.RECIVED_MESSAGE_LABEL.Size = new System.Drawing.Size(111, 13);
			this.RECIVED_MESSAGE_LABEL.TabIndex = 10;
			this.RECIVED_MESSAGE_LABEL.Text = "Received from server:";
			// 
			// MSG_READER_WORKER
			// 
			this.MSG_READER_WORKER.WorkerSupportsCancellation = true;
			this.MSG_READER_WORKER.DoWork += new System.ComponentModel.DoWorkEventHandler(this.MSG_READER_WORKER_DoWork);
			// 
			// RECIVED_MSG_TEXTBOX
			// 
			this.RECIVED_MSG_TEXTBOX.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.RECIVED_MSG_TEXTBOX.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.RECIVED_MSG_TEXTBOX.FormattingEnabled = true;
			this.RECIVED_MSG_TEXTBOX.Location = new System.Drawing.Point(495, 25);
			this.RECIVED_MSG_TEXTBOX.Name = "RECIVED_MSG_TEXTBOX";
			this.RECIVED_MSG_TEXTBOX.Size = new System.Drawing.Size(229, 199);
			this.RECIVED_MSG_TEXTBOX.TabIndex = 11;
			this.RECIVED_MSG_TEXTBOX.Click += new System.EventHandler(this.RECIVED_MSG_TEXTBOX_Click);
			// 
			// SENT_MESSAGE_LABEL
			// 
			this.SENT_MESSAGE_LABEL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SENT_MESSAGE_LABEL.AutoSize = true;
			this.SENT_MESSAGE_LABEL.Location = new System.Drawing.Point(263, 9);
			this.SENT_MESSAGE_LABEL.Name = "SENT_MESSAGE_LABEL";
			this.SENT_MESSAGE_LABEL.Size = new System.Drawing.Size(76, 13);
			this.SENT_MESSAGE_LABEL.TabIndex = 14;
			this.SENT_MESSAGE_LABEL.Text = "Sent to server:";
			// 
			// SENT_MSG_TEXTBOX
			// 
			this.SENT_MSG_TEXTBOX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.SENT_MSG_TEXTBOX.FormattingEnabled = true;
			this.SENT_MSG_TEXTBOX.Location = new System.Drawing.Point(266, 25);
			this.SENT_MSG_TEXTBOX.Name = "SENT_MSG_TEXTBOX";
			this.SENT_MSG_TEXTBOX.Size = new System.Drawing.Size(200, 199);
			this.SENT_MSG_TEXTBOX.TabIndex = 15;
			this.SENT_MSG_TEXTBOX.Click += new System.EventHandler(this.SENT_MSG_TEXTBOX_Click);
			// 
			// SEND_TO_SERV_BUTTON_CLICK
			// 
			this.SEND_TO_SERV_BUTTON_CLICK.Enabled = false;
			this.SEND_TO_SERV_BUTTON_CLICK.Location = new System.Drawing.Point(163, 128);
			this.SEND_TO_SERV_BUTTON_CLICK.Name = "SEND_TO_SERV_BUTTON_CLICK";
			this.SEND_TO_SERV_BUTTON_CLICK.Size = new System.Drawing.Size(75, 95);
			this.SEND_TO_SERV_BUTTON_CLICK.TabIndex = 16;
			this.SEND_TO_SERV_BUTTON_CLICK.Text = "Send to server";
			this.SEND_TO_SERV_BUTTON_CLICK.UseVisualStyleBackColor = true;
			this.SEND_TO_SERV_BUTTON_CLICK.Click += new System.EventHandler(this.SEND_TO_SERV_BUTTON_CLICK_Click);
			// 
			// SEND_MSG_RICHBOX
			// 
			this.SEND_MSG_RICHBOX.Enabled = false;
			this.SEND_MSG_RICHBOX.Location = new System.Drawing.Point(12, 128);
			this.SEND_MSG_RICHBOX.Name = "SEND_MSG_RICHBOX";
			this.SEND_MSG_RICHBOX.Size = new System.Drawing.Size(145, 95);
			this.SEND_MSG_RICHBOX.TabIndex = 17;
			this.SEND_MSG_RICHBOX.Text = "";
			this.SEND_MSG_RICHBOX.TextChanged += new System.EventHandler(this.SEND_MSG_RICHBOX_TextChanged);
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(735, 246);
			this.Controls.Add(this.SEND_MSG_RICHBOX);
			this.Controls.Add(this.SEND_TO_SERV_BUTTON_CLICK);
			this.Controls.Add(this.SENT_MSG_TEXTBOX);
			this.Controls.Add(this.SENT_MESSAGE_LABEL);
			this.Controls.Add(this.RECIVED_MSG_TEXTBOX);
			this.Controls.Add(this.RECIVED_MESSAGE_LABEL);
			this.Controls.Add(this.tabControl1);
			this.Name = "MainWindow";
			this.Text = "ESP Connection graphical interface";
			this.tabControl1.ResumeLayout(false);
			this.TAB_CONNECTION.ResumeLayout(false);
			this.TAB_CONNECTION.PerformLayout();
			this.TAB_SOCKETS.ResumeLayout(false);
			this.TAB_SOCKETS.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox ESP_IP;
		private System.Windows.Forms.TextBox ESP_PORT;
		private System.Windows.Forms.Button SOCKET1_BUTTON;
		private System.Windows.Forms.Button SOCKET2_BUTTON;
		private System.Windows.Forms.Label SOCKET1_STATE_LABEL;
		private System.Windows.Forms.Label SOCKET2_STATE_LABEL;
		private System.Windows.Forms.Button ESP_CONNECT_BUTTON;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage TAB_CONNECTION;
		private System.Windows.Forms.Label Label_Port;
		private System.Windows.Forms.Label Label_Adres_IP;
		private System.Windows.Forms.TabPage TAB_SOCKETS;
		private System.Windows.Forms.Label RECIVED_MESSAGE_LABEL;
		private System.ComponentModel.BackgroundWorker MSG_READER_WORKER;
		private System.Windows.Forms.ListBox RECIVED_MSG_TEXTBOX;
		private System.Windows.Forms.Label SENT_MESSAGE_LABEL;
		private System.Windows.Forms.ListBox SENT_MSG_TEXTBOX;
		private System.Windows.Forms.Button SEND_TO_SERV_BUTTON_CLICK;
		private System.Windows.Forms.RichTextBox SEND_MSG_RICHBOX;
	}
}

