using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.PathGenerators;
using QMC.Common.Vision.Cameras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.PathGenerators.PathGenerator;

using SpiralLab.Sirius;

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
//using SpiralLab.Sirius2.Scanner.Rtc.SyncAxis;

using Vector2 = System.Numerics.Vector2;
using MessageBox = System.Windows.Forms.MessageBox;
//using netDxf.Collections;

namespace QMC.Common.Parts
{
    [Serializable]
    public class LaserPitchMoveShotter : Part
    {
        #region Define
        [Serializable]
        public enum GridXyMotionPositionKeys
        {
            StartPosition,
        }

        [Serializable]
        public enum ShotShape
        {
            Line,
            Circle,
            Rectangle,
            Cross
        }
        #endregion

        #region Field
        private WorkStage m_Owner;
        #endregion

        #region Constructor
        public LaserPitchMoveShotter(string strName) : base(strName)
        {


        }
        #endregion

        #region Property
        public Camera Camera { set; get; }
        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }
        //public UvwzxyzStage Stage { set; get; }
        public XyzLDzzxzULzzxzStage Stage { set; get; }
        public XyzyStage XyzyStage { set; get; }
        public LaserPitchMoveShotterConfig Config { set; get; }
        public TwoDimensionPathGenerator GridPathGenerator { get; set; }
        public RectangleZigzagTwoDimensionPathGeneratorParameter PathGeneratorParameter { get; set; }
        #endregion

        #region Method
        protected int GetMotionLimit(MotionAxis axis, out RangeD range)
        {
            int ret = 0;
            double maxValue = 0.0;
            double minValue = 0.0;
            range = new RangeD(double.NaN, double.NaN);

            if (axis == null) return ret;

            if ((ret = GetPosition(axis, ref minValue, ref maxValue)) != 0) return ret;
            //if ((ret = axis.Motor.PositivePosition.GetPosition(ref maxValue)) != 0) return ret;

            range = new RangeD(minValue, maxValue);

            return ret;
        }

        private int GetPosition(MotionAxis axis, ref double minValue, ref double MaxValue)
        {
            int ret = 0;
            minValue = axis.Motor.NegativePosition;
            MaxValue = axis.Motor.PositivePosition;
            return ret;
        }

        private int SetPathGeneratorParameter()
        {
            int ret = 0;

            this.PathGeneratorParameter = new RectangleZigzagTwoDimensionPathGeneratorParameter();
            //this.PathGeneratorParameter.InvertedX = this.Config.Parameter.PathInvetedX;
            //this.PathGeneratorParameter.InvertedX = this.Config.Parameter.PathInvertedY;
            this.PathGeneratorParameter.PathType = PathType.StepByStep;
            this.PathGeneratorParameter.StartCoordinate = (XyCoordinate)this.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;
            this.PathGeneratorParameter.PitchCount = new Size(this.Config.Count);
            this.PathGeneratorParameter.Direction = this.Config.Direction;
            this.PathGeneratorParameter.StartLocation = this.Config.StartLocation;
            this.PathGeneratorParameter.AreaCalculationParameterType = ZigzagTwoDimensionPathGenerator.AreaCalculationParameterTypes.PitchCount;

            this.PathGeneratorParameter.PitchDistance = new SizeD(Config.PitchDistanceX, Config.PitchDistanceY);

            return ret;
        }

        public int CreatePathGeneratorSync()
        {
            return this.CreatePathGeneratorProcedure();
        }

        private int CreatePathGeneratorProcedure()
        {
            int ret = 0;
            if ((ret = this.OnCreatePathGenerator()) != 0)
            {
                //this.DieUnloadPathGenerated = false;
                return ret;
            }
            //this.DieUnloadPathGenerated = true;
            return ret;
        }

        protected virtual int OnCreatePathGenerator()
        {
            int ret = 0;

            if ((ret = this.SetPathGeneratorParameter()) != 0) return ret;
            TwoDimensionPathGenerator generator = null;

            if ((ret = this.PathGeneratorParameter.CreatePathGenerator(out generator)) != 0) return ret;
            this.GridPathGenerator = generator;

            return ret;
        }

        bool DrawSquare(IRtc rtc, ILaser laser, MotionType motionType, float width = 40, float height = 40)               //  Sirius1
        //bool DrawSquare(IRtc rtc, ILaser laser, MotionTypes motionType, float width = 40, float height = 40)                //  Sirius2
        {
            bool success = true;

            if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_NONE)
            {
                MessageBox.Show("Scanner Mode 를 선택해야 합니다.", "Information !");
                return false;
            }

