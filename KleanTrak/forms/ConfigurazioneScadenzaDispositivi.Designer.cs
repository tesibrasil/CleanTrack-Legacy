namespace KleanTrak
{
	partial class ConfigurazioneScadenzaDispositivi
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
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.toolstripcontainer = new System.Windows.Forms.ToolStripContainer();
			this.main_toolstrip = new System.Windows.Forms.ToolStrip();
			this.tsb_add = new System.Windows.Forms.ToolStripButton();
			this.tsb_delete = new System.Windows.Forms.ToolStripButton();
			this.tsb_close = new System.Windows.Forms.ToolStripButton();
			this.toolstripcontainer.ContentPanel.SuspendLayout();
			this.toolstripcontainer.TopToolStripPanel.SuspendLayout();
			this.toolstripcontainer.SuspendLayout();
			this.main_toolstrip.SuspendLayout();
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
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.HideSelection = false;
			this.listView.Location = new System.Drawing.Point(0, 0);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(790, 345);
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
			this.columnHeader1.Text = "Tipo dispositivo";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Stato inizio";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Scadenza (minuti)";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Stato fine";
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "";
			this.columnHeader5.Width = 0;
			// 
			// toolstripcontainer
			// 
			// 
			// toolstripcontainer.ContentPanel
			// 
			this.toolstripcontainer.ContentPanel.Controls.Add(this.listView);
			this.toolstripcontainer.ContentPanel.Size = new System.Drawing.Size(795, 325);
			this.toolstripcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolstripcontainer.Location = new System.Drawing.Point(0, 0);
			this.toolstripcontainer.Name = "toolstripcontainer";
			this.toolstripcontainer.Size = new System.Drawing.Size(795, 384);
			this.toolstripcontainer.TabIndex = 4;
			this.toolstripcontainer.Text = "toolStripContainer1";
			// 
			// toolstripcontainer.TopToolStripPanel
			// 
			this.toolstripcontainer.TopToolStripPanel.Controls.Add(this.main_toolstrip);
			// 
			// main_toolstrip
			// 
			this.main_toolstrip.Dock = System.Windows.Forms.DockStyle.None;
			this.main_toolstrip.ImageScalingSize = new System.Drawing.Size(52, 52);
			this.main_toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_add,
            this.tsb_delete,
            this.tsb_close});
			this.main_toolstrip.Location = new System.Drawing.Point(3, 0);
			this.main_toolstrip.Name = "main_toolstrip";
			this.main_toolstrip.Size = new System.Drawing.Size(180, 59);
			this.main_toolstrip.TabIndex = 0;
			// 
			// tsb_add
			// 
			this.tsb_add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_add.Image = global::kleanTrak.Properties.Resources.add;
			this.tsb_add.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_add.Name = "tsb_add";
			this.tsb_add.Size = new System.Drawing.Size(56, 56);
			this.tsb_add.Text = "toolStripButton1";
			this.tsb_add.Click += new System.EventHandler(this.tsb_add_Click);
			// 
			// tsb_delete
			// 
			this.tsb_delete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_delete.Image = global::kleanTrak.Properties.Resources.delete;
			this.tsb_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_delete.Name = "tsb_delete";
			this.tsb_delete.Size = new System.Drawing.Size(56, 56);
			this.tsb_delete.Text = "toolStripButton1";
			this.tsb_delete.Click += new System.EventHandler(this.tsb_delete_Click);
			// 
			// tsb_close
			// 
			this.tsb_close.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsb_close.Image = global::kleanTrak.Properties.Resources.close;
			this.tsb_close.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tsb_close.Name = "tsb_close";
			this.tsb_close.Size = new System.Drawing.Size(56, 56);
			this.tsb_close.Text = "toolStripButton1";
			this.tsb_close.Click += new System.EventHandler(this.tsb_close_Click);
			// 
			// ConfigurazioneScadenzaDispositivi
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(795, 384);
			this.ControlBox = false;
			this.Controls.Add(this.toolstripcontainer);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ConfigurazioneScadenzaDispositivi";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Configurazione scadenza dispositivi";
			this.Load += new System.EventHandler(this.ConfigurazioneScadenzaDispositivi_Load);
			this.ClientSizeChanged += new System.EventHandler(this.ConfigurazioneScadenzaDispositivi_ClientSizeChanged);
			this.toolstripcontainer.ContentPanel.ResumeLayout(false);
			this.toolstripcontainer.TopToolStripPanel.ResumeLayout(false);
			this.toolstripcontainer.TopToolStripPanel.PerformLayout();
			this.toolstripcontainer.ResumeLayout(false);
			this.toolstripcontainer.PerformLayout();
			this.main_toolstrip.ResumeLayout(false);
			this.main_toolstrip.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private ListViewEx.ListViewEx listView;
		private System.Windows.Forms.ColumnHeader columnHeader0;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ToolStripContainer toolstripcontainer;
		private System.Windows.Forms.ToolStrip main_toolstrip;
		private System.Windows.Forms.ToolStripButton tsb_add;
		private System.Windows.Forms.ToolStripButton tsb_delete;
		private System.Windows.Forms.ToolStripButton tsb_close;
	}
}