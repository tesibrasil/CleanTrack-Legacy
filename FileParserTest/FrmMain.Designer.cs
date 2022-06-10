
namespace FileParserTest
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
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.receiptsDir = new System.Windows.Forms.Button();
			this.tbmatricola = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.tbseriale = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cbmodello = new System.Windows.Forms.ComboBox();
			this.lblmodel = new System.Windows.Forms.Label();
			this.tbmessages = new System.Windows.Forms.TextBox();
			this.logfilewatcher = new System.IO.FileSystemWatcher();
			this.btnmovetoold = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.logfilewatcher)).BeginInit();
			this.SuspendLayout();
			// 
			// folderBrowserDialog1
			// 
			this.folderBrowserDialog1.ShowNewFolderButton = false;
			// 
			// receiptsDir
			// 
			this.receiptsDir.Location = new System.Drawing.Point(12, 12);
			this.receiptsDir.Name = "receiptsDir";
			this.receiptsDir.Size = new System.Drawing.Size(115, 23);
			this.receiptsDir.TabIndex = 0;
			this.receiptsDir.Text = "Dir Scontrini";
			this.receiptsDir.UseVisualStyleBackColor = true;
			this.receiptsDir.Click += new System.EventHandler(this.loadTestReceipt_Click);
			// 
			// tbmatricola
			// 
			this.tbmatricola.AutoSize = true;
			this.tbmatricola.Location = new System.Drawing.Point(38, 60);
			this.tbmatricola.Name = "tbmatricola";
			this.tbmatricola.Size = new System.Drawing.Size(50, 13);
			this.tbmatricola.TabIndex = 1;
			this.tbmatricola.Text = "Matricola";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(107, 57);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 20);
			this.textBox1.TabIndex = 2;
			// 
			// tbseriale
			// 
			this.tbseriale.Location = new System.Drawing.Point(107, 83);
			this.tbseriale.Name = "tbseriale";
			this.tbseriale.Size = new System.Drawing.Size(100, 20);
			this.tbseriale.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(38, 86);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Seriale";
			// 
			// cbmodello
			// 
			this.cbmodello.FormattingEnabled = true;
			this.cbmodello.Location = new System.Drawing.Point(238, 12);
			this.cbmodello.Name = "cbmodello";
			this.cbmodello.Size = new System.Drawing.Size(186, 21);
			this.cbmodello.TabIndex = 5;
			// 
			// lblmodel
			// 
			this.lblmodel.AutoSize = true;
			this.lblmodel.Location = new System.Drawing.Point(176, 19);
			this.lblmodel.Name = "lblmodel";
			this.lblmodel.Size = new System.Drawing.Size(44, 13);
			this.lblmodel.TabIndex = 6;
			this.lblmodel.Text = "Modello";
			// 
			// tbmessages
			// 
			this.tbmessages.BackColor = System.Drawing.Color.Black;
			this.tbmessages.ForeColor = System.Drawing.Color.Lime;
			this.tbmessages.Location = new System.Drawing.Point(15, 130);
			this.tbmessages.Multiline = true;
			this.tbmessages.Name = "tbmessages";
			this.tbmessages.Size = new System.Drawing.Size(408, 207);
			this.tbmessages.TabIndex = 7;
			// 
			// logfilewatcher
			// 
			this.logfilewatcher.EnableRaisingEvents = true;
			this.logfilewatcher.Filter = "*.log";
			this.logfilewatcher.SynchronizingObject = this;
			this.logfilewatcher.Changed += new System.IO.FileSystemEventHandler(this.logfilewatcher_Changed);
			// 
			// btnmovetoold
			// 
			this.btnmovetoold.Location = new System.Drawing.Point(238, 80);
			this.btnmovetoold.Name = "btnmovetoold";
			this.btnmovetoold.Size = new System.Drawing.Size(115, 23);
			this.btnmovetoold.TabIndex = 8;
			this.btnmovetoold.Text = "Move to Old";
			this.btnmovetoold.UseVisualStyleBackColor = true;
			this.btnmovetoold.Click += new System.EventHandler(this.btnmovetoold_Click);
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(436, 347);
			this.Controls.Add(this.btnmovetoold);
			this.Controls.Add(this.tbmessages);
			this.Controls.Add(this.lblmodel);
			this.Controls.Add(this.cbmodello);
			this.Controls.Add(this.tbseriale);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.tbmatricola);
			this.Controls.Add(this.receiptsDir);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FrmMain";
			this.Text = "Test Advantage Pass Trough";
			this.Load += new System.EventHandler(this.FrmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.logfilewatcher)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Button receiptsDir;
		private System.Windows.Forms.Label tbmatricola;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox tbseriale;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbmodello;
		private System.Windows.Forms.Label lblmodel;
		private System.Windows.Forms.TextBox tbmessages;
		private System.IO.FileSystemWatcher logfilewatcher;
		private System.Windows.Forms.Button btnmovetoold;
	}
}