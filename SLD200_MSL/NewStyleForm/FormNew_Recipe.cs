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

namespace SLD200_MSL
{
    public partial class FormNew_Recipe : Form
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부

        static WorkStage workStage;

        FormNew_SiriusEditor m_formSiriusEditor = null;

        private System.Windows.Forms.Timer timer_Recipe_Open;

        //private FormNewSub_Recipe_Vision userform_RecipeVision;
        //private FormNewSub_Recipe_GoldPowder userform_RecipeGoldPowder;

        public FormNewSub_Recipe_Vision userform_RecipeVision { get; set; }
        public FormNewSub_Recipe_GoldPowder userform_RecipeGoldPowder { get; set; }

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

            //  Layer Data 를 보여주는 ListView 설정
            listView_Recipe_TabRecipe_LayerData.View = View.Details;
            listView_Recipe_TabRecipe_LayerData.GridLines = true;         //  구분선 표시
            listView_Recipe_TabRecipe_LayerData.FullRowSelect = true;     //  한줄씩 선택 설정

            //  Recipe Open 타이머
            timer_Recipe_Open = new System.Windows.Forms.Timer();
            timer_Recipe_Open.Interval = 50;
            timer_Recipe_Open.Tick += new System.EventHandler(Timer_RecipeOpen_Func);
            timer_Recipe_Open.Enabled = true;

            //  Custom Marking Data 설정 (Recipe Load 전에는 비활성화)
            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;

