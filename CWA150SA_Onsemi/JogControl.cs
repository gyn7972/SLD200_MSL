using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Parts;
using ACS.SPiiPlusNET;
using QMC.Common.Motion.ACS.Motions;
using System.Runtime.InteropServices.ComTypes;
using QMC.Common.UI;

#region Define
public enum JogControlAxisInfo
{
    Axis, AbsolutePos, RelativePos, Step, Velocity, SetZero, GoHome
}
public enum JogControlButtonList
{
    //buttonUp, buttonDown, buttonAxisXDownYDown, buttonAxisXUpYDown, buttonAxisXDownYUp, buttonAxisXUpYUp, buttonCW, buttonCCW, buttonLeft, buttonRight, combButtonUp, combButtonDown, combButtonLeft, combButtonRight, buttonFwd, buttonBwd
    buttonUp, buttonDown, buttonAxisXDownYDown, buttonAxisXUpYDown, buttonAxisXDownYUp, buttonAxisXUpYUp, buttonCW, buttonCCW, buttonLeft, buttonRight, combButtonUp, combButtonDown, combButtonLeft, combButtonRight, buttonFwd, buttonBwd
}
#endregion

namespace CWA150SA_Onsemi300
{

    public partial class JogControl : UserControl
    {
        List<MotionAxis> m_axis;
        AxisXRevision m_AxisXRevision;
        AxisYRevision m_AxisYRevision;
        JogButtonTheta m_JogButtonTheta;
        JogButtonCombinationUVW m_JogButtonCombination_UVW;
        JogButtonCombinationXY m_JogButtonCombination_XY;
        Dictionary<MotionAxis, double> m_relativeZeroPositions;
        Dictionary<MotionAxis, double> Step { get; set; }
        Dictionary<MotionAxis, double> Velocity { get; set; }
        public int m_IntervalTime { get; set; }
        FormBaseConfiguration m_Configuration { get; set; }
        MotionPart m_Part;
        private int m_nindex;

        private MotionFunction MC_Func;

        static WaferProbeAlign waferProbeAlign;


        public JogControl(Part part)
        {
            InitializeComponent();
            m_Part = part as MotionPart;
            m_Configuration = new FormBaseConfiguration();
            this.flowLayoutPanelJogButtonAxisX.FlowDirection = FlowDirection.TopDown;
            InitdataGridViewParameterColumns();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WaferProbeAlign")
                {
                    waferProbeAlign = module as WaferProbeAlign;
                }
            }

            m_JogButtonCombination_UVW = new JogButtonCombinationUVW();
            m_JogButtonCombination_XY = new JogButtonCombinationXY();
            m_relativeZeroPositions = new Dictionary<MotionAxis, double>();
            Step = new Dictionary<MotionAxis, double>();
            Velocity = new Dictionary<MotionAxis, double>();

            MC_Func = new MotionFunction();

            if (part != null)
            {
                m_axis = part.GetAxisList();
                if (m_axis.Count > 0)
                {
                    for (int i = 0; i < m_axis.Count; i++)
                    {
                        if (m_axis[i] != null)
                        {
                            Step.Add(m_axis[i], 1);
                            Velocity.Add(m_axis[i], 10);
                        }
                    }
                }

            }

            if (m_axis != null)
            {
                UpdateDataGridViewValue();
                AddJogButton();
            }

            startTimer(100);

            #region ControlConstructor

            this.flowLayoutPanelJogButtonComb_UVW.Location = new Point(this.radioButtonContinuous.Location.X, this.radioButtonContinuous.Location.Y + this.radioButtonContinuous.Height + 6);
            this.flowLayoutPanelJogButtonAxisX.Location = new Point(this.flowLayoutPanelJogButtonComb_UVW.Location.X + this.flowLayoutPanelJogButtonComb_UVW.Width + 5, this.flowLayoutPanelJogButtonComb_UVW.Location.Y);
            this.flowLayoutPanelJogButtonAxisY.Location = new Point(this.flowLayoutPanelJogButtonAxisX.Location.X + this.flowLayoutPanelJogButtonAxisX.Width + 0, this.flowLayoutPanelJogButtonComb_UVW.Location.Y);
            this.flowLayoutPanelJogButtonComb_UVW.Size = new Size(this.m_JogButtonCombination_UVW.Size.Width + 5, this.m_JogButtonCombination_UVW.Size.Height + 5);
            this.flowLayoutPanelJogButtonComb_XY.Size = new Size(this.m_JogButtonCombination_XY.Size.Width + 5, this.m_JogButtonCombination_XY.Size.Height);
            this.flowLayoutPanelJogButtonAxisX.FlowDirection = FlowDirection.TopDown;

            if (this.flowLayoutPanelJogButtonAxisX.Controls.Count > 0)
            {
                this.flowLayoutPanelJogButtonAxisX.Size = new Size((m_Configuration.ButtonSize.Height + 20) * 3, (m_Configuration.ButtonSize.Height + 25) * this.flowLayoutPanelJogButtonAxisX.Controls.Count);
            }
            if (this.flowLayoutPanelJogButtonAxisY.Controls.Count > 0)
            {
                //this.flowLayoutPanelJogButtonAxisY.Size = new Size((m_Configuration.ButtonSize.Height + 30) * this.flowLayoutPanelJogButtonAxisY.Controls.Count, (m_Configuration.ButtonSize.Height + 20) * 3);
            }
            if (this.flowLayoutPanelJogButtonComb_UVW.Controls.Count == 0)
            {
                this.flowLayoutPanelJogButtonComb_UVW.Visible = false;
                this.flowLayoutPanelJogButtonAxisX.Location = new Point(this.radioButtonContinuous.Location.X, this.radioButtonContinuous.Location.Y + this.radioButtonContinuous.Height + 10);
                this.flowLayoutPanelJogButtonAxisY.Location = new Point(this.flowLayoutPanelJogButtonAxisX.Location.X + this.flowLayoutPanelJogButtonAxisX.Width, this.flowLayoutPanelJogButtonComb_UVW.Location.Y);
            }
            if (this.flowLayoutPanelJogButtonComb_XY.Controls.Count == 0)
            {
                this.flowLayoutPanelJogButtonComb_XY.Visible = false;
                //this.flowLayoutPanelJogButtonAxisX.Location = new Point(this.radioButtonContinuous.Location.X, this.radioButtonContinuous.Location.Y + this.radioButtonContinuous.Height + 10);
                //this.flowLayoutPanelJogButtonAxisY.Location = new Point(this.flowLayoutPanelJogButtonAxisX.Location.X + this.flowLayoutPanelJogButtonAxisX.Width, this.flowLayoutPanelJogButtonComb_UVW.Location.Y);
            }
            if (this.flowLayoutPanelJogButtonAxisX.Controls.Count == 0)
            {
                this.flowLayoutPanelJogButtonAxisX.Visible = false;
                //this.flowLayoutPanelJogButtonAxisY.Location = new Point(this.flowLayoutPanelJogButtonComb_UVW.Location.X + this.flowLayoutPanelJogButtonComb_UVW.Width + this.flowLayoutPanelJogButtonComb_XY.Width + 45, this.flowLayoutPanelJogButtonComb_UVW.Location.Y);
                this.flowLayoutPanelJogButtonAxisY.Location = new Point(this.flowLayoutPanelJogButtonComb_UVW.Location.X + this.flowLayoutPanelJogButtonComb_UVW.Width + 20, this.flowLayoutPanelJogButtonComb_UVW.Location.Y);
                this.flowLayoutPanelJogButtonAxisY.Size = new Size(160, this.flowLayoutPanelJogButtonAxisY.Size.Height);
            }
            if (this.flowLayoutPanelJogButtonComb_UVW.Controls.Count == 0 && this.flowLayoutPanelJogButtonAxisX.Controls.Count == 0)
            {
                this.flowLayoutPanelJogButtonComb_UVW.Visible = false;
                this.flowLayoutPanelJogButtonAxisX.Visible = false;
                this.flowLayoutPanelJogButtonAxisY.Location = new Point(this.radioButtonContinuous.Location.X, this.radioButtonContinuous.Location.Y + this.radioButtonContinuous.Height + 10);
            }
            if (this.flowLayoutPanelJogButtonAxisY.Controls.Count == 0)
            {
                this.flowLayoutPanelJogButtonAxisY.Visible = false;
            }
            this.radioButtonStep.Checked = true;

            #endregion
        }


