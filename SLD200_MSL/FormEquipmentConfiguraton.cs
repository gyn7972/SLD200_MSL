using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.Hmi;
using QMC.Common.Motion.Ajin.IO;

namespace SLD200_MSL
{
    #region Define
    public enum ButtonTreeViewType
    {
        Add,
        Remove,
        Reset,
        Up,
        Down
    }

    public enum ButtonFormControlType
    {
        Save,
        Load
    }
    public enum TabPages
    {
        Construct,
        IO,
        Motion,
        Alarm

    }

    public enum MotionDataGridColumns
    {
        UID, Name, No, BoardNo, DisplayAxisType
    }
    #endregion

    public partial class FormEquipmentConfiguraton : FormSubContentBase       
    {
        #region Field
        private UIMakerEditor m_Editor;
        private List<int> m_nindex = new List<int>();
        public ModuleCollection m_collectionModules;
        private List<IOPointConfiguration> m_listDigitalIOConfiguration;
        public Module m_selectedModule;
        public Part m_selectedPart;
        public Part m_selectedParentPart;
        public int m_nIndex = 0;
        FormAddIO m_formAddIO = new FormAddIO();
        #endregion

        #region Property
        public List<IOPointConfiguration> ListDigitalIOConfiguration
        {
            get { return m_listDigitalIOConfiguration; }
            set { m_listDigitalIOConfiguration = value; }
        }
        public AlarmCollection Alarms { get; set; }
        public List<AjinIoAxlBoardConfiguration> ListIOBoardConfiguration { get; set; }

        #endregion

