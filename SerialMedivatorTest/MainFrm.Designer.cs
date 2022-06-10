namespace SerialMedivatorTest
{
	partial class MainFrm
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
			if (disposing && (components != null))
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
			this.cb_com = new System.Windows.Forms.ComboBox();
			this.lblcom = new System.Windows.Forms.Label();
			this.btn_connect = new System.Windows.Forms.Button();
			this.file_timer = new System.Windows.Forms.Timer(this.components);
			this.tbreceiveddata = new System.Windows.Forms.TextBox();
			this.tbreconstructeddata = new System.Windows.Forms.TextBox();
			this.lblreceiveddata = new System.Windows.Forms.Label();
			this.lblreconstructeddata = new System.Windows.Forms.Label();
			this.btnclearreceived = new System.Windows.Forms.Button();
			this.btnreconstructed = new System.Windows.Forms.Button();
			this.tblog = new System.Windows.Forms.TextBox();
			this.lbllog = new System.Windows.Forms.Label();
			this.btnlog = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cb_com
			// 
			this.cb_com.FormattingEnabled = true;
			this.cb_com.Location = new System.Drawing.Point(83, 12);
			this.cb_com.Name = "cb_com";
			this.cb_com.Size = new System.Drawing.Size(121, 21);
			this.cb_com.TabIndex = 0;
			// 
			// lblcom
			// 
			this.lblcom.AutoSize = true;
			this.lblcom.Location = new System.Drawing.Point(13, 19);
			this.lblcom.Name = "lblcom";
			this.lblcom.Size = new System.Drawing.Size(64, 13);
			this.lblcom.TabIndex = 2;
			this.lblcom.Text = "COM PORT";
			// 
			// btn_connect
			// 
			this.btn_connect.BackColor = System.Drawing.Color.LightGreen;
			this.btn_connect.Location = new System.Drawing.Point(211, 12);
			this.btn_connect.Name = "btn_connect";
			this.btn_connect.Size = new System.Drawing.Size(113, 23);
			this.btn_connect.TabIndex = 3;
			this.btn_connect.Text = "CONNECT";
			this.btn_connect.UseVisualStyleBackColor = false;
			this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
			// 
			// file_timer
			// 
			this.file_timer.Interval = 2000;
			this.file_timer.Tick += new System.EventHandler(this.file_timer_Tick);
			// 
			// tbreceiveddata
			// 
			this.tbreceiveddata.BackColor = System.Drawing.Color.Black;
			this.tbreceiveddata.ForeColor = System.Drawing.Color.Lime;
			this.tbreceiveddata.Location = new System.Drawing.Point(16, 113);
			this.tbreceiveddata.Multiline = true;
			this.tbreceiveddata.Name = "tbreceiveddata";
			this.tbreceiveddata.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbreceiveddata.Size = new System.Drawing.Size(524, 413);
			this.tbreceiveddata.TabIndex = 4;
			// 
			// tbreconstructeddata
			// 
			this.tbreconstructeddata.BackColor = System.Drawing.Color.Black;
			this.tbreconstructeddata.ForeColor = System.Drawing.Color.Lime;
			this.tbreconstructeddata.Location = new System.Drawing.Point(562, 113);
			this.tbreconstructeddata.Multiline = true;
			this.tbreconstructeddata.Name = "tbreconstructeddata";
			this.tbreconstructeddata.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tbreconstructeddata.Size = new System.Drawing.Size(524, 413);
			this.tbreconstructeddata.TabIndex = 5;
			// 
			// lblreceiveddata
			// 
			this.lblreceiveddata.AutoSize = true;
			this.lblreceiveddata.Location = new System.Drawing.Point(13, 88);
			this.lblreceiveddata.Name = "lblreceiveddata";
			this.lblreceiveddata.Size = new System.Drawing.Size(93, 13);
			this.lblreceiveddata.TabIndex = 6;
			this.lblreceiveddata.Text = "RECEIVED DATA";
			// 
			// lblreconstructeddata
			// 
			this.lblreconstructeddata.AutoSize = true;
			this.lblreconstructeddata.Location = new System.Drawing.Point(568, 84);
			this.lblreconstructeddata.Name = "lblreconstructeddata";
			this.lblreconstructeddata.Size = new System.Drawing.Size(136, 13);
			this.lblreconstructeddata.TabIndex = 7;
			this.lblreconstructeddata.Text = "RECONSTRUCTED DATA";
			// 
			// btnclearreceived
			// 
			this.btnclearreceived.Image = ((System.Drawing.Image)(resources.GetObject("btnclearreceived.Image")));
			this.btnclearreceived.Location = new System.Drawing.Point(112, 69);
			this.btnclearreceived.Name = "btnclearreceived";
			this.btnclearreceived.Size = new System.Drawing.Size(43, 43);
			this.btnclearreceived.TabIndex = 8;
			this.btnclearreceived.UseVisualStyleBackColor = true;
			this.btnclearreceived.Click += new System.EventHandler(this.btnclearreceived_Click);
			// 
			// btnreconstructed
			// 
			this.btnreconstructed.Image = ((System.Drawing.Image)(resources.GetObject("btnreconstructed.Image")));
			this.btnreconstructed.Location = new System.Drawing.Point(710, 69);
			this.btnreconstructed.Name = "btnreconstructed";
			this.btnreconstructed.Size = new System.Drawing.Size(43, 43);
			this.btnreconstructed.TabIndex = 9;
			this.btnreconstructed.UseVisualStyleBackColor = true;
			this.btnreconstructed.Click += new System.EventHandler(this.btnreconstructed_Click);
			// 
			// tblog
			// 
			this.tblog.BackColor = System.Drawing.Color.Black;
			this.tblog.ForeColor = System.Drawing.Color.Lime;
			this.tblog.Location = new System.Drawing.Point(12, 582);
			this.tblog.Multiline = true;
			this.tblog.Name = "tblog";
			this.tblog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tblog.Size = new System.Drawing.Size(1070, 258);
			this.tblog.TabIndex = 10;
			// 
			// lbllog
			// 
			this.lbllog.AutoSize = true;
			this.lbllog.Location = new System.Drawing.Point(13, 556);
			this.lbllog.Name = "lbllog";
			this.lbllog.Size = new System.Drawing.Size(29, 13);
			this.lbllog.TabIndex = 11;
			this.lbllog.Text = "LOG";
			// 
			// btnlog
			// 
			this.btnlog.Image = ((System.Drawing.Image)(resources.GetObject("btnlog.Image")));
			this.btnlog.Location = new System.Drawing.Point(112, 533);
			this.btnlog.Name = "btnlog";
			this.btnlog.Size = new System.Drawing.Size(43, 43);
			this.btnlog.TabIndex = 12;
			this.btnlog.UseVisualStyleBackColor = true;
			this.btnlog.Click += new System.EventHandler(this.btnlog_Click);
			// 
			// MainFrm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1116, 858);
			this.Controls.Add(this.btnlog);
			this.Controls.Add(this.lbllog);
			this.Controls.Add(this.tblog);
			this.Controls.Add(this.btnreconstructed);
			this.Controls.Add(this.btnclearreceived);
			this.Controls.Add(this.lblreconstructeddata);
			this.Controls.Add(this.lblreceiveddata);
			this.Controls.Add(this.tbreconstructeddata);
			this.Controls.Add(this.tbreceiveddata);
			this.Controls.Add(this.btn_connect);
			this.Controls.Add(this.lblcom);
			this.Controls.Add(this.cb_com);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainFrm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Serial Medivator Test";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFrm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cb_com;
		private System.Windows.Forms.Label lblcom;
		private System.Windows.Forms.Button btn_connect;
		private System.Windows.Forms.Timer file_timer;
		private System.Windows.Forms.TextBox tbreceiveddata;
		private System.Windows.Forms.TextBox tbreconstructeddata;
		private System.Windows.Forms.Label lblreceiveddata;
		private System.Windows.Forms.Label lblreconstructeddata;
		private System.Windows.Forms.Button btnclearreceived;
		private System.Windows.Forms.Button btnreconstructed;
		private System.Windows.Forms.TextBox tblog;
		private System.Windows.Forms.Label lbllog;
		private System.Windows.Forms.Button btnlog;
	}
}

