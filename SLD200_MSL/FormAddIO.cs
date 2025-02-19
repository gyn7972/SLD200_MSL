using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public partial class FormAddIO : Form
    {
        #region Property

        public IOPoint SelectedIO { set; get; }
        public DioPoint SelectedIOPoint { set; get; }
        public string ModuleUid { set; get; }
        public string PartUid { set; get; }
        public string Tag { set; get; }
        public string Locator { set; get; }
        #endregion

        #region Cosntructor
        public FormAddIO()
        {
            InitializeComponent();
        }
        #endregion

        #region Method

        public void GetLocatorInfo(string ModuleUid, string PartUid, DioPoint point)
        {
            if (ModuleUid == "" && PartUid == "")
            {
                point.Locator = "";
            }
            else if (ModuleUid == "")
            {
                point.Locator = PartUid;
            }
            else if (PartUid == "")
            {
                point.Locator = ModuleUid;
            }            
            else
            {
                point.Locator = ModuleUid + "/" + PartUid;
            }
        }
        public void GetLocatorInfo(string ModuleUid, string PartUid)
        {
            if (ModuleUid == "" && PartUid == "")
            {
                SelectedIO.Configuration.Locator = "";
            }
            else if (ModuleUid == "")
            {
                SelectedIO.Configuration.Locator = PartUid;
            }
            else if (PartUid == "")
            {
                SelectedIO.Configuration.Locator = ModuleUid;
            }
            else
            {
                SelectedIO.Configuration.Locator = ModuleUid + "/" + PartUid;
            }
        }
        private void InitbaseDataGridViewIOColumns()
        {
            dataGridViewIO.Columns.Clear();
            dataGridViewIO.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";
                dataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Locator";
                column.Name = "Locator";
                dataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Address";
                column.Name = "Address";
                dataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "IoType";
                column.Name = "Type";
                dataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "ModuleUid";
                column.Name = "ModuleUid";
                dataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "PartUid";
                column.Name = "PartUid";
                dataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Tag";
                column.Name = "Tag";
                dataGridViewIO.Columns.Add(column);
            }
        }
        #endregion

        #region Event Handler
        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (dataGridViewIO.Rows.Count < 0)
            {
                return;
            }
            int nRow = dataGridViewIO.SelectedCells[0].RowIndex;
            if (nRow >= 0)
            {
                SelectedIO = dataGridViewIO.Rows[nRow].DataBoundItem as IOPoint;
                SelectedIO.PartUid = PartUid;
                SelectedIO.ModuleUid = ModuleUid;
                SelectedIO.Tag = Tag;
                GetLocatorInfo(ModuleUid, PartUid);
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void FormAddIO_Load(object sender, EventArgs e)
        {
            dataGridViewIO.DataSource = null;
            InitbaseDataGridViewIOColumns();

            List<DioPoint> listIoPoint = Equipment.GetAllDioPointList();
            dataGridViewIO.DataSource = listIoPoint;
        }
        #endregion
    }
}