        #region Constructor
        public FormEquipmentConfiguraton()
            : base(FormType.Content.ToString(), "Equipment Configuration")
        {
            this.BackColor = Configuration.PanelBackColor;

            this.panelContent.Controls.Add(this.treeViewFuctionConfig);

            base.panelContent.Controls.Add(this.tabControl1);
            this.panelContent.Controls.Add(this.baseButtonSave);
            this.panelContent.Controls.Add(this.baseButtonLoad);
            base.panelContent.BackColor = Configuration.PanelBackColor;

            this.ListIOBoardConfiguration = new List<AjinIoAxlBoardConfiguration>();
            AjinIoAxlBoardConfiguration ioBoardConfig = new AjinIoAxlBoardConfiguration();
            m_Editor = new UIMakerEditor();
            m_Editor.ButtonClickEvent += AddTurretInfo;
            FormAddAixs form = new FormAddAixs();
            InitializeComponent();

            ButtonFormContolCreat();
            InitTreeView();
            UpdateDataGridViewAlarm();
            #region ControlsConstructor
            this.flowLayoutPanelButton.FlowDirection = FlowDirection.RightToLeft;
            this.treeViewFuctionConfig.BringToFront();
            this.flowLayoutPanelButton.Hide();

            this.baseButtonSave.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.baseButtonSave.Location = new Point(this.Size.Width + Configuration.ButtonSize.Width - 150, 30);
            this.baseButtonLoad.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            this.baseButtonLoad.Location = new Point(this.Size.Width  - 155, 30);

            this.treeViewFuctionConfig.Location = new System.Drawing.Point(Configuration.ContentLocation.X + (Configuration.ContentLocation.X) / 2 + 10, flowLayoutPanelButton.Location.X + flowLayoutPanelButton.Height * 2);
            this.treeViewFuctionConfig.Size = new System.Drawing.Size(Configuration.ContentSize.Width / 2 - Configuration.ButtonSize.Width - Configuration.ContentLocation.X, Configuration.ContentSize.Height - Configuration.PanelSize.Height - this.flowLayoutPanelButton.Height * 4);

            this.tabControl1.Location = new System.Drawing.Point(this.treeViewFuctionConfig.Location.X + this.treeViewFuctionConfig.Width + 10, this.treeViewFuctionConfig.Location.Y);
            this.tabControl1.Size = new Size(Configuration.ContentSize.Width / 2 - Configuration.ButtonSize.Width + 150, this.treeViewFuctionConfig.Height);
            this.propertyGridConfig.Location = new Point(0, 0);
            this.propertyGridConfig.Size = new Size(this.tabControl1.Size.Width - Configuration.ButtonSize.Width, Configuration.ContentSize.Height - Configuration.PanelSize.Height * 3 - flowLayoutPanelButton.Height);
            this.propertyGridConfig.Dock = DockStyle.Fill;

            this.basePropertyGridIO.Location = new Point(0, 0);
            this.basePropertyGridIO.Size = new Size(this.tabControl1.Size.Width - Configuration.ButtonSize.Width - 20, this.tabControl1.Size.Height - Configuration.ButtonSize.Height);

            this.basePropertyGridMotion.Location = new Point(0, 0);
            this.basePropertyGridMotion.Size = new Size(this.tabControl1.Size.Width - Configuration.ButtonSize.Width - 20, this.tabControl1.Size.Height - Configuration.ButtonSize.Height);

            this.baseDataGridViewAlarm.Location = new Point(0, 0);
            this.baseDataGridViewAlarm.Size = new Size(this.tabControl1.Size.Width - Configuration.ButtonSize.Width - 20, this.tabControl1.Size.Height - Configuration.ButtonSize.Height);

            this.baseButtonAlarmRemove.Size = Configuration.ButtonSize;
            this.baseButtonAlarmRemove.Location = new Point(this.tabControl1.Size.Width - 10 - Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);

            this.baseButtonAlarmAdd.Size = Configuration.ButtonSize;
            this.baseButtonAlarmAdd.Location = new Point(this.tabControl1.Size.Width - 10 - Configuration.ButtonSize.Width, 0);
            #endregion

            #region buttonAxisAdd
            this.buttonAxisMotionAdd.Size = Configuration.ButtonSize;
            this.buttonAxisMotionAdd.Location = new Point(this.tabControl1.Size.Width - 10 - Configuration.ButtonSize.Width, 0);
            #endregion

            #region buttonAxisRemove
            this.buttonAxisMotionRemove.Size = Configuration.ButtonSize;
            this.buttonAxisMotionRemove.Location = new Point(this.tabControl1.Size.Width - 10 - Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            #endregion

            #region buttonAxisIoAdd
            this.buttonAxisIoAdd.Size = Configuration.ButtonSize;
            this.buttonAxisIoAdd.Location = new Point(this.tabControl1.Size.Width - 10 - Configuration.ButtonSize.Width, 0);
            #endregion

            #region buttonAxisIoRemove
            this.buttonAxisIoRemove.Size = Configuration.ButtonSize;
            this.buttonAxisIoRemove.Location = new Point(this.tabControl1.Size.Width - 10 - Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
            #endregion



            TabPageCreate();
        }

        #endregion

        #region Method
        public void TabPageCreate()
        {
            this.tabControl1.Controls.Clear();
            TabPage tabPage = new TabPage();


            foreach (TabPages item in Enum.GetValues(typeof(TabPages)))
            {
                string pageName = item.ToString();

                tabPage = new TabPage(pageName);
                tabPage.BackColor = Color.FromArgb(200, 200, 200);
                tabPage.ForeColor = Color.FromArgb(78, 78, 78);
                tabControl1.TabPages.Add(tabPage);
            }
            for (int i = 0; i < tabControl1.TabCount; i++)
            {
                switch (tabControl1.TabPages[i].Text)
                {
                    case "Construct":
                        tabControl1.TabPages[i].Controls.Add(this.propertyGridConfig);
                        break;
                    case "IO":
                        tabControl1.TabPages[i].Controls.Add(this.basePropertyGridIO);
                        tabControl1.TabPages[i].Controls.Add(this.buttonAxisIoAdd);
                        tabControl1.TabPages[i].Controls.Add(this.buttonAxisIoRemove);

                        break;
                    case "Motion":
                        tabControl1.TabPages[i].Controls.Add(this.basePropertyGridMotion);
                        tabControl1.TabPages[i].Controls.Add(this.buttonAxisMotionAdd);
                        tabControl1.TabPages[i].Controls.Add(this.buttonAxisMotionRemove);
                        break;
                    case "Alarm":
                        tabControl1.TabPages[i].Controls.Add(this.baseDataGridViewAlarm);
                        tabControl1.TabPages[i].Controls.Add(this.baseButtonAlarmAdd);
                        tabControl1.TabPages[i].Controls.Add(this.baseButtonAlarmRemove);
                        break;

                    default:
                        break;
                }
            }

        }

        private void UpdateDataGridViewAlarm()
        {
            baseDataGridViewAlarm.Columns.Clear();
            baseDataGridViewAlarm.AutoGenerateColumns = false;
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Title";
                column.Name = "Title";
                baseDataGridViewAlarm.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Grade";
                column.Name = "Grade";
                baseDataGridViewAlarm.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Source";
                column.Name = "Source";
                baseDataGridViewAlarm.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Code";
                column.Name = "Code";
                baseDataGridViewAlarm.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Cause";
                column.Name = "Cause";
                baseDataGridViewAlarm.Columns.Add(column);
            }

            baseDataGridViewAlarm.DataSource = null;
            baseDataGridViewAlarm.DataSource = Alarms;
        }
        private void AddTurretInfo()
        {

        }
        public void ButtonFormContolCreat()
        {
            Point location = new Point();
            location.X = 0;
            location.Y = 0;
            foreach (ButtonFormControlType ButtonTreeView in Enum.GetValues(typeof(ButtonFormControlType)))
            {
                BaseButton btn = new BaseButton();
                switch (ButtonTreeView)
                {

                    case ButtonFormControlType.Save:
                        btn.Text = ButtonTreeView.ToString();
                        btn.Name = string.Format("button", ButtonTreeView.ToString());
                        btn.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                        btn.Location = location;
                        btn.Click += ButtonSaveClick;
                        break;
                    case ButtonFormControlType.Load:
                        btn.Text = ButtonTreeView.ToString();
                        btn.Name = string.Format("button", ButtonTreeView.ToString());
                        btn.Size = new System.Drawing.Size(Configuration.ButtonSize.Width, Configuration.ButtonSize.Height);
                        btn.Location = location;
                        btn.Click += ButtonLoadClick;
                        break;
                }
                location.X += 100;
                flowLayoutPanelButton.Controls.Add(btn);
            }
        }

        #region InitTreeView
        private void InitTreeView()
        {
            treeViewFuctionConfig.Nodes.Clear();
            string strEq = string.Format("Equipment({0})", Equipment.Name);
            //editor.moduels = Equipment.Modules;
            TreeNode nodeEquipment = treeViewFuctionConfig.Nodes.Add(strEq);
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                TreeNode nodeModule = nodeEquipment.Nodes.Add(module.Name);
                foreach (Part part in module.Parts)
                {
                    AddPartNode(part, nodeModule);
                }
            }
        }

