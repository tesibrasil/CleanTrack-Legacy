namespace KleanTrak
{
    partial class FrmRecepitDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRecepitDetails));
            this.tb_receipt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tb_receipt
            // 
            this.tb_receipt.BackColor = System.Drawing.Color.White;
            this.tb_receipt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_receipt.Location = new System.Drawing.Point(0, 0);
            this.tb_receipt.Multiline = true;
            this.tb_receipt.Name = "tb_receipt";
            this.tb_receipt.ReadOnly = true;
            this.tb_receipt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_receipt.Size = new System.Drawing.Size(385, 507);
            this.tb_receipt.TabIndex = 0;
            // 
            // FrmRecepitDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 507);
            this.Controls.Add(this.tb_receipt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmRecepitDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CleanTrack";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_receipt;
    }
}