            string m_strTemp = "";
            float m_fLaserOnDelay = 0;
            float m_fLaserOffDelay = 0;
            float m_fMarkDelay = 0;
            float m_fJumpDelay = 0;
            float m_fMarkSpeed = 0;
            float m_fJumpSpeed = 0;

            m_fLaserOnDelay = (float)Convert.ToDouble(Config.LaserParameter.LaserOnTime.ToString().Trim());
            m_fLaserOffDelay = (float)Convert.ToDouble(Config.LaserParameter.LaserOffTime.ToString().Trim());
            m_fMarkSpeed = (float)Convert.ToDouble(Config.LaserParameter.MarkSpeed.ToString().Trim());
            m_fJumpSpeed = (float)Convert.ToDouble(Config.LaserParameter.JumpSpeed.ToString().Trim());

            var rtcMode = rtc as IRtc;                                      //  RTC6

            //Debug.Assert(rtcMode != null);

            success &= rtcMode.ListBegin(laser, ListType.Auto);
            //success &= rtcMode.ListBegin(ListTypes.Auto);         //  Sirius2

            success &= rtc.ListDelay(m_fLaserOnDelay, m_fLaserOffDelay, m_fJumpSpeed, m_fMarkSpeed, m_fMarkSpeed);

            success &= rtc.ListJump(new Vector2(-width / 2.0f, height / 2.0f));
            success &= rtc.ListMark(new Vector2(width / 2.0f, height / 2.0f));
            success &= rtc.ListMark(new Vector2(width / 2.0f, -height / 2.0f));
            success &= rtc.ListMark(new Vector2(-width / 2.0f, -height / 2.0f));
            success &= rtc.ListMark(new Vector2(-width / 2.0f, height / 2.0f));

            if (success)
            {
                success &= rtc.ListJump(Vector2.Zero);
                success &= rtc.ListEnd();
                success &= rtc.ListExecute(true);       // false);
            }