            //  Recipe Open 전에는 Hatch 모드가 Disable 이므로 Hatch Spacing 을 비활성화 한다.
            textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = false;
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
            if (tabControl_Recipe.SelectedTab == tabPage_RecipeVision) // "RecipeVision" 탭을 선택했을 때
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
            Console.WriteLine("FormNew_Recipe 활성화됨 (Show)");

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
            }
        }

        /// <summary>
        /// 화면이 비활성화(Hide)될 때 실행할 로직
        /// </summary>
        private void OnHideRecipeForm()
        {
            Console.WriteLine("FormNew_Recipe 비활성화됨 (Hide)");

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

                Recipe_Open(Equipment.RecipeName_fromMainForm);
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

            if (m_formSiriusEditor == null)
            {
                m_formSiriusEditor.CreateSiriusEditor();
            }

            foreach ( Form openForm in Application.OpenForms)
            {
                if (openForm.Name == m_formSiriusEditor.Name)
                {
                    //  Sirius Edit 창을 열 때, 무조건 도면파일을 다시 불러오도록 변경. (얼라인 된 도면을 그대로 저장하는 경우가 있어서)
                    //if (!m_formSiriusEditor.Imported_DrawingFile_SameCheck(richTextBox_Recipe_TabRecipe_DrawingFile.Text))
                    {
                        m_formSiriusEditor.Import_DrawingFile(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                    }

                    openForm.BringToFront();
                    openForm.Show();
                    return;
                }
            }

            if (richTextBox_Recipe_TabRecipe_DrawingFile.Text.Length != 0)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Drawing File 을 Open 하시겠습니까?\r\n\r\n[No : 마지막 작업하던 도면으로 Editor Open]"))
                {
                    //  해당 위치에 파일이 존재하는지 확인
                    if (File.Exists(richTextBox_Recipe_TabRecipe_DrawingFile.Text) == false)
                    {
                        var mb1 = new MessageBoxOk();
                        mb1.ShowDialog("Error !!", "도면 파일이 없습니다.\r\n\r\n[마지막 작업하던 도면으로 Editor Open]");
                        //return;
                    }
                    else
                    {
                        //  확장자가 도면인지 확인 (.sirius2, .dwg, .dxf)
                        string m_strExt = Path.GetExtension(richTextBox_Recipe_TabRecipe_DrawingFile.Text);

                        if ((m_strExt.ToUpper() == ".SIRIUS") || (m_strExt.ToUpper() == ".DXF"))
                        {
                            m_formSiriusEditor.Import_DrawingFile(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                        }
                        else
                        {
                            var mb1 = new MessageBoxOk();
                            mb1.ShowDialog("Error !!", "도면 파일 형식이 아닙니다. (*.sirius, *.dxf)\r\n\r\n[마지막 작업하던 도면으로 Editor Open]");
                            //return;
                        }
                    }
                }
            }

            m_formSiriusEditor.Show();
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

                //if ((m_strExt.ToUpper() == ".SIRIUS2") || (m_strExt.ToUpper() == ".DWG") || (m_strExt.ToUpper() == ".DXF"))           //  Sirius2
                //{
                //    var doc = DocumentFactory.CreateDefault();
                //    doc.ActOpen(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                //    workStage.siriusEditorUserControl_WorkStage.Document = doc;
                //    workStage.MainSiriusViewer.Document = doc;

                //    workStage.DrillingData_Parsing();

                //    //SpiralLab.Sirius2.Winforms.UI.SiriusEditorUserControl siriusEditor_Temp = new SpiralLab.Sirius2.Winforms.UI.SiriusEditorUserControl();

                //    //var doc = DocumentFactory.CreateDefault();
                //    //doc.ActOpen(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                //    //siriusEditor_Temp.Document = doc;

                //    foreach (var layer in workStage.siriusEditorUserControl_WorkStage.Document.InternalData.Layers)
                //    {
                //        if (layer.IsMarkerable)
                //        {
                //            listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Add(layer.Name);
                //        }
                //    }
                //}
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

                //SpiralLab.Sirius2.Winforms.UI.SiriusEditorUserControl siriusEditor_Temp = new SpiralLab.Sirius2.Winforms.UI.SiriusEditorUserControl();

                //var doc = DocumentFactory.CreateDefault();
                //doc.ActOpen(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                //siriusEditor_Temp.Document = doc;

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
            }
            else if (m_strLayerName == "Thruhole")
            {
                //if (workStage.m_nDrawing_Hole8Count > 0)
                {
                    //  Fiducial Data를 ListView에 표시
                    for (int i = 0; i < workStage.m_nDrawing_Hole8Count; i++)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = (i + 1).ToString();
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Thruhole[i].CenterX));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Thruhole[i].CenterY));
                        item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Thruhole[i].radius));
                        listView_Recipe_TabRecipe_LayerData.Items.Add(item);
                    }
                }
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
            }
            else if (m_strLayerName == "Outline")
            {

            }
            else if (m_strLayerName == "Marking")
            {

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
            }

            // 리스트뷰를 Refresh하여 보여줌
            listView_Recipe_TabRecipe_LayerData.EndUpdate();


            //  Layer 에 대한 Miscellaneous Data 표시
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
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Center", "true", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Inner", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Outer", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer = Convert.ToBoolean(temp.ToString());

                //  Fine Cam. Red
                //NativeMethods.GetPrivateProfileString(strTemp, "FineCam_Red", "0", temp, 255, strFIle);
                //Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamRed = Equipment.ToInt(temp.ToString());
                //  Fine Cam. IR
                //NativeMethods.GetPrivateProfileString(strTemp, "FineCam_IR", "0", temp, 255, strFIle);
                //Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamIR = Equipment.ToInt(temp.ToString());
                //  Coarse Cam. IR
                //NativeMethods.GetPrivateProfileString(strTemp, "CoarseCam_IR", "0", temp, 255, strFIle);
                //Equipment.stLayerRecipeSet[i].IlluminatorValue_CoarseCamIR = Equipment.ToInt(temp.ToString());

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
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_StartNumber", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_StartNumber = Equipment.ToInt(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_Digits", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Digits = Equipment.ToInt(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_IncreaseStep", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_IncreaseStep = Equipment.ToInt(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_EntityData_SuffixData", "", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SuffixData = temp.ToString();
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_Hatch_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Use = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MarkingData_SiriusTemplate_Hatch_Spacing", "0.2", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Spacing = Equipment.ToDouble(temp.ToString());

                NativeMethods.GetPrivateProfileString(strTemp, "ZCalFile_OffsetZ", "0.0", temp, 255, strFIle);
                stLayerRecipeSet[i].CalfileOffsetZAxismm = Equipment.ToDouble(temp.ToString());
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

                Equipment.stLayerRecipeSet[i].LaserParam_PulseWidth = ReadDouble(data, "Pulse_Width", 0);
                Equipment.stLayerRecipeSet[i].LaserParam_PulsePeriod = ReadDouble(data, "Pulse_Period", 0);
                Equipment.stLayerRecipeSet[i].LaserParam_Frequency = ReadInt(data, "Frequency", 0);
                Equipment.stLayerRecipeSet[i].LaserParam_DutyCycle = ReadDouble(data, "Duty_Cycle", 0.0);

                Equipment.stLayerRecipeSet[i].LaserParam_TriggerMode_External = ReadBool(data, "Trigger_Mode_External", false);
                Equipment.stLayerRecipeSet[i].ProcessPriority_P2P = ReadBool(data, "P2P", false);

                Equipment.stLayerRecipeSet[i].Miscellaneous_ReferenceLayer = ReadValue(data, "Reference_Layer", "");
                Equipment.stLayerRecipeSet[i].Miscellaneous_DefocusingDistance = ReadDouble(data, "Defocusing_Distance", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_Resizing = ReadDouble(data, "Resizing", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleDrilling_StartPosDivision = ReadInt(data, "HoleDrilling_StartPosDivision", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize = ReadDouble(data, "GroupSplitSize", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize_Height = ReadDouble(data, "GroupSplitSize_Height", 0.0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerDrillingSpeed = ReadDouble(data, "ScannerDrillingSpeed", 10.0);
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
                Equipment.stLayerRecipeSet[i].Miscellaneous_MaskIndex = ReadInt(data, "MaskIndex", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_BETPositionIndex = ReadInt(data, "BETPositionIndex", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleProcessingType = ReadInt(data, "HoleProcessingType", 0);
                //Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialAlignType = ReadInt(data, "FiducialAlignType", 0);
                //Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialMarkType = ReadInt(data, "FiducialMarkType", 0);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortByDistance_Use = ReadBool(data, "HoleSortByDistance_Use", true);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleSortingDistance = ReadDouble(data, "HoleDataSortingDistance", 0.5);

                Equipment.stLayerRecipeSet[i].ProcessOption_SocketAlign_Use = ReadBool(data, "Socket_Align_Use", false);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use = ReadBool(data, "Socket_HeightCheck_Use", false);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetX = ReadDouble(data, "Socket_HeightCheckPos_OffsetX", 0.0);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheckPos_OffsetY = ReadDouble(data, "Socket_HeightCheckPos_OffsetY", 0.0);
                
                Equipment.stLayerRecipeSet[i].ProcessOption_GoldPowderAlign_Use = ReadBool(data, "GoldPowder_Use", false);

                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Width = ReadDouble(data, "Module_Width", 125.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Height = ReadDouble(data, "Module_Height", 120.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness = ReadDouble(data, "Module_SiliconThickness", 0.0);
                Equipment.stLayerRecipeSet[i].ModuleInformation_GoldPowder_Thickness = ReadDouble(data, "Module_GoldPowderThickness", 0.0);

                Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter = ReadDouble(data, "Spiral_OuterDiameter", 0.0);
                Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter = ReadDouble(data, "Spiral_InnerDiameter", 0.0);
                Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions = ReadInt(data, "Spiral_Revolutions", 10);
                Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor = ReadDouble(data, "Spiral_AngleFactor", 10.0);

                Equipment.stLayerRecipeSet[i].EPRO_ModuleAbsorptionLevel = ReadDouble(data, "EPRO_ModuleAbsorptionLevel", -40.0);

                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center = ReadBool(data, "MAlignerVacuumUse_Center", true);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner = ReadBool(data, "MAlignerVacuumUse_Inner", false);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer = ReadBool(data, "MAlignerVacuumUse_Outer", false);

                //Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamRed = ReadInt(data, "FineCam_Red", 0);
                //Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamIR = ReadInt(data, "FineCam_IR", 0);
                //Equipment.stLayerRecipeSet[i].IlluminatorValue_CoarseCamIR = ReadInt(data, "CoarseCam_IR", 0);

                Equipment.stLayerRecipeSet[i].DustCollectorRemoteMode_Use = ReadBool(data, "DustCollector_RemoteMode_Use", false);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Upper = ReadDouble(data, "DustCollector_Frequency_Upper", 20.0);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Lower = ReadDouble(data, "DustCollector_Frequency_Lower", 20.0);
                Equipment.stLayerRecipeSet[i].DustCollectorLower_Disable = ReadBool(data, "DustCollector_Lower_Disable", false);
                Equipment.stLayerRecipeSet[i].CalfileOffsetZAxismm = ReadDouble(data, "ZCalFile_OffsetZ", 0.0);

                //  Marking Template
                Equipment.stLayerRecipeSet[i].MarkingData_SiriusTemplate_Use = ReadBool(data, "MarkingData_SiriusTemplate_Use", false);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_DataType = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_DataType", 0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Width = ReadDouble(data, "MarkingData_SiriusTemplate_EntityData_Width", 5.0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Height = ReadDouble(data, "MarkingData_SiriusTemplate_EntityData_Height", 5.0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_TextType = ReadBool(data, "MarkingData_SiriusTemplate_EntityData_TextType", true);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_PrefixData = ReadValue(data, "MarkingData_SiriusTemplate_EntityData_PrefixData", "");
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_StartNumber = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_StartNumber", 0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Digits = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_Digits", 0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_IncreaseStep = ReadInt(data, "MarkingData_SiriusTemplate_EntityData_IncreaseStep", 0);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_SuffixData = ReadValue(data, "MarkingData_SiriusTemplate_EntityData_SuffixData", "");
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Use = ReadBool(data, "MarkingData_SiriusTemplate_Hatch_Use", false);
                Equipment.stLayerRecipeSet[i].MarkingTemplate_EntityData_Hatch_Spacing = ReadDouble(data, "MarkingData_SiriusTemplate_Hatch_Spacing", 0.2);
            }
            
            return true;
        }

        private string ReadValue(Dictionary<string, string> data, string key, string defaultValue)
    => data.TryGetValue(key, out var value) ? value : defaultValue;

        private int ReadInt(Dictionary<string, string> data, string key, int defaultValue)
            => int.TryParse(ReadValue(data, key, defaultValue.ToString()), out var result) ? result : defaultValue;

        private double ReadDouble(Dictionary<string, string> data, string key, double defaultValue)
            => double.TryParse(ReadValue(data, key, defaultValue.ToString()), out var result) ? result : defaultValue;

        private bool ReadBool(Dictionary<string, string> data, string key, bool defaultValue)
            => bool.TryParse(ReadValue(data, key, defaultValue.ToString()), out var result) ? result : defaultValue;

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
                //  BET Position Index (0:0.1X, 1:0.5X, 2:1.0X, 3:1.5X, 4:2.0X)
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


                //  Spiral Parameter
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_OuterDiameter", Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_InnerDiameter", Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_Revolutions", Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_AngleFactor", Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor.ToString(), strFIle);

                //  EPRO Module Absorption Level
                NativeMethods.WritePrivateProfileString(strTemp, "EPRO_ModuleAbsorptionLevel", Equipment.stLayerRecipeSet[i].EPRO_ModuleAbsorptionLevel.ToString(), strFIle);
                
                //  M-Aligner Vacuum Use
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Center", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Inner", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Outer", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer.ToString(), strFIle);

                //  Fine Cam. Red
                //NativeMethods.WritePrivateProfileString(strTemp, "FineCam_Red", Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamRed.ToString(), strFIle);
                //  Fine Cam. IR
                //NativeMethods.WritePrivateProfileString(strTemp, "FineCam_IR", Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamIR.ToString(), strFIle);
                //  Coarse Cam. IR
                //NativeMethods.WritePrivateProfileString(strTemp, "CoarseCam_IR", Equipment.stLayerRecipeSet[i].IlluminatorValue_CoarseCamIR.ToString(), strFIle);

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
                
                //  ZCalFile Offset Z Axis (mm)
                NativeMethods.WritePrivateProfileString(strTemp, "ZCalFile_OffsetZ", Equipment.stLayerRecipeSet[i].CalfileOffsetZAxismm.ToString(), strFIle);
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

                layerDict["Spiral_OuterDiameter"] = Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter.ToString();
                layerDict["Spiral_InnerDiameter"] = Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter.ToString();
                layerDict["Spiral_Revolutions"] = Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions.ToString();
                layerDict["Spiral_AngleFactor"] = Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor.ToString();

                layerDict["EPRO_ModuleAbsorptionLevel"] = Equipment.stLayerRecipeSet[i].EPRO_ModuleAbsorptionLevel.ToString();

                layerDict["MAlignerVacuumUse_Center"] = Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center.ToString();
                layerDict["MAlignerVacuumUse_Inner"] = Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner.ToString();
                layerDict["MAlignerVacuumUse_Outer"] = Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer.ToString();

                //layerDict["FineCam_Red"] = Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamRed.ToString();
                //layerDict["FineCam_IR"] = Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamIR.ToString();
                //layerDict["CoarseCam_IR"] = Equipment.stLayerRecipeSet[i].IlluminatorValue_CoarseCamIR.ToString();

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

                layerDict["ZCalFile_OffsetZ"] = Equipment.stLayerRecipeSet[i].CalfileOffsetZAxismm.ToString();

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


        private void button_Recipe_Save_Click(object sender, EventArgs e)
        {
            string fileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Recipe Data Path";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.CreatePrompt = true;

            //saveFileDialog.InitialDirectory = ConfigManager.GetRecipeDataPath();

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
                //visionData.SaveTrainImage(Owner.TrainImage);
                stVisionRecipeSet.SaveToIni(fileName);


                var mb2 = new MessageBoxOk();
                mb2.ShowDialog("Information !", "Recipe Data를 저장하였습니다.");
            }
        }

        public bool IsNumeric(string input)
        {
            return double.TryParse(input, out _);
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
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Layer 를 선택하지 않았습니다.\r\n\r\nLayer \"Hole1\" 의 파라미터로 설정하시겠습니까?"))
                    return;

                m_nLayerIndex = 0;
                m_strLayerName = "Hole1";
            }
            else
            {
                m_strLayerName = listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items[m_nIndex].ToString();
            }

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

            //  사용 되지 않는 Layer (Layer 이름이 잘못되었을 경우)
            if (m_nLayerIndex == -1)
            {
                var mb2 = new MessageBoxOk();
                mb2.ShowDialog("Information !", "잘못된 Layer Name 입니다.");
                return;
            }

            //  Drawing File
            Equipment.stLayerRecipeSet[m_nLayerIndex].DrawingFile = richTextBox_Recipe_TabRecipe_DrawingFile.Text;                  //  Drawing File 은 0번 Layer 에만 저장한다.
            Equipment.RecipeOpen_DrawingFilePath = richTextBox_Recipe_TabRecipe_DrawingFile.Text;

            //  Laser Parameter
            Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_PulseWidth = textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_PulsePeriod = textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_Frequency = textBox_Recipe_TabRecipe_LaserParam_Frequency.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_LaserParam_Frequency.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_DutyCycle = textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text) : 0;

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
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_BETPositionIndex = comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex;
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
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center = checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked;     //  Center
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner = checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked;       //  Inner
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer = checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked;       //  Outer

            //  조명값 (Fiducial Layer 의 것만 사용한다)
            //Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamRed = textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text) : 0;
            //Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamIR = textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text) : 0;
            //Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_CoarseCamIR = textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text) : 0;

            //  m_nLayerIndex 를 하던 것에서 0번 index 만 사용하도록 변경
            //  집진기 주파수
            Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use = checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked;                         //  집진기 Remote Mode 사용 여부
            Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper = textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text) : 20.0;
            Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower = textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text) : 20.0;
            Equipment.stLayerRecipeSet[0].DustCollectorLower_Disable = checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked;                                        //  하부 집진기 사용 여부

            //  Marking Template
            Equipment.stLayerRecipeSet[0].MarkingData_SiriusTemplate_Use = checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_DataType = comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Width = textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text) : 5.0;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Height = textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text) : 5.0;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_TextType = radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked;                         //  true : Fixed Text, false : Serial Number
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_PrefixData = textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_StartNumber = textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text) : 1;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Digits = textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text) : 4;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_IncreaseStep = textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text.Length > 0 ? Equipment.ToInt(textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text) : 1;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_SuffixData = textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Hatch_Use = checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Checked;
            Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Hatch_Spacing = textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text.Length > 0 ? Equipment.ToDouble(textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text) : 0.2;

            // 
            Equipment.stLayerRecipeSet[m_nLayerIndex].CalfileOffsetZAxismm = richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text.Length > 0 ? Equipment.ToDouble(richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text) : 0.0;     //  Z-Axis Offset mm



            //  도면 데이터를 가공용 Document 에 적용
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

            var mb4 = new MessageBoxOk();
            mb4.ShowDialog("Recipe Data Apply !", "Recipe Data Apply");
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
                //filePath = openFileDialog.filePath;
                fileName = openFileDialog.FileName;

                //  Recipe Data 로드
                //Recipe_Data_Load(fileName);
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
                    
                    if(workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage != null)
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
                    radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked = true; ;
                }
                else
                {
                    radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod.Checked = true; ;
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

                //  Spiral Parameter
                textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_OuterDiameter.ToString();
                textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_InnerDiameter.ToString();
                textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = Equipment.stLayerRecipeSet[0].SpiralParam_Revolutions.ToString();
                textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = Equipment.stLayerRecipeSet[0].SpiralParam_AngleFactor.ToString();

                //  EPRO Module Absorption Level
                textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel.Text = Equipment.stLayerRecipeSet[0].EPRO_ModuleAbsorptionLevel.ToString();

                //  M-Aligner Vacuum Use
                checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center;     //  Center
                checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner;       //  Inner
                checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer;       //  Outer

                //  Illuminator
                //textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamRed.ToString();
                //textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamIR.ToString();
                //textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_CoarseCamIR.ToString();

                //  집진기 주파수
                checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;                          //  집진기 Remote Mode 사용 여부
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();
                checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked = Equipment.stLayerRecipeSet[0].DustCollectorLower_Disable;                                        //  하부 집진기 사용 여부

                //  Marking Template
                checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked = Equipment.stLayerRecipeSet[0].MarkingData_SiriusTemplate_Use;
                comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_DataType.ToString());
                textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Width.ToString();
                textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Height.ToString();

                if (Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_TextType)
                {
                    radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked = true;

                    textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;
                }
                else
                {
                    radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked = true;

                    textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = true;
                }

                textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_PrefixData;
                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_StartNumber.ToString();
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Digits.ToString();
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_IncreaseStep.ToString();
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_SuffixData;

                if (comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex == 0)            //  True Type Font 일 때만 Hatch 활성화
                {
                    checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = true;
                    textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;

                    if (Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Hatch_Use)
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

                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Hatch_Spacing.ToString();


                //  Z-Axis Offset mm
                richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text = Equipment.stLayerRecipeSet[0].CalfileOffsetZAxismm.ToString();

                int m_nCount = 0;

                do
                {
                    m_nCount++;
                } while (m_nCount < 1000);
                
                
                //  도면 Import
                m_formSiriusEditor.Import_DrawingFile(richTextBox_Recipe_TabRecipe_DrawingFile.Text);

                Equipment.SetEqpSiriusViewerDocument( m_formSiriusEditor.SiriusEditor.Document);
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
                    int m_nReturn = workStage.GetDrillingData();
                    switch (m_nReturn)
                    {
                        case (int)WorkStage.nGetDataResult.GETDATA_SUCCESS:

                            Equipment.CycleTimer_LaserDrilling.Clear();
                            Equipment.CycleTimer_LaserDrilling.TotalElapsed = TimeSpan.Zero;
                            Equipment.CycleTimer_DoneModuleCount = 0;

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
            }
        }

        public void Recipe_Data_Refresh(string m_strLayerName)
        {
            //  해당 Layer 의 데이터로 변경하여 표시

            //public enum LayerList : int
            //{
            //    Hole1 = 0,
            //    Hole2,
            //    Hole3,
            //    Hole4,
            //    Rect,
            //    Outline,
            //    Marking,
            //    Fiducial,
            //}

            int m_nIndex = -1;

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
                        m_nIndex = m_nHoleLayer_Index - 1;             //  Hole Layer 의 Index 는 0부터 시작
                    }
                    else
                    {
                        m_nIndex = (int)LayerList.Hole1;
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
                m_nIndex = (int)LayerList.Rect;
            }
            else if (m_strLayerName == "Outline")
            {
                m_nIndex = (int)LayerList.Outline;
            }
            else if (m_strLayerName == "Marking")
            {
                m_nIndex = (int)LayerList.Marking;
            }
            else if (m_strLayerName == "Fiducial")
            {
                m_nIndex = (int)LayerList.Fiducial;
            }
            else if (m_strLayerName == "Thruhole")
            {
                m_nIndex = (int)LayerList.Thruhole;
            }
            else if (m_strLayerName == "PreAlign")
            {
                m_nIndex = (int)LayerList.PreAlign;
            }

            //  Laser Parameter
            textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text = Equipment.stLayerRecipeSet[m_nIndex].LaserParam_PulseWidth.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[m_nIndex].LaserParam_PulsePeriod.ToString();
            textBox_Recipe_TabRecipe_LaserParam_Frequency.Text = Equipment.stLayerRecipeSet[m_nIndex].LaserParam_Frequency.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[m_nIndex].LaserParam_DutyCycle.ToString();

            //if (Equipment.stLayerRecipeSet[m_nIndex].LaserParam_TriggerMode_External)
            //{
            //    radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_External.Checked = true;
            //}
            //else
            //{
            //    radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_Internal.Checked = true;
            //}

            //  Process Priority
            if (Equipment.stLayerRecipeSet[m_nIndex].ProcessPriority_P2P)
            {
                radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked = true; ;
            }
            else
            {
                radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod.Checked = true; 
            }

            //  Miscellaneous
            textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_ReferenceLayer;
            textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_DefocusingDistance.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_Resizing.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_HoleDrilling_StartPosDivision.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_GroupSplitSize.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize_Height.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_GroupSplitSize_Height.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_ScannerDrillingSpeed.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_ScannerJumpSpeed.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_LaserOnDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_LaserOffDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_MarkDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_JumpDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_PolygonDelay.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_Drilling_Power.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_DrillingRepetition.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_DrillingRepetitionBundle.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_RotationAngleArc.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_CircleStartAngleWhenCircle1time.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_CircleStartAngleCircle1time.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_P2PDistance.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_MaskIndex.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_BETPositionIndex.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_MaskIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_BETPositionIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_HoleProcessingType.ToString());
            //comboBox_Recipe_TabRecipe_Miscellaneous_FiducialAlignType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_FiducialAlignType.ToString());
            //comboBox_Recipe_TabRecipe_Miscellaneous_FiducialMarkType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_FiducialMarkType.ToString());
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

            //  Spiral Parameter
            textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = Equipment.stLayerRecipeSet[m_nIndex].SpiralParam_OuterDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = Equipment.stLayerRecipeSet[m_nIndex].SpiralParam_InnerDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = Equipment.stLayerRecipeSet[m_nIndex].SpiralParam_Revolutions.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = Equipment.stLayerRecipeSet[m_nIndex].SpiralParam_AngleFactor.ToString();

            //  EPRO Module Absorption Level
            textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel.Text = Equipment.stLayerRecipeSet[0].EPRO_ModuleAbsorptionLevel.ToString();

            //  M-Aligner Vacuum Use
            checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center;     //  Center
            checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner;       //  Inner
            checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer;       //  Outer

            //  Illuminator
            //textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamRed.ToString();
            //textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamIR.ToString();
            //textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_CoarseCamIR.ToString();

            //  집진기 주파수
            checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;                          //  집진기 Remote Mode 사용 여부
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();
            checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked = Equipment.stLayerRecipeSet[0].DustCollectorLower_Disable;

            //  Marking Template
            checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked = Equipment.stLayerRecipeSet[0].MarkingData_SiriusTemplate_Use;
            comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_DataType.ToString());
            textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Width.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Height.ToString();

            if (Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_TextType)
            {
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked = true;

                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;
            }
            else
            {
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked = true;

                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = true;
            }

            textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_PrefixData;
            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_StartNumber.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Digits.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_IncreaseStep.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_SuffixData;

            if (comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex == 0)            //  True Type Font 일 때만 Hatch 활성화
            {
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;

                if (Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Hatch_Use)
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

            textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Hatch_Spacing.ToString();


            richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text = Equipment.stLayerRecipeSet[m_nIndex].CalfileOffsetZAxismm.ToString();//  하부 집진기 사용 여부
        }

        public void Recipe_Open(string m_strRecipeFile)
        {
            string fileName;

            fileName = m_strRecipeFile;

            if (Equipment.GetEqpSiriusViewerDocument() == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            if (workStage.rtc == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            //  Recipe Data 로드
            //Recipe_Data_Load(fileName);
            Recipe_Data_Load_Refactory(fileName);
            Equipment.Current_Recipe = fileName;
            Equipment.Current_DrawingFileName = System.IO.Path.GetFileName(Equipment.stLayerRecipeSet[0].DrawingFile);

            //  Recipe 명 표시
            label_Recipe_FileName.Text = System.IO.Path.GetFileName(fileName);

            // Recipe Vision Load
            string iniPath = fileName;  //ConfigManager.GetRecipeDataPath() + "\\RecipeVisionData.ini";
            stVisionRecipeSet = VisionRecipeData.LoadFromIni(iniPath);
            if (workStage.jigAligner_LowRes != null)
            {
                workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = stVisionRecipeSet.LoadTrainImage(); //Bitmap.FromFile(m_strFile);
                workStage.jigAligner_LowRes.TrainImage = stVisionRecipeSet.LoadTrainImage(); //이거 사용중.
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
                radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked = true; ;
            }
            else
            {
                radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod.Checked = true; ;
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

            //  Spiral Parameter
            textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_OuterDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_InnerDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = Equipment.stLayerRecipeSet[0].SpiralParam_Revolutions.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = Equipment.stLayerRecipeSet[0].SpiralParam_AngleFactor.ToString();

            //  EPRO Module Absorption Level
            textBox_Recipe_TabRecipe_EPRO_ModuleAbsorptionLevel.Text = Equipment.stLayerRecipeSet[0].EPRO_ModuleAbsorptionLevel.ToString();

            //  M-Aligner Vacuum Use
            checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center;     //  Center
            checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner;       //  Inner
            checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer;       //  Outer

            //  Illuminator
            //textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamRed.ToString();
            //textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamIR.ToString();
            //textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_CoarseCamIR.ToString();

            //  집진기 주파수
            checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;                          //  집진기 Remote Mode 사용 여부
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();
            checkBox_Recipe_TabRecipe_LowerDustCollector_Disable.Checked = Equipment.stLayerRecipeSet[0].DustCollectorLower_Disable;                                        //  하부 집진기 사용 여부

            //  Marking Template
            checkBox_Recipe_TabRecipe_MarkingData_toChange_Barcode.Checked = Equipment.stLayerRecipeSet[0].MarkingData_SiriusTemplate_Use;
            comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex = Equipment.ToInt(Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_DataType.ToString());
            textBox_Recipe_TabRecipe_CustomMarking_DataSize_Width.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Width.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_DataSize_Height.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Height.ToString();

            if (Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_TextType)
            {
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_FixedText.Checked = true;

                textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = false;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = false;
            }
            else
            {
                radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber.Checked = true;

                textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = true;
            }

            textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_PrefixData;
            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_StartNumber.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Digits.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_IncreaseStep.ToString();
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_SuffixData;

            if (comboBox_Recipe_TabRecipe_CustomMarking_DataType.SelectedIndex == 0)            //  True Type Font 일 때만 Hatch 활성화
            {
                checkBox_Recipe_TabRecipe_CustomMarking_Hatch_Enable.Enabled = true;
                textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Enabled = true;

                if (Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Hatch_Use)
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
            
            textBox_Recipe_TabRecipe_CustomMarking_Hatch_Spacing.Text = Equipment.stLayerRecipeSet[0].MarkingTemplate_EntityData_Hatch_Spacing.ToString();

            richTextBox_Recipe_TabRecipe_Cal_ZAxisOffset.Text = Equipment.stLayerRecipeSet[0].CalfileOffsetZAxismm.ToString();

            int m_nCount = 0;
            do
            {
                m_nCount++;
            } while (m_nCount < 1000);


            //  도면 Import
            m_formSiriusEditor.Import_DrawingFile(richTextBox_Recipe_TabRecipe_DrawingFile.Text);

            Equipment.SetEqpSiriusViewerDocument( m_formSiriusEditor.SiriusEditor.Document);
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

                MessageBox.Show("Recipe Data를 로드하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Recipe Data를 로드하지 못했습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return;
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

        private void button_Recipe_SaveAs_Click(object sender, EventArgs e)
        {
            string fileName;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Recipe Data Path";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.CreatePrompt = true;

            //saveFileDialog.InitialDirectory = ConfigManager.GetRecipeDataPath();

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
        }
        private void radioButton_Recipe_TabRecipe_CustomMarking_TextType_SerialNumber_CheckedChanged(object sender, EventArgs e)
        {
            //  Serial Number

            textBox_Recipe_TabRecipe_CustomMarking_Data_Prefix.Enabled = true;

            textBox_Recipe_TabRecipe_CustomMarking_Data_StartNumber.Enabled = true;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Digits.Enabled = true;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Increase.Enabled = true;
            textBox_Recipe_TabRecipe_CustomMarking_Data_Suffix.Enabled = true;
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
    }
}
