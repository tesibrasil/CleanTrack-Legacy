namespace KleanTrak
{
	partial class CicliDispositivoDettaglio
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
			this.listView = new System.Windows.Forms.ListView();
			this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnStampa = new System.Windows.Forms.Button();
			this.btnChiudi = new System.Windows.Forms.Button();
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
            this.columnHeader4});
			this.listView.FullRowSelect = true;
			this.listView.GridLines = true;
			this.listView.Location = new System.Drawing.Point(5, 5);
			this.listView.Name = "listView";
			this.listView.Size = new System.Drawing.Size(500, 250);
			this.listView.TabIndex = 2;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader0
			// 
			this.columnHeader0.Text = "";
			this.columnHeader0.Width = 0;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Data";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Descrizione";
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Valore";
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "";
			this.columnHeader4.Width = 0;
			// 
			// btnStampa
			// 
			this.btnStampa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnStampa.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnStampa.Location = new System.Drawing.Point(5, 260);
			this.btnStampa.Name = "btnStampa";
			this.btnStampa.Size = new System.Drawing.Size(100, 30);
			this.btnStampa.TabIndex = 1;
			this.btnStampa.Text = "Stampa";
			this.btnStampa.UseVisualStyleBackColor = true;
			this.btnStampa.Click += new System.EventHandler(this.btnStampa_Click);
			// 
			// btnChiudi
			// 
			this.btnChiudi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnChiudi.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnChiudi.Location = new System.Drawing.Point(405, 260);
			this.btnChiudi.Name = "btnChiudi";
			this.btnChiudi.Size = new System.Drawing.Size(100, 30);
			this.btnChiudi.TabIndex = 0;
			this.btnChiudi.Text = "Chiudi";
			this.btnChiudi.UseVisualStyleBackColor = true;
			this.btnChiudi.Click += new System.EventHandler(this.btnChiudi_Click);
			// 
			// CicliDispositivoDettaglio
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(510, 295);
			this.ControlBox = false;
			this.Controls.Add(this.btnChiudi);
			this.Controls.Add(this.btnStampa);
			this.Controls.Add(this.listView);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CicliDispositivoDettaglio";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Dettaglio ciclo dispositivo";
			this.Load += new System.EventHandler(this.CicliDispositivoDettaglio_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView listView;
		private System.Windows.Forms.Button btnStampa;
		private System.Windows.Forms.Button btnChiudi;
		private System.Windows.Forms.ColumnHeader columnHeader0;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
	}
}