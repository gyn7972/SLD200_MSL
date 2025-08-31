using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using SpiralLab.Sirius2.Winforms;
using QMC.Common;
using static QMC.Common.Modules.WorkStage;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.VisionPart;
using SpiralLab.Sirius;
using static QMC.Common.Modules.Loader;
using netDxf.Units;
using QMC.Core;
using static QMC.Common.Equipment;
using Microsoft.Win32;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using QMC.Common.Vision.Cameras;
using SLD200.NewStyleForm.NewSubForm;
using SLD200.NewStyleForm;
using QMC.Common.Recipe;
using System.Threading;
using QMC.Common.Q_Recipe;

namespace SLD200_MSL
{
    public partial class FormNew_Recipe : Form
    {
        //Test 변수 (Recipe 관리)
        private bool bTestRecipe = false; // MainForm 에서 Recipe Open 요청 여부

        private bool m_bFormVisible = false; // 실제 Show 상태 여부

        static WorkStage workStage;

        FormNew_SiriusEditor m_formSiriusEditor = null;

        private System.Windows.Forms.Timer timer_Recipe_Open;

        //private FormNewSub_Recipe_Vision userform_RecipeVision;
        //private FormNewSub_Recipe_GoldPowder userform_RecipeGoldPowder;

        public FormNewSub_Recipe_Vision userform_RecipeVision { get; set; }
        public FormNewSub_Recipe_GoldPowder userform_RecipeGoldPowder { get; set; }

        public Action<bool> ActionLoadRecipe;

        public FormNew_Recipe()
        {
            InitializeComponent();

            //Size 축소 / 확대 안되게 하기 위한 코드.
            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.UpdateStyles();

            //this.Load += FormNewSub_Recipe_Load; // 여기서 Load 이벤트 연결
            this.tabControl_Recipe.SelectedIndexChanged += new System.EventHandler(this.tabControl_Recipe_SelectedIndexChanged);

            FormNewSub_Recipe_Load();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void SetRecipeTabs(FormNewSub_Recipe_Vision vision, FormNewSub_Recipe_GoldPowder gold)
        {
            this.userform_RecipeVision = vision;
            this.userform_RecipeGoldPowder = gold;
            LoadSubForm();
        }

        //private void FormNewSub_Recipe_Load(object sender, EventArgs e)
        private void FormNewSub_Recipe_Load()
        {
            //GUI생성 완료 후 Data 및 Cintroller 업데이트!
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }

            MachineType_Component_Enable(Equipment.Machine_LaserType_CO2);

            m_formSiriusEditor = new FormNew_SiriusEditor();
            m_formSiriusEditor.CreateSiriusEditor();
            m_formSiriusEditor.Owner = this;

            //  Layer Data 를 보여주는 ListView 설정
            listView_Recipe_TabRecipe_LayerData.View = View.Details;
            listView_Recipe_TabRecipe_LayerData.GridLines = true;         //  구분선 표시
            listView_Recipe_TabRecipe_LayerData.FullRowSelect = true;     //  한줄씩 선택 설정

            //  Recipe Open 타이머
            timer_Recipe_Open = new System.Windows.Forms.Timer();
            timer_Recipe_Open.Interval = 200;
            timer_Recipe_Open.Tick += new System.EventHandler(Timer_RecipeOpen_Func);
            timer_Recipe_Open.Enabled = true;

            //  Custom Marking Data 설정 (Recipe Load 전에는 비활성화)
            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Enabled = false;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Enabled = false;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Enabled = false;

            //  Recipe Open 전에는 Hatch 모드가 Disable 이므로 Hatch Spacing 을 비활성화 한다.
            textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;

            //  Recipe Open 전에는 Marking Data 가 Text 이므로, Serial Number 를 초기화 하는 Reset 버튼은 비활성화 한다.
            button_Marking_SerialNumber_CountReset.Enabled = false;

            listBox_Recipe_TabRecipe_ListOfDrawingLayer.DrawMode = DrawMode.OwnerDrawFixed;
            listBox_Recipe_TabRecipe_ListOfDrawingLayer.ItemHeight = 24;

            checkBox_MasterView.Checked = false;

            InitRecipeUI_KeyPad();
            ApplyTooltips();
        }

        private void MachineType_Component_Enable(bool m_bLaserType)
        {
            //  Laser Type (true:CO2, false:UV)
            label_Recipe_TabRecipe_Miscellaneous_DrillingPower.Enabled = !m_bLaserType;
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Enabled = !m_bLaserType;
            button_Recipe_TabRecipe_Miscellaneous_DrillingPower.Enabled = !m_bLaserType;

            label_Recipe_TabRecipe_Miscellaneous_Mask.Enabled = m_bLaserType;
            comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.Enabled = m_bLaserType;
            label_Recipe_TabRecipe_Miscellaneous_BETPosition.Enabled = m_bLaserType;
            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.Enabled = m_bLaserType;            
        }

        private void LoadSubForm()
        {
            //OnCreateControl();
            if (userform_RecipeVision != null)
            {
                //userform_RecipeVision = new FormNewSub_Recipe_Vision();
                userform_RecipeVision.Dock = DockStyle.Fill;
                tabPage_RecipeVision.Controls.Add(userform_RecipeVision);
            }

            if (userform_RecipeGoldPowder != null)
            {
                //userform_RecipeGoldPowder = new FormNewSub_Recipe_GoldPowder();
                userform_RecipeGoldPowder.Dock = DockStyle.Fill;
                tabPage_RecipeGoldPowder.Controls.Add(userform_RecipeGoldPowder);
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!this.Created)
                return;

            if (this.Visible && !m_bFormVisible)
            {
                m_bFormVisible = true;
                OnShowRecipeForm();

                var selectedTab = tabControl_Recipe.SelectedTab;
                if (selectedTab == tabPage_RecipeVision)
                {
                    if (userform_RecipeVision != null &&
                        userform_RecipeVision.m_bInitialized)
                        userform_RecipeVision.OnShow();

                    if (userform_RecipeGoldPowder != null &&
                        userform_RecipeVision.m_bInitialized)
                        userform_RecipeGoldPowder.OnHide();
                }
                else if (selectedTab == tabPage_RecipeGoldPowder)
                {
                    if (userform_RecipeGoldPowder != null &&
                        userform_RecipeVision.m_bInitialized)
                        userform_RecipeGoldPowder.OnShow();

                    if (userform_RecipeVision != null &&
                        userform_RecipeVision.m_bInitialized)
                        userform_RecipeVision.OnHide();
                }
            }
            else if (!this.Visible && m_bFormVisible)
            {
                m_bFormVisible = false;
                OnHideRecipeForm();

                var selectedTab = tabControl_Recipe.SelectedTab;
                if (selectedTab == tabPage_RecipeVision)
                {
                    if (userform_RecipeVision != null
                        && userform_RecipeVision.m_bInitialized)
                        userform_RecipeVision.OnHide();
                }
                else if (selectedTab == tabPage_RecipeGoldPowder
                    && userform_RecipeVision.m_bInitialized)
                {
                    if (userform_RecipeGoldPowder != null)
                        userform_RecipeGoldPowder.OnHide();
                }
            }
        }

