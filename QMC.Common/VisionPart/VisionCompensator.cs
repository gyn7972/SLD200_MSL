using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.PathGenerators;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.PathGenerators.PathGenerator;
using static QMC.Common.VisionPart.VisionCompensator;

namespace QMC.Common.VisionPart
{
    #region VisionCompensator
    public delegate void AutoFocusEvent(out AutoFocusResult result);
    public delegate void MoveZAxisEvent(double position);
    [Serializable]
    public class VisionCompensator : VisionPart
    {
        #region Define
        #region General

        [Serializable]
        public enum OperatorKeys
        {
            Measurement,
            Verification,
            All,
            Shot
        }

        [Serializable]
        public enum OperateMode
        {
            Grid,
            Cross,
        }

        [Serializable]
        public enum VerificationType
        {
            Grid,
            Cross,
            Random,
        }

        public AutoFocusEvent AutoFocusEventEvent;
        public MoveZAxisEvent MoveZAxisEvent;

        #endregion

        #region CrossLine
        [Serializable]
        public enum CrossLineMotionPositionKeys
        {
            Center,
            Top,
            Bottom,
            Left,
            Right,
        }

        [Serializable]
        public enum VerticalDirection
        {
            TopToBottom,
            BottomToTop,
        }

        [Serializable]
        public enum HorizontalDirection
        {
            LeftToRight,
            RightToLeft,
        }
        #endregion

        #region GridXy

        [Serializable]
        public enum GridXyMotionPositionKeys
        {
            StartPosition,
        }

        [Serializable]
        public enum Direction
        {
            /// <summary>
            /// 가로 방향.
            /// </summary> 
            Horizontal,

            /// <summary>
            /// 세로 방향.
            /// </summary>
            Vertical,
        }
        #endregion
        #endregion

        #region Field
        private WaferProbeAlign m_Owner;
        private bool m_WaferProbeAlignPathGenerated;
        #endregion

        #region Property
        //public XyztStage Stage { get; set; }
        //public XyzztStage Stage { get; set; }
        public UvwzxyzStage Stage { get; set; }
        public VisionCompensatorConfig Config { get; set; }
        public VisionScale Scale { set; get; }
        public LineSearchingVisionTool LineSearchingVisionTool { get; set; }
        public TwoDimensionPathGenerator GridPathGenerator { get; set; }
        public RectangleZigzagTwoDimensionPathGeneratorParameter PathGeneratorParameter { get; set; }
        public VisionCompensatorRecipe Recipe { get; set; }
        public bool DieUnloadPathGenerated
        {
            get { return this.m_WaferProbeAlignPathGenerated; }
            set { this.m_WaferProbeAlignPathGenerated = value; }
        }
        #endregion

        #region Constructor
        public VisionCompensator(string strName) : base(strName)
        {
        }
        #endregion

        #region VisionPart Members
        public override int Create()
        {
            int ret = base.Create();

            if (LineSearchingVisionTool == null)
            {
                LineSearchingVisionTool = new VisionProLineSearchingVisionTool("LineSearchVisionTool");
            }

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
            m_Owner = this.Owner as WaferProbeAlign;
            if (m_Status == RunStatus.Stop) return 1;
            if (this.Stage == null/* || this.Stage.XyPositionCompensator == null*/) return -1;
            //return ErrorManager.Register("Motion wrong setting detected.");

            //if (this.Parameter.OperateMode == OperateMode.Cross)
            //{
            //    if ((ret = this.RunSearchCrossLine()) != 0) return ret;
            //}
            //if (this.Parameter.OperateMode == OperateMode.Grid)
            //{
            if ((ret = this.RunSearchGridXy()) != 0) return ret;
            //}

            return ret;
        }

        public override void UpdateConfigData() //참고 : Override
        {
            if (Owner is WaferProbeAlign)
            {
                WaferProbeAlign waferProbeAlign = Owner as WaferProbeAlign;
                if (waferProbeAlign != null)
                {
                    this.Config = waferProbeAlign.Config.VisionCompensatorConfig;
                }
            }

        }
        #endregion

