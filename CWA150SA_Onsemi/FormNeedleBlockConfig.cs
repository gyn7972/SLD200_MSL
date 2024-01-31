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
using QMC.Common.Modules;
using QMC.Common.Parts;

namespace CWA150SA_Onsemi300
{

    public partial class FormNeedleBlockConfig : FormSubContentBase
    {
        
        private NeedleBlock m_Owner;
        public FormNeedleBlockConfig(Part part)
            : base(FormType.withButton.ToString(), part.Name)
        {
            InitializeComponent();
            m_Owner = part as NeedleBlock;

            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();

            this.Controls.Add(this.GroupBoxCapPosition);
            this.Controls.Add(this.GroupBoxColletOffsets);
            this.Controls.Add(this.GroupBoxNeedleBlockPosition);
            this.Controls.Add(this.GroupBoxNeedlePosition);
            this.Controls.Add(this.GroupBoxNeedleParameter);

            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;
            this.flowLayoutPanelButton.Location = new Point(0, this.baseLabelTitle.Location.Y + this.baseLabelTitle.Height);

            this.CapPositionConfigGrid.Size = new Size(Configuration.ContentSize.Width / 4, Configuration.ContentSize.Height / 4);
            this.ColletOffsetsConfigGrid.Size = this.CapPositionConfigGrid.Size;
            this.NeedleBlockPositionConfigGrid.Size = this.CapPositionConfigGrid.Size;
            this.NeedlePositionConfigGrid.Size = this.CapPositionConfigGrid.Size;

            this.GroupBoxCapPosition.Size = new Size(this.CapPositionConfigGrid.Width + 10, this.CapPositionConfigGrid.Height + 20);
            this.GroupBoxColletOffsets.Size = this.GroupBoxCapPosition.Size;
            this.GroupBoxNeedlePosition.Size = this.GroupBoxCapPosition.Size;
            this.GroupBoxNeedleBlockPosition.Size = this.GroupBoxCapPosition.Size;

            this.GroupBoxCapPosition.Controls.Add(CapPositionConfigGrid);
            this.GroupBoxColletOffsets.Controls.Add(this.ColletOffsetsConfigGrid);
            this.GroupBoxNeedlePosition.Controls.Add(this.NeedlePositionConfigGrid);
            this.GroupBoxNeedleBlockPosition.Controls.Add(this.NeedleBlockPositionConfigGrid);

            this.CapPositionConfigGrid.Location = new Point(5, 15);
            this.ColletOffsetsConfigGrid.Location = new Point(5, 15);
            this.NeedleBlockPositionConfigGrid.Location = new Point(5, 15);
            this.NeedlePositionConfigGrid.Location = new Point(5, 15);

            this.GroupBoxCapPosition.Location = new Point(Configuration.ContentLocation.X, this.flowLayoutPanelButton.Location.Y + this.flowLayoutPanelButton.Height + Configuration.ControlGap);
            this.GroupBoxNeedleBlockPosition.Location = new Point(this.GroupBoxCapPosition.Location.X, this.GroupBoxCapPosition.Location.Y + this.GroupBoxCapPosition.Size.Height + 5);
            this.GroupBoxNeedlePosition.Location = new Point(this.GroupBoxCapPosition.Location.X + this.GroupBoxCapPosition.Width + 10, this.GroupBoxCapPosition.Location.Y);
            this.GroupBoxColletOffsets.Location = new Point(this.GroupBoxNeedlePosition.Location.X, this.GroupBoxNeedleBlockPosition.Location.Y);

            this.GroupBoxNeedleParameter.Location = new Point(this.GroupBoxNeedlePosition.Location.X + this.GroupBoxNeedlePosition.Width + 15, this.GroupBoxNeedlePosition.Location.Y);
            this.PropertyGridNeedleParameter.Size = new Size(Configuration.ContentSize.Width / 3, Configuration.ContentSize.Height / 2 + 25);
            this.GroupBoxNeedleParameter.Size = new Size(this.PropertyGridNeedleParameter.Width + 10, this.PropertyGridNeedleParameter.Height + 20);
            this.GroupBoxNeedleParameter.Controls.Add(this.PropertyGridNeedleParameter);
            this.PropertyGridNeedleParameter.Location = new Point(5, 15);

            CreateButton();
            UpdateDataGridViewCapPosition();
            UpdateDataGridViewNeedleBlockPosition();
            UpdateDataGridViewNeedlePosition();
            UpdateDataGridViewColletOffsets();
            SetGridConfigData();
        }

