using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KleanTrak.Core;
using LibLog;

namespace KleanTrak
{
    public partial class Query : Form
    {
        public class Ciclo
        {
            public int Id { get; set; }
            public string Dispositivo { get; set; }
            public string OperatoreInizio { get; set; }
            public string OperatoreFine { get; set; }
            public string Sterilizzatrice { get; set; }
            public string DataInizio { get; set; }
            public string DataFine { get; set; }
            public string Esito { get; set; }
        }
        private enum Col_index
        {
            Id,
            Dispositivo,
            OperatoreInizio,
            OperatoreFine,
            Sterilizzatrice,
            DataInizio,
            DataFine,
            Esito
        }
        public Query()
        {
            InitializeComponent();
            LoadData();
            KleanTrak.Globals.LocalizzaDialog(this);

            checkBoxCycleId_CheckedChanged(this, null);
            dateCheck_CheckedChanged(this, null);

            dataGridView.MultiSelect = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.SelectionChanged += DataGridView_SelectionChanged;
        }

        private void DataGridView_SelectionChanged(object sender, EventArgs e)
        {
            bool valid = dataGridView.SelectedRows.Count == 1;
            buttonApri.Enabled = valid;
            buttonStampa.Enabled = valid;
        }

        private void LoadData()
        {
            comboOperatore.DataSource = DBUtil.LoadOperatori(Globals.IDSEDE);
            comboDispositivo.DataSource = DBUtil.LoadDispositivi(Globals.IDSEDE);
            comboSterilizzatrice.DataSource = DBUtil.LoadWashers(Globals.IDSEDE, true, true);
            loadCompletato();
        }

        private void loadCompletato()
        {
            List<ComboboxItem> sourceCompleted = new List<ComboboxItem>();
            ComboboxItem item = new ComboboxItem();
            item.Text = "OK";
            item.Value = 1;
            ComboboxItem item2 = new ComboboxItem();
            item2.Text = "FAILED";
            item2.Value = 0;

            KleanTrak.Query.ComboboxItem dummy = new KleanTrak.Query.ComboboxItem()
            {
                Text = "",
                Value = -1
            };

            sourceCompleted.Add(dummy);
            sourceCompleted.Add(item);
            sourceCompleted.Add(item2);

            comboCompletato.DataSource = sourceCompleted;
        }

        private void buttonCerca_Click(object sender, EventArgs e)
        {
            try
            {
                List<Query.Ciclo> source;
                if (checkBoxCycleId.Checked)
                {
                    var cycle = DBUtil.GetCycle(Convert.ToInt32(numericUpDownIdCiclo.Value));
                    if (cycle == null)
                        throw new ApplicationException($"Ciclo con ID {numericUpDownIdCiclo.Value} non trovato");
                    source = new List<Ciclo> { cycle };
                }
                else
                {
                    int dispositivo = 0;
                    int operatore = 0;
                    int sterilizzatrice = 0;
                    string dateFromS = "";
                    string dateToS = "";
                    int successoCiclo = -1;

                    ComboboxItem tempItem = (ComboboxItem)comboDispositivo.SelectedItem;

                    if (tempItem != null && tempItem.Value > 0)
                    {
                        dispositivo = ((ComboboxItem)comboDispositivo.SelectedItem).Value;
                    }

                    tempItem = (ComboboxItem)comboOperatore.SelectedItem;

                    if (tempItem != null && tempItem.Value > 0)
                    {
                        operatore = ((ComboboxItem)comboOperatore.SelectedItem).Value;
                    }

                    tempItem = (ComboboxItem)comboSterilizzatrice.SelectedItem;

                    if (tempItem != null && tempItem.Value > 0)
                    {
                        sterilizzatrice = ((ComboboxItem)comboSterilizzatrice.SelectedItem).Value;
                    }

                    tempItem = (ComboboxItem)comboCompletato.SelectedItem;

                    if (tempItem != null && tempItem.Value >= 0)
                    {
                        successoCiclo = ((ComboboxItem)comboCompletato.SelectedItem).Value;
                    }

                    if (dateCheck.Checked)
                    {
                        dateFromS = dateFrom.Value.ToString("yyyyMMdd000000");
                        dateToS = dateTo.Value.ToString("yyyyMMdd235959");
                    }
                    source = DBUtil.GetCycles(dispositivo, operatore, sterilizzatrice, dateFromS, dateToS, successoCiclo, Globals.strDatabase);
                }
                dataGridView.DataSource = source;
                ResizeGrid();
            }
            catch (Exception exc)
            {
                dataGridView.DataSource = null;
                MessageBox.Show(exc.Message, "Error");
            }
        }


