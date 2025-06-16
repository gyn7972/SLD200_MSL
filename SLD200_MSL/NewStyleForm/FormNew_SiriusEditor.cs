using QMC.Common.Modules;
using QMC.Common.Vision.Optics;
using System;
using System.Collections.Generic;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Forms;
using QMC.Common.Parts;
using QMC.Core;
using QMC.Common;
using System.Runtime.InteropServices;
using static QMC.Common.Equipment;
using static QMC.Common.Modules.WorkStage;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

using SpiralLab.Sirius;
using System.Numerics;
using SharpGL;
using System.Xml.Linq;
using SpiralLab;

//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using SpiralLab.Sirius2;
//using SpiralLab.Sirius2.Laser;
//using SpiralLab.Sirius2.PowerMeter;
//using SpiralLab.Sirius2.Scanner;
//using SpiralLab.Sirius2.Scanner.Rtc;
//using SpiralLab.Sirius2.Winforms;
//using SpiralLab.Sirius2.Winforms.Entity;
//using SpiralLab.Sirius2.Winforms.Marker;
//using SpiralLab.Sirius2.Winforms.UI;


namespace SLD200_MSL
{
    public partial class FormNew_SiriusEditor : Form
    {
        private static FormNew_SiriusEditor m_formSiriusEditor = null;

        static WorkStage workStage;

        private System.Windows.Forms.Timer timer_Status;
        public System.Windows.Forms.Timer timer_RtcInit;

        public FormNew_SiriusEditor()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }

            //  Sirius2
            //SpiralLab.Sirius2.Config.DivideArcToLinesDistance = 0.1;                       //  2025. 03. 13.  SCH : Arc 를 표시할 때 얼마나 자세하게 그릴 것인지를 결정하는 값입니다. 값이 작을수록 더 자세하게 그립니다.

            //  Status 타이머
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 50;
            timer_Status.Tick += new System.EventHandler(Timer_Status_Func);
            //timer_Status.Enabled = true;

            //  RTC6 초기화 타이머 (1회만 적용)
            timer_RtcInit = new System.Windows.Forms.Timer();
            timer_RtcInit.Interval = 50;
            timer_RtcInit.Tick += new System.EventHandler(Timer_RtcInit_Func);
            timer_RtcInit.Enabled = true;

            SiriusEditor.OnDocumentSourceChanged += SiriusEditor_OnDocumentSourceChanged;

            textBox_SiriusEditor_Divided_W.Text = Equipment.m_fDividedX.ToString();
            textBox_SiriusEditor_Divided_H.Text = Equipment.m_fDividedY.ToString();
            checkBox_SiriusEditor_Divided.Checked = false;

