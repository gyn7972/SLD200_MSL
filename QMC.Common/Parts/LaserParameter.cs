using QMC.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SpiralLab.Sirius;

//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using SpiralLab.Sirius2;
//using SpiralLab.Sirius2.Laser;
//using SpiralLab.Sirius2.PowerMeter;
//using SpiralLab.Sirius2.PowerMap;
//using SpiralLab.Sirius2.Scanner;
//using SpiralLab.Sirius2.Scanner.Rtc;
//using SpiralLab.Sirius2.Winforms;
//using SpiralLab.Sirius2.Winforms.Entity;
//using SpiralLab.Sirius2.Winforms.Marker;
//using SpiralLab.Sirius2.Winforms.UI;

//namespace QMC.Process.WorkStage.Parts
namespace QMC.Common.Parts
{
    public class LaserParameter : Part
    {
        public enum DioPointKey
        {
            Input_Mirror_FW,
            input_Mirror_BW,
            Output_Mirror_FW,
            Output_Mirror_BW,
            Output_Laser_Enable,
        }

        private bool m_bDrillingDone;

        //private IRtc m_rtc;
        //private IRtcSyncAxis m_rtc;
        //private Rtc6SyncAxis m_rtc;
        private Rtc6 m_rtc;

        private LaserVirtual m_laser;                       //  테스트용
        //private SpectraPhysicsTalon m_laser;              //  2022. 07. 21.  SCH : SyncAxis 는 RTC6 만 됨. (RTC4, RTC5 는 지원하지 않음)

        private MarkerDefault m_drillingMarker;
        //private MarkerBase m_drillingMarker;                //  Sirius2
        private DocumentDefault m_document;
        //private DocumentBase m_document;                    //  Sirius2
        private PowerMapDefault m_powerMap;
        //private PowerMapBase m_powerMap;                    //  Sirius2

        //private PowerMeterOphir m_powerMeter;             //  2022. 07. 21.  SCH : SLD-100 에 사용되는 PowerMeter 는 Coherent 제품이다.
        public LaserParameterRecipe Recipe { set; get; }
        public LaserParameterConfig Config { set; get; }
        public int ResponseTimeout { set; get; }

        public double EtchingAngle { set; get; }

        public MarkerDefault Marker
        //public MarkerBase Marker                          //  Sirius2
        {
            get
            {
                return m_drillingMarker;
            }
        }
        public IRtc RTC                                   //  2022. 07. 21.  SCH : 안넣어도 될것 같은 코드.
        {
            get
            {
                return m_rtc;
            }
        }

        //public IRtcSyncAxis RTC                                   //  2022. 07. 21.  SCH : 안넣어도 될것 같은 코드.
        //{
        //    get
        //    {
        //        return m_rtc;
        //    }
        //}

        public ILaser Laser
        {
            get
            {
                return m_laser;
            }
        }

        /*public IPowerMeter PowerMeter
        {
            get
            {
                return m_powerMeter;
            }
        }*/

        public PowerMapDefault Map
        //public PowerMapBase Map                           //  Sirius2
        {
            set
            {
                m_powerMap = value;
                m_laser.PowerMap = m_powerMap;
            }
            get
            {
                return m_powerMap;
            }
        }

        public LaserParameter(string strName) : base(strName)
        {
            Recipe = new LaserParameterRecipe();
            Config = new LaserParameterConfig();

            ResponseTimeout = 0;
            EtchingAngle = 0;

            m_bDrillingDone = false;
        }

