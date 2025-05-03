using QMC.Common;
using QMC.Common.Interpolator;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using PositionOffset = QMC.Common.VisionPart.PositionOffset;

namespace QMC.Vision
{
    public delegate PositionOffset InspectClickEventHandler();
    public partial class _2DMappingDataControl : UserControl
    {
        private XyzyStage m_Stage;
        protected List<PositionOffset> m_Positions;

        static WorkStage workStage;

        public event InspectClickEventHandler ClickInspect;
        public _2DMappingDataControl(XyzyStage stage)
        {
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }

            m_Stage = stage as XyzyStage;
            m_Positions = new List<PositionOffset>();
            InitializeComponent();

            SetEditable(false);
            SetInspectMode(false);
            UpdateGridViewColumns();
            //UpdateDataGridData();


            string strFileName = "";
            if (workStage.Config.ParamConfig.MapFile_Path != null)
            {
                strFileName = workStage.Config.ParamConfig.MapFile_Path;

                FileInfo fi = new FileInfo(strFileName);
                if (fi.Exists)
                {
                    if (m_Stage.Interpolator == null)
                        m_Stage.Interpolator = new PerspectiveProjectionInterpolator();
                    m_Stage.Interpolator.Load(strFileName);

                    UpdateDataGridData();
                }
            }            
        }

        public void SetEditable(bool bEdit)
        {
            this.baseDataGridView2DMapData.ReadOnly = !bEdit;
            this.baseDataGridView2DMapData.EditMode = DataGridViewEditMode.EditOnEnter;
        }

        public void SetInspectMode(bool bInspect)
        {
            if(bInspect)
            {
                baseButtonStop.Text = "Inspect";
            }
        }

