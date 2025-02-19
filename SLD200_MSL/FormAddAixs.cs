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
    public partial class FormAddAixs : Form
    {
        private MotionAxis m_SelectedAxis;
        public string PartUid { get; set; }
        public string ModuleUid { get; set; }
        public string Tag { get; set; }
        public FormAddAixs()
        {
            InitializeComponent();
        }
        public MotionAxis SelectedAxis
        {
            get { return m_SelectedAxis; }
            set { m_SelectedAxis = value; }
        }

        private void InitDataGridViewActorListAxisColumns()
        {
            dataGridViewMotion.Columns.Clear();
            dataGridViewMotion.AutoGenerateColumns = false;            
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";
                dataGridViewMotion.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "No";
                column.Name = "No";
                dataGridViewMotion.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "BoardNo";
                column.Name = "BoardNo";
                dataGridViewMotion.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Module";
                column.Name = "ModuleUid";
                dataGridViewMotion.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Part";
                column.Name = "PartUid";
                dataGridViewMotion.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Tag";
                column.Name = "Tag";
                dataGridViewMotion.Columns.Add(column);
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if(dataGridViewMotion.Rows.Count == 0)
            {
                return;
            }
            int nRow = dataGridViewMotion.SelectedCells[0].RowIndex;
            if(nRow >= 0)
            {
                SelectedAxis = dataGridViewMotion.Rows[nRow].DataBoundItem as MotionAxis;
                SelectedAxis.Part = PartUid;
                SelectedAxis.Module = ModuleUid;
                SelectedAxis.Tag = Tag;
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

        private void FormAddAixs_Load(object sender, EventArgs e)
        {
            dataGridViewMotion.DataSource = null;
            InitDataGridViewActorListAxisColumns();

            List<MotionAxis> listMotions = Equipment.GetAllMotionAxisList();
            dataGridViewMotion.DataSource = listMotions;
        }
    }
}
