using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Tools;
using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    public class TwoPointAligner : PatternMatchingVisionPart
    {
        public enum AlarmKeys
        {
            eFailedPatternMatch = -9,
            eOutOfAngleTolerance = -10,
        }

        protected XytCoordinate[] m_Results;
        private MotionFunction MC_Func;
        private Task<int> task;
        //public XyStage Stage { set; get; }

        public WorkStageParameter workStageParameter { set; get; }                    //  Laser Drilling 에서 Vision 을 사용하려면 이걸 살려서 쓰자.

        public TwoPointAlignerRecipe Recipe { set; get; }


        public XytCoordinate[] Results
        {
            set
            {
                m_Results = value;
            }
            get
            {
                return m_Results;
            }
        }

        public XytCoordinate Result
        {
            set
            {
                m_Results[0] = value;
            }
            get
            {
                return m_Results[0];
            }
        }

        public VisionScale Scale
        {
            get
            {
                /*DispenserAndScale processor = Owner as DispenserAndScale;
                if (processor != null)
                {
                    return processor.Scale;
                }*/

                return null;
            }
        }

        public IlluminationDataSet IlluminationData
        {
            set
            {
                Recipe.IlluminationDataSet = value;
            }
            get
            {
                return Recipe.IlluminationDataSet;
            }
        }

        public TwoPointAligner(string strName) : base(strName)
        {
            Recipe = new TwoPointAlignerRecipe(this);
            //Result = new XytCoordinate();
            m_Results = new XytCoordinate[10];
            MC_Func = new InterpolatorMotionFunction();
        }

        protected override void InitAlarm()
        {
            base.InitAlarm();
            Alarm alarm = new Alarm();
            alarm.Code = (int)AlarmKeys.eFailedPatternMatch;
            alarm.Title = "Failed Pattern Match";
            alarm.Cause = "Failed Pattern Match";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);

            alarm = new Alarm();
            alarm.Code = (int)AlarmKeys.eOutOfAngleTolerance;
            alarm.Title = "Out Of Angle Tolerance Error";
            alarm.Cause = "Out Of Angle Tolerance";
            alarm.Source = Name;
            alarm.Grade = "Error";
            m_dicAlarms.Add(alarm.Code, alarm);
        }

        public override int Create()
        {
            int ret = 0;

            if ((ret = base.Create()) != 0) return ret;

            return ret;
        }

        public int Train()
        {
            int ret = 0;

            if ((ret = OnTrain(Recipe.TrainRoiStartLocation, Recipe.TrainRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return ret;
            }

            if (Recipe != null)
            {
                if (Recipe.PatternMatchingParameter == null)
                {
                    PatternMatchingParameters newParameter = new PatternMatchingParameters();
                    Recipe.PatternMatchingParameter = newParameter;
                }
                Recipe.PatternMatchingParameter.TrainImage = TrainImage;
                Owner.SaveRecipeData();
            }
            return ret;
        }

        public PatternMatchingResult Search()
        {
            int ret = 0;
            if ((ret = OnSearch(Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return null;
            }

            return m_PatternMatchingTool.Result;
        }

        public PatternMatchingResult Search(VisionImage image)
        {
            int ret = 0;
            if ((ret = OnSearch(image, Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                return null;
            }

            return m_PatternMatchingTool.Result;
        }

        public override void UpdateConfigData()
        {
            /*DispenserAndScale dispenser = Owner as DispenserAndScale;
            if (dispenser != null)
            {
                if (Recipe.IlluminationDataSet == null)
                    Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                Recipe.IlluminationDataSet.SetIlluminationChannel(dispenser.Config.ListIlluminationChannel);
            }*/
        }

        public override void Stop()
        {
            base.Stop();

            /*dispenserParameter.stDispenserPosParam = dispenserParameter.GetPositionInformation("Ready");
            for (int i = 0; i < dispenserParameter.stDispenserPosParam.nAxis.Count(); i++)
            {
                MC_Func.MC_MotorStop(dispenserParameter.stDispenserPosParam.nAxis[i], dispenserParameter.stDispenserPosParam.dDec[i]);
            }*/
        }

        public void ClearResults()
        {
            for (int i = 0; i < Results.Length; i++)
            {
                Results[i] = new XytCoordinate();
            }
        }

        public int OnePointAlign(XyCoordinate pos)
        {
            int ret = 0;

            List<PatternMatchingResult> listResults = new List<PatternMatchingResult>();

            VisionImage image = null;
            //if ((ret = Stage.MovePosition(position)) != 0) return ret;
            if ((ret = MovePosition(pos)) != 0) return ret;
            if (Recipe.DelayTimeBeforeScan > 0)
                Thread.Sleep(Recipe.DelayTimeBeforeScan);

            if (Illuminator != null)
            {
                if ((ret = OnSetIllumination(Recipe.IlluminationDataSet, true)) != 0) return ret;
            }

            this.Camera.GrabSync(out image);

            listResults.Add(Search(image));
            if (listResults[0] != null && listResults[0].Values.Count > 0)
            {
                XyCoordinate first = new XyCoordinate();
                first = pos + GetCoordinate(listResults[0].Values[0].X, listResults[0].Values[0].Y);
                XyCoordinate centerPos = new XyCoordinate(first.X + 7, first.Y);

                m_Results[0].X = centerPos.X;
                m_Results[0].Y = centerPos.Y;

                ret = 0;
            }
            else
            {
                ret = -1;
            }
            return ret;
        }

        public int OnePointAlign(int index, XyCoordinate pos)
        {
            int ret = 0;

            List<PatternMatchingResult> listResults = new List<PatternMatchingResult>();

            VisionImage image = null;
            //if ((ret = Stage.MovePosition(position)) != 0) return ret;
            if ((ret = MovePosition(pos)) != 0) return ret;
            if (Recipe.DelayTimeBeforeScan > 0)
                Thread.Sleep(Recipe.DelayTimeBeforeScan);

            if (Illuminator != null)
            {
                if ((ret = OnSetIllumination(Recipe.IlluminationDataSet, true)) != 0) return ret;
            }

            this.Camera.GrabSync(out image);
            if (task != null)
                task.Wait();

            //Task
            task = Task.Factory.StartNew((img) =>
            {
                VisionImage grabImage = img as VisionImage;
                ret = 0;
                listResults.Add(Search(grabImage));
                if (listResults[0] != null && listResults[0].Values.Count > 0)
                {
                    XyCoordinate first = new XyCoordinate();
                    first = pos + GetCoordinate(listResults[0].Values[0].X, listResults[0].Values[0].Y);
                    XyCoordinate centerPos = new XyCoordinate(first.X+7,first.Y);
                    //centerPos = first + 7;

                    m_Results[index].X = centerPos.X;
                    m_Results[index].Y = centerPos.Y;

                    ret = 0;
                }
                else
                {
                    ret = -1;
                }
                return ret;
            }, image);

            return ret;
        }

        public override int OnWork()
        {
            int ret = 0;

            XyCoordinate[] pos = new XyCoordinate[2] { Recipe.ReferencePosition, Recipe.GetSecondPosition() };
            XyCoordinate center = (pos[0] + pos[1]) / 2;
            List<PatternMatchingResult> listResults = new List<PatternMatchingResult>();

            foreach (XyCoordinate position in pos)
            {
                VisionImage image = null;
                //if ((ret = Stage.MovePosition(position)) != 0) return ret;
                if ((ret = MovePosition(position)) != 0) return ret;
                if (Recipe.DelayTimeBeforeScan > 0)

                    ret = 0;
                listResults.Add(Search());

            }

            if (listResults.Count == 2)
            {
                if (listResults[0] != null && listResults[0].Values.Count > 0 &&
                    listResults[1] != null && listResults[1].Values.Count > 0)
                {
                    XyCoordinate first = new XyCoordinate();
                    XyCoordinate second = new XyCoordinate();
                    first = pos[0] + GetCoordinate(listResults[0].Values[0].X, listResults[0].Values[0].Y);
                    second = pos[1] + GetCoordinate(listResults[1].Values[0].X, listResults[1].Values[0].Y);
                    double dAngle = GetAngle(first, second);
                    XyCoordinate Offset = new XyCoordinate();
                    Offset = ((first + second) / 2);

                    if (Recipe.InvertX)
                    {
                        m_Results[0].T = 180 + dAngle;
                    }
                    else
                    {
                        m_Results[0].T = dAngle;
                    }
                    m_Results[0].X = Offset.X;
                    m_Results[0].Y = Offset.Y;

                    ret = 0;
                }
                else
                {
                    ret = -1;
                }
            }

            return ret;
        }

        public int Work(int index, XyCoordinate[] pos)
        {
            int ret = 0;

            //XyCoordinate[] pos = new XyCoordinate[2] { Recipe.ReferencePosition, Recipe.GetSecondPosition() };
            XyCoordinate center = (pos[0] + pos[1]) / 2;
            List<PatternMatchingResult> listResults = new List<PatternMatchingResult>();

            foreach (XyCoordinate position in pos)
            {
                VisionImage image = null;
                //if ((ret = Stage.MovePosition(position)) != 0) return ret;
                if ((ret = MovePosition(position)) != 0) return ret;
                if (Recipe.DelayTimeBeforeScan > 0)
                    Thread.Sleep(Recipe.DelayTimeBeforeScan);

                if (Illuminator != null)
                {
                    if ((ret = OnSetIllumination(Recipe.IlluminationDataSet, true)) != 0) return ret;
                }

                this.Camera.GrabSync(out image);
                if (task != null)
                    task.Wait();
                //Task
                task = Task.Factory.StartNew((img) =>
                {
                    VisionImage grabImage = img as VisionImage;
                    ret = 0;
                    listResults.Add(Search(grabImage));
                    if (listResults.Count == 2)
                    {
                        if (listResults[0] != null && listResults[0].Values.Count > 0 &&
                            listResults[1] != null && listResults[1].Values.Count > 0)
                        {
                            XyCoordinate first = new XyCoordinate();
                            XyCoordinate second = new XyCoordinate();
                            first = pos[0] + GetCoordinate(listResults[0].Values[0].X, listResults[0].Values[0].Y);
                            second = pos[1] + GetCoordinate(listResults[1].Values[0].X, listResults[1].Values[0].Y);
                            double dAngle = GetAngle(first, second);
                            XyCoordinate Offset = new XyCoordinate();
                            Offset = ((first + second) / 2);

                            if (Recipe.InvertX)
                            {
                                m_Results[index].T = 180 + dAngle;
                            }
                            else
                            {
                                m_Results[index].T = dAngle;
                            }
                            m_Results[index].X = Offset.X;
                            m_Results[index].Y = Offset.Y;

                            ret = 0;
                        }
                        else
                        {
                            ret = -1;
                        }
                    }
                    return ret;
                }, image);
            }

            return ret;
        }

        protected int MovePosition(XyCoordinate position)
        {
            int ret = 0;

/*            if (dispenserParameter != null)
            {
                int nX = (int)DispenserAndScale.nDPAxis.X;
                int nY = (int)DispenserAndScale.nDPAxis.Y;
                dispenserParameter.stDispenserPosParam = dispenserParameter.GetPositionInformation("Ready");
                MC_Func.MC_MovePosition(dispenserParameter.stDispenserPosParam.nAxis[nX], position.X, dispenserParameter.stDispenserPosParam.dVel[0], dispenserParameter.stDispenserPosParam.dAcc[0], dispenserParameter.stDispenserPosParam.dDec[0]);
                MC_Func.MC_MovePosition(dispenserParameter.stDispenserPosParam.nAxis[nY], position.Y, dispenserParameter.stDispenserPosParam.dVel[1], dispenserParameter.stDispenserPosParam.dAcc[1], dispenserParameter.stDispenserPosParam.dDec[1]);

                while (true)
                {
                    Thread.Sleep(100);

                    if (MC_Func.MC_GetDone(dispenserParameter.stDispenserPosParam.nAxis[nX]) &&
                        MC_Func.MC_GetDone(dispenserParameter.stDispenserPosParam.nAxis[nY]))
                        break;
                }
            }*/

            return ret;
        }

        protected double GetAngle(XyCoordinate first, XyCoordinate second)
        {
            XyCoordinate pos = second - first;

            return Math.Atan2(pos.Y, pos.X) * 180 / Math.PI;
        }

        protected XyCoordinate GetCoordinate(double dX, double dY)
        {
            XyCoordinate coordinate = new XyCoordinate();
            coordinate.X = (dX - this.Camera.Resolution.Width / 2) * this.Scale.X * (this.Scale.InvertedX ? 1 : -1);
            coordinate.Y = (dY - this.Camera.Resolution.Height / 2) * this.Scale.Y * (this.Scale.InvertedY ? 1 : -1);
            return coordinate;
        }
    }
}
