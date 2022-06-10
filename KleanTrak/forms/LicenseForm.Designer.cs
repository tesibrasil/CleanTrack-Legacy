
namespace KleanTrak
{
    partial class LicenseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));
            this.dgvuo = new System.Windows.Forms.DataGridView();
            this.IDUO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LAVATRICI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.POMPE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ARMADI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IDSEDI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DESCRIZIONESEDI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btngenerate = new System.Windows.Forms.Button();
            this.btnloadfile = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvuo)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvuo
            // 
            this.dgvuo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvuo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvuo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IDUO,
            this.LAVATRICI,
            this.POMPE,
            this.ARMADI,
            this.IDSEDI,
            this.DESCRIZIONESEDI});
            this.dgvuo.Location = new System.Drawing.Point(5, 32);
            this.dgvuo.Name = "dgvuo";
            this.dgvuo.RowHeadersWidth = 51;
            this.dgvuo.Size = new System.Drawing.Size(803, 179);
            this.dgvuo.TabIndex = 0;
            this.dgvuo.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvuo_CellValidating);
            this.dgvuo.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgvuo_RowValidating);
            // 
            // IDUO
            // 
            this.IDUO.HeaderText = "IDUO";
            this.IDUO.MinimumWidth = 6;
            this.IDUO.Name = "IDUO";
            this.IDUO.Width = 125;
            // 
            // LAVATRICI
            // 
            this.LAVATRICI.HeaderText = "LAVATRICI";
            this.LAVATRICI.MinimumWidth = 6;
            this.LAVATRICI.Name = "LAVATRICI";
            this.LAVATRICI.Width = 125;
            // 
            // POMPE
            // 
            this.POMPE.HeaderText = "POMPE";
            this.POMPE.MinimumWidth = 6;
            this.POMPE.Name = "POMPE";
            this.POMPE.Width = 125;
            // 
            // ARMADI
            // 
            this.ARMADI.HeaderText = "ARMADI";
            this.ARMADI.MinimumWidth = 6;
            this.ARMADI.Name = "ARMADI";
            this.ARMADI.Width = 125;
            // 
            // IDSEDI
            // 
            this.IDSEDI.HeaderText = "IDSEDI";
            this.IDSEDI.MinimumWidth = 6;
            this.IDSEDI.Name = "IDSEDI";
            this.IDSEDI.Width = 125;
            // 
            // DESCRIZIONESEDI
            // 
            this.DESCRIZIONESEDI.HeaderText = "DESCRIZIONESEDI";
            this.DESCRIZIONESEDI.MinimumWidth = 6;
            this.DESCRIZIONESEDI.Name = "DESCRIZIONESEDI";
            this.DESCRIZIONESEDI.Width = 125;
            // 
            // btngenerate
            // 
            this.btngenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btngenerate.Location = new System.Drawing.Point(667, 218);
            this.btngenerate.Name = "btngenerate";
            this.btngenerate.Size = new System.Drawing.Size(142, 27);
            this.btngenerate.TabIndex = 1;
            this.btngenerate.Text = "Genera File Per Licenza";
            this.btngenerate.UseVisualStyleBackColor = true;
            this.btngenerate.Click += new System.EventHandler(this.btngenerate_Click);
            // 
            // btnloadfile
            // 
            this.btnloadfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnloadfile.Location = new System.Drawing.Point(543, 218);
            this.btnloadfile.Name = "btnloadfile";
            this.btnloadfile.Size = new System.Drawing.Size(117, 27);
            this.btnloadfile.TabIndex = 2;
            this.btnloadfile.Text = "Carica File";
            this.btnloadfile.UseVisualStyleBackColor = true;
            this.btnloadfile.Click += new System.EventHandler(this.btnloadfile_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "license.lic";
            this.openFileDialog.Filter = "File Json|*.json";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "File Json|*.json";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(545, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "I valori in IDSEDI e DESCRIZIONESEDI devono essere rispettivamente numeri interi " +
    "e stringhe, separati da virgola";
            // 
            // LicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 249);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnloadfile);
            this.Controls.Add(this.btngenerate);
            this.Controls.Add(this.dgvuo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestione Licenze";
            ((System.ComponentModel.ISupportInitialize)(this.dgvuo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvuo;
        private System.Windows.Forms.Button btngenerate;
        private System.Windows.Forms.Button btnloadfile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDUO;
        private System.Windows.Forms.DataGridViewTextBoxColumn IDSEDI;
        private System.Windows.Forms.DataGridViewTextBoxColumn DESCRIZIONESEDI;
        private System.Windows.Forms.DataGridViewTextBoxColumn LAVATRICI;
        private System.Windows.Forms.DataGridViewTextBoxColumn POMPE;
        private System.Windows.Forms.DataGridViewTextBoxColumn ARMADI;
        private System.Windows.Forms.Label label1;
    }
}