            //HookEditorToolbarButtons();
            //this.Load += (s, e) => HookEditorToolbarButtons(); // Load 이후 실행
        }

        private void SiriusEditor_OnDocumentSourceChanged(object sender, IDocument doc)
        {
            try
            {
                foreach (var v in SiriusEditor.Document.Views)
                {
                    v.OnCustomDraw -= SiriusView_OnCustomDraw;
                }
            }
            catch (Exception ex)
            {

            }
            SiriusEditor.Document = doc;
            try
            {

                foreach (var v in SiriusEditor.Document.Views)
                {
                    v.OnCustomDraw += SiriusView_OnCustomDraw; ;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SiriusView_OnCustomDraw(IView view)
        {
            foreach (var layer in this.SiriusEditor.Document.Layers)
            {
                if (layer.IsSelected)
                {
                    if (layer.Name.Contains("Hole"))
                    {
                        DrawGrid(view);
                    }
                    else if (layer.Name.Contains("Outline"))
                    {
                        DrawGrid(view, layer);
                    }
                }
            }
        }

        private void DrawGrid(IView view)
        {
            var layer = this.SiriusEditor.Document.Layers.Where(x => x.Name == "Hole1").FirstOrDefault();
            DrawGrid(view, layer);

        }

        private void DrawGrid(IView view, Layer layer)
        {
            if (layer != null)
            {
                if (layer.Name.Contains("Hole1"))
                {
                    foreach (var v in layer.Items)
                    {
                        double width = v.BoundRect.Width;
                        double height = v.BoundRect.Height;
                        double centerX = v.BoundRect.Center.X;
                        double centerY = v.BoundRect.Center.Y;
                        double dSplitW = Equipment.stLayerRecipeSet[(int)LayerList.Hole1].Miscellaneous_GroupSplitSize;
                        double dSplitH = Equipment.stLayerRecipeSet[(int)LayerList.Hole1].Miscellaneous_GroupSplitSize_Height;
                        int colCount = (int)Math.Ceiling(width / dSplitW);
                        int rowCount = (int)Math.Ceiling(height / dSplitH);
                        double dStartX = centerX - (colCount * dSplitW) / 2;
                        double dStartY = centerY - (rowCount * dSplitH) / 2;
                        double dEndX = centerX + (colCount * dSplitW) / 2;
                        double dEndY = centerY + (rowCount * dSplitH) / 2;

                        OpenGL renderer = view.Renderer;
                        // 바둑판의 크기와 간격 설정
                        float squareSize = 10.0f;   // 각 셀의 크기
                        int gridCount = 10;         // 가로, 세로로 그릴 셀의 개수
                        float gridSize = squareSize * gridCount; // 전체 그리드 크기

                        // 라임색 설정
                        renderer.Color(200.0f, 200.0f, 0.0f); // 라임색 (RGB: 0, 255, 0)
                        for (double dX = dStartX; dX <= dEndX; dX += dSplitW)
                        {
                            renderer.Begin(OpenGL.GL_LINES);
                            renderer.Vertex(dX, dStartY, 0.0f);          // 왼쪽 끝
                            renderer.Vertex(dX, dEndY, 0.0f);   // 오른쪽 끝
                            renderer.End();
                        }
                        for (double dY = dStartY; dY <= dEndY; dY += dSplitH)
                        {
                            renderer.Begin(OpenGL.GL_LINES);
                            renderer.Vertex(dStartX, dY, 0.0f);          // 아래쪽 끝
                            renderer.Vertex(dEndX, dY, 0.0f);   // 위쪽 끝
                            renderer.End();
                        }
                    }
                }
                else if (layer.Name.Contains("Outline"))
                {
                    OpenGL renderer = view.Renderer;

                    if (Equipment.stLayerRecipeSet == null ||
                        (int)LayerList.Outline >= Equipment.stLayerRecipeSet.Length)
                    {
                        Log.Write("SiriusEditor", "Outline 레이어의 레시피 정보가 존재하지 않습니다.");
                        return;
                    }

                    double epsilon = 0.01;

                    //if(Equipment.m_bDivided)
                    //{
                    //    float dx = Equipment.m_fDividedX;
                    //    float dy = Equipment.m_fDividedY;
                    //    QMCSiriusEditorForm editorForm = SiriusEditor as QMCSiriusEditorForm;
                    //    editorForm?.RefreshDividedRectsOnly("Outline", dx, dy);
                    //}
                    //else
                    //{
                    //    //Equipment.LastDividedRects.Clear();
                    //}

                    if (Equipment.LastDividedRects.Count > 0)
                    {
                        renderer.Color(200.0f, 200.0f, 0.0f); // Lime color

                        foreach (var rect in Equipment.LastDividedRects)
                        {
                            double left = rect.Left + epsilon;
                            double right = rect.Right - epsilon;
                            double top = rect.Top - epsilon;
                            double bottom = rect.Bottom + epsilon;

                            renderer.Begin(OpenGL.GL_LINE_LOOP);
                            renderer.Vertex(left, bottom, 0.0f);
                            renderer.Vertex(right, bottom, 0.0f);
                            renderer.Vertex(right, top, 0.0f);
                            renderer.Vertex(left, top, 0.0f);
                            renderer.End();
                        }

                        return; // 직접 렌더링 했으므로 함수 종료
                    }

                    var outlineRecipe = Equipment.stLayerRecipeSet[(int)LayerList.Outline];
                    if (outlineRecipe.Miscellaneous_GroupSplitSize <= 0 ||
                        outlineRecipe.Miscellaneous_GroupSplitSize_Height <= 0)
                    {
                        Log.Write("SiriusEditor", "Outline 레이어의 그룹 분할 크기가 유효하지 않습니다.");
                        return;
                    }

                    double dSplitW = outlineRecipe.Miscellaneous_GroupSplitSize;
                    double dSplitH = outlineRecipe.Miscellaneous_GroupSplitSize_Height;
                    //double epsilon = 0.01; // 미세하게 띄워서 시각적으로 겹쳐 보이지 않게 함

                    renderer.Color(200.0f, 200.0f, 0.0f); // 라임색

                    foreach (var entity in layer.Items)
                    {
                        if (entity == null || entity.BoundRect == null)
                            continue;

                        var bounds = entity.BoundRect;

                        double width = bounds.Width;
                        double height = bounds.Height;
                        double centerX = bounds.Center.X;
                        double centerY = bounds.Center.Y;

                        int colCount = Math.Max(1, (int)Math.Ceiling(width / dSplitW));
                        int rowCount = Math.Max(1, (int)Math.Ceiling(height / dSplitH));

                        double offsetX = centerX - (colCount * dSplitW) / 2.0 + dSplitW / 2.0;
                        double offsetY = centerY + (rowCount * dSplitH) / 2.0 - dSplitH / 2.0;

                        for (int row = 0; row < rowCount; row++)
                        {
                            for (int col = 0; col < colCount; col++)
                            {
                                double cellCenterX = offsetX + col * dSplitW;
                                double cellCenterY = offsetY - row * dSplitH;

                                double left = cellCenterX - dSplitW / 2.0 + epsilon;
                                double right = cellCenterX + dSplitW / 2.0 - epsilon;
                                double bottom = cellCenterY - dSplitH / 2.0 + epsilon;
                                double top = cellCenterY + dSplitH / 2.0 - epsilon;

                                renderer.Begin(OpenGL.GL_LINE_LOOP);
                                renderer.Vertex(left, bottom, 0.0f);
                                renderer.Vertex(right, bottom, 0.0f);
                                renderer.Vertex(right, top, 0.0f);
                                renderer.Vertex(left, top, 0.0f);
                                renderer.End();
                            }
                        }
                    }
                }
                //else if (layer.Name.Contains("Outline"))
                //{
                //    OpenGL renderer = view.Renderer;

                //    if (Equipment.stLayerRecipeSet == null ||
                //        (int)LayerList.Outline >= Equipment.stLayerRecipeSet.Length)
                //    {
                //        Log.Write("SiriusEditor", "Outline 레이어의 레시피 정보가 존재하지 않습니다.");
                //        return;
                //    }

                //    var outlineRecipe = Equipment.stLayerRecipeSet[(int)LayerList.Outline];
                //    if (outlineRecipe.Miscellaneous_GroupSplitSize <= 0 ||
                //        outlineRecipe.Miscellaneous_GroupSplitSize_Height <= 0)
                //    {
                //        Log.Write("SiriusEditor", "Outline 레이어의 그룹 분할 크기가 유효하지 않습니다.");
                //        return;
                //    }

                //    double dSplitW = outlineRecipe.Miscellaneous_GroupSplitSize;
                //    double dSplitH = outlineRecipe.Miscellaneous_GroupSplitSize_Height;
                //    double epsilon = 0.01; // 미세하게 띄워서 시각적으로 겹쳐 보이지 않게 함

                //    renderer.Color(200.0f, 200.0f, 0.0f); // 라임색

                //    foreach (var entity in layer.Items)
                //    {
                //        if (entity == null || entity.BoundRect == null)
                //            continue;

                //        var bounds = entity.BoundRect;

                //        double width = bounds.Width;
                //        double height = bounds.Height;
                //        double centerX = bounds.Center.X;
                //        double centerY = bounds.Center.Y;

                //        int colCount = Math.Max(1, (int)Math.Ceiling(width / dSplitW));
                //        int rowCount = Math.Max(1, (int)Math.Ceiling(height / dSplitH));

                //        double offsetX = centerX - (colCount * dSplitW) / 2.0 + dSplitW / 2.0;
                //        double offsetY = centerY + (rowCount * dSplitH) / 2.0 - dSplitH / 2.0;

                //        for (int row = 0; row < rowCount; row++)
                //        {
                //            for (int col = 0; col < colCount; col++)
                //            {
                //                double cellCenterX = offsetX + col * dSplitW;
                //                double cellCenterY = offsetY - row * dSplitH;

                //                double left = cellCenterX - dSplitW / 2.0 + epsilon;
                //                double right = cellCenterX + dSplitW / 2.0 - epsilon;
                //                double bottom = cellCenterY - dSplitH / 2.0 + epsilon;
                //                double top = cellCenterY + dSplitH / 2.0 - epsilon;

                //                renderer.Begin(OpenGL.GL_LINE_LOOP);
                //                renderer.Vertex(left, bottom, 0.0f);
                //                renderer.Vertex(right, bottom, 0.0f);
                //                renderer.Vertex(right, top, 0.0f);
                //                renderer.Vertex(left, top, 0.0f);
                //                renderer.End();
                //            }
                //        }
                //    }
                //}

            }
        }

        public FormNew_SiriusEditor CreateSiriusEditor()
        {
            if (m_formSiriusEditor == null)
            {
                m_formSiriusEditor = new FormNew_SiriusEditor();
            }

            return m_formSiriusEditor;
        }

        public void Import_DrawingFile(string strFileName)
        {
            // Auto인 경우에는 파일명없으면 아에 들어오면 안됨.
            //  Sirius2
            //var doc = DocumentFactory.CreateDefault();
            //doc.ActOpen(strFileName);
            //siriusEditor.Document = doc;
            if (File.Exists(strFileName) == false)
            {
                //MessageBox.Show("도면 파일이 존재하지 않습니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (SiriusEditor.Document == null)
                    SiriusEditor.Document = new DocumentDefault();

                SiriusEditor.Document.FileName = "New Document";
                SiriusEditor.Document.Action.ActNew();
                return;
            }

            // 여기서는 이거 사용하면 안됨. 
            //workStage.Import_DrawingFile(strFileName);

            //  확장자 확인
            string m_strExt = System.IO.Path.GetExtension(strFileName);
            IDocument doc = null;
            try
            {
                if (m_strExt.ToUpper() == ".DXF")
                {
                    doc = DocumentSerializer.OpenDxf(strFileName);
                }
                else if (m_strExt.ToUpper() == ".SIRIUS")
                {
                    doc = DocumentSerializer.OpenSirius(strFileName);
                }

                if (doc != null)
                {
                    // 기존 View 정리
                    if (SiriusEditor.Document != null && SiriusEditor.Document.Views != null)
                        SiriusEditor.Document.Views.Clear();

                    SiriusEditor.Document = doc;
                }
                else
                {
                    Log.Write("SLD-200", "Import_DrawingFile", "문서를 불러올 수 없습니다. 파일 형식이 잘못되었거나 파싱 실패.");
                    //throw new Exception("문서를 불러올 수 없습니다. 파일 형식이 잘못되었거나 파싱 실패.");
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                //MessageBox.Show("도면 파일을 불러오는 중 오류가 발생했습니다.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (SiriusEditor.Document == null)
                    SiriusEditor.Document = new DocumentDefault();

                SiriusEditor.Document.FileName = "New Document";
                SiriusEditor.Document.Action.ActNew();
            }

            //기존 코드
            {
                ////  Sirius1
                //if (m_strExt.ToUpper() == ".DXF")
                //{
                //    //SiriusEditor.Document.New();
                //    doc = DocumentSerializer.OpenDxf(strFileName);
                //    SiriusEditor.Document = doc;
                //}
                //else if (m_strExt.ToUpper() == ".SIRIUS")
                //{
                //    //SiriusEditor.Document.New();
                //    doc = DocumentSerializer.OpenSirius(strFileName);
                //}
                //if(doc!=null)
                //{
                //    if (SiriusEditor.Document != null)
                //    {
                //        if (SiriusEditor.Document.Views != null)
                //        {
                //            SiriusEditor.Document.Views.Clear();
                //        }
                //    }
                //    SiriusEditor.Document = doc;
                //}
            }
        }

        public bool Imported_DrawingFile_SameCheck(string strFileName)
        {
            bool bRtn = false;
            try
            {
                if (SiriusEditor.Document == null)
                {
                    return false;
                }

                if (strFileName == SiriusEditor.Document.FileName)
                    bRtn = true;
                else
                    bRtn = false;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            return bRtn;
        }

        private void Timer_Status_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.


        }


        #region RTC Initialize

        public bool Rtc_Init(bool bRetryInit = false)
        {
            bool m_bRet = true;

            if (!bRetryInit)
            {
                //SpiralLab.Sirius.Config.AngleFactor = 50;
                if (Equipment.SiriusDrawing_Rendering_Resolution < 0)
                {
                    SpiralLab.Sirius.Config.AngleFactor = 50;
                }
                else
                {
                    SpiralLab.Sirius.Config.AngleFactor = Equipment.SiriusDrawing_Rendering_Resolution;
                }

                //  Arc 를 Polyline 으로 만들 경우
                Config.LwPolylineBulgeToLines = true;
                Config.LwPolylineBulgeToLineMinThreshold = (float)0.001;
                if (Equipment.Machine_PolylineCurve_Resolution < 1)
                    Config.LwPolylineBulgePrecision = 100;
                else
                    Config.LwPolylineBulgePrecision = Equipment.Machine_PolylineCurve_Resolution;

                m_bRet = SpiralLab.Core.Initialize();                   //  Sirius1
                                                                        // create document
                                                                        // 신규 문서 생성
                var doc = new DocumentDefault();                        //  Sirius1
                                                                        //var doc = new DocumentBase();                         //  Sirius2             --> 나중에 수정해야함. 필요하면..
                                                                        // assign document into editor
                                                                        //  변수 초기화 (Laser 에서 사용)
                if (SiriusEditor == null)
                {
                    SiriusEditor = new SpiralLab.Sirius.QMCSiriusEditorForm();
                }
                // 문서 지정
                //this.SiriusViewer.Document = doc;
                this.SiriusEditor.Document = doc;
                // assign document source changed event handler
                // 내부 데이타(IDocument) 가 변경될경우 이를 이벤트 통지를 받는 핸들러 등록
                this.SiriusEditor.OnDocumentSourceChanged += SiriusEditor_OnDocumentSourceChanged1;
            }

            #region RTC 초기화
            //create Rtc for dummy (가상 RTC 카드)
            //var rtc = new RtcVirtual(0); 
            //create Rtc6 controller
            workStage.rtc = new Rtc6(0);
            //Rtc6 Ethernet
            //var rtc = new Rtc6Ethernet(0, "192.168.0.100", "255.255.255.0"); 
            if (Equipment.Machine_LaserType_CO2)                                                                                //  CO2 레이저
            {
                // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm
                float fov = 72.5f;          //  MSL-CO2 장비에서 맞춘 데이터
                // k factor (bits/mm) = 2^20 / fov
                float kfactor = (float)Math.Pow(2, 20) / fov;
                //float kfactor = (float)workStage.Config.ParamConfig.Scanner_KFactor;
                if (kfactor == 0)
                    kfactor = (float)18830.1889;

                //string correctionFile = Equipment.Scanner_Calibration_srcFilePath;
                string correctionFile = "D:\\SLD-200_Parameter\\Cor_200C.ct5";

                if (File.Exists(correctionFile) == false)
                {
                    string m_strPath = string.Format("Scanner Correction 파일이 없습니다.\r\n\r\n[{0}]", correctionFile);
                    MessageBox.Show(m_strPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                // initialize rtc controller
                m_bRet &= workStage.rtc.Initialize(kfactor, LaserMode.Co2, correctionFile); 
            }
            else                                                                                                                //  UV 레이저
            {
                // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm
                float fov = 105.0f;         //  MSL-UV 장비에서 맞춘 데이터
                // k factor (bits/mm) = 2^20 / fov
                float kfactor = (float)Math.Pow(2, 20) / fov;
                if (kfactor == 0)
                    kfactor = (float)18830.1889;
                
                //string correctionFile = Equipment.Scanner_Calibration_srcFilePath;
                string correctionFile = "D:\\SLD-200_Parameter\\Cor_200U.ct5";
                if (File.Exists(correctionFile) == false)
                {
                    string m_strPath = string.Format("Scanner Correction 파일이 없습니다.\r\n\r\n[{0}]", correctionFile);
                    MessageBox.Show(m_strPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                // initialize rtc controller
                m_bRet &= workStage.rtc.Initialize(kfactor, LaserMode.Yag1, correctionFile);
            }

            // basic frequency and pulse width
            // laser frequency : 50KHz, pulse width : 2usec (주파수 50KHz, 펄스폭 2usec)
            m_bRet &= workStage.rtc.CtlFrequency(50 * 1000, 2);
            // basic speed
            // jump and mark speed : 500mm/s (점프, 마크 속도 500mm/s)
            m_bRet &= workStage.rtc.CtlSpeed(500, 500);
            // basic delays
            // scanner and laser delays (스캐너/레이저 지연값 설정)
            m_bRet &= workStage.rtc.CtlDelay(10, 100, 200, 200, 0);

            //  rtc Head Offset
            Vector3 ScannerOffset = new Vector3(0, 0, 0);
            ScannerOffset.X = (float)Equipment.Scanner_HeadOffset_X;
            ScannerOffset.Y = (float)Equipment.Scanner_HeadOffset_Y;
            ScannerOffset.Z = (float)Equipment.Scanner_HeadOffset_Angle;
            workStage.rtc.PrimaryHeadBaseOffset = ScannerOffset;
            #endregion

            #region 레이저 소스 초기화
            // virtual laser source with max 20W power (최대 출력 20W 의 가상 레이저 소스 생성)
            workStage.laser = new LaserVirtual(0, "virtual", 20);
            // assign RTC instance at laser 
            workStage.laser.Rtc = workStage.rtc;                   //  Sirius1
            // initialize laser source
            m_bRet &= workStage.laser.Initialize();
            // set basic power output to 2W
            m_bRet &= workStage.laser.CtlPower(2);
            #endregion

            #region 마커 지정
            workStage.marker = new MarkerDefault(0, " RTC6 Marker ");           //  Sirius1
            #endregion

            this.SiriusEditor.Laser = workStage.laser;
            this.SiriusEditor.Marker = workStage.marker;                        //  Sirius1
            this.SiriusEditor.Rtc = workStage.rtc;

            workStage.InitspiralLabScannerModule();

            return m_bRet;

            //기존코드
            {
                //bool m_bRet = true;

                ////SpiralLab.Sirius.Config.AngleFactor = 50;
                //if (Equipment.SiriusDrawing_Rendering_Resolution < 0)
                //{
                //    SpiralLab.Sirius.Config.AngleFactor = 50;
                //}
                //else
                //{
                //    SpiralLab.Sirius.Config.AngleFactor = Equipment.SiriusDrawing_Rendering_Resolution;
                //}

                ////  Arc 를 Polyline 으로 만들 경우
                //Config.LwPolylineBulgeToLines = true;
                //Config.LwPolylineBulgeToLineMinThreshold = (float)0.001;
                //if (Equipment.Machine_PolylineCurve_Resolution < 1)
                //    Config.LwPolylineBulgePrecision = 100;
                //else
                //    Config.LwPolylineBulgePrecision = Equipment.Machine_PolylineCurve_Resolution;

                //m_bRet = SpiralLab.Core.Initialize();                   //  Sirius1
                ////SpiralLab.Sirius2.Core.Initialize();                  //  Sirius2
                ////this.SiriusEditor.EnablePens = true;
                //// create document
                //// 신규 문서 생성
                //var doc = new DocumentDefault();                        //  Sirius1
                ////var doc = new DocumentBase();                         //  Sirius2             --> 나중에 수정해야함. 필요하면..
                //// assign document into editor

                ////  변수 초기화 (Laser 에서 사용)
                ////if (SiriusViewer == null)
                ////{
                ////    SiriusViewer = new SpiralLab.Sirius.SiriusViewerForm();
                ////}
                //if (SiriusEditor == null)
                //{
                //    SiriusEditor = new SpiralLab.Sirius.QMCSiriusEditorForm();
                //}
                //// 문서 지정
                ////this.SiriusViewer.Document = doc;
                //this.SiriusEditor.Document = doc;


                //// assign document source changed event handler
                //// 내부 데이타(IDocument) 가 변경될경우 이를 이벤트 통지를 받는 핸들러 등록
                //this.SiriusEditor.OnDocumentSourceChanged += SiriusEditor_OnDocumentSourceChanged1;

                //#region RTC 초기화
                ////create Rtc for dummy (가상 RTC 카드)
                ////var rtc = new RtcVirtual(0); 

                ////create Rtc5 controller
                ////var rtc = new Rtc5(0);

                ////create Rtc6 controller
                //workStage.rtc = new Rtc6(0);

                ////Rtc6 Ethernet
                ////var rtc = new Rtc6Ethernet(0, "192.168.0.100", "255.255.255.0"); 

                //if (Equipment.Machine_LaserType_CO2)                                                                                //  CO2 레이저
                //{
                //    // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm
                //    float fov = 72.5f;          //  MSL-CO2 장비에서 맞춘 데이터
                //    // k factor (bits/mm) = 2^20 / fov
                //    float kfactor = (float)Math.Pow(2, 20) / fov;
                //    //float kfactor = (float)workStage.Config.ParamConfig.Scanner_KFactor;
                //    if (kfactor == 0)
                //        kfactor = (float)18830.1889;
                //    //kfactor = (float)18830.1889;
                //    // full path of correction file
                //    //var correctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");

                //    string correctionSrcFile = Equipment.Scanner_Calibration_srcFilePath;
                //    string correctionFile = "D:\\SLD-200_Parameter\\Cor_200C.ct5";

                //    if (File.Exists(correctionFile) == false)
                //    {
                //        string m_strPath = string.Format("Scanner Correction 파일이 없습니다.\r\n\r\n[{0}]", correctionFile);
                //        MessageBox.Show(m_strPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return false;
                //    }

                //    // initialize rtc controller
                //    m_bRet &= workStage.rtc.Initialize(kfactor, LaserMode.Co2, correctionFile);                                                                       //  Sirius1
                //    //if (!workStage.rtc.Initialize(kfactor, LaserMode.Co2, correctionFile))                                                                         //  Sirius1
                //    //{
                //    //    m_bRet &= false;
                //    //    //return false;
                //    //}
                //    //var rtc = ScannerFactory.CreateRtc6(0, kfactor, LaserModes.Yag1, RtcSignalLevels.ActiveHigh, RtcSignalLevels.ActiveHigh, correctionFile);     //  Sirius2
                //}
                //else                                                                                                                //  UV 레이저
                //{
                //    // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm
                //    float fov = 105.0f;         //  MSL-UV 장비에서 맞춘 데이터
                //    // k factor (bits/mm) = 2^20 / fov
                //    float kfactor = (float)Math.Pow(2, 20) / fov;
                //    //float kfactor = (float)workStage.Config.ParamConfig.Scanner_KFactor;
                //    if (kfactor == 0)
                //        kfactor = (float)18830.1889;
                //    //kfactor = (float)18830.1889;
                //    // full path of correction file
                //    //var correctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");

                //    string correctionSrcFile = Equipment.Scanner_Calibration_srcFilePath;
                //    string correctionFile = "D:\\SLD-200_Parameter\\Cor_200U.ct5";
                //    if (File.Exists(correctionFile) == false)
                //    {
                //        string m_strPath = string.Format("Scanner Correction 파일이 없습니다.\r\n\r\n[{0}]", correctionFile);
                //        MessageBox.Show(m_strPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        return false;
                //    }
                //    // initialize rtc controller
                //    m_bRet &= workStage.rtc.Initialize(kfactor, LaserMode.Yag1, correctionFile);
                //    //m_bRet &=  workStage.rtc.CtlLoadCorrectionFile(0, correctionFile);       
                //    //  Sirius1
                //    //if (!workStage.rtc.Initialize(kfactor, LaserMode.Yag1, correctionFile))                                                                         //  Sirius1
                //    //{
                //    //    //return false;
                //    //}
                //    //var rtc = ScannerFactory.CreateRtc6(0, kfactor, LaserModes.Yag1, RtcSignalLevels.ActiveHigh, RtcSignalLevels.ActiveHigh, correctionFile);     //  Sirius2
                //}

                //// basic frequency and pulse width
                //// laser frequency : 50KHz, pulse width : 2usec (주파수 50KHz, 펄스폭 2usec)
                //m_bRet &= workStage.rtc.CtlFrequency(50 * 1000, 2);
                ////if (!workStage.rtc.CtlFrequency(50 * 1000, 2))
                ////{
                ////    //return false;
                ////}

                //// basic speed
                //// jump and mark speed : 500mm/s (점프, 마크 속도 500mm/s)
                //m_bRet &= workStage.rtc.CtlSpeed(500, 500);
                ////if (!workStage.rtc.CtlSpeed(500, 500))
                ////{
                ////    //return false;
                ////}

                //// basic delays
                //// scanner and laser delays (스캐너/레이저 지연값 설정)
                //m_bRet &= workStage.rtc.CtlDelay(10, 100, 200, 200, 0);
                ////if (!workStage.rtc.CtlDelay(10, 100, 200, 200, 0))
                ////{
                ////    //return false;
                ////}

                ////  rtc Head Offset
                //Vector3 ScannerOffset = new Vector3(0, 0, 0);
                //ScannerOffset.X = (float)Equipment.Scanner_HeadOffset_X;
                //ScannerOffset.Y = (float)Equipment.Scanner_HeadOffset_Y;
                //ScannerOffset.Z = (float)Equipment.Scanner_HeadOffset_Angle;
                //workStage.rtc.PrimaryHeadBaseOffset = ScannerOffset;
                //#endregion

                ////this.SiriusEditor.Rtc = workStage.rtc6;

                //#region 레이저 소스 초기화
                //// virtual laser source with max 20W power (최대 출력 20W 의 가상 레이저 소스 생성)
                ////var laser = new LaserVirtual(0, "virtual", 20);
                //workStage.laser = new LaserVirtual(0, "virtual", 20);
                ////var laser = new IPGYLPTypeD(0, "IPG YLP D", 1, 20);
                ////var laser = new IPGYLPTypeE(0, "IPG YLP E", 1, 20);
                ////var laser = new IPGYLPN(0, "IPG YLP N", 1, 100);
                ////var laser = new JPTTypeE(0, "JPT Type E", 1, 20);
                ////var laser = new SPIG4(0, "SPI G3/4", 1, 20);
                ////var laser = new PhotonicsIndustryDX(0, "DX", 1, 20);
                ////var laser = new PhotonicsIndustryRGHAIO(0, "RGHAIO", 1, 20);
                ////var laser = new AdvancedOptoWaveFotia(0, "Fotia", 1, 20);
                ////var laser = new AdvancedOptoWaveAOPico(0, "AOPico", 1, 20);
                ////var laser = new CoherentAviaLX(0, "Avia LX", 1, 20);
                ////var laser = new CoherentDiamondJSeries(0, "Diamond JSeries", "10.0.0.1", 200.0f);
                ////var laser = new CoherentDiamondCSeries(0, "Diamond CSeries", 1, 100.0f);
                ////var laser = new SpectraPhysicsHippo(0, "Hippo", 1, 30);
                ////var laser = new SpectraPhysicsTalon(0, "Talon", 1, 20);

                //// assign RTC instance at laser 
                //workStage.laser.Rtc = workStage.rtc;                   //  Sirius1
                ////workStage.laser.Scanner = workStage.rtc6;               //  Sirius2

                //// initialize laser source
                //m_bRet &= workStage.laser.Initialize();
                ////if (!workStage.laser.Initialize())
                ////{
                ////    //return false;
                ////}

                //// set basic power output to 2W
                //m_bRet &= workStage.laser.CtlPower(2);
                ////if (!workStage.laser.CtlPower(2))
                ////{
                ////    //return false;
                ////}
                //#endregion

                ////this.SiriusEditor.Laser = workStage.laser;                  //  Sirius1

                //#region 마커 지정
                //// create default marker 
                ////marker = new MarkerDefault(0, " SyncAxis Marker ");
                ////var marker = new MarkerDefault(0);
                //workStage.marker = new MarkerDefault(0, " RTC6 Marker ");           //  Sirius1
                ////workStage.marker = new MarkerRtc(0, " RTC6 Marker ");             //  Sirius2
                ////workStage.marker.ScannerRotateAngle = 90.0;                         //  Scanner 가공 Field 를 CCW 방향으로 90도 회전 (Scanner 좌표계와 Stage 좌표계가 일치하지 않음) - 일단 보류. 이걸 하면 Scanner Cal 좌표계가 바뀌기 때문에...

                ////workStage.marker.Laser.Scanner.ScannerRotateAngle = 90.0;                     //  2022. 10. 12.  SCH : Scanner 가공 Field 를 CCW 방향으로 90도 회전
                ////  (SLD-100 은 Scanner 와 Stage 방향이 일치하지 않음. Scanner 가 CW 방향으로 90도 돌아가 있음)
                //// If scanner rotate at 90 deg
                ////rtc.MatrixStack.BaseMatrix = Matrix4x4.CreateRotationZ((float)(90 * Math.PI / 180.0));                //  Sirius2
                //#endregion

                //this.SiriusEditor.Laser = workStage.laser;
                //this.SiriusEditor.Marker = workStage.marker;                        //  Sirius1
                //this.SiriusEditor.Rtc = workStage.rtc;

                //#region RTC extension IO 
                ////// create RTC io 
                ////var rtcExt1DInput = new RtcDInputExt1(rtc, 0, "DIN RTC EXT1");
                ////rtcExt1DInput.Initialize();
                ////var rtcExt1DOutput = new RtcDOutputExt1(rtc, 0, "DOUT RTC EXT1");
                ////rtcExt1DOutput.Initialize();
                ////var rtcExt2DOutput = new RtcDOutputExt2(rtc, 0, "DIN RTC EXT2");
                ////rtcExt2DOutput.Initialize();

                //////rtc 5,6 only
                ////var rtcPin2DInput = new RtcDInput2Pin(rtc, 0, "DIN RTC PIN2");
                ////rtcPin2DInput.Initialize();
                ////var rtcPin2DOutput = new RtcDOutput2Pin(rtc, 0, "DOUT RTC PIN2");
                ////rtcPin2DOutput.Initialize();

                ////this.SiriusEditor.RtcExtension1Input = rtcExt1DInput;
                ////this.SiriusEditor.RtcExtension1Output = rtcExt1DOutput;
                ////this.SiriusEditor.RtcExtension2Output = rtcExt2DOutput;
                ////this.SiriusEditor.RtcPin2Input = rtcPin2DInput;
                ////this.SiriusEditor.RtcPin2Output = rtcPin2DOutput;
                //#endregion

                //#region XYZ 모터
                ////var motorX = new MotorVirtual(0, "X");
                ////motorX.Initialize();
                ////var motorY = new MotorVirtual(1, "Y");
                ////motorY.Initialize();
                ////var motorZ = new MotorVirtual(2, "Z");
                ////motorZ.Initialize();
                ////var motorR = new MotorVirtual(2, "R");
                ////motorR.Initialize();

                ////var motorArray = new IMotor[]
                ////{
                ////motorX,
                ////motorY,
                ////motorZ,
                ////motorR,
                ////};
                ////var motors = new MotorsDefault(0, "Group", motorArray);
                ////this.SiriusEditor.Motors = motors;

                ////var motorZ = new MotorVirtual(0, "Z");
                ////this.SiriusEditor.MotorZ = motorZ;
                //#endregion

                //#region PowerMeter
                ////// 파워메터
                ////var powerMeter = new PowerMeterVirtual(0, "Virtual", laser.MaxPowerWatt);
                //////var powerMeter = new PowerMeterOphir(0, "OphirJuno", "3040875");
                //////var powerMeter = new PowerMeterCoherentPowerMax(0, "CoherentPM", 1);
                //////var powerMeter = new PowerMeterThorLabsPMSeries(0, "PM100USB", "SERIALNO");
                ////powerMeter.Initialize();
                ////this.SiriusEditor.PowerMeter = powerMeter;
                //#endregion

                //#region Powermap
                ////var powerMap = new PowerMapDefault(0, "Virtual", "Watt");
                //////var powerMapFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "powermap", "default.map");
                //////PowerMapSerializer.Open(powerMap, powerMapFile);
                ////this.SiriusEditor.PowerMap = powerMap;
                ////laser.PowerMap = powerMap;
                //#endregion

                //{
                //    ////  Arc 를 Polyline 으로 만들 경우
                //    //if (workStage.Config.ParamConfig.ConvertArcToPolyline)
                //    //{
                //    //    Config.LwPolylineBulgeToLines = true;

                //    //    if (workStage.Config.ParamConfig.ArcToPolyline_Resolution < 1)
                //    //        Config.LwPolylineBulgePrecision = 10;
                //    //    else
                //    //        Config.LwPolylineBulgePrecision = workStage.Config.ParamConfig.ArcToPolyline_Resolution;
                //    //}
                //    //else        //  Arc 를 Bulge 값을 이용해서 Arc 처럼 만들 경우
                //    //{
                //    //    Config.LwPolylineBulgeToLines = false;
                //    //}

                //    ////  Spot Distance Control 설정 Off
                //    //workStage.m_bSpotDistanceControl_On = false;

                //    ////  Spot Distance Value
                //    //workStage.m_dSpot_Distance = 0.0;
                //    //SiriusEditor.Enabled = true;
                //}

                //workStage.InitspiralLabScannerModule();

                //return m_bRet;
            }
          }

        public bool Rtc_Close()
        {
            bool bRtn = false;
            if (workStage.rtc.CtlGetStatus(RtcStatus.Busy))
            {
                // abort marking operation
                workStage.rtc.CtlAbort();
                // wait until busy has finished
                workStage.rtc.CtlBusyWait();
            }
            workStage.rtc.Dispose();
            workStage.laser.Dispose();


            bRtn = true;
            return bRtn;
        }

        private void SiriusEditor_OnDocumentSourceChanged1(object sender, IDocument doc)
        {
            SiriusEditor.Document = doc;
            Equipment.SetEqpSiriusViewerDocument(doc);

            // 이동 비활성화 설정 추가
            //SiriusEditor.Document.Action.ActionMode = ActionModes.Select;
        }

        #endregion


        private void Timer_RtcInit_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.

            //timer_RtcInit.Enabled = false;

            if (Equipment.ScannerMode_Change_byUser == (int)RtcMode.RTC_RTC6)
            {
                //시컨스에서 초기화 했다 안했다 할거니깐.. 죽이면 안됨.
                //timer_RtcInit.Enabled = false;

                Equipment.ScannerMode_Change_byUser = (int)RtcMode.RTC_RTC6_COMPLETE;

                Log.Write("SLD-200", "RTC_Initialize", "Sirius Editor 초기화");

                if(workStage.rtc != null && Equipment._InitDeviceStatus.Scanner)
                {
                    //  이미 RTC 가 초기화 되어 있다면 Rtc 객체를 닫고 다시 초기화 한다.
                    Rtc_Close();
                    Equipment._InitDeviceStatus.Scanner = false;
                    if (Rtc_Init(true))
                    {
                        Equipment._InitDeviceStatus.Scanner = true;
                    }
                    else
                    {
                        Equipment._InitDeviceStatus.Scanner = false;
                        MessageBox.Show("Scanner Board 초기화 실패", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    if (Rtc_Init())
                    {
                        Equipment._InitDeviceStatus.Scanner = true;
                    }
                    else
                    {
                        Equipment._InitDeviceStatus.Scanner = false;
                        MessageBox.Show("Scanner Board 초기화 실패", "Information!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
        }

        private void FormNew_CommunicationTerminal_Shown(object sender, EventArgs e)
        {
            //timer_Status.Enabled = true;
        }

        private void FormNew_CommunicationTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            //timer_Status.Enabled = false;
        }

        private void FormNew_SiriusEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                {
                    //m_formSiriusEditor.Import_DrawingFile(m_formSiriusEditor.SiriusEditor.Document.FileName);
                }

                e.Cancel = true;
                Hide();
            }
        }

        private void button_DataParsing_Click(object sender, EventArgs e)
        {
            //  여기 도면 데이터를 WorkStage 의 Doc 로 넘겨준다.
            //  Sirius2
            //  workStage.siriusEditorUserControl_WorkStage = siriusEditor;
            Equipment.SetEqpSiriusViewerDocument(SiriusEditor.Document);
            if (workStage.DrillingData_Parsing())
            {
                MessageBox.Show("데이터 추출 성공", "Processing Data ...", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Equipment.SetEqpSiriusViewerDocument(SiriusEditor.Document);
                int m_nReturn = workStage.GetDrillingData();
                switch (m_nReturn)
                {
                    case (int)WorkStage.nGetDataResult.GETDATA_SUCCESS:

                        //  Hole1 제외한 나머지 Layer 의 Socket 을 가공할 것인지 여부를 결정하는 Flag 세팅
                        workStage.GetDrillingData_ProcessingFlagCheck();

                        MessageBox.Show("가공 데이터 Parsing 성공", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_FAIL:
                        MessageBox.Show("데이터가 정상적으로 로드 되지 않았습니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_NOT_GROUP:
                        MessageBox.Show("데이터가 Group 이 아닙니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_UNGROUP:
                        MessageBox.Show("데이터를 Group 해제 해야 합니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_LAYERNAME_NG:
                        MessageBox.Show("Layer Name 은 'Hole1~4', 'Rect', 'Outline', 'Marking', 'Fiducial' 5가지만 가능합니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_MOTIONTYPE_NG:
                        MessageBox.Show("Layer Motion Type 은 'StageAndScanner', 'ScannerOnly' 2가지만 가능합니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NG:
                        MessageBox.Show("Drilling Data 는 Polyline, Rectangle, Line, Circle, Arc 중 한 가지 데이터로만 구성되어야 합니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_LINECNT:
                        MessageBox.Show("Drilling Data 에 Line 데이터 개수가 4의 배수가 아닙니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NOT_CLOSED:
                        MessageBox.Show("Line 으로 이루어진 Drilling Data 가 닫힌 도형이 아닙니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_DRILDATA_NOT_GROUP:
                        MessageBox.Show("Drilling Data 가 Group 이 아닙니다.");
                        break;

                    case (int)WorkStage.nGetDataResult.GETDATA_RTCINIT:
                        MessageBox.Show("RTC 보드가 초기화 되지 않았습니다.", "Information !");
                        break;
                }
            }
            else
            {
                MessageBox.Show("Data Parsing Fail", "Processing Data ...", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        #region Rotation And Offset Move

        public int DrillingData_RotationOffset_Move(double m_dRotCenter_X, double m_dRotCenter_Y, double m_dAngle, double m_dOffsetX, double m_dOffsetY)
        {
            double m_dTemp_RotationCenter_X;
            double m_dTemp_RotationCenter_Y;
            stAlignMark m_stTemp_AlignMark = new stAlignMark();

            bool success = true;

            m_stTemp_AlignMark.dRotationCenter.X = 0.0;
            m_stTemp_AlignMark.dRotationCenter.Y = 0.0;
            m_dTemp_RotationCenter_X = m_dTemp_RotationCenter_Y = 0.0;

            if (SiriusEditor.Document == null)
            {
                MessageBox.Show("도면 데이터를 불러올 Document 가 준비되지 않았습니다.", "Information!!");
                return (int)nGetDataResult.GETDATA_FAIL;
            }

            if (SiriusEditor == null)
            {
                //  Sirius2
                //siriusEditor = new SpiralLab.Sirius2.Winforms.UI.SiriusEditorUserControl();
            }

            if (SiriusEditor.Document == null)
            {
                MessageBox.Show("도면 데이터 임시 저장용 Document 가 준비되지 않았습니다.", "Information!!");
                return (int)nGetDataResult.GETDATA_FAIL;
            }

            //  전체 가공 객체 개수
            int m_nTotalCount = 0;
            foreach (var layer in SiriusEditor.Document.Layers)
            {
                m_nTotalCount += layer.Count;
            }

            ////  Align Mark 위치 확인
            //foreach (var layer in siriusEditor.Document.InternalData.Layers)
            //{
            //    ///////////////////////
            //    ///                 ///
            //    ///     쓰루홀      ///
            //    ///                 ///
            //    ///////////////////////
            //    if (layer.Name == "쓰루홀")
            //    {
            //        foreach (var entity in layer)
            //        {
            //            switch (entity.EntityType)
            //            {
            //                case EType.Point:
            //                    var point = entity as SpiralLab.Sirius.Point;
            //                    //point.Location 
            //                    //point.DwellTime
            //                    //success &= point.Mark(markerArg);
            //                    break;

            //                case EType.Points:
            //                    var points = entity as Points;
            //                    foreach (var vertex in points)
            //                    {
            //                        //vertex.X
            //                        //vertex.Y
            //                    }
            //                    //points.DwellTime
            //                    //success &= points.Mark(markerArg);
            //                    break;

            //                case EType.Line:
            //                    var line = entity as Line;
            //                    //line.Start
            //                    //line.End
            //                    //success &= line.Mark(markerArg);
            //                    break;

            //                case EType.Arc:
            //                    var arc = entity as Arc;
            //                    //arc.Radius
            //                    //arc.Center
            //                    //arc.StartAngle
            //                    //arc.SweepAngle
            //                    //success &= arc.Mark(markerArg);
            //                    break;

            //                case EType.Circle:
            //                    var circle = entity as Circle;

            //                    if (circle.Description != null)
            //                    {
            //                        if (circle.Description.ToUpper() == "ALIGN1")
            //                        {
            //                            m_stTemp_AlignMark.dAlignMark1.X = circle.Center.X;
            //                            m_stTemp_AlignMark.dAlignMark1.Y = circle.Center.Y;

            //                            m_stTemp_AlignMark.dRotationCenter = m_stTemp_AlignMark.dAlignMark1;
            //                        }
            //                        else if (circle.Description.ToUpper() == "ALIGN2")
            //                        {
            //                            m_stTemp_AlignMark.dAlignMark2.X = circle.Center.X;
            //                            m_stTemp_AlignMark.dAlignMark2.Y = circle.Center.Y;
            //                        }
            //                        else if (circle.Description.ToUpper() == "ALIGN3")
            //                        {
            //                            m_stTemp_AlignMark.dAlignMark3.X = circle.Center.X;
            //                            m_stTemp_AlignMark.dAlignMark3.Y = circle.Center.Y;
            //                        }
            //                        else if (circle.Description.ToUpper() == "ALIGN4")
            //                        {
            //                            m_stTemp_AlignMark.dAlignMark4.X = circle.Center.X;
            //                            m_stTemp_AlignMark.dAlignMark4.Y = circle.Center.Y;
            //                        }
            //                    }
            //                    break;

            //                case EType.Rectangle:
            //                    var rectangle = entity as SpiralLab.Sirius.Rectangle;
            //                    //rectangle.Width
            //                    //rectangle.Height
            //                    //rectangle.Align
            //                    //rectangle.Location
            //                    //success &= rectangle.Mark(markerArg);
            //                    break;

            //                case EType.LWPolyline:
            //                    var lwPolyline = entity as SpiralLab.Sirius.LwPolyline;
            //                    //lwPolyline.IsClosed
            //                    //foreach (var vertex in lwPolyline)
            //                    //{
            //                    //    //vertex.X
            //                    //    //vertex.Y
            //                    //    //vertex.Bulge
            //                    //}
            //                    //success &= lwPolyline.Mark(markerArg);
            //                    break;

            //                case EType.Spiral:
            //                    var spiral = entity as Spiral;
            //                    //spiral.OutterDiameter 
            //                    //spiral.InnerDiameter
            //                    //spiral.RadialPitch
            //                    //spiral.Revolutions
            //                    //spiral.Center
            //                    //success &= spiral.Mark(markerArg);
            //                    break;

            //                case EType.Group:
            //                default:
            //                    var group = entity as Group;

            //                    //success &= group.Mark(markerArg);
            //                    break;
            //                    // case EType....
            //                    // ...

            //                    //default:
            //                    //    if (entity is IMarkerable markerable)
            //                    //    {
            //                    //        // mark entity
            //                    //        // 해당 개체(Entity) 가공 
            //                    //        //success &= markerable.Mark(markerArg);
            //                    //    }
            //                    //    break;
            //            }
            //            if (!success)
            //                break;
            //        }
            //    }
            //}

            //  가공 도면의 Align Mark 1번과 2번간의 각도 계산
            double aX = m_stTemp_AlignMark.dAlignMark1.X;
            double aY = m_stTemp_AlignMark.dAlignMark1.Y;
            double bX = m_stTemp_AlignMark.dAlignMark2.X;
            double bY = m_stTemp_AlignMark.dAlignMark2.Y;

            if ((aX == bX) && (aY == bY))           //  위치값이 이렇게 같을 경우는 문제가 있음.
            {
                m_stTemp_AlignMark.dRotationDegree = 0.0;
            }
            else                                    //  Radian 과 Degree 계산
            {
                double x = bX - aX;
                double y = bY - aY;

                double radian = Math.Atan2(y, x);
                double degree = radian * 180 / Math.PI;

                m_stTemp_AlignMark.dRotationDegree = degree;
            }


            //  도면에 Align1 마크가 없는 경우, 입력한 데이터로 
            if ((m_stTemp_AlignMark.dRotationCenter.X == 0.0) && (m_stTemp_AlignMark.dRotationCenter.Y == 0.0))
            {
                m_stTemp_AlignMark.dRotationCenter.X = m_dRotCenter_X;
                m_stTemp_AlignMark.dRotationCenter.Y = m_dRotCenter_Y;
            }

            //  전체 가공 객체를 List 로 등록
            var list = new List<IEntity>(m_nTotalCount);
            foreach (var layer in SiriusEditor.Document.Layers)
            {
                foreach (var entity in layer)
                {
                    list.Add(entity);
                }
            }

            //  List 에 등록된 가공 객체 Select
            SiriusEditor.Document.Action.ActEntitySelect(list);

            //  가공 객체 회전 이동
            SiriusEditor.Document.Action.ActEntityRotate(SiriusEditor.Document.Action.SelectedEntity, (float)m_dAngle, (float)m_dRotCenter_X, (float)m_dRotCenter_Y);
            SiriusEditor.Document.Action.ActEntityTransit(SiriusEditor.Document.Action.SelectedEntity, (float)m_dOffsetX, (float)m_dOffsetY);


            //  전체 가공 객체를 List 로 등록            
            //foreach (var layer in siriusEditor.Document.InternalData.Layers)
            //{
            //    layer.IsSelected = true;

            //    //  Layer 의 가공 객체를 List 로 등록
            //    var list = new List<IEntity>(layer.ChildCount);

            //    foreach (var entity in layer.Children)
            //    {
            //        list.Add(entity);
            //        entity.IsSelected = true;
            //    }

            //    //  List 를 배열로
            //    IEntity[] testArray = list.ToArray();

            //    //  List 에 등록된 가공 객체를 Group 으로 지정                
            //    siriusEditor.Document.ActGroup(testArray);

            //    Vector3 vector3 = new Vector3((float)0.0, (float)0.0, (float)m_dAngle);
            //    layer.Rotate(vector3);


            //    //siriusEditor.Document.ActRotate(testArray, vector3);

            //    ////  선택된 Entity 삭제
            //    //siriusEditor.Document.ActRemove(testArray);

            //    //siriusEditor.Document.ActAdd(group1);

            //    //var group1 = EntityFactory.CreateGroup("Fiducial", testArray);
            //    //group1.Rotate(vector3);

            //    //siriusEditor.Document.ActGroup(testArray);
            //    //siriusEditor.Document.ActSelect(testArray);

            //    //Vector3 vector3 = new Vector3((float)m_stTemp_AlignMark.dRotationCenter.X, (float)m_stTemp_AlignMark.dRotationCenter.Y, (float)m_dAngle);
            //    //siriusEditor.Document.ActRotate(testArray, vector3);
            //}

            ////  List 에 등록된 가공 객체 Select
            //siriusEditor.Document.Action.ActEntitySelect(list);
            //siriusEditor.Document.Selected;

            //  가공 객체 회전 이동
            //siriusEditor.Document.Action.ActEntityRotate(siriusEditor.Document.Action.SelectedEntity, (float)m_dAngle, (float)m_stTemp_AlignMark.dRotationCenter.X, (float)m_stTemp_AlignMark.dRotationCenter.Y);
            //siriusEditor.Document.Action.ActEntityTransit(siriusEditor.Document.Action.SelectedEntity, (float)m_dOffsetX, (float)m_dOffsetY);

            ////  리스트를 배열로
            //IEntity[] testArray = list.ToArray();

            //Vector3 vector3 = new Vector3((float)m_stTemp_AlignMark.dRotationCenter.X, (float)m_stTemp_AlignMark.dRotationCenter.Y, (float)m_dAngle);
            //siriusEditor.Document.ActRotate(testArray, vector3); 

            return success == true ? (int)nGetDataResult.GETDATA_SUCCESS : (int)nGetDataResult.GETDATA_FAIL;            //   0 : "데이터가 정상적으로 로드 되었습니다."
                                                                                                                        //  -1 : "데이터가 정상적으로 로드 되지 않았습니다."
        }

        #endregion


        /// <summary>
        /// Socket 얼라인 후 객체 회전 이동을 위한 데이터 Select 함수
        /// </summary>
        public bool DrillingData_Select_forAlign(int m_nSocketNum)
        {
            string m_strTemp;
            bool success = true;
            bool LayerIsGroup = false;

            //  SLD-200 에서 사용할 변수
            //  도면 데이터 개수 초기화
            int m_nHole1_ObjectCount = 0;                                       //  Hole1 데이터 개수
            int m_nHole2_ObjectCount = 0;                                       //  Hole2 데이터 개수
            int m_nHole3_ObjectCount = 0;                                       //  Hole3 데이터 개수
            int m_nHole4_ObjectCount = 0;                                       //  Hole4 데이터 개수
            int m_nHole5_ObjectCount = 0;                                       //  Hole5 데이터 개수
            int m_nHole6_ObjectCount = 0;                                       //  Hole6 데이터 개수
            int m_nHole7_ObjectCount = 0;                                       //  Hole7 데이터 개수
            int m_nHole8_ObjectCount = 0;                                       //  Hole8 데이터 개수
            int m_nHole9_ObjectCount = 0;                                       //  Hole9 데이터 개수
            int m_nHole10_ObjectCount = 0;                                      //  Hole10 데이터 개수
            int m_nRect_ObjectCount = 0;                                        //  Rect 데이터 개수
            int m_nOutline_ObjectCount = 0;                                     //  Outline 데이터 개수
            int m_nFiducial_ObjectCount = 0;                                    //  Fiducial 마크 데이터 개수
            int m_nThruhole_ObjectCount = 0;                                    //  Thruhole 데이터 개수
            int m_nMarking_ObjectCount = 0;                                     //  Marking Text 단어 개수

            int m_nLayerCount = 0;

            //  글자 개수 카운트
            int m_nTextCount = 0;

            //  글자를 구성하는 요소 개수 카운트
            int m_nTextItemCount = 0;

            if (SiriusEditor == null)
            {
                MessageBox.Show("먼저 RTC 보드를 초기화 해야 합니다.", "Information!!");
                return false;
            }

            if (SiriusEditor.Document == null)
            {
                MessageBox.Show("도면 데이터를 불러올 Document 가 준비되지 않았습니다.", "Information!!");
                return false;
            }

            int m_nGroupCount = 0;

            //  Layer 개수 체크
            m_nLayerCount = 0;
            //foreach (var layer in siriusEditorUserControl_WorkStage.Document.InternalData.Layers)
            foreach (var layer in SiriusEditor.Document.Layers)
            {
                m_nLayerCount++;
            }

            if (m_nLayerCount == 0)
            {
                MessageBox.Show("Layer 개수가 0 입니다.", "Information!!");
                return false;
            }

            //  회전을 위한 데이터 개수
            int m_nTotalCount = 0;

            //  List 를 몇개를 만들어야 할지
            int m_nListCount = 0;

            //  Layer 종류별 Count
            foreach (var layer in SiriusEditor.Document.Layers)
            {
                if (layer.IsMarkerable && (layer.Count > 0))               //  데이터가 없으면 배열 할당할 필요 없지
                {
                    if (layer.Name == "Hole1")
                    {
                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    //var rectangle = entity as SpiralLab.Sirius2.Winforms.Entity.EntityRectangle;

                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterX = (double)rectangle.ModelTranslate.X;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterY = (double)rectangle.ModelTranslate.Y;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    if (m_nHole1_ObjectCount++ == m_nSocketNum)
                                    {
                                        m_nListCount++;
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Rect")
                    {
                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    var rectangle = entity as SpiralLab.Sirius.Rectangle;

                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterX = (double)rectangle.Center.X;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterY = (double)rectangle.Center.Y;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    if (m_nRect_ObjectCount++ == m_nSocketNum)
                                    {
                                        m_nListCount++;
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Thruhole")
                    {
                        //m_nDrawing_OutlineCount = layer.Count;
                        //m_stDrawing_Outline = new stDrawingOutlineParam[m_nDrawing_OutlineCount];                   //  Outline 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    var rectangle = entity as SpiralLab.Sirius.Rectangle;

                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterX = (double)rectangle.Center.X;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterY = (double)rectangle.Center.Y;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    if (m_nThruhole_ObjectCount++ == m_nSocketNum)
                                    {
                                        m_nListCount++;
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Outline")
                    {
                        //m_nDrawing_OutlineCount = layer.Count;
                        //m_stDrawing_Outline = new stDrawingOutlineParam[m_nDrawing_OutlineCount];                   //  Outline 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    var rectangle = entity as SpiralLab.Sirius.Rectangle;

                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterX = (double)rectangle.Center.X;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterY = (double)rectangle.Center.Y;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    if (m_nOutline_ObjectCount++ == m_nSocketNum)
                                    {
                                        m_nListCount++;
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Marking")
                    {
                        //m_nDrawing_OutlineCount = layer.Count;
                        //m_stDrawing_Outline = new stDrawingOutlineParam[m_nDrawing_OutlineCount];                   //  Outline 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    var rectangle = entity as SpiralLab.Sirius.Rectangle;

                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterX = (double)rectangle.Center.X;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterY = (double)rectangle.Center.Y;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Text:
                                case EType.Barcode1D:
                                case EType.BarcodeDataMatrix:
                                case EType.BarcodeDataMatrix2:
                                case EType.BarcodeQRCode:
                                case EType.BarcodeQRCode2:

                                    if (m_nMarking_ObjectCount++ == m_nSocketNum)
                                    {
                                        m_nListCount++;
                                        break;
                                    }
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    //if (m_nThruhole_ObjectCount++ == m_nSocketNum)
                                    //{
                                    //    m_nListCount++;
                                    //    break;
                                    //}
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Fiducial")
                    {
                        //m_nDrawing_FiducialCount = layer.Count;
                        //m_stDrawing_Fiducial = new stDrawingFiducialParam[m_nDrawing_FiducialCount];                   //  Fiducial 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    //var rectangle = entity as SpiralLab.Sirius2.Winforms.Entity.EntityRectangle;

                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterX = (double)rectangle.ModelTranslate.X;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterY = (double)rectangle.ModelTranslate.Y;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount++].Height = (double)rectangle.Height;
                                    break;
                            }
                        }
                    }
                }
            }

            //  선택해야 할 List 초기화
            var list = new List<IEntity>(m_nListCount);

            //  도면 데이터 개수 초기화
            m_nHole1_ObjectCount = 0;                                       //  Hole1 데이터 개수
            m_nHole2_ObjectCount = 0;                                       //  Hole2 데이터 개수
            m_nHole3_ObjectCount = 0;                                       //  Hole3 데이터 개수
            m_nHole4_ObjectCount = 0;                                       //  Hole4 데이터 개수
            m_nHole5_ObjectCount = 0;                                       //  Hole5 데이터 개수
            m_nHole6_ObjectCount = 0;                                       //  Hole6 데이터 개수
            m_nHole7_ObjectCount = 0;                                       //  Hole7 데이터 개수
            m_nHole8_ObjectCount = 0;                                       //  Hole8 데이터 개수
            m_nHole9_ObjectCount = 0;                                       //  Hole9 데이터 개수
            m_nHole10_ObjectCount = 0;                                      //  Hole10 데이터 개수
            m_nRect_ObjectCount = 0;                                        //  Rect 데이터 개수
            m_nOutline_ObjectCount = 0;                                     //  Outline 데이터 개수
            m_nFiducial_ObjectCount = 0;                                    //  Fiducial 마크 데이터 개수
            m_nThruhole_ObjectCount = 0;                                    //  Thruhole 데이터 개수
            m_nMarking_ObjectCount = 0;                                     //  Marking Text 단어 개수


            //  Layer 종류별 Count
            foreach (var layer in SiriusEditor.Document.Layers)
            {
                if (layer.IsMarkerable && (layer.Count > 0))               //  데이터가 없으면 배열 할당할 필요 없지
                {
                    if (layer.Name == "Hole1")
                    {
                        //m_nDrawing_Hole1Count = layer.Count;
                        //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    //var rectangle = entity as SpiralLab.Sirius2.Winforms.Entity.EntityRectangle;

                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterX = (double)rectangle.ModelTranslate.X;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterY = (double)rectangle.ModelTranslate.Y;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    if (m_nHole1_ObjectCount++ == m_nSocketNum)
                                    {
                                        //  선택한 소켓의 가공 객체를 List 로 등록
                                        list.Add(group);

                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Rect")
                    {
                        //m_nDrawing_RectCount = layer.Count;
                        //m_stDrawing_Rect = new stDrawingRectParam[m_nDrawing_RectCount];                            //  Rect 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    var rectangle = entity as SpiralLab.Sirius.Rectangle;

                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterX = (double)rectangle.Center.X;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterY = (double)rectangle.Center.Y;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    if (m_nRect_ObjectCount++ == m_nSocketNum)
                                    {
                                        //  선택한 소켓의 가공 객체를 List 로 등록
                                        list.Add(group);

                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Thruhole")
                    {
                        //m_nDrawing_OutlineCount = layer.Count;
                        //m_stDrawing_Outline = new stDrawingOutlineParam[m_nDrawing_OutlineCount];                   //  Outline 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    var rectangle = entity as SpiralLab.Sirius.Rectangle;

                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterX = (double)rectangle.Center.X;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterY = (double)rectangle.Center.Y;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    if (m_nThruhole_ObjectCount++ == m_nSocketNum)
                                    {
                                        list.Add(group);
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Outline")
                    {
                        //m_nDrawing_OutlineCount = layer.Count;
                        //m_stDrawing_Outline = new stDrawingOutlineParam[m_nDrawing_OutlineCount];                   //  Outline 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    var rectangle = entity as SpiralLab.Sirius.Rectangle;

                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterX = (double)rectangle.Center.X;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterY = (double)rectangle.Center.Y;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    if (m_nOutline_ObjectCount++ == m_nSocketNum)
                                    {
                                        list.Add(group);
                                        break;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Marking")
                    {
                        //m_nDrawing_OutlineCount = layer.Count;
                        //m_stDrawing_Outline = new stDrawingOutlineParam[m_nDrawing_OutlineCount];                   //  Outline 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Hole1[m_nHole1_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    var rectangle = entity as SpiralLab.Sirius.Rectangle;

                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterX = (double)rectangle.Center.X;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].CenterY = (double)rectangle.Center.Y;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Outline[m_nOutline_ObjectCount++].Height = (double)rectangle.Height;
                                    break;

                                case EType.Text:
                                case EType.Barcode1D:
                                case EType.BarcodeDataMatrix:
                                case EType.BarcodeDataMatrix2:
                                case EType.BarcodeQRCode:
                                case EType.BarcodeQRCode2:

                                    if (m_nMarking_ObjectCount++ == m_nSocketNum)
                                    {
                                        list.Add(entity);
                                        break;
                                    }
                                    break;

                                case EType.Group:
                                    var group = entity as Group;

                                    //m_nDrawing_Hole1Count = group.Count;
                                    //m_stDrawing_Hole1 = new stDrawingHoleParam[m_nDrawing_Hole1Count];                          //  Hole1 데이터

                                    //if (m_nThruhole_ObjectCount++ == m_nSocketNum)
                                    //{
                                    //    m_nListCount++;
                                    //    break;
                                    //}
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Fiducial")
                    {
                        //m_nDrawing_FiducialCount = layer.Count;
                        //m_stDrawing_Fiducial = new stDrawingFiducialParam[m_nDrawing_FiducialCount];                   //  Fiducial 데이터

                        //  데이터 넣기
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Point:
                                    var point = entity as SpiralLab.Sirius.Point;
                                    //point.Location 
                                    //point.DwellTime
                                    //success &= point.Mark(markerArg);
                                    break;

                                case EType.Points:
                                    var points = entity as SpiralLab.Sirius.Points;
                                    //foreach (var vertex in points)
                                    //{
                                    //    //vertex.X
                                    //    //vertex.Y
                                    //}
                                    //points.DwellTime
                                    //success &= points.Mark(markerArg);
                                    break;

                                case EType.Line:
                                    //var line = entity as SpiralLab.Sirius2.Winforms.Entity.EntityLine;

                                    break;

                                case EType.Arc:
                                    var arc = entity as SpiralLab.Sirius.Arc;

                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount].CenterX = (double)arc.Center.X;
                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount].CenterY = (double)arc.Center.Y;
                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount++].radius = (double)arc.Radius;
                                    break;

                                case EType.Circle:
                                    var circle = entity as SpiralLab.Sirius.Circle;

                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount].CenterX = (double)circle.Center.X;
                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount].CenterY = (double)circle.Center.Y;
                                    //m_stDrawing_Fiducial[m_nFiducial_ObjectCount++].radius = (double)circle.Radius;
                                    break;

                                case EType.Rectangle:
                                    //var rectangle = entity as SpiralLab.Sirius2.Winforms.Entity.EntityRectangle;

                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterX = (double)rectangle.ModelTranslate.X;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].CenterY = (double)rectangle.ModelTranslate.Y;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount].Width = (double)rectangle.Width;
                                    //m_stDrawing_Rect[m_nRect_ObjectCount++].Height = (double)rectangle.Height;
                                    break;
                            }
                        }
                    }
                }
            }

            //  List 에 등록된 가공 객체 Select
            if ((list != null) && (m_nListCount > 0))
            {
                SiriusEditor.Document.Action.ActEntitySelect(list);
            }

            return success;
        }


        /// <summary>
        /// 테스트용 코드 : Select 한 데이터만 가져오기 (어디 어디 선택한 건지...?), 나중에 써먹을 지 몰라서 만들어 둠
        /// </summary>
        public bool DrillingData_Select_Check()
        {
            string m_strTemp;
            bool success = true;

            //  실제 가공 객체만 체크
            int m_nHole1_ObjectCount = 0;                                       //  Hole1 데이터 개수
            int m_nOutline_ObjectCount = 0;                                     //  Outline 데이터 개수
            int m_nThruhole_ObjectCount = 0;                                    //  Thruhole 데이터 개수
            int m_nMarking_ObjectCount = 0;                                     //  Marking Text 단어 개수


            if (SiriusEditor == null)
            {
                MessageBox.Show("먼저 RTC 보드를 초기화 해야 합니다.", "Information!!");
                return false;
            }

            if (SiriusEditor.Document == null)
            {
                MessageBox.Show("도면 데이터를 불러올 Document 가 준비되지 않았습니다.", "Information!!");
                return false;
            }

            foreach (var layer in SiriusEditor.Document.Layers)
            {
                if (layer.IsMarkerable && (layer.Count > 0))               //  데이터가 없으면 배열 할당할 필요 없지
                {
                    if (layer.Name == "Hole1")
                    {
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Group:
                                    var group = entity as Group;

                                    if (group.IsSelected)
                                    {
                                        m_nHole1_ObjectCount++;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Thruhole")
                    {
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Group:
                                    var group = entity as Group;

                                    if (group.IsSelected)
                                    {
                                        m_nThruhole_ObjectCount++;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Outline")
                    {
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Group:
                                    var group = entity as Group;

                                    if (group.IsSelected)
                                    {
                                        m_nOutline_ObjectCount++;
                                    }
                                    break;
                            }
                        }
                    }
                    else if (layer.Name == "Marking")
                    {
                        foreach (var entity in layer)
                        {
                            switch (entity.EntityType)
                            {
                                case EType.Text:
                                    var text = entity as SpiralLab.Sirius.Text;

                                    if (text.IsSelected)
                                    {
                                        m_nMarking_ObjectCount++;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }

            //  선택된 Entity 의 개수 확인
            if ((m_nHole1_ObjectCount + m_nOutline_ObjectCount + m_nThruhole_ObjectCount + m_nMarking_ObjectCount) == 0)
            {
                MessageBox.Show("선택된 데이터가 없습니다.", "Information!!");
                return false;
            }

            m_strTemp = string.Format("Hole1 : {0}, Thruhole : {1}, Outline : {2}, Marking : {3}",
                m_nHole1_ObjectCount, m_nThruhole_ObjectCount, m_nOutline_ObjectCount, m_nMarking_ObjectCount);

            MessageBox.Show(m_strTemp);

            return success;
        }

        private void button_Rotate_Click(object sender, EventArgs e)
        {
            //  입력한 소켓 관련 데이터 Select

            int m_nSocketIndex = 0;
            m_nSocketIndex = Equipment.ToInt(tb_SelectSocketNumber.Text);

            if (m_nSocketIndex < 0)
            {
                MessageBox.Show("소켓 번호를 입력하세요.", "Information!!");
                return;
            }

            //workStage.DrillingData_Select_forAlign(m_nSocketIndex);
            DrillingData_Select_forAlign(m_nSocketIndex);

            return;





            //PointD m_ptTemp = new PointD();
            //m_ptTemp.X = 0.0;
            //m_ptTemp.Y = 0.0;

            //PointD[] m_ptResult = workStage.GetClosestCircles(m_ptTemp);


            //DrillingData_RotationOffset_Move(0, 0, 15, 1, -1);


            int m_nRegionIndex = 0;
            workStage.m_nCircleDrilling_CurrentRotStep = 2;

            workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[workStage.m_nDividedRegion_Region_CurrentIndex_forZigZag].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint_PreDrilling[0].X =
                workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[workStage.m_nDividedRegion_Region_CurrentIndex_forZigZag].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint[0].X;
            workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[workStage.m_nDividedRegion_Region_CurrentIndex_forZigZag].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint_PreDrilling[0].Y =
                workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[workStage.m_nDividedRegion_Region_CurrentIndex_forZigZag].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint[0].Y;

            workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[workStage.m_nDividedRegion_Region_CurrentIndex_forZigZag].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint_PreDrilling[1].X =
                workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[workStage.m_nDividedRegion_Region_CurrentIndex_forZigZag].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint[1].X;

            workStage.entity_Position.X = workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nRegionIndex].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint_PreDrilling[0].X -
                                                    workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nRegionIndex].dRegionCenter.X +
                                                    workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nRegionIndex].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint_PreDrilling[1].X;           //  반지름 값
            workStage.entity_Position.Y = workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nRegionIndex].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint[0].Y -
                                                    workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nRegionIndex].dRegionCenter.Y;

            //  시작 위치 각도 분할을 사용할 경우 (매번 분할 각도만큼 이동하여 시작)
            if (workStage.m_nCircleDrilling_CurrentRotStep >= 1)
            {
                workStage.entity_Position_Center.X = workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nRegionIndex].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint[0].X -
                                            workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nRegionIndex].dRegionCenter.X;
                workStage.entity_Position_Center.Y = workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nRegionIndex].m_stDividedRegion_ObjectData[workStage.m_nLaserDrilling_InGroup_HoleCount].dEdgePoint[0].Y -
                                            workStage.m_stDividedRegion_GroupData[workStage.m_nDrillingWork_Group_Count].m_stDividedRegion_RegionData[m_nRegionIndex].dRegionCenter.Y;

                workStage.m_dCircleDrilling_RotDegree = 180.0;

                //entity_Position_Rot = RotatePoint(entity_Position_Center, entity_Position, DegreeToRadian((double)m_nCircleDrilling_CurrentRotStep * m_dCircleDrilling_RotDegree));
                workStage.entity_Position_Rot = RotatePoint_CurPos(workStage.entity_Position_Center, workStage.entity_Position, DegreeToRadian((double)workStage.m_nCircleDrilling_CurrentRotStep * workStage.m_dCircleDrilling_RotDegree));

                workStage.entity_Position = workStage.entity_Position_Rot;
            }
        }

        private void button_Test_OffsetAngle_Calc_Click(object sender, EventArgs e)
        {
            //  테스트

            double m_dRotCenter_X = 0.0;
            double m_dRotCenter_Y = 0.0;
            double m_dAngle = 0.0;
            double m_dOffsetX = 0.0;
            double m_dOffsetY = 0.0;

            int m_nSocketIndex = 0;
            m_nSocketIndex = Equipment.ToInt(tb_SelectSocketNumber.Text);

            if (m_nSocketIndex < 0)
            {
                MessageBox.Show("소켓 번호를 입력하세요.", "Information!!");
                return;
            }

            //if (workStage.m_stDividedRegion_GroupData == null)
            //{
            //    MessageBox.Show("Data Parsing 해야 합니다.", "Information!!");
            //    return;
            //}

            m_dOffsetX = Equipment.ToDouble(tb_ScannerOffset_X.Text);
            m_dOffsetY = Equipment.ToDouble(tb_ScannerOffset_Y.Text);
            m_dAngle = Equipment.ToDouble(tb_ScannerOffset_Angle.Text);

            //m_dRotCenter_X = workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dGroupCenter.X;
            //m_dRotCenter_Y = workStage.m_stDividedRegion_GroupData[m_nSocketIndex].dGroupCenter.Y;
            m_dRotCenter_X = double.Parse(textboxCorX.Text);
            m_dRotCenter_Y = double.Parse(textBoxCorY.Text);



            //workStage.AlignedDrillingData_FailedSocket_Select_and_OffsetMove(m_dRotCenter_X, m_dRotCenter_Y, m_dOffsetX, m_dOffsetY, m_dAngle);


            SiriusEditor.Document.Action.ActEntityRotate(SiriusEditor.Document.Action.SelectedEntity, (float)m_dAngle, (float)m_dRotCenter_X, (float)m_dRotCenter_Y);
            SiriusEditor.Document.Action.ActEntityTransit(SiriusEditor.Document.Action.SelectedEntity, (float)m_dOffsetX, (float)m_dOffsetY);
            return;


            WorkStage.st4PointPosition_Data[] st4PointDwg_Data = new WorkStage.st4PointPosition_Data[4];
            WorkStage.st4PointPosition_Data[] st4PointInspected_Data = new WorkStage.st4PointPosition_Data[4];
            WorkStage.st4PointAlign_Result m_st4PointAlign_Result = new WorkStage.st4PointAlign_Result();

            //  도면 데이터 (순서 : LB -> LT -> RT -> RB)
            st4PointDwg_Data[0].ptFiducial_Center.X = -20.0;
            st4PointDwg_Data[0].ptFiducial_Center.Y = -25.0;
            st4PointDwg_Data[1].ptFiducial_Center.X = -20.0;
            st4PointDwg_Data[1].ptFiducial_Center.Y = 25.0;
            st4PointDwg_Data[2].ptFiducial_Center.X = 20.0;
            st4PointDwg_Data[2].ptFiducial_Center.Y = 25.0;
            st4PointDwg_Data[3].ptFiducial_Center.X = 20.0;
            st4PointDwg_Data[3].ptFiducial_Center.Y = -25.0;

            //  실제 데이터 (순서 : LB -> LT -> RT -> RB)
            st4PointInspected_Data[0].ptFiducial_Center.X = -7.886;
            st4PointInspected_Data[0].ptFiducial_Center.Y = -31.168;
            st4PointInspected_Data[1].ptFiducial_Center.X = -20.827;
            st4PointInspected_Data[1].ptFiducial_Center.Y = 17.128;
            st4PointInspected_Data[2].ptFiducial_Center.X = 17.810;
            st4PointInspected_Data[2].ptFiducial_Center.Y = 27.481;
            st4PointInspected_Data[3].ptFiducial_Center.X = 30.751;
            st4PointInspected_Data[3].ptFiducial_Center.Y = -20.816;

            //  Angle, Offset 계산 (이 값만큼 Dwg 데이터를 보정해서 가공한다.) 
            m_st4PointAlign_Result = workStage.Calc_4Point_AlignData(st4PointDwg_Data, st4PointInspected_Data);
        }

        private void btnScannerOffset_Set_Click(object sender, EventArgs e)
        {
            Vector3 ScannerOffset = new Vector3(0, 0, 0);


            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "Scanner 좌표계가 변경됩니다. 계속 하시겠습니까?"))
                return;


            ScannerOffset.X = (float)Equipment.ToDouble(tb_ScannerOffset_X.Text);
            ScannerOffset.Y = (float)Equipment.ToDouble(tb_ScannerOffset_Y.Text);
            ScannerOffset.Z = (float)Equipment.ToDouble(tb_ScannerOffset_Angle.Text);

            workStage.rtc.PrimaryHeadBaseOffset = ScannerOffset;
        }

        private void SiriusEditor_OnDocumentSave(object sender)
        {
            string fileName = SiriusEditor.Document.FileName;

            // 파일명 포함하여 저장 여부 묻기
            var mb = new MessageBoxYesNo();
            string message = $"저장 하시겠습니까?\n\n파일명: {fileName}";
            if (DialogResult.Yes != mb.ShowDialog("Question ?", message))
                return;

            // 파일명이 없거나 .sirius 확장자가 아니면 강제로 .sirius 확장자로 저장
            if (string.IsNullOrEmpty(fileName) || System.IO.Path.GetExtension(fileName).ToLower() != ".sirius")
            {
                string fileDirectory = string.IsNullOrEmpty(fileName) ? "D:\\Temp" : System.IO.Path.GetDirectoryName(fileName);
                string fileBaseName = string.IsNullOrEmpty(fileName) ? "DefaultSave" : System.IO.Path.GetFileNameWithoutExtension(fileName);
                string newFilePath = System.IO.Path.Combine(fileDirectory, fileBaseName + ".sirius");

                // 경로 없으면 폴더 생성
                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory);

                SiriusEditor.OnSave(newFilePath);
            }
            else
            {
                // 원래 파일명 유지하여 저장
                SiriusEditor.OnSave(fileName);
            }
        }

        private void button_SiriusEditor_Divided_Click(object sender, EventArgs e)
        {
            float fDividedX = 0.0f;
            float fDividedY = 0.0f;

            Equipment.m_bDivided = false;
            if (checkBox_SiriusEditor_Divided.Checked)
            {
                Equipment.LastDividedRects.Clear(); // 이전 셀 정보 제거

                Equipment.m_bDivided = true;    //  분할 여부
                fDividedX = textBox_SiriusEditor_Divided_W.Text == "" ? 0.0f : float.Parse(textBox_SiriusEditor_Divided_W.Text);
                fDividedY = textBox_SiriusEditor_Divided_H.Text == "" ? 0.0f : float.Parse(textBox_SiriusEditor_Divided_H.Text);
                Equipment.m_fDividedX = fDividedX;
                Equipment.m_fDividedY = fDividedY;

                var outlineRecipe = Equipment.stLayerRecipeSet[(int)LayerList.Outline];
                if (outlineRecipe.Miscellaneous_GroupSplitSize <= 0 ||
                    outlineRecipe.Miscellaneous_GroupSplitSize_Height <= 0)
                {
                    Log.Write("SiriusEditor", "Outline 레이어의 그룹 분할 크기가 유효하지 않습니다.");
                    return;
                }
                outlineRecipe.Miscellaneous_GroupSplitSize = Equipment.m_fDividedX;
                outlineRecipe.Miscellaneous_GroupSplitSize_Height = Equipment.m_fDividedY;
                Equipment.stLayerRecipeSet[(int)LayerList.Outline] = outlineRecipe;
            }
            else
            {
                Equipment.LastDividedRects.Clear(); // 이전 셀 정보 제거

                m_bDivided = false;
                m_fDividedX = 0;
                m_fDividedY = 0;
            }

            
        }

        private void SiriusEditor_CausesValidationChanged(object sender, EventArgs e)
        {
            int ntest = 0;
        }

        private void FormNew_SiriusEditor_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void SiriusEditor_OnDocumentOpen(object sender)
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Open File";
            dlg.Filter = "Supported files (*.sirius, *.dxf)|*.sirius;*.dxf|sirius data files (*.sirius)|*.sirius|dxf cad files (*.dxf)|*.dxf|All Files (*.*)|*.*";
            dlg.FileName = string.Empty;
            dlg.Multiselect = false;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    SiriusEditor.OnOpen(dlg.FileName); // 이 시점에 문서 열림
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    MessageBox.Show("입력 문자열의 형식이 잘 못되어 도면 파일을 열 수 없습니다.", "Information !", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                
            }
        }


        private void HookEditorToolbarButtons()
        {
            var type = SiriusEditor.GetType();

            // 모든 ToolStrip을 찾음
            var toolStrips = SiriusEditor.Controls.OfType<ToolStrip>().ToList();

            foreach (var toolStrip in toolStrips)
            {
                foreach (ToolStripItem item in toolStrip.Items)
                {
                    if (item is ToolStripButton btn)
                    {
                        // 예: 버튼 툴팁에 "Bottom to Top"이 포함된 경우
                        if (!string.IsNullOrEmpty(btn.ToolTipText) && btn.ToolTipText.Contains("Bottom"))
                        {
                            btn.Click += (s, e) =>
                            {
                                MessageBox.Show("정렬 버튼 클릭됨: " + btn.ToolTipText);
                                // 여기서 정렬 후 후처리 실행 가능
                            };
                        }

                        // 예: 버튼 이미지로 식별
                        // if (btn.Image != null && btn.ImageIndex == 25) { ... }
                    }
                }
            }
        }

    }
}