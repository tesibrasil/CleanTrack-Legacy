
namespace KleanTrak
{
	partial class InsertText
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsertText));
			this.btn_cancel = new System.Windows.Forms.Button();
			this.btn_ok = new System.Windows.Forms.Button();
			this.tbtext = new System.Windows.Forms.TextBox();
			this.lbltext = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btn_cancel
			// 
			this.btn_cancel.Image = global::kleanTrak.Properties.Resources.close;
			this.btn_cancel.Location = new System.Drawing.Point(12, 87);
			this.btn_cancel.Name = "btn_cancel";
			this.btn_cancel.Size = new System.Drawing.Size(56, 56);
			this.btn_cancel.TabIndex = 5;
			this.btn_cancel.UseVisualStyleBackColor = true;
			this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
			// 
			// btn_ok
			// 
			this.btn_ok.Image = global::kleanTrak.Properties.Resources.check;
			this.btn_ok.Location = new System.Drawing.Point(264, 87);
			this.btn_ok.Name = "btn_ok";
			this.btn_ok.Size = new System.Drawing.Size(56, 56);
			this.btn_ok.TabIndex = 4;
			this.btn_ok.UseVisualStyleBackColor = true;
			this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
			// 
			// tbtext
			// 
			this.tbtext.Location = new System.Drawing.Point(12, 45);
			this.tbtext.Name = "tbtext";
			this.tbtext.Size = new System.Drawing.Size(307, 20);
			this.tbtext.TabIndex = 7;
			// 
			// lbltext
			// 
			this.lbltext.AutoSize = true;
			this.lbltext.Location = new System.Drawing.Point(13, 26);
			this.lbltext.Name = "lbltext";
			this.lbltext.Size = new System.Drawing.Size(35, 13);
			this.lbltext.TabIndex = 8;
			this.lbltext.Text = "label1";
			// 
			// InsertText
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(331, 155);
			this.ControlBox = false;
			this.Controls.Add(this.lbltext);
			this.Controls.Add(this.tbtext);
			this.Controls.Add(this.btn_cancel);
			this.Controls.Add(this.btn_ok);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimizeBox = false;
			this.Name = "InsertText";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Inserire Valore";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btn_cancel;
		private System.Windows.Forms.Button btn_ok;
		private System.Windows.Forms.TextBox tbtext;
		private System.Windows.Forms.Label lbltext;
	}
}