        public override int Create()
        {
            //m_rtc = new Rtc6SyncAxis();                             //  ScanLab XLSCAN 솔루션 (SyncAxis 를 사용하려면 이걸로 해야 함.)
            //m_rtc = new Rtc6();                                     //  ScanLab RTC6 솔루션

            if (m_dicDioPoints == null)
                m_dicDioPoints = new Dictionary<string, DioPoint>();

            m_dicDioPoints.Clear();
            foreach (DioPointKey key in Enum.GetValues(typeof(DioPointKey)))
            {
                m_dicDioPoints.Add(key.ToString(), null);
            }

            m_laser = new LaserVirtual(0, "LaserVirtual", 200);                 //  테스트용
            //m_laser = new SpectraPhysicsTalon(0, "Talon", 1, 30);             //  실제 레이저

            //m_powerMeter = new PowerMeterOphir(0, "Ophir", "", 100000);

            //m_drillingMarker = new MarkerDefault(0);
            //m_drillingMarker.Name = "DrillingMarker";
            //m_drillingMarker.OnFinished += DrillingMaker_OnFinished;

            //m_drillingMarker = new MarkerBase();       // MarkerDefault(0);   //  2025. 01. 02.  SCH : 왜 안될까?
            //m_drillingMarker.Name = "DrillingMarker";
            //m_drillingMarker.OnFinished += DrillingMaker_OnFinished;
            //m_drillingMarker.OnEnded += DrillingMarker_OnEnded;               //  Sirius2

            //Map = new PowerMapBase(0, m_drillingMarker.Name);                 //  2025. 01. 02.  SCH : 왜 안될까?


            return base.Create();
        }

        private void DrillingMarker_OnEnded(IMarker arg1, bool arg2, TimeSpan arg3)             //  Sirius2
        {
            var span = arg3;        // markerArg.EndTime - markerArg.StartTime;
            Log.Write(this, $"{arg1.Name} finished : {span.ToString()} sec");
            m_bDrillingDone = true;
        }

        //private void DrillingMaker_OnFinished(IMarker sender, IMarkerArg markerArg)           //  Sirius1
        //{
        //    var span = markerArg.EndTime - markerArg.StartTime;
        //    Log.Write(this, $"{sender.Name} finished : {span.ToString()} sec");
        //    m_bDrillingDone = true;
        //}

        public override int Initialize()
        {
            if (m_rtc != null)
            {
                //string configXmlFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "syncaxis", "syncAXISConfig.xml");
                string configXmlFileName = "D:\\ScannerParameter\\syncAXISConfig.SLD100.xml";

                //if (!m_rtc.Initialize(configXmlFileName))
                ////if (!m_rtc.Initialize())                                           //  Sirius2
                //{
                //    Console.WriteLine("RTC Board Initialize 실패.");
                //}
            }

            //if (m_laser != null)                                          //  2025. 01. 02.  SCH : 왜 안될까?
            //{
            //    m_laser.Rtc = m_rtc;

            //    if(!m_laser.Initialize())
            //    {
            //        Console.WriteLine("Laser Initialize 실패.");
            //    }
            //}

            /*if (m_powerMeter != null)
            {
                if (!m_powerMeter.Initialize())
                {
                    Console.WriteLine("PowerMeter Initialize 실패.");
                }
            }*/

            LoadPowerMap();

            return base.Initialize();
        }

        public override void Stop()
        {
            base.Stop();
            m_rtc.CtlLaserOff();
            m_rtc.CtlAbort();
            //m_powerMeter.CtlStop();
            EnableLaserEmission(false);
        }

        public override void Close()
        {

            if (m_rtc != null)
                m_rtc.Dispose();

            /*if(m_powerMeter!= null)
                m_powerMeter.Dispose();*/

            if (m_laser != null)
                m_laser.Dispose();

            base.Close();
        }

        protected override void InitAlarm()
        {
            base.InitAlarm();
        }

