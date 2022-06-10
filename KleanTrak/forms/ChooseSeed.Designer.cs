namespace KleanTrak
{
	partial class ChooseSeed
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseSeed));
			this.dgv_seeds = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgv_seeds)).BeginInit();
			this.SuspendLayout();
			// 
			// dgv_seeds
			// 
			this.dgv_seeds.ReadOnly = true;
			this.dgv_seeds.AllowUserToAddRows = false;
			this.dgv_seeds.AllowUserToDeleteRows = false;
			this.dgv_seeds.AllowUserToOrderColumns = true;
			this.dgv_seeds.AllowUserToResizeRows = false;
			this.dgv_seeds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgv_seeds.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgv_seeds.Location = new System.Drawing.Point(0, 0);
			this.dgv_seeds.MultiSelect = false;
			this.dgv_seeds.Name = "dgv_seeds";
			this.dgv_seeds.RowHeadersVisible = false;
			this.dgv_seeds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgv_seeds.Size = new System.Drawing.Size(575, 402);
			this.dgv_seeds.TabIndex = 0;
			this.dgv_seeds.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_seeds_CellMouseClick);
			// 
			// ChooseSeed
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(575, 402);
			this.Controls.Add(this.dgv_seeds);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChooseSeed";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ChooseSeed";
			this.Load += new System.EventHandler(this.ChooseSeed_Load);
			this.ClientSizeChanged += new System.EventHandler(this.ChooseSeed_ClientSizeChanged);
			((System.ComponentModel.ISupportInitialize)(this.dgv_seeds)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgv_seeds;
	}
}