        private void tabControl_Recipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tabControl_Recipe.SelectedTab == tabPage_Recipe)
            {
                OnShowRecipeForm();
            }
            else if (tabControl_Recipe.SelectedTab == tabPage_RecipeVision) // "RecipeVision" 탭을 선택했을 때
            {
                // RecipeVision 탭일 때만 표시
                if (userform_RecipeVision != null && 
                    userform_RecipeVision.m_bInitialized)
                    userform_RecipeVision.OnShow();

                // 다른 탭으로 이동하면 숨김
                if (userform_RecipeGoldPowder != null && 
                    userform_RecipeGoldPowder.m_bInitialized)
                    userform_RecipeGoldPowder.OnHide();
            }
            else if (tabControl_Recipe.SelectedTab == tabPage_RecipeGoldPowder) // "RecipeGoldPowder" 탭을 선택했을 때
            {
                // RecipeGoldPowder 탭일 때만 표시
                if (userform_RecipeGoldPowder != null && 
                    userform_RecipeGoldPowder.m_bInitialized)
                    userform_RecipeGoldPowder.OnShow();

                // 다른 탭으로 이동하면 숨김
                if (userform_RecipeVision != null && 
                    userform_RecipeVision.m_bInitialized)
                    userform_RecipeVision.OnHide();
            }
            else
            {

            }
        }

        /// <summary>
        /// 화면이 활성화(Show)될 때 실행할 로직
        /// </summary>
        private void OnShowRecipeForm()
        {
            // 예시: 레시피 데이터 새로고침
            //Console.WriteLine("FormNew_Recipe 활성화됨 (Show)");

            // 실제 구현 로직 여기에
            // e.g., RefreshRecipeUI(), UpdateDeviceStatus(), etc.
            if (Equipment.AutoManualStatus)
            {
                button_Recipe_New.Enabled = false;
                button_Recipe_Open.Enabled = false;
                button_Recipe_Apply.Enabled = false;
                button_Recipe_Save.Enabled = false;
                button_Recipe_SaveAs.Enabled = false;
                button_Recipe_TabRecipe_OpenEditor.Enabled = false;
                button_Recipe_TabRecipe_OpenDwg.Enabled = false;
                button_Recipe_TabRecipe_LayerImport.Enabled = false;
            }
            else
            {
                button_Recipe_New.Enabled = true;
                button_Recipe_Open.Enabled = true;
                button_Recipe_Apply.Enabled = true;
                button_Recipe_Save.Enabled = true;
                button_Recipe_SaveAs.Enabled = true;
                button_Recipe_TabRecipe_OpenEditor.Enabled = true;
                button_Recipe_TabRecipe_OpenDwg.Enabled = true;
                button_Recipe_TabRecipe_LayerImport.Enabled = true;

                if (listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Count > 0)
                {
                    listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex = -1;  // 선택 해제
                    listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex = 0;   // 다시 선택 → 이벤트 발생
                    //listBox_Recipe_TabRecipe_ListOfDrawingLayer_SelectedIndexChanged // <- 자동 실행
                    // 레시피 데이터 새로고침
                    //Recipe_Data_Refresh("Hole1");
                }
            }
        }

        /// <summary>
        /// 화면이 비활성화(Hide)될 때 실행할 로직
        /// </summary>
        private void OnHideRecipeForm()
        {
            //Console.WriteLine("FormNew_Recipe 비활성화됨 (Hide)");

            // 예시: 타이머 멈춤, 리소스 일시 해제 등
            // StopRecipePreviewTimer();

            if (Equipment.AutoRunStatus)
            {
                button_Recipe_New.Enabled = false;
                button_Recipe_Open.Enabled = false;
                button_Recipe_Apply.Enabled = false;
                button_Recipe_Save.Enabled = false;
                button_Recipe_SaveAs.Enabled = false;
                button_Recipe_TabRecipe_OpenEditor.Enabled = false;
                button_Recipe_TabRecipe_OpenDwg.Enabled = false;
                button_Recipe_TabRecipe_LayerImport.Enabled = false;
            }
            else
            {
                button_Recipe_New.Enabled = true;
                button_Recipe_Open.Enabled = true;
                button_Recipe_Apply.Enabled = true;
                button_Recipe_Save.Enabled = true;
                button_Recipe_SaveAs.Enabled = true;
                button_Recipe_TabRecipe_OpenEditor.Enabled = true;
                button_Recipe_TabRecipe_OpenDwg.Enabled = true;
                button_Recipe_TabRecipe_LayerImport.Enabled = true;
            }

        }

        private void FormNew_Recipe_Shown(object sender, EventArgs e)
        {
            //  화면이 처음 표시될 때 보여지는 Recipe Data

            //  BET, Mrad

            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex = Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex;

            switch (Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex)
            {
                case 0:             //  BET 0.8X
                    workStage.m_dBET_ZoomValue_Recipe = 0.8;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_8X_Mrad;
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_0_8X_Mrad.ToString();
                    break;

                case 1:             //  BET 0.9X
                    workStage.m_dBET_ZoomValue_Recipe = 0.9;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_9X_Mrad;
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_0_9X_Mrad.ToString();
                    break;

                case 2:             //  BET 1.0X
                    workStage.m_dBET_ZoomValue_Recipe = 1.0;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_0X_Mrad;
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_0X_Mrad.ToString();
                    break;

                case 3:             //  BET 1.1X
                    workStage.m_dBET_ZoomValue_Recipe = 1.1;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_1X_Mrad;
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_1X_Mrad.ToString();
                    break;

                case 4:             //  BET 1.2X
                    workStage.m_dBET_ZoomValue_Recipe = 1.2;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_2X_Mrad;
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_2X_Mrad.ToString();
                    break;
            }
        }
        //protected override void OnCreateControl()
        //{
        //    base.OnCreateControl(); // 반드시 호출
        //                            // 추가 초기화 코드
        //}


        private void Timer_RecipeOpen_Func(object sender, EventArgs e)
        {
            timer_Recipe_Open.Enabled = false;

            if (RecipeOpen_fromMainForm && (Equipment.RecipeName_fromMainForm.Length != 0))
            {
                RecipeOpen_fromMainForm = false;

                if(bTestRecipe)
                {
                    RecipeManager.Instance.OpenRecipe(Equipment.RecipeName_fromMainForm);
                    RefreshUIAfterRecipeOpen(Equipment.RecipeName_fromMainForm); // 아래 3번 헬퍼 (UI 반영)
                }
                else
                {
                    Recipe_Open(Equipment.RecipeName_fromMainForm);

                }
            }

            timer_Recipe_Open.Enabled = true;
        }

        private void button_Recipe_TabRecipe_OpenEditor_Click(object sender, EventArgs e)
        {
            //  Sirius Editor 창을 연다.
            if (workStage.rtc == null)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !!", "먼저 Scanner Board 를 초기화 해야 합니다.");
                return;
            }

            if (m_formSiriusEditor == null || m_formSiriusEditor.IsDisposed)
            {
                m_formSiriusEditor = new FormNew_SiriusEditor();
                m_formSiriusEditor.CreateSiriusEditor();
                Thread.Sleep(500);
            }

            //창이 열려있을때 새로 눌렀을 경우. ( 창이 아래에 숨는경우 )
            // 도면이 다르면 새로 Import
            string newPath = richTextBox_Recipe_TabRecipe_DrawingFile.Text;
            if (!m_formSiriusEditor.Imported_DrawingFile_SameCheck(newPath))
            {
                // Document가 null이면 새로 생성
                if (m_formSiriusEditor.SiriusEditor.Document == null)
                {
                    m_formSiriusEditor.SiriusEditor.Document = new DocumentDefault();
                    m_formSiriusEditor.SiriusEditor.Document.FileName = "NewDocument";
                    m_formSiriusEditor.SiriusEditor.Document.Action.ActNew();

                    Log.Write("SiriusEditor_Info", "Button Click", "Document was null. Created new document.");
                }
            }

            // 새 도면 불러오기
            m_formSiriusEditor.Import_DrawingFile(newPath);

            // 이미 열려 있으면 앞으로 가져오기
            if (!m_formSiriusEditor.Visible)
            {
                m_formSiriusEditor.BringToFront();
                m_formSiriusEditor.Show();
            }
            else
            {
                if (m_formSiriusEditor.WindowState == FormWindowState.Minimized)
                {
                    m_formSiriusEditor.WindowState = FormWindowState.Normal;
                }

                m_formSiriusEditor.BringToFront();
                m_formSiriusEditor.Focus();
            }
            return;
        }

        private void button_Recipe_TabRecipe_OpenDwg_Click(object sender, EventArgs e)
        {
            int m_nReturn = -1;

            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.CustomPlaces.Add(SLD200.Properties.Settings.Default.JobFolder);

                if (Equipment.DrawingFilePath.Length > 0)
                {
                    fd.InitialDirectory = Equipment.DrawingFilePath;
                }
                else
                {
                    fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Recent);          //  최근 폴더
                }

                fd.Filter = "sirius files (*.sirius)|*.sirius|All files (*.*)|*.*"; //필터 설정
                fd.FilterIndex = 1; //1번 선택시 txt , 2번 선택시 *.*

                Log.Write("SLD-200", "Button Click", "도면 파일 불러오기");
                
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    filePath = fd.FileName;

                    richTextBox_Recipe_TabRecipe_DrawingFile.Text = filePath;                    
                }
            }
        }

        private void button_Recipe_TabRecipe_LayerImport_Click(object sender, EventArgs e)
        {
            if (Equipment.GetEqpSiriusViewerDocument() == null)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !!", "RTC 보드를 초기화 해야 합니다.");
                return;
            }

            //  Layer List 전체 삭제
            listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Clear();

            if (richTextBox_Recipe_TabRecipe_DrawingFile.Text.Length != 0)
            {
                //  해당 위치에 파일이 존재하는지 확인
                if (File.Exists(richTextBox_Recipe_TabRecipe_DrawingFile.Text) == false)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Error !!", "도면 파일이 없습니다.");
                    return;
                }

                ////  확장자가 도면인지 확인 (.sirius2, .dwg, .dxf)
                //  확장자가 도면인지 확인 (.sirius, .dxf)
                string m_strExt = Path.GetExtension(richTextBox_Recipe_TabRecipe_DrawingFile.Text);

                if (m_strExt.ToUpper() == ".DXF")
                {
                    //SiriusEditor.Document.New();
                    var doc = DocumentSerializer.OpenDxf(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                    //Equipment.EqpSiriusViewer.Document = doc;
                    m_formSiriusEditor.SiriusEditor.Document.Views.Clear();
                    m_formSiriusEditor.SiriusEditor.Document = doc;
                }
                else if (m_strExt.ToUpper() == ".SIRIUS")
                {
                    //SiriusEditor.Document.New();
                    var doc = DocumentSerializer.OpenSirius(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                    //Equipment.EqpSiriusViewer.Document = doc;
                    m_formSiriusEditor.SiriusEditor.Document.Views.Clear();
                    m_formSiriusEditor.SiriusEditor.Document = doc;
                }
                else
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !!", "도면 파일이 아닙니다.\r\n\r\n[available  *.sirius, *.dxf]");
                    return;
                }

                Equipment.SetEqpSiriusViewerDocument(m_formSiriusEditor.SiriusEditor.Document);
                if (workStage.DrillingData_Parsing())
                {
                    foreach (var layer in m_formSiriusEditor.SiriusEditor.Document.Layers)
                    {
                        if (layer.IsMarkerable)
                        {
                            listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Add(layer.Name);
                        }
                    }
                }
            }
        }

        private void listBox_Recipe_TabRecipe_ListOfDrawingLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  선택된 Layer 데이터를 ListView 에 표시
            int m_nIndex = listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex;
            string m_strLayerName = "";

            if (m_nIndex < 0)
            {
                return;
            }

            m_strLayerName = listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items[m_nIndex].ToString();

            listView_Recipe_TabRecipe_LayerData.BeginUpdate();

            //  ListView Column 삭제
            listView_Recipe_TabRecipe_LayerData.Items.Clear();
            foreach (ColumnHeader header in listView_Recipe_TabRecipe_LayerData.Columns)
            {
                listView_Recipe_TabRecipe_LayerData.Columns.Remove(header);
            }

            //  ListView Column 설정
            if ((m_strLayerName == "Hole1") ||
                (m_strLayerName == "Hole2") ||
                (m_strLayerName == "Hole3") ||
                (m_strLayerName == "Hole4") ||
                (m_strLayerName == "Hole5") ||
                (m_strLayerName == "Hole6") ||
                (m_strLayerName == "Hole7") ||
                (m_strLayerName == "Hole8") ||
                (m_strLayerName == "Hole9") ||
                (m_strLayerName == "Hole10") ||
                (m_strLayerName == "Thruhole") ||
                (m_strLayerName == "Fiducial"))
            {
                listView_Recipe_TabRecipe_LayerData.Columns.Add("Index", 50, HorizontalAlignment.Center);
                listView_Recipe_TabRecipe_LayerData.Columns.Add("Center X", 80, HorizontalAlignment.Center);
                listView_Recipe_TabRecipe_LayerData.Columns.Add("Center Y", 80, HorizontalAlignment.Center);
                listView_Recipe_TabRecipe_LayerData.Columns.Add("radius", 60, HorizontalAlignment.Center);
            }
            else if ((m_strLayerName == "Rect") ||
                    (m_strLayerName == "Outline"))
            {
                listView_Recipe_TabRecipe_LayerData.Columns.Add("Index", 50, HorizontalAlignment.Center);
                listView_Recipe_TabRecipe_LayerData.Columns.Add("Center X", 80, HorizontalAlignment.Center);
                listView_Recipe_TabRecipe_LayerData.Columns.Add("Center Y", 80, HorizontalAlignment.Center);
                listView_Recipe_TabRecipe_LayerData.Columns.Add("Width", 60, HorizontalAlignment.Center);
                listView_Recipe_TabRecipe_LayerData.Columns.Add("Height", 60, HorizontalAlignment.Center);
            }
            else if (m_strLayerName == "Marking")
            {

            }

            //  ListView Data 표시
            if (m_strLayerName == "Hole1")
            {
                //if (workStage.m_nDrawing_Hole1Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole1Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole1[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole1[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole1[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Hole2")
            {
                //if (workStage.m_nDrawing_Hole2Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole2Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole2[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole2[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole2[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Hole3")
            {
                //if (workStage.m_nDrawing_Hole3Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole3Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole3[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole3[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole3[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Hole4")
            {
                //if (workStage.m_nDrawing_Hole4Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole4Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole4[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole4[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole4[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Hole5")
            {
                //if (workStage.m_nDrawing_Hole5Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole5Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole5[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole5[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole5[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Hole6")
            {
                //if (workStage.m_nDrawing_Hole6Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole6Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole6[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole6[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole6[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Hole7")
            {
                //if (workStage.m_nDrawing_Hole7Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole7Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole7[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole7[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole7[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Hole8")
            {
                //if (workStage.m_nDrawing_Hole8Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole8Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole8[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole8[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole8[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Hole9")
            {
                //if (workStage.m_nDrawing_Hole8Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole8Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole9[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole9[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole9[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Hole10")
            {
                //if (workStage.m_nDrawing_Hole8Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole8Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole10[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole10[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole10[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Thruhole")
            {
                //if (workStage.m_nDrawing_Hole8Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_ThruholeCount; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Thruhole[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Thruhole[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Thruhole[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Rect")
            {
                //if (workStage.m_nDrawing_RectCount > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_RectCount; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Rect[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Rect[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Rect[i].Width));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Rect[i].Height));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Outline")
            {
                for (int i = 0; i < workStage.m_nDrawing_OutlineCount; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = (i + 1).ToString();
                    item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Outline[i].CenterX));
                    item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Outline[i].CenterY));
                    item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Outline[i].Width));
                    item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Outline[i].Height));
                    listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                }

                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Marking")
            {
                for (int i = 0; i < workStage.m_nMarking_SocketCount; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = (i + 1).ToString();
                    item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stMarking_SocketData.m_stMarking_ObjectData[i].dObjectCenter.X));
                    item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stMarking_SocketData.m_stMarking_ObjectData[i].dObjectCenter.Y));
                    listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                }

                SetRecipeTabControlsVisible(m_strLayerName, true);
            }
            else if (m_strLayerName == "Fiducial")
            {
                //if (workStage.m_nDrawing_FiducialCount > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_FiducialCount; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Fiducial[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Fiducial[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Fiducial[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }

                SetRecipeTabControlsVisible(m_strLayerName, false);
            }
            else if (m_strLayerName == "PreAlign")
            {
                for (int i = 0; i < workStage.m_nDrawing_FiducialCount; i++)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = (i + 1).ToString();
                    item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Fiducial[i].CenterX));
                    item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Fiducial[i].CenterY));
                    item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Fiducial[i].radius));
                    listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                }

                SetRecipeTabControlsVisible(m_strLayerName, false);
            }
            else
            {

            }

            // 리스트뷰를 Refresh하여 보여줌
            listView_Recipe_TabRecipe_LayerData.EndUpdate();

            // Layer 에 대한 Miscellaneous Data 표시
            Recipe_Data_Refresh(m_strLayerName);
        }


        #region Recipe Parameter Save / Load
        public bool Recipe_Data_Load(string m_strRecipeFile)
        {
            string strTemp = "";

            bool m_bRet = true;
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            //strFIle = ConfigManager.GetRecipeDataPath() + "\\LDUL_TeachingPosition.ini";
            strFIle = m_strRecipeFile;


            //  Recipe Parameter 로드
            for (int i = 0; i < (int)System.Enum.GetValues(typeof(LayerList)).Length; i++)
            {
                strTemp = string.Format("Layer_{0}", i);

                //  Recipe File Path and Name
                NativeMethods.GetPrivateProfileString(strTemp, "Drawing_File_Name", "", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].DrawingFile = temp.ToString();

                //  Laser Pulse Width (us)
                NativeMethods.GetPrivateProfileString(strTemp, "Pulse_Width", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].LaserParam_PulseWidth = Equipment.ToDouble(temp.ToString());
                //  Laser Pulse Period (us)
                NativeMethods.GetPrivateProfileString(strTemp, "Pulse_Period", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].LaserParam_PulsePeriod = Equipment.ToDouble(temp.ToString());
                //  Laser Frequency (Hz)
                NativeMethods.GetPrivateProfileString(strTemp, "Frequency", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].LaserParam_Frequency = Equipment.ToInt(temp.ToString());
                //  Laser DutyCycle (%)
                NativeMethods.GetPrivateProfileString(strTemp, "Duty_Cycle", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].LaserParam_DutyCycle = Equipment.ToDouble(temp.ToString());

                //  Laser Trigger Mode (true: External, false: Internal)
                NativeMethods.GetPrivateProfileString(strTemp, "Trigger_Mode_External", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].LaserParam_TriggerMode_External = Convert.ToBoolean(temp.ToString());

                //  Process Priority (true: Space of P2P, false: Pulse Period)
                NativeMethods.GetPrivateProfileString(strTemp, "P2P", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ProcessPriority_P2P = Convert.ToBoolean(temp.ToString());

                //  Reference Layer
                NativeMethods.GetPrivateProfileString(strTemp, "Reference_Layer", "", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ReferenceLayer = temp.ToString();
                //  Defocusing Distance (mm)
                NativeMethods.GetPrivateProfileString(strTemp, "Defocusing_Distance", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_DefocusingDistance = Equipment.ToDouble(temp.ToString());
                //  Resizing (mm)
                NativeMethods.GetPrivateProfileString(strTemp, "Resizing", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_Resizing = Equipment.ToDouble(temp.ToString());
                //  Hole Drilling Start Position Division (등분)
                NativeMethods.GetPrivateProfileString(strTemp, "HoleDrilling_StartPosDivision", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleDrilling_StartPosDivision = Equipment.ToInt(temp.ToString());
                //  Group Split Size (mm)
                NativeMethods.GetPrivateProfileString(strTemp, "GroupSplitSize", "3.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize = Equipment.ToDouble(temp.ToString());
                //  Group Split Size - Height (mm)
                NativeMethods.GetPrivateProfileString(strTemp, "GroupSplitSize_Height", "3.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize_Height = Equipment.ToDouble(temp.ToString());
                //  Scanner Drilling Speed (mm/s)
                NativeMethods.GetPrivateProfileString(strTemp, "ScannerDrillingSpeed", "10", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerDrillingSpeed = Equipment.ToDouble(temp.ToString());
                //  Scanner Jump Speed (mm/s)
                NativeMethods.GetPrivateProfileString(strTemp, "ScannerJumpSpeed", "100", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerJumpSpeed = Equipment.ToDouble(temp.ToString());
                //  Laser On Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "LaserOnDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOnDelay = Equipment.ToInt(temp.ToString());
                //  Laser Off Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "LaserOffDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOffDelay = Equipment.ToInt(temp.ToString());
                //  Mark Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "MarkDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_MarkDelay = Equipment.ToInt(temp.ToString());
                //  Jump Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "JumpDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_JumpDelay = Equipment.ToInt(temp.ToString());
                //  Polygon Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "PolygonDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_PolygonDelay = Equipment.ToInt(temp.ToString());
                //  Drilling Power (%)
                NativeMethods.GetPrivateProfileString(strTemp, "DrillingPower", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_Drilling_Power = Equipment.ToDouble(temp.ToString());
                //  Space of P2P Distance (mm)
                NativeMethods.GetPrivateProfileString(strTemp, "P2PDistance", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_P2PDistance = Equipment.ToDouble(temp.ToString());
                //  Drilling Repetation
                NativeMethods.GetPrivateProfileString(strTemp, "DrillingRepetation", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetition = Equipment.ToInt(temp.ToString());
                //  Drilling Repetition bundle
                NativeMethods.GetPrivateProfileString(strTemp, "DrillingRepetitionBundle", "100", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetitionBundle = Equipment.ToInt(temp.ToString());
                //  Rotation Angle when Arc
                NativeMethods.GetPrivateProfileString(strTemp, "RotationAngleArc", "360.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_RotationAngleArc = Equipment.ToDouble(temp.ToString());
                //  Rotation Start Angle when Circle Processing 1 time
                NativeMethods.GetPrivateProfileString(strTemp, "RotationStartAngle_Circle1time", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_CircleStartAngleCircle1time = Equipment.ToDouble(temp.ToString());
                //  Mask Index (0:None, 1:Mask1, 2:Mask2, 3:Mask3, 4:Mask4)
                NativeMethods.GetPrivateProfileString(strTemp, "MaskIndex", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_MaskIndex = Equipment.ToInt(temp.ToString());
                //  BET Position Index (0:0.1X, 1:0.5X, 2:1.0X, 3:1.5X, 4:2.0X)
                NativeMethods.GetPrivateProfileString(strTemp, "BETPositionIndex", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_BETPositionIndex = Equipment.ToInt(temp.ToString());
                //  Hole Processing Type (0:Circle, 1:Spiral)
                NativeMethods.GetPrivateProfileString(strTemp, "HoleProcessingType", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleProcessingType = Equipment.ToInt(temp.ToString());
                //  Fiducial Align Type (0:Circle Find, 1:Pattern Matching)
                //NativeMethods.GetPrivateProfileString(strTemp, "FiducialAlignType", "0", temp, 255, strFIle);
                //Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialAlignType = Equipment.ToInt(temp.ToString());
                //  Fiducial Mark Type (0:Circle, 1:Gold Powder)
                //NativeMethods.GetPrivateProfileString(strTemp, "FiducialMarkType", "0", temp, 255, strFIle);
                //Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialMarkType = Equipment.ToInt(temp.ToString());
                //  Hole Data Sort by Distance Use
                NativeMethods.GetPrivateProfileString(strTemp, "HoleSortByDistance_Use", "true", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortByDistance_Use = Convert.ToBoolean(temp.ToString());
                //  Hole Data Sorting Distance
                NativeMethods.GetPrivateProfileString(strTemp, "HoleDataSortingDistance", "0.5", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortingDistance = Equipment.ToDouble(temp.ToString());

                //  Process Options
                NativeMethods.GetPrivateProfileString(strTemp, "Socket_Align_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketAlign_Use = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Socket_HeightCheck_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Socket_HeightCheckPos_OffsetX", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetX = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Socket_HeightCheckPos_OffsetY", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetY = Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString(strTemp, "GoldPowder_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ProcessOption_GoldPowderAlign_Use = Convert.ToBoolean(temp.ToString());

                //  Module Information
                NativeMethods.GetPrivateProfileString(strTemp, "Module_Width", "125.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Width = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Module_Height", "120.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Height = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Module_SiliconThickness", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Module_GoldPowderThickness", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Thickness = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Module_GoldPowderPercent", "75.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Percent = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Module_GoldPowderLimit", "0.05", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Limit = Equipment.ToDouble(temp.ToString());


                //  Spiral Parameter
                NativeMethods.GetPrivateProfileString(strTemp, "Spiral_OuterDiameter", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Spiral_InnerDiameter", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Spiral_Revolutions", "10", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions = Equipment.ToInt(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Spiral_AngleFactor", "10.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor = Equipment.ToDouble(temp.ToString());

                //  EPRO Module Absorption Level
                NativeMethods.GetPrivateProfileString(strTemp, "EPRO_ModuleAbsorptionLevel", "-40.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].EPRO_ModuleAbsorptionLevel = Equipment.ToDouble(temp.ToString());

                //  M-Aligner Vacuum Use
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Ignore", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Ignore = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Center", "true", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Inner", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Outer", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer = Convert.ToBoolean(temp.ToString());

                //  Dust Collector
                NativeMethods.GetPrivateProfileString(strTemp, "DustCollector_RemoteMode_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].DustCollectorRemoteMode_Use = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "DustCollector_Frequency_Upper", "20.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Upper = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "DustCollector_Frequency_Lower", "20.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Lower = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "DustCollector_Lower_Disable", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].DustCollectorLower_Disable = Convert.ToBoolean(temp.ToString());

                //  Marking Template
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingData_SiriusTemplate_Use = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_DataType", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_DataType = Equipment.ToInt(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_Width", "5.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Width = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_Height", "5.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Height = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_TextType", "true", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_TextType = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_PrefixData", "", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_PrefixData = temp.ToString();
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_StartNumber", "1", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_StartNumber = Equipment.ToInt(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_Digits", "3", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Digits = Equipment.ToInt(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_IncreaseStep", "1", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_IncreaseStep = Equipment.ToInt(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_SuffixData", "", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SuffixData = temp.ToString();
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_Hatch_Use", "true", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Use = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_Hatch_Spacing", "0.1", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Spacing = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_SerialNumberType_IncreaseType", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SerialNumberIncreaseType = Equipment.ToInt(temp.ToString());

                NativeMethods.GetPrivateProfileString(strTemp, "ZCalFile_OffsetZ", "0.0", temp, 255, strFIle);
                stLayerRecipeSet[i].CalfileOffsetZAxismm = Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString(strTemp, "ChuckMSL_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ChuckMSL_Enable = Equipment.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Align3Point_Enable", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Align3Point_Enable = Equipment.ToBoolean(temp.ToString());
            }

            return m_bRet;
        }
        public bool Recipe_Data_Load_Refactory(string strRecipeFile)
        {
            if (string.IsNullOrWhiteSpace(strRecipeFile) || !File.Exists(strRecipeFile))
                return false;

            var iniLines = File.ReadAllLines(strRecipeFile);
            string currentSection = "";
            var sectionData = new Dictionary<string, Dictionary<string, string>>();

            foreach (var rawLine in iniLines)
            {
                string line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                    continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line.Substring(1, line.Length - 2);
                    if (!sectionData.ContainsKey(currentSection))
                        sectionData[currentSection] = new Dictionary<string, string>();
                }
                else if (currentSection != "")
                {
                    var kvp = line.Split(new[] { '=' }, 2);
                    if (kvp.Length == 2)
                    {
                        sectionData[currentSection][kvp[0].Trim()] = kvp[1].Trim();
                    }
                }
            }

            int layerCount = (int)System.Enum.GetValues(typeof(LayerList)).Length;
            for (int i = 0; i < layerCount; i++)
            {
                string section = $"Layer_{i}";

                if (!sectionData.ContainsKey(section))
                    continue;

                var data = sectionData[section];

                Equipment.stLayerRecipeSet[i].DrawingFile = ReadValue(data, "Drawing_File_Name", "");

                Equipment.stLayerRecipeSet[i].LaserParam_PulseWidth = ReadDouble(data, "Pulse_Width", 1);
                Equipment.stLayerRecipeSet[i].LaserParam_PulsePeriod = ReadDouble(data, "Pulse_Period", 0);
                Equipment.stLayerRecipeSet[i].LaserParam_Frequency = ReadInt(data, "Frequency", 7000);
                Equipment.stLayerRecipeSet[i].LaserParam_DutyCycle = ReadDouble(data, "Duty_Cycle", 0.2);

                Equipment.stLayerRecipeSet[i].LaserParam_TriggerMode_External = ReadBool(data, "Trigger_Mode_External", false);
                Equipment.stLayerRecipeSet[i].ProcessPriority_P2P = ReadBool(data, "P2P", true);

                Equipment.stLayerRecipeSet[i].Miscellaneous_ReferenceLayer = ReadValue(data, "Reference_Layer", "");
                Equipment.stLayerRecipeSet[i].Miscellaneous_DefocusingDistance = ReadDouble(data, "Defocusing_Distance", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_Resizing = ReadDouble(data, "Resizing", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleDrilling_StartPosDivision = ReadInt(data, "HoleDrilling_StartPosDivision", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize = ReadDouble(data, "GroupSplitSize", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize_Height = ReadDouble(data, "GroupSplitSize_Height", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerDrillingSpeed = ReadDouble(data, "ScannerDrillingSpeed", 100.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerJumpSpeed = ReadDouble(data, "ScannerJumpSpeed", 100.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOnDelay = ReadInt(data, "LaserOnDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOffDelay = ReadInt(data, "LaserOffDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_MarkDelay = ReadInt(data, "MarkDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_JumpDelay = ReadInt(data, "JumpDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_PolygonDelay = ReadInt(data, "PolygonDelay", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_Drilling_Power = ReadDouble(data, "DrillingPower", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_P2PDistance = ReadDouble(data, "P2PDistance", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetition = (short)ReadInt(data, "DrillingRepetation", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetitionBundle = (short)ReadInt(data, "DrillingRepetitionBundle", 100);
                Equipment.stLayerRecipeSet[i].Miscellaneous_RotationAngleArc = ReadDouble(data, "RotationAngleArc", 360.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_CircleStartAngleCircle1time = ReadDouble(data, "RotationStartAngle_Circle1time", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_MaskIndex = ReadInt(data, "MaskIndex", 4);
                Equipment.stLayerRecipeSet[i].Miscellaneous_BETPositionIndex = ReadInt(data, "BETPositionIndex", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleProcessingType = ReadInt(data, "HoleProcessingType", 0);
                //Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialAlignType = ReadInt(data, "FiducialAlignType", 0);
                //Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialMarkType = ReadInt(data, "FiducialMarkType", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortByDistance_Use = ReadBool(data, "HoleSortByDistance_Use", true);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortingDistance = ReadDouble(data, "HoleDataSortingDistance", 0.5);

                Equipment.stLayerRecipeSet[i].ProcessOption_SocketAlign_Use = ReadBool(data, "Socket_Align_Use", true);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use = ReadBool(data, "Socket_HeightCheck_Use", true);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetX = ReadDouble(data, "Socket_HeightCheckPos_OffsetX", 0.0);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetY = ReadDouble(data, "Socket_HeightCheckPos_OffsetY", 0.0);
                
                Equipment.stLayerRecipeSet[i].ProcessOption_GoldPowderAlign_Use = ReadBool(data, "GoldPowder_Use", false);

                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Width = ReadDouble(data, "Module_Width", 125.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Height = ReadDouble(data, "Module_Height", 120.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness = ReadDouble(data, "Module_SiliconThickness", 0.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Thickness = ReadDouble(data, "Module_GoldPowderThickness", 0.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Percent = ReadDouble(data, "Module_GoldPowderPercent", 75.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Limit = ReadDouble(data, "Module_GoldPowderLimit", 0.05);

                Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter = ReadDouble(data, "Spiral_OuterDiameter", 0.0);
                Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter = ReadDouble(data, "Spiral_InnerDiameter", 0.0);
                Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions = ReadInt(data, "Spiral_Revolutions", 10);
                Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor = ReadDouble(data, "Spiral_AngleFactor", 10.0);

                Equipment.stLayerRecipeSet[i].EPRO_ModuleAbsorptionLevel = ReadDouble(data, "EPRO_ModuleAbsorptionLevel", -20.0);

                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Ignore = ReadBool(data, "MAlignerVacuumUse_Ignore", false);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center = ReadBool(data, "MAlignerVacuumUse_Center", true);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner = ReadBool(data, "MAlignerVacuumUse_Inner", false);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer = ReadBool(data, "MAlignerVacuumUse_Outer", false);

                Equipment.stLayerRecipeSet[i].DustCollectorRemoteMode_Use = ReadBool(data, "DustCollector_RemoteMode_Use", false);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Upper = ReadDouble(data, "DustCollector_Frequency_Upper", 20.0);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Lower = ReadDouble(data, "DustCollector_Frequency_Lower", 20.0);
                Equipment.stLayerRecipeSet[i].DustCollectorLower_Disable = ReadBool(data, "DustCollector_Lower_Disable", false);
                Equipment.stLayerRecipeSet[i].CalfileOffsetZAxismm = ReadDouble(data, "ZCalFile_OffsetZ", 0.0);
                Equipment.stLayerRecipeSet[i].ChuckMSL_Enable = ReadBool(data, "ChuckMSL_Use", false);
                Equipment.stLayerRecipeSet[i].Align3Point_Enable = ReadBool(data, "Align3Point_Enable", false);

                //  Marking Template
                Equipment.stLayerRecipeSet[i].MarkingData_SiriusTemplate_Use = ReadBool(data, "MarkingData_SiriusTemplate_Use", false);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_DataType = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_DataType", 0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Width = ReadDouble(data, "MarkingData_SiriusTemplate_EntityData_Width", 5.0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Height = ReadDouble(data, "MarkingData_SiriusTemplate_EntityData_Height", 5.0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_TextType = ReadBool(data, "MarkingData_SiriusTemplate_EntityData_TextType", true);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_PrefixData = ReadValue(data, "MarkingData_SiriusTemplate_EntityData_PrefixData", "");
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_StartNumber = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_StartNumber", 1);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Digits = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_Digits", 3);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_IncreaseStep = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_IncreaseStep", 1);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SuffixData = ReadValue(data, "MarkingData_SiriusTemplate_EntityData_SuffixData", "");
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Use = ReadBool(data, "MarkingData_SiriusTemplate_Hatch_Use", false);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Spacing = ReadDouble(data, "MarkingData_SiriusTemplate_Hatch_Spacing", 0.1);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SerialNumberIncreaseType = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_SerialNumberType_IncreaseType", 0);
            }
            
            return true;
        }
        public void Recipe_Data_Refresh(string m_strLayerName)
        {
            //  해당 Layer 의 데이터로 변경하여 표시
            int nIndex = -1;
            //  Hole 인지?
            string m_strLayer = m_strLayerName.Length > 4 ? m_strLayerName.Substring(0, 4) : m_strLayerName;

            //if (m_strLayerName == "Hole1")
            if (m_strLayer == "Hole")                   //  Layer 가 Hole 이면?
            {
                //  Hole 로 시작하는 Layer 이면, 뒤에 숫자를 가져온다.
                string m_strHoleLayer_Number = m_strLayerName.Substring(4);

                if (IsNumeric(m_strHoleLayer_Number))
                {
                    int m_nHoleLayer_Index = Equipment.ToInt(m_strHoleLayer_Number);
                    if ((m_nHoleLayer_Index >= 1) && (m_nHoleLayer_Index <= 50))
                    {
                        nIndex = m_nHoleLayer_Index - 1;             //  Hole Layer 의 Index 는 0부터 시작
                    }
                    else
                    {
                        nIndex = (int)LayerList.Hole1;
                    }
                }
            }
            #region 간소화
            //else if (m_strLayerName == "Hole2")
            //{
            //    m_nIndex = (int)LayerList.Hole2;
            //}
            //else if (m_strLayerName == "Hole3")
            //{
            //    m_nIndex = (int)LayerList.Hole3;
            //}
            //else if (m_strLayerName == "Hole4")
            //{
            //    m_nIndex = (int)LayerList.Hole4;
            //}
            //else if (m_strLayerName == "Hole5")
            //{
            //    m_nIndex = (int)LayerList.Hole5;
            //}
            //else if (m_strLayerName == "Hole6")
            //{
            //    m_nIndex = (int)LayerList.Hole6;
            //}
            //else if (m_strLayerName == "Hole7")
            //{
            //    m_nIndex = (int)LayerList.Hole7;
            //}
            //else if (m_strLayerName == "Hole8")
            //{
            //    m_nIndex = (int)LayerList.Hole8;
            //}
            //else if (m_strLayerName == "Hole9")
            //{
            //    m_nIndex = (int)LayerList.Hole9;
            //}
            //else if (m_strLayerName == "Hole10")
            //{
            //    m_nIndex = (int)LayerList.Hole10;
            //}
            #endregion
            else if (m_strLayerName == "Rect")
            {
                nIndex = (int)LayerList.Rect;
            }
            else if (m_strLayerName == "Outline")
            {
                nIndex = (int)LayerList.Outline;
            }
            else if (m_strLayerName == "Marking")
            {
                nIndex = (int)LayerList.Marking;
            }
            else if (m_strLayerName == "Fiducial")
            {
                nIndex = (int)LayerList.Fiducial;
            }
            else if (m_strLayerName == "Thruhole")
            {
                nIndex = (int)LayerList.Thruhole;
            }
            else if (m_strLayerName == "PreAlign")
            {
                nIndex = (int)LayerList.PreAlign;
            }

            if (nIndex < 0)
            {

                Log.Write("SLD-200", Equipment.User_Name, "Recipe_Data_Refresh - Fail.");
                return;
            }

            // m_nIndex 여기서 Hole Index 내부에 data가 0이거나 없으면.. 
            // 값을 넣어줘야함.
            // 신규로 Layer가 만들어졌을때.
            // 신규 Hole 레이어(값 비었으면) → 이전 Hole 데이터 복사
            //if ((m_strLayer == "Hole" && m_nIndex > (int)LayerList.Hole1) &&
            //    m_strLayerName == "Outline" && m_strLayerName == "Marking" &&
            //    m_strLayerName == "Thruhole")
            //{

            //}

            //  Laser Parameter
            if (Equipment.stLayerRecipeSet[nIndex].LaserParam_Frequency <= 0 ||
                Equipment.stLayerRecipeSet[nIndex].LaserParam_PulseWidth <= 0)
            {
                if (Equipment.stLayerRecipeSet[nIndex].LaserParam_Frequency <= 0 ||
                    Equipment.stLayerRecipeSet[nIndex].LaserParam_PulseWidth <= 0)
                {
                    Equipment.stLayerRecipeSet[nIndex].LaserParam_PulseWidth = Equipment.stLayerRecipeSet[nIndex - 1].LaserParam_PulseWidth;
                    Equipment.stLayerRecipeSet[nIndex].LaserParam_PulsePeriod = Equipment.stLayerRecipeSet[nIndex - 1].LaserParam_PulsePeriod;
                    Equipment.stLayerRecipeSet[nIndex].LaserParam_Frequency = Equipment.stLayerRecipeSet[nIndex - 1].LaserParam_Frequency;
                    Equipment.stLayerRecipeSet[nIndex].LaserParam_DutyCycle = Equipment.stLayerRecipeSet[nIndex - 1].LaserParam_DutyCycle;
                    Equipment.stLayerRecipeSet[nIndex].ProcessPriority_P2P = Equipment.stLayerRecipeSet[nIndex - 1].ProcessPriority_P2P;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_ReferenceLayer = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_ReferenceLayer;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_DefocusingDistance = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_DefocusingDistance;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_Resizing = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_Resizing;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_HoleDrilling_StartPosDivision = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_HoleDrilling_StartPosDivision;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_GroupSplitSize = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_GroupSplitSize;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_GroupSplitSize_Height = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_GroupSplitSize_Height;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_ScannerDrillingSpeed = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_ScannerDrillingSpeed;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_ScannerJumpSpeed = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_ScannerJumpSpeed;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_LaserOnDelay = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_LaserOnDelay;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_LaserOffDelay = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_LaserOffDelay;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_MarkDelay = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_MarkDelay;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_JumpDelay = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_JumpDelay;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_PolygonDelay = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_PolygonDelay;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_Drilling_Power = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_Drilling_Power;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_DrillingRepetition = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_DrillingRepetition;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_DrillingRepetitionBundle = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_DrillingRepetitionBundle;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_RotationAngleArc = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_RotationAngleArc;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_CircleStartAngleCircle1time = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_CircleStartAngleCircle1time;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_P2PDistance = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_P2PDistance;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_MaskIndex = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_MaskIndex;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_BETPositionIndex = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_BETPositionIndex;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_HoleProcessingType = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_HoleProcessingType;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_HoleSortByDistance_Use = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_HoleSortByDistance_Use;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_HoleSortingDistance = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_HoleSortingDistance;
                    Equipment.stLayerRecipeSet[nIndex].ProcessOption_SocketAlign_Use = Equipment.stLayerRecipeSet[nIndex - 1].ProcessOption_SocketAlign_Use;
                    Equipment.stLayerRecipeSet[nIndex].ProcessOption_SocketHeightCheck_Use = Equipment.stLayerRecipeSet[nIndex - 1].ProcessOption_SocketHeightCheck_Use;
                    Equipment.stLayerRecipeSet[nIndex].ProcessOption_SocketHeightCheckPos_OffsetX = Equipment.stLayerRecipeSet[nIndex - 1].ProcessOption_SocketHeightCheckPos_OffsetX;
                    Equipment.stLayerRecipeSet[nIndex].ProcessOption_SocketHeightCheckPos_OffsetY = Equipment.stLayerRecipeSet[nIndex - 1].ProcessOption_SocketHeightCheckPos_OffsetY;
                    Equipment.stLayerRecipeSet[nIndex].ProcessOption_GoldPowderAlign_Use = Equipment.stLayerRecipeSet[nIndex - 1].ProcessOption_GoldPowderAlign_Use;
                    Equipment.stLayerRecipeSet[nIndex].ModuleInformation_Module_Width = Equipment.stLayerRecipeSet[nIndex - 1].ModuleInformation_Module_Width;
                    Equipment.stLayerRecipeSet[nIndex].ModuleInformation_Module_Height = Equipment.stLayerRecipeSet[nIndex - 1].ModuleInformation_Module_Height;
                    Equipment.stLayerRecipeSet[nIndex].ModuleInformation_Silicon_Thickness = Equipment.stLayerRecipeSet[nIndex - 1].ModuleInformation_Silicon_Thickness;
                    Equipment.stLayerRecipeSet[nIndex].ModuleInformation_GoldPowder_Thickness = Equipment.stLayerRecipeSet[nIndex - 1].ModuleInformation_GoldPowder_Thickness;
                    Equipment.stLayerRecipeSet[nIndex].ModuleInformation_GoldPowder_Percent = Equipment.stLayerRecipeSet[nIndex - 1].ModuleInformation_GoldPowder_Percent;
                    Equipment.stLayerRecipeSet[nIndex].ModuleInformation_GoldPowder_Limit = Equipment.stLayerRecipeSet[nIndex - 1].ModuleInformation_GoldPowder_Limit;
                    Equipment.stLayerRecipeSet[nIndex].SpiralParam_OuterDiameter = Equipment.stLayerRecipeSet[nIndex - 1].SpiralParam_OuterDiameter;
                    Equipment.stLayerRecipeSet[nIndex].SpiralParam_InnerDiameter = Equipment.stLayerRecipeSet[nIndex - 1].SpiralParam_InnerDiameter;
                    Equipment.stLayerRecipeSet[nIndex].SpiralParam_Revolutions = Equipment.stLayerRecipeSet[nIndex - 1].SpiralParam_Revolutions;
                    Equipment.stLayerRecipeSet[nIndex].SpiralParam_AngleFactor = Equipment.stLayerRecipeSet[nIndex - 1].SpiralParam_AngleFactor;
                    Equipment.stLayerRecipeSet[nIndex].EPRO_ModuleAbsorptionLevel = Equipment.stLayerRecipeSet[nIndex - 1].EPRO_ModuleAbsorptionLevel;
                    Equipment.stLayerRecipeSet[nIndex].MAligner_VacuumPos_Ignore = Equipment.stLayerRecipeSet[nIndex - 1].MAligner_VacuumPos_Ignore;
                    Equipment.stLayerRecipeSet[nIndex].MAligner_VacuumPos_Center = Equipment.stLayerRecipeSet[nIndex - 1].MAligner_VacuumPos_Center;
                    Equipment.stLayerRecipeSet[nIndex].MAligner_VacuumPos_Inner = Equipment.stLayerRecipeSet[nIndex - 1].MAligner_VacuumPos_Inner;
                    Equipment.stLayerRecipeSet[nIndex].MAligner_VacuumPos_Outer = Equipment.stLayerRecipeSet[nIndex - 1].MAligner_VacuumPos_Outer;
                    Equipment.stLayerRecipeSet[nIndex].DustCollectorRemoteMode_Use = Equipment.stLayerRecipeSet[nIndex - 1].DustCollectorRemoteMode_Use;
                    Equipment.stLayerRecipeSet[nIndex].DustCollectorFreq_Upper = Equipment.stLayerRecipeSet[nIndex - 1].DustCollectorFreq_Upper;
                    Equipment.stLayerRecipeSet[nIndex].DustCollectorFreq_Lower = Equipment.stLayerRecipeSet[nIndex - 1].DustCollectorFreq_Lower;
                    Equipment.stLayerRecipeSet[nIndex].DustCollectorLower_Disable = Equipment.stLayerRecipeSet[nIndex - 1].DustCollectorLower_Disable;
                    Equipment.stLayerRecipeSet[nIndex].MarkingData_SiriusTemplate_Use = Equipment.stLayerRecipeSet[nIndex - 1].MarkingData_SiriusTemplate_Use;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_DataType = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_DataType;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_TextType = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_TextType;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_PrefixData = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_PrefixData;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_StartNumber = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_StartNumber;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_Digits = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_Digits;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_IncreaseStep = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_IncreaseStep;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_SuffixData = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_SuffixData;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_Hatch_Use = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_Hatch_Use;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_Hatch_Spacing = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_Hatch_Spacing;
                    Equipment.stLayerRecipeSet[nIndex].MarkingTemplate_EntityData_SerialNumberIncreaseType = Equipment.stLayerRecipeSet[nIndex - 1].MarkingTemplate_EntityData_SerialNumberIncreaseType;
                    Equipment.stLayerRecipeSet[nIndex].CalfileOffsetZAxismm = Equipment.stLayerRecipeSet[nIndex - 1].CalfileOffsetZAxismm;
                    Equipment.stLayerRecipeSet[nIndex].ChuckMSL_Enable = Equipment.stLayerRecipeSet[nIndex - 1].ChuckMSL_Enable;
                    Equipment.stLayerRecipeSet[nIndex].Align3Point_Enable = Equipment.stLayerRecipeSet[nIndex - 1].Align3Point_Enable;
                    Equipment.stLayerRecipeSet[nIndex].Miscellaneous_BETPositionIndex = Equipment.stLayerRecipeSet[nIndex - 1].Miscellaneous_BETPositionIndex;
                    switch (Equipment.stLayerRecipeSet[nIndex].Miscellaneous_BETPositionIndex)
                    {
                        case 0:
                            workStage.m_dBET_ZoomValue_Recipe = 0.8;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_8X_Mrad;
                            break;

                        case 1:
                            workStage.m_dBET_ZoomValue_Recipe = 0.9;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_9X_Mrad;
                            break;

                        case 2:
                            workStage.m_dBET_ZoomValue_Recipe = 1.0;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_0X_Mrad;
                            break;

                        case 3:
                            workStage.m_dBET_ZoomValue_Recipe = 1.1;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_1X_Mrad;
                            break;

                        case 4:
                            workStage.m_dBET_ZoomValue_Recipe = 1.2;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_2X_Mrad;
                            break;
                    }
                }
            }

            textBox_Recipe_TabRecipe_LaserParam_Frequency.Text = Equipment.stLayerRecipeSet[nIndex].LaserParam_Frequency.ToString();
            textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text = Equipment.stLayerRecipeSet[nIndex].LaserParam_PulseWidth.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[nIndex].LaserParam_PulsePeriod.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[nIndex].LaserParam_DutyCycle.ToString();

            //  Process Priority
            if (Equipment.stLayerRecipeSet[nIndex].ProcessPriority_P2P)
            {
                radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked = true;

                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = false;
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = false;
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = false;
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = true;
            }
            else
            {
                radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod.Checked = true;

                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = true;
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = true;
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = true;
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = false;
            }

            //  Miscellaneous
            textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_ReferenceLayer;
            textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_DefocusingDistance.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_Resizing.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_HoleDrilling_StartPosDivision.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_GroupSplitSize.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_GroupSplitSize_Height.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_ScannerDrillingSpeed.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_ScannerJumpSpeed.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_LaserOnDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_LaserOffDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_MarkDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_JumpDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_PolygonDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_Drilling_Power.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_DrillingRepetition.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_DrillingRepetitionBundle.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_RotationAngleArc.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_CircleStartAngleCircle1time.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text = Equipment.stLayerRecipeSet[nIndex].Miscellaneous_P2PDistance.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[nIndex].Miscellaneous_MaskIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[nIndex].Miscellaneous_HoleProcessingType.ToString());
            checkBox_Recipe_TabRecipe_Miscellaneous_HoleDrillingOrder_SortByDistance.Checked = Equipment.stLayerRecipeSet[0].Miscellaneous_HoleSortByDistance_Use;
            textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_HoleSortingDistance.ToString();

            //  process Options
            checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use;                         //  Socket Align 기능 사용 여부
            checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use;             //  Socket Height Check 기능 사용 여부
            textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetX.Text = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetX.ToString();
            textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetY.Text = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetY.ToString();

            checkBox_Recipe_TabRecipe_ProcessOptions_GoldPowderAlign.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_GoldPowderAlign_Use;

            //  Module Information
            textBox_Recipe_TabRecipe_ModuleInformation_Width.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_Height.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Silicon_Thickness.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderThickness.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Thickness.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderPercent.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Percent.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderLimit.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Limit.ToString();

            //  Spiral Parameter
            textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = Equipment.stLayerRecipeSet[nIndex].SpiralParam_OuterDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = Equipment.stLayerRecipeSet[nIndex].SpiralParam_InnerDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = Equipment.stLayerRecipeSet[nIndex].SpiralParam_Revolutions.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = Equipment.stLayerRecipeSet[nIndex].SpiralParam_AngleFactor.ToString();

            //  EPRO Module Absorption Level
            textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel.Text = Equipment.stLayerRecipeSet[0].EPRO_ModuleAbsorptionLevel.ToString();

            //  M-Aligner Vacuum Use
            checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Ignore;         //  Ignore
            checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center;     //  Center
            checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner;       //  Inner
            checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer;       //  Outer

            //  집진기 주파수
            checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;                          //  집진기 Remote Mode 사용 여부
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();
            checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked = Equipment.stLayerRecipeSet[0].DustCollectorLower_Disable;

            //  Marking Template
            checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingData_SiriusTemplate_Use;
            comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_DataType.ToString());

            if (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_TextType)
            {
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked = true;

                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = false;
            }
            else
            {
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked = true;

                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = true;
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
            }
            textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_PrefixData;
            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_StartNumber.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Digits.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_IncreaseStep.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SuffixData;

            switch (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType)
            {
                case (int)WorkStage.nSerialNumber_IncreaseType.forEachModule:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
                    break;

                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked = true;
                    break;

                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Checked = true;
                    break;
            }
            if (comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex == 0)            //  True Type Font 일 때만 Hatch 활성화
            {
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;

                if (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Hatch_Use)
                {
                    checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;
                }
                else
                {
                    checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked = false;
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
                }
            }
            else                                                                                //  그 외에는 비활성화
            {
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
            }

            textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Hatch_Spacing.ToString();

            //m_nIndex <- 이거 먹나?
            richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text = Equipment.stLayerRecipeSet[nIndex].CalfileOffsetZAxismm.ToString();

            //  Chuck MSL 사용 여부
            checkBox_Recipe_TabRecipe_ChuckMSL_Enable.Checked = Equipment.stLayerRecipeSet[0].ChuckMSL_Enable;

            //  3-Point Align 사용 여부
            checkBox_Recipe_TabRecipe_3PointAlign_Enable.Checked = Equipment.stLayerRecipeSet[0].Align3Point_Enable;

            
        }


        //  RecipeManager 사용을 위한 함수
        #region UI Refresh Helper

        /// <summary>
        /// 레시피 로드 직후, 화면에 모든 값을 반영한다 (단일 진입점).
        /// - recipeFilePath: Label에 표시할 레시피 파일 경로(파일명만 사용)
        /// </summary>
        private void RefreshUIAfterRecipeOpen(string recipeFilePath)
        {
            // 1) 상단 파일명 표시
            label_Recipe_FileName.Text = Path.GetFileName(recipeFilePath);

            // 2) 공용/글로벌 영역 즉시 반영 (CommonData + Layer[0] 기반)
            //   - Dust Collector / Remote 모드 / 주파수 / 하부 집진기 비활성
            checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked =
                Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text =
                Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text =
                Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();
            checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked =
                Equipment.stLayerRecipeSet[0].DustCollectorLower_Disable;

            //   - Chuck MSL / 3점 얼라인 / Z-Offset
            checkBox_Recipe_TabRecipe_ChuckMSL_Enable.Checked = Equipment.stLayerRecipeSet[0].ChuckMSL_Enable;
            checkBox_Recipe_TabRecipe_3PointAlign_Enable.Checked = Equipment.stLayerRecipeSet[0].Align3Point_Enable;
            richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text =
                Equipment.stLayerRecipeSet[0].CalfileOffsetZAxismm.ToString();

            // 3) 마킹 템플릿(Barcode/TTF/Serial 등) 전체 바인딩
            checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked =
                Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingData_SiriusTemplate_Use;
            comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex =
                Equipment.ToInt(Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_DataType.ToString());

            bool isFixedText = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_TextType;
            radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked = isFixedText;
            radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked = !isFixedText;

            // 고정/시리얼에 따른 Enable/Disable
            bool serialEnabled = !isFixedText;
            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = serialEnabled;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = serialEnabled;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = serialEnabled;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = serialEnabled;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Enabled = serialEnabled;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Enabled = serialEnabled;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Enabled = serialEnabled;
            button_Marking_SerialNumber_CountReset.Enabled = serialEnabled;

            // 시리얼 증가 타입 반영
            switch (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType)
            {
                case (int)WorkStage.nSerialNumber_IncreaseType.forEachModule:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
                    break;
                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked = true;
                    break;
                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Checked = true;
                    break;
            }

            textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text =
                Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_PrefixData;
            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text =
                Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_StartNumber.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text =
                Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Digits.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text =
                Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_IncreaseStep.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text =
                Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SuffixData;

            // 4) Miscellaneous(가공/지연/마스크/BET/P2P 등) 전역값 먼저 반영
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_HoleDrilling_StartPosDivision.ToString();

            textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize.ToString();

            // Height가 0이면 Width 사용 (기존 로직 유지)
            if (Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize_Height == 0)
                Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize_Height = Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize;

            textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize_Height.ToString();

            textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_ScannerDrillingSpeed.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_ScannerJumpSpeed.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_LaserOnDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_LaserOffDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_MarkDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_JumpDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_PolygonDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_Drilling_Power.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_DrillingRepetition.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_DrillingRepetitionBundle.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_RotationAngleArc.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_CircleStartAngleCircle1time.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text =
                Equipment.stLayerRecipeSet[0].Miscellaneous_P2PDistance.ToString();

            comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex =
                Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_MaskIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex =
                Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString());

            // BET Zoom 변경 시 Mrad 라벨 자동 갱신(기존 이벤트 로직 재사용)
            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex_SelectedIndexChanged(
                comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex, EventArgs.Empty);

            // 5) 도면 경로 표시 + 도면 Import + 레이어 리스트 구성
            richTextBox_Recipe_TabRecipe_DrawingFile.Text = Equipment.stLayerRecipeSet[0].DrawingFile;
            ImportDrawingAndBuildLayerList(richTextBox_Recipe_TabRecipe_DrawingFile.Text);

            // 6) 첫 레이어 선택해서 (기존 SelectedIndexChanged 로직을) 자동 실행
            if (listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Count > 0)
            {
                listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex = -1;
                listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex = 0;
            }

            // 7) 자동/수동 상태에 따른 버튼 Enable (기존 OnShowRecipeForm 규칙 유지)
            if (Equipment.AutoManualStatus)
            {
                button_Recipe_New.Enabled = false;
                button_Recipe_Open.Enabled = false;
                button_Recipe_Apply.Enabled = false;
                button_Recipe_Save.Enabled = false;
                button_Recipe_SaveAs.Enabled = false;
                button_Recipe_TabRecipe_OpenEditor.Enabled = false;
                button_Recipe_TabRecipe_OpenDwg.Enabled = false;
                button_Recipe_TabRecipe_LayerImport.Enabled = false;
            }
            else
            {
                button_Recipe_New.Enabled = true;
                button_Recipe_Open.Enabled = true;
                button_Recipe_Apply.Enabled = true;
                button_Recipe_Save.Enabled = true;
                button_Recipe_SaveAs.Enabled = true;
                button_Recipe_TabRecipe_OpenEditor.Enabled = true;
                button_Recipe_TabRecipe_OpenDwg.Enabled = true;
                button_Recipe_TabRecipe_LayerImport.Enabled = true;
            }
        }

        /// <summary>
        /// 도면 파일을 Sirius 문서로 열고, Markerable Layer를 리스트에 채운다.
        /// (버튼 핸들러의 기존 로직을 그대로 재사용)
        /// </summary>
        private void ImportDrawingAndBuildLayerList(string drawingPath)
        {
            // 레이어 리스트 초기화
            listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Clear();

            if (string.IsNullOrWhiteSpace(drawingPath))
                return;
            if (!File.Exists(drawingPath))
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Error !!", "도면 파일이 없습니다.");
                return;
            }

            string ext = Path.GetExtension(drawingPath).ToUpperInvariant();
            if (ext == ".DXF")
            {
                var doc = DocumentSerializer.OpenDxf(drawingPath);
                m_formSiriusEditor.SiriusEditor.Document.Views.Clear();
                m_formSiriusEditor.SiriusEditor.Document = doc;
            }
            else if (ext == ".SIRIUS")
            {
                var doc = DocumentSerializer.OpenSirius(drawingPath);
                m_formSiriusEditor.SiriusEditor.Document.Views.Clear();
                m_formSiriusEditor.SiriusEditor.Document = doc;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !!", "도면 파일이 아닙니다.\r\n\r\n[available  *.sirius, *.dxf]");
                return;
            }

            // 장비 공유 Viewer Document에 세팅 + 드릴링 데이터 파싱 + 레이어 목록 채우기
            Equipment.SetEqpSiriusViewerDocument(m_formSiriusEditor.SiriusEditor.Document);
            if (workStage.DrillingData_Parsing())
            {
                foreach (var layer in m_formSiriusEditor.SiriusEditor.Document.Layers)
                {
                    if (layer.IsMarkerable)
                        listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Add(layer.Name);
                }
            }
        }
        #endregion



        //Multy - Recipe를 위한 함수
        public void RefreshLayerUI(string layerName)
        {
            var result = RecipeManager.Instance.GetLayerRecipeData(layerName);
            if (result.Index < 0)
                return;

            var layerData = result.LayerData;
            var CommonData = result.CommonData;

            //  Laser Parameter
            textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text = layerData.LaserParam_PulseWidth.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = layerData.LaserParam_PulsePeriod.ToString();
            textBox_Recipe_TabRecipe_LaserParam_Frequency.Text = layerData.LaserParam_Frequency.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = layerData.LaserParam_DutyCycle.ToString();

            //  Process Priority
            if (layerData.ProcessPriority_P2P)
            {
                radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked = true;

                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = false;
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = false;
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = false;
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = true;
            }
            else
            {
                radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod.Checked = true;

                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = true;
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = true;
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = true;
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = false;
            }

            //  Miscellaneous
            textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer.Text = layerData.Miscellaneous_ReferenceLayer;
            textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text = layerData.Miscellaneous_DefocusingDistance.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text = layerData.Miscellaneous_Resizing.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text = layerData.Miscellaneous_HoleDrilling_StartPosDivision.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text = layerData.Miscellaneous_GroupSplitSize.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text = layerData.Miscellaneous_GroupSplitSize_Height.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text = layerData.Miscellaneous_ScannerDrillingSpeed.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text = layerData.Miscellaneous_ScannerJumpSpeed.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text = layerData.Miscellaneous_LaserOnDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text = layerData.Miscellaneous_LaserOffDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text = layerData.Miscellaneous_MarkDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text = layerData.Miscellaneous_JumpDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text = layerData.Miscellaneous_PolygonDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text = layerData.Miscellaneous_Drilling_Power.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text = layerData.Miscellaneous_DrillingRepetition.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text = layerData.Miscellaneous_DrillingRepetitionBundle.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text = layerData.Miscellaneous_RotationAngleArc.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text = layerData.Miscellaneous_CircleStartAngleCircle1time.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text = layerData.Miscellaneous_P2PDistance.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex = Equipment.ToInt(layerData.Miscellaneous_MaskIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex = Equipment.ToInt(layerData.Miscellaneous_HoleProcessingType.ToString());

            //  Spiral Parameter
            textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = layerData.SpiralParam_OuterDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = layerData.SpiralParam_InnerDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = layerData.SpiralParam_Revolutions.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = layerData.SpiralParam_AngleFactor.ToString();


            // CommonData ///

            //  Miscellaneous
            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex = Equipment.ToInt(CommonData.Miscellaneous_BETPositionIndex.ToString());
            checkBox_Recipe_TabRecipe_Miscellaneous_HoleDrillingOrder_SortByDistance.Checked = CommonData.Miscellaneous_HoleSortByDistance_Use;
            textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance.Text = CommonData.Miscellaneous_HoleSortingDistance.ToString();

            //  process Options
            checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign.Checked = CommonData.ProcessOption_SocketAlign_Use;                         //  Socket Align 기능 사용 여부
            checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck.Checked = CommonData.ProcessOption_SocketHeightCheck_Use;             //  Socket Height Check 기능 사용 여부
            textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetX.Text = CommonData.ProcessOption_SocketHeightCheckPos_OffsetX.ToString();
            textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetY.Text = CommonData.ProcessOption_SocketHeightCheckPos_OffsetY.ToString();
            checkBox_Recipe_TabRecipe_ProcessOptions_GoldPowderAlign.Checked = CommonData.ProcessOption_GoldPowderAlign_Use;

            //  Module Information
            textBox_Recipe_TabRecipe_ModuleInformation_Width.Text = CommonData.ModuleInformation_Module_Width.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_Height.Text = CommonData.ModuleInformation_Module_Height.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text = CommonData.ModuleInformation_Silicon_Thickness.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderThickness.Text = CommonData.ModuleInformation_GoldPowder_Thickness.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderPercent.Text = CommonData.ModuleInformation_GoldPowder_Percent.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderLimit.Text = CommonData.ModuleInformation_GoldPowder_Limit.ToString();

            //  EPRO Module Absorption Level
            textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel.Text = CommonData.EPRO_ModuleAbsorptionLevel.ToString();

            //  M-Aligner Vacuum Use
            checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked = CommonData.MAligner_VacuumPos_Ignore;     //  Ignore
            checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = CommonData.MAligner_VacuumPos_Center;     //  Center
            checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = CommonData.MAligner_VacuumPos_Inner;       //  Inner
            checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = CommonData.MAligner_VacuumPos_Outer;       //  Outer

            //  집진기 주파수
            checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = CommonData.DustCollectorRemoteMode_Use;                          //  집진기 Remote Mode 사용 여부
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = CommonData.DustCollectorFreq_Upper.ToString();
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = CommonData.DustCollectorFreq_Lower.ToString();
            checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked = CommonData.DustCollectorLower_Disable;

            richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text = CommonData.CalfileOffsetZAxismm.ToString();
            checkBox_Recipe_TabRecipe_ChuckMSL_Enable.Checked = CommonData.ChuckMSL_Enable;          //  Chuck MSL 사용 여부
            checkBox_Recipe_TabRecipe_3PointAlign_Enable.Checked = CommonData.Align3Point_Enable;    //  3-Point Align 사용 여부

            //  Marking Template
            checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingData_SiriusTemplate_Use;
            comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_DataType.ToString());
            //textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Width.ToString();
            //textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Height.ToString();
            if (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_TextType)
            {
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = false;
            }
            else
            {
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = true;
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
            }

            textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_PrefixData;
            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_StartNumber.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Digits.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_IncreaseStep.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SuffixData;

            switch (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType)
            {
                case (int)WorkStage.nSerialNumber_IncreaseType.forEachModule:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
                    break;

                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked = true;
                    break;

                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Checked = true;
                    break;
            }
            if (comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex == 0)            //  True Type Font 일 때만 Hatch 활성화
            {
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;

                if (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Hatch_Use)
                {
                    checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;
                }
                else
                {
                    checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked = false;
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
                }
            }
            else                                                                                //  그 외에는 비활성화
            {
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
            }
            textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Hatch_Spacing.ToString();
        }

        public void Recipe_Open(string strRecipeFile)
        {
            string fileName = string.Empty;

            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = strRecipeFile;

                //  Recipe Data 로드
                Recipe_Data_Load_Refactory(fileName);
                Equipment.Current_Recipe = fileName;
                Equipment.Current_DrawingFileName = System.IO.Path.GetFileName(Equipment.stLayerRecipeSet[0].DrawingFile);

                //  Recipe 명 표시
                label_Recipe_FileName.Text = System.IO.Path.GetFileName(fileName);

                // Recipe Vision Load
                string iniPath = fileName;  //ConfigManager.GetRecipeDataPath() + "\\RecipeVisionData.ini";
                Equipment.stVisionRecipeSet = VisionRecipeData.LoadFromIni(iniPath);
                if (workStage.jigAligner_LowRes != null)
                {
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Equipment.stVisionRecipeSet.LoadTrainImage(); //Bitmap.FromFile(m_strFile);
                    workStage.jigAligner_LowRes.TrainImage = Equipment.stVisionRecipeSet.LoadTrainImage(); //이거 사용중.
                }

                //workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MaxInstance = 
                if (Equipment.stVisionRecipeSet.PrePatternMatching != null)
                {
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MaxTolerance = Equipment.stVisionRecipeSet.PrePatternMatching.MaxTolerance;
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MaxInstance = Equipment.stVisionRecipeSet.PrePatternMatching.MaxInstance;
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MinScore = Equipment.stVisionRecipeSet.PrePatternMatching.MinScore;
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.DuplicateChecked = Equipment.stVisionRecipeSet.PrePatternMatching.DuplicateChecked;
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.UseMaskImage = Equipment.stVisionRecipeSet.PrePatternMatching.UseMaskImage;

                    if (workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage != null)
                    {
                        workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Equipment.stVisionRecipeSet.LoadTrainImage().GetImage();
                    }

                    workStage.jigAligner_LowRes.Recipe.TrainRoiStartLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiStartLocation;
                    workStage.jigAligner_LowRes.Recipe.TrainRoiEndLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiEndLocation;
                    workStage.jigAligner_LowRes.Recipe.InspectRoiStartLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiStartLocation;
                    workStage.jigAligner_LowRes.Recipe.InspectRoiEndLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiEndLocation;
                }

                //  Recipe 창에 데이터 표시
                //  Drawing File
                richTextBox_Recipe_TabRecipe_DrawingFile.Text = Equipment.stLayerRecipeSet[0].DrawingFile;

                //  Laser Parameter
                textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text = Equipment.stLayerRecipeSet[0].LaserParam_PulseWidth.ToString();
                textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[0].LaserParam_PulsePeriod.ToString();
                textBox_Recipe_TabRecipe_LaserParam_Frequency.Text = Equipment.stLayerRecipeSet[0].LaserParam_Frequency.ToString();
                textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[0].LaserParam_DutyCycle.ToString();
                //if (Equipment.stLayerRecipeSet[0].LaserParam_TriggerMode_External)
                //{
                //    radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_External.Checked = true;
                //}
                //else
                //{
                //    radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_Internal.Checked = true;
                //}

                //  Process Priority
                if (Equipment.stLayerRecipeSet[0].ProcessPriority_P2P)
                {
                    radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked = true;

                    textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = false;
                    textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = false;
                    textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = false;
                    textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = true;
                }
                else
                {
                    radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod.Checked = true;

                    textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = true;
                    textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = true;
                    textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = true;
                    textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = false;
                }

                //  Miscellaneous
                textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_ReferenceLayer;
                textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_DefocusingDistance.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_Resizing.ToString();
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_HoleDrilling_StartPosDivision.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize.ToString();

                //  Scan Field Height Size 가 0일 경우, Width 값을 사용한다.
                if (Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize_Height == 0)
                {
                    Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize_Height = Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize;
                }
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize_Height.ToString();

                textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_ScannerDrillingSpeed.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_ScannerJumpSpeed.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_LaserOnDelay.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_LaserOffDelay.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_MarkDelay.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_JumpDelay.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_PolygonDelay.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_Drilling_Power.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_DrillingRepetition.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_DrillingRepetitionBundle.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_RotationAngleArc.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_CircleStartAngleCircle1time.ToString();
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_P2PDistance.ToString();
                //comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_MaskIndex.ToString();
                //comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString();
                comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_MaskIndex.ToString());
                comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString());
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_HoleProcessingType.ToString());
                //comboBox_Recipe_TabRecipe_Miscellaneous_FiducialAlignType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialAlignType.ToString());
                //comboBox_Recipe_TabRecipe_Miscellaneous_FiducialMarkType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialMarkType.ToString());
                checkBox_Recipe_TabRecipe_Miscellaneous_HoleDrillingOrder_SortByDistance.Checked = Equipment.stLayerRecipeSet[0].Miscellaneous_HoleSortByDistance_Use;
                textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_HoleSortingDistance.ToString();

                //  process Options
                checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use;                         //  Socket Align 기능 사용 여부
                checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use;             //  Socket Height Check 기능 사용 여부
                textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetX.Text = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetX.ToString();
                textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetY.Text = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetY.ToString();

                checkBox_Recipe_TabRecipe_ProcessOptions_GoldPowderAlign.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_GoldPowderAlign_Use;

                //  Module Information
                textBox_Recipe_TabRecipe_ModuleInformation_Width.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width.ToString();
                textBox_Recipe_TabRecipe_ModuleInformation_Height.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height.ToString();
                textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Silicon_Thickness.ToString();
                textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderThickness.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Thickness.ToString();
                textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderPercent.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Percent.ToString();
                textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderLimit.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Limit.ToString();

                //  Spiral Parameter
                textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_OuterDiameter.ToString();
                textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_InnerDiameter.ToString();
                textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = Equipment.stLayerRecipeSet[0].SpiralParam_Revolutions.ToString();
                textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = Equipment.stLayerRecipeSet[0].SpiralParam_AngleFactor.ToString();

                //  EPRO Module Absorption Level
                textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel.Text = Equipment.stLayerRecipeSet[0].EPRO_ModuleAbsorptionLevel.ToString();

                //  M-Aligner Vacuum Use
                checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Ignore;         //  Ignore
                checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center;     //  Center
                checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner;       //  Inner
                checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer;       //  Outer

                //  집진기 주파수
                checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;                          //  집진기 Remote Mode 사용 여부
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();
                checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked = Equipment.stLayerRecipeSet[0].DustCollectorLower_Disable;                                        //  하부 집진기 사용 여부

                //  Marking Template
                checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingData_SiriusTemplate_Use;
                comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_DataType.ToString());
                //textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Width.ToString();
                //textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Height.ToString();
                if (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_TextType)
                {
                    radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked = true;

                    textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;

                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Enabled = false;
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Enabled = false;
                    button_Marking_SerialNumber_CountReset.Enabled = false;
                }
                else
                {
                    radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked = true;

                    textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = true;

                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Enabled = true;
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Enabled = true;
                    button_Marking_SerialNumber_CountReset.Enabled = true;
                }

                switch (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType)
                {
                    case (int)WorkStage.nSerialNumber_IncreaseType.forEachModule:
                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
                        break;

                    case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket:
                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked = true;
                        break;

                    case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous:
                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Checked = true;
                        break;
                }
                textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_PrefixData;
                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_StartNumber.ToString();
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Digits.ToString();
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_IncreaseStep.ToString();
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SuffixData;
                if (comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex == 0)            //  True Type Font 일 때만 Hatch 활성화
                {
                    checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;

                    if (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Hatch_Use)
                    {
                        checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked = true;

                        textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;
                    }
                    else
                    {
                        checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked = false;

                        textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
                    }
                }
                else                                                                                //  그 외에는 비활성화
                {
                    checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = false;
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
                }
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Hatch_Spacing.ToString();

                //  BET, Mrad
                switch (Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex)
                {
                    case 0:             //  BET 0.8X
                        workStage.m_dBET_ZoomValue_Recipe = 0.8;
                        workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_8X_Mrad;
                        label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_0_8X_Mrad.ToString();
                        break;

                    case 1:             //  BET 0.9X
                        workStage.m_dBET_ZoomValue_Recipe = 0.9;
                        workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_9X_Mrad;
                        label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_0_9X_Mrad.ToString();
                        break;

                    case 2:             //  BET 1.0X
                        workStage.m_dBET_ZoomValue_Recipe = 1.0;
                        workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_0X_Mrad;
                        label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_0X_Mrad.ToString();
                        break;

                    case 3:             //  BET 1.1X
                        workStage.m_dBET_ZoomValue_Recipe = 1.1;
                        workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_1X_Mrad;
                        label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_1X_Mrad.ToString();
                        break;

                    case 4:             //  BET 1.2X
                        workStage.m_dBET_ZoomValue_Recipe = 1.2;
                        workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_2X_Mrad;
                        label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_2X_Mrad.ToString();
                        break;
                }

                if (workStage.m_beamExpander_Comm != null)
                {
                    if (workStage.m_beamExpander_Comm.IsOpen)
                    {
                        //  Recipe 에 설정된 BET 설정과 현재 설정이 다르면 변경
                        if ((workStage.m_dBET_ZoomValue < (workStage.m_dBET_ZoomValue_Recipe - 0.005)) || (workStage.m_dBET_ZoomValue > (workStage.m_dBET_ZoomValue_Recipe + 0.005)))
                        {
                            workStage.BeamExpander_Send_Motor_SetPosition((int)WorkStage.nMotorizedBET.ZoomMotor, workStage.m_dBET_ZoomValue_Recipe);
                            Thread.Sleep(500);
                        }

                        if ((workStage.m_dBET_MradValue < (workStage.m_dBET_MradValue_Recipe - 0.005)) || (workStage.m_dBET_MradValue > (workStage.m_dBET_MradValue_Recipe + 0.005)))
                        {
                            workStage.BeamExpander_Send_Motor_SetPosition((int)WorkStage.nMotorizedBET.BeamExpansionMotor, workStage.m_dBET_MradValue_Recipe);
                        }
                    }
                }

                switch (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType)
                {
                    case (int)WorkStage.nSerialNumber_IncreaseType.forEachModule:
                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
                        break;

                    case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket:
                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked = true;
                        break;

                    case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous:
                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Checked = true;
                        break;
                }

                //  Z-Axis Offset mm
                richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text = Equipment.stLayerRecipeSet[0].CalfileOffsetZAxismm.ToString();

                //  Chuck MSL 사용 여부
                checkBox_Recipe_TabRecipe_ChuckMSL_Enable.Checked = Equipment.stLayerRecipeSet[0].ChuckMSL_Enable;
                //  3-Point Align 사용 여부
                checkBox_Recipe_TabRecipe_3PointAlign_Enable.Checked = Equipment.stLayerRecipeSet[0].Align3Point_Enable;

                int m_nCount = 0;
                do
                {
                    m_nCount++;
                } while (m_nCount < 1000);

                //  도면 Import
                m_formSiriusEditor.Import_DrawingFile(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                Equipment.SetEqpSiriusViewerDocument(m_formSiriusEditor.SiriusEditor.Document);
                //Equipment.EqpSiriusViewer_Origin.Document = m_formSiriusEditor.SiriusEditor.Document;
                //  자동운전 중 모듈 가공 시 이 위치의 도면파일을 로드한다.
                Equipment.RecipeOpen_DrawingFilePath = richTextBox_Recipe_TabRecipe_DrawingFile.Text;
                if (workStage.DrillingData_Parsing())
                {
                    //  Layer List 전체 삭제
                    listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Clear();
                    // Todo : 20250426 확인
                    if (m_formSiriusEditor.SiriusEditor.Document != null)
                    {
                        foreach (var layer in m_formSiriusEditor.SiriusEditor.Document.Layers)
                        {
                            if (layer.IsMarkerable)
                            {
                                listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Add(layer.Name);
                            }
                        }
                    }

                    // 다른 곳 사용시!!! 아래 switch 구문 messagebox Log 등으로 수정 필요.!
                    // 선택 가공을 위해 Drilling Data Parsing 도 해준다.
                    var mb = new MessageBoxOk();
                    //int m_nReturn = workStage.GetDrillingData();
                    int m_nReturn = workStage.GetDrillingData(true);
                    switch (m_nReturn)
                    {
                        case (int)WorkStage.nGetDataResult.GETDATA_SUCCESS:

                            workStage.DrillingManager.CycleTimer_LaserDrilling.Clear();
                            workStage.DrillingManager.CycleTimer_LaserDrilling.TotalElapsed = TimeSpan.Zero;
                            workStage.DrillingManager.CycleTimer_TargetModuleCount = 0;
                            workStage.DrillingManager.CycleTimer_DoneModuleCount = 0;
                            workStage.DrillingManager.CycleTimer_NGModuleCount = 0;
                            workStage.DrillingManager.CycleTimer_DoneSocketCount = 0;
                            workStage.DrillingManager.CycleTimer_NGSocketCount = 0;

                            //  Hole1 제외한 나머지 Layer 의 Socket 을 가공할 것인지 여부를 결정하는 Flag 세팅
                            workStage.GetDrillingData_ProcessingFlagCheck();
                            mb.ShowDialog("Information !!", "가공 데이터 Parsing 성공 및 Recipe Data를 로드 성공.");
                            break;

                        case (int)WorkStage.nGetDataResult.GETDATA_FAIL:
                            mb.ShowDialog("Error !!", "데이터가 정상적으로 로드 되지 않았습니다.");
                            break;

                        case (int)WorkStage.nGetDataResult.GETDATA_NOT_GROUP:
                            mb.ShowDialog("Error !!", "데이터가 Group 이 아닙니다.");
                            break;

                        case (int)WorkStage.nGetDataResult.GETDATA_UNGROUP:
                            mb.ShowDialog("Error !!", "데이터를 Group 해제 해야 합니다.");
                            break;

                        case (int)WorkStage.nGetDataResult.GETDATA_LAYERNAME_NG:
                            mb.ShowDialog("Error !!", "Layer Name 은 'Hole1~4', 'Rect', 'Outline', 'Marking', 'Fiducial' 5가지만 가능합니다.");
                            break;

                        case (int)WorkStage.nGetDataResult.GETDATA_MOTIONTYPE_NG:
                            mb.ShowDialog("Error !!", "Layer Motion Type 은 'StageAndScanner', 'ScannerOnly' 2가지만 가능합니다.");
                            break;

                        case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NG:
                            mb.ShowDialog("Error !!", "Drilling Data 는 Polyline, Rectangle, Line, Circle, Arc 중 한 가지 데이터로만 구성되어야 합니다.");
                            break;

                        case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_LINECNT:
                            mb.ShowDialog("Error !!", "Drilling Data 에 Line 데이터 개수가 4의 배수가 아닙니다.");
                            break;

                        case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NOT_CLOSED:
                            mb.ShowDialog("Error !!", "Line 으로 이루어진 Drilling Data 가 닫힌 도형이 아닙니다.");
                            break;

                        case (int)WorkStage.nGetDataResult.GETDATA_RTCINIT:
                            mb.ShowDialog("Error !!", "RTC 보드가 초기화 되지 않았습니다.");
                            break;
                    }
                }
                else
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Error !!", "Recipe Data를 로드하지 못했습니다.");
                }

                if (listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Count > 0)
                {
                    listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex = -1;  // 선택 해제
                    listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex = 0;   // 다시 선택 → 이벤트 발생

                }

                workStage.ResetProcess();

            }

            return;
        }

        private string ReadValue(Dictionary<string, string> data, string key, string defaultValue)
    => data.TryGetValue(key, out var value) ? value : defaultValue;
        private int ReadInt(Dictionary<string, string> data, string key, int defaultValue)
            => int.TryParse(ReadValue(data, key, defaultValue.ToString()), out var result) ? result : defaultValue;
        private double ReadDouble(Dictionary<string, string> data, string key, double defaultValue)
            => double.TryParse(ReadValue(data, key, defaultValue.ToString()), out var result) ? result : defaultValue;
        private bool ReadBool(Dictionary<string, string> data, string key, bool defaultValue)
            => bool.TryParse(ReadValue(data, key, defaultValue.ToString()), out var result) ? result : defaultValue;
        public bool IsNumeric(string input)
        {
            return double.TryParse(input, out _);
        }
        
        //Save
        public void Recipe_Data_Save(string m_strRecipeFile)
        {
            string strTemp = "";

            string strFIle = "";
            //strFIle = ConfigManager.GetRecipeDataPath() + "\\LDUL_TeachingPosition.ini";
            strFIle = m_strRecipeFile;

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);

                strTemp = string.Format("{0} 파일을 생성하였습니다. 다시 시도하십시오.", System.IO.Path.GetFileName(strFIle));
                MessageBox.Show(strTemp, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

                //생성하고 바로 열어서 쓰면 저장 안됨. 여기서는...
                //if (File.Exists(strFIle) == false)
                //{
                //    strTemp = string.Format("{0} 파일을 읽지 못하였습니다. 다시 시도하십시오.", System.IO.Path.GetFileName(strFIle));
                //    MessageBox.Show(strTemp, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
            }

            //  Recipe Parameter 저장
            for ( int i = 0; i < (int)System.Enum.GetValues(typeof(LayerList)).Length; i++)
            {
                strTemp = string.Format("Layer_{0}", i);

                //  Recipe File Path and Name
                NativeMethods.WritePrivateProfileString(strTemp, "Drawing_File_Name", Equipment.stLayerRecipeSet[i].DrawingFile, strFIle);

                //  Laser Pulse Width (us)
                NativeMethods.WritePrivateProfileString(strTemp, "Pulse_Width", Equipment.stLayerRecipeSet[i].LaserParam_PulseWidth.ToString(), strFIle);
                //  Laser Pulse Period (us)
                NativeMethods.WritePrivateProfileString(strTemp, "Pulse_Period", Equipment.stLayerRecipeSet[i].LaserParam_PulsePeriod.ToString(), strFIle);
                //  Laser Frequency (Hz)
                NativeMethods.WritePrivateProfileString(strTemp, "Frequency", Equipment.stLayerRecipeSet[i].LaserParam_Frequency.ToString(), strFIle);
                //  Duty Cycle (%)
                NativeMethods.WritePrivateProfileString(strTemp, "Duty_Cycle", Equipment.stLayerRecipeSet[i].LaserParam_DutyCycle.ToString(), strFIle);

                //  Laser Trigger Mode (true: External, false: Internal)
                NativeMethods.WritePrivateProfileString(strTemp, "Trigger_Mode_External", Equipment.stLayerRecipeSet[i].LaserParam_TriggerMode_External.ToString(), strFIle);

                //  Process Priority (true: Space of P2P, false: Pulse Period)
                NativeMethods.WritePrivateProfileString(strTemp, "P2P", Equipment.stLayerRecipeSet[i].ProcessPriority_P2P.ToString(), strFIle);

                //  Reference Layer
                NativeMethods.WritePrivateProfileString(strTemp, "Reference_Layer", Equipment.stLayerRecipeSet[i].Miscellaneous_ReferenceLayer, strFIle);
                //  Defocusing Distance (mm)
                NativeMethods.WritePrivateProfileString(strTemp, "Defocusing_Distance", Equipment.stLayerRecipeSet[i].Miscellaneous_DefocusingDistance.ToString(), strFIle);
                //  Resizing (mm)
                NativeMethods.WritePrivateProfileString(strTemp, "Resizing", Equipment.stLayerRecipeSet[i].Miscellaneous_Resizing.ToString(), strFIle);
                //  Hole Drilling Start Position Division (등분)
                NativeMethods.WritePrivateProfileString(strTemp, "HoleDrilling_StartPosDivision", Equipment.stLayerRecipeSet[i].Miscellaneous_HoleDrilling_StartPosDivision.ToString(), strFIle);
                //  Group Split Size (mm)
                NativeMethods.WritePrivateProfileString(strTemp, "GroupSplitSize", Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize.ToString(), strFIle);
                //  Group Split Size - Height (mm)
                NativeMethods.WritePrivateProfileString(strTemp, "GroupSplitSize_Height", Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize_Height.ToString(), strFIle);
                //  Scanner Drilling Speed (mm/s)
                NativeMethods.WritePrivateProfileString(strTemp, "ScannerDrillingSpeed", Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerDrillingSpeed.ToString(), strFIle);
                //  Scanner Jump Speed (mm/s)
                NativeMethods.WritePrivateProfileString(strTemp, "ScannerJumpSpeed", Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerJumpSpeed.ToString(), strFIle);
                //  Laser On Delay (us)
                NativeMethods.WritePrivateProfileString(strTemp, "LaserOnDelay", Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOnDelay.ToString(), strFIle);
                //  Laser Off Delay (us)
                NativeMethods.WritePrivateProfileString(strTemp, "LaserOffDelay", Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOffDelay.ToString(), strFIle);
                //  Mark Delay (us)
                NativeMethods.WritePrivateProfileString(strTemp, "MarkDelay", Equipment.stLayerRecipeSet[i].Miscellaneous_MarkDelay.ToString(), strFIle);
                //  Jump Delay (us)
                NativeMethods.WritePrivateProfileString(strTemp, "JumpDelay", Equipment.stLayerRecipeSet[i].Miscellaneous_JumpDelay.ToString(), strFIle);
                //  Polygon Delay (us)
                NativeMethods.WritePrivateProfileString(strTemp, "PolygonDelay", Equipment.stLayerRecipeSet[i].Miscellaneous_PolygonDelay.ToString(), strFIle);
                //  Drilling Power (%)
                NativeMethods.WritePrivateProfileString(strTemp, "DrillingPower", Equipment.stLayerRecipeSet[i].Miscellaneous_Drilling_Power.ToString(), strFIle);
                //  Space of P2P Distance (mm)
                NativeMethods.WritePrivateProfileString(strTemp, "P2PDistance", Equipment.stLayerRecipeSet[i].Miscellaneous_P2PDistance.ToString(), strFIle);
                //  Drilling Repetation
                NativeMethods.WritePrivateProfileString(strTemp, "DrillingRepetation", Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetition.ToString(), strFIle);
                //  Drilling Repetition Bundle
                NativeMethods.WritePrivateProfileString(strTemp, "DrillingRepetitionBundle", Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetitionBundle.ToString(), strFIle);
                //  Rotation Angle when Arc
                NativeMethods.WritePrivateProfileString(strTemp, "RotationAngleArc", Equipment.stLayerRecipeSet[i].Miscellaneous_RotationAngleArc.ToString(), strFIle);
                //  Rotation Start Angle when Circle Processing 1 time
                NativeMethods.WritePrivateProfileString(strTemp, "RotationStartAngle_Circle1time", Equipment.stLayerRecipeSet[i].Miscellaneous_CircleStartAngleCircle1time.ToString(), strFIle);
                //  Mask Index (0:None, 1:Mask1, 2:Mask2, 3:Mask3, 4:Mask4)
                NativeMethods.WritePrivateProfileString(strTemp, "MaskIndex", Equipment.stLayerRecipeSet[i].Miscellaneous_MaskIndex.ToString(), strFIle);
                //  BET Position Index (0:0.8x, 1:0.9x, 2:1.0x, 3:1.1x, 4:1.2x)
                NativeMethods.WritePrivateProfileString(strTemp, "BETPositionIndex", Equipment.stLayerRecipeSet[i].Miscellaneous_BETPositionIndex.ToString(), strFIle);
                //  Hole Processing Type (0:Circle, 1:Spiral)
                NativeMethods.WritePrivateProfileString(strTemp, "HoleProcessingType", Equipment.stLayerRecipeSet[i].Miscellaneous_HoleProcessingType.ToString(), strFIle);
                //  Fiducial Align Type (0:Circle Find, 1:Pattern Matching)
                //NativeMethods.WritePrivateProfileString(strTemp, "FiducialAlignType", Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialAlignType.ToString(), strFIle);
                //  Fiducial Mark Type (0:Circle, 1:Gold Powder)
                //NativeMethods.WritePrivateProfileString(strTemp, "FiducialMarkType", Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialMarkType.ToString(), strFIle);
                //  Hole Data Sort by Distance Use
                NativeMethods.WritePrivateProfileString(strTemp, "HoleSortByDistance_Use", Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortByDistance_Use.ToString(), strFIle);
                //  Hole Data Sorting Distance
                NativeMethods.WritePrivateProfileString(strTemp, "HoleDataSortingDistance", Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortingDistance.ToString(), strFIle);

                //  Process Options
                NativeMethods.WritePrivateProfileString(strTemp, "Socket_Align_Use", Equipment.stLayerRecipeSet[i].ProcessOption_SocketAlign_Use.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Socket_HeightCheck_Use", Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Socket_HeightCheckPos_OffsetX", Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetX.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Socket_HeightCheckPos_OffsetY", Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetY.ToString(), strFIle);

                NativeMethods.WritePrivateProfileString(strTemp, "GoldPowder_Use", Equipment.stLayerRecipeSet[i].ProcessOption_GoldPowderAlign_Use.ToString(), strFIle);

                //  Module Information  
                NativeMethods.WritePrivateProfileString(strTemp, "Module_Width", Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Width.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Module_Height", Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Height.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Module_SiliconThickness", Equipment.stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Module_GoldPowderThickness", Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Thickness.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Module_GoldPowderPercent", Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Percent.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Module_GoldPowderLimit", Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Limit.ToString(), strFIle);


                //  Spiral Parameter
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_OuterDiameter", Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_InnerDiameter", Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_Revolutions", Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_AngleFactor", Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor.ToString(), strFIle);

                //  EPRO Module Absorption Level
                NativeMethods.WritePrivateProfileString(strTemp, "EPRO_ModuleAbsorptionLevel", Equipment.stLayerRecipeSet[i].EPRO_ModuleAbsorptionLevel.ToString(), strFIle);

                //  M-Aligner Vacuum Use
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Ignore", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Ignore.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Center", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Inner", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Outer", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer.ToString(), strFIle);

                //  Dust Collector
                NativeMethods.WritePrivateProfileString(strTemp, "DustCollector_RemoteMode_Use", Equipment.stLayerRecipeSet[i].DustCollectorRemoteMode_Use.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "DustCollector_Frequency_Upper", Equipment.stLayerRecipeSet[i].DustCollectorFreq_Upper.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "DustCollector_Frequency_Lower", Equipment.stLayerRecipeSet[i].DustCollectorFreq_Lower.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "DustCollector_Lower_Disable", Equipment.stLayerRecipeSet[i].DustCollectorLower_Disable.ToString(), strFIle);

                //  Marking Template
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_Use", Equipment.stLayerRecipeSet[i].MarkingData_SiriusTemplate_Use.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_DataType", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_DataType.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_Width", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Width.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_Height", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Height.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_TextType", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_TextType.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_PrefixData", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_PrefixData.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_StartNumber", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_StartNumber.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_Digits", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Digits.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_IncreaseStep", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_IncreaseStep.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_SuffixData", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SuffixData.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_Hatch_Use", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Use.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_Hatch_Spacing", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Spacing.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_SerialNumberType_IncreaseType", Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SerialNumberIncreaseType.ToString(), strFIle);
                
                //  ZCalFile Offset Z Axis (mm)
                NativeMethods.WritePrivateProfileString(strTemp, "ZCalFile_OffsetZ", Equipment.stLayerRecipeSet[i].CalfileOffsetZAxismm.ToString(), strFIle);
                //  Chuck MSL Use
                NativeMethods.WritePrivateProfileString(strTemp, "ChuckMSL_Use", Equipment.stLayerRecipeSet[i].ChuckMSL_Enable.ToString(), strFIle);
                //  Align 3 Point Use
                NativeMethods.WritePrivateProfileString(strTemp, "Align3Point_Enable", Equipment.stLayerRecipeSet[i].Align3Point_Enable.ToString(), strFIle);
            }
        }
        public void Recipe_Data_Save_Refactory(string strRecipeFile)
        {
            if (string.IsNullOrWhiteSpace(strRecipeFile))
                return;

            if (!File.Exists(strRecipeFile))
            {
                // 파일이 없으면 먼저 생성만 하고 리턴
                using (FileStream fs = File.Create(strRecipeFile)) { }

                string strTemp = string.Format("{0} 파일을 생성하였습니다. 다시 시도하십시오.", System.IO.Path.GetFileName(strRecipeFile));
                MessageBox.Show(strTemp, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //PreAlign Param
            var iniData = new Dictionary<string, Dictionary<string, string>>();
            int layerCount = (int)System.Enum.GetValues(typeof(LayerList)).Length;

            for (int i = 0; i < layerCount; i++)
            {
                string section = $"Layer_{i}";
                var layerDict = new Dictionary<string, string>();

                layerDict["Drawing_File_Name"] = Equipment.stLayerRecipeSet[i].DrawingFile;
                layerDict["Pulse_Width"] = Equipment.stLayerRecipeSet[i].LaserParam_PulseWidth.ToString();
                layerDict["Pulse_Period"] = Equipment.stLayerRecipeSet[i].LaserParam_PulsePeriod.ToString();
                layerDict["Frequency"] = Equipment.stLayerRecipeSet[i].LaserParam_Frequency.ToString();
                layerDict["Duty_Cycle"] = Equipment.stLayerRecipeSet[i].LaserParam_DutyCycle.ToString();
                layerDict["Trigger_Mode_External"] = Equipment.stLayerRecipeSet[i].LaserParam_TriggerMode_External.ToString();
                layerDict["P2P"] = Equipment.stLayerRecipeSet[i].ProcessPriority_P2P.ToString();

                layerDict["Reference_Layer"] = Equipment.stLayerRecipeSet[i].Miscellaneous_ReferenceLayer;
                layerDict["Defocusing_Distance"] = Equipment.stLayerRecipeSet[i].Miscellaneous_DefocusingDistance.ToString();
                layerDict["Resizing"] = Equipment.stLayerRecipeSet[i].Miscellaneous_Resizing.ToString();
                layerDict["HoleDrilling_StartPosDivision"] = Equipment.stLayerRecipeSet[i].Miscellaneous_HoleDrilling_StartPosDivision.ToString();
                layerDict["GroupSplitSize"] = Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize.ToString();
                layerDict["GroupSplitSize_Height"] = Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize_Height.ToString();
                layerDict["ScannerDrillingSpeed"] = Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerDrillingSpeed.ToString();
                layerDict["ScannerJumpSpeed"] = Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerJumpSpeed.ToString();
                layerDict["LaserOnDelay"] = Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOnDelay.ToString();
                layerDict["LaserOffDelay"] = Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOffDelay.ToString();
                layerDict["MarkDelay"] = Equipment.stLayerRecipeSet[i].Miscellaneous_MarkDelay.ToString();
                layerDict["JumpDelay"] = Equipment.stLayerRecipeSet[i].Miscellaneous_JumpDelay.ToString();
                layerDict["PolygonDelay"] = Equipment.stLayerRecipeSet[i].Miscellaneous_PolygonDelay.ToString();
                layerDict["DrillingPower"] = Equipment.stLayerRecipeSet[i].Miscellaneous_Drilling_Power.ToString();
                layerDict["P2PDistance"] = Equipment.stLayerRecipeSet[i].Miscellaneous_P2PDistance.ToString();
                layerDict["DrillingRepetation"] = Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetition.ToString();
                layerDict["DrillingRepetitionBundle"] = Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetitionBundle.ToString();
                layerDict["RotationAngleArc"] = Equipment.stLayerRecipeSet[i].Miscellaneous_RotationAngleArc.ToString();
                layerDict["RotationStartAngle_Circle1time"] = Equipment.stLayerRecipeSet[i].Miscellaneous_CircleStartAngleCircle1time.ToString();
                layerDict["MaskIndex"] = Equipment.stLayerRecipeSet[i].Miscellaneous_MaskIndex.ToString();
                layerDict["BETPositionIndex"] = Equipment.stLayerRecipeSet[i].Miscellaneous_BETPositionIndex.ToString();
                layerDict["HoleProcessingType"] = Equipment.stLayerRecipeSet[i].Miscellaneous_HoleProcessingType.ToString();
                //layerDict["FiducialAlignType"] = Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialAlignType.ToString();
                //layerDict["FiducialMarkType"] = Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialMarkType.ToString();
                layerDict["HoleSortByDistance_Use"] = Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortByDistance_Use.ToString();
                layerDict["HoleDataSortingDistance"] = Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortingDistance.ToString();

                layerDict["Socket_Align_Use"] = Equipment.stLayerRecipeSet[i].ProcessOption_SocketAlign_Use.ToString();
                layerDict["Socket_HeightCheck_Use"] = Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use.ToString();
                layerDict["Socket_HeightCheckPos_OffsetX"] = Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetX.ToString();
                layerDict["Socket_HeightCheckPos_OffsetY"] = Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetY.ToString();

                layerDict["GoldPowder_Use"] = Equipment.stLayerRecipeSet[i].ProcessOption_GoldPowderAlign_Use.ToString();

                layerDict["Module_Width"] = Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Width.ToString();
                layerDict["Module_Height"] = Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Height.ToString();
                layerDict["Module_SiliconThickness"] = Equipment.stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness.ToString();
                layerDict["Module_GoldPowderThickness"] = Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Thickness.ToString();
                layerDict["Module_GoldPowderPercent"] = Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Percent.ToString();
                layerDict["Module_GoldPowderLimit"] = Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Limit.ToString();

                layerDict["Spiral_OuterDiameter"] = Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter.ToString();
                layerDict["Spiral_InnerDiameter"] = Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter.ToString();
                layerDict["Spiral_Revolutions"] = Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions.ToString();
                layerDict["Spiral_AngleFactor"] = Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor.ToString();

                layerDict["EPRO_ModuleAbsorptionLevel"] = Equipment.stLayerRecipeSet[i].EPRO_ModuleAbsorptionLevel.ToString();

                layerDict["MAlignerVacuumUse_Ignore"] = Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Ignore.ToString();
                layerDict["MAlignerVacuumUse_Center"] = Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center.ToString();
                layerDict["MAlignerVacuumUse_Inner"] = Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner.ToString();
                layerDict["MAlignerVacuumUse_Outer"] = Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer.ToString();

                layerDict["DustCollector_RemoteMode_Use"] = Equipment.stLayerRecipeSet[i].DustCollectorRemoteMode_Use.ToString();
                layerDict["DustCollector_Frequency_Upper"] = Equipment.stLayerRecipeSet[i].DustCollectorFreq_Upper.ToString();
                layerDict["DustCollector_Frequency_Lower"] = Equipment.stLayerRecipeSet[i].DustCollectorFreq_Lower.ToString();
                layerDict["DustCollector_Lower_Disable"] = Equipment.stLayerRecipeSet[i].DustCollectorLower_Disable.ToString();

                //  Marking Template
                layerDict["MarkingData_SiriusTemplate_Use"] = Equipment.stLayerRecipeSet[i].MarkingData_SiriusTemplate_Use.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_DataType"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_DataType.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_Width"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Width.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_Height"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Height.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_TextType"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_TextType.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_PrefixData"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_PrefixData.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_StartNumber"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_StartNumber.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_Digits"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Digits.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_IncreaseStep"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_IncreaseStep.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_SuffixData"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SuffixData.ToString();
                layerDict["MarkingData_SiriusTemplate_Hatch_Use"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Use.ToString();
                layerDict["MarkingData_SiriusTemplate_Hatch_Spacing"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Spacing.ToString();
                layerDict["MarkingData_SiriusTemplate_EntityData_SerialNumberType_IncreaseType"] = Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SerialNumberIncreaseType.ToString();

                layerDict["ZCalFile_OffsetZ"] = Equipment.stLayerRecipeSet[i].CalfileOffsetZAxismm.ToString();
                layerDict["ChuckMSL_Use"] = Equipment.stLayerRecipeSet[i].ChuckMSL_Enable.ToString();
                layerDict["Align3Point_Enable"] = Equipment.stLayerRecipeSet[i].Align3Point_Enable.ToString();

                iniData[section] = layerDict;
            }

            SaveIniFile_Fast(strRecipeFile, iniData);
        }
        private void SaveIniFile_Fast(string filePath, Dictionary<string, Dictionary<string, string>> iniData)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            // 1. 백업용 폴더 준비
            string folder = Path.GetDirectoryName(filePath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
            string backupFolder = Path.Combine(folder, "Backup");

            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
            }

            // 2. 백업 파일 이름 만들기
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss"); // 년월일_시분초
            string backupFilePath = Path.Combine(backupFolder, $"{fileNameWithoutExt}_{timestamp}.ini");

            // 3. 저장할 내용 만들기
            StringBuilder sb = new StringBuilder();

            foreach (var section in iniData)
            {
                sb.AppendLine($"[{section.Key}]");

                foreach (var kvp in section.Value)
                {
                    sb.AppendLine($"{kvp.Key}={kvp.Value}");
                }

                sb.AppendLine(); // 섹션 간 줄 띄움
            }

            string content = sb.ToString();

            // Todo : WriteAllText :: 함수 안정성에 대하여 검증 필요. ( 기능은 구현됨 )
            // 4. 먼저 백업 파일 저장
            File.WriteAllText(backupFilePath, content, Encoding.UTF8);

            // 5. 정상 파일 저장
            File.WriteAllText(filePath, content, Encoding.UTF8);
            //File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
        #endregion

        private void button_Recipe_New_Click(object sender, EventArgs e)
        {
            string folderPath = string.Empty;
            string defaultRecipeFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Recipe");
            if (string.IsNullOrWhiteSpace(Equipment.Current_Recipe))
            {
                // 기본 폴더 사용
                folderPath = defaultRecipeFolder;
            }
            else
            {
                folderPath = Path.GetDirectoryName(Equipment.Current_Recipe);
            }

            // 폴더 없으면 생성
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 새 파일명 설정
            string ext = ".ini";  // 확장자 필요시 조정
            string fileName = Path.Combine(folderPath, "NewRecipe" + ext);
            {
                if (File.Exists(fileName) == false)
                {
                    using (FileStream fs = File.Create(fileName))
                    {
                        // 파일만 생성하고 바로 닫음
                    }
                }

                if (bTestRecipe)
                {
                    // 초기 스켈레톤 저장 후 곧바로 열기
                    RecipeManager.Instance.SaveRecipe(fileName);
                    RecipeManager.Instance.OpenRecipe(fileName);

                    // 비전/레이어/UI 반영
                    RefreshUIAfterRecipeOpen(fileName);

                    new MessageBoxOk().ShowDialog("Information !!", "Recipe Data를 새로 생성하였습니다.");
                }
                else
                {
                    //  Recipe Data 저장
                    Recipe_Data_Save_Refactory(fileName);
                    Equipment.Current_Recipe = fileName;
                    // Vision Data 저장
                    stVisionRecipeSet.SaveToIni(fileName);
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !!", "Recipe Data를 새로 생성하였습니다.");
                    Recipe_Open(fileName); // Recipe Open
                }
                    
            }
        }

        private void button_Recipe_Open_Click(object sender, EventArgs e)
        {
            string filePath = "";
            string fileName = "";
            
            if (Equipment.GetEqpSiriusViewer() == null)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !!", "먼저 Scanner Board 를 초기화 해야 합니다.");
                return;
            }

            if (workStage.rtc == null)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !!", "먼저 Scanner Board 를 초기화 해야 합니다.");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Recipe Data Path";

            if (Equipment.RecipeFilePath.Length > 0)
            {
                openFileDialog.InitialDirectory = Equipment.RecipeFilePath;
            }
            else
            {
                openFileDialog.InitialDirectory = ConfigManager.GetRecipeDataPath();
            }
            
            openFileDialog.Filter = "Recipe File(*.ini)|*.ini";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if(bTestRecipe)
                {
                    fileName = openFileDialog.FileName;

                    // 기존: Recipe_Data_Load_Refactory(fileName);
                    RecipeManager.Instance.OpenRecipe(fileName);

                    // 기존 흐름 유지: 장비 상태 라벨/비전/레이어 목록 등 UI 반영
                    RefreshUIAfterRecipeOpen(fileName);
                }
                else
                {
                    fileName = openFileDialog.FileName;
                    //  Recipe Data 로드
                    Recipe_Data_Load_Refactory(fileName);
                    Equipment.Current_Recipe = fileName;
                    Equipment.Current_DrawingFileName = System.IO.Path.GetFileName(Equipment.stLayerRecipeSet[0].DrawingFile);

                    // Recipe Vision Load
                    string iniPath = fileName;  //ConfigManager.GetRecipeDataPath() + "\\RecipeVisionData.ini";
                    Equipment.stVisionRecipeSet = VisionRecipeData.LoadFromIni(iniPath);
                    if (workStage.jigAligner_LowRes != null)
                    {
                        workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Equipment.stVisionRecipeSet.LoadTrainImage(); //Bitmap.FromFile(m_strFile);
                        workStage.jigAligner_LowRes.TrainImage = Equipment.stVisionRecipeSet.LoadTrainImage(); //이거 사용중.
                    }
                    //workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MaxInstance = 
                    if (Equipment.stVisionRecipeSet.PrePatternMatching != null)
                    {
                        workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MaxTolerance = Equipment.stVisionRecipeSet.PrePatternMatching.MaxTolerance;
                        workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MaxInstance = Equipment.stVisionRecipeSet.PrePatternMatching.MaxInstance;
                        workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.MinScore = Equipment.stVisionRecipeSet.PrePatternMatching.MinScore;
                        workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.DuplicateChecked = Equipment.stVisionRecipeSet.PrePatternMatching.DuplicateChecked;
                        workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.UseMaskImage = Equipment.stVisionRecipeSet.PrePatternMatching.UseMaskImage;

                        if (workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage != null)
                        {
                            workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Equipment.stVisionRecipeSet.LoadTrainImage().GetImage();
                        }

                        workStage.jigAligner_LowRes.Recipe.TrainRoiStartLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiStartLocation;
                        workStage.jigAligner_LowRes.Recipe.TrainRoiEndLocation = Equipment.stVisionRecipeSet.pointPreTrainRoiEndLocation;
                        workStage.jigAligner_LowRes.Recipe.InspectRoiStartLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiStartLocation;
                        workStage.jigAligner_LowRes.Recipe.InspectRoiEndLocation = Equipment.stVisionRecipeSet.pointPreInspectRoiEndLocation;
                    }

                    //  Recipe 창에 데이터 표시
                    //  Recipe 명 표시
                    label_Recipe_FileName.Text = System.IO.Path.GetFileName(fileName);
                    //  Drawing File
                    richTextBox_Recipe_TabRecipe_DrawingFile.Text = Equipment.stLayerRecipeSet[0].DrawingFile;
                    //  Laser Parameter
                    textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text = Equipment.stLayerRecipeSet[0].LaserParam_PulseWidth.ToString();
                    textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[0].LaserParam_PulsePeriod.ToString();
                    textBox_Recipe_TabRecipe_LaserParam_Frequency.Text = Equipment.stLayerRecipeSet[0].LaserParam_Frequency.ToString();
                    textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[0].LaserParam_DutyCycle.ToString();

                    if (Equipment.stLayerRecipeSet[0].ProcessPriority_P2P)
                    {
                        radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked = true;

                        textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = false;
                        textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = false;
                        textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = false;
                        textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = true;
                    }
                    else
                    {
                        radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod.Checked = true;

                        textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = true;
                        textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = true;
                        textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = true;
                        textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = false;
                    }

                    //  Miscellaneous
                    textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_ReferenceLayer;
                    textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_DefocusingDistance.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_Resizing.ToString();
                    comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_HoleDrilling_StartPosDivision.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize.ToString();

                    //  Scan Field Height Size 가 0일 경우, Width 값을 사용한다.
                    if (Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize_Height == 0)
                    {
                        Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize_Height = Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize;
                    }
                    textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_GroupSplitSize_Height.ToString();

                    textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_ScannerDrillingSpeed.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_ScannerJumpSpeed.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_LaserOnDelay.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_LaserOffDelay.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_MarkDelay.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_JumpDelay.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_PolygonDelay.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_Drilling_Power.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_DrillingRepetition.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_DrillingRepetitionBundle.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_RotationAngleArc.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_CircleStartAngleCircle1time.ToString();
                    textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_P2PDistance.ToString();
                    //comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_MaskIndex.ToString();
                    //comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString();
                    comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_MaskIndex.ToString());
                    comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString());
                    comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_HoleProcessingType.ToString());
                    //comboBox_Recipe_TabRecipe_Miscellaneous_FiducialAlignType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialAlignType.ToString());
                    //comboBox_Recipe_TabRecipe_Miscellaneous_FiducialMarkType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialMarkType.ToString());
                    checkBox_Recipe_TabRecipe_Miscellaneous_HoleDrillingOrder_SortByDistance.Checked = Equipment.stLayerRecipeSet[0].Miscellaneous_HoleSortByDistance_Use;
                    textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_HoleSortingDistance.ToString();

                    //  process Options
                    checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use;                         //  Socket Align 기능 사용 여부
                    checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use;             //  Socket Height Check 기능 사용 여부
                    textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetX.Text = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetX.ToString();
                    textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetY.Text = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetY.ToString();

                    checkBox_Recipe_TabRecipe_ProcessOptions_GoldPowderAlign.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_GoldPowderAlign_Use;

                    //  Module Information
                    textBox_Recipe_TabRecipe_ModuleInformation_Width.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width.ToString();
                    textBox_Recipe_TabRecipe_ModuleInformation_Height.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height.ToString();
                    textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Silicon_Thickness.ToString();
                    textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderThickness.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Thickness.ToString();
                    textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderPercent.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Percent.ToString();
                    textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderLimit.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Limit.ToString();

                    //  Spiral Parameter
                    textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_OuterDiameter.ToString();
                    textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_InnerDiameter.ToString();
                    textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = Equipment.stLayerRecipeSet[0].SpiralParam_Revolutions.ToString();
                    textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = Equipment.stLayerRecipeSet[0].SpiralParam_AngleFactor.ToString();

                    //  EPRO Module Absorption Level
                    textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel.Text = Equipment.stLayerRecipeSet[0].EPRO_ModuleAbsorptionLevel.ToString();

                    //  M-Aligner Vacuum Use
                    checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Ignore;         //  Ignore
                    checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center;     //  Center
                    checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner;       //  Inner
                    checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer;       //  Outer

                    //  집진기 주파수
                    checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;                          //  집진기 Remote Mode 사용 여부
                    textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
                    textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();
                    checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked = Equipment.stLayerRecipeSet[0].DustCollectorLower_Disable;                                        //  하부 집진기 사용 여부

                    //  Marking Template
                    checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingData_SiriusTemplate_Use;
                    comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_DataType.ToString());
                    //textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Width.ToString();
                    //textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Height.ToString();
                    if (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_TextType)
                    {
                        radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked = true;

                        textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
                        textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
                        textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
                        textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;

                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Enabled = false;
                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Enabled = false;
                        button_Marking_SerialNumber_CountReset.Enabled = false;
                    }
                    else
                    {
                        radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked = true;

                        textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = true;
                        textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = true;
                        textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = true;
                        textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = true;

                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Enabled = true;
                        radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Enabled = true;
                        button_Marking_SerialNumber_CountReset.Enabled = true;
                    }

                    switch (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType)
                    {
                        case (int)WorkStage.nSerialNumber_IncreaseType.forEachModule:
                            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
                            break;

                        case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket:
                            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked = true;
                            break;

                        case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous:
                            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Checked = true;
                            break;
                    }
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_PrefixData;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_StartNumber.ToString();
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Digits.ToString();
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_IncreaseStep.ToString();
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SuffixData;
                    if (comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex == 0)            //  True Type Font 일 때만 Hatch 활성화
                    {
                        checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = true;
                        textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;

                        if (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Hatch_Use)
                        {
                            checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked = true;

                            textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;
                        }
                        else
                        {
                            checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked = false;

                            textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
                        }
                    }
                    else                                                                                //  그 외에는 비활성화
                    {
                        checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = false;
                        textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
                    }
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text = Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_Hatch_Spacing.ToString();

                    //  BET, Mrad
                    switch (Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex)
                    {
                        case 0:             //  BET 0.8X
                            workStage.m_dBET_ZoomValue_Recipe = 0.8;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_8X_Mrad;
                            label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_0_8X_Mrad.ToString();
                            break;

                        case 1:             //  BET 0.9X
                            workStage.m_dBET_ZoomValue_Recipe = 0.9;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_9X_Mrad;
                            label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_0_9X_Mrad.ToString();
                            break;

                        case 2:             //  BET 1.0X
                            workStage.m_dBET_ZoomValue_Recipe = 1.0;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_0X_Mrad;
                            label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_0X_Mrad.ToString();
                            break;

                        case 3:             //  BET 1.1X
                            workStage.m_dBET_ZoomValue_Recipe = 1.1;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_1X_Mrad;
                            label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_1X_Mrad.ToString();
                            break;

                        case 4:             //  BET 1.2X
                            workStage.m_dBET_ZoomValue_Recipe = 1.2;
                            workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_2X_Mrad;
                            label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_2X_Mrad.ToString();
                            break;
                    }

                    if (workStage.m_beamExpander_Comm != null)
                    {
                        if (workStage.m_beamExpander_Comm.IsOpen)
                        {
                            //  Recipe 에 설정된 BET 설정과 현재 설정이 다르면 변경
                            if ((workStage.m_dBET_ZoomValue < (workStage.m_dBET_ZoomValue_Recipe - 0.005)) || (workStage.m_dBET_ZoomValue > (workStage.m_dBET_ZoomValue_Recipe + 0.005)))
                            {
                                workStage.BeamExpander_Send_Motor_SetPosition((int)WorkStage.nMotorizedBET.ZoomMotor, workStage.m_dBET_ZoomValue_Recipe);
                                Thread.Sleep(500);
                            }

                            if ((workStage.m_dBET_MradValue < (workStage.m_dBET_MradValue_Recipe - 0.005)) || (workStage.m_dBET_MradValue > (workStage.m_dBET_MradValue_Recipe + 0.005)))
                            {
                                workStage.BeamExpander_Send_Motor_SetPosition((int)WorkStage.nMotorizedBET.BeamExpansionMotor, workStage.m_dBET_MradValue_Recipe);
                            }
                        }
                    }

                    switch (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType)
                    {
                        case (int)WorkStage.nSerialNumber_IncreaseType.forEachModule:
                            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
                            break;

                        case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket:
                            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked = true;
                            break;

                        case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous:
                            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Checked = true;
                            break;
                    }

                    //  Z-Axis Offset mm
                    richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text = Equipment.stLayerRecipeSet[0].CalfileOffsetZAxismm.ToString();
                    //  Chuck MSL 사용 여부
                    checkBox_Recipe_TabRecipe_ChuckMSL_Enable.Checked = Equipment.stLayerRecipeSet[0].ChuckMSL_Enable;
                    //  3-Point Align 사용 여부
                    checkBox_Recipe_TabRecipe_3PointAlign_Enable.Checked = Equipment.stLayerRecipeSet[0].Align3Point_Enable;

                    int m_nCount = 0;
                    do
                    {
                        m_nCount++;
                    } while (m_nCount < 1000);


                    //  도면 Import
                    m_formSiriusEditor.Import_DrawingFile(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                    Equipment.SetEqpSiriusViewerDocument(m_formSiriusEditor.SiriusEditor.Document);
                    //Equipment.EqpSiriusViewer_Origin.Document = m_formSiriusEditor.SiriusEditor.Document;
                    //  자동운전 중 모듈 가공 시 이 위치의 도면파일을 로드한다.
                    Equipment.RecipeOpen_DrawingFilePath = richTextBox_Recipe_TabRecipe_DrawingFile.Text;
                    if (workStage.DrillingData_Parsing())
                    {
                        //  Layer List 전체 삭제
                        listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Clear();
                        // Todo : 20250426 확인
                        if (m_formSiriusEditor.SiriusEditor.Document != null)
                        {
                            foreach (var layer in m_formSiriusEditor.SiriusEditor.Document.Layers)
                            {
                                if (layer.IsMarkerable)
                                {
                                    listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Add(layer.Name);
                                }
                            }
                        }

                        // 다른 곳 사용시!!! 아래 switch 구문 messagebox Log 등으로 수정 필요.!
                        // 선택 가공을 위해 Drilling Data Parsing 도 해준다.
                        var mb = new MessageBoxOk();
                        //int m_nReturn = workStage.GetDrillingData();
                        int m_nReturn = workStage.GetDrillingData(true);
                        switch (m_nReturn)
                        {
                            case (int)WorkStage.nGetDataResult.GETDATA_SUCCESS:

                                workStage.DrillingManager.CycleTimer_LaserDrilling.Clear();
                                workStage.DrillingManager.CycleTimer_LaserDrilling.TotalElapsed = TimeSpan.Zero;
                                workStage.DrillingManager.CycleTimer_TargetModuleCount = 0;
                                workStage.DrillingManager.CycleTimer_DoneModuleCount = 0;
                                workStage.DrillingManager.CycleTimer_NGModuleCount = 0;
                                workStage.DrillingManager.CycleTimer_DoneSocketCount = 0;
                                workStage.DrillingManager.CycleTimer_NGSocketCount = 0;

                                //  Hole1 제외한 나머지 Layer 의 Socket 을 가공할 것인지 여부를 결정하는 Flag 세팅
                                workStage.GetDrillingData_ProcessingFlagCheck();
                                mb.ShowDialog("Information !!", "가공 데이터 Parsing 성공 및 Recipe Data를 로드 성공.");
                                break;

                            case (int)WorkStage.nGetDataResult.GETDATA_FAIL:
                                mb.ShowDialog("Error !!", "데이터가 정상적으로 로드 되지 않았습니다.");
                                break;

                            case (int)WorkStage.nGetDataResult.GETDATA_NOT_GROUP:
                                mb.ShowDialog("Error !!", "데이터가 Group 이 아닙니다.");
                                break;

                            case (int)WorkStage.nGetDataResult.GETDATA_UNGROUP:
                                mb.ShowDialog("Error !!", "데이터를 Group 해제 해야 합니다.");
                                break;

                            case (int)WorkStage.nGetDataResult.GETDATA_LAYERNAME_NG:
                                mb.ShowDialog("Error !!", "Layer Name 은 'Hole1~4', 'Rect', 'Outline', 'Marking', 'Fiducial' 5가지만 가능합니다.");
                                break;

                            case (int)WorkStage.nGetDataResult.GETDATA_MOTIONTYPE_NG:
                                mb.ShowDialog("Error !!", "Layer Motion Type 은 'StageAndScanner', 'ScannerOnly' 2가지만 가능합니다.");
                                break;

                            case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NG:
                                mb.ShowDialog("Error !!", "Drilling Data 는 Polyline, Rectangle, Line, Circle, Arc 중 한 가지 데이터로만 구성되어야 합니다.");
                                break;

                            case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_LINECNT:
                                mb.ShowDialog("Error !!", "Drilling Data 에 Line 데이터 개수가 4의 배수가 아닙니다.");
                                break;

                            case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NOT_CLOSED:
                                mb.ShowDialog("Error !!", "Line 으로 이루어진 Drilling Data 가 닫힌 도형이 아닙니다.");
                                break;

                            case (int)WorkStage.nGetDataResult.GETDATA_RTCINIT:
                                mb.ShowDialog("Error !!", "RTC 보드가 초기화 되지 않았습니다.");
                                break;
                        }
                    }
                    else
                    {
                        var mb = new MessageBoxOk();
                        mb.ShowDialog("Error !!", "Recipe Data를 로드하지 못했습니다.");
                    }

                    if (listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Count > 0)
                    {
                        listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex = -1;  // 선택 해제
                        listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex = 0;   // 다시 선택 → 이벤트 발생
                    }
                }

                workStage.ResetProcess();
            }
        }
        private void button_Recipe_Apply_Click(object sender, EventArgs e)
        {
            //  Recipe 창의 데이터를 Equipment Recipe Set에 적용
            //  Layer Index 확인
            int m_nLayerIndex = -1;
            int m_nIndex = listBox_Recipe_TabRecipe_ListOfDrawingLayer.SelectedIndex;
            string m_strLayerName = "";
            if (m_nIndex < 0)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Layer 를 선택하지 않았습니다.");
                return;
            }
            m_strLayerName = listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items[m_nIndex].ToString();

            //  도면 확인
            if (richTextBox_Recipe_TabRecipe_DrawingFile.Text.Length <= 0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "도면 파일이 없습니다.");
                return;
            }

            //  Layer Index 확인
            //  Hole 인지?
            string m_strLayer = m_strLayerName.Length > 4 ? m_strLayerName.Substring(0, 4) : m_strLayerName;
            if (m_strLayer == "Hole")                   //  Layer 가 Hole 이면?
            {
                //  Hole 로 시작하는 Layer 이면, 뒤에 숫자를 가져온다.
                string m_strHoleLayer_Number = m_strLayerName.Substring(4);

                if (IsNumeric(m_strHoleLayer_Number))
                {
                    int m_nHoleLayer_Index = Equipment.ToInt(m_strHoleLayer_Number);
                    if ((m_nHoleLayer_Index >= 1) && (m_nHoleLayer_Index <= 50))
                    {
                        m_nLayerIndex = m_nHoleLayer_Index - 1;             //  Hole Layer 의 Index 는 0부터 시작
                    }
                    else
                    {
                        m_nLayerIndex = (int)LayerList.Hole1;
                    }
                }
            }
            #region 간소화
            //if (m_strLayerName == "Hole1")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole1;
            //}
            //else if (m_strLayerName == "Hole2")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole2;
            //}
            //else if (m_strLayerName == "Hole3")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole3;
            //}
            //else if (m_strLayerName == "Hole4")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole4;
            //}
            //else if (m_strLayerName == "Hole5")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole5;
            //}
            //else if (m_strLayerName == "Hole6")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole6;
            //}
            //else if (m_strLayerName == "Hole7")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole7;
            //}
            //else if (m_strLayerName == "Hole8")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole8;
            //}
            //else if (m_strLayerName == "Hole9")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole9;
            //}
            //else if (m_strLayerName == "Hole10")
            //{
            //    m_nLayerIndex = (int)LayerList.Hole10;
            //}
            #endregion
            else if (m_strLayerName == "Rect")
            {
                m_nLayerIndex = (int)LayerList.Rect;
            }
            else if (m_strLayerName == "Outline")
            {
                m_nLayerIndex = (int)LayerList.Outline;
            }
            else if (m_strLayerName == "Marking")
            {
                m_nLayerIndex = (int)LayerList.Marking;
            }
            else if (m_strLayerName == "Fiducial")
            {
                m_nLayerIndex = (int)LayerList.Fiducial;
            }
            else if (m_strLayerName == "Thruhole")
            {
                m_nLayerIndex = (int)LayerList.Thruhole;
            }
            else if (m_strLayerName == "PreAlign")
            {
                m_nLayerIndex = (int)LayerList.PreAlign;
            }
            else
            {
                var mb2 = new MessageBoxOk();
                mb2.ShowDialog("Information !", "잘못된 Layer Name 입니다.");
                return;
            }

            //  사용 되지 않는 Layer (Layer 이름이 잘못되었을 경우)
            if (m_nLayerIndex == -1)
            {
                var mb2 = new MessageBoxOk();
                mb2.ShowDialog("Information !", "잘못된 Layer Name 입니다.");
                return;
            }

            //  Drawing File
            Equipment.stLayerRecipeSet[0].DrawingFile = richTextBox_Recipe_TabRecipe_DrawingFile.Text;                  //  Drawing File 은 0번 Layer 에만 저장한다.
            Equipment.RecipeOpen_DrawingFilePath = richTextBox_Recipe_TabRecipe_DrawingFile.Text;

            //  Laser Parameter
            if ( Equipment.Machine_LaserType_CO2)
            {
                Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_PulseWidth = 
                    textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text) : 1;
                Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_PulsePeriod = 
                    textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text) : 1;
                Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_Frequency = 
                    textBox_Recipe_TabRecipe_LaserParam_Frequency.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_LaserParam_Frequency.Text) : 7000;
                Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_DutyCycle = 
                    textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text) : 1;

            }
            else
            {
                Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_PulseWidth = 
                    textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text) : 1;
                Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_PulsePeriod = 
                    textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text) : 1;
                Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_Frequency = 
                    textBox_Recipe_TabRecipe_LaserParam_Frequency.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_LaserParam_Frequency.Text) : 500000;
                Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_DutyCycle = 
                    textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text) : 1;

            }

            //Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_TriggerMode_External = radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_External.Checked;

            //  Process Priority
            Equipment.stLayerRecipeSet[m_nLayerIndex].ProcessPriority_P2P = radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked;

            //  Miscellaneous
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_ReferenceLayer = textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer.Text;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_DefocusingDistance = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text);
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_Resizing = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text);
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_HoleDrilling_StartPosDivision = comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text.Length > 0 ? Equipment.ToInt(comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_GroupSplitSize = textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text) : 4.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_GroupSplitSize_Height = textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text) : 4.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_ScannerDrillingSpeed = textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_ScannerJumpSpeed = textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_LaserOnDelay = textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_LaserOffDelay = textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_MarkDelay = textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_JumpDelay = textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_PolygonDelay = textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_Drilling_Power = textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_DrillingRepetition = textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_DrillingRepetitionBundle = textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text) : 100;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_RotationAngleArc = textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text) : 360.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_CircleStartAngleCircle1time = textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text) : 0.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_P2PDistance = textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_MaskIndex = comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex;
            Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex = comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_HoleProcessingType = comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex;
            //Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_FiducialAlignType = comboBox_Recipe_TabRecipe_Miscellaneous_FiducialAlignType.SelectedIndex;
            //Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_FiducialMarkType = comboBox_Recipe_TabRecipe_Miscellaneous_FiducialMarkType.SelectedIndex;

            //  m_nLayerIndex 를 하던 것에서 0번 index 만 사용하도록 변경
            Equipment.stLayerRecipeSet[0].Miscellaneous_HoleSortByDistance_Use = checkBox_Recipe_TabRecipe_Miscellaneous_HoleDrillingOrder_SortByDistance.Checked;                                                                                             //  Hole Data Sort by Distance Use
            Equipment.stLayerRecipeSet[0].Miscellaneous_HoleSortingDistance = textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance.Text) : 0.5;     //  Hole Data Sorting Distance

            //  m_nLayerIndex 를 하던 것에서 0번 index 만 사용하도록 변경
            //  Process Options
            Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use = checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign.Checked;                         //  Socket Align 기능 사용 여부
            Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use = checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck.Checked;             //  Socket Height Check 기능 사용 여부
            Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetX = textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetX.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetX.Text) : 0.0;     //  Socket Height Check Position Offset X
            Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheckPos_OffsetY = textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetY.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetY.Text) : 0.0;     //  Socket Height Check Position Offset Y

            Equipment.stLayerRecipeSet[0].ProcessOption_GoldPowderAlign_Use = checkBox_Recipe_TabRecipe_ProcessOptions_GoldPowderAlign.Checked;

            //  m_nLayerIndex 를 하던 것에서 0번 index 만 사용하도록 변경
            //  Module Information
            Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width = textBox_Recipe_TabRecipe_ModuleInformation_Width.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_ModuleInformation_Width.Text) : 125.0;
            Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height = textBox_Recipe_TabRecipe_ModuleInformation_Height.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_ModuleInformation_Height.Text) : 120.0;
            Equipment.stLayerRecipeSet[0].ModuleInformation_Silicon_Thickness = textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text) : 0.0;
            Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Thickness = textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderThickness.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderThickness.Text) : 0.0;
            Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Percent = textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderPercent.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderPercent.Text) : 0.0;
            Equipment.stLayerRecipeSet[0].ModuleInformation_GoldPowder_Limit = textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderLimit.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderLimit.Text) : 0.0;

            //  Spiral Parameter
            Equipment.stLayerRecipeSet[m_nLayerIndex].SpiralParam_OuterDiameter = textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text) : 0.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].SpiralParam_InnerDiameter = textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text) : 0.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].SpiralParam_Revolutions = textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text) : 10;
            Equipment.stLayerRecipeSet[m_nLayerIndex].SpiralParam_AngleFactor = textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text) : 10.0;

            //  m_nLayerIndex 를 하던 것에서 0번 index 만 사용하도록 변경
            //  EPRO Module Absorption Level
            Equipment.stLayerRecipeSet[0].EPRO_ModuleAbsorptionLevel = textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel.Text) : -40.0;

            //  m_nLayerIndex 를 하던 것에서 0번 index 만 사용하도록 변경
            //  M-Aligner Vacuum Use
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Ignore = checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked;         //  Ignore
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center = checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked;     //  Center
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner = checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked;       //  Inner
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer = checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked;       //  Outer

            //  m_nLayerIndex 를 하던 것에서 0번 index 만 사용하도록 변경
            //  집진기 주파수
            Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use = checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked;                         //  집진기 Remote Mode 사용 여부
            Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper = textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text) : 20.0;
            Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower = textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text) : 20.0;
            Equipment.stLayerRecipeSet[0].DustCollectorLower_Disable = checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked;                                        //  하부 집진기 사용 여부

            //  Marking Template
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingData_SiriusTemplate_Use = checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked;
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_DataType = comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex;
            //Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_Width = textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text) : 5.0;
            //Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_Height = textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text) : 5.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_TextType = radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked;                         //  true : Fixed Text, false : Serial Number
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_PrefixData = textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text;
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_StartNumber = textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text) : 1;
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_Digits = textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text) : 3;
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_IncreaseStep = textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text) : 1;
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_SuffixData = textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text;
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_Hatch_Use = checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked;
            Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_Hatch_Spacing = textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text) : 0.2;

            if (radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked)
            {
                Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_SerialNumberIncreaseType = (int)WorkStage.nSerialNumber_IncreaseType.forEachModule;
            }
            else if (radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked)
            {
                Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_SerialNumberIncreaseType = (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket;
            }
            else
            {
                Equipment.stLayerRecipeSet[m_nLayerIndex].MarkingTemplate_EntityData_SerialNumberIncreaseType = (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous;
            }

            // 
            Equipment.stLayerRecipeSet[m_nLayerIndex].CalfileOffsetZAxismm = richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text.Length > 0 ? Equipment.ToDouble(richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text) : 0.0;     //  Z-Axis Offset mm
            Equipment.stLayerRecipeSet[0].ChuckMSL_Enable = checkBox_Recipe_TabRecipe_ChuckMSL_Enable.Checked; //  Chuck MSL 사용 여부
            Equipment.stLayerRecipeSet[0].Align3Point_Enable = checkBox_Recipe_TabRecipe_3PointAlign_Enable.Checked; //  3-Point Align 사용 여부

            //  선택한 BET 
            switch (Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex)
            {
                case 0:
                    workStage.m_dBET_ZoomValue_Recipe = 0.8;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_8X_Mrad;
                    break;

                case 1:
                    workStage.m_dBET_ZoomValue_Recipe = 0.9;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_0_9X_Mrad;
                    break;

                case 2:
                    workStage.m_dBET_ZoomValue_Recipe = 1.0;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_0X_Mrad;
                    break;

                case 3:
                    workStage.m_dBET_ZoomValue_Recipe = 1.1;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_1X_Mrad;
                    break;

                case 4:
                    workStage.m_dBET_ZoomValue_Recipe = 1.2;
                    workStage.m_dBET_MradValue_Recipe = Equipment.BET_1_2X_Mrad;
                    break;
            }


            //  도면 데이터를 가공용 Document에 적용
            Equipment.SetEqpSiriusViewerDocument(m_formSiriusEditor.SiriusEditor.Document);
            //  Frequency 데이터가 있는지 체크
            if (m_nLayerIndex == (int)LayerList.Hole1)
            {
                if (Equipment.stLayerRecipeSet[(int)LayerList.Hole1].LaserParam_Frequency <= 0)
                {
                    var mb3 = new MessageBoxOk();
                    mb3.ShowDialog("Information !", "\"Hole1\" Layer 의 Frequency 가 0 입니다.");
                }
            }
            if (m_nLayerIndex == (int)LayerList.Thruhole)
            {
                if (Equipment.stLayerRecipeSet[(int)LayerList.Thruhole].LaserParam_Frequency <= 0)
                {
                    var mb3 = new MessageBoxOk();
                    mb3.ShowDialog("Information !", "\"Thruhole\" Layer 의 Frequency 가 0 입니다.");
                }
            }
            if (m_nLayerIndex == (int)LayerList.Outline)
            {
                if (Equipment.stLayerRecipeSet[(int)LayerList.Outline].LaserParam_Frequency <= 0)
                {
                    var mb3 = new MessageBoxOk();
                    mb3.ShowDialog("Information !", "\"Outline\" Layer 의 Frequency 가 0 입니다.");
                }
            }

            //  도면 데이터 체크
            workStage.DrillingData_Verification();

            var mb4 = new MessageBoxOk();
            mb4.ShowDialog("Recipe Data Apply !", "Recipe Data Apply");
        }
        private void button_Recipe_Save_Click(object sender, EventArgs e)
        {
            string fileName;

            if (bTestRecipe)
            {
                RecipeManager.Instance.SaveRecipe(); // 현재 경로로 저장
                MessageBox.Show("Recipe 저장 완료");
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Recipe Data Path";
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.CreatePrompt = true;

                if (Equipment.Current_Recipe.Length <= 0)
                {
                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !", "레시피를 불러오지 않았습니다.");
                    return;
                }

                if (Equipment.RecipeFilePath.Length > 0)
                {
                    saveFileDialog.InitialDirectory = Equipment.RecipeFilePath;
                }
                else
                {
                    saveFileDialog.InitialDirectory = ConfigManager.GetRecipeDataPath();
                }

                saveFileDialog.Filter = "Recipe File(*.ini)|*.ini";

                DirectoryInfo di = new DirectoryInfo(ConfigManager.GetRecipeDataPath());
                if (!di.Exists == false)
                {
                    di.Create();
                }

                fileName = Equipment.Current_Recipe;
                string m_strTemp = string.Format("레시피 설정을 저장하시겠습니까?\r\n\r\nName : {0}", System.IO.Path.GetFileName(fileName));
                var mb1 = new MessageBoxYesNo();
                if (DialogResult.Yes == mb1.ShowDialog("Question ?", m_strTemp))
                {
                    if (File.Exists(fileName) == false)
                    {
                        //File.Create(fileName);
                        using (FileStream fs = File.Create(fileName))
                        {
                            // 파일만 생성하고 바로 닫음
                        }
                    }

                    //  Recipe Data 저장
                    //Recipe_Data_Save(fileName);
                    Recipe_Data_Save_Refactory(fileName);
                    Equipment.Current_Recipe = fileName;

                    // Vision Data 저장
                    stVisionRecipeSet.SaveToIni(fileName);


                    var mb2 = new MessageBoxOk();
                    mb2.ShowDialog("Information !", "Recipe Data를 저장하였습니다.");

                    Recipe_Open(fileName); // Recipe Open
                }
            }
        }
        private void button_Recipe_SaveAs_Click(object sender, EventArgs e)
        {
            string fileName;

            if (bTestRecipe)
            {
                using (var sfd = new SaveFileDialog { Filter = "Recipe File(*.ini)|*.ini" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        RecipeManager.Instance.SaveRecipe(sfd.FileName);
                        RefreshUIAfterRecipeOpen(sfd.FileName); // 파일명 라벨 등 갱신
                    }
                }
            }
            else
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Recipe Data Path";
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.CreatePrompt = true;

                if (Equipment.RecipeFilePath.Length > 0)
                {
                    saveFileDialog.InitialDirectory = Equipment.RecipeFilePath;
                }
                else
                {
                    saveFileDialog.InitialDirectory = ConfigManager.GetRecipeDataPath();
                }

                saveFileDialog.Filter = "Recipe File(*.ini)|*.ini";

                DirectoryInfo di = new DirectoryInfo(ConfigManager.GetRecipeDataPath());
                if (!di.Exists == false)
                {
                    di.Create();
                }

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    fileName = saveFileDialog.FileName;

                    if (File.Exists(fileName) == false)
                    {
                        //File.Create(fileName);
                        using (FileStream fs = File.Create(fileName))
                        {
                            // 파일만 생성하고 바로 닫음
                        }
                    }

                    //  Recipe Data 저장
                    //Recipe_Data_Save(fileName);
                    Recipe_Data_Save_Refactory(fileName);
                    Equipment.Current_Recipe = fileName;

                    // Vision Data 저장
                    //visionData.SaveTrainImage(Owner.TrainImage);
                    stVisionRecipeSet.SaveToIni(fileName);

                    var mb = new MessageBoxOk();
                    mb.ShowDialog("Information !!", "Recipe Data를 저장하였습니다.");

                    Recipe_Open(fileName); // Recipe Open
                }
            }
        }

        private void button_DutyCycle_Calc_Click(object sender, EventArgs e)
        {
            try
            {
                // 입력값 가져오기
                double frequency = double.Parse(textBox_Recipe_TabRecipe_LaserParam_Frequency.Text);
                double pulseWidthUs = double.Parse(textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text);

                // Period 계산
                double periodSeconds = 1 / frequency;

                // Duty Cycle 계산
                double dutyCycle = (pulseWidthUs / (periodSeconds * 1_000_000)) * 100;

                // 결과 출력
                textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = dutyCycle.ToString(); //$"Duty Cycle: {dutyCycle:F2}%";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void button_PulseWidth_Calc_Click(object sender, EventArgs e)
        {
            try
            {
                // 입력값 가져오기
                double frequency = double.Parse(textBox_Recipe_TabRecipe_LaserParam_Frequency.Text);
                double dutyCycle = double.Parse(textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text);

                // Period 계산 (초 단위) 
                double periodSeconds = 1 / frequency;

                // Pulse Width 계산 (μs 단위)
                double pulseWidthUs = (dutyCycle * periodSeconds / 100) * 1_000_000;

                // 결과 출력
                textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text = pulseWidthUs.ToString("F2"); // 소수점 2자리까지 표시

                // PulseWidth Min Max 계산
                double pulseWidthUs_Min = (0 * periodSeconds / 100) * 1_000_000;
                double pulseWidthUs_Max = (100 * periodSeconds / 100) * 1_000_000;

                // Min Max 범위 표시
                label_PulseWidth_Range.Text = string.Format("({0:0.00us} ~ {1:0.00us})", pulseWidthUs_Min, pulseWidthUs_Max);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText_CheckedChanged(object sender, EventArgs e)
        {
            //  Fixed Text
            textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Enabled = true;

            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;

            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Enabled = false;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Enabled = false;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Enabled = false;
            button_Marking_SerialNumber_CountReset.Enabled = false;


            switch (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType)
            {
                case (int)WorkStage.nSerialNumber_IncreaseType.forEachModule:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
                    break;

                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked = true;
                    break;

                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Checked = true;
                    break;
            }
        }
        private void radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber_CheckedChanged(object sender, EventArgs e)
        {
            //  Serial Number;

            textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Enabled = true;

            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = true;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = true;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = true;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = true;

            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Enabled = true;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Enabled = true;
            radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Enabled = true;
            button_Marking_SerialNumber_CountReset.Enabled = true;

            switch (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType)
            {
                case (int)WorkStage.nSerialNumber_IncreaseType.forEachModule:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module.Checked = true;
                    break;

                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket.Checked = true;
                    break;

                case (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous:
                    radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous.Checked = true;
                    break;
            }

            if(radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked)
            {
                int nIncrease = 0;
                nIncrease = Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text);
                if (nIncrease <= 0)
                {
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text = "1";
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Refresh();
                }
            }
            
        }
        private void comboBox_Recipe_TabRecipe_CustomMarking_DataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  True Type Font 일 때만 Hatch 활성화

            int m_nIndex = comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex;

            if (m_nIndex == 0) // True Type Font
            {
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = true;

                if (checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked)
                {
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;
                }
                else
                {
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
                }
            }
            else
            {
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
            }
        }
        private void checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable_CheckedChanged(object sender, EventArgs e)
        {
            //  Hatch Enable

            if (checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked)
            {
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;
            }
            else
            {
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
            }
        }
        private void comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  BET Zoom 배율에 따라 Mrad 값 변경
            // 

            int m_nIndex = comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex;

            if (m_nIndex < 0)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !!", "BET Zoom 배율을 선택해야 합니다.");

                return;
            }

            switch( m_nIndex)
            {
                case 0:                 //  0.8x
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_0_8X_Mrad.ToString();
                    break;

                case 1:                 //  0.9x
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_0_9X_Mrad.ToString();
                    break;

                case 2:                 //  1.0x
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_0X_Mrad.ToString();
                    break;

                case 3:                 //  1.1x
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_1X_Mrad.ToString();
                    break;

                case 4:                 //  1.2x
                    label_Recipe_TabRecipe_Miscellaneous_ZoomPosition.Text = Equipment.BET_1_2X_Mrad.ToString();
                    break;
            }
        }
        
        private void radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module_CheckedChanged(object sender, EventArgs e)
        {
            Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType = (int)WorkStage.nSerialNumber_IncreaseType.forEachModule;
        }

        private void radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket_CheckedChanged(object sender, EventArgs e)
        {
            Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType = (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket;
        }

        private void radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous_CheckedChanged(object sender, EventArgs e)
        {
            Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_SerialNumberIncreaseType = (int)WorkStage.nSerialNumber_IncreaseType.forEachSocket_Continuous;
        }
        
        private void button_Marking_SerialNumber_Preview_Click(object sender, EventArgs e)
        {
            //  마킹 데이터 미리보기

            string m_strMarkingData = "";

            int m_nStartNumber = Equipment.m_nSerialNumberMarkingCount;             //  미리 보기에서는 다음번 마킹시 가공 될 번호를 보여준다. (현재 Count + Increase Step)
            int m_nDigits = Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text) < 0 ? 1 : Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text);
            int m_nIncreaseStep = Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text);

            if (radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked)          //  고정 Text Data
            {
                m_strMarkingData = textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text;
            }
            else                                                                            //  Serial Number Data
            {
                //  Prefix 있으면 붙이고
                if (textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text.Length > 0)
                {
                    m_strMarkingData = textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text;
                }

                //  Serial Number 계산해서 만들고
                m_strMarkingData += string.Format("{0:D" + m_nDigits.ToString() + "}", m_nStartNumber);             //  다음꺼가 아니라 지금 마킹될 데이터

                //  Suffix 있으면 붙이고
                if (textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text.Length > 0)
                {
                    m_strMarkingData += textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text;
                }
            }

            label_Recipe_Marking_SerialNumber_Current.Text = m_strMarkingData;
        }
        private void button_Marking_SerialNumber_CountReset_Click(object sender, EventArgs e)
        {
            if (radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "시리얼 넘버를 초기화 하시겠습니까?\r\n\r\n[Start Number 부터 다시 시작됩니다.]"))
                    return;

                Equipment.m_nSerialNumberMarkingCount = Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text);

                string m_strMarkingData = "";

                int m_nStartNumber = Equipment.m_nSerialNumberMarkingCount;             //  미리 보기에서는 다음번 마킹시 가공 될 번호를 보여준다. (현재 Count + Increase Step)
                int m_nDigits = Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text) < 0 ? 1 : Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text);
                int m_nIncreaseStep = Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text);

                if (Equipment.stLayerRecipeSet[(int)LayerList.Marking].MarkingTemplate_EntityData_TextType)          //  고정 Text Data
                {
                    m_strMarkingData = textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text;
                }
                else                                                                            //  Serial Number Data
                {
                    //  Prefix 있으면 붙이고
                    if (textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text.Length > 0)
                    {
                        m_strMarkingData = textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text;
                    }

                    //  Serial Number 계산해서 만들고
                    m_strMarkingData += string.Format("{0:D" + m_nDigits.ToString() + "}", m_nStartNumber + m_nIncreaseStep);

                    //  Suffix 있으면 붙이고
                    if (textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text.Length > 0)
                    {
                        m_strMarkingData += textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text;
                    }
                }
                label_Recipe_Marking_SerialNumber_Current.Text = m_strMarkingData;

                int nIncrease = 0;
                nIncrease = Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text);
                if (nIncrease <= 0)
                {
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text = "1";
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Refresh();
                }
            }
        }
        private void listBox_Recipe_TabRecipe_ListOfDrawingLayer_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            ListBox listBox = sender as ListBox;
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            // 선택 여부에 따라 배경색 변경
            Color backColor = isSelected ? Color.DodgerBlue : Color.White;
            Color textColor = isSelected ? Color.White : Color.Black;

            using (SolidBrush backgroundBrush = new SolidBrush(backColor))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            {
                // 배경
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);

                // 텍스트
                string itemText = listBox.Items[e.Index].ToString();
                e.Graphics.DrawString(itemText, e.Font, textBrush, e.Bounds);
            }

            // 포커스 테두리 제거
            e.DrawFocusRectangle();
        }
        private void SetRecipeTabControlsVisible(string strLayerName, bool bEnabled)
        {
            Control[] targetControls = new Control[]
            {
                // 공통 //Hole1에서 설정.
                button_Recipe_TabRecipe_Cal_ZAxisOffset,
                richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset,
                textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel,
                checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore,
                checkBox_Recipe_TabRecipe_MAlignVacuum_Outer,
                checkBox_Recipe_TabRecipe_MAlignVacuum_Center,
                checkBox_Recipe_TabRecipe_MAlignVacuum_Inner,
                checkBox_Recipe_TabRecipe_LowerDustCollector_Disable,
                checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode,
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper,
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower,
                checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck,
                checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign,
                checkBox_Recipe_TabRecipe_ProcessOptions_GoldPowderAlign,
                textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetY,
                textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetX,
                textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness,
                textBox_Recipe_TabRecipe_ModuleInformation_Height,
                textBox_Recipe_TabRecipe_ModuleInformation_Width,
                button_GoldPowderThickness,
                textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderThickness,
                textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderPercent,
                textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderLimit,
                checkBox_Recipe_TabRecipe_ChuckMSL_Enable,
                checkBox_Recipe_TabRecipe_3PointAlign_Enable,
                
                //공정 Param
                textBox_Recipe_TabRecipe_LaserParam_Frequency,
                button_PulseWidth_Calc,
                textBox_Recipe_TabRecipe_LaserParam_DutyCycle,
                button_DutyCycle_Calc,
                textBox_Recipe_TabRecipe_LaserParam_PulseWidth,
                radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod,
                radioButton_Recipe_TabRecipe_ProcessPriority_P2P,
                textBox_Recipe_TabRecipe_SpiralParam_AngleFactor,
                textBox_Recipe_TabRecipe_SpiralParam_Revolutions,
                textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter,
                textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter,
                textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time,
                textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance,
                checkBox_Recipe_TabRecipe_Miscellaneous_HoleDrillingOrder_SortByDistance,
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height,
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType,
                textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle,
                button_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay,
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision,
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize,
                textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed,
                textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed,
                textBox_Recipe_TabRecipe_Miscellaneous_Resizing,
                textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance,
                textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer,
                comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex,
                comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition,
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance,
                
                //Marking
                button_Marking_SerialNumber_CountReset,
                button_Marking_SerialNumber_Preview,
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous,
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module,
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket,
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase,
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing,
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable,
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber,
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText,
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix,
                textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix,
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits,
                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber,
                comboBox_Recipe_TabRecipe_CustomMarking_DataType,
                checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode,

                groupBox6
            };

            Control[] targetControlsHole1 = new Control[]
            {
                // 공통 //Hole1에서 설정.
                button_Recipe_TabRecipe_Cal_ZAxisOffset,
                richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset,
                textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel,
                checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore,
                checkBox_Recipe_TabRecipe_MAlignVacuum_Outer,
                checkBox_Recipe_TabRecipe_MAlignVacuum_Center,
                checkBox_Recipe_TabRecipe_MAlignVacuum_Inner,
                checkBox_Recipe_TabRecipe_LowerDustCollector_Disable,
                checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode,
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper,
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower,
                checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck,
                checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign,
                checkBox_Recipe_TabRecipe_ProcessOptions_GoldPowderAlign,
                textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetY,
                textBox_Recipe_TabRecipe_SocketHeightCheckPosition_OffsetX,
                textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness,
                textBox_Recipe_TabRecipe_ModuleInformation_Height,
                textBox_Recipe_TabRecipe_ModuleInformation_Width,
                button_GoldPowderThickness,
                textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderThickness,
                textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderPercent,
                textBox_Recipe_TabRecipe_ModuleInformation_GoldPowderLimit,
                checkBox_Recipe_TabRecipe_ChuckMSL_Enable,
                checkBox_Recipe_TabRecipe_3PointAlign_Enable,
                
                //공정 Param
                textBox_Recipe_TabRecipe_LaserParam_Frequency,
                button_PulseWidth_Calc,
                textBox_Recipe_TabRecipe_LaserParam_DutyCycle,
                button_DutyCycle_Calc,
                textBox_Recipe_TabRecipe_LaserParam_PulseWidth,
                radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod,
                radioButton_Recipe_TabRecipe_ProcessPriority_P2P,
                textBox_Recipe_TabRecipe_SpiralParam_AngleFactor,
                textBox_Recipe_TabRecipe_SpiralParam_Revolutions,
                textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter,
                textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter,
                textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time,
                textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance,
                checkBox_Recipe_TabRecipe_Miscellaneous_HoleDrillingOrder_SortByDistance,
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height,
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType,
                textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle,
                button_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                label_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay,
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision,
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize,
                textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed,
                textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed,
                textBox_Recipe_TabRecipe_Miscellaneous_Resizing,
                textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance,
                textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer,
                comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex,
                comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition,
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance
            };

            Control[] targetControlsMarking = new Control[]
            {
                //공정 Param
                textBox_Recipe_TabRecipe_LaserParam_Frequency,
                button_PulseWidth_Calc,
                textBox_Recipe_TabRecipe_LaserParam_DutyCycle,
                button_DutyCycle_Calc,
                textBox_Recipe_TabRecipe_LaserParam_PulseWidth,
                radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod,
                radioButton_Recipe_TabRecipe_ProcessPriority_P2P,
                textBox_Recipe_TabRecipe_SpiralParam_AngleFactor,
                textBox_Recipe_TabRecipe_SpiralParam_Revolutions,
                textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter,
                textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter,
                textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time,
                textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance,
                checkBox_Recipe_TabRecipe_Miscellaneous_HoleDrillingOrder_SortByDistance,
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height,
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType,
                textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle,
                button_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                label_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay,
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision,
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize,
                textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed,
                textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed,
                textBox_Recipe_TabRecipe_Miscellaneous_Resizing,
                textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance,
                textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer,
                comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex,
                comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition,
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance,
                
               //Marking
                button_Marking_SerialNumber_CountReset,
                button_Marking_SerialNumber_Preview,
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Continuous,
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Module,
                radioButton_Recipe_TabRecipe_CustomMarking_SerialIncreaseType_Socket,
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase,
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing,
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable,
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber,
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText,
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix,
                textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix,
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits,
                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber,
                comboBox_Recipe_TabRecipe_CustomMarking_DataType,
                checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode,
                groupBox6
           };

            Control[] targetControlsThruhole = new Control[]
            {
                //공정 Param
                textBox_Recipe_TabRecipe_LaserParam_Frequency,
                button_PulseWidth_Calc,
                textBox_Recipe_TabRecipe_LaserParam_DutyCycle,
                button_DutyCycle_Calc,
                textBox_Recipe_TabRecipe_LaserParam_PulseWidth,
                radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod,
                radioButton_Recipe_TabRecipe_ProcessPriority_P2P,
                textBox_Recipe_TabRecipe_SpiralParam_AngleFactor,
                textBox_Recipe_TabRecipe_SpiralParam_Revolutions,
                textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter,
                textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter,
                textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time,
                textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance,
                checkBox_Recipe_TabRecipe_Miscellaneous_HoleDrillingOrder_SortByDistance,
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height,
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType,
                textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle,
                button_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                label_Recipe_TabRecipe_Miscellaneous_DrillingPower,
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay,
                textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay,
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision,
                textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize,
                textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed,
                textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed,
                textBox_Recipe_TabRecipe_Miscellaneous_Resizing,
                textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance,
                textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer,
                comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex,
                comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex,
                textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition,
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance
           };

            if (checkBox_MasterView.Checked)
            {
                foreach (var control in targetControls)
                {
                    control.Enabled = true;
                }
                return;
            }


            foreach (var control in targetControls)
            {
                control.Enabled = false;
            }

            //  ListView Column 설정
            if ((strLayerName == "Hole1"))
            {
                foreach (var control in targetControlsHole1)
                {
                    control.Enabled = bEnabled;
                }
                MachineType_Component_Enable(Equipment.Machine_LaserType_CO2);
            }
            else if ((strLayerName == "Hole2") ||
                (strLayerName == "Hole3") ||
                (strLayerName == "Hole4") ||
                (strLayerName == "Hole5") ||
                (strLayerName == "Hole6") ||
                (strLayerName == "Hole7") ||
                (strLayerName == "Hole8") ||
                (strLayerName == "Hole9") ||
                (strLayerName == "Hole10") ||
                (strLayerName == "Thruhole") ||
                (strLayerName == "Rect") ||
                (strLayerName == "Outline"))
            {
                foreach (var control in targetControlsThruhole)
                {
                    control.Enabled = bEnabled;
                }
                MachineType_Component_Enable(Equipment.Machine_LaserType_CO2);
            }
            else if (strLayerName == "Marking")
            {
                foreach (var control in targetControlsMarking)
                {
                    control.Enabled = bEnabled;
                }
                MachineType_Component_Enable(Equipment.Machine_LaserType_CO2);
            }
            else if (strLayerName == "PreAlign" || strLayerName == "Fiducial")
            {
                MachineType_Component_Enable(Equipment.Machine_LaserType_CO2);
                foreach (var control in targetControls)
                {
                    control.Enabled = bEnabled;
                }
            }

            
        }
        

        // 이거 각각 폼에 만들어야함.
        private void InitRecipeUI_KeyPad()
        {
            RegisterKeyPadDoubleClickHandlers(this); // 폼 전체에 대해 수행
        }
        private void RegisterKeyPadDoubleClickHandlers(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                // 조건: 숫자 입력용 TextBox 또는 RichTextBox만
                bool isTargetTextBox = ctrl is TextBox || ctrl is RichTextBox;

                if (isTargetTextBox && ctrl.Tag?.ToString().Contains("KeyPad") == true)
                {
                    ctrl.DoubleClick -= textBox_DoubleClick_OpenKeyPad; // 중복 연결 방지
                    ctrl.DoubleClick += textBox_DoubleClick_OpenKeyPad;

                    // 키보드 입력 제한용 Validating 연결
                    ctrl.Validating -= textBox_Validate_KeyPadRange;
                    ctrl.Validating += textBox_Validate_KeyPadRange;
                }

                // 하위 컨트롤 재귀 탐색
                if (ctrl.HasChildren)
                    RegisterKeyPadDoubleClickHandlers(ctrl);
            }
        }
        private void textBox_DoubleClick_OpenKeyPad(object sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                string currentText = ctrl.Text ?? "0";
                var dlg = new FormNew_KeyPad();
                dlg.StartPosition = FormStartPosition.CenterScreen;

                // Tag 파싱
                var meta = KeyPadMeta.ParseFromTag(ctrl.Tag?.ToString());
                dlg.MinValue = meta.Min;
                dlg.MaxValue = meta.Max;

                if (double.TryParse(currentText, out double value))
                    dlg.SetInitialValue(value);
                else
                    dlg.SetInitialValue(0);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string result = dlg.EnteredValue.ToString(meta.Format);
                    ctrl.Text = result;
                }
            }
        }
        private void textBox_Validate_KeyPadRange(object sender, CancelEventArgs e)
        {
            if (sender is TextBoxBase tb && tb.Tag != null)
            {
                var meta = KeyPadMeta.ParseFromTag(tb.Tag.ToString());

                if (double.TryParse(tb.Text, out double val))
                {
                    if (val < meta.Min)
                    {
                        tb.Text = meta.Min.ToString(meta.Format);
                        //MessageBox.Show($"최소값 {meta.Min}보다 작습니다."); // 또는 자동 보정만
                    }
                    else if (val > meta.Max)
                    {
                        tb.Text = meta.Max.ToString(meta.Format);
                        //MessageBox.Show($"최대값 {meta.Max}보다 큽니다.");
                    }
                    else
                    {
                        tb.Text = val.ToString(meta.Format);
                    }
                }
                else
                {
                    // 숫자 아님 → 초기화
                    tb.Text = meta.Min.ToString(meta.Format);
                }
            }
        }

        private void checkBox_Stage_Chuck_Use_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_Recipe_TabRecipe_Miscellaneous_DefocusingDistance_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text.Length == 0)
                return;

            double defocusValue = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_DefocusingDistance = defocusValue;
            }

            MessageBox.Show("Hole1 ~ Hole50 레이어에 Defocusing Distance가 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_Resizing_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text.Length == 0)
                return;

            double defocusValue = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_Resizing = defocusValue;
            }

            MessageBox.Show("Hole1 ~ Hole50 레이어에 Resizing 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize = value;
            }

            MessageBox.Show("GroupSplitSize가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize_Height = value;
            }

            MessageBox.Show("GroupSplitSize_Height가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerDrillingSpeed = value;
            }

            MessageBox.Show("ScannerDrillingSpeed가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerJumpSpeed = value;
            }

            MessageBox.Show("ScannerJumpSpeed가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_LaserOnDelay_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOnDelay = value;
            }

            MessageBox.Show("LaserOnDelay가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_LaserOffDelay_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOffDelay = value;
            }

            MessageBox.Show("LaserOffDelay가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_MarkDelay_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_MarkDelay = value;
            }

            MessageBox.Show("MarkDelay가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_JumpDelay_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_JumpDelay = value;
            }

            MessageBox.Show("JumpDelay가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_PolygonDelay_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_PolygonDelay = value;
            }

            MessageBox.Show("PolygonDelay가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_DrillingPower_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_Drilling_Power = value;
            }

            MessageBox.Show("DrillingPower가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_P2PDistance_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_P2PDistance = value;
            }

            MessageBox.Show("P2PDistance가 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_DrillingRepetition_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text.Length == 0)
                return;

            int value = Equipment.ToInt(textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetition = value;
            }

            MessageBox.Show("DrillingRepetition 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text.Length == 0)
                return;

            int value = Equipment.ToInt(textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetitionBundle = value;
            }

            MessageBox.Show("DrillingRepetitionBundle 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_RotationAngleArc = value;
            }

            MessageBox.Show("RotationAngleWhenArc 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_CircleStartAngleCircle1time = value;
            }

            MessageBox.Show("CircleStartAngleWhenCircle1time 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_HoleOrder_SortDistance.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortingDistance = value;
            }

            MessageBox.Show("HoleSortingDistance 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Cal_ZAxisOffset_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            if (richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text.Length == 0)
                return;

            double value = Equipment.ToDouble(richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].CalfileOffsetZAxismm = value;
            }

            MessageBox.Show("CalfileOffsetZAxismm 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;


            if (comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text.Length == 0)
                return;

            int value = Equipment.ToInt(comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text);

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleDrilling_StartPosDivision = value;
            }

            MessageBox.Show("HoleDrilling_StartPosDivision 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button_Recipe_TabRecipe_Miscellaneous_MaskIndex_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            int value = comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex;

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_MaskIndex = value;
            }

            MessageBox.Show("MaskIndex 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_BETPositionIndex_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            int value = comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex;

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_BETPositionIndex = value;
            }

            MessageBox.Show("BETPositionIndex 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Recipe_TabRecipe_Miscellaneous_HoleProcessingType_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Hole1 ~ Hole50 레이어에 일괄 적용하시겠습니까?"))
                return;

            int value = comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex;

            for (int i = (int)LayerList.Hole1; i <= (int)LayerList.Hole50; i++)
            {
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleProcessingType = value;
            }

            MessageBox.Show("HoleProcessingType 값이 Hole1~Hole50 레이어에 일괄 적용되었습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore_CheckedChanged(object sender, EventArgs e)
        {
            //checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked = false;
            //Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Ignore = false;
            if (checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked)
            {
                checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = false;
                checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = false;
                checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = false;

                Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Ignore = true;
            }
        }

        private void checkBox_Recipe_TabRecipe_MAlignVacuum_Center_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked)
            {
                checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked = false;
            }

            
        }

        private void checkBox_Recipe_TabRecipe_MAlignVacuum_Inner_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked)
            {
                checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked = false;
            }
        }

        private void checkBox_Recipe_TabRecipe_MAlignVacuum_Outer_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked)
            {
                checkBox_Recipe_TabRecipe_MAlignVacuum_Ignore.Checked = false;
            }
        }

        private void button_Recipe_TabRecipe_SpiralParam_Pitch_Click(object sender, EventArgs e)
        {
            int nIndex = comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex;
            double m_dTemp_OuterDiameter = 0.0;
            double m_dTemp_InnerDiameter = 0.0;
            double m_dTemp_Revolutions = 0.0;
            double m_dTemp_AngleFactor = 0.0;
            m_dTemp_OuterDiameter = Equipment.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text);
            m_dTemp_InnerDiameter = Equipment.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text);
            m_dTemp_Revolutions = Equipment.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text);
            m_dTemp_AngleFactor = Equipment.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text);
            // Hole Center
            //entity_Position.X = m_stLaserDrilling_SocketData[m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nDividedRegion_Region_CurrentIndex_forZigZag].m_stDividedRegion_ObjectData[nObject].dEdgePoint[0].X -
            //                    m_stLaserDrilling_SocketData[m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nDividedRegion_Region_CurrentIndex_forZigZag].dRegionCenter.X;
            //entity_Position.Y = m_stLaserDrilling_SocketData[m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nDividedRegion_Region_CurrentIndex_forZigZag].m_stDividedRegion_ObjectData[nObject].dEdgePoint[0].Y -
            //                    m_stLaserDrilling_SocketData[m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nDividedRegion_Region_CurrentIndex_forZigZag].dRegionCenter.Y;
            //double entity_Position_Rot = RotatePoint(scanner_Center, entity_Position, Math.PI / 2.0);
            PointD center = new PointD(0.0, 0.0);
            double pitch = 0.0;

            switch (nIndex)
            {
                case (int)HoleProcessingType.Circle:
                    label_Recipe_TabRecipe_SpiralParam_Pitch.Text = string.Format("---");
                    break;
                case (int)HoleProcessingType.Spiral_Polyline:
                    label_Recipe_TabRecipe_SpiralParam_Pitch.Text = string.Format("---");
                    break;
                case (int)HoleProcessingType.Spiral_Arc: // Spiral Arc Circle
                    workStage.MarkSpiralArc(m_dTemp_OuterDiameter, m_dTemp_InnerDiameter, (int)m_dTemp_Revolutions, m_dTemp_AngleFactor, center, out pitch);
                    label_Recipe_TabRecipe_SpiralParam_Pitch.Text = string.Format("{0:F5}", pitch);
                    break;
                case (int)HoleProcessingType.Spiral_Circle: // Spiral
                    workStage.MarkSpiralCircle(m_dTemp_OuterDiameter, m_dTemp_InnerDiameter, (int)m_dTemp_Revolutions, m_dTemp_AngleFactor, center, out pitch);
                    label_Recipe_TabRecipe_SpiralParam_Pitch.Text = string.Format("{0:F5}", pitch);
                    break;
                default:
                    MessageBox.Show("Spiral Hole Processing Type이 아닙니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
        }

        private void ApplyTooltips()
        {
            var tooltipHelper = new QMC.Common.UI.ControlTooltipHelper();
            tooltipHelper.AddTooltips(new Dictionary<Control, string>
            {
                { button_Recipe_New, "Create a new recipe file." },
                { button_Recipe_Apply, "Apply the current recipe settings to the system." },
                { button_Recipe_Save, "Save changes to the current recipe." },
                { button_Recipe_SaveAs, "Save current settings as a new recipe." },
                { button_Recipe_Open, "Open a saved recipe file." },
                { textBox_Recipe_TabRecipe_LaserParam_Frequency, "Laser repetition rate in kHz." },
                { textBox_Recipe_TabRecipe_LaserParam_PulseWidth, "Laser pulse width in ns." },
                { comboBox_Recipe_TabRecipe_CustomMarking_DataType, "Choose data type: Date, Serial, or Custom Text." },
                { checkBox_Recipe_TabRecipe_ChuckMSL_Enable, "Enable if MSL chuck should be used." },
                {checkBox_Recipe_TabRecipe_3PointAlign_Enable, "Socket Align - 3점으로 적용시 사용 (정밀도 낮아짐)" },
            });
        }

        private void button_Open_Recipe_Queue_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Recipe File (*.ini)|*.ini";
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var file in openFileDialog.FileNames)
                {
                    Equipment.RecipeQueue.AddRecipe(file);
                    listBox_RecipeQueue.Items.Add(file); // UI 리스트에 표시
                }
            }
        }

        private void radioButton_Recipe_TabRecipe_ProcessPriority_P2P_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked)
            {
                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = false;
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = false;
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = false;

                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = true;
            }
        }

        private void radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod.Checked)
            {
                textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Enabled = true;
                textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Enabled = true;
                textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Enabled = true;

                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Enabled = false;
            }
        }
    }
}
