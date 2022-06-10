using KleanTrak.Model;
using KleanTrak.Core;
using LibLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KleanTrak
{
    public partial class SediUO : Form
    {
        public List<Query.ComboboxItem> seeds = null;
        public List<Query.ComboboxItem> uos = null;
        public List<DBUtil.UoSedi> uos_sites = null;
        public SediUO()
        {
            InitializeComponent();
        }

        private void UpdateModel()
        {
            try
            {
                seeds = DBUtil.GetSeeds();
                uos = DBUtil.GetUoList();
                uos_sites = DBUtil.GetSeedsUo();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "UpdateModel");
                throw;
            }
        }
        private void UpdateView()
        {
            try
            {
                tvsediuo.Nodes.Clear();
                foreach (var uo in uos)
                {
                    var uo_node = new TreeNode { Text = uo.Text, Tag = uo };
                    uo_node.Nodes.AddRange(GetUoNodes(uo.Value));
                    tvsediuo.Nodes.Add(uo_node);
                }
                tvsediuo.ExpandAll();
                tvsediuo_AfterSelect(this, new TreeViewEventArgs(null));
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "UpdateView");
                throw;
            }
        }
        private void SediUO_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateModel();
                UpdateView();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "SediUO_Load");
                MessageBox.Show(ex.ToString());
            }
        }

        private TreeNode[] GetUoNodes(int value)
        {
            var retlist = new List<TreeNode>();
            try
            {
                var seedids = uos_sites
                    .Where(uos => uos.iduo == value)
                    .Select(uos => uos.idsede);
                foreach (var seed in seeds.Where(s => seedids.Contains(s.Value)))
                    retlist.Add(new TreeNode { Text = seed.Text, Tag = seed });
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "GetUoNodes");
                MessageBox.Show(ex.ToString());
            }
            return retlist.ToArray();
        }

        private void tvsediuo_MouseClick(object sender, MouseEventArgs e)
        {
            return; // use buttons instead
            if (e.Button == MouseButtons.Left)
                return;
            context_menu.Show(this, e.Location);
        }

        private void tsmi_add_uo_Click(object sender, EventArgs e)
        {
            try
            {
                var itfrm = new InsertText { Label = "Inserire Descrizione UO" };
                itfrm.ShowDialog();
                if (!itfrm.DataValid)
                    return;
                if (!DBUtil.AddUo(itfrm.UserText, out int newid))
                    return;
                UpdateModel();
                UpdateView();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "tsmi_add_uo_Click");
                MessageBox.Show(ex.ToString());
            }
        }

        private void tsmi_add_site_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvsediuo.SelectedNode == null || tvsediuo.SelectedNode.Parent != null)
                {
                    MessageBox.Show("Selezionare prima una UO",
                        "Selezione Mancante",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                var iduo = (tvsediuo.SelectedNode.Tag as Query.ComboboxItem).Value;
                int siteid = -1;
                var result = MessageBox.Show("Si desidera selezionarne una sede esistente?",
                    "Scelta Sede",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (DialogResult.Yes == result)
                {
                    var choosefrm = new ChooseSeed();
                    choosefrm.ShowDialog();
                    if (choosefrm.DataValid)
                    {
                        siteid = choosefrm.Idseed;
                        if (uos_sites.Where(us => us.iduo == iduo && us.idsede == siteid).Count() > 0)
                        {
                            MessageBox.Show("Sede già associata alla UO",
                                "Sede Già Associata",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                            siteid = -1;
                        }
                    }
                }
                else
                {
                    var itfrm = new InsertText { Label = "Inserire Descrizione Sede" };
                    itfrm.ShowDialog();
                    if (!itfrm.DataValid)
                        return;
                    if (!DBUtil.AddSite(itfrm.UserText, out siteid))
                        return;
                }
                if (siteid <= 0)
                    return;
                if (!DBUtil.AddSiteUo(iduo, siteid))
                    return;
                UpdateModel();
                UpdateView();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "tsmi_add_site_Click");
                MessageBox.Show(ex.ToString());
            }
        }

        private void tsmi_rename_uo_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvsediuo.SelectedNode == null || tvsediuo.SelectedNode.Parent != null)
                {
                    MessageBox.Show("Selezionare prima una UO",
                        "Selezione Mancante",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                var iduo = (tvsediuo.SelectedNode.Tag as Query.ComboboxItem).Value;
                var itfrm = new InsertText { Label = "Inserire Descrizione UO" };
                itfrm.ShowDialog();
                if (!itfrm.DataValid)
                    return;
                if (!DBUtil.RenameUo(iduo, itfrm.UserText))
                    return;
                UpdateModel();
                UpdateView();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "tsmi_rename_uo_Click");
                MessageBox.Show(ex.ToString());
            }
        }

        private void tsmi_rename_site_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvsediuo.SelectedNode == null || tvsediuo.SelectedNode.Parent == null)
                {
                    MessageBox.Show("Selezionare prima una Sede",
                        "Selezione Mancante",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                var siteid = (tvsediuo.SelectedNode.Tag as Query.ComboboxItem).Value;
                var itfrm = new InsertText { Label = "Inserire Descrizione Sede" };
                itfrm.ShowDialog();
                if (!itfrm.DataValid)
                    return;
                if (!DBUtil.RenameSite(siteid, itfrm.UserText))
                    return;
                UpdateModel();
                UpdateView();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "tsmi_rename_uo_Click");
                MessageBox.Show(ex.ToString());
            }
        }

        private void tsmi_delete_uo_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvsediuo.SelectedNode == null || tvsediuo.SelectedNode.Parent != null)
                {
                    MessageBox.Show("Selezionare prima una UO",
                        "Selezione Mancante",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                var iduo = (tvsediuo.SelectedNode.Tag as Query.ComboboxItem).Value;
                if (uos_sites.Where(us => us.iduo == iduo).Count() > 0)
                {
                    MessageBox.Show("Eliminare prima le sedi di questa UO",
                        "Eliminare Sedi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                if (!DBUtil.DeleteUo(iduo))
                    return;
                UpdateModel();
                UpdateView();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "tsmi_rename_uo_Click");
                MessageBox.Show(ex.ToString());
            }
        }

        private void tsmi_delete_site_Click(object sender, EventArgs e)
        {
            try
            {
                if (tvsediuo.SelectedNode == null || tvsediuo.SelectedNode.Parent == null)
                {
                    MessageBox.Show("Selezionare prima una Sede",
                        "Selezione Mancante",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    return;
                }
                var siteid = (tvsediuo.SelectedNode.Tag as Query.ComboboxItem).Value;
                var uoid = (tvsediuo.SelectedNode.Parent.Tag as Query.ComboboxItem).Value;
                var items = new List<ComboItem>();
                items.Add(new ComboItem { Description = "Elimina Sede Definitivamente", Id = 1 });
                items.Add(new ComboItem { Description = "Elimina Associazione Sede UO", Id = 2 });
                var frmchoose = new ChooseItem();
                frmchoose.SetItems(items);
                frmchoose.Combo_label = "Scegliere la azione";
                frmchoose.Text = "Azione";
                frmchoose.ShowDialog();
                if (!frmchoose.Data_valid)
                    return;
                if (frmchoose.Selected_item.Id == 2)
                {
                    if (!DBUtil.DeleteSiteUo(uoid, siteid))
                        return;
                }
                else
                {
                    // eliminazione definitiva check dati
                    var msg = "";
                    if (StateTransactions.Get().Where(st => st.Id_sede == siteid).Count() > 0)
                        msg = $"Ci sono transazioni di stato associate a questa sede {Environment.NewLine}";
                    if (Readers.Get(siteid).Count() > 0)
                        msg += $"Ci sono Lettori associati a questa sede {Environment.NewLine}";
                    if (Devices.GetDevicesList(siteid, null, "", true).Count > 0)
                        msg += $"Ci sono Sonde associate a questa sede {Environment.NewLine}";
                    if (DBUtil.GetOperatorsCount(siteid) > 0)
                        msg += $"Ci sono Operatori associati a questa sede {Environment.NewLine}";
                    if (DBUtil.GetWashersCount(siteid) > 0)
                        msg += $"Ci sono Lavatrici, Pompe o Armadi associati a questa sede {Environment.NewLine}";
                    if (msg.Length > 0)
                    {
                        MessageBox.Show(msg,
                            "Eliminare Associazioni",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        return;
                    }
                    if (!DBUtil.DeleteSite(siteid))
                        return;
                }
                UpdateModel();
                UpdateView();
            }
            catch (Exception ex)
            {
                Logger.Get().Write(ex, "tsmi_delete_site_Click");
                MessageBox.Show(ex.ToString());
            }
        }      

        private void tvsediuo_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null && e.Node.Parent == null) // selezionata UO
            {
                buttonAddUO.Enabled = true;
                buttonRenameUO.Enabled = true;
                buttonDeleteUO.Enabled = true;
                buttonAddSite.Enabled = true;
                buttonRenameSite.Enabled = false;
                buttonDeleteSite.Enabled = false;
            }
            else if (e.Node != null && e.Node != null) // selezionata sede
            {
                buttonAddUO.Enabled = true;
                buttonRenameUO.Enabled = false;
                buttonDeleteUO.Enabled = false;
                buttonAddSite.Enabled = false;
                buttonRenameSite.Enabled = true;
                buttonDeleteSite.Enabled = true;
            }
            else
            {
                buttonAddUO.Enabled = true;
                buttonRenameUO.Enabled = false;
                buttonDeleteUO.Enabled = false;
                buttonAddSite.Enabled = false;
                buttonRenameSite.Enabled = false;
                buttonDeleteSite.Enabled = false;
            }
        }
    }
}