        public class ComboboxItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
            public override string ToString()
            {
                return Text;
            }
        }

        private void Query_ClientSizeChanged(object sender, EventArgs e) => ResizeGrid();
        private void Query_Shown(object sender, EventArgs e) => ResizeGrid();

        private void ResizeGrid()
        {
            try
            {
                int cent_width = System.Convert.ToInt32(Math.Floor((decimal)this.Width / 100));
                dataGridView.Columns[(int)Col_index.Id].Width = cent_width * 5;
                dataGridView.Columns[(int)Col_index.Dispositivo].Width = cent_width * 10;
                dataGridView.Columns[(int)Col_index.OperatoreInizio].Width = cent_width * 15;
                dataGridView.Columns[(int)Col_index.OperatoreFine].Width = cent_width * 15;
                dataGridView.Columns[(int)Col_index.Sterilizzatrice].Width = cent_width * 15;
                dataGridView.Columns[(int)Col_index.DataInizio].Width = cent_width * 15;
                dataGridView.Columns[(int)Col_index.DataFine].Width = cent_width * 15;
                dataGridView.Columns[(int)Col_index.Esito].Width = cent_width * 5;
            }
            catch (Exception ex)
            {
                Logger.Get().Write("", "Kleantrak.Query", ex.ToString(), null, Logger.LogLevel.Error);
            }
        }

        private void checkBoxCycleId_CheckedChanged(object sender, EventArgs e)
        {
            var flag = checkBoxCycleId.Checked;

            numericUpDownIdCiclo.Enabled = flag;

            panelComplete.Enabled = !flag;
            panelDisp.Enabled = !flag;
            panelOp.Enabled = !flag;
            panelDate.Enabled = !flag;
            panelSter.Enabled = !flag;
        }

        private void dateCheck_CheckedChanged(object sender, EventArgs e)
        {
            dateFrom.Enabled = dateCheck.Checked;
            dateTo.Enabled = dateCheck.Checked;
        }

        private void buttonApri_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                List<Ciclo> l = dataGridView.DataSource as List<Query.Ciclo>;
                var cycle = l[row.Index];
                new FrmReceiptsView(cycle.Id).ShowDialog();           
            }
            catch (Exception ex)
            {
                Logger.Get().Write("", "Kleantrak.Query", ex.ToString(), null, Logger.LogLevel.Error);
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void buttonStampa_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridViewRow row = dataGridView.SelectedRows[0];
                List<Ciclo> l = dataGridView.DataSource as List<Query.Ciclo>;
                var cycle = l[row.Index];
                int deviceId = -1;
                foreach (var cbItem in comboDispositivo.DataSource as List<ComboboxItem>)
                {
                    if (cbItem.Text == cycle.Dispositivo)
                    {
                        deviceId = cbItem.Value;
                        break;
                    }
                }
                CicliDispositivo.StampaDettaglioCiclo(cycle.Id, deviceId, new System.Drawing.Printing.PrintDocument());
            }
            catch (Exception ex)
            {
                Logger.Get().Write("", "Kleantrak.Query", ex.ToString(), null, Logger.LogLevel.Error);
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            buttonApri_Click(this, null);
        }
    }
}
