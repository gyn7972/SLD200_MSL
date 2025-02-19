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
using QMC.Common.VisionPart;
using QMC.Common.Vision.EureSys;
using QMC.Common.Vision.Optics.Leesos;
using QMC.Common.Motion.Ajin;
using QMC.Common.Motion.Ajin.IO;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common;

namespace SLD200_MSL
{
    public partial class Form1 : Form
    {
        List<AjinAxlIoBoard> listBoard;

        AjinAxlMotionBoard m_MotionBoard;

        //GrabLinkMultiCamCamera m_Camera;
        public Form1()
        {
            InitializeComponent();

            listBoard = new List<AjinAxlIoBoard>();
            //listMotionBoard = new List<MotionBoard>();
            InitMotion();
            InitCameara();
            InitIlluminator();
            this.Load += Form1_Load;
        }

        

        private void InitMotion()
        {
            AjinAxlMotionBoard MotionBoard = new AjinAxlMotionBoard();
            MotionBoard.Configuration.Name = "Board1";
            MotionBoard.Configuration.No = 0;


            AjinAxlAxis Axis = new AjinAxlAxis();
            AjinAxlHomingSpecification specification = Axis.Configuration.HomingSpecification as AjinAxlHomingSpecification;
            specification.UseAjinAxlFunction = true;
            specification.AjinAxlHomingParameter.Direction = Directions.Ccw;
            specification.AjinAxlHomingParameter.FirstSearchVelocity = 30;
            specification.AjinAxlHomingParameter.FirstSearchAcc = 300;
            specification.AjinAxlHomingParameter.HomeClearTime = 1500;
            specification.AjinAxlHomingParameter.HomeSignal = HomeSignals.NegativeLimit;
            specification.AjinAxlHomingParameter.IndexSearchVelocity = 1;
            specification.AjinAxlHomingParameter.LastVelocity = 10;
            specification.AjinAxlHomingParameter.SecondSearchVelocity = 10;
            specification.AjinAxlHomingParameter.SecondSearchAcc = 100;
            specification.AjinAxlHomingParameter.ZPhaseMethod = ZPhaseMethods.None;

            specification.Acceleration = 100;
            specification.Deceleration = 100;
            specification.EnableIndexSearch = false;
            specification.EnablePreciseSearch = false;
            specification.EscapeDistance = 0.5;
            specification.HomePosition = 0;
            specification.Method = HomingMethod.NegativeSensor;
            specification.NegativePosition = 0;
            specification.PreciseSearchVelocityPercent = 10;
            specification.Velocity = 10;

            Axis.Configuration.UID = 1;
            Axis.Configuration.HomingSpecification = specification;

            Axis.Configuration.AccelUnit = (uint)AXM.AccelUnit.UnitPerSec2;
            Axis.Configuration.PulsePerPosition = new Fraction(1000, 1);
            Axis.Configuration.MotionType = AjinAxlMotionType.Trapezoidal;
            Axis.Configuration.MaxVelocity = 100;
            Axis.Configuration.No = 7;
            Axis.Board = MotionBoard;
            

            m_MotionBoard = MotionBoard;
            MotionBoard.AddAxis(Axis);
            MotionBoard.Open();
        }

