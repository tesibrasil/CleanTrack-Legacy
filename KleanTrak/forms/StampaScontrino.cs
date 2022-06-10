using System;
using System.Data.Odbc;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace KleanTrak
{
	public class StampaScontrino : ChangeStateBaseForm
	{
		private int m_iIDUltimaRigaStampata = 0;
		private PrintDocument printDocument1;

        public StampaScontrino()
		{
            InitializeComponent();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
				components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StampaScontrino));
            this.textBoxDispositivo = new System.Windows.Forms.TextBox();
            this.textBoxOperatore = new System.Windows.Forms.TextBox();
            this.labelDispositivo1 = new System.Windows.Forms.Label();
            this.labelOperatore1 = new System.Windows.Forms.Label();
            this.btnAnnulla = new System.Windows.Forms.Button();
            this.btnSalva = new System.Windows.Forms.Button();
            this.labelOperatore2 = new System.Windows.Forms.Label();
            this.labelDispositivo2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelAutoOperatore = new System.Windows.Forms.Label();
            this.labelAutoDispositivo = new System.Windows.Forms.Label();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.SuspendLayout();
            // 
            // textBoxDispositivo
            // 
            this.textBoxDispositivo.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDispositivo.ForeColor = System.Drawing.Color.Orange;
            this.textBoxDispositivo.Location = new System.Drawing.Point(218, 170);
            this.textBoxDispositivo.Name = "textBoxDispositivo";
            this.textBoxDispositivo.Size = new System.Drawing.Size(518, 42);
            this.textBoxDispositivo.TabIndex = 2;
            this.textBoxDispositivo.Visible = false;
            this.textBoxDispositivo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDispositivo_KeyDown);
            // 
            // textBoxOperatore
            // 
            this.textBoxOperatore.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOperatore.ForeColor = System.Drawing.Color.Blue;
            this.textBoxOperatore.Location = new System.Drawing.Point(218, 80);
            this.textBoxOperatore.Name = "textBoxOperatore";
            this.textBoxOperatore.PasswordChar = '*';
            this.textBoxOperatore.Size = new System.Drawing.Size(518, 42);
            this.textBoxOperatore.TabIndex = 0;
            this.textBoxOperatore.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxOperatore_KeyDown);
            // 
            // labelDispositivo1
            // 
            this.labelDispositivo1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDispositivo1.ForeColor = System.Drawing.Color.Black;
            this.labelDispositivo1.Location = new System.Drawing.Point(9, 170);
            this.labelDispositivo1.Name = "labelDispositivo1";
            this.labelDispositivo1.Size = new System.Drawing.Size(182, 44);
            this.labelDispositivo1.TabIndex = 1;
            this.labelDispositivo1.Text = "Dispositivo:";
            this.labelDispositivo1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelDispositivo1.Visible = false;
            // 
            // labelOperatore1
            // 
            this.labelOperatore1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOperatore1.ForeColor = System.Drawing.Color.Blue;
            this.labelOperatore1.Location = new System.Drawing.Point(9, 80);
            this.labelOperatore1.Name = "labelOperatore1";
            this.labelOperatore1.Size = new System.Drawing.Size(182, 45);
            this.labelOperatore1.TabIndex = 0;
            this.labelOperatore1.Text = "Operatore:";
            this.labelOperatore1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAnnulla
            // 
            this.btnAnnulla.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAnnulla.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnnulla.Location = new System.Drawing.Point(386, 268);
            this.btnAnnulla.Name = "btnAnnulla";
            this.btnAnnulla.Size = new System.Drawing.Size(300, 53);
            this.btnAnnulla.TabIndex = 4;
            this.btnAnnulla.Text = "Annulla";
            this.btnAnnulla.Click += new System.EventHandler(this.btnAnnulla_Click);
            // 
            // btnSalva
            // 
            this.btnSalva.Enabled = false;
            this.btnSalva.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSalva.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalva.Location = new System.Drawing.Point(59, 268);
            this.btnSalva.Name = "btnSalva";
            this.btnSalva.Size = new System.Drawing.Size(309, 53);
            this.btnSalva.TabIndex = 5;
            this.btnSalva.Text = "Conferma";
            this.btnSalva.Click += new System.EventHandler(this.btnSalva_Click);
            // 
            // labelOperatore2
            // 
            this.labelOperatore2.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOperatore2.ForeColor = System.Drawing.Color.Black;
            this.labelOperatore2.Location = new System.Drawing.Point(200, 80);
            this.labelOperatore2.Name = "labelOperatore2";
            this.labelOperatore2.Size = new System.Drawing.Size(545, 45);
            this.labelOperatore2.TabIndex = 6;
            this.labelOperatore2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelOperatore2.Visible = false;
            // 
            // labelDispositivo2
            // 
            this.labelDispositivo2.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDispositivo2.ForeColor = System.Drawing.Color.Black;
            this.labelDispositivo2.Location = new System.Drawing.Point(200, 170);
            this.labelDispositivo2.Name = "labelDispositivo2";
            this.labelDispositivo2.Size = new System.Drawing.Size(545, 44);
            this.labelDispositivo2.TabIndex = 0;
            this.labelDispositivo2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelDispositivo2.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelAutoOperatore
            // 
            this.labelAutoOperatore.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAutoOperatore.ForeColor = System.Drawing.Color.Blue;
            this.labelAutoOperatore.Location = new System.Drawing.Point(9, 38);
            this.labelAutoOperatore.Name = "labelAutoOperatore";
            this.labelAutoOperatore.Size = new System.Drawing.Size(736, 42);
            this.labelAutoOperatore.TabIndex = 8;
            this.labelAutoOperatore.Text = "Passare badge utente";
            this.labelAutoOperatore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAutoOperatore.Visible = false;
            // 
            // labelAutoDispositivo
            // 
            this.labelAutoDispositivo.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAutoDispositivo.ForeColor = System.Drawing.Color.Orange;
            this.labelAutoDispositivo.Location = new System.Drawing.Point(5, 130);
            this.labelAutoDispositivo.Name = "labelAutoDispositivo";
            this.labelAutoDispositivo.Size = new System.Drawing.Size(735, 40);
            this.labelAutoDispositivo.TabIndex = 9;
            this.labelAutoDispositivo.Text = "Passare tag sonda";
            this.labelAutoDispositivo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAutoDispositivo.Visible = false;
            // 
            // ConsegnaDispositivo
            // 
            this.ClientSize = new System.Drawing.Size(750, 331);
			this.ControlBox = false;
			this.Controls.Add(this.labelAutoDispositivo);
            this.Controls.Add(this.labelAutoOperatore);
            this.Controls.Add(this.textBoxOperatore);
            this.Controls.Add(this.textBoxDispositivo);
            this.Controls.Add(this.labelDispositivo2);
            this.Controls.Add(this.labelOperatore2);
            this.Controls.Add(this.btnSalva);
            this.Controls.Add(this.btnAnnulla);
            this.Controls.Add(this.labelOperatore1);
            this.Controls.Add(this.labelDispositivo1);
            this.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Stampascontrino";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "CLEAN TRACK - Stampa scontrino ultimo ciclo";
            this.Load += new System.EventHandler(this.ConsegnaDispositivo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
		}

        private void ConsegnaDispositivo_Load(object sender, System.EventArgs e)
		{
			Globals.LocalizzaDialog(this);
            base.OnLoad();
		}

		private int id_prev_cycle = -1;
		private void btnSalva_Click(object sender, System.EventArgs e)
		{
            timer1.Enabled = false;
			if (!DBUtil.GetLastCycleId(m_nDispositivo, out int id_last_cycle))
			{
				MessageBox.Show(Globals.strTable[156], "Clean Track");
				return;
			}
			CicliDispositivo.StampaDettaglioCiclo(id_last_cycle, m_nDispositivo, printDocument1);
            m_nDispositivo = -1;
        }
		private void btnAnnulla_Click(object sender, System.EventArgs e)
		{
            timer1.Enabled = false;
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		private void timer1_Tick(object sender, System.EventArgs e)
		{
			m_iConfirmSeconds--;

			this.btnSalva.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));

            if (m_iConfirmSeconds >= 0)
			    btnSalva.Text = KleanTrak.Globals.strTable[115] + m_iConfirmSeconds.ToString() + KleanTrak.Globals.strTable[116];

            if (m_iConfirmSeconds == 0)
                btnSalva_Click(null, null);
		}
	}
}
