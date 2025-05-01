using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision.Optics;
using QMC.Core;
using SpiralLab.Sirius;
using static QMC.Common.Equipment;
using static QMC.Common.Modules.WorkStage;
using Bitmap = System.Drawing.Bitmap;
using static QMC.Common.Modules.Loader;
using static QMC.Common.Modules.Unloader;
using Point = System.Drawing.Point;
using System.Runtime.CompilerServices;

namespace SLD200_MSL
{
    public partial class FormNew_Main : Form
    {
        public FormNew_Recipe RecipeForm;

        public ProgressForm m_FormProgress;                             //  장비 초기화 시 진행창 표시
        public bool m_bHomeProgress_Show;

        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Laser laser;
        static Vision vision;
        static Bds bds;



        //  모듈 진행 상태 표시용 변수 
        private int Rows = 1; // 세로 개수 (기본값)
        private int Columns = 1; // 가로 개수 (기본값)
        private int CellSize_Width = 50; // 각 셀의 크기 (가로)
        private int CellSize_Height = 50; // 각 셀의 크기 (세로)


        // 작업 상태를 저장하는 배열 (0: 미작업, 1: 진행 중, 2: 완료)
        private int[,] pcbStatus; // 작업 상태 배열



        private System.Windows.Forms.Timer timer_Main_Status;

        private Thread m_MainStatusThread;
        private bool m_bMainStatusCycleExit;


        #region Action
        bool m_SiriusViewerRefresy;
        #endregion

        public FormNew_Main()
        {
            InitializeComponent();

            this.Load += FormNew_Main_Load;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;

            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }

                if (module.Name == "Loader")
                {
                    loader = module as Loader;
                }

                if (module.Name == "Unloader")
                {
                    unloader = module as Unloader;
                }

                if (module.Name == "Laser")
                {
                    laser = module as Laser;
                }

                if (module.Name == "Vision")
                {
                    vision = module as Vision;
                }

                if (module.Name == "BDS")
                {
                    bds = module as Bds;
                }
            }

            //  Main Status 타이머
            timer_Main_Status = new System.Windows.Forms.Timer();
            timer_Main_Status.Interval = 100;
            timer_Main_Status.Tick += new System.EventHandler(Timer_MainStatus_Func);
            timer_Main_Status.Enabled = true;

            //ThreadStart();

            //  WorkStage 에서 모듈 할당
            workStage.Module_Allocation();
            unloader.Module_Allocation();
            loader.Module_Allocation();

            //  통신 Parts 초기화 (Connect 옵션에 따라 활성화 된 것들만 초기화 됨)
            Comm_Init();

            m_FormProgress = new ProgressForm("Initialize", "장비 초기화 진행중...");
            m_bHomeProgress_Show = false;

            //  Sirius Viewer
            //workStage.SiriusEditor = new SpiralLab.Sirius.SiriusEditorForm();
            Equipment.EqpSiriusViewer = new SpiralLab.Sirius.SiriusViewerForm();
            Equipment.EqpSiriusViewer_Origin = new SpiralLab.Sirius.SiriusViewerForm();


            //  Fiducial Align Data 를 보여주는 ListView 설정
            listView_Main_FiducialAlignData.View = View.Details;
            listView_Main_FiducialAlignData.GridLines = true;         //  구분선 표시
            listView_Main_FiducialAlignData.FullRowSelect = true;     //  한줄씩 선택 설정


            //  Module Socket Processing 상태를 보여주는 Picture Box
            Initialize_SocketStatus(Columns, Rows); // 초기화
            pictureBox_ModuleProcessingStatus.Paint += PictureBox_ModuleProcessingStatus_Paint;

            //  마크 이미지
            //  scannerCompensator
            string m_strFile = string.Format("{0}\\ScannerCal.bmp", ConfigManager.GetPatternImagePath());
            if (File.Exists(m_strFile))
            {
                workStage.scannerCompensator.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                workStage.scannerCompensator.TrainImage = Bitmap.FromFile(m_strFile); //이거 사용중.
                if (workStage.scannerCompensator != null)
                {
                    workStage.scannerCompensator.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                    workStage.scannerCompensator.TrainImage = Bitmap.FromFile(m_strFile); //이거 사용중.

                    //workStage.PatternMatchingImage_Reticle_Loaded_Upper = true;
                }
            }
            
            m_strFile = string.Format("{0}\\PreAlign.bmp", ConfigManager.GetPatternImagePath());
            if (File.Exists(m_strFile))
            {
                workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                workStage.jigAligner_LowRes.TrainImage = Bitmap.FromFile(m_strFile); //이거 사용중.
                if (workStage.jigAligner_LowRes != null)
                {
                    workStage.jigAligner_LowRes.Recipe.PatternMatchingParameter.TrainImage = Bitmap.FromFile(m_strFile);
                    workStage.jigAligner_LowRes.TrainImage = Bitmap.FromFile(m_strFile); //이거 사용중.

                    //workStage.PatternMatchingImage_Reticle_Loaded_Upper = true;
                }
            }

            //InitImageViewer();

