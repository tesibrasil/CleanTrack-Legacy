namespace MDGTest
{
	partial class FrmMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
			this.lblip = new System.Windows.Forms.Label();
			this.nudip1 = new System.Windows.Forms.NumericUpDown();
			this.lblpunto1 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.nudip2 = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.nudip3 = new System.Windows.Forms.NumericUpDown();
			this.nudip4 = new System.Windows.Forms.NumericUpDown();
			this.lblport = new System.Windows.Forms.Label();
			this.nudport = new System.Windows.Forms.NumericUpDown();
			this.lblcmdtosend = new System.Windows.Forms.Label();
			this.cbcmdtosend = new System.Windows.Forms.ComboBox();
			this.btnsend = new System.Windows.Forms.Button();
			this.btncheckconnection = new System.Windows.Forms.Button();
			this.tberrors = new System.Windows.Forms.TextBox();
			this.lbltraceedit = new System.Windows.Forms.Label();
			this.nudmacid = new System.Windows.Forms.NumericUpDown();
			this.lblmacid = new System.Windows.Forms.Label();
			this.tbcommand = new System.Windows.Forms.TextBox();
			this.tbchecksum = new System.Windows.Forms.TextBox();
			this.btngetchecksum = new System.Windows.Forms.Button();
			this.lblcommand = new System.Windows.Forms.Label();
			this.lblchksum = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudip1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudip2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudip3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudip4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudport)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudmacid)).BeginInit();
			this.SuspendLayout();
			// 
			// lblip
			// 
			this.lblip.AutoSize = true;
			this.lblip.Location = new System.Drawing.Point(14, 50);
			this.lblip.Name = "lblip";
			this.lblip.Size = new System.Drawing.Size(72, 13);
			this.lblip.TabIndex = 0;
			this.lblip.Text = "IP ADDRESS";
			// 
			// nudip1
			// 
			this.nudip1.Location = new System.Drawing.Point(92, 48);
			this.nudip1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudip1.Name = "nudip1";
			this.nudip1.Size = new System.Drawing.Size(50, 20);
			this.nudip1.TabIndex = 1;
			// 
			// lblpunto1
			// 
			this.lblpunto1.AutoSize = true;
			this.lblpunto1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
			this.lblpunto1.Location = new System.Drawing.Point(146, 40);
			this.lblpunto1.Name = "lblpunto1";
			this.lblpunto1.Size = new System.Drawing.Size(22, 31);
			this.lblpunto1.TabIndex = 2;
			this.lblpunto1.Text = ".";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
			this.label1.Location = new System.Drawing.Point(228, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(22, 31);
			this.label1.TabIndex = 4;
			this.label1.Text = ".";
			// 
			// nudip2
			// 
			this.nudip2.Location = new System.Drawing.Point(174, 48);
			this.nudip2.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudip2.Name = "nudip2";
			this.nudip2.Size = new System.Drawing.Size(50, 20);
			this.nudip2.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
			this.label2.Location = new System.Drawing.Point(310, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(22, 31);
			this.label2.TabIndex = 6;
			this.label2.Text = ".";
			// 
			// nudip3
			// 
			this.nudip3.Location = new System.Drawing.Point(256, 48);
			this.nudip3.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudip3.Name = "nudip3";
			this.nudip3.Size = new System.Drawing.Size(50, 20);
			this.nudip3.TabIndex = 5;
			// 
			// nudip4
			// 
			this.nudip4.Location = new System.Drawing.Point(338, 48);
			this.nudip4.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.nudip4.Name = "nudip4";
			this.nudip4.Size = new System.Drawing.Size(50, 20);
			this.nudip4.TabIndex = 7;
			// 
			// lblport
			// 
			this.lblport.AutoSize = true;
			this.lblport.Location = new System.Drawing.Point(411, 50);
			this.lblport.Name = "lblport";
			this.lblport.Size = new System.Drawing.Size(37, 13);
			this.lblport.TabIndex = 8;
			this.lblport.Text = "PORT";
			// 
			// nudport
			// 
			this.nudport.Location = new System.Drawing.Point(454, 48);
			this.nudport.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.nudport.Name = "nudport";
			this.nudport.Size = new System.Drawing.Size(81, 20);
			this.nudport.TabIndex = 9;
			// 
			// lblcmdtosend
			// 
			this.lblcmdtosend.AutoSize = true;
			this.lblcmdtosend.Location = new System.Drawing.Point(17, 129);
			this.lblcmdtosend.Name = "lblcmdtosend";
			this.lblcmdtosend.Size = new System.Drawing.Size(114, 13);
			this.lblcmdtosend.TabIndex = 10;
			this.lblcmdtosend.Text = "COMMAND TO SEND";
			// 
			// cbcmdtosend
			// 
			this.cbcmdtosend.FormattingEnabled = true;
			this.cbcmdtosend.Location = new System.Drawing.Point(152, 126);
			this.cbcmdtosend.Name = "cbcmdtosend";
			this.cbcmdtosend.Size = new System.Drawing.Size(154, 21);
			this.cbcmdtosend.TabIndex = 11;
			this.cbcmdtosend.SelectedIndexChanged += new System.EventHandler(this.cbcmdtosend_SelectedIndexChanged);
			// 
			// btnsend
			// 
			this.btnsend.Enabled = false;
			this.btnsend.Location = new System.Drawing.Point(316, 124);
			this.btnsend.Name = "btnsend";
			this.btnsend.Size = new System.Drawing.Size(75, 23);
			this.btnsend.TabIndex = 12;
			this.btnsend.Text = "SEND";
			this.btnsend.UseVisualStyleBackColor = true;
			this.btnsend.Click += new System.EventHandler(this.btnsend_Click);
			// 
			// btncheckconnection
			// 
			this.btncheckconnection.Location = new System.Drawing.Point(555, 45);
			this.btncheckconnection.Name = "btncheckconnection";
			this.btncheckconnection.Size = new System.Drawing.Size(142, 23);
			this.btncheckconnection.TabIndex = 13;
			this.btncheckconnection.Text = "CHECK CONNECTION";
			this.btncheckconnection.UseVisualStyleBackColor = true;
			this.btncheckconnection.Click += new System.EventHandler(this.btnopencloseconn_Click);
			// 
			// tberrors
			// 
			this.tberrors.Location = new System.Drawing.Point(20, 178);
			this.tberrors.Multiline = true;
			this.tberrors.Name = "tberrors";
			this.tberrors.ReadOnly = true;
			this.tberrors.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.tberrors.Size = new System.Drawing.Size(840, 324);
			this.tberrors.TabIndex = 20;
			// 
			// lbltraceedit
			// 
			this.lbltraceedit.AutoSize = true;
			this.lbltraceedit.Location = new System.Drawing.Point(17, 162);
			this.lbltraceedit.Name = "lbltraceedit";
			this.lbltraceedit.Size = new System.Drawing.Size(141, 13);
			this.lbltraceedit.TabIndex = 21;
			this.lbltraceedit.Text = "COMMUNICATION TRACING";
			// 
			// nudmacid
			// 
			this.nudmacid.Location = new System.Drawing.Point(90, 17);
			this.nudmacid.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
			this.nudmacid.Name = "nudmacid";
			this.nudmacid.Size = new System.Drawing.Size(50, 20);
			this.nudmacid.TabIndex = 23;
			// 
			// lblmacid
			// 
			this.lblmacid.AutoSize = true;
			this.lblmacid.Location = new System.Drawing.Point(12, 19);
			this.lblmacid.Name = "lblmacid";
			this.lblmacid.Size = new System.Drawing.Size(44, 13);
			this.lblmacid.TabIndex = 22;
			this.lblmacid.Text = "MAC ID";
			// 
			// tbcommand
			// 
			this.tbcommand.Location = new System.Drawing.Point(89, 506);
			this.tbcommand.Name = "tbcommand";
			this.tbcommand.Size = new System.Drawing.Size(677, 20);
			this.tbcommand.TabIndex = 24;
			// 
			// tbchecksum
			// 
			this.tbchecksum.Location = new System.Drawing.Point(90, 537);
			this.tbchecksum.Name = "tbchecksum";
			this.tbchecksum.Size = new System.Drawing.Size(66, 20);
			this.tbchecksum.TabIndex = 25;
			// 
			// btngetchecksum
			// 
			this.btngetchecksum.Location = new System.Drawing.Point(163, 534);
			this.btngetchecksum.Name = "btngetchecksum";
			this.btngetchecksum.Size = new System.Drawing.Size(117, 23);
			this.btngetchecksum.TabIndex = 26;
			this.btngetchecksum.Text = "GET CHECKSUM";
			this.btngetchecksum.UseVisualStyleBackColor = true;
			this.btngetchecksum.Click += new System.EventHandler(this.btngetchecksum_Click);
			// 
			// lblcommand
			// 
			this.lblcommand.AutoSize = true;
			this.lblcommand.Location = new System.Drawing.Point(20, 509);
			this.lblcommand.Name = "lblcommand";
			this.lblcommand.Size = new System.Drawing.Size(63, 13);
			this.lblcommand.TabIndex = 27;
			this.lblcommand.Text = "COMMAND";
			// 
			// lblchksum
			// 
			this.lblchksum.AutoSize = true;
			this.lblchksum.Location = new System.Drawing.Point(20, 540);
			this.lblchksum.Name = "lblchksum";
			this.lblchksum.Size = new System.Drawing.Size(67, 13);
			this.lblchksum.TabIndex = 28;
			this.lblchksum.Text = "CHECKSUM";
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(872, 582);
			this.Controls.Add(this.lblchksum);
			this.Controls.Add(this.lblcommand);
			this.Controls.Add(this.btngetchecksum);
			this.Controls.Add(this.tbchecksum);
			this.Controls.Add(this.tbcommand);
			this.Controls.Add(this.nudmacid);
			this.Controls.Add(this.lblmacid);
			this.Controls.Add(this.lbltraceedit);
			this.Controls.Add(this.tberrors);
			this.Controls.Add(this.btncheckconnection);
			this.Controls.Add(this.btnsend);
			this.Controls.Add(this.cbcmdtosend);
			this.Controls.Add(this.lblcmdtosend);
			this.Controls.Add(this.nudport);
			this.Controls.Add(this.lblport);
			this.Controls.Add(this.nudip4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.nudip3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nudip2);
			this.Controls.Add(this.lblpunto1);
			this.Controls.Add(this.nudip1);
			this.Controls.Add(this.lblip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "FrmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MDG TEST";
			this.Load += new System.EventHandler(this.FrmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudip1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudip2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudip3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudip4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudport)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudmacid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblip;
		private System.Windows.Forms.NumericUpDown nudip1;
		private System.Windows.Forms.Label lblpunto1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown nudip2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown nudip3;
		private System.Windows.Forms.NumericUpDown nudip4;
		private System.Windows.Forms.Label lblport;
		private System.Windows.Forms.NumericUpDown nudport;
		private System.Windows.Forms.Label lblcmdtosend;
		private System.Windows.Forms.ComboBox cbcmdtosend;
		private System.Windows.Forms.Button btnsend;
		private System.Windows.Forms.Button btncheckconnection;
		private System.Windows.Forms.TextBox tberrors;
		private System.Windows.Forms.Label lbltraceedit;
		private System.Windows.Forms.NumericUpDown nudmacid;
		private System.Windows.Forms.Label lblmacid;
		private System.Windows.Forms.TextBox tbcommand;
		private System.Windows.Forms.TextBox tbchecksum;
		private System.Windows.Forms.Button btngetchecksum;
		private System.Windows.Forms.Label lblcommand;
		private System.Windows.Forms.Label lblchksum;
	}
}