        private void buttonIOInit_Click(object sender, EventArgs e)
        {
            listBoard.Clear();
            int nCount = int.Parse(textBoxBoardCount.Text);
            int nInputCount = int.Parse(textBoxInputCount.Text);
            int nOutputCount = int.Parse(textBoxOutputCount.Text);
            uint nModuleNo = 0;
            for (int i = 0; i < nCount; i++)
            {
                AjinAxlIoBoard ajinAxlIoBoard = new AjinAxlIoBoard();

                for (int input = 0; input < nInputCount; input++)
                {
                    AjinAxlDioModule module = new AjinAxlDioModule();
                    module.Board = ajinAxlIoBoard;
                    module.Configuration.No = nModuleNo++;
                    module.Configuration.RefreshTime = 10;
                    module.Configuration.InputCount = 4;
                    ajinAxlIoBoard.Modules.Add(module);
                }

                for (int output = 0; output < nOutputCount; output++)
                {
                    AjinAxlDioModule module = new AjinAxlDioModule();
                    module.Board = ajinAxlIoBoard;
                    module.Configuration.No = nModuleNo++;
                    module.Configuration.RefreshTime = 10;
                    module.Configuration.OutputCount = 4;
                    ajinAxlIoBoard.Modules.Add(module);
                }

                listBoard.Add(ajinAxlIoBoard);
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            int nError = 0;
            foreach (AjinAxlIoBoard board in listBoard)
            {
                if ((nError = board.Open()) != 0)
                {
                    MessageBox.Show(string.Format("Error : {0}", nError));
                    break;
                }
            }
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            int nIndex = int.Parse(textBoxReadIndex.Text);
            if (nIndex >= 0)
            {
                if (listBoard[0].Modules[0].Read() == 0)
                {
                    StringBuilder sb = new StringBuilder();
                    byte[] value = listBoard[0].Modules[0].InputBuffer.GetValues();
                    for (int i = 0; i < value.Length; i++)
                    {
                        sb.AppendFormat("{0}/", value[i]);
                    }


                    MessageBox.Show(sb.ToString());

                }
            }
        }

        private void buttonInterrupt_Click(object sender, EventArgs e)
        {
            DioModule module = listBoard[0].Modules[0] as DioModule;
            if (module != null)
            {
                module.SetInterrupt(8, ActiveLevel.High, DigitalEdge.Up, DioEdgeDetectedCallback);
                module.SetInterrupt(8, ActiveLevel.High, DigitalEdge.Down, DioEdgeDetectedCallback);
            }
        }

        public void DioEdgeDetectedCallback(int address, DigitalEdge edge)
        {

            MessageBox.Show(string.Format("IO [{0}] : {1} ", address, edge));
        }

        private void buttonMotion_Click(object sender, EventArgs e)
        {
            //if(listMotionBoard.Count > 0 && listMotionBoard[0].Axes.Count > 0)
            {
                AjinAxlAxis axis = m_MotionBoard.GetAxis(0) as AjinAxlAxis;
                FormAxisMotion form = new FormAxisMotion(axis);
                form.Show(Owner);
            }

        }

        string m_strMotionFileName = "d:\\Motion.dat";
        private void buttonSave_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(m_strMotionFileName, FileMode.Create))
            {
                m_MotionBoard.Save(fs);
            }

        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            AjinAxlMotionBoard MotionBoard = new AjinAxlMotionBoard();
            using (FileStream fs = new FileStream(m_strMotionFileName, FileMode.Open))
            {
                MotionBoard.Load(fs);
            }

            if (m_MotionBoard != null)
            {
                m_MotionBoard.Close();
            }
            m_MotionBoard = MotionBoard;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_MotionBoard.Close();
            //m_Camera.Close();
        }

        private void buttonAxisForm_Click(object sender, EventArgs e)
        {
            FormAxisConfiguration FormAxisConfiguration = new FormAxisConfiguration();
            FormAxisConfiguration.Show(Owner);
        }

        private void buttonBoardForm_Click(object sender, EventArgs e)
        {

            FormMotionBoardConfiguration FormMotionBoardConfiguration = new FormMotionBoardConfiguration();
            FormMotionBoardConfiguration.Show(Owner);   
        }

        //private void buttonHomeForm_Click(object sender, EventArgs e)
        //{
        //    FormModuleInitialize FormAxisHomeSequence = new FormModuleInitialize();
        //    FormAxisHomeSequence.Show(Owner);   
        //}

        private void buttonIOModule_Click(object sender, EventArgs e)
        {
            FormIOModule formioModule = new FormIOModule();
            formioModule.Show();

        }

        private void buttonIOBoard_Click(object sender, EventArgs e)
        {
            FormIOBoard formIOBoard = new FormIOBoard();
            formIOBoard.Show();

        }

        private void buttonDigitalIO_Click(object sender, EventArgs e)
        {
            FormDigitalIO formDigitalIO = new FormDigitalIO();
            formDigitalIO.Show();

        }

        private PositionConverter m_Converter = new LinearPositionConverter();
        private void buttonLoadInterpolation_Click(object sender, EventArgs e)
        {
            m_Converter.Load("D:\\test.csv");
        }

        private void buttonTestInterpolation_Click(object sender, EventArgs e)
        {
            XyCoordinate target = new XyCoordinate();
            target.X = double.Parse(textBoxTargetX.Text);
            target.Y = double.Parse(textBoxTargetY.Text);

            XyCoordinate result;
            result = m_Converter.ToCompensationCoordinate(target);
            MessageBox.Show(result.ToString());
        }

        private void InitCameara()
        {
            //m_Camera = new GrabLinkMultiCamCamera("Test");
            //m_Camera.CamFilePath = "d:\\MC-A500M-35_RG_3tap.cam";
            //m_Camera.Channel = 0;
            //m_Camera.TapConfiguration = GrabLinkMultiCamCamera.eTapConfiguration.BASE_3T8;
            //m_Camera.TapGeometry = GrabLinkMultiCamCamera.eTapGeometry.A1X3_1Y;
            //m_Camera.NextTrigMode = MultiCamCamera.NextTriggerMode.Same;
            //m_Camera.BoardTopology = GrabLinkMultiCamCamera.eBoardTopology.DUO;
            //m_Camera.CameraResolution = new Size(2448, 2048);
            //m_Camera.Connector = GrabLinkMultiCamCamera.eConnector.A;

            //m_Camera.Open();

            //propertyGridCamera.SelectedObject = m_Camera;


        }
        private void buttonStartCamera_Click(object sender, EventArgs e)
        {
            //m_Camera.StartLive();
            //visionImageViewer1.Camera = m_Camera;
        }

