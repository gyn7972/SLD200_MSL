using QMC.Common.Modules;
using QMC.Common.Vision;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    #region Revision
    public class Revision : PatternMatchingVisionPart
    {
        #region Field
        private QMCEdgeDetector[] m_qmcED;
        #endregion

        #region Property

        //private ReelFeederAndMounter m_Owner;                             //  Laser Drilling 에서 Vision 을 사용하려면 이걸 살려서 쓰자.
        public RevisionRecipe Recipe { get; set; }

        public RevisionResult SearchResult { set; get; }

        public IlluminationDataSet IlluminationBottomData
        {
            set
            {
                Recipe.IlluminationBottomDataSet = value;
            }
            get
            {
                return Recipe.IlluminationBottomDataSet;
            }
        }

        public IlluminationDataSet IlluminationMounterData
        {
            set
            {
                Recipe.IlluminationMounterDataSet = value;
            }
            get
            {
                return Recipe.IlluminationMounterDataSet;
            }
        }
        public VisionScale Scale
        {
            get
            {
/*                ReelFeederAndMounter mounter = Owner as ReelFeederAndMounter;
                if (mounter != null)
                    return mounter.BottomScale;
                else*/
                    return null;
            }
        }
        #endregion

        #region Constructor
        public Revision(string strName) : base(strName)
        {
            Recipe = new RevisionRecipe(this);

            m_qmcED = new QMCEdgeDetector[2];
            for (int iter = 0; iter < 2; iter++)
            {
                m_qmcED[iter] = new QMCEdgeDetector();
            }

            //m_Owner = Owner as ReelFeederAndMounter;
        }
        #endregion

        #region Method
        public int Train()
        {
            int ret = 0;

            if ((ret = OnTrain(Recipe.TrainRoiStartLocation, Recipe.TrainRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationBottomData)) != 0)
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

        private int BeforeSearchDies(/*Collet collet*/)
        {
            int ret = 0;


            //if (ColletCountDatas[collet.ArmIndex] == null)
            //{
            //    ColletCountDatas.Add(new ColletCountData(collet.ArmIndex));
            //}

            return ret;
        }

        public PatternMatchingResult Search()
        {
            int ret = 0;

            if ((ret = OnSearch(Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationBottomData)) != 0)
            {
                return null;
            }

            //RevisionResult result = new RevisionResult();
            return m_PatternMatchingTool.Result;
        }

        protected List<Task<int>> OnAfterTakePicture()
        {
            List<Task<int>> ars = new List<Task<int>>();

            ////if (this.Configuration.Body.IsUseGoback == false) return ars;
            //Dictionary<string, MovingProjection> dicMovingProjection = m_Owner.RevisionZ.GetDefaultMovingProjections();
            //ars.Add(m_Owner.RevisionZ.BeginMove(dicMovingProjection));

            return ars;
        }
        #endregion

        #region Part Members
        public override int Create()
        {
            int ret = base.Create();


            return ret;
        }

        public override void Close()
        {
            base.Close();
        }

        public override void UpdateRecipeData()
        {

        }

        public override void UpdateConfigData()
        {

/*            ReelFeederAndMounter mounter = Owner as ReelFeederAndMounter;
            if (mounter != null)
            {
                if (Recipe.IlluminationMounterDataSet == null)
                    Recipe.IlluminationMounterDataSet = new IlluminationDataSet(Name + "_Mounter");

                if (Recipe.IlluminationBottomDataSet == null)
                    Recipe.IlluminationBottomDataSet = new IlluminationDataSet(Name + "_Bottom");

                Recipe.IlluminationBottomDataSet.SetIlluminationChannel(mounter.Config.ListBottomIlluminationChannel);
                Recipe.IlluminationMounterDataSet.SetIlluminationChannel(mounter.Config.ListMounterIlluminationChannel);
            }*/

        }

        public override int OnWork()
        {
            int ret = 0;
/*
            if (SearchResult == null)
                SearchResult = new RevisionResult();
            //Collet collet = null;

            List<Task<int>> ars = null;
            m_Owner = this.Owner as ReelFeederAndMounter;

            RevisionResult checkSearchResult = new RevisionResult();

            double dXSpec = Recipe.XSpec;
            double dYSpec = Recipe.YSpec;
            double dAngleSpec = Recipe.AngleTolerance;

            if (this.Recipe == null)
                throw new ArgumentNullException("Assigned Recipe");

            //if (m_Owner.GetCollet(Turret.PoistionKey.RevisionPart) == null)
            //    throw new ArgumentNullException("collet not exist");

            ////die = collet.Port.Location.GetMaterial() as Die;
            ////if (die.Presence != MaterialPresence.Exist || die.IsTrash == true) return ret;

            //// Revision Camera AutoFocus 위치로 이동
            //collet = m_Owner.GetCollet(Turret.PoistionKey.RevisionPart);
            //double dPosition = m_Owner.AutoFocuser.Results[collet.ArmIndex].BestFocusPosition;
            //if ((ret = m_Owner.RevisionZ.MovePosition(dPosition)) != 0) return ret;

            ////bool inposition = false;

            int dSleep = Recipe.Sleep;
            Thread.Sleep(dSleep);
            int nCutCount = Recipe.CutCount;

            int AvgCount = Recipe.AvgCount;
            RevisionResult[] searchResultArray = new RevisionResult[AvgCount];
            VisionImage[] imageArray = new VisionImage[AvgCount];
            Task[] taskImage = new Task[AvgCount];

            PointF[] ptCollet = new PointF[AvgCount];
            PointF[] ptChip = new PointF[AvgCount];
            object objLock = new object();
            ////시작

            //CycleTimer Revisiontimer = new CycleTimer();
            //Revisiontimer.Start();

            //CycleTimer timer = new CycleTimer();

            for (int iter = 0; iter < AvgCount; iter++)
            {
                //Log.Write("RevisionCycleTime", string.Format("Grab Interval : {0}", timer.Latest));

                taskImage[iter] = Task.Factory.StartNew((index) =>
                {
                    CycleTimer cycleTimer = new CycleTimer();
                    cycleTimer.Start();
                    //시작
                    int nIndex = (int)index;
                    RevisionResult result = new RevisionResult();
                    searchResultArray[nIndex] = new RevisionResult();
                    XytCoordinate coordinate = new XytCoordinate();
                    XyCoordinate convertValue = new XyCoordinate();
                    PointD reversePoint;
                    ptCollet[nIndex] = new PointF();
                    ptChip[nIndex] = new PointF();
                    lock (objLock)
                    {
                        PatternMatchingResult PatternMatchResult = new PatternMatchingResult();
                        PatternMatchResult = this.Search();
                        imageArray[nIndex] = this.m_PatternMatchingTool.InputImage;
                        if (PatternMatchResult != null && PatternMatchResult.Values.Count > 0)
                        {
                            reversePoint = new PointD(this.m_Owner.BottomCamera.Resolution.Width - PatternMatchResult.Values[0].X, this.m_Owner.BottomCamera.Resolution.Height - PatternMatchResult.Values[0].Y);
                            VisionScale.ConvertPosition<XyCoordinate>(this.m_Owner.BottomScale, this.m_Owner.BottomCamera.Resolution, reversePoint, out convertValue);
                            searchResultArray[nIndex].PixelPosition = new PointF((float)PatternMatchResult.Values[0].X, (float)PatternMatchResult.Values[0].Y);
                            searchResultArray[nIndex].Position = new XytCoordinate(convertValue.X, convertValue.Y, PatternMatchResult.Values[0].R);
                            searchResultArray[nIndex].Found = PatternMatchResult.Found;
                        }
                    }
                    bool bPatternMatching = Recipe.UsePatternMatching;
                    if (searchResultArray[nIndex].Found == true)
                    {
                        bool bFound = false;
                        if (Recipe.UseColletCenter)
                        {
                            m_qmcED[0].ThresholdIntensity = Recipe.Threshold;
                            m_qmcED[0].m_nChipThreshold = Recipe.ChipThres;
                            m_qmcED[0].m_nColletSize = Recipe.ColletSize;
                            m_qmcED[0].m_nAvgCount = Recipe.EdgeAvgCount;
                            m_qmcED[0].m_nFindColletToleranceX = (int)Recipe.ColletTolerenceX;
                            m_qmcED[0].m_bShowInspectImage = Recipe.ShowEnableInspectionImage;
                            m_qmcED[0].m_nHpfOffset = Recipe.HighPassFilterOffset;

                            ptChip[nIndex] = searchResultArray[nIndex].PixelPosition;

                            // 패턴매칭 사용시
                            if (bPatternMatching)
                            {
                                try
                                {
                                    // 칩의 중앙만 가져옴
                                    bFound = m_qmcED[0].GetColletCenterForPatternMatching(imageArray[nIndex].RawData, imageArray[nIndex].Header.Width, imageArray[nIndex].Header.Height, out ptCollet[nIndex], ptChip[nIndex]);
                                }
                                catch (Exception ex)
                                {
                                    bFound = false;
                                }
                            }
                            // 패턴매칭 미사용시
                            else
                            {
                                try
                                {
                                    // 콜렛과 칩의 중앙을 가져옴
                                    bFound = m_qmcED[0].GetGetColletCenter(imageArray[nIndex].RawData, imageArray[nIndex].Header.Width, imageArray[nIndex].Header.Height, out ptCollet[nIndex], out ptChip[nIndex]);
                                }
                                catch (Exception ex)
                                {
                                    bFound = false;
                                }
                            }


                        }
                        else
                        {
                            bFound = true;
                            ptCollet[nIndex].X = this.m_Owner.BottomCamera.Resolution.Width / 2;
                            ptCollet[nIndex].Y = this.m_Owner.BottomCamera.Resolution.Height / 2;
                            ptChip[nIndex] = searchResultArray[nIndex].PixelPosition;
                        }

                        // 칩을 찾았을 경우
                        if (bFound)
                        {
                            coordinate.X = (ptChip[nIndex].X - ptCollet[nIndex].X) * this.Scale.X;
                            coordinate.Y = (ptCollet[nIndex].Y - ptChip[nIndex].Y) * this.Scale.Y;
                            coordinate.T = searchResultArray[nIndex].Position.T;
                            searchResultArray[nIndex].Position = coordinate;
                        }
                        else
                        {
                            searchResultArray[nIndex].Found = false;
                        }

                        ReelFeederAndMounter mounter = Owner as ReelFeederAndMounter;
                        mounter.RevisionResults = searchResultArray[nIndex];
                    }

                    if (Recipe.SaveRevisionImage && nIndex == AvgCount - 1)
                    {
                        //string strPath = string.Format("C:\\Program Files\\QMC\\RevisionImage\\Image_{1}_{2}.bmp", collet.ArmIndex, DateTime.Now.ToString("yyyyMMddHHmmss"));
                        //imageArray[nIndex].Save(strPath, VisionImage.FileFilter.bmp);
                    }

                    //끝
                    //cycleTimer.End();
                    //Log.Write("RevisionCycleTime", string.Format("{0} : {1}", nIndex, cycleTimer.Latest));

                }, iter);


                //}

            }

            //// Revision Camera GoBack
            ars = this.OnAfterTakePicture();


            //// To Do : 추후 TACT을 감소하는 목적으로 비동기 호출이 가능하다.
            for (int iter = 0; iter < AvgCount; iter++)
            {
                if (taskImage[iter].IsCompleted == false)
                {
                    taskImage[iter].Wait();
                }
            }

            ////끝
            //Revisiontimer.End();
            //Log.Write("RevisionCycleTime", string.Format("Revision Interval : {0}", Revisiontimer.Latest));

            RevisionResult sum = new RevisionResult();
            int nCount = 0;
            for (int iter = 0; iter < AvgCount; iter++)
            {
                if (searchResultArray[iter].Found)
                {
                    sum.Position += searchResultArray[iter].Position;

                    nCount++;
                }
                SearchResult.Found = false;
                if (nCutCount < 1)
                {
                    nCutCount = 1;
                }
            }

            if (nCount >= nCutCount)
            {
                SearchResult.Position = sum.Position / nCount;
                if (Math.Abs(SearchResult.Position.X) <= dXSpec && Math.Abs(SearchResult.Position.Y) <= dYSpec && Math.Abs(SearchResult.Position.T) <= dAngleSpec)
                {
                    SearchResult.Found = true;
                    ret = 0;
                }
            }
            else
            {
                ret = -1;
            }
            //if (collet != null)
            //{
            //    if (searchResult.Found && collet.Die != null)
            //    {
            //        collet.LPFSourceX.AddValue(searchResult.Position.X - collet.LPFSourceX.CurrentValue);
            //        collet.LPFSourceY.AddValue(collet.LPFSourceY.CurrentValue - searchResult.Position.Y);
            //        collet.Die.CompensationOffsetOnRevision = searchResult.Position;
            //        collet.Die.IsFind = true;
            //    }
            //}

            ////?

            String strLog;
            strLog = string.Format("ArmIndex,X,Y,T, Count : , {0},{1},{2},{3}, {4}", SearchResult.Found.ToString(), SearchResult.Position.X.ToString()
                , SearchResult.Position.Y.ToString()
                , SearchResult.Position.T.ToString()
                , nCount);
*/
            return ret;
        }
        #endregion
    }

    #endregion
}