            m_SiriusViewerRefresy = false;
            workStage.ActionSiriusViewerRefresy += OnSiriusViewerRefresy;

        }

        #region Action
        public void OnSiriusViewerRefresy(bool bRtn)
        {
            m_SiriusViewerRefresy = bRtn;
        }
        #endregion

       
        private void InitImageViewer()
        {
            if (this.ImageViewer_Main_highs.IsHandleCreated)
            {
                this.ImageViewer_Main_highs.SizeMode = PictureBoxSizeMode.CenterImage;
                this.ImageViewer_Main_highs.SuspendDisplay();
                this.ImageViewer_Main_highs.StopUpdateTask();

                //Fine은 Workstage Camera와 연동
                this.ImageViewer_Main_highs.Camera = workStage.Camera_HighRes;

                this.ImageViewer_Main_highs.ResumeDisplay();
                this.ImageViewer_Main_highs.StartUpdateTask();
            }

            if (this.ImageViewer_Main_Rows.IsHandleCreated)
            {
                this.ImageViewer_Main_Rows.SizeMode = PictureBoxSizeMode.CenterImage;
                this.ImageViewer_Main_Rows.SuspendDisplay();
                this.ImageViewer_Main_Rows.StopUpdateTask();

                //Prealign은 jigAligner와 연동
                this.ImageViewer_Main_Rows.Camera = workStage.jigAligner_LowRes.Camera;

                this.ImageViewer_Main_Rows.ResumeDisplay();
                this.ImageViewer_Main_Rows.StartUpdateTask();
            }

            // control 초기화 
            checkBox_Main_AutoRun.Text = "MANUAL";
            checkBox_Main_AutoRun.BackColor = Color.LightGray;
            checkBox_Main_AutoRun.ForeColor = Color.Black;
        }


        #region Socket 작업 상황 Display

        // 작업 상태 배열 초기화
        private void Initialize_SocketStatus(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            pcbStatus = new int[Rows, Columns];

            // 모든 상태를 초기화 (0: 미작업)
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    pcbStatus[i, j] = 0;
                }
            }

            //  Cell Size 계산
            CellSize_Width = pictureBox_ModuleProcessingStatus.Width / Columns;
            CellSize_Height = pictureBox_ModuleProcessingStatus.Height / Rows;

            Rows = Rows > 0 ? Rows : 1; // 최소 1행
            Columns = Columns > 0 ? Columns : 1; // 최소 1열
            CellSize_Width = CellSize_Width > 0 ? CellSize_Width : 1; // 최소 1픽셀
            CellSize_Height = CellSize_Height > 0 ? CellSize_Height : 1; // 최소 1픽셀

            // PictureBox 크기 조정
            if (pictureBox_ModuleProcessingStatus != null)
            {
                pictureBox_ModuleProcessingStatus.Width = Columns * CellSize_Width;
                pictureBox_ModuleProcessingStatus.Height = Rows * CellSize_Height;
                pictureBox_ModuleProcessingStatus.Invalidate(); // 다시 그리기
            }
        }

        // 작업 상태 업데이트 메서드
        public void Update_SocketStatus(int row, int column, int status)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                pcbStatus[row, column] = status;
                pictureBox_ModuleProcessingStatus.Invalidate(); // PictureBox 다시 그리기
            }
        }

        // 가로, 세로 배열 크기 변경 메서드
        public void Change_SocketArraySize(int columns, int rows)
        {
            Initialize_SocketStatus(columns, rows);
        }

        private void PictureBox_ModuleProcessingStatus_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Color cellColor;
                    switch (pcbStatus[i, j])
                    {
                        case 0:
                            cellColor = Color.LightGray; // 미작업
                            break;
                        case 1:
                            cellColor = Color.Yellow;   // 진행 중
                            break;
                        case 2:
                            cellColor = Color.Green;    // 완료
                            break;
                        default:
                            cellColor = Color.Red;      // 오류
                            break;
                    }

                    // 셀 그리기
                    using (Brush brush = new SolidBrush(cellColor))
                    {
                        g.FillRectangle(brush, j * CellSize_Width, i * CellSize_Height, CellSize_Width, CellSize_Height);
                    }

                    // 셀 테두리 그리기
                    using (Pen pen = new Pen(Color.Black))
                    {
                        g.DrawRectangle(pen, j * CellSize_Width, i * CellSize_Height, CellSize_Width, CellSize_Height);
                    }
                }
            }
        }

        // 작업 상태 업데이트 메서드
        public void UpdatePCBStatus(int row, int column, int status)
        {
            if (row >= 0 && row < Rows && column >= 0 && column < Columns)
            {
                pcbStatus[row, column] = status;
                pictureBox_ModuleProcessingStatus.Invalidate(); // PictureBox 다시 그리기
            }
        }

        #endregion


        #region Thread

        public void ThreadStart()
        {
            //  Status Cycle
            m_bMainStatusCycleExit = false;
            m_MainStatusThread = new Thread(new ThreadStart(OnMainStatusCycle));
            m_MainStatusThread.Start();
        }

        protected void OnMainStatusCycle()
        {
            while (true)
            {
                if (m_bMainStatusCycleExit)
                {
                    break;
                }
                if (OnMainStatusRun() != 0)
                {
                    break;
                }

                Thread.Sleep(10);
            }
        }

        protected int OnMainStatusRun()
        {
            int ret = 0;

            //  자동 운전 중에 도면 로드하는 쓰레드

            if (Equipment.m_bDrawingFileOpen_1time)
            {
                Equipment.m_bDrawingFileOpen_1time = false;

                workStage.Import_DrawingFile(Equipment.RecipeOpen_DrawingFilePath);

                MessageBox.Show("도면 로드 완료", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            


            //if (m_bHomeProgress_Show && (workStage.m_bHomeOK || workStage.m_bHomeProgressForm_Close))
            //{
            //    workStage.m_bHomeProgressForm_Close = false;
            //    m_bHomeProgress_Show = false;

            //    m_FormProgress.Hide();
            //}

            ////  메인 화면 도면 갱신 (요상스럽도다... 메인 화면에 도면을 불러온 후 다른 화면으로 넘어갔다가 돌아오면, 메인 화면의 Viewer 에 도면이 사라진다. 보이기만 안보이는 게 아니라 데이터도 사라진다. 
            ////                      그래서 Equipment 에 SiriusView 를 하나 임시로 두고, 서로 데이터가 다를 경우(로드된 파일명) 임시 Viewer 의 데이터를 메인 화면의 Viewer 로 가져온다.
            //if ((SiriusViewer_Main.Document != null) && (Equipment.EqpSiriusViewer.Document != null))
            //{
            //    //if ((SiriusViewer_Main.Document.FileName != Equipment.EqpSiriusViewer.Document.FileName) &&
            //    //    (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None))            //  자동운전이 아닐 때만 데이터를 Copy 하도록
            //    if ((SiriusViewer_Main.Document != Equipment.EqpSiriusViewer.Document) &&
            //        (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None))            //  자동운전이 아닐 때만 데이터를 Copy 하도록
            //    {
            //        SiriusViewer_Main.Document = Equipment.EqpSiriusViewer.Document;
            //        //workStage.SiriusEditor.Document = Equipment.EqpSiriusViewer.Document;
            //        //workStage.MainSiriusEditor.Document = Equipment.EqpSiriusViewer.Document;
            //    }
            //}

            return ret;
        }

        public void ThreadStop()
        {
            m_bMainStatusCycleExit = true;

            if (m_MainStatusThread != null)
            {
                m_MainStatusThread.Join();
            }
        }
        #endregion

        public void Comm_Init()
        {
            workStage.Illuminator_Init();

            //  Power Meter (Exit Position) - for UV Only
            if (workStage.m_powerMeter_ExitPos_Comm == null)
            {
                workStage.PowerMeterComm_ExitPos_Init();
            }
            else
            {
                if (!workStage.m_powerMeter_ExitPos_Comm.IsOpen)
                    workStage.PowerMeterComm_ExitPos_Init();
            }

            //  Power Meter (Target Position)
            if (workStage.m_powerMeter_TargetPos_Comm == null)
            {
                workStage.PowerMeterComm_TargetPos_Init();
            }
            else
            {
                if (!workStage.m_powerMeter_TargetPos_Comm.IsOpen)
                    workStage.PowerMeterComm_TargetPos_Init();
            }

            //  Motorized Beam Expander - for CO₂Only
            if (workStage.m_beamExpander_Comm == null)
            {
                workStage.BeamExpanderComm_Init();
            }
            else
            {
                if (!workStage.m_beamExpander_Comm.IsOpen)
                    workStage.BeamExpanderComm_Init();
            }

            //  Dust Collector (Upper Position)
            if (workStage.m_dustCollector_UpperPos_Comm == null)
            {
                workStage.DustCollector_UpperPos_Comm_Init();
            }
            else
            {
                if (!workStage.m_dustCollector_UpperPos_Comm.IsOpen)
                    workStage.DustCollector_UpperPos_Comm_Init();
            }

            //  Dust Collector (Lower Position)
            if (workStage.m_dustCollector_LowerPos_Comm == null)
            {
                workStage.DustCollector_LowerPos_Comm_Init();
            }
            else
            {
                if (!workStage.m_dustCollector_LowerPos_Comm.IsOpen)
                    workStage.DustCollector_LowerPos_Comm_Init();
            }

            //  Electro Pneumatic Regulator
            if (workStage.m_electroRegulator_Comm == null)
            {
                workStage.ElectroPneumaticRegulator_Comm_Init();
            }
            else
            {
                if (!workStage.m_electroRegulator_Comm.IsOpen)
                    workStage.ElectroPneumaticRegulator_Comm_Init();
            }

            //  Laser
            if (workStage.m_SocketLaser == null)
            {
                workStage.Laser_Socket_Connect();
            }

            //  Laser Height Sensor
            if (workStage.m_SocketLaserHeightSensor == null)
            {
                workStage.LaserSensor_Socket_Connect();
            }
        }

        public bool _isMainStatusRunning = false; // 중복 실행 방지 플래그
        private void Timer_MainStatus_Func(object sender, EventArgs e)
        {
            //  동시에 진행되지 않는 함수들만 동일한 타이머로 한다.
            // 중복 실행 방지
            if (_isMainStatusRunning)
            {
                //Console.WriteLine("Scanner Calibration is already running. Skipping this call.");
                return;
            }

            try
            {
                _isMainStatusRunning = true;

                timer_Main_Status.Enabled = false;

                //  Home Progress 창 닫기
                if (m_bHomeProgress_Show && (workStage.m_bHomeOK || workStage.m_bHomeProgressForm_Close))
                {
                    workStage.m_bHomeProgressForm_Close = false;
                    m_bHomeProgress_Show = false;

                    m_FormProgress.Hide();
                }

                //  메인 화면 도면 갱신 (요상스럽도다... 메인 화면에 도면을 불러온 후 다른 화면으로 넘어갔다가 돌아오면, 메인 화면의 Viewer 에 도면이 사라진다. 보이기만 안보이는 게 아니라 데이터도 사라진다. 
                //                      그래서 Equipment 에 SiriusView 를 하나 임시로 두고, 서로 데이터가 다를 경우(로드된 파일명) 임시 Viewer 의 데이터를 메인 화면의 Viewer 로 가져온다.
                if ((SiriusViewer_Main.Document != null) && (Equipment.EqpSiriusViewer.Document != null))
                {
                    //if ((SiriusViewer_Main.Document.FileName != Equipment.EqpSiriusViewer.Document.FileName) &&
                    //    (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None))            //  자동운전이 아닐 때만 데이터를 Copy 하도록

                    //m_SiriusViewerRefresy
                    if ((SiriusViewer_Main.Document != Equipment.EqpSiriusViewer.Document) &&

                        ((workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None) ||
                        m_SiriusViewerRefresy))            //  자동운전이 아닐 때만 데이터를 Copy 하도록
                    {
                        if (m_SiriusViewerRefresy)
                        {
                            m_SiriusViewerRefresy = false;
                        }
                        while (true)
                        {
                            try
                            {
                                if (this.InvokeRequired)
                                {
                                    this.Invoke(new System.Action(() => {
                                        SiriusViewer_Main.Document = Equipment.EqpSiriusViewer.Document;

                                    }));
                                }
                                else
                                {
                                    if(SiriusViewer_Main.Document != Equipment.EqpSiriusViewer.Document)
                                    {
                                        SiriusViewer_Main.Document = (IDocument)Equipment.EqpSiriusViewer.Document;

                                    }
                                }
                                break;
                            }
                            catch (Exception ex)
                            {
                                Log.Write(ex);
                            }
                        }

                        //workStage.SiriusEditor.Document = Equipment.EqpSiriusViewer.Document;
                        //workStage.MainSiriusEditor.Document = Equipment.EqpSiriusViewer.Document;
                    }
                }
                //m_btimer_MainWork_Stop = false;

                //  Main Processing Status 표시
                checkBox_Main_ProcessStatus_LD_LPort_Complete.Checked = Equipment.m_bMainProcessStatus_LD_LPort_Complete;
                checkBox_Main_ProcessStatus_LD_RPort_Complete.Checked = Equipment.m_bMainProcessStatus_LD_RPort_Complete;
                checkBox_Main_ProcessStatus_LD_Module_PortPickUp_Complete.Checked = Equipment.m_bMainProcessStatus_LD_Module_PortPickUp_Complete;
                checkBox_Main_ProcessStatus_LD_Module_MAlignerPutDown_Complete.Checked = Equipment.m_bMainProcessStatus_LD_Module_MAlignerPutDown_Complete;
                checkBox_Main_ProcessStatus_LD_MAlign_Complete.Checked = Equipment.m_bMainProcessStatus_LD_M_Aligner_Align_Complete;
                checkBox_Main_ProcessStatus_LD_Module_MAlignerPickUp_Complete.Checked = Equipment.m_bMainProcessStatus_LD_Module_MAlignerPickUp_Complete;
                checkBox_Main_ProcessStatus_LD_Module_WorkStagePutDown_Complete.Checked = Equipment.m_bMainProcessStatus_LD_Module_WorkStagePutDown_Complete;
                checkBox_Main_ProcessStatus_WorkStage_Module_Process_Complete.Checked = Equipment.m_bMainProcessStatus_WorkStage_Module_Process_Complete;
                checkBox_Main_ProcessStatus_UL_Module_PickUp_Complete.Checked = Equipment.m_bMainProcessStatus_UL_Module_WorkStagePickUp_Complete;
                checkBox_Main_ProcessStatus_UL_Module_PutDown_Complete.Checked = Equipment.m_bMainProcessStatus_UL_Module_PortPutDown_Complete;


		    //  Socket 가공 진행 상태 표시
            if (workStage.Main_SocketPositions_Draw)
            {
                workStage.Main_SocketPositions_Draw = false;

                Change_SocketArraySize(workStage.Main_SocketPositions_ColumnCount, workStage.Main_SocketPositions_RowCount);

                workStage.Main_SocketPositions_Drawed = true;
            }
                
            //  Socket 가공 진행 상태 다시 그리기
            //if (workStage.Main_SocketPositions_Drawed)
            //{
            //    if (workStage.Main_SocketPositions_SetStatus)
            //    {
            //        workStage.Main_SocketPositions_SetStatus = false;

            //        (int rows, int columns) = workStage.GetRowColumnFromPosition(workStage.Main_SocketPositions, workStage.Main_SocketPositions_CurrentSocketPosition);
            //        Update_SocketStatus(rows, columns, workStage.Main_SocketPositions_ProcessingStatus);
            //    }

            //    //  완료된 소켓 상태 표시
            //    if (workStage.Main_SocketPositions_SetCompleteStatus)
            //    {
            //        workStage.Main_SocketPositions_SetCompleteStatus = false;

            //        (int rows, int columns) = workStage.GetRowColumnFromPosition(workStage.Main_SocketPositions, workStage.Main_SocketPositions_SocketCompletePosition);
            //        Update_SocketStatus(rows, columns, workStage.Main_SocketPositions_CompleteStatus);
            //    }
            //}

                //  계속 진행 버튼 활성화
                if (Equipment.MachineStop_byTimeout_Loader)
                {
                    button_Main_Loader_Continue.Enabled = true;
                }
                else
                {
                    button_Main_Loader_Continue.Enabled = false;
                }

                if (Equipment.MachineStop_byTimeout_Unloader)
                {
                    button_Main_Unloader_Continue.Enabled = true;
                }
                else
                {
                    button_Main_Unloader_Continue.Enabled = false;
                }

                if (Equipment.SocketStopped)
                {
                    button_Main_WorkStage_Continue.Enabled = true;
                }
                else
                {
                    button_Main_WorkStage_Continue.Enabled = false;
                }


                //  Cycle Stop 으로 Loader, Unloader, Main Work 가 Stop 되면 자동운전을 종료한다.
                if (Equipment.AutoRunStatus &&

                    Equipment.CycleStop &&
                    Equipment.CycleStopped_LoaderTransfer &&
                    Equipment.CycleStopped_UnloaderTransfer &&
                    Equipment.CycleStopped_MainWork)
                {
                    //Equipment.AutoRunStatus = false;
                    ////  Main Work Timer Stop
                    //workStage.m_btimer_MainWork_Stop = true;
                    //workStage.timer_MainWork.Enabled = false;
                    ////  Laser Drilling Timer Stop
                    //workStage.m_btimer_LaserDrillingWork_Stop = true;
                    //workStage.timer_LaserDrillingWork.Enabled = false;
                    ////  Loader Work Timer Stop
                    //loader.m_btimer_LoaderWork_Stop = true;
                    //loader.timer_LoaderWork.Enabled = false;
                    ////  Unloader Work Timer Stop
                    //unloader.m_btimer_UnloaderWork_Stop = true;
                    //unloader.timer_UnloaderWork.Enabled = false;

                    Equipment.AutoRunStatus = false;

                    workStage.timer_MainWork.Stop();
                    //workStage.timer_MainWork.Enabled = false;
                    workStage.m_MainWork_Start = false;
                    //workStage.m_nMainWork_Step = (int)WorkStage.MainWork_Step.None;

                    loader.timer_LoaderWork.Stop();
                    //loader.timer_LoaderWork.Enabled = false;
                    loader.m_LoaderWork_Start = false;
                    //loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.None;

                    workStage.timer_LaserDrillingWork.Stop();
                    //workStage.timer_LaserDrillingWork.Enabled = false;
                    workStage.m_LaserDrillingWork_Start = false;
                    //workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;

                    unloader.timer_UnloaderWork.Stop();
                    //unloader.timer_UnloaderWork.Enabled = false;
                    unloader.m_UnloaderWork_Start = false;
                    //unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.None;

                    MessageBox.Show("자동 운전 종료", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                if (workStage.Camera_HighRes.Opened)
                {
                    ImageViewer_Main_highs.SetImageNDisplay(workStage.Camera_HighRes.LatestImage);
                }

                if (workStage.jigAligner_LowRes.Camera.Opened)
                {
                    ImageViewer_Main_Rows.SetImageNDisplay(workStage.jigAligner_LowRes.Camera.LatestImage);
                }

                //if (!m_btimer_MainWork_Stop)
                //{
                timer_Main_Status.Enabled = true;
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Timer_Main Status_Elapsed: {ex.Message}");
            }
            finally
            {
                _isMainStatusRunning = false; // 플래그 해제
            }



        }


        private void button_Main_Home_Click(object sender, EventArgs e)
        {
            Log.Write("SLD-200", Equipment.User_Name, "Button Click", "장비 초기화");

            if (!Equipment.AjinBoard_Opened)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "AJIN 모션 제어기가 연결되지 않았습니다.");
                return;
            }

            if (AlarmManager.Instance.IsAlarm)
            {
                // 알람을 클리어 해주세요 메세지
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "알람 해제 바랍니다.");
                return;
            }

            //if (Equipment.User_Mode == null)
            //{
            //    var mb1 = new MessageBoxOk();
            //    mb1.ShowDialog("Information !", "먼저 로그인 하십시오.");
            //    return;
            //}

            if (workStage.m_nHomeStep == (int)WorkStage.Home_Step.None)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "장비를 초기화 하시겠습니까?"))
                    return;

                workStage.Module_Allocation();
                unloader.Module_Allocation();
                loader.Module_Allocation();

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //  이것저것 다 리셋 - 시작
                workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;
                workStage.m_nFindAlignMark_Step = (int)WorkStage.FindAlignMark_Step.None;
                workStage.m_nReticleCheck_HighResCam_Step = (int)WorkStage.ReticleCheck_HighResCam_Step.None;
                workStage.m_nReticleCheck_LowResCam_Step = (int)WorkStage.ReticleCheck_LowResCam_Step.None;
                workStage.m_nSafetyPos_Move_Step = (int)WorkStage.SafetyPos_Move_Step.None;

                workStage.m_bFindAlignMark_OK = false;

                Equipment.MachineStop_byUser = true;


                //  Main Work 타이머
                //workStage.m_btimer_MainWork_Stop = true;                
                //workStage.timer_MainWork.Enabled = false;
                ////  Sub Work 타이머
                //workStage.m_btimer_LaserDrillingWork_Stop = true;
                //workStage.timer_LaserDrillingWork.Enabled = false;
                //workStage.m_btimer_SubWork_Stop = true;
                //workStage.timer_SubWork.Enabled = false;

                workStage.timer_MainWork.Stop();
                workStage.timer_MainWork.Enabled = false;
                workStage.m_MainWork_Start = false;
                //workStage.m_nMainWork_Step = (int)WorkStage.MainWork_Step.None;
                loader.timer_LoaderWork.Stop();
                loader.timer_LoaderWork.Enabled = false;
                loader.m_LoaderWork_Start = false;
                //loader.m_nLoader_Transfer_Step = (int)Loader.Loader_Transfer_Step.None;
                workStage.timer_LaserDrillingWork.Stop();
                workStage.timer_LaserDrillingWork.Enabled = false;
                workStage.m_LaserDrillingWork_Start = false;
                //workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;
                unloader.timer_UnloaderWork.Stop();
                unloader.timer_UnloaderWork.Enabled = false;
                unloader.m_UnloaderWork_Start = false;
                //unloader.m_nUnloader_Transfer_Step = (int)Unloader.Unloader_Transfer_Step.None;


                //  Product Align 타이머
                //workStage.timer_VisionAlign_Stop = true;
                //workStage.timer_VisionAlign.Enabled = false;

                //  Motion 홈 실행 타이머
                workStage.m_btimer_Motion_Home_Stop = true;
                workStage.timer_Motion_Home.Enabled = false;
                workStage.m_MotionHome_Start = false;

                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z1, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_X, 2000);
                loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_Y, 2000);
                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z0, 2000);
                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z1, 2000);
                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_X, 2000);
                unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_Z, 2000);
                workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.X, 2000);
                workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Y, 2000);
                workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Z, 2000);
                if (Equipment.Machine_LaserType_CO2)
                {
                    workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.MASK_Y, 2000);
                }

                //  이것저것 다 리셋 - 끝
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///

                workStage.m_bHomeOK = false;
                m_bHomeProgress_Show = true;
                workStage.m_nHomeStep = (int)WorkStage.Home_Step.Start;

                //  Motion 홈 실행 타이머
                workStage.m_btimer_Motion_Home_Stop = false;
                workStage.timer_Motion_Home.Enabled = true;
                workStage.m_MotionHome_Start = true;

                workStage.m_bHomeProgressForm_Close = false;

                if (!m_FormProgress.HasChildren)            //  Progress 창을 실수로 닫았다면, 다시 메모리 할당하자.
                {
                    m_FormProgress = new ProgressForm("Initialize", "장비 초기화 진행중...");
                }

                m_FormProgress.StartPosition = FormStartPosition.CenterScreen;
                m_FormProgress.TopMost = true;
                m_FormProgress.Show();

                // 문서 생성후 뷰어에 지정
                var doc = new DocumentDefault();
                SiriusViewer_Main.Document = doc;
            }
            else
            {
                try
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "장비 초기화를 중지 하시겠습니까?"))
                        return;

                    //  Motion 홈 실행 타이머
                    workStage.timer_Motion_Home.Enabled = false;
                    workStage.m_btimer_Motion_Home_Stop = true;
                    workStage.timer_Motion_Home.Stop();
                    workStage.m_MotionHome_Start = false;
                    workStage.m_nHomeStep = (int)WorkStage.Home_Step.None;

                    if (Equipment.AjinBoard_Opened)
                    {
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z0, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.Z1, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_X, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.TR_Z, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_X, 2000);
                        loader.MC_Func.MC_MotorStop((int)LoaderParameter.AxisAjinEnum.ALN_Y, 2000);
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z0, 2000);
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.Z1, 2000);
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_X, 2000);
                        unloader.MC_Func.MC_MotorStop((int)UnloaderParameter.AxisAjinEnum.TR_Z, 2000);
                        workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.X, 2000);
                        workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Y, 2000);
                        workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Z, 2000);

                        if (Equipment.Machine_LaserType_CO2)
                        {
                            workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.MASK_Y, 2000);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private void FormNew_Main_Shown(object sender, EventArgs e)
        {
            //  메인 화면 열린 후 타이머 시작
            workStage.timer_Comm.Enabled = true;                                 //  Comm
            workStage.timer_Comm.Start();

            //loader.timer_LoaderWork.Enabled = true;                            //  Loader 
            //unloader.timer_UnloaderWork.Enabled = true;                        //  Unloader
        }

        private void button_Main_Start_Click(object sender, EventArgs e)
        {
            //  Main Work Start
            Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Start 버튼");

            //  Chiller 상태 체크 - Run 신호를 내보내는지
            if (!workStage.workStageParameter.IsDO_Chiller_Run())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Chiller 가 [[ OFF ]] 상태입니다.\r\n\r\nChiller 를 [[ ON ]] 상태로 변경 후 다시 시도 바랍니다.");
                return;
            }

            //  Chiller 상태 체크 - Run 신호를 내보내고 있는데 Run, 신호가 들어오지 않는 경우
            if (workStage.workStageParameter.IsDO_Chiller_Run() && !workStage.workStageParameter.DI_Chiller_Run())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Chiller 가 동작하지 않습니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                return;
            }

            //  Chiller 상태 체크 - Run 신호를 내보내고 있는데, 알람 신호가 들어오는 경우
            if (workStage.workStageParameter.IsDO_Chiller_Run() && !workStage.workStageParameter.DI_Chiller_Alarm_Check())
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Chiller 가 Alarm 상태입니다.\r\n\r\nChiller 상태를 확인 후 다시 시도 바랍니다.");
                return;
            }

            if (Equipment.AutoRunStatus)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "장비가 [[ 운전중 ]] 입니다.");
                return;
            }

            if (!Equipment.AutoManualStatus)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "장비가 [[ AUTO ]] 상태가 아닙니다.");
                return;
            }

            if (checkBox_Test_DryRun.Checked)
            {
                workStage.m_bMainWorkCycle_DryRun = true;
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "[[ Dry Run ]] 을 시작하시겠습니까?\r\n\r\n[Dry Run]"))
                    return;
            }
            else
            {
                workStage.m_bMainWorkCycle_DryRun = false;
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "자동운전을 시작하시겠습니까?"))
                    return;
            }


            // Process Status
            var pos = ProcessManager.GetFirstUnprocessedPosition();
            if (pos.HasValue)
            {
                string layerName = pos.Value.layerName;
                int socketIndex = pos.Value.socketIndex;

                workStage.SetProcess_Layer(layerName);
                workStage.SetProcess_SocketNumber(socketIndex);
                workStage.SetProcessRunning(); //"가공중";
            }
            else
            {
                workStage.SetProcessCompleted(); //"모든 소켓 가공 완료";
            }


            //  여기서 정지 후 재시작시 상태 및 소켓 정보 확인 후 구동
            if (workStage.m_nLaserDrilling_MainStep_Recovery == (int)LaserDrilling_Step.DrillingData_PreAlign_Start)
            {
                //  Pre Align 중이었으니 그대로 시작

                Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Pre Align 부터 다시 시작");

                workStage.m_nLaserDrilling_MainStep = (int)LaserDrilling_Step.DrillingData_PreAlign_Start;
            }
            else if (workStage.m_nLaserDrilling_MainStep_Recovery == (int)LaserDrilling_Step.DrillingData_SocketAlign_Start)
            {
                //  Socket Align 중이었으니 그대로 시작

                Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Socket Align 부터 다시 시작");

                workStage.m_nLaserDrilling_MainStep = (int)LaserDrilling_Step.DrillingData_SocketAlign_Start;
            }
            else if (((workStage.m_nLaserDrilling_MainStep_Recovery >= (int)LaserDrilling_Step.ThruHole_DrillingWork_Start) &&
                    (workStage.m_nLaserDrilling_MainStep_Recovery >= (int)LaserDrilling_Step.ThruHole_DrillingWork_CompleteCheck)) ||

                    ((workStage.m_nLaserDrilling_MainStep_Recovery >= (int)LaserDrilling_Step.OutLine_DrillingWork_Start) &&
                    (workStage.m_nLaserDrilling_MainStep_Recovery >= (int)LaserDrilling_Step.OutLine_DrillingWork_CompleteCheck)) ||

                    ((workStage.m_nLaserDrilling_MainStep_Recovery >= (int)LaserDrilling_Step.Marking_DrillingWork_Start) &&
                    (workStage.m_nLaserDrilling_MainStep_Recovery >= (int)LaserDrilling_Step.Marking_DrillingWork_CompleteCheck)) ||

                    ((workStage.m_nLaserDrilling_MainStep_Recovery >= (int)LaserDrilling_Step.DividedRegion_DrillingWork_Start) &&
                    (workStage.m_nLaserDrilling_MainStep_Recovery >= (int)LaserDrilling_Step.DrillingWork_CompleteCheck)))
            {
                //  가공중이었으니, 다음 소켓 Index 부터 소켓 얼라인 시작

                Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Socket 가공 진행하던 부분 다시 시작");

                //  현재 소켓의 모든 Layer 상태 확인. (하나라도 true 인 게 있으면 다음 소켓 인덱스로 시작)

                //  Drilling 시작 파라미터 설정
                if (!workStage.IsProcessing)
                {
                    //  가공할 것이 없음. --> 강제 종료처럼 밖으로 빼내기

                    Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Socket 가공 진행할 것이 없으므로 Out");

                    workStage.m_bLaserDrilling_Complete = true;
                    workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;
                    workStage.m_nSocketAlign_MainStep = (int)WorkStage.SocketAlign_Step.None;
                }
                else
                {
                    //  가공할 것이 있음.

                    Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Socket 가공 진행할 것이 있음");

                    if (workStage.CurrentLayerName == "Hole1")
                    {
                        for ( int i = 0; i < workStage.m_stLayerType.m_nLayerCount; i++)
                        {
                            if (workStage.m_stLayerType.m_nLayerIndex[i] == (int)LayerList.Hole1)
                            {
                                workStage.m_nLaserDrilling_LayerCount = i;                              //  Layer 이름이 "Hole1" 인 Layer 의 Index 를 넣어줌

                                break;
                            }
                        }

                        workStage.m_nDrillingWork_Group_Count = workStage.CurrentSocketNumber;          //  소켓 번호 설정 (다음 소켓 ???)
                    }
                    else if (workStage.CurrentLayerName == "Thruhole")
                    {
                        for (int i = 0; i < workStage.m_stLayerType.m_nLayerCount; i++)
                        {
                            if (workStage.m_stLayerType.m_nLayerIndex[i] == (int)LayerList.Thruhole)
                            {
                                workStage.m_nLaserDrilling_LayerCount = i;                              //  Layer 이름이 "Thruhole" 인 Layer 의 Index 를 넣어줌

                                break;
                            }
                        }

                        workStage.m_nDrillingWork_Group_Count = workStage.CurrentSocketNumber;          //  소켓 번호 설정 (다음 소켓 ???)
                    }
                    else if (workStage.CurrentLayerName == "Outline")
                    {
                        for (int i = 0; i < workStage.m_stLayerType.m_nLayerCount; i++)
                        {
                            if (workStage.m_stLayerType.m_nLayerIndex[i] == (int)LayerList.Outline)
                            {
                                workStage.m_nLaserDrilling_LayerCount = i;                              //  Layer 이름이 "Outline" 인 Layer 의 Index 를 넣어줌

                                break;
                            }
                        }

                        workStage.m_nDrillingWork_Group_Count = workStage.CurrentSocketNumber;          //  소켓 번호 설정 (다음 소켓 ???)
                    }
                    else if (workStage.CurrentLayerName == "Marking")
                    {
                        for (int i = 0; i < workStage.m_stLayerType.m_nLayerCount; i++)
                        {
                            if (workStage.m_stLayerType.m_nLayerIndex[i] == (int)LayerList.Marking)
                            {
                                workStage.m_nLaserDrilling_LayerCount = i;                              //  Layer 이름이 "Marking" 인 Layer 의 Index 를 넣어줌

                                break;
                            }
                        }

                        workStage.m_nDrillingWork_Group_Count = workStage.CurrentSocketNumber;          //  소켓 번호 설정 (다음 소켓 ???)
                    }

                    workStage.m_nLaserDrilling_MainStep = (int)LaserDrilling_Step.DrillingData_SocketAlign_Start;
                }
            }
            else
            {
                if (!workStage.IsProcessing)
                {
                    //  가공할 것이 없음.

                    Log.Write("SLD-200", Equipment.User_Name, "Button Click", "Socket 가공 완료 상태. 진행할 Socket 없음.");

                    workStage.m_bLaserDrilling_Complete = true;
                    workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;
                    workStage.m_nSocketAlign_MainStep = (int)WorkStage.SocketAlign_Step.None;
                }
                else
                {
                    //  가공은 이미 끝난 상태에서는 여기에 들어오지 않아야 함..

                    Log.Write("SLD-200", Equipment.User_Name, "Button Click", "가공은 이미 끝난 상태에서는 여기에 들어오지 않아야 함.");
                }    
            }


            // 아래 변수가 자동운전 Tick 돌리는 변수임.
            workStage.m_MainWork_Start = true;
            workStage.m_LaserDrillingWork_Start = true;
            workStage.m_ProductAlign_Start = true;
            workStage.m_SubWork_Start = true;
            loader.m_LoaderWork_Start = true;
            unloader.m_UnloaderWork_Start = true;

            button_Main_Start.BackColor = Color.LightGreen;
            button_Main_Start.ForeColor = Color.Black;

            Equipment.AutoRunStatus = true;

            return;


            // 아래 구문 확인 필요. - 선택 가공시 사용?
            string m_strTemp = "";
            if (!workStage.m_bHomeOK)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "먼저 장비 초기화를 해야 합니다.");
                return;
            }

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

            if (Equipment.AutoRunStatus)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "자동 운전 중입니다.");
                return;
            }

            if (Equipment.RecipeOpen_DrawingFilePath.Length == 0)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "도면(레시피)을 로드 하십시오.");
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                //  Laser Defocusing 양 체크 (너무 크면 안됨)
                if (Math.Abs(Equipment.stLayerRecipeSet[i].Miscellaneous_DefocusingDistance) >= 3.0)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "Laser Defocusing 양이 너무 큽니다.\r\n\r\n[-3.0mm < z < 3.0mm] 범위로 조정");
                    i = 4;
                    return;
                }

                //  Resizing 크기 체크
                if (Math.Abs(Equipment.stLayerRecipeSet[i].Miscellaneous_Resizing) >= 0.1)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Information !", "가공 홀 크기 조정량이 너무 큽니다.\r\n\r\n[-0.1mm < z < 0.1mm] 범위로 조정");
                    i = 4;
                    return;
                }
            }

            if (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None)
            {
                var mb = new MessageBoxYesNo();

                ////////////////////////////////////////////////////////////////////////////
                ////  Socket 선택 가공인지 확인용
                ///
                if (workStage.m_stDividedRegion_GroupData == null)              //  Parsing 해야 확인할 수 있는 데이터
                {
                    Log.Write("SLD-200", Equipment.User_Name, "Start Button Click", "Data Parsing 진행. (GetDrillingData)");

                    workStage.GetDrillingData();
                } 

                double m_dSelectedGroup_Center_X = 999.0;
                double m_dSelectedGroup_Center_Y = 999.0;
                foreach (var layer in Equipment.EqpSiriusViewer.Document.Layers)
                {
                    if (layer.IsMarkerable && (layer.Count > 0))               //  데이터가 없으면 배열 할당할 필요 없지
                    {
                        if (layer.Name == "Hole1")
                        {
                            baseTextBox_SocketCountPerModule.Text = layer.Count.ToString();

                            foreach (var entity in layer)
                            {
                                switch (entity.EntityType)
                                {
                                    case EType.Group:
                                        var group = entity as Group;

                                        if (group.IsSelected)
                                        {
                                            m_dSelectedGroup_Center_X = group.Location.X;
                                            m_dSelectedGroup_Center_Y = group.Location.Y;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

                baseTextBox_Socket_Index.Text = "All";
                workStage.m_nSelectedSocket_Index = -1;

				if (workStage.m_stDividedRegion_GroupData != null)
				{
					//  선택된 Socket 이 몇번 Socket 인지 확인
	                for (int i = 0; i < workStage.m_stDividedRegion_GroupData.Length; i++)
	                {
	                    if ((m_dSelectedGroup_Center_X == workStage.m_stDividedRegion_GroupData[i].dGroupCenter.X) &&
	                        (m_dSelectedGroup_Center_Y == workStage.m_stDividedRegion_GroupData[i].dGroupCenter.Y))
	                    {
	                        baseTextBox_Socket_Index.Text = i.ToString();
	                        workStage.m_nSelectedSocket_Index = i;
	                        break;
	                    }
	                }
	         	}
                ////  Socket 선택 가공인지 확인용                
                ////////////////////////////////////////////////////////////////////////////
                //if (laserDrilling.m_nAutoCal_ScannerCamCenter_Step > (int)LaserDrilling.AutoCalScannerCameraCenter_Step.None)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "스캐너와 카메라 Offset 자동 보정 진행중입니다.");
                //    return;
                //}
                //  도면 갱신 (Main 화면의 Sirius Document 를 가공할때 사용하는 Document 로 복사)
                //workStage.SiriusEditor.Document = SiriusViewer_Main.Document;

                Equipment.EqpSiriusViewer.Document = SiriusViewer_Main.Document;     
                if (workStage.m_nSelectedSocket_Index >= 0)
                {
                    m_strTemp = string.Format("선택 가공을 시작하시겠습니까?\r\n\r\n[소켓 번호 : {0}]", workStage.m_nSelectedSocket_Index);
                }
                else
                {
                    if (workStage.Config.ParamConfig.bProductAlign_Enable)
                    {
                        m_strTemp = "전체 가공을 시작하시겠습니까?";
                    }
                    else
                    {
                        m_strTemp = "전체 가공을 시작하시겠습니까?\r\n\r\n[소켓 얼라인 사용 안함]";
                    }
                }

                if (DialogResult.Yes != mb.ShowDialog("Question ?", m_strTemp))
                    return;

                if (!workStage.workStageParameter.IsDO_BeamDump_Coolant_Supply() || !workStage.workStageParameter.IsDO_Scanner_Coolant_Supply() ||
                    (Equipment.Machine_LaserType_CO2 && (!workStage.workStageParameter.IsDO_Mask_Coolant_Supply() || !workStage.workStageParameter.IsDO_VarioScan_Coolant_Supply())))
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Warning !", "냉각수를 순환 시키고 작업을 진행해야 합니다.");
                    return;
                }

                ////  StageZ 한계위치 설정되어 있는지 체크
                //if (laserDrilling.Config.ParamConfig.Interlock_StageZ_UpperPos <= 0.0)
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Stage Z축 한계 높이가 설정되어 있지 않습니다.\r\n\r\n(Config -> [17] Interlock  확인)");
                //    return;
                //}

                //laserDrilling.laserDrillingParameter.stLaserDrillingPosParam = laserDrilling.laserDrillingParameter.GetPositionInformation("WorkStage_WorkHeight");

                ////  StageZ 한계위치를 초과하여 이동하는지 체크
                //if (laserDrilling.Config.ParamConfig.Interlock_StageZ_UpperPos < laserDrilling.laserDrillingParameter.stLaserDrillingPosParam.dTarget[(int)WorkStageParameter.MotionKey.Z])
                //{
                //    var mb1 = new MessageBoxOk();
                //    mb1.ShowDialog("Warning !", "Laser 가공 높이가 Stage Z축 한계 높이를 초과합니다.\r\n\r\n[ Work Cancel ]");
                //    return;
                //}

                ////  가공 시간 초기화
                //Equipment.WorkElapsedTick = 0;
                //Equipment.WorkElapsedTick_Outline = 0;
                //Equipment.WorkElapsedTick_Thruhole = 0;
                //Equipment.WorkElapsedTick_Drilling = 0;
                //Equipment.WorkElapsedTick_Marking = 0;

                Equipment.SeqTestMode = false;

                workStage.m_bLaserDrilling_SocketStopped = false;
                Equipment.SocketStopped = false;

                workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.Start;
                workStage.timer_LaserDrillingWork.Enabled = true;
                //laserDrilling.StartThread();

                WorkStartTick = Environment.TickCount;
            }
            else
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "가공을 중지하시겠습니까?"))
                    return;

                Equipment.MachineStop_byUser = true;

                WorkStartTick = 0;
                WorkStartTick_Outline = 0;
                WorkStartTick_Thruhole = 0;
                WorkStartTick_Drilling = 0;

                //workStage.timer_LaserDrillingWork.Enabled = false;
                //laserDrilling.StopThread();
                workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;

                workStage.laser.Rtc.CtlAbort();             //  실행중인 리스트 명령(busy 상태를)을 강제 종료
                Thread.Sleep(2000);
                workStage.laser.Rtc.CtlReset();             //  에러 해제
            }
        }

        private void button_Main_RtcInit_Click(object sender, EventArgs e)
        {
            //if (!workStage.Rtc_Init())
            //{
            //    MessageBox.Show("RTC 초기화 실패");
            //}

            workStage.Module_Allocation();
            unloader.Module_Allocation();
            loader.Module_Allocation();

            //  카메라는 여러번 초기화 할 수 있으니, 이 조건을 걸어서 스캐너 초기화를 1회만 하도록 한다.
            if (Equipment.ScannerMode_Change_byUser != (int)RtcMode.RTC_RTC6_COMPLETE)
            {
                // 문서 생성후 뷰어에 지정
                var doc = new DocumentDefault();
                SiriusViewer_Main.Document = doc;

                Equipment.ScannerMode_Change_byUser = (int)RtcMode.RTC_RTC6;
            }
        }

        private void ProgressForm_StopProcess(object target)
        {
            Part part = target as Part;
            if (part != null)
            {
                //part.Stop();
            }
        }

        private void button_Main_RecipeOpen_Click(object sender, EventArgs e)
        {
            //workStage.workStageParameter.stWorkStagePosParam = workStage.workStageParameter.GetPositionInformation("Processing");
            //int a = 0;
            //workStage.DrillingData_RotationOffset_Move(0, 0, 15, 1, -1);
            //return;

            string fileName;

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


            if (workStage.rtc == null)
            {
                MessageBox.Show("먼저 Scanner Board 를 초기화 해야 합니다.", "Information!!");
                return;
            }


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;

                Equipment.RecipeOpen_fromMainForm = true;
                Equipment.RecipeName_fromMainForm = fileName;

                //RecipeForm = new FormNew_Recipe();

                //RecipeForm.Recipe_Open(fileName);
            }

            //Equipment.EqpSiriusViewer.Document = SiriusViewer_Main.Document;                            //  메인 화면에 보이는 도면을 가공하기 위함

            ////  Data Parsing
            //workStage.GetDrillingData();



            ////  선택된 Socket 의 Center 좌표 확인
            //double m_dSelectedGroup_Center_X = 999.0;
            //double m_dSelectedGroup_Center_Y = 999.0;
            //foreach (var layer in Equipment.EqpSiriusViewer.Document.Layers)
            //{
            //    if (layer.IsMarkerable && (layer.Count > 0))               //  데이터가 없으면 배열 할당할 필요 없지
            //    {
            //        if (layer.Name == "Hole1")
            //        {
            //            baseTextBox_SocketCountPerModule.Text = layer.Count.ToString();

            //            foreach (var entity in layer)
            //            {
            //                switch (entity.EntityType)
            //                {
            //                    case EType.Group:
            //                        var group = entity as Group;

            //                        if (group.IsSelected)
            //                        {
            //                            m_dSelectedGroup_Center_X = group.Location.X;
            //                            m_dSelectedGroup_Center_Y = group.Location.Y;
            //                        }
            //                        break;
            //                }
            //            }
            //        }
            //    }
            //}

            ////  선택된 Socket 의 Center 좌표를 TextBox 에 표시
            ////baseTextBox_Socket_Center_X.Text = m_dSelectedGroup_Center_X.ToString();
            ////baseTextBox_Socket_Center_Y.Text = m_dSelectedGroup_Center_Y.ToString();

            //baseTextBox_Socket_Index.Text = "All";
            //workStage.m_nSelectedSocket_Index = -1;

            ////  선택된 Socket 이 몇번 Socket 인지 확인
            //for (int i = 0; i < workStage.m_stDividedRegion_GroupData.Length; i++)
            //{
            //    if ((m_dSelectedGroup_Center_X == workStage.m_stDividedRegion_GroupData[i].dGroupCenter.X) &&
            //        (m_dSelectedGroup_Center_Y == workStage.m_stDividedRegion_GroupData[i].dGroupCenter.Y))
            //    {
            //        baseTextBox_Socket_Index.Text = i.ToString();
            //        workStage.m_nSelectedSocket_Index = i;
            //        break;
            //    }
            //}
        }

        private void button_TEST_RotOffset_Click(object sender, EventArgs e)
        {
            //LwPolyline lwPolyLineVertices = new LwPolyline();

            //lwPolyLineVertices = workStage.SpiralData_Create(20, 10, 10, 10, 0, 0);
            //SiriusViewer_Main.Document.Action.ActEntityAdd(lwPolyLineVertices);

            //Equipment.CycleStop = false;
            //Equipment.CycleStopped_LoaderTransfer = true;
            //Equipment.CycleStopped_UnloaderTransfer = true;
            //Equipment.CycleStopped_MainWork = true;
            //Equipment.AutoRunStatus = true;

            //Change_SocketArraySize(4, 5);

            //workStage.BeamExpander_Send();
            //workStage.BeamExpander_Send_ZoomMotor_Reverse();

            double m_dData = 0.24;

            return;


            //string m_strRet = ConvertDecimalToHex(2000);

            //return;




            //  선택 객체만 회전, Offset 이동

            PointD m_dRot_Center = new PointD();
            PointD m_dOffset = new PointD();
            double m_dAngle = 0.0;

            m_dRot_Center.X = 10.0;
            m_dRot_Center.Y = 20.0;
            m_dOffset.X = 10.0;
            m_dOffset.Y = 10.0;
            m_dAngle = 10.0;

            //  선택한 가공 객체 회전 이동
            //Equipment.EqpSiriusViewer.Document.Action.ActEntityRotate(Equipment.EqpSiriusViewer.Document.Action.SelectedEntity, (float)m_dAngle, (float)m_dRotCenter_X, (float)m_dRotCenter_Y);
            //Equipment.EqpSiriusViewer.Document.Action.ActEntityTransit(Equipment.EqpSiriusViewer.Document.Action.SelectedEntity, (float)m_dOffsetX, (float)m_dOffsetY);
            SiriusViewer_Main.Document.Action.ActEntityRotate(SiriusViewer_Main.Document.Action.SelectedEntity, (float)m_dAngle, (float)m_dRot_Center.X, (float)m_dRot_Center.Y);
            SiriusViewer_Main.Document.Action.ActEntityTransit(SiriusViewer_Main.Document.Action.SelectedEntity, (float)m_dOffset.X, (float)m_dOffset.Y);
        }

        public string ConvertDecimalToHex(int decimalNumber)
        {
            if (decimalNumber < 0 || decimalNumber > 9999)
            {
                //throw new ArgumentOutOfRangeException("decimalNumber", "4자리 10진수여야 합니다.");
                return "NG";
            }

            return decimalNumber.ToString("X4");
        }

        private bool CheckAutoRunStatus()
        {
            bool bRtn = false;

            //  자동 운전 시작
            if (AlarmManager.Instance.IsAlarm)
            {
                // 알람을 클리어 해주세요 메세지
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "알람 해제 바랍니다.");
                return bRtn = false;
            }

            workStage.SetRecoveryLaserDrilling_MainStep(workStage.m_nLaserDrilling_MainStep);
            workStage.m_nLaserDrilling_MainStep = workStage.m_nLaserDrilling_MainStep_Recovery;

            loader.SetRecovery();
            unloader.SetRecovery();

            //  테스트 : 강제로 Dry Run
            //workStage.m_bMainWorkCycle_DryRun = true;
            Equipment.DryRun_ProcessingTime = Equipment.ToInt(baseTextBox_DryRun_ProcessingTime.Text);

            if (!workStage.m_bMainWorkCycle_DryRun && (Equipment.RecipeOpen_DrawingFilePath.Length <= 0))
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "도면(레시피)을 로드 하십시오.");
                return bRtn = false;
            }

            Equipment.SeqTestMode = false;

            Equipment.MachineStop_byTimeout_Loader = false;
            Equipment.MachineStop_byTimeout_Unloader = false;
            Equipment.MachineStop_byTimeout_WorkStage = false;

            Equipment.SocketStop = false;
            Equipment.SocketStopped = false;
            Equipment.CycleStop = false;
            Equipment.CycleStopped_LoaderTransfer = false;
            Equipment.CycleStopped_UnloaderTransfer = false;
            Equipment.CycleStopped_MainWork = false;

            checkBox_Main_SocketStop.Checked = false;
            checkBox_Main_CycleStop.Checked = false;
            checkBox_Main_Loader_Transfer_Pause.Checked = false;
            checkBox_Main_Loader_LPort_Pause.Checked = false;
            checkBox_Main_Loader_RPort_Pause.Checked = false;

            //  선택 가공 인덱스를 전체 가공으로 변경
            workStage.m_nSelectedSocket_Index = -1;

            //최종 AutoRunStatus 로 장비 구동 상태 확인 및 제어!!
            Equipment.AutoManualStatus = true;

            workStage._isMainWorkRunning = false;
            workStage._isLaserDrillingWorkRunning = false;
            loader._isLoaderWorkRunning = false;
            unloader._isUnloaderWorkRunning = false;

            return bRtn = true;
        }

        private void button_Main_AutoRun_Click(object sender, EventArgs e)
        {
            //  자동 운전 시작
            if (AlarmManager.Instance.IsAlarm)
            {
                // 알람을 클리어 해주세요 메세지
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "알람 해제 바랍니다.");
                return;
            }

            //  Chiller 가 알람 상태인지 체크
            if (!workStage.workStageParameter.DI_Chiller_Alarm_Check())
            {
                workStage.AlarmPost(WorkStage.AlarmKey.Chiller_Alarm);
                return;
            }

            //  Chiller 동작 신호가 On 인데 Chiller 가 동작하지 않을경우
            if (workStage.workStageParameter.IsDO_Chiller_Run() &&
                !workStage.workStageParameter.DI_Chiller_Run())
            {
                //  Chiller Stop 메세지
                workStage.AlarmPost(WorkStage.AlarmKey.Chiller_Stop);
                return;
            }

            if (checkBox_Test_DryRun.Checked)
            {
                workStage.m_bMainWorkCycle_DryRun = true;
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "[[DryRyn]]을 시작하시겠습니까?\r\n\r\n[Dry Run]"))
                    return;
            }
            else
            {
                workStage.m_bMainWorkCycle_DryRun = false;
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "자동운전을 시작하시겠습니까?"))
                    return;
            }

            workStage.SetRecoveryLaserDrilling_MainStep(workStage.m_nLaserDrilling_MainStep);
            workStage.m_nLaserDrilling_MainStep = workStage.m_nLaserDrilling_MainStep_Recovery;

            loader.SetRecovery();
            unloader.SetRecovery();

            //  테스트 : 강제로 Dry Run
            //workStage.m_bMainWorkCycle_DryRun = true;
            Equipment.DryRun_ProcessingTime = Equipment.ToInt(baseTextBox_DryRun_ProcessingTime.Text);

            if (!workStage.m_bMainWorkCycle_DryRun && (Equipment.RecipeOpen_DrawingFilePath.Length <= 0))
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "도면(레시피)을 로드 하십시오.");
                return;
            }

            Equipment.SeqTestMode = false;

            Equipment.MachineStop_byTimeout_Loader = false;
            Equipment.MachineStop_byTimeout_Unloader = false;
            Equipment.MachineStop_byTimeout_WorkStage = false;
            
            Equipment.SocketStop = false;
            Equipment.SocketStopped = false;
            Equipment.CycleStop = false;
            Equipment.CycleStopped_LoaderTransfer = false;
            Equipment.CycleStopped_UnloaderTransfer = false;
            Equipment.CycleStopped_MainWork = false;

            checkBox_Main_SocketStop.Checked = false;
            checkBox_Main_CycleStop.Checked = false;
            checkBox_Main_Loader_Transfer_Pause.Checked = false;
            checkBox_Main_Loader_LPort_Pause.Checked = false;
            checkBox_Main_Loader_RPort_Pause.Checked = false;


            //  Loader L, R Port 바로 시작
            loader.m_bStacker0_Run_byUser = true;
            loader.m_bStacker1_Run_byUser = true;


            //  선택 가공 인덱스를 전체 가공으로 변경
            workStage.m_nSelectedSocket_Index = -1;

            //최종 AutoRunStatus 로 장비 구동 상태 확인 및 제어!!
            Equipment.AutoRunStatus = true;

            workStage._isMainWorkRunning = false;
            workStage._isLaserDrillingWorkRunning = false;
            loader._isLoaderWorkRunning = false;
            unloader._isUnloaderWorkRunning = false;
        }

        private void button_Main_Stop_Click(object sender, EventArgs e)
        {
            //  자동 운전 중지

            Log.Write("SLD-200", Equipment.User_Name, "Button Click", "자동 운전 Stop 버튼");

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "자동운전을 중지하시겠습니까?"))
                return;

            Equipment.AutoRunStatus = false;        // 자동운전중
            Equipment.AutoManualStatus = false;     // Auto / Manual 상태 유/무 

            workStage._isMainWorkRunning = false;
            workStage._isLaserDrillingWorkRunning = false;
            loader._isLoaderWorkRunning = false;
            unloader._isUnloaderWorkRunning = false;

            // 아래 변수가 자동운전 Tick 돌리는 변수임.
            workStage.m_MainWork_Start = false;
            workStage.m_LaserDrillingWork_Start = false;
            workStage.m_ProductAlign_Start = false;
            workStage.m_SubWork_Start = false;
            loader.m_LoaderWork_Start = false;
            unloader.m_UnloaderWork_Start = false;

            button_Main_Start.BackColor = Color.LightGray;
            button_Main_Start.ForeColor = Color.Black;

            checkBox_Main_AutoRun.Checked = false;

            // X
            //workStage.timer_MainWork.Stop();
            //workStage.timer_MainWork.Enabled = false;
            //workStage.m_MainWork_Start = false;
            //loader.timer_LoaderWork.Stop();
            //loader.timer_LoaderWork.Enabled = false;
            //loader.m_LoaderWork_Start = false;
            //workStage.timer_LaserDrillingWork.Stop();
            //workStage.timer_LaserDrillingWork.Enabled = false;
            //workStage.m_LaserDrillingWork_Start = false;
            //unloader.timer_UnloaderWork.Stop();
            //unloader.timer_UnloaderWork.Enabled = false;
            //unloader.m_UnloaderWork_Start = false;

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //
            //  장비 운전 정지 시점의 모든 상태 데이터 저장
            //
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //  Loader 상태
            loader.m_nLD_RESTORE_Transfer_Step = loader.m_nLoader_Transfer_Step;
            loader.m_nLD_RESTORE_Transfer_MoveType = loader.m_nLoaderTransferMoveType;
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete;               //  Work Stage 에 Module Put Down 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete;               //  Stacker0 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete;               //  Stacker1 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete;               //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete;                 //  M-Aligner 에 Module Put Down 완료 여부
            loader.m_nLD_RESTORE_MainWork_Cycle_Step = workStage.m_nMainWork_Step;                                                                                              //  Main Work Cycle Step
            loader.m_nLD_RESTORE_DryRun_Cycle_Step = workStage.m_nDryRun_Step;                                                                                                  //  Dry Run Cycle Step
            loader.m_nLD_RESTORE_LaserDrilling_Cycle_Step = workStage.m_nLaserDrilling_MainStep;                                                                                //  Laser Drilling Cycle Step
            loader.m_bLD_RESTORE_MainWork_Cycle_Complete = workStage.m_bMainWorkCycle_Complete;                                                                                 //  Main Work Cycle 완료 여부
            loader.m_bLD_RESTORE_Transfer_fromStacker0_Module_PickUp_Complete_Flag = loader.m_bLD_Transfer_fromStacker0_Module_PickUp_Complete_Flag;                            //  Stacker0 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_Transfer_fromStacker1_Module_PickUp_Complete_Flag = loader.m_bLD_Transfer_fromStacker1_Module_PickUp_Complete_Flag;                            //  Stacker1 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_Transfer_fromMAligner_Module_PickUp_Complete_Flag = loader.m_bLD_Transfer_fromMAligner_Module_PickUp_Complete_Flag;                            //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_Transfer_toMAligner_Module_PutDown_Complete_Flag = loader.m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag;                              //  M-Aligner 에 Module Put Down 완료 여부
            loader.m_bLD_RESTORE_Transfer_toWorkStage_Module_PutDown_Complete_Flag = loader.m_bLD_Transfer_toWorkStage_Module_PutDown_Complete_Flag;                            //  Work Stage 에 Module Put Down 완료 여부

            //  Unloader 상태
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = unloader.m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;     //  Work Stage 에서 Module Pick Up 완료 여부
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete;         //  Stacker0 에 Module Put Down 완료 여부
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete;         //  Stacker1 에 Module Put Down 완료 여부
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete;                     //  NG-Port 에 Module Put Down 완료 여부        
            unloader.m_nUL_RESTORE_Transfer_Step = unloader.m_nUnloader_Transfer_Step;                                                                                          //  Unloader Transfer Step
            unloader.m_nUL_RESTORE_Transfer_MoveType = unloader.m_nUnloaderTransferMoveType;                                                                                    //  Unloader Transfer Move Type
            unloader.m_bUL_RESTORE_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = unloader.m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag;                      //  Unloader 가 Work Stage 에서 Module Pick Up 완료 여부
            unloader.m_bUL_RESTORE_LD_Transfer_toWorkStage_Module_PutDown_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete;                       //  Loader 가 Work Stage 에 Module Put Down 완료 여부
            unloader.m_nUL_RESTORE_MainWork_Cycle_Step = workStage.m_nMainWork_Step;                                                                                            //  Main Work Cycle Step
            unloader.m_nUL_RESTORE_DryRun_Cycle_Step = workStage.m_nDryRun_Step;                                                                                                //  Dry Run Cycle Step
            unloader.m_nUL_RESTORE_LaserDrilling_Cycle_Step = workStage.m_nLaserDrilling_MainStep;                                                                              //  Laser Drilling Cycle Step
            unloader.m_bUL_RESTORE_MainWork_Cycle_Complete = workStage.m_bMainWorkCycle_Complete;                                                                               //  Main Work Cycle 완료 여부
            unloader.m_nUL_RESTORE_MainWork_Cycle_ResultOKNG = workStage.m_nMainWorkCycle_ResultOKNG;                                                                           //  Main Work Cycle 결과 (OK, NG) : OK 인 경우에만 R-Port 로 가져감
            unloader.m_bUL_RESTORE_MainWorkCycle_ResultOK_toRPort = workStage.m_bMainWorkCycle_ResultOK_toRPort;                                                                //  OK 인 Module 을 R-Port 로 가져갈 것인지 L-Port 로 가져갈 것인지
        }

        private void button_TEST_RTCInit_Click(object sender, EventArgs e)
        {
            //SiriusViewer_Main.Document = Equipment.EqpSiriusViewer_Origin.Document;
            //workStage.Import_DrawingFile(Equipment.RecipeOpen_DrawingFilePath);
            //m_formSiriusEditor.Import_DrawingFile(richTextBox_Recipe_TabRecipe_DrawingFile.Text);
            //Equipment.m_bDrawingFileOpen_1time = true;

            UpdatePCBStatus(0, 0, 3);
            return;



            //  RTC 초기화 테스트
            bool m_bRet = true;

            if (Equipment.Machine_LaserType_CO2)                                                                                //  CO2 레이저
            {
                // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm
                float fov = 72.5f;          //  MSL-CO2 장비에서 맞춘 데이터
                // k factor (bits/mm) = 2^20 / fov
                float kfactor = (float)Math.Pow(2, 20) / fov;
                //float kfactor = (float)workStage.Config.ParamConfig.Scanner_KFactor;
                if (kfactor == 0)
                    kfactor = (float)18830.1889;
                //kfactor = (float)18830.1889;
                // full path of correction file
                //var correctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");
                string correctionFile = "D:\\SLD-200_Parameter\\Cor_200C.ct5";

                if (File.Exists(correctionFile) == false)
                {
                    string m_strPath = string.Format("Scanner Correction 파일이 없습니다.\r\n\r\n[{0}]", correctionFile);
                    MessageBox.Show(m_strPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // initialize rtc controller
                m_bRet = workStage.rtc.Initialize(kfactor, LaserMode.Co2, correctionFile);                                                                       //  Sirius1
                //if (!workStage.rtc.Initialize(kfactor, LaserMode.Co2, correctionFile))                                                                         //  Sirius1
                //{
                //    m_bRet &= false;
                //    //return false;
                //}
                //var rtc = ScannerFactory.CreateRtc6(0, kfactor, LaserModes.Yag1, RtcSignalLevels.ActiveHigh, RtcSignalLevels.ActiveHigh, correctionFile);     //  Sirius2
            }
            else                                                                                                                //  UV 레이저
            {
                // theoretically size of scanner field of view (이론적인 FOV 크기) : 60mm
                float fov = 105.0f;         //  MSL-UV 장비에서 맞춘 데이터
                // k factor (bits/mm) = 2^20 / fov
                float kfactor = (float)Math.Pow(2, 20) / fov;
                //float kfactor = (float)workStage.Config.ParamConfig.Scanner_KFactor;
                if (kfactor == 0)
                    kfactor = (float)18830.1889;
                //kfactor = (float)18830.1889;
                // full path of correction file
                //var correctionFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");
                string correctionFile = "D:\\SLD-200_Parameter\\Cor_200U.ct5";
                if (File.Exists(correctionFile) == false)
                {
                    string m_strPath = string.Format("Scanner Correction 파일이 없습니다.\r\n\r\n[{0}]", correctionFile);
                    MessageBox.Show(m_strPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ;
                }
                // initialize rtc controller
                m_bRet = workStage.rtc.Initialize(kfactor, LaserMode.Yag1, correctionFile);                                                                       //  Sirius1
                //if (!workStage.rtc.Initialize(kfactor, LaserMode.Yag1, correctionFile))                                                                         //  Sirius1
                //{
                //    //return false;
                //}
                //var rtc = ScannerFactory.CreateRtc6(0, kfactor, LaserModes.Yag1, RtcSignalLevels.ActiveHigh, RtcSignalLevels.ActiveHigh, correctionFile);     //  Sirius2
            }


            if (m_bRet)
            {
                //  RTC 초기화 성공
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "RTC 초기화 성공");
            }
            else
            {
                //  RTC 초기화 실패
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Error !", "RTC 초기화 실패");
            }
        }

        private void checkBox_Main_Loader_RPort_Pause_CheckedChanged(object sender, EventArgs e)
        {
            //  Loader R-Port Pause 체크박스

            Equipment.Loader_RPort_Pause = checkBox_Main_Loader_RPort_Pause.Checked;
        }

        private void checkBox_Main_SocketStop_CheckedChanged(object sender, EventArgs e)
        {
            //  Socket Stop 일 경우, 현재 가공중인 Socket 완료 후 정지

            //  대상
            //  Loader : Transfer, L-Port, R-Port
            //  Unloader : Transfer
            //  Work Stage : Main Work

            Equipment.SocketStop = checkBox_Main_SocketStop.Checked;
            Equipment.SocketStopped = false;

            if (SocketStop)
            {
                //  Socket Stop 을 설정했으므로 Cycle Stop 은 Cancel
                Equipment.CycleStop = false;
                checkBox_Main_CycleStop.Checked = false;
            }
        }

        private void checkBox_Main_CycleStop_CheckedChanged(object sender, EventArgs e)
        {
            //  Cycle Stop 일 경우, 현재 동작중인 Cycle 완료 후 정지

            //  대상
            //  Loader : Transfer, L-Port, R-Port
            //  Unloader : Transfer
            //  Work Stage : Main Work

            Equipment.CycleStop = checkBox_Main_CycleStop.Checked;

            //Equipment.Loader_LPort_Pause = checkBox_Main_Loader_LPort_Pause.Checked;
            //Equipment.Loader_RPort_Pause = checkBox_Main_Loader_RPort_Pause.Checked;

            if (Equipment.CycleStop)
            {
                //  Cycle Stop 을 설정했으므로 Socket Stop 은 Cancel
                Equipment.SocketStop = false;
                Equipment.SocketStopped = false;
                checkBox_Main_SocketStop.Checked = false;
            }
        }

        private void button_Main_Reset_Click(object sender, EventArgs e)
        {
            //  임시

            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "모든 데이터를 리셋 하시겠습니까?\r\n\r\n[Loader 부터 다시 시작]"))
                return;

            // 가공 Data 초기화
            ProcessManager.Reset();

            //  가공 Sequence Index 초기화 (Loading 부터 시작)
            Equipment.m_bMainProcessStatus_LD_LPort_Complete = false;                       //  Loader LPort 투입 완료
            Equipment.m_bMainProcessStatus_LD_RPort_Complete = false;                       //  Loader RPort 투입 완료
            Equipment.m_bMainProcessStatus_LD_Module_PortPickUp_Complete = false;           //  Loader Port 에서 Module Pick Up 완료
            Equipment.m_bMainProcessStatus_LD_Module_MAlignerPutDown_Complete = false;      //  Loader M-Aligner 에 Module Put Down 완료
            Equipment.m_bMainProcessStatus_LD_M_Aligner_Align_Complete = false;             //  Loader M-Align 완료
            Equipment.m_bMainProcessStatus_LD_Module_MAlignerPickUp_Complete = false;       //  Loader M-Aligner 에서 Module Pick Up 완료
            Equipment.m_bMainProcessStatus_LD_Module_WorkStagePutDown_Complete = false;     //  Loader Work Stage 에 Module Put Down 완료
            Equipment.m_bMainProcessStatus_WorkStage_Module_Process_Complete = false;       //  Work Stage Process 완료
            Equipment.m_bMainProcessStatus_UL_Module_WorkStagePickUp_Complete = false;      //  Unloader Work Stage 에서 Module Pick Up 완료
            Equipment.m_bMainProcessStatus_UL_Module_PortPutDown_Complete = false;          //  Unloader Port 에 Module Put Down 완료

            //  Layer Info List 초기화
            ProcessManager.Init();

            //  Loader 파츠 사용 변수 초기화
            loader.m_nLoaderTransferMoveType = (int)LoaderTransferMoveType.Cycle_None; //  Transfer Move Type
            loader.m_nLoader_Transfer_Step = (int)Loader_Transfer_Step.None;
            loader.m_nStacker0_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
            loader.m_nStacker1_ModulePickupWaitingPos_Step = (int)StackerModulePickupWaitingPos_Step.None;
            loader.m_nMAlign_Step = (int)MAlign_Step.None;

            loader.m_bStacker0_Complete = false;
            loader.m_bStacker1_Complete = false;

            loader.m_bMAlignZone_ModuleExist = false;

            loader.m_nLD_RESTORE_Transfer_Step = 0;
            loader.m_nLD_RESTORE_Transfer_MoveType = 0;
            loader.m_bLD_RESTORE_Transfer_toWorkStage_Module_PutDown_Complete_Flag = false;                 //  Work Stage 에 Module Put Down 완료 여부
            loader.m_bLD_RESTORE_Transfer_fromStacker0_Module_PickUp_Complete_Flag = false;                 //  Stacker0 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_Transfer_fromStacker1_Module_PickUp_Complete_Flag = false;                 //  Stacker1 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_Transfer_fromMAligner_Module_PickUp_Complete_Flag = false;                 //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_Transfer_toMAligner_Module_PutDown_Complete_Flag = false;                  //  M-Aligner 에 Module Put Down 완료 여부
            loader.m_nLD_RESTORE_MainWork_Cycle_Step = 0;
            loader.m_nLD_RESTORE_DryRun_Cycle_Step = 0;
            loader.m_nLD_RESTORE_LaserDrilling_Cycle_Step = 0;
            loader.m_bLD_RESTORE_MainWork_Cycle_Complete = false;
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;         //  Stacker0 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;         //  Stacker1 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;         //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;          //  M-Aligner 에 Module Put Down 완료 여부
            loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;         //  Work Stage 에 Module Put Down 완료 여부

            loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = false;                    //  Stacker 에서 Module Pick Up 완료 여부
            loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = false;                    //  Stacker 에서 Module Pick Up 완료 여부
            loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = false;                    //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = false;                     //  M-Aligner 에 Module Put Down 완료 여부
            loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = false;                    //  Work Stage 에 Module Put Down 완료 여부
            loader.m_bLD_Transfer_fromStacker0_Module_PickUp_Complete_Flag = false;                         //  Stacker0 에서 Module Pick Up 완료 여부
            loader.m_bLD_Transfer_fromStacker1_Module_PickUp_Complete_Flag = false;                         //  Stacker1 에서 Module Pick Up 완료 여부
            loader.m_bLD_Transfer_fromMAligner_Module_PickUp_Complete_Flag = false;                         //  M-Aligner 에서 Module Pick Up 완료 여부
            loader.m_bLD_Transfer_toWorkStage_Module_PutDown_Complete_Flag = false;                         //  Work Stage 에 Module Put Down 완료 여부
            loader.m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag = false;                          //  M-Aligner 에 Module Put Down 완료 여부

            loader.m_bStacker0_Run_byUser = false;                                                          //  Stacker0 Module Pick Up Cycle
            loader.m_bStacker1_Run_byUser = false;                                                          //  Stacker1 Module Pick Up Cycle

            loader.m_bLD_LPort_Complete = false;                                                            //  L-Port 동작 완료 여부
            loader.m_bLD_RPort_Complete = false;                                                            //  R-Port 동작 완료 여부
            loader.m_bLD_TR_ModulePickUp_LPort_Complete = false;                                            //  Transfer L-Port Module Pick Up 동작 완료 여부
            loader.m_bLD_TR_ModulePickUp_RPort_Complete = false;                                            //  Transfer R-Port Module Pick Up 동작 완료 여부
            loader.m_bLD_TR_ModulePutDown_MAligner_Complete = false;                                        //  Transfer Module Put Down 동작 완료 여부
            loader.m_bLD_MAligner_Exist = false;                                                            //  M-Aligner 로 Module Pick & Place
            loader.m_bLD_MAlign_Complete = false;                                                           //  M-Aligner 동작 완료 여부
            loader.m_bLD_TR_ModulePickUp_MAligner_Complete = false;                                         //  M-Aligner Module Pick Up 동작 완료 여부
            loader.m_bLD_WorkStage_LoadingComplete = false;                                                 //  Work Stage 로 Module Loading 완료 여부

            //  Unloader 파츠 사용 변수 초기화
            unloader.m_nUnloader_Transfer_Step = (int)Unloader_Transfer_Step.None;
            unloader.m_nStacker0_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;
            unloader.m_nStacker1_ModulePutdownWaitingPos_Step = (int)StackerModulePutdownWaitingPos_Step.None;

            unloader.m_bStacker0_Complete = false;
            unloader.m_bStacker1_Complete = false;

            unloader.m_nUL_RESTORE_Transfer_Step = 0;
            unloader.m_nUL_RESTORE_Transfer_MoveType = 0;
            unloader.m_bUL_RESTORE_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = false;
            unloader.m_bUL_RESTORE_LD_Transfer_toWorkStage_Module_PutDown_Complete = false;
            unloader.m_nUL_RESTORE_MainWork_Cycle_Step = 0;
            unloader.m_nUL_RESTORE_DryRun_Cycle_Step = 0;
            unloader.m_nUL_RESTORE_LaserDrilling_Cycle_Step = 0;
            unloader.m_bUL_RESTORE_MainWork_Cycle_Complete = false;
            unloader.m_nUL_RESTORE_MainWork_Cycle_ResultOKNG = (int)WorkStage.MainCycle_Result.None;
            unloader.m_bUL_RESTORE_MainWorkCycle_ResultOK_toRPort = false;                                  //  OK 인 Module 을 R-Port 로 가져갈 것인지 L-Port 로 가져갈 것인지

            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;    //  Work Stage 에서 Module Pick Up 완료 여부
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;      //  Stacker0 에 Module Put Down 완료 여부
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;      //  Stacker1 에 Module Put Down 완료 여부
            unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;            //  NG-Port 에 Module Put Down 완료 여부     

            unloader.m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = false;               //  Work Stage 에서 Module Pick Up 완료 여부
            unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = false;                 //  Stacker0 에 Module Put Down 완료 여부
            unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = false;                 //  Stacker1 에 Module Put Down 완료 여부
            unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = false;                       //  NG-Port 에 Module Put Down 완료 여부
            unloader.m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = false;                      //  Work Stage 에서 Module Pick Up 완료 여부

            //  Main 파츠 사용 변수 초기화
            workStage.m_bMainWorkCycle_Complete = false;
            //workStage.m_bMainWorkCycle_ResultOK = false;
            workStage.m_nMainWorkCycle_ResultOKNG = (int)WorkStage.MainCycle_Result.None;
            workStage.m_bMainWorkCycle_ResultOK_toRPort = true;
            workStage.m_nMainWork_Step = (int)MainWork_Step.None;                                 //  Main Work Step
            workStage.m_nMainWorkCycleType = (int)MainWorkCycleType.Cycle_None;                   //  자동 운전 시 사용하는 변수
            workStage.m_bMainWorkCycle_DryRun = false;


            //  Laser Drilling 파츠 사용 변수 초기화
            workStage.m_bLaserDrilling_Complete = false;
            workStage.m_nLaserDrilling_MainStep = (int)LaserDrilling_Step.None;

            workStage.ResetRecovery();
            unloader.ResetRecovery();
            loader.ResetRecovery();
            

        }

        private void checkBox_Main_Loader_Transfer_Pause_CheckedChanged(object sender, EventArgs e)
        {
            Equipment.Loader_Transfer_Pause = checkBox_Main_Loader_Transfer_Pause.Checked;
        }

        private void button_Main_Pause_Click(object sender, EventArgs e)
        {

        }

        private void button_Main_CameraInit_Click(object sender, EventArgs e)
        {
            //  카메라 초기화
            Task<int> task = Task.Factory.StartNew<int>(() =>
            {
                workStage.Camera_HighRes.SetRunStatus(Part.RunStatus.Run);
                workStage.Camera_LowRes.SetRunStatus(Part.RunStatus.Run);
                workStage.Camera_HighRes.Initialize();
                workStage.Camera_LowRes.Initialize();
                return 0;
            });

            ProgressForm ProgressForm = new ProgressForm(workStage.Name, "Camera Initializing...", task, workStage.Camera_HighRes);
            ProgressForm.StopProcess += ProgressForm_StopProcess;
            ProgressForm.StartPosition = FormStartPosition.CenterScreen;
            ProgressForm.ShowDialog();

            workStage.Camera_HighRes.Initialize();
            workStage.Camera_LowRes.Initialize();
        }

        private void button_Main_Loader_Continue_Click(object sender, EventArgs e)
        {
            //  Loader Cycle Continue

            if (Equipment.MachineStop_byTimeout_Loader)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Loader Cycle Continue...");

                loader.Loader_Transfer_Restart_MoveType_Check();

                //  Timeout 이 발생하여 Cycle Stop 상태인 경우
                Equipment.MachineStop_byTimeout_Loader = false;                
            }
        }

        private void button_Main_Unloader_Continue_Click(object sender, EventArgs e)
        {
            //  Unloader Cycle Continue

            if (Equipment.MachineStop_byTimeout_Unloader)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Unloader Cycle Continue...");

                unloader.Unloader_Transfer_Restart_MoveType_Check();

                //  Timeout 이 발생하여 Cycle Stop 상태인 경우
                Equipment.MachineStop_byTimeout_Unloader = false;
            }
        }

        private void button_Main_WorkStage_Continue_Click(object sender, EventArgs e)
        {
            //  Laser Drilling Cycle Continue

            if (Equipment.SocketStopped)
            {
                var mb = new MessageBoxOk();
                mb.ShowDialog("Information !", "Laser Drilling Cycle Continue...");

                workStage.WorkStage_Restart_Check();

                Equipment.SocketStopped = false;
            }
        }


        public void Main_FiducialAlignData_Clear()
        {
            listView_Main_FiducialAlignData.BeginUpdate();

            //  ListView Column 삭제
            listView_Main_FiducialAlignData.Items.Clear();
            foreach (ColumnHeader header in listView_Main_FiducialAlignData.Columns)
            {
                listView_Main_FiducialAlignData.Columns.Remove(header);
            }

            //  ListView Column 설정
            listView_Main_FiducialAlignData.Columns.Add("No", 50, HorizontalAlignment.Center);
            listView_Main_FiducialAlignData.Columns.Add("Socket", 55, HorizontalAlignment.Center);
            listView_Main_FiducialAlignData.Columns.Add("Devi. X", 70, HorizontalAlignment.Center);
            listView_Main_FiducialAlignData.Columns.Add("Devi. Y", 70, HorizontalAlignment.Center);
            listView_Main_FiducialAlignData.Columns.Add("Hole Size", 70, HorizontalAlignment.Center);

            listView_Main_FiducialAlignData.EndUpdate();
        }


        public void Main_FiducialAlignData_Add()
        {
            listView_Main_FiducialAlignData.BeginUpdate();

            ////  ListView Column 삭제
            //listView_Main_FiducialAlignData.Items.Clear();
            //foreach (ColumnHeader header in listView_Main_FiducialAlignData.Columns)
            //{
            //    listView_Main_FiducialAlignData.Columns.Remove(header);
            //}

            ////  ListView Column 설정
            //listView_Main_FiducialAlignData.Columns.Add("No", 50, HorizontalAlignment.Center);
            //listView_Main_FiducialAlignData.Columns.Add("Socket", 55, HorizontalAlignment.Center);
            //listView_Main_FiducialAlignData.Columns.Add("Devi. X", 70, HorizontalAlignment.Center);
            //listView_Main_FiducialAlignData.Columns.Add("Devi. Y", 70, HorizontalAlignment.Center);
            //listView_Main_FiducialAlignData.Columns.Add("Hole Size", 70, HorizontalAlignment.Center);

            //  Fiducial Data를 ListView에 표시
            for (int i = 0; i < workStage.m_nDrawing_Hole1Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Text = (i + 1).ToString();
                item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole1[i].CenterX));
                item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole1[i].CenterY));
                item.SubItems.Add(string.Format("{0:0.000}", workStage.m_stDrawing_Hole1[i].radius));
            listView_Main_FiducialAlignData.Items.Add(item);
            }

            listView_Main_FiducialAlignData.EndUpdate();
        }

        private void buttonForceMaterialOut_Click(object sender, EventArgs e)
        {
            workStage.m_bLaserDrilling_Complete = true;
            workStage.m_nLaserDrilling_MainStep = 0;
            workStage.m_nSocketAlign_MainStep = 0;
        }
        private void FormNew_Main_Load(object sender, EventArgs e)
        {
            InitImageViewer();
        }

        private void checkBox_Main_AutoRun_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Main_AutoRun.Checked)
            {
                if (Equipment.AutoRunStatus)
                    return;

                if (!Equipment.AutoManualStatus && CheckAutoRunStatus())
                {
                    checkBox_Main_AutoRun.Text = "AUTO";
                    checkBox_Main_AutoRun.BackColor = Color.LightGreen;
                    checkBox_Main_AutoRun.ForeColor = Color.Black;

                    Equipment.AutoManualStatus = true;
                }
                else if (Equipment.AutoManualStatus) // 장비가 정상적으로 구동 중일때는 변경되면 안되는데.
                {
                    checkBox_Main_AutoRun.Text = "MANUAL";
                    checkBox_Main_AutoRun.BackColor = Color.LightGray;
                    checkBox_Main_AutoRun.ForeColor = Color.Black;

                    Equipment.AutoManualStatus = false;
                }
            }
            else
            {
                if (Equipment.AutoRunStatus) // 장비가 정상적으로 구동 중일때는 변경되면 안되는데.
                {
                    var mb = new MessageBoxYesNo();
                    if (DialogResult.Yes != mb.ShowDialog("Question ?", "정지 하시겠습니까?"))
                        return ;

                    checkBox_Main_AutoRun.Text = "MANUAL";
                    checkBox_Main_AutoRun.BackColor = Color.LightGray;
                    checkBox_Main_AutoRun.ForeColor = Color.Black;

                    Equipment.AutoManualStatus = false;
                }
                else
                {
                    checkBox_Main_AutoRun.Text = "MANUAL";
                    checkBox_Main_AutoRun.BackColor = Color.LightGray;
                    checkBox_Main_AutoRun.ForeColor = Color.Black;

                    Equipment.AutoManualStatus = false;
                }
            }
        }

        private void button_Test12_Click(object sender, EventArgs e)
        {
            workStage.AlarmPost(QMC.Common.Modules.WorkStage.AlarmKey.PreAlignFail);
        }

        private void button_TEST12_Click(object sender, EventArgs e)
        {
            loader.AlarmPost(Loader.AlarmKey.eMAligner_Wide_Fail);

            loader.AlarmPost(Loader.AlarmKey.LD_TransferX_Move_StackerPos_Timeout);

            workStage.AlarmPost(WorkStage.AlarmKey.PreAlignFail);

        }

        private void checkBox_Main_Loader_LPort_Pause_CheckedChanged(object sender, EventArgs e)
        {
            // To do: Test code임. - 아래의 조건을 시컨스에 맞춰 넣어야함!!!!
            //  Loader L-Port Pause 체크박스
            Equipment.Loader_LPort_Pause = checkBox_Main_Loader_LPort_Pause.Checked;
        }

        private void button_TestbyUser_LPort_Start_Click(object sender, EventArgs e)
        {
            // To do: Test code임. - 아래의 조건을 시컨스에 맞춰 넣어야함!!!!
            if (Equipment.AutoRunStatus)
            {
                var mb = new MessageBoxYesNo();
                if (DialogResult.Yes != mb.ShowDialog("Question ?", "Loader Transfer 시작 하시겠습니까?"))
                    return;

                loader.m_bStacker1_Run_byUser = true;
                loader.m_nLoaderTransfer_ProcessStep = (int)LoaderTransferProcessStep.LoaderStep_ModulePickup_fromStacker;
            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Information !", "Auto Run 상태가 아닙니다.");
                return;
            }
        }
    }
}