        private void AddPartNode(Part part, TreeNode node)
        {
            TreeNode partNode = node.Nodes.Add(part.Name);
        }
        #endregion

        #region RecursiveFindParents
        public void RecursivefindParents(TreeNode treeNode)
        {
            if (treeNode.Parent != null)
            {
                RecursivefindParents(treeNode.Parent);
            }
            m_nindex.Add(treeNode.Index);
        }
        #endregion


        #endregion

        #region EventHandler

        #region ButtonSaveClick
        public void ButtonSaveClick(object sender, EventArgs e)
        {
            Equipment.SaveIOPoints();
            Equipment.SaveMotionAxis();

        }
        #endregion

        #region ButtonLoadClick
        public void ButtonLoadClick(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            this.propertyGridConfig.SelectedObject = null;
            treeViewFuctionConfig.Nodes.Clear();
            Equipment.LoadIOBoards();


            if (m_selectedPart != null)
            {
            }
            InitTreeView();
        }
        #endregion        

        #region treeViewConfig_AfterSelect
        public void treeViewConfig_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = treeViewFuctionConfig.SelectedNode;
            IDictionary dicMotion;
            IDictionary dicIO;

            if (selectedNode != null)
            {
                m_nindex.Clear();
                RecursivefindParents(selectedNode);
                for (int i = 0; i < m_nindex.Count; i++)
                {
                    int nIter = m_nindex[i];
                    if (i == 0)
                    {
                        m_selectedModule = null;
                        m_selectedPart = null;
                        m_selectedParentPart = null;
                    }
                    else if (i == 1)
                    {
                        m_selectedModule = m_collectionModules[nIter];
                        propertyGridConfig.SelectedObject = null;
                        basePropertyGridMotion.SelectedObject = null;
                        basePropertyGridIO.SelectedObject = null;
                    }
                    else if (i == 2)
                    {
                        m_selectedPart = m_selectedModule.Parts[nIter];
                        propertyGridConfig.SelectedObject = m_selectedPart;
                        if (m_selectedPart != null || m_selectedParentPart != null)
                        {
                            dicMotion = m_selectedPart.Axes;
                            dicIO = m_selectedPart.DioPoints;
                            basePropertyGridMotion.SelectedObject = new DictionaryPropertyGridAdapter(dicMotion);
                            basePropertyGridIO.SelectedObject = new DictionaryPropertyGridAdapter(dicIO);
                        }

                    }
                    else
                    {
                        m_selectedParentPart = m_selectedPart;
                    }
                }
                if (m_selectedPart != null)
                {

                    UpdateDataGridViewAlarm();

                }
                else if (m_selectedModule != null)
                {
                    propertyGridConfig.SelectedObject = m_selectedModule;
                    UpdateDataGridViewAlarm();

                }
                else
                {
                    propertyGridConfig.SelectedObject = null;
                    baseDataGridViewAlarm.DataSource = null;
                    basePropertyGridMotion.SelectedObject = null;
                    UpdateDataGridViewAlarm();
                }
            }
        }
        #endregion

