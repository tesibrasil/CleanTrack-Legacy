namespace KleanTrak
{
    partial class FrmReceiptsView
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReceiptsView));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.dgv_receipts = new System.Windows.Forms.DataGridView();
            this.maintoolstrip = new System.Windows.Forms.ToolStrip();
            this.tslblwasher = new System.Windows.Forms.ToolStripLabel();
            this.tscombowasher = new System.Windows.Forms.ToolStripComboBox();
            this.tsl_selected_dates = new System.Windows.Forms.ToolStripLabel();
            this.tsb_calendar = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_receipts)).BeginInit();
            this.maintoolstrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.dgv_receipts);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(800, 411);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(800, 450);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.maintoolstrip);
            // 
            // dgv_receipts
            // 
            this.dgv_receipts.AllowUserToAddRows = false;
            this.dgv_receipts.AllowUserToDeleteRows = false;
            this.dgv_receipts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.dgv_receipts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_receipts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_receipts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_receipts.Location = new System.Drawing.Point(0, 0);
            this.dgv_receipts.MultiSelect = false;
            this.dgv_receipts.Name = "dgv_receipts";
            this.dgv_receipts.ReadOnly = true;
            this.dgv_receipts.RowHeadersVisible = false;
            this.dgv_receipts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_receipts.Size = new System.Drawing.Size(800, 411);
            this.dgv_receipts.TabIndex = 0;
            this.dgv_receipts.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_receipts_CellDoubleClick);
            // 
            // maintoolstrip
            // 
            this.maintoolstrip.Dock = System.Windows.Forms.DockStyle.None;
            this.maintoolstrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.maintoolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslblwasher,
            this.tscombowasher,
            this.tsl_selected_dates,
            this.tsb_calendar});
            this.maintoolstrip.Location = new System.Drawing.Point(3, 0);
            this.maintoolstrip.Name = "maintoolstrip";
            this.maintoolstrip.Size = new System.Drawing.Size(309, 39);
            this.maintoolstrip.TabIndex = 0;
            // 
            // tslblwasher
            // 
            this.tslblwasher.Name = "tslblwasher";
            this.tslblwasher.Size = new System.Drawing.Size(86, 36);
            this.tslblwasher.Text = "toolStripLabel1";
            // 
            // tscombowasher
            // 
            this.tscombowasher.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscombowasher.Name = "tscombowasher";
            this.tscombowasher.Size = new System.Drawing.Size(121, 39);
            this.tscombowasher.SelectedIndexChanged += new System.EventHandler(this.tscombowasher_SelectedIndexChanged);
            // 
            // tsl_selected_dates
            // 
            this.tsl_selected_dates.Name = "tsl_selected_dates";
            this.tsl_selected_dates.Size = new System.Drawing.Size(52, 36);
            this.tsl_selected_dates.Text = "sel dates";
            // 
            // tsb_calendar
            // 
            this.tsb_calendar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_calendar.Image = ((System.Drawing.Image)(resources.GetObject("tsb_calendar.Image")));
            this.tsb_calendar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_calendar.Name = "tsb_calendar";
            this.tsb_calendar.Size = new System.Drawing.Size(36, 36);
            this.tsb_calendar.Text = "toolStripButton1";
            this.tsb_calendar.Click += new System.EventHandler(this.tsb_calendar_Click);
            // 
            // FrmReceiptsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmReceiptsView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CleanTrack";
            this.Load += new System.EventHandler(this.FrmReceiptsView_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_receipts)).EndInit();
            this.maintoolstrip.ResumeLayout(false);
            this.maintoolstrip.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip maintoolstrip;
        private System.Windows.Forms.ToolStripLabel tslblwasher;
        private System.Windows.Forms.ToolStripComboBox tscombowasher;
        private System.Windows.Forms.DataGridView dgv_receipts;
        private System.Windows.Forms.ToolStripLabel tsl_selected_dates;
        private System.Windows.Forms.ToolStripButton tsb_calendar;
    }
}