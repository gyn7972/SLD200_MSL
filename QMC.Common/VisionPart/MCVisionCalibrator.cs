using QMC.Common.Motion.Ajin.Motions;
using QMC.Common.Parts;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    public class MCVisionCalibrator : VisionCalibrator
    {
        public enum AxisKey
        {
            X = 0,
            Y = 1,
        }

        public enum MounterAxisKey
        {
            X = 0,
            Y = 2
        }

        private MotionFunction MC_Func;

        public WorkStageParameter workStageParameter { set; get; }                            //  Laser Drilling 에서 Vision 을 사용해야 하면 이걸 갖다 쓰자. 

        public MCVisionCalibrator(string strName) : base(strName)
        {
            MC_Func = new InterpolatorMotionFunction();
        }

//        protected override int PatternMatchingAfterMove(XyCoordinate coordinate, out PatternMatchingResult result)
//        {
//            int ret = 0;
//            result = null;

//            #region 주석


//            //if (dispenserParameter == null)
//            //    return -1;

//            ////if ((ret = this.XytStage.MovePosition(coordinate)) != 0) return ret;
//            //dispenserParameter.stDispenserPosParam = dispenserParameter.GetPositionInformation("Ready");
//            //int nAxis = (int)AxisKey.X;
//            //MC_Func.MC_MovePosition(dispenserParameter.stDispenserPosParam.nAxis[nAxis], coordinate.X, dispenserParameter.stDispenserPosParam.dVel[nAxis], dispenserParameter.stDispenserPosParam.dAcc[nAxis], dispenserParameter.stDispenserPosParam.dDec[nAxis]);
//            //nAxis = (int)AxisKey.Y;
//            //MC_Func.MC_MovePosition(dispenserParameter.stDispenserPosParam.nAxis[nAxis], coordinate.Y, dispenserParameter.stDispenserPosParam.dVel[nAxis], dispenserParameter.stDispenserPosParam.dAcc[nAxis], dispenserParameter.stDispenserPosParam.dDec[nAxis]);
//            //while(true)
//            //{
//            //    Thread.Sleep(100);
//            //    if(MC_Func.MC_GetDone(dispenserParameter.stDispenserPosParam.nAxis[(int)AxisKey.X]) && MC_Func.MC_GetDone(dispenserParameter.stDispenserPosParam.nAxis[(int)AxisKey.Y]))
//            //    {
//            //        break;
//            //    }
//            //}

//            //for (int i = 0; i < 5; i++)
//            //{
//            //    result = this.Search();

//            //    if (result.Values.Count > 0)
//            //    {
//            //        break;
//            //    }
//            //}

//            //if (result.Values.Count < 1)
//            //{
//            //    //Matching 실패.
//            //    ret = -1;
//            //}

//            //FireUpdateResult(result);
//            #endregion

///*            if (dispenserParameter != null)
//            {
//                //if ((ret = this.XytStage.MovePosition(coordinate)) != 0) return ret;
//                dispenserParameter.stDispenserPosParam = dispenserParameter.GetPositionInformation("Ready");
//                int nAxis = (int)AxisKey.X;
//                MC_Func.MC_MovePosition(dispenserParameter.stDispenserPosParam.nAxis[nAxis], coordinate.X, dispenserParameter.stDispenserPosParam.dVel[nAxis], dispenserParameter.stDispenserPosParam.dAcc[nAxis], dispenserParameter.stDispenserPosParam.dDec[nAxis]);
//                nAxis = (int)AxisKey.Y;
//                MC_Func.MC_MovePosition(dispenserParameter.stDispenserPosParam.nAxis[nAxis], coordinate.Y, dispenserParameter.stDispenserPosParam.dVel[nAxis], dispenserParameter.stDispenserPosParam.dAcc[nAxis], dispenserParameter.stDispenserPosParam.dDec[nAxis]);
//                while (true)
//                {
//                    Thread.Sleep(100);
//                    if (MC_Func.MC_GetDone(dispenserParameter.stDispenserPosParam.nAxis[(int)AxisKey.X]) && MC_Func.MC_GetDone(dispenserParameter.stDispenserPosParam.nAxis[(int)AxisKey.Y]))
//                    {
//                        break;
//                    }
//                }

//                for (int i = 0; i < 5; i++)
//                {
//                    result = this.Search();

//                    if (result.Values.Count > 0)
//                    {
//                        break;
//                    }
//                }

//                if (result.Values.Count < 1)
//                {
//                    //Matching 실패.
//                    ret = -1;
//                }

//                FireUpdateResult(result);
//            }
//            else if (mounterParameter != null)
//            {
//                //if ((ret = this.XytStage.MovePosition(coordinate)) != 0) return ret;
//                mounterParameter.stFeederMounterPosParam = mounterParameter.GetPositionInformation("Ready");
//                int nAxis = (int)MounterAxisKey.X;
//                MC_Func.MC_MovePosition(mounterParameter.stFeederMounterPosParam.nAxis[nAxis], coordinate.X, mounterParameter.stFeederMounterPosParam.dVel[nAxis], mounterParameter.stFeederMounterPosParam.dAcc[nAxis], mounterParameter.stFeederMounterPosParam.dDec[nAxis]);
//                nAxis = (int)MounterAxisKey.Y;
//                MC_Func.MC_MovePosition(mounterParameter.stFeederMounterPosParam.nAxis[nAxis], coordinate.Y, mounterParameter.stFeederMounterPosParam.dVel[nAxis], mounterParameter.stFeederMounterPosParam.dAcc[nAxis], mounterParameter.stFeederMounterPosParam.dDec[nAxis]);
//                while (true)
//                {
//                    Thread.Sleep(100);
//                    if (MC_Func.MC_GetDone(mounterParameter.stFeederMounterPosParam.nAxis[(int)AxisKey.X]) && MC_Func.MC_GetDone(mounterParameter.stFeederMounterPosParam.nAxis[(int)AxisKey.Y]))
//                    {
//                        break;
//                    }
//                }

//                for (int i = 0; i < 5; i++)
//                {
//                    result = this.Search();

//                    if (result.Values.Count > 0)
//                    {
//                        break;
//                    }
//                }

//                if (result.Values.Count < 1)
//                {
//                    //Matching 실패.
//                    ret = -1;
//                }

//                FireUpdateResult(result);
//            }
//            else
//            {
//                return -1;
//            }*/

//            return ret;
//        }

        public new XytCoordinate GetCurrentPosition()
        {
            XytCoordinate current = new XytCoordinate();

/*            if (dispenserParameter != null)
            {

                dispenserParameter.stDispenserPosParam = dispenserParameter.GetPositionInformation("Ready");

                current.X = MC_Func.MC_GetEncPos(dispenserParameter.stDispenserPosParam.nAxis[0]);
                current.Y = MC_Func.MC_GetEncPos(dispenserParameter.stDispenserPosParam.nAxis[1]);
            }
            else if(mounterParameter !=null)
            {
                mounterParameter.stFeederMounterPosParam = mounterParameter.GetPositionInformation("Ready");

                current.X = MC_Func.MC_GetEncPos(mounterParameter.stFeederMounterPosParam.nAxis[0]);
                current.Y = MC_Func.MC_GetEncPos(mounterParameter.stFeederMounterPosParam.nAxis[2]);
            }*/
            
            return current;
        }

    }
}
