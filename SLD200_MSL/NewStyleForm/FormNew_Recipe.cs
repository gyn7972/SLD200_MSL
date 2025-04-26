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

namespace SLD200_MSL
{
    public partial class FormNew_Recipe : Form
    {
        static WorkStage workStage;

        FormNew_SiriusEditor m_formSiriusEditor = null;

        private System.Windows.Forms.Timer timer_Recipe_Open;

        public FormNew_Recipe()
        {
            InitializeComponent();

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }

                //if (module.Name == "Loader")
                //{
                //    loader = module as Loader;
                //}

                //if (module.Name == "Unloader")
                //{
                //    unloader = module as Unloader;
                //}

                //if (module.Name == "Vision")
                //{
                //    vision = module as Vision;
                //}

                //if (module.Name == "BDS")
                //{
                //    bds = module as Bds;
                //}
            }

            m_formSiriusEditor = new FormNew_SiriusEditor();

            //  Layer Data 를 보여주는 ListView 설정
            listView_Recipe_TabRecipe_LayerData.View = View.Details;
            listView_Recipe_TabRecipe_LayerData.GridLines = true;         //  구분선 표시
            listView_Recipe_TabRecipe_LayerData.FullRowSelect = true;     //  한줄씩 선택 설정


            //  Recipe Open 타이머
            timer_Recipe_Open = new System.Windows.Forms.Timer();
            timer_Recipe_Open.Interval = 10;
            timer_Recipe_Open.Tick += new System.EventHandler(Timer_RecipeOpen_Func);
            timer_Recipe_Open.Enabled = true;
        }

        private void Timer_RecipeOpen_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.

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

            if (Equipment.EqpSiriusViewer == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            if (workStage.rtc == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
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
                    if (!m_formSiriusEditor.Imported_DrawingFile_SameCheck(richTextBox_Recipe_TabRecipe_DrawingFile.Text))
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
                        MessageBox.Show("도면 파일이 없습니다.\r\n\r\n[마지막 작업하던 도면으로 Editor Open]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            MessageBox.Show("도면 파일이 아닙니다. (*.sirius, *.dxf)\r\n\r\n[마지막 작업하던 도면으로 Editor Open]", "Information!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (Equipment.EqpSiriusViewer == null)
            {
                MessageBox.Show("RTC 보드를 초기화 해야 합니다.", "Information!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //  Layer List 전체 삭제
            listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Clear();

            if (richTextBox_Recipe_TabRecipe_DrawingFile.Text.Length != 0)
            {
                //  해당 위치에 파일이 존재하는지 확인
                if (File.Exists(richTextBox_Recipe_TabRecipe_DrawingFile.Text) == false)
                {
                    MessageBox.Show("도면 파일이 없습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    m_formSiriusEditor.SiriusEditor.Document = doc;
                }
                else if (m_strExt.ToUpper() == ".SIRIUS")
                {
                    //SiriusEditor.Document.New();
                    var doc = DocumentSerializer.OpenSirius(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                    //Equipment.EqpSiriusViewer.Document = doc;
                    m_formSiriusEditor.SiriusEditor.Document = doc;
                }
                else
                {
                    //MessageBox.Show("도면 파일이 아닙니다.\r\n\r\n[*.sirius2, *.dwg, *.dxf", "Information!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MessageBox.Show("도면 파일이 아닙니다.\r\n\r\n[*.sirius, *.dxf", "Information!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //SpiralLab.Sirius2.Winforms.UI.SiriusEditorUserControl siriusEditor_Temp = new SpiralLab.Sirius2.Winforms.UI.SiriusEditorUserControl();

                //var doc = DocumentFactory.CreateDefault();
                //doc.ActOpen(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
                //siriusEditor_Temp.Document = doc;

                Equipment.EqpSiriusViewer.Document = m_formSiriusEditor.SiriusEditor.Document;

                workStage.DrillingData_Parsing();

                foreach (var layer in m_formSiriusEditor.SiriusEditor.Document.Layers)
                {
                    if (layer.IsMarkerable)
                    {
                        listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Add(layer.Name);
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
                Equipment.stLayerRecipeSet[i].LaserParam_PulseWidth = Convert.ToInt32(temp.ToString());
                //  Laser Pulse Period (us)
                NativeMethods.GetPrivateProfileString(strTemp, "Pulse_Period", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].LaserParam_PulsePeriod = Convert.ToInt32(temp.ToString());
                //  Laser Frequency (Hz)
                NativeMethods.GetPrivateProfileString(strTemp, "Frequency", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].LaserParam_Frequency = Convert.ToInt32(temp.ToString());
                //  Laser DutyCycle (%)
                NativeMethods.GetPrivateProfileString(strTemp, "Duty_Cycle", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].LaserParam_DutyCycle = Convert.ToDouble(temp.ToString());

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
                Equipment.stLayerRecipeSet[i].Miscellaneous_DefocusingDistance = Convert.ToDouble(temp.ToString());
                //  Resizing (mm)
                NativeMethods.GetPrivateProfileString(strTemp, "Resizing", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_Resizing = Convert.ToDouble(temp.ToString());
                //  Hole Drilling Start Position Division (등분)
                NativeMethods.GetPrivateProfileString(strTemp, "HoleDrilling_StartPosDivision", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleDrilling_StartPosDivision = Convert.ToInt32(temp.ToString());
                //  Group Split Size (mm)
                NativeMethods.GetPrivateProfileString(strTemp, "GroupSplitSize", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_GroupSplitSize = Convert.ToDouble(temp.ToString());
                //  Scanner Drilling Speed (mm/s)
                NativeMethods.GetPrivateProfileString(strTemp, "ScannerDrillingSpeed", "10", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerDrillingSpeed = Convert.ToDouble(temp.ToString());
                //  Scanner Jump Speed (mm/s)
                NativeMethods.GetPrivateProfileString(strTemp, "ScannerJumpSpeed", "100", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_ScannerJumpSpeed = Convert.ToDouble(temp.ToString());
                //  Laser On Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "LaserOnDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOnDelay = Convert.ToInt32(temp.ToString());
                //  Laser Off Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "LaserOffDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_LaserOffDelay = Convert.ToInt32(temp.ToString());
                //  Mark Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "MarkDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_MarkDelay = Convert.ToInt32(temp.ToString());
                //  Jump Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "JumpDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_JumpDelay = Convert.ToInt32(temp.ToString());
                //  Polygon Delay (us)
                NativeMethods.GetPrivateProfileString(strTemp, "PolygonDelay", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_PolygonDelay = Convert.ToInt32(temp.ToString());
                //  Drilling Power (%)
                NativeMethods.GetPrivateProfileString(strTemp, "DrillingPower", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_Drilling_Power = Convert.ToDouble(temp.ToString());
                //  Space of P2P Distance (mm)
                NativeMethods.GetPrivateProfileString(strTemp, "P2PDistance", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_P2PDistance = Convert.ToDouble(temp.ToString());
                //  Drilling Repetation
                NativeMethods.GetPrivateProfileString(strTemp, "DrillingRepetation", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetition = Convert.ToInt16(temp.ToString());
                //  Drilling Repetition bundle
                NativeMethods.GetPrivateProfileString(strTemp, "DrillingRepetitionBundle", "50", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_DrillingRepetitionBundle = Convert.ToInt16(temp.ToString());
                //  Rotation Angle when Arc
                NativeMethods.GetPrivateProfileString(strTemp, "RotationAngleArc", "360.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_RotationAngleArc = Convert.ToDouble(temp.ToString());
                //  Mask Index (0:None, 1:Mask1, 2:Mask2, 3:Mask3, 4:Mask4)
                NativeMethods.GetPrivateProfileString(strTemp, "MaskIndex", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_MaskIndex = Convert.ToInt32(temp.ToString());
                //  BET Position Index (0:0.1X, 1:0.5X, 2:1.0X, 3:1.5X, 4:2.0X)
                NativeMethods.GetPrivateProfileString(strTemp, "BETPositionIndex", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_BETPositionIndex = Convert.ToInt32(temp.ToString());
                //  Hole Processing Type (0:Circle, 1:Spiral)
                NativeMethods.GetPrivateProfileString(strTemp, "HoleProcessingType", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_HoleProcessingType = Convert.ToInt32(temp.ToString());
                //  Fiducial Align Type (0:Circle Find, 1:Pattern Matching)
                NativeMethods.GetPrivateProfileString(strTemp, "FiducialAlignType", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialAlignType = Convert.ToInt32(temp.ToString());
                //  Fiducial Mark Type (0:Circle, 1:Gold Powder)
                NativeMethods.GetPrivateProfileString(strTemp, "FiducialMarkType", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialMarkType = Convert.ToInt32(temp.ToString());                

                //  Process Options
                NativeMethods.GetPrivateProfileString(strTemp, "Socket_Align_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketAlign_Use = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Socket_HeightCheck_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use = Convert.ToBoolean(temp.ToString());

                //  Module Information
                NativeMethods.GetPrivateProfileString(strTemp, "Module_Width", "125.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Width = Convert.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Module_Height", "120.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Height = Convert.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Module_SiliconThickness", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness = Convert.ToDouble(temp.ToString());

                //  Spiral Parameter
                NativeMethods.GetPrivateProfileString(strTemp, "Spiral_OuterDiameter", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter = Convert.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Spiral_InnerDiameter", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter = Convert.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Spiral_Revolutions", "10", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions = Convert.ToInt32(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "Spiral_AngleFactor", "10.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor = Convert.ToDouble(temp.ToString());

                //  M-Aligner Vacuum Use
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Center", "true", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Inner", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "MAlignerVacuumUse_Outer", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer = Convert.ToBoolean(temp.ToString());

                //  Fine Cam. Red
                NativeMethods.GetPrivateProfileString(strTemp, "FineCam_Red", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamRed = Convert.ToInt32(temp.ToString());
                //  Fine Cam. IR
                NativeMethods.GetPrivateProfileString(strTemp, "FineCam_IR", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamIR = Convert.ToInt32(temp.ToString());
                //  Coarse Cam. IR
                NativeMethods.GetPrivateProfileString(strTemp, "CoarseCam_IR", "0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].IlluminatorValue_CoarseCamIR = Convert.ToInt32(temp.ToString());

                //  Dust Collector
                NativeMethods.GetPrivateProfileString(strTemp, "DustCollector_RemoteMode_Use", "false", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].DustCollectorRemoteMode_Use = Convert.ToBoolean(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "DustCollector_Frequency_Upper", "20.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Upper = Convert.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "DustCollector_Frequency_Lower", "20.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].DustCollectorFreq_Lower = Convert.ToDouble(temp.ToString());

                // Pre Align
                NativeMethods.GetPrivateProfileString(strTemp, "PreAlignPosX1", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].PreAlignPos1.X = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "PreAlignPosY1", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].PreAlignPos1.Y = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "PreAlignPosX2", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].PreAlignPos2.X = Equipment.ToDouble(temp.ToString());
                NativeMethods.GetPrivateProfileString(strTemp, "PreAlignPosY2", "0.0", temp, 255, strFIle);
                Equipment.stLayerRecipeSet[i].PreAlignPos2.Y = Equipment.ToDouble(temp.ToString());

            }

            return m_bRet;
        }

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
                //  Mask Index (0:None, 1:Mask1, 2:Mask2, 3:Mask3, 4:Mask4)
                NativeMethods.WritePrivateProfileString(strTemp, "MaskIndex", Equipment.stLayerRecipeSet[i].Miscellaneous_MaskIndex.ToString(), strFIle);
                //  BET Position Index (0:0.1X, 1:0.5X, 2:1.0X, 3:1.5X, 4:2.0X)
                NativeMethods.WritePrivateProfileString(strTemp, "BETPositionIndex", Equipment.stLayerRecipeSet[i].Miscellaneous_BETPositionIndex.ToString(), strFIle);
                //  Hole Processing Type (0:Circle, 1:Spiral)
                NativeMethods.WritePrivateProfileString(strTemp, "HoleProcessingType", Equipment.stLayerRecipeSet[i].Miscellaneous_HoleProcessingType.ToString(), strFIle);
                //  Fiducial Align Type (0:Circle Find, 1:Pattern Matching)
                NativeMethods.WritePrivateProfileString(strTemp, "FiducialAlignType", Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialAlignType.ToString(), strFIle);
                //  Fiducial Mark Type (0:Circle, 1:Gold Powder)
                NativeMethods.WritePrivateProfileString(strTemp, "FiducialMarkType", Equipment.stLayerRecipeSet[i].Miscellaneous_FiducialMarkType.ToString(), strFIle);

                //  Process Options
                NativeMethods.WritePrivateProfileString(strTemp, "Socket_Align_Use", Equipment.stLayerRecipeSet[i].ProcessOption_SocketAlign_Use.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Socket_HeightCheck_Use", Equipment.stLayerRecipeSet[i].ProcessOption_SocketHeightCheck_Use.ToString(), strFIle);

                //  Module Information  
                NativeMethods.WritePrivateProfileString(strTemp, "Module_Width", Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Width.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Module_Height", Equipment.stLayerRecipeSet[i].ModuleInformation_Module_Height.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Module_SiliconThickness", Equipment.stLayerRecipeSet[i].ModuleInformation_Silicon_Thickness.ToString(), strFIle);

                //  Spiral Parameter
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_OuterDiameter", Equipment.stLayerRecipeSet[i].SpiralParam_OuterDiameter.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_InnerDiameter", Equipment.stLayerRecipeSet[i].SpiralParam_InnerDiameter.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_Revolutions", Equipment.stLayerRecipeSet[i].SpiralParam_Revolutions.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "Spiral_AngleFactor", Equipment.stLayerRecipeSet[i].SpiralParam_AngleFactor.ToString(), strFIle);

                //  M-Aligner Vacuum Use
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Center", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Center.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Inner", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Inner.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "MAlignerVacuumUse_Outer", Equipment.stLayerRecipeSet[i].MAligner_VacuumPos_Outer.ToString(), strFIle);

                //  Fine Cam. Red
                NativeMethods.WritePrivateProfileString(strTemp, "FineCam_Red", Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamRed.ToString(), strFIle);
                //  Fine Cam. IR
                NativeMethods.WritePrivateProfileString(strTemp, "FineCam_IR", Equipment.stLayerRecipeSet[i].IlluminatorValue_FineCamIR.ToString(), strFIle);
                //  Coarse Cam. IR
                NativeMethods.WritePrivateProfileString(strTemp, "CoarseCam_IR", Equipment.stLayerRecipeSet[i].IlluminatorValue_CoarseCamIR.ToString(), strFIle);

                //  Dust Collector
                NativeMethods.WritePrivateProfileString(strTemp, "DustCollector_RemoteMode_Use", Equipment.stLayerRecipeSet[i].DustCollectorRemoteMode_Use.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "DustCollector_Frequency_Upper", Equipment.stLayerRecipeSet[i].DustCollectorFreq_Upper.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "DustCollector_Frequency_Lower", Equipment.stLayerRecipeSet[i].DustCollectorFreq_Lower.ToString(), strFIle);

                // Pre Align
                NativeMethods.WritePrivateProfileString(strTemp, "PreAlignPosX1", Equipment.stLayerRecipeSet[i].PreAlignPos1.X.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "PreAlignPosY1", Equipment.stLayerRecipeSet[i].PreAlignPos1.Y.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "PreAlignPosX2", Equipment.stLayerRecipeSet[i].PreAlignPos2.X.ToString(), strFIle);
                NativeMethods.WritePrivateProfileString(strTemp, "PreAlignPosY2", Equipment.stLayerRecipeSet[i].PreAlignPos2.Y.ToString(), strFIle);

            }
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
                Recipe_Data_Save(fileName);
                Equipment.Current_Recipe = fileName;

                MessageBox.Show("Recipe Data를 저장하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            //  Layer Index 확인
            if (m_strLayerName == "Hole1")
            {
                m_nLayerIndex = (int)LayerList.Hole1;
            }
            else if (m_strLayerName == "Hole2")
            {
                m_nLayerIndex = (int)LayerList.Hole2;
            }
            else if (m_strLayerName == "Hole3")
            {
                m_nLayerIndex = (int)LayerList.Hole3;
            }
            else if (m_strLayerName == "Hole4")
            {
                m_nLayerIndex = (int)LayerList.Hole4;
            }
            else if (m_strLayerName == "Hole5")
            {
                m_nLayerIndex = (int)LayerList.Hole5;
            }
            else if (m_strLayerName == "Hole6")
            {
                m_nLayerIndex = (int)LayerList.Hole6;
            }
            else if (m_strLayerName == "Hole7")
            {
                m_nLayerIndex = (int)LayerList.Hole7;
            }
            else if (m_strLayerName == "Hole8")
            {
                m_nLayerIndex = (int)LayerList.Hole8;
            }
            else if (m_strLayerName == "Hole9")
            {
                m_nLayerIndex = (int)LayerList.Hole9;
            }
            else if (m_strLayerName == "Hole10")
            {
                m_nLayerIndex = (int)LayerList.Hole10;
            }
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

            //  Drawing File
            Equipment.stLayerRecipeSet[0].DrawingFile = richTextBox_Recipe_TabRecipe_DrawingFile.Text;                  //  Drawing File 은 0번 Layer 에만 저장한다.

            //  Laser Parameter
            Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_PulseWidth = textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text.Length > 0 ? Convert.ToInt32(textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_PulsePeriod = textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text.Length > 0 ? Convert.ToInt32(textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_Frequency = textBox_Recipe_TabRecipe_LaserParam_Frequency.Text.Length > 0 ? Convert.ToInt32(textBox_Recipe_TabRecipe_LaserParam_Frequency.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_DutyCycle = textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text) : 0;

            Equipment.stLayerRecipeSet[m_nLayerIndex].LaserParam_TriggerMode_External = radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_External.Checked;

            //  Process Priority
            Equipment.stLayerRecipeSet[m_nLayerIndex].ProcessPriority_P2P = radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked;

            //  Miscellaneous
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_ReferenceLayer = textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer.Text;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_DefocusingDistance = Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text);
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_Resizing = Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text);
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_HoleDrilling_StartPosDivision = comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text.Length > 0 ? Convert.ToInt32(comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_GroupSplitSize = textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_ScannerDrillingSpeed = textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_ScannerDrillingSpeed.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_ScannerJumpSpeed = textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_ScannerJumpSpeed.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_LaserOnDelay = textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_LaserOnDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_LaserOffDelay = textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_LaserOffDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_MarkDelay = textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_MarkDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_JumpDelay = textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_JumpDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_PolygonDelay = textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_PolygonDelay.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_Drilling_Power = textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_DrillingPower.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_DrillingRepetition = textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text.Length > 0 ? Convert.ToInt16(textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetition.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_DrillingRepetitionBundle = textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text.Length > 0 ? Convert.ToInt16(textBox_Recipe_TabRecipe_Miscellaneous_DrillingRepetitionBundle.Text) : 50;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_RotationAngleArc = textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_RotationAngleWhenArc.Text) : 360.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_P2PDistance = textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text) : 0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_MaskIndex = comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_BETPositionIndex = comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex;
            Equipment.stLayerRecipeSet[m_nLayerIndex].Miscellaneous_HoleProcessingType = comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex;
            Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialAlignType = comboBox_Recipe_TabRecipe_Miscellaneous_FiducialAlignType.SelectedIndex;
            Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialMarkType = comboBox_Recipe_TabRecipe_Miscellaneous_FiducialMarkType.SelectedIndex;

            //  Process Options
            Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use = checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign.Checked;                         //  Socket Align 기능 사용 여부
            Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use = checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck.Checked;             //  Socket Height Check 기능 사용 여부

            //  Module Information
            Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width = textBox_Recipe_TabRecipe_ModuleInformation_Width.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_ModuleInformation_Width.Text) : 125.0;
            Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height = textBox_Recipe_TabRecipe_ModuleInformation_Height.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_ModuleInformation_Height.Text) : 120.0;
            Equipment.stLayerRecipeSet[0].ModuleInformation_Silicon_Thickness = textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text) : 0.0;

            //  Spiral Parameter
            Equipment.stLayerRecipeSet[m_nLayerIndex].SpiralParam_OuterDiameter = textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text) : 0.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].SpiralParam_InnerDiameter = textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text) : 0.0;
            Equipment.stLayerRecipeSet[m_nLayerIndex].SpiralParam_Revolutions = textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text.Length > 0 ? Convert.ToInt32(textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text) : 10;
            Equipment.stLayerRecipeSet[m_nLayerIndex].SpiralParam_AngleFactor = textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text) : 10.0;

            //  M-Aligner Vacuum Use
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center = checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked;     //  Center
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner = checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked;       //  Inner
            Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer = checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked;       //  Outer

            //  조명값 (Fiducial Layer 의 것만 사용한다)
            Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamRed = textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text.Length > 0 ? Convert.ToInt32(textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text) : 0;
            Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamIR = textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text.Length > 0 ? Convert.ToInt32(textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text) : 0;
            Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_CoarseCamIR = textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text.Length > 0 ? Convert.ToInt32(textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text) : 0;

            //  집진기 주파수
            Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use = checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked;                         //  집진기 Remote Mode 사용 여부
            Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper = textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text) : 20.0;
            Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower = textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text) : 20.0;

            //PreAlign
            Equipment.stLayerRecipeSet[0].PreAlignPos1.X =
                textBox_Recipe_TabRecipe_PreAlignPosX1.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_PreAlignPosX1.Text) : 0.0;
            Equipment.stLayerRecipeSet[0].PreAlignPos1.Y =
                textBox_Recipe_TabRecipe_PreAlignPosY1.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_PreAlignPosY1.Text) : 0.0;
            Equipment.stLayerRecipeSet[0].PreAlignPos2.X =
                textBox_Recipe_TabRecipe_PreAlignPosX2.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_PreAlignPosX2.Text) : 0.0;
            Equipment.stLayerRecipeSet[0].PreAlignPos2.Y =
                textBox_Recipe_TabRecipe_PreAlignPosY2.Text.Length > 0 ? Convert.ToDouble(textBox_Recipe_TabRecipe_PreAlignPosY2.Text) : 0.0;


            //  도면 데이터를 가공용 Document 에 적용
            Equipment.EqpSiriusViewer.Document = m_formSiriusEditor.SiriusEditor.Document;

            MessageBox.Show("Recipe Data Apply", "Recipe Data Apply");
        }

        private void button_Recipe_Open_Click(object sender, EventArgs e)
        {
            string fileName;

            if (Equipment.EqpSiriusViewer == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }

            if (workStage.rtc == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
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
                fileName = openFileDialog.FileName;

                //  Recipe Data 로드
                Recipe_Data_Load(fileName);
                Equipment.Current_Recipe = fileName;


                //  Recipe 명 표시
                label_Recipe_FileName.Text = System.IO.Path.GetFileName(fileName);


                //  Recipe 창에 데이터 표시

                //  Drawing File
                richTextBox_Recipe_TabRecipe_DrawingFile.Text = Equipment.stLayerRecipeSet[0].DrawingFile;

                //  Laser Parameter
                textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text = Equipment.stLayerRecipeSet[0].LaserParam_PulseWidth.ToString();
                textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[0].LaserParam_PulsePeriod.ToString();
                textBox_Recipe_TabRecipe_LaserParam_Frequency.Text = Equipment.stLayerRecipeSet[0].LaserParam_Frequency.ToString();
                textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[0].LaserParam_DutyCycle.ToString();

                if (Equipment.stLayerRecipeSet[0].LaserParam_TriggerMode_External)
                {
                    radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_External.Checked = true;
                }
                else
                {
                    radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_Internal.Checked = true;
                }

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
                textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_P2PDistance.ToString();
                //comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_MaskIndex.ToString();
                //comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString();
                comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_MaskIndex.ToString());
                comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString());
                comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_HoleProcessingType.ToString());
                comboBox_Recipe_TabRecipe_Miscellaneous_FiducialAlignType.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialAlignType.ToString());
                comboBox_Recipe_TabRecipe_Miscellaneous_FiducialMarkType.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialMarkType.ToString());

                //  process Options
                checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use;                         //  Socket Align 기능 사용 여부
                checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use;             //  Socket Height Check 기능 사용 여부

                //  Module Information
                textBox_Recipe_TabRecipe_ModuleInformation_Width.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width.ToString();
                textBox_Recipe_TabRecipe_ModuleInformation_Height.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height.ToString();
                textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Silicon_Thickness.ToString();

                //  Spiral Parameter
                textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_OuterDiameter.ToString();
                textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_InnerDiameter.ToString();
                textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = Equipment.stLayerRecipeSet[0].SpiralParam_Revolutions.ToString();
                textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = Equipment.stLayerRecipeSet[0].SpiralParam_AngleFactor.ToString();

                //  M-Aligner Vacuum Use
                checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center;     //  Center
                checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner;       //  Inner
                checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer;       //  Outer

                //  Illuminator
                textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamRed.ToString();
                textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamIR.ToString();
                textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_CoarseCamIR.ToString();

                //  집진기 주파수
                checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;                         //  집진기 Remote Mode 사용 여부
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
                textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();

                // PreAlign
                textBox_Recipe_TabRecipe_PreAlignPosX1.Text = Equipment.stLayerRecipeSet[0].PreAlignPos1.X.ToString();
                textBox_Recipe_TabRecipe_PreAlignPosY1.Text = Equipment.stLayerRecipeSet[0].PreAlignPos1.Y.ToString();
                textBox_Recipe_TabRecipe_PreAlignPosX2.Text = Equipment.stLayerRecipeSet[0].PreAlignPos2.X.ToString();
                textBox_Recipe_TabRecipe_PreAlignPosY2.Text = Equipment.stLayerRecipeSet[0].PreAlignPos2.Y.ToString();

                int m_nCount = 0;

                do
                {
                    m_nCount++;
                } while (m_nCount < 1000);
                
                
                //  도면 Import
                m_formSiriusEditor.Import_DrawingFile(richTextBox_Recipe_TabRecipe_DrawingFile.Text);

                Equipment.EqpSiriusViewer.Document = m_formSiriusEditor.SiriusEditor.Document;
                //Equipment.EqpSiriusViewer_Origin.Document = m_formSiriusEditor.SiriusEditor.Document;

                //  자동운전 중 모듈 가공 시 이 위치의 도면파일을 로드한다.
                RecipeOpen_DrawingFilePath = richTextBox_Recipe_TabRecipe_DrawingFile.Text;

                workStage.DrillingData_Parsing();

                //  Layer List 전체 삭제
                listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Clear();

                // Todo : 20250426 확인
                if(m_formSiriusEditor.SiriusEditor.Document != null)
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
            
            if (m_strLayerName == "Hole1")
            {
                m_nIndex = (int)LayerList.Hole1;
            }
            else if (m_strLayerName == "Hole2")
            {
                m_nIndex = (int)LayerList.Hole2;
            }
            else if (m_strLayerName == "Hole3")
            {
                m_nIndex = (int)LayerList.Hole3;
            }
            else if (m_strLayerName == "Hole4")
            {
                m_nIndex = (int)LayerList.Hole4;
            }
            else if (m_strLayerName == "Hole5")
            {
                m_nIndex = (int)LayerList.Hole5;
            }
            else if (m_strLayerName == "Hole6")
            {
                m_nIndex = (int)LayerList.Hole6;
            }
            else if (m_strLayerName == "Hole7")
            {
                m_nIndex = (int)LayerList.Hole7;
            }
            else if (m_strLayerName == "Hole8")
            {
                m_nIndex = (int)LayerList.Hole8;
            }
            else if (m_strLayerName == "Hole9")
            {
                m_nIndex = (int)LayerList.Hole9;
            }
            else if (m_strLayerName == "Hole10")
            {
                m_nIndex = (int)LayerList.Hole10;
            }
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

            //  Laser Parameter
            textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text = Equipment.stLayerRecipeSet[m_nIndex].LaserParam_PulseWidth.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[m_nIndex].LaserParam_PulsePeriod.ToString();
            textBox_Recipe_TabRecipe_LaserParam_Frequency.Text = Equipment.stLayerRecipeSet[m_nIndex].LaserParam_Frequency.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[m_nIndex].LaserParam_DutyCycle.ToString();

            if (Equipment.stLayerRecipeSet[m_nIndex].LaserParam_TriggerMode_External)
            {
                radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_External.Checked = true;
            }
            else
            {
                radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_Internal.Checked = true;
            }

            //  Process Priority
            if (Equipment.stLayerRecipeSet[m_nIndex].ProcessPriority_P2P)
            {
                radioButton_Recipe_TabRecipe_ProcessPriority_P2P.Checked = true; ;
            }
            else
            {
                radioButton_Recipe_TabRecipe_ProcessPriority_PulsePeriod.Checked = true; ;
            }

            //  Miscellaneous
            textBox_Recipe_TabRecipe_Miscellaneous_ReferenceLayer.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_ReferenceLayer;
            textBox_Recipe_TabRecipe_Miscellaneous_DefocusingDistance.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_DefocusingDistance.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_Resizing.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_Resizing.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleDrilling_StartPosDivision.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_HoleDrilling_StartPosDivision.ToString();
            textBox_Recipe_TabRecipe_Miscellaneous_GroupSplitSize.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_GroupSplitSize.ToString();
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
            textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_P2PDistance.ToString();
            //comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_MaskIndex.ToString();
            //comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.Text = Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_BETPositionIndex.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_MaskIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_BETPositionIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_HoleProcessingType.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_FiducialAlignType.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_FiducialAlignType.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_FiducialMarkType.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[m_nIndex].Miscellaneous_FiducialMarkType.ToString());

            //  process Options
            checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use;                         //  Socket Align 기능 사용 여부
            checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use;             //  Socket Height Check 기능 사용 여부

            //  Module Information
            textBox_Recipe_TabRecipe_ModuleInformation_Width.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_Height.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Silicon_Thickness.ToString();

            //  Spiral Parameter
            textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = Equipment.stLayerRecipeSet[m_nIndex].SpiralParam_OuterDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = Equipment.stLayerRecipeSet[m_nIndex].SpiralParam_InnerDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = Equipment.stLayerRecipeSet[m_nIndex].SpiralParam_Revolutions.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = Equipment.stLayerRecipeSet[m_nIndex].SpiralParam_AngleFactor.ToString();

            //  M-Aligner Vacuum Use
            checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center;     //  Center
            checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner;       //  Inner
            checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer;       //  Outer

            //  Illuminator
            textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamRed.ToString();
            textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamIR.ToString();
            textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_CoarseCamIR.ToString();

            //  집진기 주파수
            checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;                         //  집진기 Remote Mode 사용 여부
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();

            // PreAlign
            textBox_Recipe_TabRecipe_PreAlignPosX1.Text = Equipment.stLayerRecipeSet[0].PreAlignPos1.X.ToString();
            textBox_Recipe_TabRecipe_PreAlignPosY1.Text = Equipment.stLayerRecipeSet[0].PreAlignPos1.Y.ToString();
            textBox_Recipe_TabRecipe_PreAlignPosX2.Text = Equipment.stLayerRecipeSet[0].PreAlignPos2.X.ToString();
            textBox_Recipe_TabRecipe_PreAlignPosY2.Text = Equipment.stLayerRecipeSet[0].PreAlignPos2.Y.ToString();
        }

        public void Recipe_Open(string m_strRecipeFile)
        {
            string fileName;

            fileName = m_strRecipeFile;

            //  Recipe Data 로드
            Recipe_Data_Load(fileName);
            Equipment.Current_Recipe = fileName;


            //  Recipe 명 표시
            label_Recipe_FileName.Text = System.IO.Path.GetFileName(fileName);


            //  Recipe 창에 데이터 표시

            //  Drawing File
            richTextBox_Recipe_TabRecipe_DrawingFile.Text = Equipment.stLayerRecipeSet[0].DrawingFile;

            //  Laser Parameter
            textBox_Recipe_TabRecipe_LaserParam_PulseWidth.Text = Equipment.stLayerRecipeSet[0].LaserParam_PulseWidth.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[0].LaserParam_PulsePeriod.ToString();
            textBox_Recipe_TabRecipe_LaserParam_Frequency.Text = Equipment.stLayerRecipeSet[0].LaserParam_Frequency.ToString();
            textBox_Recipe_TabRecipe_LaserParam_DutyCycle.Text = Equipment.stLayerRecipeSet[0].LaserParam_DutyCycle.ToString();

            if (Equipment.stLayerRecipeSet[0].LaserParam_TriggerMode_External)
            {
                radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_External.Checked = true;
            }
            else
            {
                radioButton_Recipe_TabRecipe_LaserParam_TriggerMode_Internal.Checked = true;
            }

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
            textBox_Recipe_TabRecipe_Miscellaneous_P2PDistance.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_P2PDistance.ToString();
            //comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_MaskIndex.ToString();
            //comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.Text = Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString();
            comboBox_Recipe_TabRecipe_Miscellaneous_MaskIndex.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_MaskIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_BETPositionIndex.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_BETPositionIndex.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_HoleProcessingType.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_HoleProcessingType.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_FiducialAlignType.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialAlignType.ToString());
            comboBox_Recipe_TabRecipe_Miscellaneous_FiducialMarkType.SelectedIndex = Convert.ToInt16(Equipment.stLayerRecipeSet[0].Miscellaneous_FiducialMarkType.ToString());

            //  process Options
            checkBox_Recipe_TabRecipe_ProcessOptions_SocketAlign.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketAlign_Use;                         //  Socket Align 기능 사용 여부
            checkBox_Recipe_TabRecipe_ProcessOptions_SocketHeightCheck.Checked = Equipment.stLayerRecipeSet[0].ProcessOption_SocketHeightCheck_Use;             //  Socket Height Check 기능 사용 여부

            //  Module Information
            textBox_Recipe_TabRecipe_ModuleInformation_Width.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Width.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_Height.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Module_Height.ToString();
            textBox_Recipe_TabRecipe_ModuleInformation_SiliconThickness.Text = Equipment.stLayerRecipeSet[0].ModuleInformation_Silicon_Thickness.ToString();

            //  Spiral Parameter
            textBox_Recipe_TabRecipe_SpiralParam_OuterDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_OuterDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_InnerDiameter.Text = Equipment.stLayerRecipeSet[0].SpiralParam_InnerDiameter.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_Revolutions.Text = Equipment.stLayerRecipeSet[0].SpiralParam_Revolutions.ToString();
            textBox_Recipe_TabRecipe_SpiralParam_AngleFactor.Text = Equipment.stLayerRecipeSet[0].SpiralParam_AngleFactor.ToString();

            //  M-Aligner Vacuum Use
            checkBox_Recipe_TabRecipe_MAlignVacuum_Center.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Center;     //  Center
            checkBox_Recipe_TabRecipe_MAlignVacuum_Inner.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Inner;       //  Inner
            checkBox_Recipe_TabRecipe_MAlignVacuum_Outer.Checked = Equipment.stLayerRecipeSet[0].MAligner_VacuumPos_Outer;       //  Outer

            //  Illuminator
            textBox_Recipe_TabRecipe_Illuminator_FineCamRed.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamRed.ToString();
            textBox_Recipe_TabRecipe_Illuminator_FineCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_FineCamIR.ToString();
            textBox_Recipe_TabRecipe_Illuminator_CoarseCamIR.Text = Equipment.stLayerRecipeSet[(int)Equipment.LayerList.Fiducial].IlluminatorValue_CoarseCamIR.ToString();

            //  집진기 주파수
            checkBox_Recipe_TabRecipe_ProcessOptions_DustCollector_RemoteMode.Checked = Equipment.stLayerRecipeSet[0].DustCollectorRemoteMode_Use;                         //  집진기 Remote Mode 사용 여부
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Upper.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Upper.ToString();
            textBox_Recipe_TabRecipe_DustCollectorFrequency_Lower.Text = Equipment.stLayerRecipeSet[0].DustCollectorFreq_Lower.ToString();

            // PreAlign
            textBox_Recipe_TabRecipe_PreAlignPosX1.Text = Equipment.stLayerRecipeSet[0].PreAlignPos1.X.ToString();
            textBox_Recipe_TabRecipe_PreAlignPosY1.Text = Equipment.stLayerRecipeSet[0].PreAlignPos1.Y.ToString();
            textBox_Recipe_TabRecipe_PreAlignPosX2.Text = Equipment.stLayerRecipeSet[0].PreAlignPos2.X.ToString();
            textBox_Recipe_TabRecipe_PreAlignPosY2.Text = Equipment.stLayerRecipeSet[0].PreAlignPos2.Y.ToString();

            //  도면 Import
            m_formSiriusEditor.Import_DrawingFile(richTextBox_Recipe_TabRecipe_DrawingFile.Text);

            Equipment.EqpSiriusViewer.Document = m_formSiriusEditor.SiriusEditor.Document;

            workStage.DrillingData_Parsing();

            foreach (var layer in m_formSiriusEditor.SiriusEditor.Document.Layers)
            {
                if (layer.IsMarkerable)
                {
                    listBox_Recipe_TabRecipe_ListOfDrawingLayer.Items.Add(layer.Name);
                }
            }

            MessageBox.Show("Recipe Data를 로드하였습니다.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void richTextBox_Recipe_TabRecipe_DrawingFile_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
