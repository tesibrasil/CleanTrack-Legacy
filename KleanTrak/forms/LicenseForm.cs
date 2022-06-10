using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KleanTrack.License;
using Newtonsoft.Json;
using LibLog;
using System.IO;

namespace KleanTrak
{
    public partial class LicenseForm : Form
    {
        private List<UoClaims> model = new List<UoClaims>();
        public LicenseForm()
        {
            InitializeComponent();
        }
        private void UpdateView()
        {
            try
            {
                dgvuo.Rows.Clear();
                foreach (var uo_claim in model)
                {
                    if (!uo_claim.Claims.TryGetValue("Armadi", out int armadi))
                        armadi = 0;
                    if (!uo_claim.Claims.TryGetValue("Lavatrici", out int lavatrici))
                        lavatrici = 0;
                    if (!uo_claim.Claims.TryGetValue("Pompe", out int pompe))
                        pompe = 0;
                    dgvuo.Rows.Add(uo_claim.Iduo,
                        lavatrici,
                        pompe,
                        armadi,
                        string.Join(",", uo_claim.Sedi.Keys),
                        string.Join(",", uo_claim.Sedi.Values));
                }
            }
            catch (Exception e)
            {
                Logger.Get().Write(e, "");
                throw;
            }
        }
        private void UpdateModel()
        {
            try
            {
                model.Clear();
                // l'ultima riga è per il nuovo inserimento
                for (int i = 0; i < dgvuo.Rows.Count - 1; i++)
                {
                    var newuoclaim = new UoClaims();
                    newuoclaim.Iduo = int.Parse(dgvuo.Rows[i].Cells[0].Value.ToString());
                    newuoclaim.Claims.Add("Lavatrici", int.Parse(dgvuo.Rows[i].Cells[1].Value.ToString()));
                    newuoclaim.Claims.Add("Pompe", int.Parse(dgvuo.Rows[i].Cells[2].Value.ToString()));
                    newuoclaim.Claims.Add("Armadi", int.Parse(dgvuo.Rows[i].Cells[3].Value.ToString()));
                    newuoclaim.Sedi = new Dictionary<int, string>();
                    var sediKeys = dgvuo.Rows[i].Cells[4].Value.ToString().Split(',', '.', ' ', '-', '_', '/', '\\');
                    var sediDescString = dgvuo.Rows[i].Cells[5].Value.ToString().Split(',', '.', ' ', '-', '_', '/', '\\');
                    var sediValues = new List<string>();
                    foreach (var descSede in sediDescString)
                    {
                        if (!string.IsNullOrWhiteSpace(descSede))
                            sediValues.Add(descSede);
                    }
                    int j = 0;
                    foreach (var s in sediKeys)
                    {
                        newuoclaim.Sedi.Add(int.Parse(s), sediValues.Count > j ? sediValues[j] : "");
                        j++;
                    }
                    model.Add(newuoclaim);
                }
            }
            catch (Exception e)
            {
                Logger.Get().Write(e, "");
                throw;
            }
        }
        private void btnloadfile_Click(object sender, EventArgs e)
        {
            FileStream fstream = null;
            StreamReader freader = null;
            try
            {
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName == "")
                    return;
                fstream = File.OpenRead(openFileDialog.FileName);
                freader = new StreamReader(fstream);
                model = JsonConvert.DeserializeObject<List<UoClaims>>(freader.ReadToEnd());
                UpdateView();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "");
                MessageBox.Show(ex.ToString(),
                    "Errore",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (freader != null)
                    freader.Close();
                if (fstream != null)
                    fstream.Close();
            }
        }
        private void btngenerate_Click(object sender, EventArgs e)
        {
            // aprire file save dialog per salvare il file
            FileStream fstream = null;
            try
            {
                UpdateModel();
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName == "")
                    return;
                var filecontente = JsonConvert.SerializeObject(model);
                fstream = File.OpenWrite(saveFileDialog.FileName);
                var bytes = Encoding.UTF8.GetBytes(filecontente);
                fstream.Write(bytes, 0, bytes.Count());
                fstream.Flush();
                fstream.Close();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "");
                MessageBox.Show(ex.ToString(),
                    "Errore",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (fstream != null)
                {
                    fstream.Close();
                    fstream.Dispose();
                }
            }
        }

        private void dgvuo_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.FormattedValue.ToString() == "")
                    return;
                int value = -1;
                if (e.ColumnIndex < 4)
                {
                    value = int.Parse(e.FormattedValue.ToString());
                    if (value < 0)
                        throw new ApplicationException("Il valore deve essere un numero intero positivo");
                }
                else if (e.ColumnIndex == 4) // sedi, must be comma separated ints
                {
                    var sedis = e.FormattedValue.ToString().Split(',', '.', ' ', '-', '_', '/', '\\');
                    foreach (var s in sedis)
                    {
                        bool isInt = int.TryParse(s, out int val);
                        if (!isInt || val < 0)
                            throw new ApplicationException("I valori devono essere numeri interi positivi, separati da virgola");
                    }
                }
                if (e.ColumnIndex != 0)
                    return;
                // controllo duplicazione iduo
                for (int i = 0; i < dgvuo.Rows.Count; i++)
                {
                    if (i == e.RowIndex || dgvuo.Rows[i].Cells[0].Value == null)
                        continue;
                    var iduo = int.Parse(dgvuo.Rows[i].Cells[0].Value.ToString());
                    if (iduo == value)
                    {
                        MessageBox.Show("Iduo già presente", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                e.Cancel = true;
            }
        }

        private void dgvuo_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (dgvuo.Rows[e.RowIndex].Cells[0].Value == null &&
                    dgvuo.Rows[e.RowIndex].Cells[1].Value == null &&
                    dgvuo.Rows[e.RowIndex].Cells[2].Value == null &&
                    dgvuo.Rows[e.RowIndex].Cells[3].Value == null &&
                    dgvuo.Rows[e.RowIndex].Cells[4].Value == null)
                    return;
                if (dgvuo.Rows[e.RowIndex].Cells[0].Value == null ||
                    dgvuo.Rows[e.RowIndex].Cells[1].Value == null ||
                    dgvuo.Rows[e.RowIndex].Cells[2].Value == null ||
                    dgvuo.Rows[e.RowIndex].Cells[3].Value == null ||
                    dgvuo.Rows[e.RowIndex].Cells[4].Value == null)
                    throw new ApplicationException();
            }
            catch (Exception)
            {
                MessageBox.Show("Completare tutte le colonne con numeri interi positivi o uguali a zero");
                e.Cancel = true;
            }
        }
    }
}