        #region Method
        private void UpdateDataGridViewCapPosition()
        {
            CapPositionConfigGrid.Columns.Clear();
            CapPositionConfigGrid.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Position";
                CapPositionConfigGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Target";
                CapPositionConfigGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Z";
                column.Name = "Z[mm]";
                CapPositionConfigGrid.Columns.Add(column);
            }
        }

        private void UpdateDataGridViewNeedleBlockPosition()
        {
            NeedleBlockPositionConfigGrid.Columns.Clear();
            NeedleBlockPositionConfigGrid.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Position";
                NeedleBlockPositionConfigGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Target";
                NeedleBlockPositionConfigGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "X";
                column.Name = "X[mm]";
                NeedleBlockPositionConfigGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Y";
                column.Name = "Y[mm]";
                NeedleBlockPositionConfigGrid.Columns.Add(column);
            }
        }

        private void UpdateDataGridViewNeedlePosition()
        {
            NeedlePositionConfigGrid.Columns.Clear();
            NeedlePositionConfigGrid.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Position";
                NeedlePositionConfigGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Type";
                column.Name = "Target";
                NeedlePositionConfigGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Z";
                column.Name = "Z[mm]";
                NeedlePositionConfigGrid.Columns.Add(column);
            }
        }

        private void UpdateDataGridViewColletOffsets()
        {
            ColletOffsetsConfigGrid.Columns.Clear();
            ColletOffsetsConfigGrid.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";
                ColletOffsetsConfigGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "OffsetX";
                column.Name = "OffsetX";
                ColletOffsetsConfigGrid.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "OffsetY";
                column.Name = "OffsetY";
                ColletOffsetsConfigGrid.Columns.Add(column);
            }
        }
        
        private void SetGridConfigData()
        {
            CapPositionConfigGrid.DataSource = m_Owner.Config.CapPositions;
            NeedlePositionConfigGrid.DataSource = m_Owner.Config.NeedlePosition;
            NeedleBlockPositionConfigGrid.DataSource = m_Owner.Config.NeedleBlockPosition;
            ColletOffsetsConfigGrid.DataSource = m_Owner.Config.ColletOffsets;
            PropertyGridNeedleParameter.SelectedObject = m_Owner.Config;
        }

        #endregion

        public void CreateButton()
        {
            /*
            foreach (FormDieLoderConfig.ButtonType buttonInterlock in Enum.GetValues(typeof(FormDieLoderConfig.ButtonType)))
            {
                BaseButton btn = new BaseButton();
                switch (buttonInterlock)
                {
                    case FormDieLoderConfig.ButtonType.Save:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Click += SaveButton_Click;
                        break;
                    case FormDieLoderConfig.ButtonType.Load:
                        btn.Text = buttonInterlock.ToString();
                        btn.Name = string.Format("button", buttonInterlock.ToString());
                        btn.Click += LoadButton_Click;
                        break;
                }
                flowLayoutPanelButton.Controls.Add(btn);
            }
            */
        }

        #region EventHandler
        private void SaveButton_Click(object sender, EventArgs e)
        {
            /*
            Module dieloader = m_Owner.Owner as DieLoader;
            if (dieloader != null)
            {
                DataManager.Instance.UpdateConfigData(dieloader);
                dieloader.UpdateConfigData();
            }
            */
        }
        private void LoadButton_Click(object sender, EventArgs e)
        {
            /*
            Module dieloader = m_Owner.Owner as DieLoader;
            if (dieloader != null)
            {
                dieloader.GetConfigData();
                DataManager.Instance.ApplyConfigData(dieloader);
            }
            */
        }
        #endregion
    }
}