        public override int OnPrepareToWork()
        {
            if (m_rtc != null)
            {
                // basic frequency and pulse width
                // laser frequency : 50KHz, pulse width : 2usec (주파수 50KHz, 펄스폭 2usec)
                m_rtc.CtlFrequency(Recipe.Frequency * 1000, Recipe.PulseWidth);
                // basic sped
                // jump and mark speed : 500mm/s (점프, 마크 속도 500mm/s)
                //m_rtc.CtlSpeed(Recipe.JumpSpeed, Recipe.MarkSpeed);
                // basic delays
                // scanner and laser delays (스캐너/레이저 지연값 설정)
                //m_rtc.CtlDelay(Recipe.LaserOnDelay, Recipe.LaserOffDelay, Recipe.JumpDelay, Recipe.MarkDelay, 0);
            }

            if(m_laser!= null)
            {
                m_laser.CtlPower(Recipe.Power, "ETCHING");
            }

            //m_document = DocumentSerializer.OpenSirius(Recipe.DocumentPath) as DocumentDefault;           //  2025. 01. 02.  SCH : 왜 안될까?


            return base.OnPrepareToWork();
        }

        //protected bool MarkByMarker(IRtc rtc, ILaser laser, IMarker marker, IMotor motor)
        protected bool MarkByMarker(IRtc rtc, ILaser laser, IMarker marker)
        {
            bool bRet = false;

            //marker.Ready(new MarkerBase()
            //{
            //    Document = m_document,
            //    Rtc = rtc,
            //    Laser = laser,
            //    MotorZ = motor,
            //});

            ////Offset 추가.
            //marker.MarkerArg.Offsets.Clear();
            //marker.MarkerArg.Offsets.Add(new Offset(0, 0, (float)EtchingAngle));
            //m_bDrillingDone = false;
            return marker.Start();
        }
        public override int OnWork()
        {
            int ret = 0;
            if(Config.SimulationMode)
            {
                Thread.Sleep(1000);
                m_bDrillingDone = true;
            }
            else
            {
                if ((ret = base.OnWork()) != 0) return ret;
                if ((ret = Backward()) != 0) return ret;
                //if (!MarkByMarker(m_rtc, m_laser, m_drillingMarker, null)) return -1;
            }
            
            return ret;
        }
        public override int OnAfterWork()
        {
            int ret = base.OnAfterWork();
            if(ret != 0) return ret;

            while(true)
            {
                if (m_bDrillingDone)
                    break;

                Thread.Sleep(1);
            }

            return ret;
        }

        public override void UpdateConfigData()
        {
            base.UpdateConfigData();

        }

        public override void UpdateRecipeData()
        {
            base.UpdateRecipeData();

            
        }

        public virtual bool IsMirrorForward()
        {
            bool bRet = false;
            DioValue on, off;
            DioPoint dioInputMirrorForward = m_dicDioPoints[DioPointKey.Input_Mirror_FW.ToString()];
            DioPoint dioInputMirrorBackward = m_dicDioPoints[DioPointKey.input_Mirror_BW.ToString()];
            if (dioInputMirrorForward != null)
            {
                on = dioInputMirrorForward.GetValue();
            }
            else
            {
                on = DioValue.On;
            }

            if (dioInputMirrorBackward != null)
            {
                off = dioInputMirrorBackward.GetValue();
            }
            else
            {
                off = DioValue.Off;
            }

            if (on == DioValue.On && off == DioValue.Off)
            {
                bRet = true;
            }


            return bRet;
        }