        #region InitdataGridViewParameterColumns
        private void InitdataGridViewParameterColumns()
        {
            dataGridViewJogControl.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewJogControl.Columns.Clear();
            dataGridViewJogControl.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Axis";
                column.Name = "Axis";
                column.ReadOnly = true;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Resizable = DataGridViewTriState.False;
                dataGridViewJogControl.RowTemplate.Height = 21;             //  높이
                dataGridViewJogControl.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "AbsolutePos";
                column.Name = "AbsolutePos";
                column.ReadOnly = true;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Resizable = DataGridViewTriState.False;
                dataGridViewJogControl.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "RelativePos";
                column.Name = "RelativePos";
                column.ReadOnly = true;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Resizable = DataGridViewTriState.False;
                dataGridViewJogControl.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Step";
                column.Name = "Step";
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Resizable = DataGridViewTriState.False;
                dataGridViewJogControl.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Velocity";
                column.Name = "Velocity";
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.Resizable = DataGridViewTriState.False;
                dataGridViewJogControl.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Resizable = DataGridViewTriState.False;
                dataGridViewJogControl.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.Resizable = DataGridViewTriState.False;
                dataGridViewJogControl.Columns.Add(column);
            }
        }
        #endregion

        #region UpdateDataGridViewValue
        public void UpdateDataGridViewValue()
        {
            dataGridViewJogControl.Rows.Clear();
            for (int i = 0; i < m_axis.Count; i++)
            {
                DataGridViewButtonCell buttonCellSetZero = new DataGridViewButtonCell();
                buttonCellSetZero.Value = "Set Zero";

                DataGridViewButtonCell buttonCellGoHome = new DataGridViewButtonCell();
                buttonCellGoHome.Value = "Go Home";
                dataGridViewJogControl.Rows.Add();

                if (m_axis[i] != null)
                {
                    double dPos = 0;
                    //m_axis[i].GetActualPosition(ref dPos);
                    //if (m_axis[i].Configuration.BoardType == MotionBoardType.Ajin)                                  //  2023. 07. 18.  SCH : SLD-100 에서는 주석
                        dPos = MC_Func.MC_GetEncPos(m_axis[i].No);
                    //else if (m_axis[i].Configuration.BoardType == MotionBoardType.ACS)                              //  2023. 07. 18.  SCH : SLD-100 에서는 주석
                        //dPos = ACSSPiiPlusMotionBoard.Api.GetRPosition((Axis)m_axis[i].No);                         //  2023. 07. 18.  SCH : SLD-100 에서는 주석
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.AbsolutePos, i].Value = dPos;
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.Axis, i].Value = m_axis[i].Name;
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.RelativePos, i].Value = dPos;
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, i].Value = Step[m_axis[i]];
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.Velocity, i].Value = Velocity[m_axis[i]];
                    this.dataGridViewJogControl.Rows[i].Cells[5] = buttonCellSetZero;
                    this.dataGridViewJogControl.Rows[i].Cells[6] = buttonCellGoHome;
                }

            }
        }
        #endregion

        #region daragridview_buttonCellClick
        private void daragridview_buttonCellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewJogControl.SelectedCells[0].RowIndex >= 0)
            {
                m_nindex = dataGridViewJogControl.SelectedCells[0].RowIndex;
                if (m_nindex >= 0)
                {
                    MotionAxis ax = m_axis[m_nindex];
                    if (e.ColumnIndex == (int)JogControlAxisInfo.SetZero)
                    {
                        double dPos = 0;
                        //ax.GetActualPosition(ref dPos);
                        if (ax.Configuration.BoardType == MotionBoardType.Ajin)
                            dPos = MC_Func.MC_GetEncPos(ax.No);
                        else if (ax.Configuration.BoardType == MotionBoardType.ACS)
                            dPos = ACSSPiiPlusMotionBoard.Api.GetRPosition((Axis)ax.No);
                        if (m_relativeZeroPositions.ContainsKey(ax))
                        {
                            m_relativeZeroPositions[ax] = dPos;
                        }
                        else
                        {
                            m_relativeZeroPositions.Add(ax, dPos);
                        }

                    }
                    else if (e.ColumnIndex == (int)JogControlAxisInfo.GoHome)
                    {
                        //ax.MovePosition(0);
                        //MC_Func.MC_MovePosition(waferProbeAlign.waferProbeAlignParameter.stWaferProbeAlignPosParam.nAxis[(int)WaferProbeAlign.nAxis.IZ], 
                        //                        ax.Velocity, ax.Configuration.Velocity, ax.Configuration.Acceleration, ax.Configuration.Deceleration);
                        //MC_Func.MC_MovePosition((int)WaferProbeAlignParameter.AxisAjinEnum.EZ,
                        //                        ax.Velocity, ax.Configuration.Velocity, ax.Configuration.Acceleration, ax.Configuration.Deceleration);
                    }
                }
            }
        }
        #endregion

        #region AddJogButton
        public void AddJogButton()
        {
            flowLayoutPanelJogButtonAxisY.Controls.Clear();
            flowLayoutPanelJogButtonAxisX.Controls.Clear();
            flowLayoutPanelJogButtonComb_UVW.Controls.Clear();
            flowLayoutPanelJogButtonComb_XY.Controls.Clear();

            foreach (MotionAxis axe in m_axis)
            {
                if (axe == null)
                {
                    continue;
                }
                if (axe.Configuration.DisplayAxisType == DisplayAxisType.Vertical)
                {
                    m_AxisYRevision = new AxisYRevision();
                    m_AxisYRevision.Location = new Point(0, 0);
                    m_AxisYRevision.AxisValue = axe;
                    m_AxisYRevision.AxisList.Add(axe);
                    m_AxisYRevision.SetButtonName(axe.Name);
                    flowLayoutPanelJogButtonAxisY.Controls.Add(m_AxisYRevision);
                    m_AxisYRevision.JogButtonClick += JogbuttonEvent;
                    m_AxisYRevision.JogButtonDown += JogbuttonDownEvent;
                    m_AxisYRevision.JogButtonUp += JogbuttonUpEvent;
                }
                else if (axe.Configuration.DisplayAxisType == DisplayAxisType.Horizontal)
                {
                    m_AxisXRevision = new AxisXRevision();
                    m_AxisXRevision.Location = new Point(0, 0);
                    m_AxisXRevision.AxisValue = axe;
                    m_AxisXRevision.AxisList.Add(axe);
                    m_AxisXRevision.SetButtonName(axe.Name);
                    flowLayoutPanelJogButtonAxisX.Controls.Add(m_AxisXRevision);
                    m_AxisXRevision.JogButtonClick += JogbuttonEvent;
                    m_AxisXRevision.JogButtonDown += JogbuttonDownEvent;
                    m_AxisXRevision.JogButtonUp += JogbuttonUpEvent;
                }
                else if (axe.Configuration.DisplayAxisType == DisplayAxisType.Theta)
                {
                    m_JogButtonTheta = new JogButtonTheta();
                    m_JogButtonTheta.thetaValue = axe;
                    m_JogButtonTheta.AxisList.Add(axe);
                    m_JogButtonTheta.SetButtonName(axe.Name);
                    flowLayoutPanelJogButtonAxisX.Controls.Add(m_JogButtonTheta);
                    m_JogButtonTheta.JogButtonClick += JogbuttonEvent;
                    m_JogButtonTheta.JogButtonDown += JogbuttonDownEvent;
                    m_JogButtonTheta.JogButtonUp += JogbuttonUpEvent;
                }
                else if (axe.Configuration.DisplayAxisType == DisplayAxisType.CombinationVertical)
                {
                    if (m_JogButtonCombination_XY.VerticalAxis == null)
                    {
                        m_JogButtonCombination_XY.VerticalAxis = axe;
                        m_JogButtonCombination_XY.AxisList.Add(axe);
                    }
                    else
                    {
                        if (axe.Configuration.DisplayAxisType == DisplayAxisType.CombinationVertical)
                        {
                            m_AxisYRevision = new AxisYRevision();
                            m_AxisYRevision.Location = new Point(0, 0);
                            m_AxisYRevision.AxisValue = axe;
                            m_AxisYRevision.AxisList.Add(axe);
                            m_AxisYRevision.SetButtonName(axe.Name);
                            flowLayoutPanelJogButtonAxisY.Controls.Add(m_AxisYRevision);
                            m_AxisYRevision.JogButtonClick += JogbuttonEvent;
                            m_AxisYRevision.JogButtonDown += JogbuttonDownEvent;
                            m_AxisYRevision.JogButtonUp += JogbuttonUpEvent;
                        }
                        else if (axe.Configuration.DisplayAxisType == DisplayAxisType.CombinationHorizontal)
                        {
                            m_AxisXRevision = new AxisXRevision();
                            m_AxisXRevision.Location = new Point(0, 0);
                            m_AxisXRevision.AxisValue = axe;
                            m_AxisXRevision.AxisList.Add(axe);
                            m_AxisXRevision.SetButtonName(axe.Name);
                            flowLayoutPanelJogButtonAxisX.Controls.Add(m_AxisXRevision);
                            m_AxisXRevision.JogButtonClick += JogbuttonEvent;
                            m_AxisXRevision.JogButtonDown += JogbuttonDownEvent;
                            m_AxisXRevision.JogButtonUp += JogbuttonUpEvent;
                        }
                    }
                }
                else if (axe.Configuration.DisplayAxisType == DisplayAxisType.CombinationHorizontal)
                {
                    if (m_JogButtonCombination_XY.HorizontalAxis == null)
                    {
                        m_JogButtonCombination_XY.HorizontalAxis = axe;
                        m_JogButtonCombination_XY.AxisList.Add(axe);
                    }
                    else
                    {
                        if (axe.Configuration.DisplayAxisType == DisplayAxisType.CombinationVertical)
                        {
                            m_AxisYRevision = new AxisYRevision();
                            m_AxisYRevision.Location = new Point(0, 0);
                            m_AxisYRevision.AxisValue = axe;
                            m_AxisYRevision.AxisList.Add(axe);
                            m_AxisYRevision.SetButtonName(axe.Name);
                            flowLayoutPanelJogButtonAxisY.Controls.Add(m_AxisYRevision);
                            m_AxisYRevision.JogButtonClick += JogbuttonEvent;
                            m_AxisYRevision.JogButtonDown += JogbuttonDownEvent;
                            m_AxisYRevision.JogButtonUp += JogbuttonUpEvent;
                        }
                        else if (axe.Configuration.DisplayAxisType == DisplayAxisType.CombinationHorizontal)
                        {
                            m_AxisXRevision = new AxisXRevision();
                            m_AxisXRevision.Location = new Point(0, 0);
                            m_AxisXRevision.AxisValue = axe;
                            m_AxisXRevision.AxisList.Add(axe);
                            m_AxisXRevision.SetButtonName(axe.Name);
                            flowLayoutPanelJogButtonAxisX.Controls.Add(m_AxisXRevision);
                            m_AxisXRevision.JogButtonClick += JogbuttonEvent;
                            m_AxisXRevision.JogButtonDown += JogbuttonDownEvent;
                            m_AxisXRevision.JogButtonUp += JogbuttonUpEvent;
                        }
                    }
                }
                else if (axe.Configuration.DisplayAxisType == DisplayAxisType.UVW_Horizontal)
                {
                    if (m_JogButtonCombination_UVW.HorizontalAxis == null)
                    {
                        m_JogButtonCombination_UVW.HorizontalAxis = axe;
                        m_JogButtonCombination_UVW.AxisList.Add(axe);
                    }
                }
                else if (axe.Configuration.DisplayAxisType == DisplayAxisType.UVW_Vertical)
                {
                    if (m_JogButtonCombination_UVW.VerticalAxis1 == null)
                    {
                        m_JogButtonCombination_UVW.VerticalAxis1 = axe;
                        m_JogButtonCombination_UVW.AxisList.Add(axe);
                    }
                    else if (m_JogButtonCombination_UVW.VerticalAxis2 == null)
                    {
                        m_JogButtonCombination_UVW.VerticalAxis2 = axe;
                        m_JogButtonCombination_UVW.AxisList.Add(axe);
                    }
                }
            }

            m_JogButtonCombination_UVW.JogButtonUVWClick += JogbuttonEvent_UVW;
            m_JogButtonCombination_UVW.JogButtonUVWDown += JogbuttonDownEvent_UVW;
            m_JogButtonCombination_UVW.JogButtonUVWUp += JogbuttonUpEvent_UVW;
            //m_JogButtonCombination_UVW.SetButtonName(m_JogButtonCombination_UVW.HorizontalAxis.Name, m_JogButtonCombination_UVW.VerticalAxis.Name);

            if (flowLayoutPanelJogButtonComb_UVW.Controls != null)
            {
                flowLayoutPanelJogButtonComb_UVW.Controls.Add(m_JogButtonCombination_UVW);
            }

            m_JogButtonCombination_XY.JogButtonXYClick += JogbuttonEvent;
            m_JogButtonCombination_XY.JogButtonXYDown += JogbuttonDownEvent;
            m_JogButtonCombination_XY.JogButtonXYUp += JogbuttonUpEvent;
            //m_JogButtonCombination_XY.SetButtonName(m_JogButtonCombination_XY.HorizontalAxis.Name, m_JogButtonCombination_XY.VerticalAxis.Name);

            if (flowLayoutPanelJogButtonComb_XY.Controls != null)
            {
                flowLayoutPanelJogButtonComb_XY.Controls.Add(m_JogButtonCombination_XY);
            }

            //this.flowLayoutPanelJogButtonComb_XY.Location = new Point(this.flowLayoutPanelJogButtonComb_UVW.Location.X + this.flowLayoutPanelJogButtonComb_UVW.Size.Width + 100, this.flowLayoutPanelJogButtonComb_UVW.Location.Y + 6);
            //this.flowLayoutPanelJogButtonComb_XY.Location = new Point(this.flowLayoutPanelJogButtonComb_UVW.Location.X + this.flowLayoutPanelJogButtonComb_UVW.Size.Width + 70, this.flowLayoutPanelJogButtonComb_UVW.Location.Y + 6);
            this.flowLayoutPanelJogButtonComb_XY.Location = new Point(this.flowLayoutPanelJogButtonComb_UVW.Location.X + this.flowLayoutPanelJogButtonComb_UVW.Size.Width + this.flowLayoutPanelJogButtonAxisY.Size.Width + 88, this.flowLayoutPanelJogButtonComb_UVW.Location.Y + 6); 


            //this.flowLayoutPanelJogButtonAxisY.Location = new Point(this.flowLayoutPanelJogButtonComb_UVW.Location.X + this.flowLayoutPanelJogButtonComb_UVW.Size.Width + 10 + this.flowLayoutPanelJogButtonComb_XY.Size.Width, this.flowLayoutPanelJogButtonComb_UVW.Location.Y + 6);
            //this.flowLayoutPanelJogButtonAxisY.Location = new Point(this.flowLayoutPanelJogButtonComb_UVW.Location.X + this.flowLayoutPanelJogButtonComb_UVW.Size.Width - 70, this.flowLayoutPanelJogButtonComb_UVW.Location.Y + 6);

            //if (m_JogButtonCombination_UVW.HorizontalAxis != null && m_JogButtonCombination_UVW.VerticalAxis != null)
            //{
            //    m_JogButtonCombination_UVW.JogButtonUVWClick += JogbuttonEvent;
            //    m_JogButtonCombination_UVW.JogButtonUVWDown += JogbuttonDownEvent;
            //    m_JogButtonCombination_UVW.JogButtonUVWUp += JogbuttonUpEvent;
            //    m_JogButtonCombination_UVW.SetButtonName(m_JogButtonCombination_UVW.HorizontalAxis.Name, m_JogButtonCombination_UVW.VerticalAxis.Name);

            //    if (flowLayoutPanelJogButtonComb_UVW.Controls != null)
            //    {
            //        flowLayoutPanelJogButtonComb_UVW.Controls.Add(m_JogButtonCombination_UVW);
            //    }
            //}
            //else if (m_JogButtonCombination_UVW.HorizontalAxis != null)
            //{
            //    m_AxisXRevision = new AxisXRevision();
            //    m_AxisXRevision.AxisValue = m_JogButtonCombination_UVW.HorizontalAxis;
            //    m_AxisXRevision.AxisList.Add(m_AxisXRevision.AxisValue);
            //    m_AxisXRevision.SetButtonName(m_JogButtonCombination_UVW.HorizontalAxis.Name);
            //    flowLayoutPanelJogButtonAxisX.Controls.Add(m_AxisXRevision);
            //    m_AxisXRevision.JogButtonClick += JogbuttonEvent;
            //    m_AxisXRevision.JogButtonDown += JogbuttonDownEvent;
            //    m_AxisXRevision.JogButtonUp += JogbuttonUpEvent;
            //    m_JogButtonCombination_UVW.HorizontalAxis = null;
            //}
            //else if (m_JogButtonCombination_UVW.VerticalAxis != null)
            //{
            //    m_AxisYRevision = new AxisYRevision();
            //    m_AxisYRevision.AxisValue = m_JogButtonCombination_UVW.VerticalAxis;
            //    m_AxisYRevision.AxisList.Add(m_AxisYRevision.AxisValue);
            //    m_AxisYRevision.SetButtonName(m_JogButtonCombination_UVW.VerticalAxis.Name);
            //    flowLayoutPanelJogButtonAxisY.Controls.Add(m_AxisYRevision);
            //    m_AxisYRevision.JogButtonClick += JogbuttonEvent;
            //    m_AxisYRevision.JogButtonDown += JogbuttonDownEvent;
            //    m_AxisYRevision.JogButtonUp += JogbuttonUpEvent;
            //}

            //if (m_JogButtonCombination_XY.HorizontalAxis != null && m_JogButtonCombination_XY.VerticalAxis != null)
            //{
            //    m_JogButtonCombination_XY.JogButtonXYClick += JogbuttonEvent;
            //    m_JogButtonCombination_XY.JogButtonXYDown += JogbuttonDownEvent;
            //    m_JogButtonCombination_XY.JogButtonXYUp += JogbuttonUpEvent;
            //    m_JogButtonCombination_XY.SetButtonName(m_JogButtonCombination_XY.HorizontalAxis.Name, m_JogButtonCombination_XY.VerticalAxis.Name);

            //    if (flowLayoutPanelJogButtonComb_XY.Controls != null)
            //    {
            //        flowLayoutPanelJogButtonComb_XY.Controls.Add(m_JogButtonCombination_XY);
            //    }
            //}
            //else if (m_JogButtonCombination_XY.HorizontalAxis != null)
            //{
            //    m_AxisXRevision = new AxisXRevision();
            //    m_AxisXRevision.AxisValue = m_JogButtonCombination_XY.HorizontalAxis;
            //    m_AxisXRevision.AxisList.Add(m_AxisXRevision.AxisValue);
            //    m_AxisXRevision.SetButtonName(m_JogButtonCombination_XY.HorizontalAxis.Name);
            //    flowLayoutPanelJogButtonAxisX.Controls.Add(m_AxisXRevision);
            //    m_AxisXRevision.JogButtonClick += JogbuttonEvent;
            //    m_AxisXRevision.JogButtonDown += JogbuttonDownEvent;
            //    m_AxisXRevision.JogButtonUp += JogbuttonUpEvent;
            //    m_JogButtonCombination_XY.HorizontalAxis = null;
            //}
            //else if (m_JogButtonCombination_XY.VerticalAxis != null)
            //{
            //    m_AxisYRevision = new AxisYRevision();
            //    m_AxisYRevision.AxisValue = m_JogButtonCombination_XY.VerticalAxis;
            //    m_AxisYRevision.AxisList.Add(m_AxisYRevision.AxisValue);
            //    m_AxisYRevision.SetButtonName(m_JogButtonCombination_XY.VerticalAxis.Name);
            //    flowLayoutPanelJogButtonAxisY.Controls.Add(m_AxisYRevision);
            //    m_AxisYRevision.JogButtonClick += JogbuttonEvent;
            //    m_AxisYRevision.JogButtonDown += JogbuttonDownEvent;
            //    m_AxisYRevision.JogButtonUp += JogbuttonUpEvent;
            //}

        }
        #endregion

        #region MouseClickEvent

        /// <summary>
        /// 기존 것 - 시작
        /// </summary>
        public void JogbuttonEvent(JogControlButtonList type, List<MotionAxis> axisList)
        {
            switch (type)
            {
                case JogControlButtonList.buttonUp:
                case JogControlButtonList.buttonCW:
                case JogControlButtonList.buttonRight:
                    buttonUp_Click(axisList);
                    break;
                case JogControlButtonList.combButtonUp:
                case JogControlButtonList.combButtonRight:
                    combButtonUp_Click(axisList, type);
                    break;
                case JogControlButtonList.buttonDown:
                case JogControlButtonList.buttonCCW:
                case JogControlButtonList.buttonLeft:
                    buttonDown_Click(axisList);
                    break;
                case JogControlButtonList.combButtonDown:
                case JogControlButtonList.combButtonLeft:
                    combButtonDown_Click(axisList, type);
                    break;
                case JogControlButtonList.buttonAxisXUpYUp:
                case JogControlButtonList.buttonAxisXUpYDown:
                case JogControlButtonList.buttonAxisXDownYUp:
                case JogControlButtonList.buttonAxisXDownYDown:
                    combbutton_Click(axisList, type);
                    break;

            }
        }        

        private void combButtonUp_Click(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonStep.Checked == true)
            {
                double x = Step[axisList[0]];           //  StageX
                double y = Step[axisList[1]];           //  StageY
                double lfTargetPos = 0.0;
                int nDirection = 1;
                double dVelocity = 0;
                int axisNo = -1;

                switch (type)
                {
                    //case JogControlButtonList.combButtonRight:
                    case JogControlButtonList.combButtonUp:
                        dVelocity = Velocity[axisList[1]];
                        if (axisList[1].Direction == MotionDirection.Backward)
                        //if (axisList[1].Direction == MotionDirection.Forward)
                            nDirection = -1;
                        //axisList[0].MoveDistance(x * nDirection, dVelocity, dVelocity * 5, dVelocity * 5);

                        if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            //axisNo = Int32.Parse(axisList[0].Description);
                            //MC_Func.MC_MoveRelPosition(axisList[1].No, y * (double)nDirection, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);       //  SLO-300
                            axisNo = Int32.Parse(axisList[1].Description);
                            //MC_Func.MC_MoveRelPosition(axisList[1].No, y * (double)nDirection, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);       //  SLO-300
                            MC_Func.MC_MoveRelPosition(axisNo, y * (double)nDirection, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                        }
                        else if (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[1].Description);
                                //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[0].No);
                                lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                                lfTargetPos += y * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.ToPoint(0,                     //  '0' - Absolute position
                                                    (Axis)axisNo,
                                                    lfTargetPos);                       //  Target position
                            }
                        }
                        break;

                    //case JogControlButtonList.combButtonUp:
                    case JogControlButtonList.combButtonRight:
                        dVelocity = Velocity[axisList[0]];
                        if (axisList[0].Direction == MotionDirection.Backward)
                        //if (axisList[0].Direction == MotionDirection.Forward)
                            nDirection = -1;
                        //axisList[1].MoveDistance(y * nDirection, dVelocity, dVelocity * 5, dVelocity * 5);

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            //axisNo = Int32.Parse(axisList[1].Description);
                            axisNo = Int32.Parse(axisList[0].Description);
                            //MC_Func.MC_MoveRelPosition(axisList[0].No, x * (double)nDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                            MC_Func.MC_MoveRelPosition(axisNo, x * (double)nDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[0].Configuration.Description);
                                //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                                lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                                lfTargetPos += x * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.ToPoint(MotionFlags.ACSC_NONE,                                   //  '0' - Absolute position
                                                    (Axis)axisNo,
                                                    lfTargetPos);                       //  Target position
                            }
                        }
                        break;
                }
            }
        }

        private void combButtonDown_Click(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonStep.Checked == true)
            {
                double x = Step[axisList[0]];
                double y = Step[axisList[1]];
                double lfTargetPos = 0.0;
                double dVelocity = 0;
                int nDirection = -1;
                int axisNo = -1;

                switch (type)
                {
                    //case JogControlButtonList.combButtonLeft:
                    case JogControlButtonList.combButtonDown:
                        dVelocity = Velocity[axisList[1]];
                        if (axisList[1].Direction == MotionDirection.Backward)
                        //if (axisList[1].Direction == MotionDirection.Forward)
                            nDirection = 1;
                        //axisList[0].MoveDistance(x * nDirection, dVelocity, dVelocity * 5, dVelocity * 5);

                        if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            //axisNo = Int32.Parse(axisList[0].Description);
                            axisNo = Int32.Parse(axisList[1].Description);
                            //MC_Func.MC_MoveRelPosition(axisList[1].No, y * (double)nDirection, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                            MC_Func.MC_MoveRelPosition(axisNo, y * (double)nDirection, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                        }
                        else if (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[1].Description);
                                //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[0].No);
                                lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                                lfTargetPos += y * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                                                    (Axis)axisNo,
                                                    lfTargetPos);                       //  Target position
                            }
                        }
                        break;

                    //case JogControlButtonList.combButtonDown:
                    case JogControlButtonList.combButtonLeft:
                        dVelocity = Velocity[axisList[0]];
                        if (axisList[0].Direction == MotionDirection.Backward)
                        //if (axisList[0].Direction == MotionDirection.Forward)
                            nDirection = 1;
                        //axisList[1].MoveDistance(y * nDirection, dVelocity, dVelocity * 5, dVelocity * 5);

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            //axisNo = Int32.Parse(axisList[1].Description);
                            axisNo = Int32.Parse(axisList[0].Description);
                            //MC_Func.MC_MoveRelPosition(axisList[0].No, x * (double)nDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                            MC_Func.MC_MoveRelPosition(axisNo, x * (double)nDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[0].Description);
                                //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                                lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                                lfTargetPos += x * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                                                    (Axis)axisNo,
                                                    lfTargetPos);                       //  Target position
                            }
                        }
                        break;
                }
            }
        }

        private void buttonUp_Click(List<MotionAxis> axisList)
        {
            double lfTargetPos = 0.0;
            int nDirection = 1;
            int axisNo = -1;

            if (radioButtonStep.Checked == true)
            {
                if (axisList != null)
                {
                    double dVelocity = 0;
                    foreach (MotionAxis axis in axisList)
                    {
                        //if (axis.Direction == MotionDirection.Backward)
                        if (axis.Direction == MotionDirection.Forward)
                            nDirection = -1;

                        dVelocity = Velocity[axis];
                        //axis.MoveDistance(Step[axis] * nDirection, dVelocity, dVelocity * 5, dVelocity * 5); //참고 Step Move

                        if (axis.Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            //axisNo = Int32.Parse(axis.Description);
                            axisNo = Int32.Parse(axis.Description);

                            if (axisNo == (int)WaferProbeAlignParameter.AxisAjinEnum.VZ)
                            {
                                nDirection *= -1;
                            }

                            //MC_Func.MC_MoveRelPosition(axis.No, Step[axis] * (double)nDirection, axis.Configuration.Velocity, axis.Configuration.Acceleration, axis.Configuration.Deceleration);
                            MC_Func.MC_MoveRelPosition(axisNo, Step[axis] * (double)nDirection, axis.Configuration.Velocity, axis.Configuration.Acceleration, axis.Configuration.Deceleration);
                        }
                        else if (axis.Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axis.Description);
                                //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axis.No);
                                lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                                lfTargetPos += Step[axis] * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.ToPoint(0,                     //  '0' - Absolute position
                                                    (Axis)axisNo,                      //  Axis number
                                                    lfTargetPos);                       //  Target position
                            }
                        }
                    }
                }
            }
        }

        private void buttonDown_Click(List<MotionAxis> axisList)
        {
            double lfTargetPos = 0.0;
            int nDirection = -1;
            int axisNo = -1;

            if (radioButtonStep.Checked == true)
            {
                if (axisList != null)
                {
                    double dVelocity = 0;
                    foreach (MotionAxis axis in axisList)
                    {
                        //if (axis.Direction == MotionDirection.Backward)
                        if (axis.Direction == MotionDirection.Forward)
                            nDirection = 1;

                        dVelocity = Velocity[axis];
                        //axis.MoveDistance(Step[axis] * nDirection, dVelocity, dVelocity * 5, dVelocity * 5);

                        if (axis.Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            //axisNo = Int32.Parse(axis.Description);
                            axisNo = Int32.Parse(axis.Description);

                            if (axisNo == (int)WaferProbeAlignParameter.AxisAjinEnum.VZ)
                            {
                                nDirection *= -1;
                            }

                            //MC_Func.MC_MoveRelPosition(axis.No, Step[axis] * (double)nDirection, axis.Configuration.Velocity, axis.Configuration.Acceleration, axis.Configuration.Deceleration);
                            MC_Func.MC_MoveRelPosition(axisNo, Step[axis] * (double)nDirection, axis.Configuration.Velocity, axis.Configuration.Acceleration, axis.Configuration.Deceleration);
                        }
                        else if (axis.Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axis.Description);
                                //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axis.No);
                                lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                                lfTargetPos += Step[axis] * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.ToPoint(0,                     //  '0' - Absolute position
                                                    (Axis)axisNo,                      //  Axis number
                                                    lfTargetPos);                       //  Target position
                            }
                        }
                    }
                }
            }
        }

        private void combbutton_Click(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonStep.Checked == true)
            {
                double lfTargetPos1 = 0.0;
                double lfTargetPos2 = 0.0;
                double x = Step[axisList[0]];
                double y = Step[axisList[1]];
                int nFirstDirection = 1;
                int nSecondDirection = 1;
                double dVelocity1 = 0;
                double dVelocity2 = 0;
                int axisNo1 = -1;
                int axisNo2 = -1;

                if (axisList != null)
                {
                    if (type == JogControlButtonList.buttonAxisXUpYDown)
                    {
                        y = y * -1;
                        x = x * -1;
                    }
                    else if (type == JogControlButtonList.buttonAxisXDownYUp)
                    {
                        //x = x * -1;
                        //y = y * -1;
                    }
                    else if (type == JogControlButtonList.buttonAxisXDownYDown)
                    {
                        //x = x * -1;
                        y = y * -1;
                    }
                    else
                    {
                        x = x * -1;
                    }

                    //if (axisList[0].Direction == MotionDirection.Backward)
                    if (axisList[0].Direction == MotionDirection.Forward)
                        nFirstDirection = -1;
                    if (axisList[1].Direction == MotionDirection.Backward)
                    //if (axisList[1].Direction == MotionDirection.Forward)
                        nSecondDirection = -1;

                    dVelocity1 = Velocity[axisList[0]];
                    dVelocity2 = Velocity[axisList[1]];

                    //axisList[0].MoveDistance(x * nFirstDirection, dVelocity, dVelocity * 5, dVelocity * 5);
                    //axisList[1].MoveDistance(y * nSecondDirection, dVelocity, dVelocity * 5, dVelocity * 5);

                    if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                    {
                        //axisNo1 = Int32.Parse(axisList[0].Description);
                        axisNo1 = Int32.Parse(axisList[0].Description);
                        //MC_Func.MC_MoveRelPosition(axisList[0].No, x * (double)nFirstDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        MC_Func.MC_MoveRelPosition(axisNo1, x * (double)nFirstDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                    }
                    else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                    {
                        if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                        {
                            axisNo1 = Int32.Parse(axisList[0].Description);
                            //lfTargetPos2 = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                            lfTargetPos1 = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo1);
                            if (axisList[0].Name == "StageY")
                                lfTargetPos1 += y * (double)nFirstDirection;
                            else if (axisList[0].Name == "StageX")
                                lfTargetPos1 += x * (double)nFirstDirection;

                            ACSSPiiPlusMotionBoard.Api.ToPoint(0,                     //  '0' - Absolute position
                                                (Axis)axisNo1,               //  Axis number
                                                lfTargetPos1);                      //  Target position
                        }
                    }

                    if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                    {
                        //axisNo2 = Int32.Parse(axisList[1].Description);
                        axisNo2 = Int32.Parse(axisList[1].Description);
                        //MC_Func.MC_MoveRelPosition(axisList[1].No, y * (double)nSecondDirection, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                        MC_Func.MC_MoveRelPosition(axisNo2, y * (double)nSecondDirection, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                    }
                    else if (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS)
                    {
                        if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                        {
                            axisNo2 = Int32.Parse(axisList[1].Description);
                            //lfTargetPos2 = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                            lfTargetPos2 = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo2);
                            if (axisList[1].Name == "StageY")
                                lfTargetPos2 += y * (double)nSecondDirection;
                            else if (axisList[1].Name == "StageX")
                                lfTargetPos1 += x * (double)nSecondDirection;

                            ACSSPiiPlusMotionBoard.Api.ToPoint(0,                     //  '0' - Absolute position
                                                (Axis)axisNo2,               //  Axis number
                                                lfTargetPos2);                      //  Target position
                        }
                    }

                }
            }
        }
        /// <summary>
        /// 기존 것 - 끝
        /// </summary>


        /// <summary>
        /// UVW 관련 - 시작
        /// </summary>
        public void JogbuttonEvent_UVW(JogControlButtonList type, List<MotionAxis> axisList)
        {
            switch (type)
            {
                case JogControlButtonList.buttonUp:
                case JogControlButtonList.buttonCW:
                case JogControlButtonList.buttonRight:
                    buttonUp_UVW_Click(axisList, type);                             //  UVW 이동 - CW 회전(UVW 복합 구동)
                    break;
                case JogControlButtonList.combButtonUp:
                case JogControlButtonList.combButtonRight:
                    combButtonUp_UVW_Click(axisList, type);                         //  UVW 이동 - 뒤쪽(VW 복합 구동), 오른쪽(U 단독 구동)
                    break;
                case JogControlButtonList.buttonDown:
                case JogControlButtonList.buttonCCW:
                case JogControlButtonList.buttonLeft:
                    buttonDown_UVW_Click(axisList, type);                           //  UVW 이동 - CCW 회전(UVW 복합 구동)
                    break;
                case JogControlButtonList.combButtonDown:
                case JogControlButtonList.combButtonLeft:
                    combButtonDown_UVW_Click(axisList, type);                       //  UVW 이동 - 앞쪽(VW 복합 구동), 왼쪽(U 단독 구동)
                    break;
                case JogControlButtonList.buttonAxisXUpYUp:
                case JogControlButtonList.buttonAxisXUpYDown:
                case JogControlButtonList.buttonAxisXDownYUp:
                case JogControlButtonList.buttonAxisXDownYDown:
                    combbutton_UVW_Click(axisList, type);                           //  UVW 이동 - 사선 방향(UVW 복합 구동)
                    break;
            }
        }

        private void combButtonUp_UVW_Click(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonStep.Checked == true)
            {
                double u = Step[axisList[0]];
                double v = Step[axisList[1]];
                double w = Step[axisList[2]];
                double lfTargetPos = 0.0;
                double dVelocity = 0;
                int nDirection_U = 1;
                int nDirection_V = 1;
                int nDirection_W = 1;
                int axisNo_U = -1;
                int axisNo_V = -1;
                int axisNo_W = -1;

                switch (type)
                {
                    case JogControlButtonList.combButtonUp:                                                             //  UVW 뒤쪽 이동 (VW 축)
                        dVelocity = Velocity[axisList[1]];

                        if (axisList[1].Direction == MotionDirection.Backward)
                            nDirection_V = -1;

                        if (axisList[2].Direction == MotionDirection.Backward)
                            nDirection_W = -1;

                        if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo_V = Int32.Parse(axisList[1].Description);
                            axisNo_W = Int32.Parse(axisList[2].Description);

                            //  뒤쪽으로 이동 (U0, V+, W+)
                            v = v;
                            w = v;

                            MC_Func.MC_MoveRelPosition(axisNo_V, v * (double)nDirection_V, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                            MC_Func.MC_MoveRelPosition(axisNo_W, w * (double)nDirection_W, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);         //  v 축과 동일한 속도로 구동하기 위해 v 축 속도를 사용한다.
                        }
                        else if (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            //{
                            //    axisNo = Int32.Parse(axisList[1].Description);
                            //    //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[0].No);
                            //    lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                            //    lfTargetPos += y * (double)nDirection;

                            //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,                     //  '0' - Absolute position
                            //                        (Axis)axisNo,
                            //                        lfTargetPos);                       //  Target position
                            //}
                        }
                        break;


                    case JogControlButtonList.combButtonRight:                                                          //  UVW 오른쪽 이동 (U 축)
                        dVelocity = Velocity[axisList[0]];

                        if (axisList[0].Direction == MotionDirection.Backward)
                            nDirection_U = 1;

                        //if (axisList[0].Direction == MotionDirection.Forward)
                        //    nDirection_U = -1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo_U = Int32.Parse(axisList[0].Description);

                            //  오른쪽으로 이동 (U+, V0, W0)
                            //v = v;
                            //w = v;

                            MC_Func.MC_MoveRelPosition(axisNo_U, u * (double)nDirection_U, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            //{
                            //    axisNo = Int32.Parse(axisList[0].Configuration.Description);
                            //    //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                            //    lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                            //    lfTargetPos += x * (double)nDirection;

                            //    ACSSPiiPlusMotionBoard.Api.ToPoint(MotionFlags.ACSC_NONE,                                   //  '0' - Absolute position
                            //                        (Axis)axisNo,
                            //                        lfTargetPos);                       //  Target position
                            //}
                        }
                        break;
                }
            }
        }

        private void combButtonDown_UVW_Click(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonStep.Checked == true)
            {
                double u = Step[axisList[0]];
                double v = Step[axisList[1]];
                double w = Step[axisList[2]];
                double lfTargetPos = 0.0;
                double dVelocity = 0;
                int nDirection_U = 1;
                int nDirection_V = 1;
                int nDirection_W = 1;
                int axisNo_U = -1;
                int axisNo_V = -1;
                int axisNo_W = -1;

                switch (type)
                {
                    case JogControlButtonList.combButtonDown:                                                   //  UVW 앞쪽 이동 (VW 축)   
                        dVelocity = Velocity[axisList[1]];

                        if (axisList[1].Direction == MotionDirection.Backward)
                            nDirection_V = -1;

                        if (axisList[2].Direction == MotionDirection.Backward)
                            nDirection_W = -1;

                        if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo_V = Int32.Parse(axisList[1].Description);
                            axisNo_W = Int32.Parse(axisList[2].Description);

                            //  앞쪽으로 이동 (U0, V-, W-)
                            v = v * -1.0;
                            w = v;

                            MC_Func.MC_MoveRelPosition(axisNo_V, v * (double)nDirection_V, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                            MC_Func.MC_MoveRelPosition(axisNo_W, w * (double)nDirection_W, axisList[1].Configuration.Velocity, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);         //  v 축과 동일한 속도로 구동하기 위해 v 축 속도를 사용한다.
                        }
                        else if (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            //{
                            //    axisNo = Int32.Parse(axisList[1].Description);
                            //    //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[0].No);
                            //    lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                            //    lfTargetPos += y * (double)nDirection;

                            //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                            //                        (Axis)axisNo,
                            //                        lfTargetPos);                       //  Target position
                            //}
                        }
                        break;


                    case JogControlButtonList.combButtonLeft:                                                   //  UVW 왼쪽 이동 (U 축)
                        dVelocity = Velocity[axisList[0]];

                        if (axisList[0].Direction == MotionDirection.Forward)
                            nDirection_U = 1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo_U = Int32.Parse(axisList[0].Description);

                            //  왼쪽으로 이동 (U-, V0, W0)
                            u = u * -1.0;

                            MC_Func.MC_MoveRelPosition(axisNo_U, u * (double)nDirection_U, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            //{
                            //    axisNo = Int32.Parse(axisList[0].Description);
                            //    //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                            //    lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                            //    lfTargetPos += x * (double)nDirection;

                            //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                            //                        (Axis)axisNo,
                            //                        lfTargetPos);                       //  Target position
                            //}
                        }
                        break;
                }
            }
        }

        private void buttonUp_UVW_Click(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonStep.Checked == true)
            {
                double u = Step[axisList[0]];
                double v = Step[axisList[1]];
                double w = Step[axisList[2]];
                double lfTargetPos = 0.0;
                double dVelocity = 0;
                int nDirection_U = 1;
                int nDirection_V = 1;
                int nDirection_W = 1;
                int axisNo_U = -1;
                int axisNo_V = -1;
                int axisNo_W = -1;

                switch (type)
                {
                    case JogControlButtonList.buttonCW:                                                     //  CWA-150SA 에서는 이거만 쓴다.

                        dVelocity = Velocity[axisList[0]];

                        if (axisList[0].Direction == MotionDirection.Backward)
                            nDirection_U = -1;

                        if (axisList[1].Direction == MotionDirection.Backward)
                            nDirection_V = -1;

                        if (axisList[2].Direction == MotionDirection.Backward)
                            nDirection_W = -1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo_U = Int32.Parse(axisList[0].Description);
                            axisNo_V = Int32.Parse(axisList[1].Description);
                            axisNo_W = Int32.Parse(axisList[2].Description);

                            //  시계방향 회전 (U+, V+, W-)
                            v = u;
                            w = u * -1.0;

                            MC_Func.MC_MoveRelPosition(axisNo_U, u * (double)nDirection_U, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  U
                            MC_Func.MC_MoveRelPosition(axisNo_V, v * (double)nDirection_V, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  V       //  U 축과 동일한 Pitch, 동일한 Speed 로 구동하기 위해 
                            MC_Func.MC_MoveRelPosition(axisNo_W, w * (double)nDirection_W, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  W       //  U 축과 동일한 Pitch, 동일한 Speed 로 구동하기 위해 
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            //{
                            //    axisNo = Int32.Parse(axisList[1].Description);
                            //    //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[0].No);
                            //    lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                            //    lfTargetPos += u * (double)nDirection;

                            //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                            //                        (Axis)axisNo,
                            //                        lfTargetPos);                       //  Target position
                            //}
                        }
                        break;


                    //case JogControlButtonList.combButtonLeft:
                    //    dVelocity = Velocity[axisList[0]];
                    //    //if (axisList[0].Direction == MotionDirection.Backward)
                    //    if (axisList[0].Direction == MotionDirection.Forward)
                    //        nDirection = 1;
                    //    //axisList[1].MoveDistance(y * nDirection, dVelocity, dVelocity * 5, dVelocity * 5);

                    //    if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                    //    {
                    //        //axisNo = Int32.Parse(axisList[1].Description);
                    //        axisNo = Int32.Parse(axisList[0].Description);
                    //        //MC_Func.MC_MoveRelPosition(axisList[0].No, x * (double)nDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                    //        MC_Func.MC_MoveRelPosition(axisNo, x * (double)nDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                    //    }
                    //    else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                    //    {
                    //        if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //        {
                    //            axisNo = Int32.Parse(axisList[0].Description);
                    //            //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                    //            lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                    //            lfTargetPos += x * (double)nDirection;

                    //            ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                    //                                (Axis)axisNo,
                    //                                lfTargetPos);                       //  Target position
                    //        }
                    //    }
                    //    break;
                }
            }
        }

        private void buttonDown_UVW_Click(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonStep.Checked == true)
            {
                double u = Step[axisList[0]];
                double v = Step[axisList[1]];
                double w = Step[axisList[2]];
                double lfTargetPos = 0.0;
                double dVelocity = 0;
                int nDirection_U = 1;
                int nDirection_V = 1;
                int nDirection_W = 1;
                int axisNo_U = -1;
                int axisNo_V = -1;
                int axisNo_W = -1;

                switch (type)
                {
                    case JogControlButtonList.buttonCCW:                                                     //  CWA-150SA 에서는 이거만 쓴다.

                        dVelocity = Velocity[axisList[0]];

                        if (axisList[0].Direction == MotionDirection.Backward)
                            nDirection_U = -1;

                        if (axisList[1].Direction == MotionDirection.Backward)
                            nDirection_V = -1;

                        if (axisList[2].Direction == MotionDirection.Backward)
                            nDirection_W = -1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo_U = Int32.Parse(axisList[0].Description);
                            axisNo_V = Int32.Parse(axisList[1].Description);
                            axisNo_W = Int32.Parse(axisList[2].Description);

                            //  반시계방향 회전 (U-, V-, W+)
                            u = u * -1.0;
                            v = u;
                            w = u * -1.0;

                            MC_Func.MC_MoveRelPosition(axisNo_U, u * (double)nDirection_U, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  U
                            MC_Func.MC_MoveRelPosition(axisNo_V, v * (double)nDirection_V, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  V       //  U 축과 동일한 Pitch, 동일한 Speed 로 구동하기 위해 
                            MC_Func.MC_MoveRelPosition(axisNo_W, w * (double)nDirection_W, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  W       //  U 축과 동일한 Pitch, 동일한 Speed 로 구동하기 위해 
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            //{
                            //    axisNo = Int32.Parse(axisList[1].Description);
                            //    //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[0].No);
                            //    lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                            //    lfTargetPos += u * (double)nDirection;

                            //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                            //                        (Axis)axisNo,
                            //                        lfTargetPos);                       //  Target position
                            //}
                        }
                        break;


                    //case JogControlButtonList.combButtonLeft:
                    //    dVelocity = Velocity[axisList[0]];
                    //    //if (axisList[0].Direction == MotionDirection.Backward)
                    //    if (axisList[0].Direction == MotionDirection.Forward)
                    //        nDirection = 1;
                    //    //axisList[1].MoveDistance(y * nDirection, dVelocity, dVelocity * 5, dVelocity * 5);

                    //    if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                    //    {
                    //        //axisNo = Int32.Parse(axisList[1].Description);
                    //        axisNo = Int32.Parse(axisList[0].Description);
                    //        //MC_Func.MC_MoveRelPosition(axisList[0].No, x * (double)nDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                    //        MC_Func.MC_MoveRelPosition(axisNo, x * (double)nDirection, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                    //    }
                    //    else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                    //    {
                    //        if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                    //        {
                    //            axisNo = Int32.Parse(axisList[0].Description);
                    //            //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                    //            lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                    //            lfTargetPos += x * (double)nDirection;

                    //            ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                    //                                (Axis)axisNo,
                    //                                lfTargetPos);                       //  Target position
                    //        }
                    //    }
                    //    break;
                }
            }
        }

        private void combbutton_UVW_Click(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonStep.Checked == true)
            {
                double u = Step[axisList[0]];
                double v = Step[axisList[1]];
                double w = Step[axisList[2]];
                double lfTargetPos = 0.0;
                double dVelocity = 0;
                int nDirection_U = 1;
                int nDirection_V = 1;
                int nDirection_W = 1;
                int axisNo_U = -1;
                int axisNo_V = -1;
                int axisNo_W = -1;

                if (axisList != null)
                {
                    if (type == JogControlButtonList.buttonAxisXUpYDown)                //  Right - Bottom 방향 
                    {
                        v = v * -1.0;
                        w = w * -1.0;
                    }
                    else if (type == JogControlButtonList.buttonAxisXDownYUp)           //  Left - Top 방향
                    {
                        u = u * -1.0;
                    }
                    else if (type == JogControlButtonList.buttonAxisXDownYDown)         //  Left - Bottom 방향
                    {
                        u = u * -1.0;
                        v = v * -1.0;
                        w = w * -1.0;
                    }
                    else                                                                //  Right - Top 방향
                    {
                        
                    }

                    if (axisList[0].Direction == MotionDirection.Backward)
                        nDirection_U = -1;

                    if (axisList[1].Direction == MotionDirection.Backward)
                        nDirection_V = -1;

                    if (axisList[2].Direction == MotionDirection.Backward)
                        nDirection_W = -1;

                    dVelocity = Velocity[axisList[0]];

                    if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                    {
                        axisNo_U = Int32.Parse(axisList[0].Description);
                        axisNo_V = Int32.Parse(axisList[1].Description);
                        axisNo_W = Int32.Parse(axisList[2].Description);

                        MC_Func.MC_MoveRelPosition(axisNo_U, u * (double)nDirection_U, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        MC_Func.MC_MoveRelPosition(axisNo_V, v * (double)nDirection_V, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        MC_Func.MC_MoveRelPosition(axisNo_W, w * (double)nDirection_W, axisList[0].Configuration.Velocity, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                    }
                    else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                    {
                        //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                        //{
                        //    axisNo1 = Int32.Parse(axisList[0].Description);
                        //    //lfTargetPos2 = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                        //    lfTargetPos1 = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo1);
                        //    if (axisList[0].Name == "StageY")
                        //        lfTargetPos1 += y * (double)nFirstDirection;
                        //    else if (axisList[0].Name == "StageX")
                        //        lfTargetPos1 += x * (double)nFirstDirection;

                        //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,                     //  '0' - Absolute position
                        //                        (Axis)axisNo1,               //  Axis number
                        //                        lfTargetPos1);                      //  Target position
                        //}
                    }
                }
            }
        }
        /// <summary>
        /// UVW 관련 - 끝
        /// </summary>
        /// 
        #endregion


        #region MouseDownEvent

        /// <summary>
        /// 기존 것 - 시작
        /// </summary>
        public void JogbuttonDownEvent(JogControlButtonList type, List<MotionAxis> axisList)
        {
            switch (type)
            {
                case JogControlButtonList.buttonUp:
                case JogControlButtonList.buttonRight:
                case JogControlButtonList.buttonCW:
                    buttonUp_Down(axisList);
                    break;
                case JogControlButtonList.combButtonUp:
                case JogControlButtonList.combButtonRight:
                    combButtonUp_Down(axisList, type);
                    break;
                case JogControlButtonList.buttonDown:
                case JogControlButtonList.buttonLeft:
                case JogControlButtonList.buttonCCW:
                    buttonDown_Down(axisList);
                    break;
                case JogControlButtonList.combButtonDown:
                case JogControlButtonList.combButtonLeft:
                    combButtonDown_Down(axisList, type);
                    break;
                case JogControlButtonList.buttonAxisXUpYUp:
                case JogControlButtonList.buttonAxisXUpYDown:
                case JogControlButtonList.buttonAxisXDownYUp:
                case JogControlButtonList.buttonAxisXDownYDown:
                    Combbutton_Down(axisList, type);
                    break;
            }
        }
        private void combButtonUp_Down(List<MotionAxis> axisList, JogControlButtonList type)
        {
            double lfVelocity = 0.0f;
            int nDirection = 1;
            int axisNo = -1;

            if (radioButtonContinuous.Checked == true)
            {
                double x = Velocity[axisList[0]];
                double y = Velocity[axisList[1]];
                switch (type)
                {
                    case JogControlButtonList.combButtonUp:
                        //if (axisList[1].Direction == MotionDirection.Forward)
                        if (axisList[1].Direction == MotionDirection.Backward)
                            nDirection = -1;

                        if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo = Int32.Parse(axisList[1].Description);
                            MC_Func.MC_JogMove(axisNo, y * (double)nDirection, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                        }
                        else if (ACSSPiiPlusMotionBoard.Api.IsConnected && (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS))
                        {
                            axisNo = Int32.Parse(axisList[1].Description);

                            lfVelocity = y * (double)nDirection;

                            ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,             //  Velocity flag
                                            (Axis)axisNo,                                   //  Axis number
                                            lfVelocity);                                            //  Velocity
                        }
                        break;

                    case JogControlButtonList.combButtonRight:
                        //if (axisList[0].Direction == MotionDirection.Forward)
                        if (axisList[0].Direction == MotionDirection.Backward)    
                            nDirection = -1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo = Int32.Parse(axisList[0].Description);
                            MC_Func.MC_JogMove(axisNo, x * (double)nDirection, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[0].Description);

                                lfVelocity = x * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,             //  Velocity flag
                                                (Axis)axisNo,                                   //  Axis number
                                                lfVelocity);                                            //  Velocity
                            }
                        }
                        break;
                }
            }
        }
        private void combButtonDown_Down(List<MotionAxis> axisList, JogControlButtonList type)
        {
            double lfVelocity = 0.0f;
            int nDirection = -1;
            int axisNo = -1;

            if (radioButtonContinuous.Checked == true)
            {
                double x = Velocity[axisList[0]];
                double y = Velocity[axisList[1]];
                switch (type)
                {
                    case JogControlButtonList.combButtonDown:
                        //if (axisList[1].Direction == MotionDirection.Forward)
                        if (axisList[1].Direction == MotionDirection.Backward)
                            nDirection = 1;

                        if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo = Int32.Parse(axisList[1].Description);
                            MC_Func.MC_JogMove(axisNo, y * (double)nDirection, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                        }
                        else if (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[1].Description);

                                lfVelocity = y * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,             //  Velocity flag
                                                (Axis)axisNo,                                   //  Axis number
                                                lfVelocity);                                            //  Velocity
                            }
                        }
                        break;

                    case JogControlButtonList.combButtonLeft:
                        //if (axisList[0].Direction == MotionDirection.Forward)
                        if (axisList[0].Direction == MotionDirection.Backward)
                            nDirection = 1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo = Int32.Parse(axisList[0].Description);
                            MC_Func.MC_JogMove(axisNo, x * (double)nDirection, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[0].Description);

                                lfVelocity = x * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,             //  Velocity flag
                                                (Axis)axisNo,                                   //  Axis number
                                                lfVelocity);                                            //  Velocity
                            }
                        }
                        break;
                }
            }
        }
        private void buttonUp_Down(List<MotionAxis> axisList)
        {
            double lfVelocity = 0.0f;
            int nDirection = 1;
            int axisNo = -1;

            if (radioButtonContinuous.Checked == true)
            {
                if (axisList != null)
                {
                    foreach (MotionAxis axis in axisList)
                    {
                        if (axis.Direction == MotionDirection.Forward)
                        //if (axis.Direction == MotionDirection.Backward)
                            nDirection = -1;

                        if (axis.Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo = Int32.Parse(axis.Description);

                            if (axisNo == (int)WaferProbeAlignParameter.AxisAjinEnum.VZ)
                            {
                                nDirection *= -1;
                            }

                            MC_Func.MC_JogMove(axisNo, Velocity[axis] * (double)nDirection, axis.Configuration.Acceleration, axis.Configuration.Deceleration);
                        }
                        else if (axis.Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axis.Description);

                                lfVelocity = Velocity[axis] * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,         //  Velocity flag
                                            (Axis)axisNo,                                          //  Axis number
                                            lfVelocity);                                            //  Velocity
                            }
                        }
                    }
                }
            }
        }
        private void buttonDown_Down(List<MotionAxis> axisList)
        {
            double lfVelocity = 0.0f;
            int nDirection = -1;
            int axisNo = -1;

            if (radioButtonContinuous.Checked == true)
            {
                if (axisList != null)
                {
                    foreach (MotionAxis axis in axisList)
                    {
                        if (axis.Direction == MotionDirection.Forward)
                        //if (axis.Direction == MotionDirection.Backward)
                            nDirection = 1;

                        if (axis.Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo = Int32.Parse(axis.Description);

                            if (axisNo == (int)WaferProbeAlignParameter.AxisAjinEnum.VZ)
                            {
                                nDirection *= -1;
                            }

                            MC_Func.MC_JogMove(axisNo, Velocity[axis] * (double)nDirection, axis.Configuration.Acceleration, axis.Configuration.Deceleration);
                        }
                        else if (axis.Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axis.Description);

                                lfVelocity = Velocity[axis] * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,         //  Velocity flag
                                            (Axis)axisNo,                                          //  Axis number
                                            lfVelocity);                                            //  Velocity
                            }
                        }
                    }
                }
            }
        }
        private void Combbutton_Down(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonContinuous.Checked == true)
            {
                double x = Velocity[axisList[0]];
                double y = Velocity[axisList[1]];
                int nFirstDirection = 1;
                int nSecondDirection = 1;
                int axisNo1 = -1;
                int axisNo2 = -1;

                if (axisList != null)
                {
                    if (type == JogControlButtonList.buttonAxisXUpYDown)
                    {
                        y = y * -1;
                        x = x * -1;
                    }
                    else if (type == JogControlButtonList.buttonAxisXDownYUp)
                    {
                        //x = x * -1;
                        //y = y * -1;
                    }
                    else if (type == JogControlButtonList.buttonAxisXDownYDown)
                    {
                        //x = x * -1;
                        y = y * -1;
                    }
                    else
                    {
                        x = x * -1;
                    }

                    //if (axisList[0].Direction == MotionDirection.Backward)
                    if (axisList[0].Direction == MotionDirection.Forward)
                        nFirstDirection = -1;

                    //if (axisList[1].Direction == MotionDirection.Forward)
                    if (axisList[1].Direction == MotionDirection.Backward)
                        nSecondDirection = -1;

                    if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                    {
                        axisNo1 = Int32.Parse(axisList[0].Description);
                        MC_Func.MC_JogMove(axisNo1, x * (double)nFirstDirection, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                    }
                    else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                    {
                        if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                        {
                            axisNo1 = Int32.Parse(axisList[0].Description);

                            ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,         //  Velocity flag
                                        (Axis)axisNo1,                                   //  Axis number
                                        x * (double)nFirstDirection);                           //  Velocity
                        }
                    }

                    if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                    {
                        axisNo2 = Int32.Parse(axisList[1].Description);
                        MC_Func.MC_JogMove(axisNo2, y * (double)nSecondDirection, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                    }
                    else if (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS)
                    {
                        if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                        {
                            axisNo2 = Int32.Parse(axisList[1].Description);

                            ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,         //  Velocity flag
                                        (Axis)axisNo2,                                   //  Axis number
                                        y * (double)nSecondDirection);                           //  Velocity
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 기존 것 - 끝
        /// </summary>
        /// 


        /// <summary>
        /// UVW 관련 - 시작
        /// </summary>
        public void JogbuttonDownEvent_UVW(JogControlButtonList type, List<MotionAxis> axisList)
        {
            switch (type)
            {
                case JogControlButtonList.buttonUp:
                case JogControlButtonList.buttonRight:
                case JogControlButtonList.buttonCW:
                    buttonUp_UVW_Down(axisList, type);
                    break;
                case JogControlButtonList.combButtonUp:
                case JogControlButtonList.combButtonRight:
                    combButtonUp_UVW_Down(axisList, type);
                    break;
                case JogControlButtonList.buttonDown:
                case JogControlButtonList.buttonLeft:
                case JogControlButtonList.buttonCCW:
                    buttonDown_UVW_Down(axisList, type);
                    break;
                case JogControlButtonList.combButtonDown:
                case JogControlButtonList.combButtonLeft:
                    combButtonDown_UVW_Down(axisList, type);
                    break;
                case JogControlButtonList.buttonAxisXUpYUp:
                case JogControlButtonList.buttonAxisXUpYDown:
                case JogControlButtonList.buttonAxisXDownYUp:
                case JogControlButtonList.buttonAxisXDownYDown:
                    Combbutton_UVW_Down(axisList, type);
                    break;
            }
        }
        private void combButtonUp_UVW_Down(List<MotionAxis> axisList, JogControlButtonList type)
        {
            double lfVelocity = 0.0f;
            int nDirection = 1;
            int axisNo = -1;
            int axisNoV = -1;
            int axisNoW = -1;

            if (radioButtonContinuous.Checked == true)
            {
                double x = Velocity[axisList[0]];
                double y = Velocity[axisList[1]];
                switch (type)
                {
                    case JogControlButtonList.combButtonUp:
                        if (axisList[1].Direction == MotionDirection.Backward)
                            nDirection = -1;

                        if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNoV = Int32.Parse(axisList[1].Description);
                            axisNoW = Int32.Parse(axisList[2].Description);
                            MC_Func.MC_JogMove(axisNoV, y * (double)nDirection, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                            MC_Func.MC_JogMove(axisNoW, y * (double)nDirection, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                        }
                        else if (ACSSPiiPlusMotionBoard.Api.IsConnected && (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS))
                        {
                            axisNo = Int32.Parse(axisList[1].Description);

                            lfVelocity = y * (double)nDirection;

                            ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,             //  Velocity flag
                                            (Axis)axisNo,                                   //  Axis number
                                            lfVelocity);                                            //  Velocity
                        }
                        break;

                    case JogControlButtonList.combButtonRight:
                        if (axisList[0].Direction == MotionDirection.Backward)
                            nDirection = -1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo = Int32.Parse(axisList[0].Description);
                            MC_Func.MC_JogMove(axisNo, x * (double)nDirection, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[0].Description);

                                lfVelocity = x * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,             //  Velocity flag
                                                (Axis)axisNo,                                   //  Axis number
                                                lfVelocity);                                            //  Velocity
                            }
                        }
                        break;
                }
            }
        }
        private void combButtonDown_UVW_Down(List<MotionAxis> axisList, JogControlButtonList type)
        {
            double lfVelocity = 0.0f;
            int nDirection = -1;
            int axisNo = -1;
            int axisNoV = -1;
            int axisNoW = -1;

            if (radioButtonContinuous.Checked == true)
            {
                double x = Velocity[axisList[0]];
                double y = Velocity[axisList[1]];
                switch (type)
                {
                    case JogControlButtonList.combButtonDown:
                        if (axisList[1].Direction == MotionDirection.Backward)
                            nDirection = 1;

                        if (axisList[1].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNoV = Int32.Parse(axisList[1].Description);
                            axisNoW = Int32.Parse(axisList[2].Description);
                            MC_Func.MC_JogMove(axisNoV, y * (double)nDirection, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                            MC_Func.MC_JogMove(axisNoW, y * (double)nDirection, axisList[1].Configuration.Acceleration, axisList[1].Configuration.Deceleration);
                        }
                        else if (axisList[1].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[1].Description);

                                lfVelocity = y * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,             //  Velocity flag
                                                (Axis)axisNo,                                   //  Axis number
                                                lfVelocity);                                            //  Velocity
                            }
                        }
                        break;

                    case JogControlButtonList.combButtonLeft:
                        if (axisList[0].Direction == MotionDirection.Backward)
                            nDirection = 1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo = Int32.Parse(axisList[0].Description);
                            MC_Func.MC_JogMove(axisNo, x * (double)nDirection, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axisList[0].Description);

                                lfVelocity = x * (double)nDirection;

                                ACSSPiiPlusMotionBoard.Api.Jog(MotionFlags.ACSC_AMF_VELOCITY,             //  Velocity flag
                                                (Axis)axisNo,                                   //  Axis number
                                                lfVelocity);                                            //  Velocity
                            }
                        }
                        break;
                }
            }
        }
        private void buttonUp_UVW_Down(List<MotionAxis> axisList, JogControlButtonList type)
        {
            double u = Velocity[axisList[0]];
            double v = Velocity[axisList[1]];
            double w = Velocity[axisList[2]];
            double lfTargetPos = 0.0;
            double dVelocity = 0;
            int nDirection_U = 1;
            int nDirection_V = 1;
            int nDirection_W = 1;
            int axisNo_U = -1;
            int axisNo_V = -1;
            int axisNo_W = -1;

            if (radioButtonContinuous.Checked == true)
            {
                double x = Velocity[axisList[0]];
                double y = Velocity[axisList[1]];
                switch (type)
                {
                    case JogControlButtonList.buttonUp:
                    case JogControlButtonList.buttonRight:
                        
                        break;


                    case JogControlButtonList.buttonCW:                                                     //  CWA-150SA 에서는 이거만 쓴다.

                        dVelocity = Velocity[axisList[0]];

                        if (axisList[0].Direction == MotionDirection.Backward)
                            nDirection_U = -1;

                        if (axisList[1].Direction == MotionDirection.Backward)
                            nDirection_V = -1;

                        if (axisList[2].Direction == MotionDirection.Backward)
                            nDirection_W = -1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo_U = Int32.Parse(axisList[0].Description);
                            axisNo_V = Int32.Parse(axisList[1].Description);
                            axisNo_W = Int32.Parse(axisList[2].Description);

                            //  시계방향 회전 (U+, V+, W-)
                            v = u;
                            w = u * -1.0;

                            MC_Func.MC_JogMove(axisNo_U, u * (double)nDirection_U, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  U
                            MC_Func.MC_JogMove(axisNo_V, v * (double)nDirection_V, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  V       //  U 축과 동일한 Pitch, 동일한 Speed 로 구동하기 위해 
                            MC_Func.MC_JogMove(axisNo_W, w * (double)nDirection_W, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  W       //  U 축과 동일한 Pitch, 동일한 Speed 로 구동하기 위해 
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            //{
                            //    axisNo = Int32.Parse(axisList[1].Description);
                            //    //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[0].No);
                            //    lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                            //    lfTargetPos += u * (double)nDirection;

                            //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                            //                        (Axis)axisNo,
                            //                        lfTargetPos);                       //  Target position
                            //}
                        }
                        break;
                }
            }
        }
        private void buttonDown_UVW_Down(List<MotionAxis> axisList, JogControlButtonList type)
        {
            double u = Velocity[axisList[0]];
            double v = Velocity[axisList[1]];
            double w = Velocity[axisList[2]];
            double lfTargetPos = 0.0;
            double dVelocity = 0;
            int nDirection_U = 1;
            int nDirection_V = 1;
            int nDirection_W = 1;
            int axisNo_U = -1;
            int axisNo_V = -1;
            int axisNo_W = -1;

            if (radioButtonContinuous.Checked == true)
            {
                double x = Velocity[axisList[0]];
                double y = Velocity[axisList[1]];
                switch (type)
                {
                    case JogControlButtonList.buttonDown:
                    case JogControlButtonList.buttonLeft:
                        
                        break;


                    case JogControlButtonList.buttonCCW:                                                     //  CWA-150SA 에서는 이거만 쓴다.

                        dVelocity = Velocity[axisList[0]];

                        if (axisList[0].Direction == MotionDirection.Forward)
                            nDirection_U = -1;

                        if (axisList[1].Direction == MotionDirection.Forward)
                            nDirection_V = -1;

                        if (axisList[2].Direction == MotionDirection.Forward)
                            nDirection_W = -1;

                        if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            axisNo_U = Int32.Parse(axisList[0].Description);
                            axisNo_V = Int32.Parse(axisList[1].Description);
                            axisNo_W = Int32.Parse(axisList[2].Description);

                            //  시계방향 회전 (U+, V+, W-)
                            v = u;
                            w = u * -1.0;

                            MC_Func.MC_JogMove(axisNo_U, u * (double)nDirection_U, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  U
                            MC_Func.MC_JogMove(axisNo_V, v * (double)nDirection_V, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  V       //  U 축과 동일한 Pitch, 동일한 Speed 로 구동하기 위해 
                            MC_Func.MC_JogMove(axisNo_W, w * (double)nDirection_W, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration); //  W       //  U 축과 동일한 Pitch, 동일한 Speed 로 구동하기 위해 
                        }
                        else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            //{
                            //    axisNo = Int32.Parse(axisList[1].Description);
                            //    //lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[0].No);
                            //    lfTargetPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                            //    lfTargetPos += u * (double)nDirection;

                            //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,                                   //  '0' - Absolute position
                            //                        (Axis)axisNo,
                            //                        lfTargetPos);                       //  Target position
                            //}
                        }
                        break;
                }
            }
        }
        private void Combbutton_UVW_Down(List<MotionAxis> axisList, JogControlButtonList type)
        {
            if (radioButtonContinuous.Checked == true)
            {
                double u = Velocity[axisList[0]];
                double v = Velocity[axisList[1]];
                double w = Velocity[axisList[2]];
                double lfTargetPos = 0.0;
                double dVelocity = 0;
                int nDirection_U = 1;
                int nDirection_V = 1;
                int nDirection_W = 1;
                int axisNo_U = -1;
                int axisNo_V = -1;
                int axisNo_W = -1;

                if (axisList != null)
                {
                    if (type == JogControlButtonList.buttonAxisXUpYDown)                //  Right - Bottom 방향 
                    {
                        v = v * -1.0;
                        w = w * -1.0;
                    }
                    else if (type == JogControlButtonList.buttonAxisXDownYUp)           //  Left - Top 방향
                    {
                        u = u * -1.0;
                    }
                    else if (type == JogControlButtonList.buttonAxisXDownYDown)         //  Left - Bottom 방향
                    {
                        u = u * -1.0;
                        v = v * -1.0;
                        w = w * -1.0;
                    }
                    else                                                                //  Right - Top 방향
                    {

                    }

                    if (axisList[0].Direction == MotionDirection.Backward)
                        nDirection_U = -1;

                    if (axisList[1].Direction == MotionDirection.Backward)
                        nDirection_V = -1;

                    if (axisList[2].Direction == MotionDirection.Backward)
                        nDirection_W = -1;

                    dVelocity = Velocity[axisList[0]];

                    if (axisList[0].Board.Configuration.BoardType == MotionBoardType.Ajin)
                    {
                        axisNo_U = Int32.Parse(axisList[0].Description);
                        axisNo_V = Int32.Parse(axisList[1].Description);
                        axisNo_W = Int32.Parse(axisList[2].Description);

                        MC_Func.MC_JogMove(axisNo_U, u * (double)nDirection_U, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        MC_Func.MC_JogMove(axisNo_V, v * (double)nDirection_V, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                        MC_Func.MC_JogMove(axisNo_W, w * (double)nDirection_W, axisList[0].Configuration.Acceleration, axisList[0].Configuration.Deceleration);
                    }
                    else if (axisList[0].Board.Configuration.BoardType == MotionBoardType.ACS)
                    {
                        //if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                        //{
                        //    axisNo1 = Int32.Parse(axisList[0].Description);
                        //    //lfTargetPos2 = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisList[1].No);
                        //    lfTargetPos1 = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo1);
                        //    if (axisList[0].Name == "StageY")
                        //        lfTargetPos1 += y * (double)nFirstDirection;
                        //    else if (axisList[0].Name == "StageX")
                        //        lfTargetPos1 += x * (double)nFirstDirection;

                        //    ACSSPiiPlusMotionBoard.Api.ToPoint(0,                     //  '0' - Absolute position
                        //                        (Axis)axisNo1,               //  Axis number
                        //                        lfTargetPos1);                      //  Target position
                        //}
                    }
                }
            }
        }
        /// <summary>
        /// UVW 관련 - 끝
        /// </summary>

        #endregion


        #region MouseUpEvent

        /// <summary>
        /// 기존 것 - 시작
        /// </summary>
        public void JogbuttonUpEvent(JogControlButtonList type, List<MotionAxis> axisList)
        {
            switch (type)
            {
                case JogControlButtonList.buttonUp:
                case JogControlButtonList.buttonDown:
                case JogControlButtonList.buttonLeft:
                case JogControlButtonList.buttonRight:
                case JogControlButtonList.buttonCW:
                case JogControlButtonList.buttonCCW:
                case JogControlButtonList.combButtonUp:
                case JogControlButtonList.combButtonDown:
                case JogControlButtonList.combButtonLeft:
                case JogControlButtonList.combButtonRight:
                case JogControlButtonList.buttonAxisXUpYUp:
                case JogControlButtonList.buttonAxisXUpYDown:
                case JogControlButtonList.buttonAxisXDownYUp:
                case JogControlButtonList.buttonAxisXDownYDown:
                    button_Up(axisList);
                    break;
            }
        }

        private void button_Up(List<MotionAxis> axisList)
        {
            int axisNo = -1;

            if (radioButtonContinuous.Checked == true)
            {
                if (axisList != null)
                {
                    foreach (MotionAxis axis in axisList)
                    {
                        //axis.Stop();                        
                        if (axis.Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            //axisNo = Int32.Parse(axis.Description);
                            axisNo = Int32.Parse(axis.Description);
                            //MC_Func.MC_JogStop(axis.No);
                            MC_Func.MC_JogStop(axisNo);
                        }
                        else if (axis.Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axis.Description);
                                //ACSSPiiPlusMotionBoard.Api.Halt((Axis)axis.No);
                                ACSSPiiPlusMotionBoard.Api.Halt((Axis)axisNo);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 기존 것 - 끝
        /// </summary>
        /// 


        /// <summary>
        /// UVW 관련 - 시작
        /// </summary>
        public void JogbuttonUpEvent_UVW(JogControlButtonList type, List<MotionAxis> axisList)
        {
            switch (type)
            {
                case JogControlButtonList.buttonUp:
                case JogControlButtonList.buttonDown:
                case JogControlButtonList.buttonLeft:
                case JogControlButtonList.buttonRight:
                case JogControlButtonList.buttonCW:
                case JogControlButtonList.buttonCCW:
                case JogControlButtonList.combButtonUp:
                case JogControlButtonList.combButtonDown:
                case JogControlButtonList.combButtonLeft:
                case JogControlButtonList.combButtonRight:
                case JogControlButtonList.buttonAxisXUpYUp:
                case JogControlButtonList.buttonAxisXUpYDown:
                case JogControlButtonList.buttonAxisXDownYUp:
                case JogControlButtonList.buttonAxisXDownYDown:
                    button_UVW_Up(axisList);
                    break;
            }
        }

        private void button_UVW_Up(List<MotionAxis> axisList)
        {
            int axisNo = -1;

            if (radioButtonContinuous.Checked == true)
            {
                if (axisList != null)
                {
                    foreach (MotionAxis axis in axisList)
                    {
                        //axis.Stop();                        
                        if (axis.Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            //axisNo = Int32.Parse(axis.Description);
                            axisNo = Int32.Parse(axis.Description);
                            //MC_Func.MC_JogStop(axis.No);
                            MC_Func.MC_JogStop(axisNo);
                        }
                        else if (axis.Board.Configuration.BoardType == MotionBoardType.ACS)
                        {
                            if (ACSSPiiPlusMotionBoard.Api.IsConnected)
                            {
                                axisNo = Int32.Parse(axis.Description);
                                //ACSSPiiPlusMotionBoard.Api.Halt((Axis)axis.No);
                                ACSSPiiPlusMotionBoard.Api.Halt((Axis)axisNo);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// UVW 관련 - 끝
        /// </summary>

        #endregion

        #region timer
        private void startTimer(int intervalTime)
        {
            timerJogControl.Interval = intervalTime;
            timerJogControl.Enabled = true;
        }

        private void TimerFunction(object sender, EventArgs e)
        {
            timerJogControl.Stop();
            resetDataGrid();
            timerJogControl.Start();
        }
        #endregion

        #region resetDataGrid
        private void resetDataGrid()
        {
            int axisNo = -1;

            if (m_axis != null)
            {
                for (int i = 0; i < m_axis.Count; i++)
                {
                    double dPos = 0;
                    if (m_axis[i] != null)
                    {
                        //m_axis[i].GetActualPosition(ref dPos);

                        if (m_axis[i].Board.Configuration.BoardType == MotionBoardType.Ajin)
                        {
                            //if (ACSSPiiPlusMotionBoard.Api.IsConnected && waferProbeAlign.ACS_Motion_isSimulationMode)
                            //{
                            //    dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)m_axis[i].No);
                            //}
                            //else
                            {
                                axisNo = Int32.Parse(m_axis[i].Description);
                                //dPos = MC_Func.MC_GetEncPos(m_axis[i].No);
                                dPos = MC_Func.MC_GetEncPos(axisNo);
                            }
                        }
                        //else if (ACSSPiiPlusMotionBoard.Api.IsConnected && (m_axis[i].Board.Configuration.BoardType == MotionBoardType.ACS))
                        //{
                        //    axisNo = Int32.Parse(m_axis[i].Description);
                        //    //dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)m_axis[i].No);
                        //    dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                        //}
                        else if (ACSSPiiPlusMotionBoard.Api.IsConnected && (m_axis[i].Board.Configuration.BoardType == MotionBoardType.ACS))
                        {
                            axisNo = Int32.Parse(m_axis[i].Description);
                            //dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)m_axis[i].No);
                            dPos = ACSSPiiPlusMotionBoard.Api.GetFPosition((Axis)axisNo);
                        }

                        this.dataGridViewJogControl[(int)JogControlAxisInfo.AbsolutePos, i].Value = Math.Round(dPos, 4);
                        if (m_relativeZeroPositions.ContainsKey(m_axis[i]))
                        {
                            dPos -= m_relativeZeroPositions[m_axis[i]];
                        }

                        this.dataGridViewJogControl[(int)JogControlAxisInfo.RelativePos, i].Value = Math.Round(dPos, 4);
                        this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, i].Value = Math.Round(Step[m_axis[i]], 4);
                        this.dataGridViewJogControl[(int)JogControlAxisInfo.Velocity, i].Value = Math.Round(Velocity[m_axis[i]], 4);
                    }

                }
            }
        }
        #endregion

        #region radioButtonCheckedChanged
        private void radioButtonStep_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonStep.Checked == true)
            {
                radioButtonContinuous.Checked = false;
            }
        }

        private void radioButtonContinuous_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonContinuous.Checked == true)
            {
                radioButtonStep.Checked = false;
            }
        }

        #endregion

        #region dataGridViewJogControl_CellValueChanged
        private void dataGridViewJogControl_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            decimal m_OutNumber = 0;
            string cellData = "";

            if (dataGridViewJogControl.SelectedCells.Count > 0 && dataGridViewJogControl.SelectedCells[0] != null)
            {
                int nIndex = dataGridViewJogControl.SelectedCells[0].RowIndex;
                for (int i = 0; i < dataGridViewJogControl.RowCount; i++)
                {
                    if (dataGridViewJogControl[e.ColumnIndex, i].Value != null)
                        cellData = dataGridViewJogControl[e.ColumnIndex, i].Value.ToString();       //  공백이 아닐 때만 데이터로 사용
                    else
                        cellData = "1";                                                             //  공백이면 1 로 세팅

                    bool canConvert = decimal.TryParse(cellData, out m_OutNumber);                  //  데이터가 숫자로만 이루어져 있는지 체크
                    if (canConvert == true)
                    {
                        cellData = m_OutNumber.ToString();                                          //  숫자만으로 되어 있으면 데이터로 사용
                    }
                    else
                    {
                        cellData = "1";                                                             //  문자가 섞여 있으면 1로 세팅
                    }

                    //string cellData = dataGridViewJogControl[e.ColumnIndex, i].Value.ToString();
                    switch ((JogControlAxisInfo)e.ColumnIndex)
                    {
                        case JogControlAxisInfo.Step:
                            Step[m_axis[i]] = double.Parse(cellData);
                            break;
                        case JogControlAxisInfo.Velocity:
                            Velocity[m_axis[i]] = double.Parse(cellData);
                            break;
                    }

                }
            }

        }
        #endregion

        #region ToggleButton Event
        private void baseToggleButton1_Click(object sender, EventArgs e)
        {
            bool IsClicked = !baseToggleButton1.GetButtonStatus();
            if (IsClicked)
            {
                this.baseToggleButton1.UpdateToggleStatus(IsClicked);
                this.baseToggleButton01.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton0001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonDivision.UpdateToggleStatus(!IsClicked);
                for (int iter = 0; iter < dataGridViewJogControl.RowCount; iter++)
                {
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, iter].Value = 1;
                }
            }
            else
            {
                this.baseToggleButton1.UpdateToggleStatus(false);
                this.baseToggleButton01.UpdateToggleStatus(false);
                this.baseToggleButton001.UpdateToggleStatus(false);
                this.baseToggleButton0001.UpdateToggleStatus(false);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(false);
                this.baseToggleButtonDivision.UpdateToggleStatus(false);
            }
        }

        private void baseToggleButton01_Click(object sender, EventArgs e)
        {
            bool IsClicked = !baseToggleButton01.GetButtonStatus();
            if (IsClicked)
            {
                this.baseToggleButton1.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton01.UpdateToggleStatus(IsClicked);
                this.baseToggleButton001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton0001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonDivision.UpdateToggleStatus(!IsClicked);
                for (int iter = 0; iter < dataGridViewJogControl.RowCount; iter++)
                {
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, iter].Value = 0.1;
                }
            }
            else
            {
                this.baseToggleButton1.UpdateToggleStatus(false);
                this.baseToggleButton01.UpdateToggleStatus(false);
                this.baseToggleButton001.UpdateToggleStatus(false);
                this.baseToggleButton0001.UpdateToggleStatus(false);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(false);
                this.baseToggleButtonDivision.UpdateToggleStatus(false);
            }
        }

        private void baseToggleButton001_Click(object sender, EventArgs e)
        {
            bool IsClicked = !baseToggleButton001.GetButtonStatus();
            if (IsClicked)
            {
                this.baseToggleButton1.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton01.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton001.UpdateToggleStatus(IsClicked);
                this.baseToggleButton0001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonDivision.UpdateToggleStatus(!IsClicked);
                for (int iter = 0; iter < dataGridViewJogControl.RowCount; iter++)
                {
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, iter].Value = 0.01;
                }
            }
            else
            {
                this.baseToggleButton1.UpdateToggleStatus(false);
                this.baseToggleButton01.UpdateToggleStatus(false);
                this.baseToggleButton001.UpdateToggleStatus(false);
                this.baseToggleButton0001.UpdateToggleStatus(false);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(false);
                this.baseToggleButtonDivision.UpdateToggleStatus(false);
            }
        }

        private void baseToggleButton0001_Click(object sender, EventArgs e)
        {
            bool IsClicked = !baseToggleButton0001.GetButtonStatus();
            if (IsClicked)
            {
                this.baseToggleButton1.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton01.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton0001.UpdateToggleStatus(IsClicked);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonDivision.UpdateToggleStatus(!IsClicked);
                for (int iter = 0; iter < dataGridViewJogControl.RowCount; iter++)
                {
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, iter].Value = 0.001;
                }
            }
            else
            {
                this.baseToggleButton1.UpdateToggleStatus(false);
                this.baseToggleButton01.UpdateToggleStatus(false);
                this.baseToggleButton001.UpdateToggleStatus(false);
                this.baseToggleButton0001.UpdateToggleStatus(false);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(false);
                this.baseToggleButtonDivision.UpdateToggleStatus(false);
            }
        }

        private void baseToggleButtonMultiplication_Click(object sender, EventArgs e)
        {
            bool IsClicked = !baseToggleButtonMultiplication.GetButtonStatus();
            if (IsClicked)
            {
                this.baseToggleButton1.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton01.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton0001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(IsClicked);
                this.baseToggleButtonDivision.UpdateToggleStatus(!IsClicked);
                for (int iter = 0; iter < dataGridViewJogControl.RowCount; iter++)
                {
                    double nValue = 0;
                    double.TryParse(this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, iter].Value.ToString(), out nValue);
                    this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, iter].Value = nValue * 2.0;
                }
            }
            else
            {
                this.baseToggleButton1.UpdateToggleStatus(false);
                this.baseToggleButton01.UpdateToggleStatus(false);
                this.baseToggleButton001.UpdateToggleStatus(false);
                this.baseToggleButton0001.UpdateToggleStatus(false);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(false);
                this.baseToggleButtonDivision.UpdateToggleStatus(false);
            }
        }

        private void baseToggleButtonDivision_Click(object sender, EventArgs e)
        {
            bool IsClicked = !baseToggleButtonDivision.GetButtonStatus();
            if (IsClicked)
            {
                this.baseToggleButton1.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton01.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButton0001.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(!IsClicked);
                this.baseToggleButtonDivision.UpdateToggleStatus(IsClicked);
                for (int iter = 0; iter < dataGridViewJogControl.RowCount; iter++)
                {
                    double nValue = 0.0;
                    double.TryParse(this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, iter].Value.ToString(), out nValue);
                    if (nValue > 0.001)
                    {
                        this.dataGridViewJogControl[(int)JogControlAxisInfo.Step, iter].Value = nValue / 2.0;
                    }
                }
            }
            else
            {
                this.baseToggleButton1.UpdateToggleStatus(false);
                this.baseToggleButton01.UpdateToggleStatus(false);
                this.baseToggleButton001.UpdateToggleStatus(false);
                this.baseToggleButton0001.UpdateToggleStatus(false);
                this.baseToggleButtonMultiplication.UpdateToggleStatus(false);
                this.baseToggleButtonDivision.UpdateToggleStatus(false);
            }
        }
        #endregion
    }
}
