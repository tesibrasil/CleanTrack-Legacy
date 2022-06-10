
namespace KleanTrak
{
	partial class SediUO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SediUO));
            this.tvsediuo = new System.Windows.Forms.TreeView();
            this.context_menu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_add_uo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_add_site = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_rename_uo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_rename_site = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_delete_uo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_delete_site = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonRenameUO = new System.Windows.Forms.Button();
            this.buttonAddUO = new System.Windows.Forms.Button();
            this.groupBoxUO = new System.Windows.Forms.GroupBox();
            this.buttonDeleteUO = new System.Windows.Forms.Button();
            this.groupBoxSedi = new System.Windows.Forms.GroupBox();
            this.buttonDeleteSite = new System.Windows.Forms.Button();
            this.buttonRenameSite = new System.Windows.Forms.Button();
            this.buttonAddSite = new System.Windows.Forms.Button();
            this.context_menu.SuspendLayout();
            this.groupBoxUO.SuspendLayout();
            this.groupBoxSedi.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvsediuo
            // 
            this.tvsediuo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvsediuo.Location = new System.Drawing.Point(0, 0);
            this.tvsediuo.Name = "tvsediuo";
            this.tvsediuo.Size = new System.Drawing.Size(412, 379);
            this.tvsediuo.TabIndex = 0;
            this.tvsediuo.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvsediuo_AfterSelect);
            this.tvsediuo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tvsediuo_MouseClick);
            // 
            // context_menu
            // 
            this.context_menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_add_uo,
            this.tsmi_add_site,
            this.tsmi_rename_uo,
            this.tsmi_rename_site,
            this.tsmi_delete_uo,
            this.tsmi_delete_site});
            this.context_menu.Name = "context_menu";
            this.context_menu.Size = new System.Drawing.Size(154, 136);
            // 
            // tsmi_add_uo
            // 
            this.tsmi_add_uo.Name = "tsmi_add_uo";
            this.tsmi_add_uo.Size = new System.Drawing.Size(153, 22);
            this.tsmi_add_uo.Text = "Aggiungi UO";
            this.tsmi_add_uo.Click += new System.EventHandler(this.tsmi_add_uo_Click);
            // 
            // tsmi_add_site
            // 
            this.tsmi_add_site.Name = "tsmi_add_site";
            this.tsmi_add_site.Size = new System.Drawing.Size(153, 22);
            this.tsmi_add_site.Text = "Aggiungi Sede";
            this.tsmi_add_site.Click += new System.EventHandler(this.tsmi_add_site_Click);
            // 
            // tsmi_rename_uo
            // 
            this.tsmi_rename_uo.Name = "tsmi_rename_uo";
            this.tsmi_rename_uo.Size = new System.Drawing.Size(153, 22);
            this.tsmi_rename_uo.Text = "Rinomina UO";
            this.tsmi_rename_uo.Click += new System.EventHandler(this.tsmi_rename_uo_Click);
            // 
            // tsmi_rename_site
            // 
            this.tsmi_rename_site.Name = "tsmi_rename_site";
            this.tsmi_rename_site.Size = new System.Drawing.Size(153, 22);
            this.tsmi_rename_site.Text = "Rinomina Sede";
            this.tsmi_rename_site.Click += new System.EventHandler(this.tsmi_rename_site_Click);
            // 
            // tsmi_delete_uo
            // 
            this.tsmi_delete_uo.Name = "tsmi_delete_uo";
            this.tsmi_delete_uo.Size = new System.Drawing.Size(153, 22);
            this.tsmi_delete_uo.Text = "Elimina UO";
            this.tsmi_delete_uo.Click += new System.EventHandler(this.tsmi_delete_uo_Click);
            // 
            // tsmi_delete_site
            // 
            this.tsmi_delete_site.Name = "tsmi_delete_site";
            this.tsmi_delete_site.Size = new System.Drawing.Size(153, 22);
            this.tsmi_delete_site.Text = "Elimina Sede";
            this.tsmi_delete_site.Click += new System.EventHandler(this.tsmi_delete_site_Click);
            // 
            // buttonRenameUO
            // 
            this.buttonRenameUO.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRenameUO.Location = new System.Drawing.Point(65, 16);
            this.buttonRenameUO.Name = "buttonRenameUO";
            this.buttonRenameUO.Size = new System.Drawing.Size(62, 25);
            this.buttonRenameUO.TabIndex = 1;
            this.buttonRenameUO.Text = "Rinomina";
            this.buttonRenameUO.UseVisualStyleBackColor = true;
            this.buttonRenameUO.Click += new System.EventHandler(this.tsmi_rename_uo_Click);
            // 
            // buttonAddUO
            // 
            this.buttonAddUO.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonAddUO.Location = new System.Drawing.Point(3, 16);
            this.buttonAddUO.Name = "buttonAddUO";
            this.buttonAddUO.Size = new System.Drawing.Size(62, 25);
            this.buttonAddUO.TabIndex = 4;
            this.buttonAddUO.Text = "Aggiungi";
            this.buttonAddUO.UseVisualStyleBackColor = true;
            this.buttonAddUO.Click += new System.EventHandler(this.tsmi_add_uo_Click);
            // 
            // groupBoxUO
            // 
            this.groupBoxUO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxUO.Controls.Add(this.buttonDeleteUO);
            this.groupBoxUO.Controls.Add(this.buttonRenameUO);
            this.groupBoxUO.Controls.Add(this.buttonAddUO);
            this.groupBoxUO.Location = new System.Drawing.Point(4, 385);
            this.groupBoxUO.Name = "groupBoxUO";
            this.groupBoxUO.Size = new System.Drawing.Size(194, 44);
            this.groupBoxUO.TabIndex = 5;
            this.groupBoxUO.TabStop = false;
            this.groupBoxUO.Text = "UO";
            // 
            // buttonDeleteUO
            // 
            this.buttonDeleteUO.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonDeleteUO.Location = new System.Drawing.Point(127, 16);
            this.buttonDeleteUO.Name = "buttonDeleteUO";
            this.buttonDeleteUO.Size = new System.Drawing.Size(62, 25);
            this.buttonDeleteUO.TabIndex = 5;
            this.buttonDeleteUO.Text = "Elimina";
            this.buttonDeleteUO.UseVisualStyleBackColor = true;
            this.buttonDeleteUO.Click += new System.EventHandler(this.tsmi_delete_uo_Click);
            // 
            // groupBoxSedi
            // 
            this.groupBoxSedi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSedi.Controls.Add(this.buttonDeleteSite);
            this.groupBoxSedi.Controls.Add(this.buttonRenameSite);
            this.groupBoxSedi.Controls.Add(this.buttonAddSite);
            this.groupBoxSedi.Location = new System.Drawing.Point(211, 385);
            this.groupBoxSedi.Name = "groupBoxSedi";
            this.groupBoxSedi.Size = new System.Drawing.Size(194, 44);
            this.groupBoxSedi.TabIndex = 6;
            this.groupBoxSedi.TabStop = false;
            this.groupBoxSedi.Text = "Sedi";
            // 
            // buttonDeleteSite
            // 
            this.buttonDeleteSite.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonDeleteSite.Location = new System.Drawing.Point(127, 16);
            this.buttonDeleteSite.Name = "buttonDeleteSite";
            this.buttonDeleteSite.Size = new System.Drawing.Size(62, 25);
            this.buttonDeleteSite.TabIndex = 5;
            this.buttonDeleteSite.Text = "Elimina";
            this.buttonDeleteSite.UseVisualStyleBackColor = true;
            this.buttonDeleteSite.Click += new System.EventHandler(this.tsmi_delete_site_Click);
            // 
            // buttonRenameSite
            // 
            this.buttonRenameSite.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRenameSite.Location = new System.Drawing.Point(65, 16);
            this.buttonRenameSite.Name = "buttonRenameSite";
            this.buttonRenameSite.Size = new System.Drawing.Size(62, 25);
            this.buttonRenameSite.TabIndex = 1;
            this.buttonRenameSite.Text = "Rinomina";
            this.buttonRenameSite.UseVisualStyleBackColor = true;
            this.buttonRenameSite.Click += new System.EventHandler(this.tsmi_rename_site_Click);
            // 
            // buttonAddSite
            // 
            this.buttonAddSite.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonAddSite.Location = new System.Drawing.Point(3, 16);
            this.buttonAddSite.Name = "buttonAddSite";
            this.buttonAddSite.Size = new System.Drawing.Size(62, 25);
            this.buttonAddSite.TabIndex = 4;
            this.buttonAddSite.Text = "Aggiungi";
            this.buttonAddSite.UseVisualStyleBackColor = true;
            this.buttonAddSite.Click += new System.EventHandler(this.tsmi_add_site_Click);
            // 
            // SediUO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 431);
            this.Controls.Add(this.groupBoxSedi);
            this.Controls.Add(this.groupBoxUO);
            this.Controls.Add(this.tvsediuo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SediUO";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UO Sedi";
            this.Load += new System.EventHandler(this.SediUO_Load);
            this.context_menu.ResumeLayout(false);
            this.groupBoxUO.ResumeLayout(false);
            this.groupBoxSedi.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView tvsediuo;
		private System.Windows.Forms.ContextMenuStrip context_menu;
		private System.Windows.Forms.ToolStripMenuItem tsmi_add_uo;
		private System.Windows.Forms.ToolStripMenuItem tsmi_add_site;
		private System.Windows.Forms.ToolStripMenuItem tsmi_rename_uo;
		private System.Windows.Forms.ToolStripMenuItem tsmi_rename_site;
		private System.Windows.Forms.ToolStripMenuItem tsmi_delete_uo;
		private System.Windows.Forms.ToolStripMenuItem tsmi_delete_site;
        private System.Windows.Forms.Button buttonRenameUO;
        private System.Windows.Forms.Button buttonAddUO;
        private System.Windows.Forms.GroupBox groupBoxUO;
        private System.Windows.Forms.Button buttonDeleteUO;
        private System.Windows.Forms.GroupBox groupBoxSedi;
        private System.Windows.Forms.Button buttonDeleteSite;
        private System.Windows.Forms.Button buttonRenameSite;
        private System.Windows.Forms.Button buttonAddSite;
    }
}