        private void UpdateGridViewColumns()
        {
            this.baseDataGridView2DMapData.Columns.Clear();
            this.baseDataGridView2DMapData.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Position X";
                column.HeaderText = "PositionX";
                column.DataPropertyName = "PositionX";
                this.baseDataGridView2DMapData.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Position Y";
                column.HeaderText = "PositionY";
                column.DataPropertyName = "PositionY";
                this.baseDataGridView2DMapData.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "Offset X";
                column.HeaderText = "OffsetX";
                column.DataPropertyName = "OffsetX";
                this.baseDataGridView2DMapData.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Name = "OffSet Y";
                column.HeaderText = "OffsetY";
                column.DataPropertyName = "OffsetY";
                this.baseDataGridView2DMapData.Columns.Add(column);
            }
            UpdateDataGridData();
        }

        public void UpdateDataGridData()
        {
            this.baseDataGridView2DMapData.Rows.Clear();
            if (this.m_Stage.Interpolator != null)
            {
                m_Positions = this.m_Stage.Interpolator.GetPositionOffsetList();
                foreach (PositionOffset position in m_Positions)
                {
                    int nRow = this.baseDataGridView2DMapData.Rows.Add();
                    this.baseDataGridView2DMapData.Rows[nRow].Cells[0].Value = position.Position.X;
                    this.baseDataGridView2DMapData.Rows[nRow].Cells[1].Value = position.Position.Y;
                    this.baseDataGridView2DMapData.Rows[nRow].Cells[2].Value = position.Offset.X;
                    this.baseDataGridView2DMapData.Rows[nRow].Cells[3].Value = position.Offset.Y;
                }
            }
        }

        private void baseButton_Click(object sender, EventArgs e)
        {
            BaseButton button = sender as BaseButton;

            double m_dTarget_X = 0.0;
            double m_dTarget_Y = 0.0;
            double m_dVel = 50.0;
            double m_dAccDec = m_dVel * 10.0;

            if (button.Text == "Move")
            {
                if (this.baseDataGridView2DMapData.SelectedCells.Count > 0)
                {
                    int nSelectedIndex = this.baseDataGridView2DMapData.SelectedCells[0].RowIndex;
                    if (baseDataGridView2DMapData.RowCount > nSelectedIndex && nSelectedIndex >= 0)
                    {
                        //PositionOffset positionoffset = (PositionOffset)this.baseDataGridView2DMapData.Rows[nSelectedIndex].DataBoundItem;
                        PositionOffset positionoffset = m_Positions[nSelectedIndex];

                        m_dTarget_X = positionoffset.Position.X;
                        m_dTarget_Y = positionoffset.Position.Y;

                        this.m_Stage.MovePosition(new XyCoordinate(m_dTarget_X, m_dTarget_Y));

                        //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                        //                        (Axis)WorkStageParameter.AxisAcsEnum.StageY,
                        //                        m_dTarget_Y);                           //  Target position

                        //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                        //                        (Axis)WorkStageParameter.AxisAcsEnum.StageX,
                        //                        m_dTarget_X);                           //  Target position
                    }
                }
            }
            else if(button.Text == "Inspect")
            { 
                if(ClickInspect != null)
                {
                    PositionOffset result = ClickInspect();
                    if (result.Position.X != 0 || result.Position.Y != 0)
                    {
                        if (this.baseDataGridView2DMapData.SelectedCells.Count > 0)
                        {
                            int nSelectedIndex = this.baseDataGridView2DMapData.SelectedCells[0].RowIndex;
                            m_Positions[nSelectedIndex] = result;
                            this.baseDataGridView2DMapData.Rows[nSelectedIndex].Cells[0].Value = result.Position.X;
                            this.baseDataGridView2DMapData.Rows[nSelectedIndex].Cells[1].Value = result.Position.Y;
                            this.baseDataGridView2DMapData.Rows[nSelectedIndex].Cells[2].Value = result.Offset.X;
                            this.baseDataGridView2DMapData.Rows[nSelectedIndex].Cells[3].Value = result.Offset.Y;
                        }
                    }
                }
                else
                {
                    MessageBoxOk messageBox = new MessageBoxOk();
                    messageBox.ShowDialog("Error", "Event(ClickInspect)에 연결된 함수가 없습니다.");
                }
            }
            else if(button.Text == "Offset  Move")
            {
                if (this.baseDataGridView2DMapData.SelectedCells.Count > 0)
                {
                    int nSelectedIndex = this.baseDataGridView2DMapData.SelectedCells[0].RowIndex;
                    if (baseDataGridView2DMapData.RowCount > nSelectedIndex && nSelectedIndex >= 0)
                    {
                        //PositionOffset positionoffset = (PositionOffset)this.baseDataGridView2DMapData.Rows[nSelectedIndex].DataBoundItem;
                        PositionOffset positionoffset = m_Positions[nSelectedIndex];

                        m_dTarget_X = positionoffset.Position.X + positionoffset.Offset.X;
                        m_dTarget_Y = positionoffset.Position.Y + positionoffset.Offset.Y;

                        this.m_Stage.MovePosition(new XyCoordinate(m_dTarget_X, m_dTarget_Y));

                        //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                        //                        (Axis)WorkStageParameter.AxisAcsEnum.StageY,
                        //                        m_dTarget_Y);                           //  Target position

                        //ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                      //  '0' - Absolute position
                        //                        (Axis)WorkStageParameter.AxisAcsEnum.StageX,
                        //                        m_dTarget_X);                           //  Target position
                    }
                }
            }
            else
            {
                this.m_Stage.Stop();
                
            }
        }

        private void baseButtonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            string strFileName = "";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                strFileName = dialog.FileName;

                if (m_Stage.Interpolator == null)
                    m_Stage.Interpolator = new PerspectiveProjectionInterpolator();
                m_Stage.Interpolator.Load(strFileName);
            }

            UpdateDataGridData();
        }

        protected void UpdatePositionOffsetData()
        {
            m_Positions.Clear();
            foreach(DataGridViewRow row in baseDataGridView2DMapData.Rows)
            {
                
                PositionOffset position = new PositionOffset();
                string strValue;
                strValue = string.Format("{0}, {1}, {2}, {3}", row.Cells[0].Value, row.Cells[1].Value, row.Cells[2].Value, row.Cells[3].Value);
                position.SetValue(strValue);
                m_Positions.Add(position);
            }
        }

        private void baseButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            string strFileName = "";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                strFileName = dialog.FileName;
                UpdatePositionOffsetData();
                m_Stage.Interpolator.SetPositionOffsetList(m_Positions);
                if (m_Stage.Interpolator != null)
                {
                    m_Stage.Interpolator.Save(strFileName);
                }
            }
        }
    }
}
