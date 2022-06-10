using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace KleanTrak
{
	/// <summary>
	/// Descrizione di riepilogo per FormInfo.
	/// </summary>
	public class FormInfo : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button1;
		private Label label7;

		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormInfo()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Pulire le risorse in uso.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					pictureBox1.Image.Dispose();
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Codice generato da Progettazione Windows Form
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInfo));
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(5, 200);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(350, 25);
			this.label5.TabIndex = 5;
			this.label5.Text = "Tel. (+39) 041 46.41.77";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(5, 225);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(350, 25);
			this.label4.TabIndex = 6;
			this.label4.Text = "Fax. (+39) 041 46.44.15";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(5, 250);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(350, 25);
			this.label6.TabIndex = 7;
			this.label6.Text = "imaging@tesi.mi.it";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(5, 125);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(350, 25);
			this.label1.TabIndex = 2;
			this.label1.Text = "Tesi Elettronica e Sistemi Informativi S.p.A.";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(5, 150);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(350, 25);
			this.label2.TabIndex = 3;
			this.label2.Text = "Via Friuli Venezia Giulia, 77";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(5, 175);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(350, 25);
			this.label3.TabIndex = 4;
			this.label3.Text = "30030 Pianiga (VE)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(5, 5);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(351, 114);
			this.pictureBox1.TabIndex = 10;
			this.pictureBox1.TabStop = false;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(5, 305);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(350, 30);
			this.button1.TabIndex = 9;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(5, 275);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(350, 25);
			this.label7.TabIndex = 11;
			this.label7.Text = "www.tesi.mi.it";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FormInfo
			// 
			this.ClientSize = new System.Drawing.Size(361, 340);
			this.ControlBox = false;
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormInfo";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CleanTrack";
			this.Load += new System.EventHandler(this.FormInfo_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void FormInfo_Load(object sender, System.EventArgs e)
		{
			// KleanTrak.Globals.LocalizzaDialog(this);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

	}
}