        #region PrepertyGridValueChanged
        private void propertyGridConfig_PropertyValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            TreeNode selectedNode = treeViewFuctionConfig.SelectedNode;

            PropertyGrid propertyGrid = sender as PropertyGrid;
            if (propertyGrid != null && treeViewFuctionConfig.Nodes.Count != 0)
            {
                if (m_selectedPart != null)
                {
                    selectedNode.Text = m_selectedPart.Name;
                }
                else if (m_selectedModule != null)
                {
                    selectedNode.Text = m_selectedModule.Name;
                }
            }
        }
        #endregion

        private void buttonAxisMotionAdd_Click(object sender, EventArgs e)
        {
            if (m_selectedPart != null && m_selectedPart.Axes.Count > 0)
            {
                FormAddAixs formAddAxis = new FormAddAixs();
                formAddAxis.PartUid = m_selectedPart.Name;
                formAddAxis.ModuleUid = m_selectedModule.Name;
                if (basePropertyGridMotion.SelectedGridItem.PropertyDescriptor != null)
                {
                    formAddAxis.Tag = basePropertyGridMotion.SelectedGridItem.PropertyDescriptor.Name;
                }
                formAddAxis.BringToFront();

                formAddAxis.StartPosition = FormStartPosition.CenterScreen;
                if (formAddAxis.ShowDialog() == DialogResult.OK)
                {
                    //추가 했을때
                    MotionAxis axis = formAddAxis.SelectedAxis;

                    m_selectedPart.SetMotion(basePropertyGridMotion.SelectedGridItem.PropertyDescriptor.Name, axis);
                    IDictionary dicMotion = m_selectedPart.Axes;
                    basePropertyGridMotion.SelectedObject = new DictionaryPropertyGridAdapter(dicMotion);
                }
            }
        }
        private void buttonAxisMotionRemove_Click(object sender, EventArgs e)
        {
            if (m_selectedPart != null && m_selectedPart.Axes.Count > 0)
            {
                string strAxis = basePropertyGridMotion.SelectedGridItem.PropertyDescriptor.Name;
                MotionAxis axis = m_selectedPart.GetMotion(strAxis);
                if (axis != null)
                {
                    axis.Module = "";
                    axis.Part = "";
                    axis.Tag = "";
                }

                m_selectedPart.SetMotion(strAxis, null);

                IDictionary dicMotion = m_selectedPart.Axes;
                basePropertyGridMotion.SelectedObject = new DictionaryPropertyGridAdapter(dicMotion);
            }
        }

        private void baseButtonAlarmAdd_Click(object sender, EventArgs e)
        {
            if (m_selectedPart != null)
            {
                FormAddAlarm form = new FormAddAlarm(m_selectedPart);
                form.BringToFront();
                form.StartPosition = FormStartPosition.CenterScreen;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    //추가 했을때
                    if (form != null)
                    {
                        UpdateDataGridViewAlarm();
                    }
                }
            }
        }

        private void baseButtonAlarmRemove_Click(object sender, EventArgs e)
        {
            if (m_selectedPart != null)
            {
                if (baseDataGridViewAlarm.Rows.Count < 0)
                {
                    return;
                }
                if (baseDataGridViewAlarm.Rows.Count > 0)
                {
                    int nRow = baseDataGridViewAlarm.SelectedCells[0].RowIndex;
                    if (nRow >= 0)
                    {
                        Alarm alarm = baseDataGridViewAlarm.Rows[nRow].DataBoundItem as Alarm;

                        UpdateDataGridViewAlarm();
                    }
                }

            }
        }

        private void buttonAxisIoAdd_Click(object sender, EventArgs e)
        {
            if (m_selectedPart != null && m_selectedPart.DioPoints.Count > 0)
            {

                m_formAddIO.ModuleUid = m_selectedModule.Name;
                m_formAddIO.PartUid = m_selectedPart.Name;
                if (basePropertyGridIO.SelectedGridItem.PropertyDescriptor != null)
                {
                    m_formAddIO.Tag = basePropertyGridIO.SelectedGridItem.PropertyDescriptor.Name;
                }
                m_formAddIO.BringToFront();


                m_formAddIO.StartPosition = FormStartPosition.CenterScreen;
                if (m_formAddIO.ShowDialog() == DialogResult.OK)
                {
                    //추가 했을때                   
                    DioPoint DioPoint = (DioPoint)m_formAddIO.SelectedIO;
                    m_selectedPart.SetDioPoint(basePropertyGridIO.SelectedGridItem.PropertyDescriptor.Name, DioPoint);

                    IDictionary dicIO = m_selectedPart.DioPoints;
                    basePropertyGridIO.SelectedObject = new DictionaryPropertyGridAdapter(dicIO);
                }
            }
        }

        private void buttonAxisIoRemove_Click(object sender, EventArgs e)
        {
            if (m_selectedPart != null && m_selectedPart.DioPoints.Count > 0)
            {
                string strDioPoint = basePropertyGridIO.SelectedGridItem.PropertyDescriptor.Name;
                DioPoint point = m_selectedPart.GetDioPoint(strDioPoint);
                if (point != null)
                {
                    point.ModuleUid = "";
                    point.PartUid = "";
                    point.Tag = "";
                    m_formAddIO.GetLocatorInfo(point.ModuleUid, point.PartUid, point);
                }
                m_selectedPart.SetDioPoint(strDioPoint, null);

                IDictionary dicDioPoint = m_selectedPart.DioPoints;
                basePropertyGridIO.SelectedObject = new DictionaryPropertyGridAdapter(dicDioPoint);
            }
        }

        #endregion

        #region 화면 깜빡임 제거
        //화면
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        #endregion
    }
}