        #region Method
        private int SearchCrossLine(Enum direction, OperatorKeys operate, ref CompensationValue result)
        {
            int ret = 0;
            double pitch = 0.0;
            LineSearchingResult lineResult = null;
            double startCoordinate = 0.0;
            double endCoordinate = 0.0;
            LineD horizontalLine = new LineD();
            LineD verticalLine = new LineD();
            PointD intersectPoint = new PointD();
            //XyztCoordinate centerCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Center].Coordinate;
            //XyzztCoordinate centerCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Center].Coordinate;
            UvwzxyzCoordinate centerCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Center].Coordinate;
            LineSearchingVisionToolParameter parameter = new LineSearchingVisionToolParameter();
            if (m_Status == RunStatus.Stop) return 1;
            #region Horizontal Direction
            if (direction.GetType() == typeof(HorizontalDirection))
            {
                // 거리 및 시작 위치, 끝 위치 셋팅.
                if (this.Config.Parameter.HorizontalDirection == HorizontalDirection.LeftToRight)
                {
                    if (operate == OperatorKeys.Measurement)
                        pitch += this.Config.Parameter.PitchDistanceX;
                    else if (operate == OperatorKeys.Verification)
                        pitch += this.Config.Parameter.VerficationPitchDistanceX;

                    startCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Left].X;
                    endCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Right].X;
                }
                else
                {
                    if (operate == OperatorKeys.Measurement)
                        pitch += -this.Config.Parameter.PitchDistanceX;
                    else if (operate == OperatorKeys.Verification)
                        pitch += -this.Config.Parameter.VerficationPitchDistanceX;

                    startCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Right].X;
                    endCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Left].X;
                }

                for (double i = startCoordinate; i <= endCoordinate; i += pitch)
                {
                    if (m_Status == RunStatus.Stop) return 1;
                    Dictionary<string, MovingProjection> dicMovingProjection = Stage.GetDefaultMovingProjections();

                    dicMovingProjection[UvwzxyzStage.MotionKey.U.ToString()].Position = i;
                    dicMovingProjection[UvwzxyzStage.MotionKey.U.ToString()].Velocity = Config.Parameter.Velocity;
                    dicMovingProjection[UvwzxyzStage.MotionKey.V.ToString()].Position = centerCoordinate.V;
                    dicMovingProjection[UvwzxyzStage.MotionKey.V.ToString()].Velocity = Config.Parameter.Velocity;
                    dicMovingProjection[UvwzxyzStage.MotionKey.W.ToString()].Position = centerCoordinate.V;
                    dicMovingProjection[UvwzxyzStage.MotionKey.W.ToString()].Velocity = Config.Parameter.Velocity;
                    dicMovingProjection[UvwzxyzStage.MotionKey.EZ.ToString()].Position = centerCoordinate.EZ;
                    dicMovingProjection[UvwzxyzStage.MotionKey.EZ.ToString()].Velocity = Config.Parameter.Velocity;

                    if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;
                    //SafeThread.Delay(this.ConstructConfiguration.DelayAfterMove);
                    Thread.Sleep(Config.Parameter.MoveToDelay);

                    parameter.ExpectedLineAngle = 90;

                    // X(수평) 방향으로 움직일때 Y의 변화량. ( 수평 )                    
                    LineSearchingVisionTool.Parameter = parameter;
                    if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;
                    //LineSearchingAgent.Result.Lines.

                    if (lineResult.Lines.Count <= 0) continue;

                    //if (result.OffsetYAxisX.TargetValues.Count == 0)
                    horizontalLine = LineSearchingVisionTool.Result.Lines[0];

                    parameter.ExpectedLineAngle = 0;

                    // X(수평) 방향으로 움직일때 X의 변화량. ( 수직 )
                    LineSearchingVisionTool.Parameter = parameter;
                    if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

                    if (lineResult.Lines.Count <= 0) continue;

                    //if (result.OffsetXAxisX.TargetValues.Count == 0)
                    verticalLine = LineSearchingVisionTool.Result.Lines[0];

                    LineD.GetIntersectPoint(horizontalLine, verticalLine, out intersectPoint);

                    if (((WaferProbeAlign)this.Owner).Config.ParamConfig.ManualScale_Usage)            //  2023. 05. 26.  SCH : Scanner Compensator 하면서 Scale 있는 파일은 함께 수정해 봄.
                    {
                        VisionScale m_TempScale = new VisionScale();
                        m_TempScale.X = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_Scale_X;
                        m_TempScale.Y = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_Scale_Y;
                        m_TempScale.InvertedX = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X;
                        m_TempScale.InvertedY = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y;

                        result.OffsetXAxisX.Add((this.m_Owner.Camera_Upper.Resolution.Width / 2 - intersectPoint.X) * m_TempScale.X * (this.Config.Parameter.InvertedX == true ? 1 : -1));
                        result.OffsetYAxisX.Add((intersectPoint.Y - this.m_Owner.Camera_Upper.Resolution.Height / 2) * m_TempScale.Y * (this.Config.Parameter.InvertedY == true ? 1 : -1));

                        //  위에 Invert 안먹으면
                        //  (((WaferProbeAlign)this.Owner).Config.ParamConfig.HighVision_ScaleInvert_Y ? -1 : 1)  이걸로 교체
                    }
                    else
                    {
                        result.OffsetXAxisX.Add((this.m_Owner.Camera_Upper.Resolution.Width / 2 - intersectPoint.X) * this.m_Owner.Scale.X * (this.Config.Parameter.InvertedX == true ? 1 : -1));
                        result.OffsetYAxisX.Add((intersectPoint.Y - this.m_Owner.Camera_Upper.Resolution.Height / 2) * this.m_Owner.Scale.Y * (this.Config.Parameter.InvertedY == true ? 1 : -1));
                    }
                }

                //for (int i = 0; i < result.OffsetYAxisX.Count; i++)
                //{
                //    Log.Write("MotionVisionCompensator", string.Format("[CrossMode] OffsetYAxisX [{0}] : {1}, {2}", operate.ToString(), result.OffsetYAxisX[i].Target.ToString("F" + this.ConstructConfiguration.DecimalPoint.ToString()), result.OffsetYAxisX.TargetValues[i].Value.ToString("F" + this.ConstructConfiguration.DecimalPoint.ToString())));
                //}

                //for (int i = 0; i < result.OffsetXAxisX.Count; i++)
                //{
                //    Log.Write("MotionVisionCompensator", string.Format("[CrossMode] OffsetXAxisX [{0}] : {1}, {2}", operate.ToString(), result.OffsetXAxisX.TargetValues[i].Target.ToString("F" + this.ConstructConfiguration.DecimalPoint.ToString()), result.OffsetXAxisX.TargetValues[i].Value.ToString("F" + this.ConstructConfiguration.DecimalPoint.ToString())));
                //}
            }
            #endregion

            #region VerticalDirection
            else if (direction.GetType() == typeof(VerticalDirection))
            {
                // 거리 및 시작 위치, 끝 위치 셋팅.
                if (this.Config.Parameter.VerticalDirection == VerticalDirection.BottomToTop)
                {
                    if (operate == OperatorKeys.Measurement)
                        pitch += this.Config.Parameter.PitchDistanceY;
                    else if (operate == OperatorKeys.Verification)
                        pitch += this.Config.Parameter.VerficationPitchDistanceY;

                    startCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Bottom].Y;
                    endCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Top].Y;
                }
                else
                {
                    if (operate == OperatorKeys.Measurement)
                        pitch += -this.Config.Parameter.PitchDistanceY;
                    else if (operate == OperatorKeys.Verification)
                        pitch += -this.Config.Parameter.VerficationPitchDistanceY;

                    startCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Top].Y;
                    endCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Bottom].Y;
                }

                for (double i = startCoordinate; i <= endCoordinate; i += pitch)
                {
                    if (m_Status == RunStatus.Stop) return 1;
                    Dictionary<string, MovingProjection> dicMovingProjection = Stage.GetDefaultMovingProjections();

                    dicMovingProjection[UvwzxyzStage.MotionKey.U.ToString()].Position = centerCoordinate.U;
                    dicMovingProjection[UvwzxyzStage.MotionKey.U.ToString()].Velocity = Config.Parameter.Velocity;
                    dicMovingProjection[UvwzxyzStage.MotionKey.V.ToString()].Position = i;
                    dicMovingProjection[UvwzxyzStage.MotionKey.V.ToString()].Velocity = Config.Parameter.Velocity;
                    dicMovingProjection[UvwzxyzStage.MotionKey.W.ToString()].Position = i;
                    dicMovingProjection[UvwzxyzStage.MotionKey.W.ToString()].Velocity = Config.Parameter.Velocity;
                    dicMovingProjection[UvwzxyzStage.MotionKey.EZ.ToString()].Position = centerCoordinate.EZ;
                    dicMovingProjection[UvwzxyzStage.MotionKey.EZ.ToString()].Velocity = Config.Parameter.Velocity;

                    if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;
                    //SafeThread.Delay(this.ConstructConfiguration.DelayAfterMove);
                    Thread.Sleep(Config.Parameter.MoveToDelay);

                    parameter.ExpectedLineAngle = 0;

                    // Y(수직) 방향으로 움직일때 X의 변화량.
                    LineSearchingVisionTool.Parameter = parameter;
                    if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

                    if (lineResult.Lines.Count <= 0) continue;

                    verticalLine = LineSearchingVisionTool.Result.Lines[0];

                    parameter.ExpectedLineAngle = 90;

                    // Y(수직) 방향으로 움직일때 Y의 변화량.
                    LineSearchingVisionTool.Parameter = parameter;
                    if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

                    if (lineResult.Lines.Count <= 0) continue;

                    horizontalLine = LineSearchingVisionTool.Result.Lines[0];

                    LineD.GetIntersectPoint(horizontalLine, verticalLine, out intersectPoint);

                    if (((WaferProbeAlign)this.Owner).Config.ParamConfig.ManualScale_Usage)            //  2023. 05. 26.  SCH : Scanner Compensator 하면서 Scale 있는 파일은 함께 수정해 봄.
                    {
                        VisionScale m_TempScale = new VisionScale();
                        m_TempScale.X = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_Scale_X;
                        m_TempScale.Y = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_Scale_Y;
                        m_TempScale.InvertedX = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X;
                        m_TempScale.InvertedY = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y;

                        result.OffsetXAxisY.Add((this.m_Owner.Camera_Upper.Resolution.Width / 2 - intersectPoint.X) * m_TempScale.X * (this.Config.Parameter.InvertedX == true ? 1 : -1));
                        result.OffsetYAxisY.Add((intersectPoint.Y - this.m_Owner.Camera_Upper.Resolution.Height / 2) * m_TempScale.Y * (this.Config.Parameter.InvertedY == true ? 1 : -1));

                        //  위에 Invert 안먹으면
                        //  (((WaferProbeAlign)this.Owner).Config.ParamConfig.HighVision_ScaleInvert_Y ? -1 : 1)  이걸로 교체
                    }
                    else
                    {
                        result.OffsetXAxisY.Add((this.m_Owner.Camera_Upper.Resolution.Width / 2 - intersectPoint.X) * this.m_Owner.Scale.X * (this.Config.Parameter.InvertedX == true ? 1 : -1));
                        result.OffsetYAxisY.Add((intersectPoint.Y - this.m_Owner.Camera_Upper.Resolution.Height / 2) * this.m_Owner.Scale.Y * (this.Config.Parameter.InvertedY == true ? 1 : -1));
                    }                    
                }

                //for (int i = 0; i < result.OffsetXAxisY.Count; i++)
                //{
                //    Log.Write("MotionVisionCompensator", string.Format("[CrossMode] OffsetXAxisY [{0}] : {1}, {2}", operate.ToString(), result.OffsetXAxisY.TargetValues[i].Target.ToString("F" + this.ConstructConfiguration.DecimalPoint.ToString()), result.OffsetXAxisY.TargetValues[i].Value.ToString("F" + this.ConstructConfiguration.DecimalPoint.ToString())));
                //}

                //for (int i = 0; i < result.OffsetYAxisY.Count; i++)
                //{
                //    Log.Write("MotionVisionCompensator", string.Format("[CrossMode] OffsetYAxisY [{0}] : {1}, {2}", operate.ToString(), result.OffsetYAxisY.TargetValues[i].Target.ToString("F" + this.ConstructConfiguration.DecimalPoint.ToString()), result.OffsetYAxisY.TargetValues[i].Value.ToString("F" + this.ConstructConfiguration.DecimalPoint.ToString())));
                //}
            }
            #endregion

            return ret;
        }

        private int RunSearchCrossLine()
        {
            int ret = 0;

            CompensationValue result = new CompensationValue();
            //CrossLinePositionCompensator compensator = null;

            //if (this.Simulation.IsSimulatedWithoutResource() == true) return ret;

            try
            {
                if (this.Config.Parameter.Operator == OperatorKeys.All ||
                    this.Config.Parameter.Operator == OperatorKeys.Measurement)
                {

                    //if (this.Motion.XyPositionCompensator != null)
                    //    this.Motion.XyPositionCompensator.Enabled = false;

                    // 수평 Scan 시작.
                    if ((ret = this.SearchCrossLine(this.Config.Parameter.HorizontalDirection, OperatorKeys.Measurement, ref result)) != 0) return ret;

                    // 수직 Scan 시작.
                    if ((ret = this.SearchCrossLine(this.Config.Parameter.VerticalDirection, OperatorKeys.Measurement, ref result)) != 0) return ret;

                    //Config.CrossSearchResults.Add(result);
                }

                if (this.Config.Parameter.Operator == OperatorKeys.All ||
                    this.Config.Parameter.Operator == OperatorKeys.Verification)
                {


                    result = new CompensationValue();

                    // 수평 Scan 시작.
                    if ((ret = this.SearchCrossLine(this.Config.Parameter.HorizontalDirection, OperatorKeys.Verification, ref result)) != 0) return ret;

                    // 수직 Scan 시작.
                    if ((ret = this.SearchCrossLine(this.Config.Parameter.VerticalDirection, OperatorKeys.Verification, ref result)) != 0) return ret;
                }
            }
            catch (Exception ex)
            {
                //ret = ErrorManager.Register(ex);
                return -1;
            }

            return ret;
        }

        private int SearchGridXy(OperatorKeys mode, out List<PositionOffset> results, out XyCoordinate CommandPosition)
        {
            int ret = 0;
            #region Local Variable
            //XyztCoordinate position = new XyztCoordinate();
            //XyzztCoordinate position = new XyzztCoordinate();
            UvwzxyzCoordinate position = new UvwzxyzCoordinate();
            VisionImage image = null;
            LineD horizontalLine = new LineD();
            LineD verticalLine = new LineD();
            //LineSearchingVisionToolParameter parameter = new LineSearchingVisionToolParameter();
            VisionProLineSearchingVisionToolParameter parameter = new VisionProLineSearchingVisionToolParameter();
            LineSearchingResult lineResult = null;
            PointD intersectPoint = new PointD();
            XyzCoordinate currentPosition = new XyzCoordinate();
            XyCoordinate resultPosition = new XyCoordinate();
            PositionOffset result = new PositionOffset();
            results = new List<PositionOffset>();
            CommandPosition = new XyCoordinate();
            CycleTimer timer = new CycleTimer();
            #endregion

            if (m_Status == RunStatus.Stop) return 1;
            //if ((ret = GridPathGenerator.Generate() != 0)) return ret;

            #region 주석
            //for (int i = 0; i < this.GridPathGenerator.Paths.Count; i++)
            //{
            //    timer.Start();
            //    position = new XyzCoordinate(this.GridPathGenerator.Paths[i].X, this.GridPathGenerator.Paths[i].Y, this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Z);
            //    Dictionary<string, MovingProjection> dicMovingProjection = Stage.GetDefaultMovingProjections();

            //    dicMovingProjection[XyzStage.MotionKey.X.ToString()].Position = position.X;
            //    dicMovingProjection[XyzStage.MotionKey.X.ToString()].Velocity = Config.Parameter.Velocity;
            //    dicMovingProjection[XyzStage.MotionKey.X.ToString()].Acceleration = Config.Parameter.Acc;
            //    dicMovingProjection[XyzStage.MotionKey.X.ToString()].Deceleration = Config.Parameter.Dcc;

            //    dicMovingProjection[XyzStage.MotionKey.Y.ToString()].Position = position.Y;
            //    dicMovingProjection[XyzStage.MotionKey.Y.ToString()].Velocity = Config.Parameter.Velocity;
            //    dicMovingProjection[XyzStage.MotionKey.Y.ToString()].Acceleration = Config.Parameter.Acc;
            //    dicMovingProjection[XyzStage.MotionKey.Y.ToString()].Deceleration = Config.Parameter.Dcc;

            //    dicMovingProjection[XyzStage.MotionKey.Z.ToString()].Position = position.Z;
            //    dicMovingProjection[XyzStage.MotionKey.Z.ToString()].Velocity = Config.Parameter.Velocity;
            //    dicMovingProjection[XyzStage.MotionKey.Z.ToString()].Acceleration = Config.Parameter.Acc;
            //    dicMovingProjection[XyzStage.MotionKey.Z.ToString()].Deceleration = Config.Parameter.Dcc;

            //    Log.Write("MotionTime", string.Format("Move Start!"));
            //    if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;
            //    Log.Write("MotionTime", string.Format("Move End!"));
            //    //SafeThread.Delay(this.ConstructConfiguration.DelayAfterMove);
            //    Thread.Sleep(Config.Parameter.MoveToDelay);

            //    XytCoordinate currentPos = new XytCoordinate();
            //    //this.Stage.GetCommandPosition(ref currentPos);
            //    this.Stage.GetActualPosition(ref currentPos);
            //    CommandPosition = (XyCoordinate)currentPos;

            //    if (this.Config.Parameter.Direction == ZigzagTwoDimensionPathGenerator.Direction.Horizontal)
            //    {
            //        #region Horizontal Search Direction
            //        parameter.ExpectedLineAngle = 90;

            //        parameter.RunParams.EdgeDetectionParams.ContrastThreshold = 15;
            //        parameter.RunParams.EdgeDetectionParams.GradientKernelSizeInPixels = 5;
            //        // X(수평) 방향으로 움직일때 Y의 변화량. ( 수평 )
            //        LineSearchingVisionTool.Parameter = parameter;
            //        this.Camera.GrabSync(out image);
            //        LineSearchingVisionTool.InputImage = image;
            //        LineSearchingVisionTool.Parameter.ExpectedLineAngle = parameter.ExpectedLineAngle;
            //        LineSearchingVisionTool.Parameter.PolarityConstant = LineSearchingVisionTool.PolarityConstants.DarkToLight;
            //        LineSearchingVisionTool.Parameter.AngleTolerance = parameter.AngleTolerance;
            //        LineSearchingVisionTool.Parameter.LineCount = parameter.LineCount;
            //        Log.Write("ToolTime", string.Format("LineSearchingVisionTool Start!"));
            //        if ((ret = this.LineSearchingVisionTool.Run()) != 0)
            //        {
            //            if ((ret = this.m_Owner.autoFocuser.Work()) != 0) return ret;

            //            this.Camera.GrabSync(out image);

            //            LineSearchingVisionTool.InputImage = image;

            //            if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

            //            if (this.LineSearchingVisionTool.Result.Lines.Count <= 0) continue;

            //            this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Z = this.m_Owner.autoFocuser.Config.FocusPosition / 1000;

            //            Log.Write("MotionVisionCompensator", string.Format("Horizontal LineSearching Failed"));
            //            return ret;
            //        }
            //        Log.Write("ToolTime", string.Format("LineSearchingVisionTool End!"));
            //        if (this.LineSearchingVisionTool.Result.Lines.Count <= 0)
            //        {
            //            if ((ret = this.m_Owner.autoFocuser.Work()) != 0) return ret;

            //            this.Camera.GrabSync(out image);

            //            LineSearchingVisionTool.InputImage = image;

            //            if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

            //            if (this.LineSearchingVisionTool.Result.Lines.Count <= 0) continue;

            //            this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Z = this.m_Owner.autoFocuser.Config.FocusPosition / 1000;
            //        }

            //        horizontalLine = this.LineSearchingVisionTool.Result.Lines[0];
            //        #endregion

            //        #region Vertical Search Direction
            //        parameter.ExpectedLineAngle = 0;

            //        // X(수평) 방향으로 움직일때 X의 변화량. ( 수직 )
            //        LineSearchingVisionTool.Parameter = parameter;
            //        if ((ret = this.LineSearchingVisionTool.Run()) != 0)
            //        {
            //            if ((ret = this.m_Owner.autoFocuser.Work()) != 0) return ret;

            //            this.Camera.GrabSync(out image);

            //            LineSearchingVisionTool.InputImage = image;

            //            if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

            //            if (this.LineSearchingVisionTool.Result.Lines.Count <= 0) continue;

            //            this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Z = this.m_Owner.autoFocuser.Config.FocusPosition / 1000;
            //            Log.Write("MotionVisionCompensator", string.Format("Vertical LineSearching Failed"));
            //            return ret;
            //        }

            //        if (this.LineSearchingVisionTool.Result.Lines.Count <= 0)
            //        {
            //            if ((ret = this.m_Owner.autoFocuser.Work()) != 0) return ret;

            //            this.Camera.GrabSync(out image);

            //            LineSearchingVisionTool.InputImage = image;

            //            if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

            //            if (this.LineSearchingVisionTool.Result.Lines.Count <= 0) continue;

            //            this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Z = this.m_Owner.autoFocuser.Config.FocusPosition / 1000;
            //        }

            //        verticalLine = this.LineSearchingVisionTool.Result.Lines[0];
            //        #endregion
            //    }
            //    #region Vertical - 수정필요
            //    else if (this.Config.Parameter.Direction == ZigzagTwoDimensionPathGenerator.Direction.Vertical)
            //    {
            //        #region Vertical Direction

            //        parameter.ExpectedLineAngle = 0;
            //        parameter.RunParams.EdgeDetectionParams.ContrastThreshold = 15;
            //        parameter.RunParams.EdgeDetectionParams.GradientKernelSizeInPixels = 5;
            //        // X(수평) 방향으로 움직일때 Y의 변화량. ( 수평 )
            //        LineSearchingVisionTool.Parameter = parameter;
            //        this.Camera.GrabSync(out image);
            //        LineSearchingVisionTool.InputImage = image;
            //        LineSearchingVisionTool.Parameter.ExpectedLineAngle = parameter.ExpectedLineAngle;
            //        LineSearchingVisionTool.Parameter.PolarityConstant = LineSearchingVisionTool.PolarityConstants.DarkToLight;
            //        LineSearchingVisionTool.Parameter.AngleTolerance = parameter.AngleTolerance;
            //        LineSearchingVisionTool.Parameter.LineCount = parameter.LineCount;
            //        // Y(수직) 방향으로 움직일때 X의 변화량.
            //        LineSearchingVisionTool.Parameter = parameter;
            //        if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

            //        if (lineResult.Lines.Count <= 0) continue;

            //        verticalLine = lineResult.Lines[0];

            //        parameter.ExpectedLineAngle = 90;

            //        // Y(수직) 방향으로 움직일때 Y의 변화량.
            //        LineSearchingVisionTool.Parameter = parameter;
            //        if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

            //        if (lineResult.Lines.Count <= 0) continue;

            //        horizontalLine = lineResult.Lines[0];
            //        #endregion
            //    }
            //    #endregion

            //    LineD.GetIntersectPoint(horizontalLine, verticalLine, out intersectPoint);
            //    if (this.Stage.GetActualPosition(ref currentPosition) != 0) continue;
            //    resultPosition = new XyCoordinate((this.m_Owner.Camera.Resolution.Width / 2 - intersectPoint.X) * this.m_Owner.Scale.X * (this.Config.Parameter.InvertedX == true ? 1 : -1), (intersectPoint.Y - this.m_Owner.Camera.Resolution.Height / 2) * this.m_Owner.Scale.Y * (this.Config.Parameter.InvertedY == true ? 1 : -1));

            //    result = new PositionOffset((XyCoordinate)currentPosition, resultPosition);
            //    timer.End();

            //    Log.Write("MotionVisionCompensator", string.Format("[GridMode][{0}] ({1}/{2}({3}%)) StartCoordinate: {4}, PathCoordinate: {5}, StageCurrent: {6}, SearchResult: {7}, Offset: {8}, InvertedX: {9}, InvertedY: {10}, Interval: {11} ms",
            //        mode.ToString(),
            //        i + 1,
            //        this.GridPathGenerator.Paths.Count,
            //        ((Convert.ToDouble(i + 1) / Convert.ToDouble(this.GridPathGenerator.Paths.Count)) * 100D).ToString("0.000"),
            //        this.PathGeneratorParameter.StartCoordinate,
            //        this.GridPathGenerator.Paths[i],
            //        currentPosition,
            //        resultPosition,
            //        result.Offset,
            //        this.Config.Parameter.InvertedX,
            //        this.Config.Parameter.InvertedY,
            //        timer.Latest.Interval
            //        ));

            //    Log.Write("VerificationMotionVisionCompensator", string.Format("[GridMode][{0}], Position : [X : {1}, Y : {2}], Result : [X : {3}, Y : {4}]", mode.ToString(), currentPosition.X.ToString("0.0000"), currentPosition.Y.ToString("0.000"), result.Offset.X.ToString("0.0000"), result.Offset.Y.ToString("0.0000")));
            //    results.Add(result);
            //}
            #endregion

            //XyztCoordinate startPosition = this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;
            //XyzztCoordinate startPosition = this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;
            UvwzxyzCoordinate startPosition = this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;
            //position = new XyzCoordinate(startPosition.X, startPosition.Y, this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Z);
            position = startPosition;
            Dictionary<string, MovingProjection> dicMovingProjection = Stage.GetDefaultMovingProjections();

            for (int y = 0; y < this.Config.Parameter.Count.Y; y++)
            {
                if (y != 0)
                {
                    position.Y += 5;
                }

                for (int x = 0; x < this.Config.Parameter.Count.X; x++)
                {
                    if(x == 0)
                    {
                        position.X = startPosition.X;
                    }
                    if (x != 0)
                    {
                        position.X += 5;
                    }

                    dicMovingProjection[UvwzxyzStage.MotionKey.U.ToString()].Position = position.U;
                    dicMovingProjection[UvwzxyzStage.MotionKey.V.ToString()].Position = position.V;
                    dicMovingProjection[UvwzxyzStage.MotionKey.W.ToString()].Position = position.V;
                    dicMovingProjection[UvwzxyzStage.MotionKey.EZ.ToString()].Position = position.EZ;

                    Log.Write("MotionTime", string.Format("Move Start!"));
                    if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;
                    Log.Write("MotionTime", string.Format("Move End!"));
                    //SafeThread.Delay(this.ConstructConfiguration.DelayAfterMove);
                    Thread.Sleep(Config.Parameter.MoveToDelay);

                    if (this.Config.Parameter.Direction == ZigzagTwoDimensionPathGenerator.Direction.Horizontal)
                    {
                        #region Horizontal Search Direction
                        parameter.ExpectedLineAngle = 90;
                        parameter.RunParams.EdgeDetectionParams.ContrastThreshold = 15;
                        parameter.RunParams.EdgeDetectionParams.GradientKernelSizeInPixels = 5;

                        // X(수평) 방향으로 움직일때 Y의 변화량. ( 수평 )
                        this.Camera.GrabSync(out image);
                        LineSearchingVisionTool.Parameter = parameter;
                        LineSearchingVisionTool.InputImage = image;
                        LineSearchingVisionTool.Parameter.ExpectedLineAngle = parameter.ExpectedLineAngle;
                        LineSearchingVisionTool.Parameter.PolarityConstant = LineSearchingVisionTool.PolarityConstants.DarkToLight;
                        LineSearchingVisionTool.Parameter.AngleTolerance = parameter.AngleTolerance;
                        LineSearchingVisionTool.Parameter.LineCount = parameter.LineCount;

                        if ((ret = this.LineSearchingVisionTool.Run()) != 0)
                        {
                            if ((ret = this.m_Owner.autoFocuser_Upper.Work()) != 0)
                            {
                                return ret;
                            }

                            this.Camera.GrabSync(out image);

                            LineSearchingVisionTool.InputImage = image;

                            if ((ret = this.LineSearchingVisionTool.Run()) != 0)
                            {
                                return ret;
                            }

                            if (this.LineSearchingVisionTool.Result.Lines.Count <= 0)
                            {
                                continue;
                            }

                            this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].EZ = this.m_Owner.autoFocuser_Upper.Config.FocusPosition / 1000;

                            Log.Write("MotionVisionCompensator", string.Format("Horizontal LineSearching Failed"));
                            return ret;
                        }
                        if (this.LineSearchingVisionTool.Result.Lines.Count <= 0)
                        {
                            if ((ret = this.m_Owner.autoFocuser_Upper.Work()) != 0) return ret;

                            this.Camera.GrabSync(out image);

                            LineSearchingVisionTool.InputImage = image;

                            if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

                            if (this.LineSearchingVisionTool.Result.Lines.Count <= 0) continue;

                            this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].EZ = this.m_Owner.autoFocuser_Upper.Config.FocusPosition / 1000;
                        }

                        horizontalLine = this.LineSearchingVisionTool.Result.Lines[0];
                        #endregion

                        #region Vertical Search Direction
                        parameter.ExpectedLineAngle = 0;

                        // X(수평) 방향으로 움직일때 X의 변화량. ( 수직 )
                        LineSearchingVisionTool.Parameter = parameter;
                        if ((ret = this.LineSearchingVisionTool.Run()) != 0)
                        {
                            if ((ret = this.m_Owner.autoFocuser_Upper.Work()) != 0)
                            {
                                return ret;
                            }

                            this.Camera.GrabSync(out image);

                            LineSearchingVisionTool.InputImage = image;

                            if ((ret = this.LineSearchingVisionTool.Run()) != 0)
                            {
                                return ret;
                            }

                            if (this.LineSearchingVisionTool.Result.Lines.Count <= 0)
                            {
                                continue;
                            }

                            this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].EZ = this.m_Owner.autoFocuser_Upper.Config.FocusPosition / 1000;
                            Log.Write("MotionVisionCompensator", string.Format("Vertical LineSearching Failed"));
                            return ret;
                        }

                        if (this.LineSearchingVisionTool.Result.Lines.Count <= 0)
                        {
                            if ((ret = this.m_Owner.autoFocuser_Upper.Work()) != 0) return ret;

                            this.Camera.GrabSync(out image);

                            LineSearchingVisionTool.InputImage = image;

                            if ((ret = this.LineSearchingVisionTool.Run()) != 0) return ret;

                            if (this.LineSearchingVisionTool.Result.Lines.Count <= 0) continue;

                            this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].EZ = this.m_Owner.autoFocuser_Upper.Config.FocusPosition / 1000;
                        }

                        verticalLine = this.LineSearchingVisionTool.Result.Lines[0];
                        #endregion
                    }

                    LineD.GetIntersectPoint(horizontalLine, verticalLine, out intersectPoint);
                    if (this.Stage.GetActualPosition(ref currentPosition) != 0) continue;

                    if (((WaferProbeAlign)this.Owner).Config.ParamConfig.ManualScale_Usage)            //  2023. 05. 26.  SCH : Scanner Compensator 하면서 Scale 있는 파일은 함께 수정해 봄.
                    {
                        VisionScale m_TempScale = new VisionScale();
                        m_TempScale.X = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_Scale_X;
                        m_TempScale.Y = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_Scale_Y;
                        m_TempScale.InvertedX = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_X;
                        m_TempScale.InvertedY = ((WaferProbeAlign)this.Owner).Config.ParamConfig.UpperVision_ScaleInvert_Y;

                        resultPosition = new XyCoordinate((this.m_Owner.Camera_Upper.Resolution.Width / 2 - intersectPoint.X) * m_TempScale.X * (this.Config.Parameter.InvertedX == true ? 1 : -1), (intersectPoint.Y - this.m_Owner.Camera_Upper.Resolution.Height / 2) * m_TempScale.Y * (this.Config.Parameter.InvertedY == true ? 1 : -1));

                        //  위에 Invert 안먹으면
                        //  (((WaferProbeAlign)this.Owner).Config.ParamConfig.HighVision_ScaleInvert_Y ? -1 : 1)  이걸로 교체
                    }
                    else
                    {
                        resultPosition = new XyCoordinate((this.m_Owner.Camera_Upper.Resolution.Width / 2 - intersectPoint.X) * this.m_Owner.Scale.X * (this.Config.Parameter.InvertedX == true ? 1 : -1), (intersectPoint.Y - this.m_Owner.Camera_Upper.Resolution.Height / 2) * this.m_Owner.Scale.Y * (this.Config.Parameter.InvertedY == true ? 1 : -1));
                    }                                       

                    result = new PositionOffset((XyCoordinate)currentPosition, resultPosition);
                    timer.End();

                    Log.Write("MotionVisionCompensator", string.Format("({0}/{1}({2}%)) StartCoordinate: {3}, StageCurrent: {4}, Offset: {5}, Interval: {6} ms",
                        (y * this.Config.Parameter.Count.X) + x + 1,
                        this.Config.Parameter.Count,
                        ((Convert.ToDouble((y * this.Config.Parameter.Count.X) + x + 1) / Convert.ToDouble(this.GridPathGenerator.Paths.Count)) * 100D).ToString("0.000"),
                        startPosition,
                        currentPosition,
                        result.Offset,
                        timer.Latest.Interval
                        ));

                    Log.Write("VerificationMotionVisionCompensator", string.Format("[GridMode][{0}], Position : [X : {1}, Y : {2}], Result : [X : {3}, Y : {4}]", mode.ToString(), currentPosition.X.ToString("0.0000"), currentPosition.Y.ToString("0.000"), result.Offset.X.ToString("0.0000"), result.Offset.Y.ToString("0.0000")));
                    Log.Write("VisionCompensatorX", String.Format($"{result.Offset.X}"));
                    Log.Write("VisionCompensatorY", String.Format($"{result.Offset.Y}"));

                    Log.Write("CheckOffsetMove", String.Format($"Before Current : {currentPosition}"));
                    dicMovingProjection[UvwzxyzStage.MotionKey.U.ToString()].Position = position.U + result.Offset.X;
                    dicMovingProjection[UvwzxyzStage.MotionKey.V.ToString()].Position = position.V + result.Offset.Y;
                    dicMovingProjection[UvwzxyzStage.MotionKey.W.ToString()].Position = position.V + result.Offset.Y;
                    dicMovingProjection[UvwzxyzStage.MotionKey.EZ.ToString()].Position = position.EZ;

                    if ((ret = this.Stage.Move(dicMovingProjection)) != 0) return ret;

                    if (this.Stage.GetActualPosition(ref currentPosition) != 0) continue;

                    Log.Write("CheckOffsetMovoe", String.Format($"After Current : {currentPosition}"));

                    results.Add(result);
                }
            }

            return ret;
        }

        private int RunSearchGridXy()
        {
            int ret = 0;
            List<PositionOffset> results = null;
            XyCoordinate position = new XyCoordinate();
            //GridPositionCompensator compensator = null;
            CycleTimer timer = new CycleTimer();
            //this.Motion.XyPositionCompensator.Enabled = false;
            if (m_Status == RunStatus.Stop) return 1;
            if (this.Config.Parameter.Operator == OperatorKeys.All || this.Config.Parameter.Operator == OperatorKeys.Measurement)
            {
                timer.Start();
                if ((ret = this.CreatePathGeneratorSync(OperatorKeys.Measurement)) != 0) return ret;
                timer.End();
                Log.Write("MotionVisionCompensator", string.Format("Create Path Generator [{0}] Completed. Inverval: {1} ms", OperatorKeys.Measurement, timer.Latest.Interval));

                timer.Start();
                if ((ret = this.SearchGridXy(OperatorKeys.Measurement, out results, out position)) != 0) return ret;
                timer.End();
                Log.Write("MotionVisionCompensator", string.Format("Search Grid [{0}] Complete. Inverval: {1} ms", OperatorKeys.Measurement, timer.Latest.Interval));

                Config.XyGridSearchResults = results;
                Config.Positions.Add(position);

                //if (this.Motion.XyPositionCompensator != null && this.Motion.XyPositionCompensator is GridPositionCompensator)
                //{
                //    compensator = this.Motion.XyPositionCompensator as GridPositionCompensator;
                //    compensator.SetOffset(result);
                //}
            }
            //if (this.Config.Parameter.Operator == OperatorKeys.All || this.Config.Parameter.Operator == OperatorKeys.Verification)
            //{
            //    //this.Motion.XyPositionCompensator.Enabled = true;
            //    timer.Start();
            //    if ((ret = this.CreatePathGeneratorSync(OperatorKeys.Verification)) != 0) return ret;
            //    timer.End();
            //    Log.Write("MotionVisionCompensator", string.Format("Create Path Generator [{0}] Completed. Inverval: {1} ms", OperatorKeys.Verification, timer.Latest.Interval));

            //    timer.Start();
            //    if ((ret = this.SearchGridXy(OperatorKeys.Verification, out results, out position)) != 0) return ret;
            //    timer.End();
            //    Log.Write("MotionVisionCompensator", string.Format("Search Grid [{0}] Complete. Inverval: {1} ms", OperatorKeys.Verification, timer.Latest.Interval));
            //}
            //this.Motion.XyPositionCompensator.Enabled = this.Motion.XyPositionCompensator.Configuration.Body.Enable;

            return ret;
        }

        private int OnGetLimitPosition()
        {
            int ret = 0;
            //XyztCoordinate centerCoordinate;
            //XyzztCoordinate centerCoordinate;
            UvwzxyzCoordinate centerCoordinate;
            RangeD xRange = new RangeD();
            RangeD yRange = new RangeD();

            if (this.Stage != null)
            {
                this.GetMotionLimit(this.Stage.Axes["X"], out xRange);
                this.GetMotionLimit(this.Stage.Axes["Y"], out yRange);
                if (this.Config.Parameter.OperateMode == OperateMode.Cross)
                {
                    centerCoordinate = this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Center].Coordinate;

                    if (xRange.Minimum != double.NaN && xRange.Maximum != double.NaN)
                    {
                        this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Right].Coordinate = new UvwzxyzCoordinate(xRange.Maximum, centerCoordinate.V, centerCoordinate.W, centerCoordinate.EZ, centerCoordinate.X, centerCoordinate.Y, centerCoordinate.VZ);
                        this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Left].Coordinate = new UvwzxyzCoordinate(yRange.Minimum, centerCoordinate.V, centerCoordinate.W, centerCoordinate.EZ, centerCoordinate.X, centerCoordinate.Y, centerCoordinate.VZ);
                    }

                    if (yRange.Minimum != double.NaN && yRange.Maximum != double.NaN)
                    {
                        this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Top].Coordinate = new UvwzxyzCoordinate(centerCoordinate.U, yRange.Maximum, yRange.Maximum, centerCoordinate.EZ, centerCoordinate.X, centerCoordinate.Y, centerCoordinate.VZ);
                        this.Config.Parameter.CrossPositions[(int)CrossLineMotionPositionKeys.Bottom].Coordinate = new UvwzxyzCoordinate(centerCoordinate.U, yRange.Minimum, yRange.Minimum, centerCoordinate.EZ, centerCoordinate.X, centerCoordinate.Y, centerCoordinate.VZ);
                    }
                }
                //PartConfigurator.Save(this.Configuration);
            }

            return ret;
        }

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

        private int SetPathGeneratorParameter(OperatorKeys keys)
        {
            int ret = 0;
            XyCoordinate xyCoord = new XyCoordinate();

            this.PathGeneratorParameter = new RectangleZigzagTwoDimensionPathGeneratorParameter();
            //this.PathGeneratorParameter.InvertedX = this.Config.Parameter.PathInvetedX;
            //this.PathGeneratorParameter.InvertedX = this.Config.Parameter.PathInvertedY;
            this.PathGeneratorParameter.PathType = PathType.StepByStep;

            //this.PathGeneratorParameter.StartCoordinate = (XyCoordinate)this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate;
            xyCoord.X = this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate.U;
            xyCoord.Y = this.Config.Parameter.CrossPositions[(int)GridXyMotionPositionKeys.StartPosition].Coordinate.V;
            this.PathGeneratorParameter.StartCoordinate = xyCoord;

            this.PathGeneratorParameter.PitchCount = new Size(this.Config.Parameter.Count);
            this.PathGeneratorParameter.Direction = this.Config.Parameter.Direction;
            this.PathGeneratorParameter.StartLocation = this.Config.Parameter.StartLocation;
            this.PathGeneratorParameter.AreaCalculationParameterType = ZigzagTwoDimensionPathGenerator.AreaCalculationParameterTypes.PitchCount;

            if (keys == OperatorKeys.Measurement)
                this.PathGeneratorParameter.PitchDistance = new SizeD(Config.Parameter.PitchDistanceX, Config.Parameter.PitchDistanceY);
            else if (keys == OperatorKeys.Verification)
                this.PathGeneratorParameter.PitchDistance = new SizeD(Config.Parameter.VerficationPitchDistanceX, Config.Parameter.VerficationPitchDistanceY);
            else if (keys == OperatorKeys.All)
                return -1; //ErrorManager.Register("Wrong OperatorKey Detected.");

            return ret;
        }

        public int CreatePathGeneratorSync(OperatorKeys keys)
        {
            return this.CreatePathGeneratorProcedure(keys);
        }

        private int CreatePathGeneratorProcedure(OperatorKeys keys)
        {
            int ret = 0;
            if ((ret = this.OnCreatePathGenerator(keys)) != 0)
            {
                this.DieUnloadPathGenerated = false;
                return ret;
            }
            this.DieUnloadPathGenerated = true;
            return ret;
        }

        protected virtual int OnCreatePathGenerator(OperatorKeys keys)
        {
            int ret = 0;

            if ((ret = this.SetPathGeneratorParameter(keys)) != 0) return ret;
            TwoDimensionPathGenerator generator = null;

            if ((ret = this.PathGeneratorParameter.CreatePathGenerator(out generator)) != 0) return ret;
            this.GridPathGenerator = generator;

            return ret;
        }

        #endregion

    }
    #endregion

    #region VisionCompensatorParameter
    [Serializable]
    public class VisionCompensatorParameter
    {
        [Browsable(false)]
        //public XyztPositionDataCollection CrossPositions { set; get; }
        //public XyzztPositionDataCollection CrossPositions { set; get; }
        public UvwzxyzPositionDataCollection CrossPositions { set; get; }

        [Browsable(false)]
        //public XyztPositionDataCollection GridPositions { set; get; }
        //public XyzztPositionDataCollection GridPositions { set; get; }
        public UvwzxyzPositionDataCollection GridPositions { set; get; }

        private OperatorKeys m_Operator;
        private OperateMode m_OperateMode;
        private VerticalDirection m_VerticalDirection;
        private HorizontalDirection m_HorizontalDirection;
        private ZigzagTwoDimensionPathGenerator.Direction m_Direction;
        private double m_PitchDistanceX;
        private double m_PitchDistanceY;
        private double m_VerificationPitchX;
        private double m_VerificationPitchY;
        private Point m_Count;
        private ZigzagTwoDimensionPathGenerator.StartLocation m_StartLocation;
        private double m_Velocity;
        private double m_Acc;
        private double m_Dcc;
        private bool m_InvertedX;
        private bool m_InvertedY;
        private int m_MoveToDelay;

        public VisionCompensatorParameter()
        {
            Init();
            CrossPositions.Clear();
            foreach (CrossLineMotionPositionKeys key in Enum.GetValues(typeof(CrossLineMotionPositionKeys)))
            {
                //XyztPositionData positionBase = new XyztPositionData();
                //XyzztPositionData positionBase = new XyzztPositionData();
                UvwzxyzPositionData positionBase = new UvwzxyzPositionData();

                positionBase.Name = key.ToString();
                CrossPositions.Add(positionBase);

                //XyztPositionData positionTarget = new XyztPositionData();
                //XyzztPositionData positionTarget = new XyzztPositionData();
                UvwzxyzPositionData positionTarget = new UvwzxyzPositionData();

                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                CrossPositions.Add(positionTarget);
            }

            GridPositions.Clear();
            foreach (GridXyMotionPositionKeys key in Enum.GetValues(typeof(GridXyMotionPositionKeys)))
            {
                //XyztPositionData positionBase = new XyztPositionData();
                //XyzztPositionData positionBase = new XyzztPositionData();
                UvwzxyzPositionData positionBase = new UvwzxyzPositionData();

                positionBase.Name = key.ToString();
                GridPositions.Add(positionBase);

                //XyztPositionData positionTarget = new XyztPositionData();
                //XyzztPositionData positionTarget = new XyzztPositionData();
                UvwzxyzPositionData positionTarget = new UvwzxyzPositionData();

                positionTarget.Name = key.ToString();
                positionTarget.Type = TargetType.Offset;
                GridPositions.Add(positionTarget);
            }
        }
        private void Init()
        {
            if (CrossPositions == null)
            {
                //CrossPositions = new XyztPositionDataCollection();
                //CrossPositions = new XyzztPositionDataCollection();
                CrossPositions = new UvwzxyzPositionDataCollection();
            }

            if (GridPositions == null)
            {
                //GridPositions = new XyztPositionDataCollection();
                //GridPositions = new XyzztPositionDataCollection();
                GridPositions = new UvwzxyzPositionDataCollection();
            }

            m_Operator = OperatorKeys.All;
            m_OperateMode = OperateMode.Cross;
            m_VerticalDirection = VerticalDirection.TopToBottom;
            m_HorizontalDirection = HorizontalDirection.RightToLeft;
            m_Direction = ZigzagTwoDimensionPathGenerator.Direction.Horizontal;
            m_PitchDistanceX = 0;
            m_PitchDistanceY = 0;
            m_VerificationPitchX = 0;
            m_VerificationPitchY = 0;
            m_Count = new Point();
            m_StartLocation = new ZigzagTwoDimensionPathGenerator.StartLocation();
            m_Velocity = 10;
            m_Acc = 100;
            m_Dcc = 100;
            m_InvertedX = false;
            m_InvertedY = false;
            m_MoveToDelay = 100;
        }

        public void UpdateGridPosition()
        {
            foreach (GridXyMotionPositionKeys key in Enum.GetValues(typeof(GridXyMotionPositionKeys)))
            {
                bool bFind = false;
                foreach (var position in GridPositions)
                {
                    if (position.Name == key.ToString())
                    {
                        bFind = true;
                        break;
                    }
                }
                if (!bFind)
                {
                    //XyztPositionData positionBase = new XyztPositionData();
                    //XyzztPositionData positionBase = new XyzztPositionData();
                    UvwzxyzPositionData positionBase = new UvwzxyzPositionData();

                    positionBase.Name = key.ToString();
                    GridPositions.Add(positionBase);

                    //XyztPositionData positionTarget = new XyztPositionData();
                    //XyzztPositionData positionTarget = new XyzztPositionData();
                    UvwzxyzPositionData positionTarget = new UvwzxyzPositionData();

                    positionTarget.Name = key.ToString();
                    positionTarget.Type = TargetType.Offset;
                    GridPositions.Add(positionTarget);
                }
            }
        }

        [Browsable(false)]
        public OperatorKeys Operator
        {
            get { return m_Operator; }
            set { m_Operator = value; }
        }
        [Browsable(false)]
        public OperateMode OperateMode
        {
            get { return m_OperateMode; }
            set { m_OperateMode = value; }
        }
        [Browsable(false)]
        public VerticalDirection VerticalDirection
        {
            get { return m_VerticalDirection; }
            set { m_VerticalDirection = value; }
        }
        [Browsable(false)]
        public HorizontalDirection HorizontalDirection
        {
            get { return m_HorizontalDirection; }
            set { m_HorizontalDirection = value; }
        }
        [Category("GridXY")]
        public ZigzagTwoDimensionPathGenerator.Direction Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }
        [Browsable(false)]
        public double PitchDistanceX
        {
            get { return m_PitchDistanceX; }
            set { m_PitchDistanceX = value; }
        }
        [Browsable(false)]
        public double VerficationPitchDistanceX
        {
            get { return m_PitchDistanceY; }
            set { m_PitchDistanceY = value; }
        }
        [Browsable(false)]
        public double VerficationPitchDistanceY
        {
            get { return m_VerificationPitchX; }
            set { m_VerificationPitchX = value; }
        }
        [Browsable(false)]
        public double PitchDistanceY
        {
            get { return m_VerificationPitchY; }
            set { m_VerificationPitchY = value; }
        }

        [Category("GridXY")]
        public Point Count
        {
            get { return m_Count; }
            set { m_Count = value; }
        }
        [Category("GridXY")]
        public ZigzagTwoDimensionPathGenerator.StartLocation StartLocation
        {
            get { return this.m_StartLocation; }
            set { this.m_StartLocation = value; }
        }
        [Browsable(false)]
        public double Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }
        [Browsable(false)]
        public double Acc
        {
            get { return m_Acc; }
            set { m_Acc = value; }
        }
        [Browsable(false)]
        public double Dcc
        {
            get { return m_Dcc; }
            set { m_Dcc = value; }
        }
        [Browsable(false)]
        public bool InvertedX
        {
            get { return m_InvertedX; }
            set { m_InvertedX = value; }
        }
        [Browsable(false)]
        public bool InvertedY
        {
            get { return m_InvertedY; }
            set { m_InvertedY = value; }
        }
        [Category("기타")]
        public int MoveToDelay
        {
            get { return m_MoveToDelay; }
            set { m_MoveToDelay = value; }
        }
    }
    #endregion

    #region CompensationValue
    [Serializable]
    public class CompensationValue
    {
        #region Define

        #endregion

        #region Field
        private List<double> m_OffsetXAxisX;
        private List<double> m_OffsetXAxisY;
        private List<double> m_OffsetYAxisX;
        private List<double> m_OffsetYAxisY;
        #endregion

        #region Constructor
        public CompensationValue()
        {
            this.OffsetXAxisX = new List<double>();
            this.OffsetXAxisY = new List<double>();
            this.OffsetYAxisX = new List<double>();
            this.OffsetYAxisY = new List<double>();
        }
        #endregion

        #region Porperty

        public List<double> OffsetXAxisX
        {
            get { return this.m_OffsetXAxisX; }
            set { this.m_OffsetXAxisX = value; }
        }

        public List<double> OffsetXAxisY
        {
            get { return this.m_OffsetXAxisY; }
            set { this.m_OffsetXAxisY = value; }
        }

        public List<double> OffsetYAxisX
        {
            get { return this.m_OffsetYAxisX; }
            set { this.m_OffsetYAxisX = value; }
        }

        public List<double> OffsetYAxisY
        {
            get { return this.m_OffsetYAxisY; }
            set { this.m_OffsetYAxisY = value; }
        }
        #endregion
    }
    #endregion

    #region PositionOffset
    [Serializable]
    public struct PositionOffset/* : IComparable, IComparer*/
    {
        private XyCoordinate m_Position;
        private XyCoordinate m_Offset;

        public PositionOffset(XyCoordinate position, XyCoordinate offset)
        {
            this.m_Position = position;
            this.m_Offset = offset;
        }

        public XyCoordinate Position
        {
            get { return this.m_Position; }
            set { this.m_Position = value; }
        }

        public XyCoordinate Offset
        {
            get { return this.m_Offset; }
            set { this.m_Offset = value; }
        }

        public override string ToString()
        {
            //return string.Format("Position = {0}, Offset = {1}", this.Position, this.Offset);                                 //  2023. 05. 19.  SCH : QMC.Vision 에는 이게 아니라
            return string.Format("{0}, {1}, {2}, {3}", this.Position.X, this.Position.Y, this.Offset.X, this.Offset.Y);         //                      이걸로 되어 있어서... 이렇게 해봄 흐흐
        }

        public void SetValue(string strValue)
        {
            string[] strValues = strValue.Split(',');

            if (strValues.Length == 4)
            {
                try
                {
                    m_Position.X = double.Parse(strValues[0]);
                    m_Position.Y = double.Parse(strValues[1]);
                    m_Offset.X = double.Parse(strValues[2]);
                    m_Offset.Y = double.Parse(strValues[3]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        //#region IComparable Members
        //public int CompareTo(object obj)
        //{
        //    MapPositionOffset b = (MapPositionOffset)obj;

        //    if (this.Position.X == b.Position.X && this.Position.Y == b.Position.Y) return 0;

        //    else if (this.Position.X < b.Position.X)
        //        return -1;
        //    else if (this.Position.X > b.Position.X)
        //        return 1;
        //    else if (this.Position.Y < b.Position.Y)
        //        return -1;
        //    else
        //        return 1;
        //}
        //#endregion
        //#region IComparer Members
        //public int Compare(object x, object y)
        //{
        //    MapPositionOffset a = (MapPositionOffset)x;
        //    MapPositionOffset b = (MapPositionOffset)y;

        //    return a.CompareTo(b.Position);
        //}
        //#endregion
    }
    #endregion
}
