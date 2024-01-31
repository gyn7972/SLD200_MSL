using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Optics;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QMC.Common.VisionPart
{
    public class RealTimeScanner : PatternMatchingVisionPart
    {
        public enum MotionKey
        {
            X,
            Y
        }
        public enum FunctionID
        {
            Train,
            Search,
        }

        //public XytStage Stage { set; get; }
        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }
        public UvwzxyzStage Stage { set; get; }
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
        public LoadingQueue LoadingQueue { set; get; }

        public RealTimeScannerConfig Config { set; get; }
        public DieLoaderRecipe.RecipeSubMaterial RecipeSubMaterial { set; get; }
        public RectangleD LastestArea { set; get; }
        public bool FirstSearchCompleted { set; get; }

        public RealTimeScannerRecipe Recipe { set; get; }
        public RealTimeScannerResult Result { set; get; }
        public RealTimeScanner(string strName) : base(strName)
        {
            LastestArea = new RectangleD();
            FirstSearchCompleted = false;
            Recipe = new RealTimeScannerRecipe(this);
            Config = new RealTimeScannerConfig();
            IlluminationData = new IlluminationDataSet(Name);
        }

        #region Part
        public override int Create()
        {
            int ret = base.Create();


            return ret;
        }

        public override void Stop()
        {
            base.Stop();
        }
        #endregion

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

        public int Scan(RealTimeScanParameter parameter, out RealTimeScannerResult result)
        {
            int ret = 0;
            result = null;
            
            if ((ret = OnScan(parameter, out result)) != 0) return ret;

            return ret;
        }

        public Task<int> BeginScan(RealTimeScanParameter parameter)
        {
            RealTimeScannerResult result = null;
            return Task.Factory.StartNew(() =>
            {
                int ret = 0;
                if((ret = Scan(parameter, out result))!= 0) return ret;

                Result = result;

                return ret;
            });
        }

        protected int OnScan(RealTimeScanParameter parameter, out RealTimeScannerResult result)
        {
            int ret = 0;
            bool bAngleChange = false;
            RectangleD area = new RectangleD();
            PatternMatchingResult searchResult = null;
            DieLoader dieLoader = this.Owner as DieLoader;
            result = new RealTimeScannerResult();

            if(LoadingQueue.Count() == 0 && FirstSearchCompleted == true)
            {
                area = this.LastestArea;
            }
            else
            {
                area = GetCurrentRowArea(parameter, true);
                this.LastestArea = area;
                this.FirstSearchCompleted = true;
            }

            if((ret = SearchDies(parameter, area, true, out bAngleChange, out searchResult))!= 0) return ret;

            for (int i = 0; i < searchResult.Values.Count; i++)
            {
                XyCoordinate position;
                if ((ret = VisionScale.ConvertPosition<XyCoordinate, XyCoordinate>(dieLoader.Scale, this.Camera.Resolution, (XyCoordinate)parameter.CurrentPosition, searchResult.Values[i], out position)) != 0) return ret;
                result.SearchPositions.Add(new XytCoordinate(position.X, position.Y, 0));
                result.ResultOverlays = searchResult.ResultOverlays;
            }
            return ret;
        }

        protected int SearchDies(RealTimeScanParameter parameter, RectangleD area, bool checkAngle, out bool isAngleChange, out PatternMatchingResult result)
        {
            int ret = 0;
            double average = 0.0;
            bool isFindAngle = false;
            XyCoordinate position = new XyCoordinate();
            //PatternMatchingResult result = null;
            XytCoordinate currentPosition = new XytCoordinate();
            XytCoordinateCollection rotatePositions = new XytCoordinateCollection();
            DieLoader dieLoader = this.Owner as DieLoader;
            result = null;

            isAngleChange = false;
            if ((ret = this.Stage.GetCommandPosition(ref currentPosition)) != 0) return ret;

            if (area.Width < 0 || area.Height < 0)
            {
                //Error
                return -1;
            }

            if ((ret = OnSearch(Recipe.InspectRoiStartLocation, Recipe.InspectRoiEndLocation, Recipe.PatternMatchingParameter, IlluminationData)) != 0)
            {
                result = null;
                return ret;
            }
            result = m_PatternMatchingTool.Result;

            return ret;
        }

        private int SetRectangleArea(double x, double y)
        {
            int ret = 0;
            DieLoadSubstrate loadSubstrate = null;
            RectangleD originRectangle = new RectangleD();
            RectangleD loadSubstrateRectangle = new RectangleD();

            loadSubstrate = this.GetSubstrateInfomation();
            originRectangle = this.ConvertToRectangleD(this.Recipe.WorkingArea);

            if (loadSubstrate == null)
                throw new ArgumentNullException("LoadSubstrate");

            if (loadSubstrate != null && loadSubstrate.SubstrateSpecification.rectangleWorkingAreaSetCompleted != true)
            {
                loadSubstrate.SubstrateSpecification.rectangleWorkingArea = originRectangle;
                loadSubstrate.SubstrateSpecification.rectangleWorkingAreaSetCompleted = true;

                Log.Write(this.Name, string.Format($"OriginRectangle : {originRectangle} , RectangleWorkingAreaSetCompleted : {loadSubstrate.SubstrateSpecification.rectangleWorkingAreaSetCompleted}"));
            }

            if (loadSubstrate != null && loadSubstrate.SubstrateSpecification.rectangleWorkingAreaSetCompleted != false)
            {
                loadSubstrateRectangle = loadSubstrate.SubstrateSpecification.rectangleWorkingArea;

                if (x < loadSubstrateRectangle.X)
                {
                    loadSubstrateRectangle.Width += loadSubstrateRectangle.X - x;
                    loadSubstrateRectangle.X = x;
                }

                if (y < loadSubstrateRectangle.Y)
                {
                    loadSubstrateRectangle.Height += loadSubstrateRectangle.Y - y;
                    loadSubstrateRectangle.Y = y;
                }

                if (loadSubstrateRectangle.X + loadSubstrateRectangle.Width < x)
                    loadSubstrateRectangle.Width = Math.Abs(x - loadSubstrateRectangle.X);

                if (loadSubstrateRectangle.Y + loadSubstrateRectangle.Height < y)
                    loadSubstrateRectangle.Height = Math.Abs(y - loadSubstrateRectangle.Y);

                loadSubstrate.SubstrateSpecification.rectangleWorkingArea = loadSubstrateRectangle;
            }

            return ret;
        }
        private int SetRoi(RealTimeScanParameter parameter, RectangleD area)
        {
            int ret = 0;
            RoiVisionToolParameter roiParameter = null;

            roiParameter = m_RoiInspect.Parameter;

            if (roiParameter != null)
            {
                if (roiParameter.Overlay != null)
                    roiParameter.Overlay.Visible = true;

                roiParameter.StartLocation = new Point((int)area.X, (int)area.Y);
                roiParameter.Size = new Size((int)area.Width, (int)area.Height);
            }
            else
            {
                //ErrorManager.Register("VisionTool not found.");
                return -1;
            }
                

            return ret;
        }
        private RectangleD GetCurrentRowArea(RealTimeScanParameter parameter, bool bIsReference)
        {
            PointD startPoint = new PointD();
            RectangleD area = new RectangleD();

            if (bIsReference == true)
            {
                startPoint = new PointD(parameter.Reference.X - (parameter.Pitch.Width * 2), parameter.Reference.Y - parameter.Pitch.Height * 4);
                area = new RectangleD(startPoint, parameter.Pitch.Width * 4, parameter.Pitch.Height * 9);

            }
            else
            {
                if (this.Recipe.PriorityDirection == RealTimeScannerRecipe.Direction.ToTop)
                {
                    startPoint = new PointD(parameter.Pitch.Width / 2, 0);
                    area = new RectangleD(startPoint, parameter.Pitch.Width, (this.Camera.Resolution.Height / 2) - (parameter.Pitch.Height / 2));
                }
                else if (this.Recipe.PriorityDirection == RealTimeScannerRecipe.Direction.ToBottom)
                {
                    startPoint = new PointD(parameter.Pitch.Width / 2, (this.Camera.Resolution.Height / 2) + (parameter.Pitch.Height / 2));
                    area = new RectangleD(startPoint, parameter.Pitch.Width, startPoint.Y - parameter.Pitch.Height);
                }
                else if (this.Recipe.PriorityDirection == RealTimeScannerRecipe.Direction.ToLeft)
                {
                    startPoint = new PointD(0, parameter.Pitch.Height / 2);
                    area = new RectangleD(startPoint, (this.Camera.Resolution.Width / 2) - (parameter.Pitch.Width / 2), parameter.Pitch.Height);
                }
                else if (this.Recipe.PriorityDirection == RealTimeScannerRecipe.Direction.ToRight)
                {
                    startPoint = new PointD((this.Camera.Resolution.Width / 2) + (parameter.Pitch.Width / 2), parameter.Pitch.Height / 2);
                    area = new RectangleD(startPoint, startPoint.X - parameter.Pitch.Width, parameter.Pitch.Height);
                }
            }

            return area;
        }

 		public override void UpdateConfigData() //참고 : 오버라이드,, 파트 콜
        {
            DieLoader dieLoader = Owner as DieLoader;
            if(dieLoader != null)
            {
                Config = dieLoader.Config.RealTimeScannerConfig;

                if (Recipe.IlluminationDataSet == null)
                    Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                Recipe.IlluminationDataSet.SetIlluminationChannel(dieLoader.Config.ListIlluminationChannel);
            }
        }

        public override void UpdateRecipeData()
        {
            DieLoader dieLoader = Owner as DieLoader;
            if (dieLoader != null)
            {
                this.Recipe = dieLoader.Recipe.ScannerRecipe;
                if(this.Recipe.IlluminationDataSet != null)
                {
                    IlluminationData = this.Recipe.IlluminationDataSet;
                }
                else
                {
                    this.Recipe.IlluminationDataSet = new IlluminationDataSet(Name);
                }
            }
        }
        private bool CheckAreaPer(RealTimeScanParameter parameter)
        {
            int ret = 0;
            RectangleD rectangle = this.GetRectangleArea(true);
            XyCoordinate currentPosition = new XyCoordinate();


            currentPosition = (XyCoordinate)parameter.CurrentPosition;

            if (this.Recipe.SecondarySearchDirection == RealTimeScannerRecipe.Direction.ToRight)
            {
                if (rectangle.X < currentPosition.X)
                {
                    return (rectangle.Y < currentPosition.Y && currentPosition.Y < rectangle.Y + rectangle.Height);
                }
                else
                {
                    return (rectangle.Y < currentPosition.Y && currentPosition.Y < rectangle.Y + rectangle.Height);
                }
            }
            else if (this.Recipe.SecondarySearchDirection == RealTimeScannerRecipe.Direction.ToLeft)
            {
                if (currentPosition.X < rectangle.X + rectangle.Width)
                {
                    return (rectangle.Y < currentPosition.Y && currentPosition.Y < rectangle.Y + rectangle.Height);
                }
                else
                {
                    return (rectangle.Y < currentPosition.Y && currentPosition.Y < rectangle.Y + rectangle.Height);
                }
            }

            return true;
        }
        private RectangleD GetRectangleArea(bool isAreaPer)
        {
            DieLoadSubstrate loadSubstrate = null;
            RectangleD originRectangle = new RectangleD();
            RectangleD rectangle = new RectangleD();

            loadSubstrate = this.GetSubstrateInfomation();
            originRectangle = this.ConvertToRectangleD(this.Recipe.WorkingArea);

            if (loadSubstrate != null && loadSubstrate.SubstrateSpecification.rectangleWorkingAreaSetCompleted != false && this.Config.CheckArea == true)
            {
                rectangle = loadSubstrate.SubstrateSpecification.rectangleWorkingArea;
                Log.Write("RealTimeScanTest", string.Format($"rectangleWorkingArea : {loadSubstrate.SubstrateSpecification.rectangleWorkingArea}"));
            }
            else
            {
                rectangle = originRectangle;
                Log.Write("RealTimeScanTest", string.Format($"originRectangle : {originRectangle}"));
            }

            if (isAreaPer == true)
            {
                rectangle.X = rectangle.X * (this.Config.AreaPercent / 100);
                rectangle.Y = rectangle.Y * (this.Config.AreaPercent / 100);
                rectangle.Width = rectangle.Width * (this.Config.AreaPercent / 100);
                rectangle.Height = rectangle.Height * (this.Config.AreaPercent / 100);
            }

            return rectangle;
        }
        private RectangleD ConvertToRectangleD(SizeD size)
        {
            PointD point;
            RectangleD rectagle = new RectangleD();

            point = ((PointD)size) * -0.5;
            point.X -= this.Config.AreaMargin.Width;
            point.Y -= this.Config.AreaMargin.Height;
            size.Width += this.Config.AreaMargin.Width * 2;
            size.Height += this.Config.AreaMargin.Height * 2;
            rectagle = new RectangleD(point, size.Width, size.Height);

            return rectagle;
        }
        private DieLoadSubstrate GetSubstrateInfomation()
        {
            DieLoader dieLoader = this.Owner as DieLoader;
            DieLoadSubstrate loadSubstrate = null;
            if(dieLoader != null)
            {
                loadSubstrate = dieLoader.GetMaterial() as DieLoadSubstrate;
            }
            

            return loadSubstrate;
        }
    }
}
