namespace KleanTrak
{
	partial class ChooseItem
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
			this.lbl_combo = new System.Windows.Forms.Label();
			this.cb_items = new System.Windows.Forms.ComboBox();
			this.btn_ok = new System.Windows.Forms.Button();
			this.btn_cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbl_combo
			// 
			this.lbl_combo.AutoSize = true;
			this.lbl_combo.Font = new System.Drawing.Font("Tahoma", 10F);
			this.lbl_combo.Location = new System.Drawing.Point(12, 21);
			this.lbl_combo.Name = "lbl_combo";
			this.lbl_combo.Size = new System.Drawing.Size(42, 17);
			this.lbl_combo.TabIndex = 0;
			this.lbl_combo.Text = "label1";
			// 
			// cb_items
			// 
			this.cb_items.FormattingEnabled = true;
			this.cb_items.Location = new System.Drawing.Point(15, 41);
			this.cb_items.Name = "cb_items";
			this.cb_items.Size = new System.Drawing.Size(308, 21);
			this.cb_items.TabIndex = 1;
			// 
			// btn_ok
			// 
			this.btn_ok.Image = global::kleanTrak.Properties.Resources.check;
			this.btn_ok.Location = new System.Drawing.Point(267, 88);
			this.btn_ok.Name = "btn_ok";
			this.btn_ok.Size = new System.Drawing.Size(56, 56);
			this.btn_ok.TabIndex = 2;
			this.btn_ok.UseVisualStyleBackColor = true;
			this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
			// 
			// btn_cancel
			// 
			this.btn_cancel.Image = global::kleanTrak.Properties.Resources.close;
			this.btn_cancel.Location = new System.Drawing.Point(15, 88);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new System.Drawing.Size(56, 56);
			this.btn_cancel.TabIndex = 3;
			this.btn_cancel.UseVisualStyleBackColor = true;
			this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
			// 
			// ChooseItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(345, 162);
			this.Controls.Add(this.btn_cancel);
			this.Controls.Add(this.btn_ok);
			this.Controls.Add(this.cb_items);
			this.Controls.Add(this.lbl_combo);
			this.Name = "ChooseItem";
			this.Text = "ChooseItem";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lbl_combo;
		private System.Windows.Forms.ComboBox cb_items;
		private System.Windows.Forms.Button btn_ok;
		private System.Windows.Forms.Button btn_cancel;
	}
}