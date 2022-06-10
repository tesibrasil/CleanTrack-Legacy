namespace KleanTrak
{
	partial class ConfigurazioneTransazioni
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
			this.listView = new ListViewEx.ListViewEx();
			this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tscontainer = new System.Windows.Forms.ToolStripContainer();
			this.tsactions = new System.Windows.Forms.ToolStrip();
			this.tsadd = new System.Windows.Forms.ToolStripButton();
			this.tsdelete = new System.Windows.Forms.ToolStripButton();
			this.tsclose = new System.Windows.Forms.ToolStripButton();
			this.tscontainer.ContentPanel.SuspendLayout();
			this.tscontainer.TopToolStripPanel.SuspendLayout();
			this.tscontainer.SuspendLayout();
			this.tsactions.SuspendLayout();
			this.SuspendLayout();
			// 
			// listView
			// 
			this.listView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(736, 347);
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			this.listView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView_ColumnClick);
			this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
			// 
			// columnHeader0
			// 
			this.columnHeader0.Text = "";
			this.columnHeader0.Width = 0;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Stato iniziale";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Stato finale";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "";
			this.columnHeader3.Width = 0;
			// 
			// tscontainer
			// 
			// 
			// tscontainer.ContentPanel
			// 
			this.tscontainer.ContentPanel.Controls.Add(this.listView);
			this.tscontainer.ContentPanel.Size = new System.Drawing.Size(718, 389);
			this.tscontainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tscontainer.Location = new System.Drawing.Point(0, 0);
			this.tscontainer.Name = "tscontainer";
			this.tscontainer.Size = new System.Drawing.Size(718, 448);
			this.tscontainer.TabIndex = 4;
			this.tscontainer.Text = "toolStripContainer1";
			// 
			// tscontainer.TopToolStripPanel
			// 
			this.tscontainer.TopToolStripPanel.Controls.Add(this.tsactions);
			// 
			// tsactions
			// 
			this.tsactions.Dock = System.Windows.Forms.DockStyle.None;
			this.tsactions.ImageScalingSize = new System.Drawing.Size(52, 52);
			this.tsactions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsadd,
            this.tsdelete,
            this.tsclose});
			this.tsactions.Location = new System.Drawing.Point(3, 0);
			this.tsactions.Name = "tsactions";
			this.tsactions.Size = new System.Drawing.Size(180, 59);
			this.tsactions.TabIndex = 0;
			// 
			// tsadd
			// 
			this.tsadd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsadd.Image = global::kleanTrak.Properties.Resources.add;
			this.tsadd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsadd.Name = "tsadd";
			this.tsadd.Size = new System.Drawing.Size(56, 56);
			this.tsadd.Text = "toolStripButton1";
			this.tsadd.Click += new System.EventHandler(this.tsadd_Click);
			// 
			// tsdelete
			// 
			this.tsdelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsdelete.Image = global::kleanTrak.Properties.Resources.delete;
			this.tsdelete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsdelete.Name = "tsdelete";
			this.tsdelete.Size = new System.Drawing.Size(56, 56);
			this.tsdelete.Text = "toolStripButton2";
			this.tsdelete.Click += new System.EventHandler(this.tsdelete_Click);
			// 
			// tsclose
			// 
			this.tsclose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsclose.Image = global::kleanTrak.Properties.Resources.close;
			this.tsclose.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsclose.Name = "tsclose";
			this.tsclose.Size = new System.Drawing.Size(56, 56);
			this.tsclose.Text = "toolStripButton1";
			this.tsclose.Click += new System.EventHandler(this.tsclose_Click);
			// 
			// ConfigurazioneTransazioni
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(718, 448);
			this.ControlBox = false;
			this.Controls.Add(this.tscontainer);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConfigurazioneTransazioni";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Lista dei possibili passaggi di stato";
			this.Load += new System.EventHandler(this.ConfigurazioneTransazioni_Load);
			this.ClientSizeChanged += new System.EventHandler(this.ConfigurazioneTransazioni_ClientSizeChanged);
			this.tscontainer.ContentPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.ResumeLayout(false);
			this.tscontainer.TopToolStripPanel.PerformLayout();
			this.tscontainer.ResumeLayout(false);
			this.tscontainer.PerformLayout();
			this.tsactions.ResumeLayout(false);
			this.tsactions.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private ListViewEx.ListViewEx listView;
		private System.Windows.Forms.ColumnHeader columnHeader0;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ToolStripContainer tscontainer;
		private System.Windows.Forms.ToolStrip tsactions;
		private System.Windows.Forms.ToolStripButton tsadd;
		private System.Windows.Forms.ToolStripButton tsdelete;
		private System.Windows.Forms.ToolStripButton tsclose;
	}
}