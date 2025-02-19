using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public class BaseDataGridView : DataGridView
    {
        public BaseDataGridView()
        {
            this.DefaultCellStyle.BackColor = Color.FromArgb(200, 200, 200);
            this.BackgroundColor = Color.FromArgb(78, 78, 78);
            this.GridColor = Color.FromArgb(3, 3, 3);
            this.ForeColor = Color.FromArgb(3,3,3);

            this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(78, 78, 78);
            this.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(78, 78, 78);

            this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            this.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            this.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(78, 78, 78);
            this.RowHeadersDefaultCellStyle.ForeColor = Color.FromArgb(78, 78, 78);

            this.RowHeadersVisible = false;

            this.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            this.AllowUserToAddRows = false;
        }
        public DataGridViewRow GetNewRow()
        {
            DataGridViewRow row = new DataGridViewRow();
            if (this.Columns == null) return row;

            for (int i = 0; i < this.Columns.Count; i++)
            {
                if ((this.Columns[i] is DataGridViewButtonColumn))
                    row.Cells.Add(new DataGridViewButtonCell());

                else if ((this.Columns[i] is DataGridViewCheckBoxColumn))
                    row.Cells.Add(new DataGridViewCheckBoxCell());

                else if ((this.Columns[i] is DataGridViewComboBoxColumn))
                    row.Cells.Add(new DataGridViewComboBoxCell());

                else if ((this.Columns[i] is DataGridViewImageColumn))
                    row.Cells.Add(new DataGridViewImageCell());

                else if ((this.Columns[i] is DataGridViewLinkColumn))
                    row.Cells.Add(new DataGridViewLinkCell());

                else if ((this.Columns[i] is DataGridViewTextBoxColumn))
                    row.Cells.Add(new DataGridViewTextBoxCell());
            }
            return row;
        }
    }
}