        public virtual bool IsMirrorBackward()
        {
            bool bRet = false;
            DioValue on, off;
            DioPoint dioInputMirrorForward = m_dicDioPoints[DioPointKey.Input_Mirror_FW.ToString()];
            DioPoint dioInputMirrorBackward = m_dicDioPoints[DioPointKey.input_Mirror_BW.ToString()];
            if (dioInputMirrorForward != null)
            {
                on = dioInputMirrorForward.GetValue();
            }
            else
            {
                on = DioValue.Off;
            }

            if (dioInputMirrorBackward != null)
            {
                off = dioInputMirrorBackward.GetValue();
            }
            else
            {
                off = DioValue.On;
            }

            if (on == DioValue.Off && off == DioValue.On)
            {
                bRet = true;
            }


            return bRet;
        }
        public virtual int Forward()
        {
            int ret = 0;
            DioPoint dioOutputMirrorForward = m_dicDioPoints[DioPointKey.Output_Mirror_FW.ToString()];
            DioPoint dioOutputMirrorBackward = m_dicDioPoints[DioPointKey.Output_Mirror_BW.ToString()];
            if (dioOutputMirrorForward != null && dioOutputMirrorBackward != null)
            {
                if ((ret = dioOutputMirrorForward.Write(DioValue.On)) != 0) return ret;
                Thread.Sleep(10);
                if ((ret = dioOutputMirrorBackward.Write(DioValue.Off)) != 0) return ret;
            }
            else
            {
                ret = -1;
                return ret;
            }



            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsMirrorForward())
                    break;
                if (ResponseTimeout > 0)
                {
                    TimeSpan processTime = DateTime.Now - startTime;
                    if (processTime.TotalSeconds >= ResponseTimeout)
                    {
                        //Timeout
                        ret = -1;
                        break;
                    }
                }
                Thread.Sleep(1);
            }

            return ret;
        }

        public virtual Task<int> BeginForward()
        {
            return Task.Factory.StartNew(() =>
            {
                return Forward();
            });
        }

        public virtual int Backward()
        {
            int ret = 0;

            DioPoint dioOutputMirrorForward = m_dicDioPoints[DioPointKey.Output_Mirror_FW.ToString()];
            DioPoint dioOutputMirrorBackward = m_dicDioPoints[DioPointKey.Output_Mirror_BW.ToString()];

            if (dioOutputMirrorForward != null && dioOutputMirrorBackward != null)
            {
                if ((ret = dioOutputMirrorForward.Write(DioValue.Off)) != 0) return ret;
                if ((ret = dioOutputMirrorBackward.Write(DioValue.On)) != 0) return ret;
            }
            else
            {
                ret = -1;
                return ret;
            }

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (IsMirrorBackward())
                    break;
                if (ResponseTimeout > 0)
                {
                    TimeSpan processTime = DateTime.Now - startTime;
                    if (processTime.TotalSeconds >= ResponseTimeout)
                    {
                        //Timeout
                        ret = -1;
                        break;
                    }
                }
                Thread.Sleep(1);
            }
            return ret;
        }

        public virtual Task<int> BeginBackward()
        {
            return Task.Factory.StartNew(() =>
            {
                return Backward();
            });
        }

        public int EnableLaserEmission(bool bEnable)
        {
            int ret = 0;

            DioPoint dioLaserEnable = m_dicDioPoints[DioPointKey.Output_Laser_Enable.ToString()];
            if (dioLaserEnable != null)
            {
                if(bEnable)
                    ret = dioLaserEnable.Write(DioValue.On);
                else
                    ret = dioLaserEnable.Write(DioValue.Off);
            }

            return ret;
        }

        public int SavePowerMap()
        {
            int ret = 0;
            var mapFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map", "powermap.map");

            if (!PowerMapSerializer.Save(Map, mapFile))                             //  Sirius1
            //if (!PowerMapSerializer.Save(mapFile, Map))                           //  Sirius2
            {
                ret = -1;
            }

            return ret;
        }

        public int LoadPowerMap()
        {
            int ret = 0;

            //var mapFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "map", "powermap.map");
            //FileInfo info = new FileInfo(mapFile);
            //if (info.Exists)
            //{
            //    var pm = PowerMapSerializer.Open(mapFile, Map);
            //    //if (null != pm)                                               
            //    //{
            //    //    Map = pm as PowerMapDefault;
            //    //    m_laser.PowerMap = Map;
            //    //}
            //    //else
            //    //{
            //    //    ret = -1;
            //    //}
            //}
            //else
            //{
            //    ret = 1;
            //}            

            return ret;
        }
    }
}