        private void buttonStopCamera_Click(object sender, EventArgs e)
        {
            //m_Camera.StopLive();
            //visionImageViewer1.Camera = null;
        }

        private void buttonCrossLine_Click(object sender, EventArgs e)
        {
            //visionImageViewer1.VisibleCrossLine = !visionImageViewer1.VisibleCrossLine;

        }

        DigitalIlluminator illuminator;
        private void InitIlluminator()
        {
            //illuminator = new DigitalIlluminator("Source");
            //illuminator.BoudRate = 9600;
            //illuminator.DataBits = 8;
            //illuminator.Parity = System.IO.Ports.Parity.None;
            //illuminator.StopBits = System.IO.Ports.StopBits.One;
            //illuminator.PortName = "COM4";
            //propertyGrid1.SelectedObject = illuminator;
        }

        private void buttonIlOpen_Click(object sender, EventArgs e)
        {
            illuminator.Initialize();
        }

        private void buttonIlClose_Click(object sender, EventArgs e)
        {
            illuminator.Close();
        }

        
        private void buttonTurnOn_Click(object sender, EventArgs e)
        {
            int nCheck = illuminator.CheckPowerOn(1);
            if (nCheck == 0)
                illuminator.TurnOnOff(DigitalIlluminatorCommunicator.Commands.OFF, 1);
            else
                illuminator.TurnOnOff(DigitalIlluminatorCommunicator.Commands.ON, 1);
        }

        private void buttonSetBright_Click(object sender, EventArgs e)
        {
            int nVolume = int.Parse(textBoxVolume.Text);
            illuminator.SetVolume(nVolume, 1);
        }

        private void buttonLog_Click(object sender, EventArgs e)
        {
            string strClass = textBoxClass.Text;
            string strSource = textBoxSource.Text;
            string strMsg = textBoxMessage.Text;
            LogManager.Instance.Write(LogLevel.Normal, strClass, strSource, strMsg);
        }

        private void buttonLogTest2_Click(object sender, EventArgs e)
        {
            string strClass = textBoxClass.Text;
            string strSource = textBoxSource.Text;
            string strMsg = textBoxMessage.Text;
            LogManager.Instance.SetWriterLogLevel(LogLevel.Normal);
            LogManager.Instance.Write(LogLevel.Lowest, strClass, strSource, strMsg);
            LogManager.Instance.Write(LogLevel.Normal, strClass + "_1", strSource, strMsg);
        }

        private void buttonMaterialCreate_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
 
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        //QMC.Common.Opticon.OpticonBarcodeReader m_BarcodeReader = new QMC.Common.Opticon.OpticonBarcodeReader("Reader");
        //QMC.Common.Opticon.OpticonBarcodeReaderConfig m_Config = new QMC.Common.Opticon.OpticonBarcodeReaderConfig();

        private void Form1_Load(object sender, EventArgs e)
        {
            //propertyGrid2.SelectedObject = m_Config;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //if (m_BarcodeReader == null)
            //    m_BarcodeReader = new QMC.Common.Opticon.OpticonBarcodeReader("Reader");

            //m_BarcodeReader.Create();
            //m_BarcodeReader.Config = m_Config;
            //m_BarcodeReader.Initialize();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //if(m_BarcodeReader != null)
            //    m_BarcodeReader.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            /*string strMessage;
            if(m_BarcodeReader!=null)
            {
                int ret = m_BarcodeReader.Read(out strMessage);
                if(ret == 0)
                {
                    MessageBox.Show("Read Done!!");
                    //textBox1.Text = strMessage;
                }
                else
                {
                    MessageBox.Show("Error!!!");
                }
            }*/
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            /*if (m_BarcodeReader == null)
                m_BarcodeReader = new QMC.Common.Opticon.OpticonBarcodeReader("Reader");

            m_BarcodeReader.Create();
            m_BarcodeReader.Config = m_Config;
            m_BarcodeReader.Initialize();*/
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            /*if (m_BarcodeReader != null)
                m_BarcodeReader.Close();*/
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            /*string strMessage;
            if (m_BarcodeReader != null)
            {
                int ret = m_BarcodeReader.Read(out strMessage);
                if (ret == 0)
                {
                    MessageBox.Show("Read Done!!");
                    //textBox1.Text = strMessage;
                }
                else
                {
                    MessageBox.Show("Error!!!");
                }
            }*/
        }
    }
}