            return success;
        }

        bool DrawCross(IRtc rtc, ILaser laser, MotionType motionType, double width = 0.5, double height = 0.5)            //  Sirius1
        //bool DrawCross(IRtc rtc, ILaser laser, MotionTypes motionType, double width = 0.5, double height = 0.5)              //  Sirius2
        {
            bool success = true;

            if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_NONE)
            {
                MessageBox.Show("Scanner Mode 를 선택해야 합니다.", "Information !");
                return false;
            }

            string m_strTemp = "";
            float m_fLaserOnDelay = 0;
            float m_fLaserOffDelay = 0;
            float m_fMarkDelay = 0;
            float m_fJumpDelay = 0;
            float m_fPolygonDelay = 0;
            float m_fMarkSpeed = 0;
            float m_fJumpSpeed = 0;

            m_fLaserOnDelay = (float)Convert.ToDouble(Config.LaserParameter.LaserOnTime.ToString().Trim());
            m_fLaserOffDelay = (float)Convert.ToDouble(Config.LaserParameter.LaserOffTime.ToString().Trim());
            m_fMarkSpeed = (float)Convert.ToDouble(Config.LaserParameter.MarkSpeed.ToString().Trim());
            m_fJumpSpeed = (float)Convert.ToDouble(Config.LaserParameter.JumpSpeed.ToString().Trim());

            var rtcMode = rtc as IRtc;                                      //  RTC6

            //Debug.Assert(rtcMode != null);

            success &= rtcMode.ListBegin(laser, ListType.Auto);
            //success &= rtcMode.ListBegin(ListTypes.Auto);             //  Sirius2

            success &= rtc.ListDelay(m_fLaserOnDelay, m_fLaserOffDelay, m_fJumpSpeed, m_fMarkSpeed, m_fMarkSpeed);
            success &= rtc.ListSpeed(m_fJumpSpeed, m_fMarkSpeed);

            success &= rtc.ListJump(new Vector2(-(float)(width / 2.0), (float)0.0));
            success &= rtc.ListMark(new Vector2((float)(width / 2.0), (float)0.0));
            success &= rtc.ListJump(new Vector2((float)0.0, -(float)(height / 2.0)));
            success &= rtc.ListMark(new Vector2((float)0.0, (float)(height / 2.0)));

            if (success)
            {
                success &= rtc.ListJump(Vector2.Zero);
                success &= rtc.ListEnd();
                success &= rtc.ListExecute(true);       // false);
            }

            return success;
        }

        bool DrawCircle(IRtc rtc, ILaser laser, MotionType motionType, float radius = 20)                 //  Sirius1
        //bool DrawCircle(IRtc rtc, ILaser laser, MotionTypes motionType, float radius = 20)                   //  Sirius2
        {
            bool success = true;

            if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_NONE)
            {
                MessageBox.Show("Scanner Mode 를 선택해야 합니다.", "Information !");
                return false;
            }

            string m_strTemp = "";
            float m_fLaserOnDelay = 0;
            float m_fLaserOffDelay = 0;
            float m_fMarkDelay = 0;
            float m_fJumpDelay = 0;
            float m_fMarkSpeed = 0;
            float m_fJumpSpeed = 0;

            m_fLaserOnDelay = (float)Convert.ToDouble(Config.LaserParameter.LaserOnTime.ToString().Trim());
            m_fLaserOffDelay = (float)Convert.ToDouble(Config.LaserParameter.LaserOffTime.ToString().Trim());
            m_fMarkSpeed = (float)Convert.ToDouble(Config.LaserParameter.MarkSpeed.ToString().Trim());
            m_fJumpSpeed = (float)Convert.ToDouble(Config.LaserParameter.JumpSpeed.ToString().Trim());

            var rtcMode = rtc as IRtc;                                      //  RTC6

            //Debug.Assert(rtcMode != null);

            success &= rtcMode.ListBegin(laser, ListType.Auto);
            //success &= rtcMode.ListBegin(ListTypes.Auto);               //  Sirius2

            success &= rtc.ListDelay(m_fLaserOnDelay, m_fLaserOffDelay, m_fJumpSpeed, m_fMarkSpeed, m_fMarkSpeed);

            success &= rtc.ListJump(new Vector2(radius, 0));
            success &= rtc.ListArc(Vector2.Zero, 360.0f);
            success &= rtc.ListJump(Vector2.Zero);

            if (success)
            {
                success &= rtc.ListJump(Vector2.Zero);
                success &= rtc.ListEnd();
                success &= rtc.ListExecute(true);       // false);
            }

            return success;
        }

        private bool DrawLine(IRtc rtc, ILaser laser, MotionType motionType, float x1, float y1, float x2, float y2)              //  Sirius1
        //private bool DrawLine(IRtc rtc, ILaser laser, MotionTypes motionType, float x1, float y1, float x2, float y2)                //  Sirius2
        {
            if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_NONE)
            {
                MessageBox.Show("Scanner Mode 를 선택해야 합니다.", "Information !");
                return false;
            }

            if (rtc.CtlGetStatus(RtcStatus.Busy))
                return false;
            //Console.WriteLine("WARNING !!! LASER IS BUSY ... DrawLine");

            bool success = true;

            string m_strTemp = "";
            float m_fLaserOnDelay = 0;
            float m_fLaserOffDelay = 0;
            float m_fMarkDelay = 0;
            float m_fJumpDelay = 0;
            float m_fMarkSpeed = 0;
            float m_fJumpSpeed = 0;

            m_fLaserOnDelay = (float)Convert.ToDouble(Config.LaserParameter.LaserOnTime.ToString().Trim());
            m_fLaserOffDelay = (float)Convert.ToDouble(Config.LaserParameter.LaserOffTime.ToString().Trim());
            m_fMarkSpeed = (float)Convert.ToDouble(Config.LaserParameter.MarkSpeed.ToString().Trim());
            m_fJumpSpeed = (float)Convert.ToDouble(Config.LaserParameter.JumpSpeed.ToString().Trim());

            var rtcMode = rtc as IRtc;                                      //  RTC6

            //Debug.Assert(rtcMode != null);

            success &= rtcMode.ListBegin(laser, ListType.Auto);
            //success &= rtcMode.ListBegin(ListTypes.Auto);                   //  Sirius2

            success &= rtc.ListDelay(m_fLaserOnDelay, m_fLaserOffDelay, m_fJumpSpeed, m_fMarkSpeed, m_fMarkSpeed);

            success &= rtc.ListJump(new Vector2(x1, y1));
            success &= rtc.ListMark(new Vector2(x2, y2));

            if (success)
            {
                success &= rtc.ListJump(Vector2.Zero);
                success &= rtc.ListEnd();
                success &= rtc.ListExecute(true);       // false);
            }

            return success;
        }
        #endregion

        #region VisionPart Members
        public override int Create()
        {
            int ret = base.Create();


            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public override void Stop()
        {
            base.Stop();
            Stage.Stop();
        }

        public override int OnWork()
        {
            int ret = 0;
            m_Owner = this.Owner as WorkStage;
            if (m_Status == RunStatus.Stop) return 1;
            if (this.Stage == null) return -1;

            if ((ret = this.RunSearchGridXy()) != 0) return ret;

            return ret;
        }

        public override void UpdateConfigData() //참고 : Override
        {
            if (Owner is WorkStage)
            {
                WorkStage workStage = Owner as WorkStage;
                if (workStage != null)
                {
                    this.Config = workStage.Config.LaserPitchMoveShotterConfig;
                }
            }

        }

        private int ShotGridXy()
        {
            int ret = 0;

            CycleTimer timer = new CycleTimer();
            XyzCoordinate position = new XyzCoordinate();
            Dictionary<string, MovingProjection> dicMovingProjection = Stage.GetDefaultMovingProjections();

            if (m_Status == RunStatus.Stop) return 1;

            for (int i = 0; i < this.GridPathGenerator.Paths.Count; i++)
            {
                timer.Start();
                position = new XyzCoordinate(this.GridPathGenerator.Paths[i].X, this.GridPathGenerator.Paths[i].Y, this.Config.GridPositions[(int)GridXyMotionPositionKeys.StartPosition].Z);

                this.MoveStageXY(dicMovingProjection, position.X, position.Y);

                XyzCoordinate currentPos = new XyzCoordinate();
                this.Stage.GetCommandPosition(ref currentPos);
                Log.Write(this, $"Current Position : {currentPos}");

                this.OnShot();
            }

            return ret;
        }

        private int RunSearchGridXy()
        {
            int ret = 0;
            CycleTimer timer = new CycleTimer();

            if (m_Status == RunStatus.Stop) return 1;

            if ((ret = this.CreatePathGeneratorSync()) != 0) return ret;

            if ((ret = this.ShotGridXy()) != 0) return ret;

            return ret;
        }

        private int OnShot()
        {
            int ret = 0;

            //if (Equipment.RtcMode_syncAxis == (int)Equipment.RtcMode.RTC_NONE)
            if (Config.LaserParameter.RtcMode == Equipment.RtcMode.RTC_NONE)
            {
                //var mb1 = new MessageBoxOk();
                //mb1.ShowDialog("Information !", "먼저 Scanner Mode 를 선택해야 합니다.\n\n[EditMode 화면]");
                return ret;
            }

            int m_nIndex = -1;
            double m_dTemp1 = 0.0;
            double m_dTemp2 = 0.0;

            m_nIndex = (int)Config.LaserParameter.Shape;

            m_dTemp1 = Config.LaserParameter.ShotSize.Width;
            m_dTemp2 = Config.LaserParameter.ShotSize.Height;

            switch (m_nIndex)
            {
                case 0:             //  Line
                    //DrawLine(m_Owner.rtc6, m_Owner.laser, MotionType.ScannerOnly, (float)-m_dTemp1 / 2, (float)-m_dTemp1 / 2, (float)m_dTemp1 / 2, (float)m_dTemp1 / 2);
                    break;


                case 1:             //  Circle
                    //DrawCircle(m_Owner.rtc6, m_Owner.laser, MotionType.ScannerOnly, (float)m_dTemp1);
                    break;


                case 2:             //  Rectangle
                    //DrawSquare(m_Owner.rtc6, m_Owner.laser, MotionType.ScannerOnly, (float)m_dTemp1, (float)m_dTemp2);
                    break;


                case 3:             //  Cross
                    //DrawCross(m_Owner.rtc6, m_Owner.laser, MotionType.ScannerOnly, (float)m_dTemp1, (float)m_dTemp2);
                    break;
            }

            return ret;
        }

        public int MoveStageXY(Dictionary<string, MovingProjection> dicMovingProjection, double x, double y)
        {
            int ret = 0;

            //Get Motion Setting

            if (dicMovingProjection != null)
            {
                dicMovingProjection[XyzStage.MotionKey.X.ToString()].Position = x;
                dicMovingProjection[XyzStage.MotionKey.Y.ToString()].Position = y;

                this.Stage.Move(dicMovingProjection);
                Thread.Sleep(Config.MoveToDelay);
            }

            return ret;
        }
        #endregion
    }
}
