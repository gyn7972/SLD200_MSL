using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Optics;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class ColletXYPositionCalibrator : VisionPart
    {

        #region Define
        [Serializable]
        public enum BlobMeasureConstants
        {
            CenterMassX,
            CenterMassY,
            Acircularity
        }

        [Serializable]
        public enum RelatedMotionPositionKeys
        {
            BackLight,
        }
        [Serializable]
        public enum FunctionID
        {
            Train,
            Search,
        }
        #endregion
        protected VisionProRoiVisionTool m_RoiInspect;
        protected VisionProBlobVisionTool m_BlobTool;
        private Dictionary<string, XyCoordinate> m_XYCalOffsetData;

        public List<Collet> Collets { get; set; }

        protected VisionScale m_Scale;
        private Collet m_Collet;

        public MotionPart Motion { set; get; }
        public VisionImage TestImage { set; get; }
        //public XyztStage Stage { set; get; }
        //public XyzztStage Stage { set; get; }
        public UvwzxyzStage Stage { set; get; }
        public ColletFindCenterResult CenterResult { set; get; }
        public ColletXYPositionCalibratorRecipe XyCalibratorRecipe { get; set; }

        public ColletXYPositionCalibratorConfig XYCalibratoeConfig { set; get; }
        public ColletXYPositionCalibratorRecipe Recipe { set; get; }

        public PathGenerator PathGenerator { set; get; }

        public IlluminationDataSet IlluminationData
        {
            set; get;
        }

        public bool Simulated { set; get; }

        public ColletXYPositionCalibrator(string strName) : base(strName)
        {
            m_RoiInspect = new VisionProRoiVisionTool();
            TestImage = new VisionImage();
            XyCalibratorRecipe = new ColletXYPositionCalibratorRecipe(this);
            IlluminationData = new IlluminationDataSet(strName);
            m_BlobTool = new VisionProBlobVisionTool();
            PathGenerator = new PathGenerator();
            CenterResult = new ColletFindCenterResult(new XyCoordinate(), null, 0, new PointD());
            m_XYCalOffsetData = new Dictionary<string, XyCoordinate>();
        }

        public override int Create()
        {
            int ret = base.Create();

            if (m_RoiInspect == null)
                m_RoiInspect = new VisionProRoiVisionTool();
            if (m_BlobTool == null)
                m_BlobTool = new VisionProBlobVisionTool();
            if (XyCalibratorRecipe == null)
            {
                XyCalibratorRecipe = new ColletXYPositionCalibratorRecipe(this);
            }
            return ret;
        }

        public void SetXYCalOIffsetData(string strCollet, XyCoordinate offset)
        {
            m_XYCalOffsetData.Add(strCollet, offset);
        }

        private int FindColletCenter(out ColletFindCenterResult result)
        {
            int ret = 0;
            double acircularity = 0.0;
            BlobResult blobResult = null;
            BlobVisionTool tool = null;
            PathParameter coarsePathGeneratorParameter = null;
            PathParameter finePathGeneratorParameter = null;
            List<ColletFindCenterResult> results = new List<ColletFindCenterResult>();
            PointD centerPoint = new PointD();
            PointD measurePoint = new PointD();
            XyCoordinate positions = new XyCoordinate();
            VisionImage image = null;

            if (this.Motion != null)
            {
                if (this.Owner is DieLoader)
                {
                    DieLoader module = this.Owner as DieLoader;
                    this.XYCalibratoeConfig.StartPosition = module.Recipe.StartPosition;
                    this.Stage = module.Stage;
                    this.m_Scale = module.Scale;

                }
                else if (this.Owner is DieUnloader)
                {
                    DieUnloader module = this.Owner as DieUnloader;
                    this.XYCalibratoeConfig.StartPosition = module.Recipe.StartPosition;
                    this.Stage = module.Stage;
                    this.m_Scale = module.Scale;
                }
            }
            result = null;
            //rough PathGenerator
            coarsePathGeneratorParameter = CopyUtility.GetDeepCopy<PathParameter>(this.XyCalibratorRecipe.PathParameter);
            coarsePathGeneratorParameter.CenterCoordinate = (XyCoordinate)this.XyCalibratorRecipe.CenterCoordinate;

            if ((ret = this.PathGenerator.Generate(coarsePathGeneratorParameter)) != 0) return ret;

            //사용자가 설정한 Path대로 돌면서 Blob을 진행하여 원형도(가장 원일때 1)가 1에 가까운 값을 찾아준다.
            for (int i = 0; i < this.PathGenerator.Paths.Count; i++)
            {//?
                if ((ret = this.Stage.MovePosition(this.PathGenerator.Paths[i])) != 0) return ret;
                if ((ret = OnSetIllumination(IlluminationData, true)) != 0) return ret;
                if ((ret = this.Camera.GrabSync(out image)) != 0) return ret;
                if ((ret = this.m_BlobTool.Run()) != 0) return ret;
                blobResult = m_BlobTool.Result;
                if (blobResult == null || blobResult.PixelValues.Count == 0 || blobResult.PixelValues.Count > 1) continue;
                centerPoint = new PointD(blobResult.PixelValues[0][BlobMeasureConstants.CenterMassX.ToString()].Value, blobResult.PixelValues[0][BlobMeasureConstants.CenterMassY.ToString()].Value);
                acircularity = blobResult.PixelValues[0][BlobMeasureConstants.Acircularity.ToString()].Value;
                result = new ColletFindCenterResult(this.PathGenerator.Paths[i], blobResult, acircularity, centerPoint);
                results.Add(result);

                //if (this.Recipe.UseSaveImageLogAllImage == true)
                //{
                //    if ((ret = this.SaveImageLog(image, "All Image", result.Acircularity)) != 0) return ret;
                //}
            }

            if (results.Count == 0) return ret;

            results = results.OrderBy(i => i.Acircularity).ToList();
            result = results[0];

            measurePoint = new PointD(this.Camera.Resolution.Width - result.CenterPoint.X, this.Camera.Resolution.Height - result.CenterPoint.Y);
            if ((ret = VisionScale.ConvertPosition<XyCoordinate>(this.m_Scale, this.Camera.Resolution, measurePoint, out positions)) != 0) return ret;
            //?스케일 확인
            //이포지션을 어디다 넣어줘야하는지?? 
            //?????????????????????????????????????????????????????????????????

            //result 결과 위치로 이동하여 한번 더 PathGenerator를 돌며 Search
            if (this.XyCalibratorRecipe.UseFinePathGenerator == true)
            {
                #region Fine PathGenerator
                finePathGeneratorParameter = CopyUtility.GetDeepCopy<PathParameter>(this.XyCalibratorRecipe.PathParameter);
                finePathGeneratorParameter.CenterCoordinate = result.ResultPosition;

                if ((ret = this.PathGenerator.Generate(finePathGeneratorParameter)) != 0) return ret;

                for (int i = 0; i < this.PathGenerator.Paths.Count; i++)
                {
                    if ((ret = this.Stage.MovePosition(this.PathGenerator.Paths[i])) != 0) return ret;
                    if ((ret = OnSetIllumination(IlluminationData, true)) != 0) return ret;
                    if ((ret = this.Camera.GrabSync(out image)) != 0) return ret;
                    if ((ret = this.m_BlobTool.Run()) != 0) return ret;
                    blobResult = m_BlobTool.Result;
                    if (blobResult == null || blobResult.PixelValues.Count == 0 || blobResult.PixelValues.Count > 1) continue;
                    centerPoint = new PointD(blobResult.PixelValues[0][BlobMeasureConstants.CenterMassX.ToString()].Value, blobResult.PixelValues[0][BlobMeasureConstants.CenterMassY.ToString()].Value);
                    acircularity = blobResult.PixelValues[0][BlobMeasureConstants.Acircularity.ToString()].Value;
                    result = new ColletFindCenterResult(this.PathGenerator.Paths[i], blobResult, acircularity, centerPoint);
                    results.Add(result);

                    //if (this.AssignedSubRecipeItem.UseSaveImageLogAllImage == true)
                    //{
                    //    if ((ret = this.SaveImageLog(image, "All Image", result.Acircularity)) != 0) return ret;
                    //}
                }

                if (results.Count == 0) return ret;

                results = results.OrderBy(i => i.Acircularity).ToList();
                result = results[0];

                measurePoint = new PointD(this.Camera.Resolution.Width - result.CenterPoint.X, this.Camera.Resolution.Height - result.CenterPoint.Y);
                if ((ret = VisionScale.ConvertPosition<XyCoordinate>(this.m_Scale, this.Camera.Resolution, measurePoint, out positions)) != 0) return ret;

                //this.WriteLog(LogLevel.Normal, string.Format("Second Result Position : [{0},{1}], BackLight Position : [{2},{3}], Acircularity : [{2}]", positions.X, positions.Y, result.ResultPosition.X, result.ResultPosition.Y, result.Acircularity));

                //if (this.AssignedSubRecipeItem.UseSaveImageLogResultImage == true)
                //{
                //    if ((ret = this.SaveImageLog(image, "Result Image", result.Acircularity)) != 0) return ret;
                //}
                #endregion
            }
            //????????????????????????????????????????? ResultPosition??
            result.ResultPosition = positions;

            if ((ret = this.Stage.MovePosition(result.ResultPosition)) != 0) return ret;

            if ((ret = OnSetIllumination(IlluminationData, true)) != 0) return ret;
            if ((ret = this.Camera.GrabSync(out image)) != 0) return ret;
            if ((ret = this.m_BlobTool.Run()) != 0) return ret;
            blobResult = m_BlobTool.Result;
            return ret;
        }
        public int OnCalibrate(Module module, ref XyCoordinate positions)
        {
            int ret = 0;
            BlobResult blobResult = null;
            //BlobProviderParameter parameter = this.AssignedSubRecipeItem.BlobParameter;
            PointD centerPoint = new PointD();
            PointD measurePoint = new PointD();
            ColletFindCenterResult result = null;

            if (module != null)
            {
                if (module is DieLoader)
                {
                    DieLoader DieLoadermodule = module as DieLoader;
                    m_Scale = DieLoadermodule.Scale;
                }
                else if (module is DieUnloader)
                {
                    DieUnloader DieUnLoadermodule = module as DieUnloader;
                    m_Scale = DieUnLoadermodule.Scale;
                }
                else if (module is DieTransfer)
                {
                    DieTransfer DieTransfermodule = module as DieTransfer;
                    m_Scale = new VisionScale(1, 1);
                }
            }

            //계속 남아있는지 확인 테스트 필요

            //if ((ret = this.BlobAgent.SetParameter(this.JobName, parameter)) != 0) return ret;
            m_BlobTool.Parameter.Polarity = this.XyCalibratorRecipe.Porarity;
            m_BlobTool.Parameter.MinPixels = (int)(Math.Pow(this.XyCalibratorRecipe.ColletDiameter / (this.m_Scale.X * 1000) / 2, 2) * Math.PI); ;
            m_BlobTool.Parameter.HardThreshold = this.XyCalibratorRecipe.HardThreshold;
            m_BlobTool.Parameter.ResultOverlayVisible = true;
            #region 수정전
            //if (this.ReleatedMotion == null)
            //{
            //    if ((ret = this.BlobAgent.BlobSync(this.JobName, out blobResult)) != 0) return ret;
            //}
            //else
            //{
            //    if ((ret = this.FindColletCenter(out result)) != 0) return ret;

            //    if (result == null)
            //        blobResult = null;
            //    else
            //        blobResult = result.BlobResult;
            //}

            //if (blobResult == null || blobResult.PixelValues.Count < 1)
            //{
            //    //Log.Write("XYCalibrate", blobResult.PixelValues.Count.ToString());
            //    Log.Write("XYCalibrate", string.Format("BlobResult is null"));
            //    if ((ret = this.Alarms[AlarmKeys.BlobFailed].Post(this)) != 0) return ret;
            //}
            #endregion
            VisionImage image;

            if (Simulated)
            {
                image = TestImage;
            }
            else
            {
                if ((ret = OnSetIllumination(IlluminationData, true)) != 0) return ret;
                if ((ret = Camera.GrabSync(Purpose.Processing, out image)) != 0) return ret;
            }

            m_RoiInspect.Parameter.StartLocation = this.XyCalibratorRecipe.InspectRoiStartLocation;
            m_RoiInspect.Parameter.EndLocation = this.XyCalibratorRecipe.InspectRoiEndLocation;
            m_RoiInspect.InputImage = image;
            if ((ret = m_RoiInspect.Run()) != 0) return ret;
            m_BlobTool.InputImage = m_RoiInspect.OutputImage;
            #region 수정 후 - AutoSearch 분리
            if ((ret = this.m_BlobTool.Run()) != 0) return ret;
            blobResult = m_BlobTool.Result;
            if (blobResult == null || blobResult.PixelValues.Count < 1)
            {
                Log.Write(Name, string.Format("BlobResult is null"));
                //this.Alarms[AlarmKeys.BlobFailed].Cause = "ColletXy Center Point Search Failed.";
                //this.Alarms[AlarmKeys.BlobFailed].Remedy = "1. Please Change the Illuminator Light Value.\r\n or \r\n2. Move 'Motion' so that 'BackLight' is in the center of 'Collet'.";
                //if ((ret = this.Alarms[AlarmKeys.BlobFailed].Post(this)) != 0) return ret;
            }
            #endregion

            positions.X = positions.X + m_RoiInspect.Parameter.StartLocation.X;
            positions.Y = positions.Y + m_RoiInspect.Parameter.StartLocation.Y;

            Log.Write(Name, string.Format("Count : {0}, Pixel : {1}", blobResult.PixelValues.Count.ToString(), blobResult.PixelValues[0]));
            centerPoint = new PointD(blobResult.PixelValues[0][BlobMeasureConstants.CenterMassX.ToString()].Value, blobResult.PixelValues[0][BlobMeasureConstants.CenterMassY.ToString()].Value);
            measurePoint = new PointD(this.Camera.Resolution.Width - centerPoint.X, this.Camera.Resolution.Height - centerPoint.Y);
            if ((ret = VisionScale.ConvertPosition<XyCoordinate>(this.m_Scale, this.Camera.Resolution, measurePoint, out positions)) != 0) return ret;
            //result = m_BlobTool;
            //  this.WriteLog(LogLevel.Normal, string.Format("OnCalibrate() : position X = {0}, position Y = {1}", positions.X, positions.Y));
            CenterResult.BlobResult = blobResult;
            CenterResult.CenterPoint = centerPoint;
            CenterResult.ResultPosition = positions;
            CenterResult.MeasurePoint = measurePoint;

            XYCalibratoeConfig.CalibrationValue = new XyCoordinate(centerPoint.X, centerPoint.Y);

            if (module != null)
            {
                if (module is DieLoader)
                {
                    DieLoader DieLoadermodule = module as DieLoader;
                    int nindex = DieLoadermodule.DieTransfer.GetColletIndex(Turret.PoistionKey.LoadZ) - 1;
                    if (nindex <= DieLoadermodule.DieTransfer.Collets.Count)
                        DieLoadermodule.Config.ColletXYOffset[nindex] = new XyCoordinate(positions.X, positions.Y);

                }
                else if (module is DieUnloader)
                {
                    DieUnloader DieUnLoadermodule = module as DieUnloader;
                    int nindex = DieUnLoadermodule.DieTransfer.GetColletIndex(Turret.PoistionKey.UnloadZ) - 1;
                    if (nindex <= DieUnLoadermodule.DieTransfer.Collets.Count)
                        DieUnLoadermodule.Config.ColletXYOffset[nindex] = new XyCoordinate(positions.X, positions.Y);
                }
                else if (module is DieTransfer)
                {
                    DieTransfer DieTransfermodule = module as DieTransfer;
                    int nindex = DieTransfermodule.GetColletIndex(Turret.PoistionKey.RevisionPart) - 1;
                    if (nindex <= DieTransfermodule.Collets.Count)
                        DieTransfermodule.Config.ColletXYOffset[nindex] = new XyCoordinate(positions.X, positions.Y);
                }
            }

            return ret;
        }
        public int OnAutoCalibrate(Collet collet, ref XyCoordinate positions)
        {
            int ret = 0;

            BlobResult blobResult = null;
            BlobVisionToolParameter parameter = new BlobVisionToolParameter();
            PointD centerPoint = new PointD();
            PointD measurePoint = new PointD();
            ColletFindCenterResult result = null;

            parameter.MinPixels = (int)(Math.Pow(this.XyCalibratorRecipe.ColletDiameter / (this.m_Scale.X * 1000) / 2, 2) * Math.PI);
            parameter.HardThreshold = this.XyCalibratorRecipe.HardThreshold;
            //계속 남아있는지 확인 테스트 필요
            parameter.ResultOverlayVisible = true;

            m_BlobTool.Parameter.MinPixels = (int)(Math.Pow(this.XyCalibratorRecipe.ColletDiameter / (this.m_Scale.X * 1000) / 2, 2) * Math.PI);
            m_BlobTool.Parameter.HardThreshold = this.XyCalibratorRecipe.HardThreshold;
            m_BlobTool.Parameter.ResultOverlayVisible = true;

            if (this.Motion == null) return ret;

            if ((ret = this.FindColletCenter(out result)) != 0) return ret;

            if (result == null)
                blobResult = null;
            else
                blobResult = result.BlobResult;

            if (blobResult == null || blobResult.PixelValues.Count < 1)
            {
                Log.Write(Name, string.Format("BlobResult is null"));
                //this.Alarms[AlarmKeys.BlobFailed].Cause = "ColletXy Center Point Search Failed.";
                //this.Alarms[AlarmKeys.BlobFailed].Remedy = "1. Please Change the Illuminator Light Value.\r\n or \r\n2. Move 'Motion' so that 'BackLight' is in the center of 'Collet'.";
                //if ((ret = this.Alarms[AlarmKeys.BlobFailed].Post(this)) != 0) return ret;
            }


            Log.Write(Name, string.Format("Count : {0}, Pixel : {1}", blobResult.PixelValues.Count.ToString(), blobResult.PixelValues[0]));
            centerPoint = new PointD(blobResult.PixelValues[0][BlobMeasureConstants.CenterMassX.ToString()].Value, blobResult.PixelValues[0][BlobMeasureConstants.CenterMassY.ToString()].Value);
            measurePoint = new PointD(this.Camera.Resolution.Width - centerPoint.X, this.Camera.Resolution.Height - centerPoint.Y);
            if ((ret = VisionScale.ConvertPosition<XyCoordinate>(this.m_Scale, this.Camera.Resolution, measurePoint, out positions)) != 0) return ret;


            //  this.WriteLog(LogLevel.Normal, string.Format("OnCalibrate() : position X = {0}, position Y = {1}", positions.X, positions.Y));

            return ret;
        }


        public override void Close()
        {
            base.Close();
        }


        public RoiVisionTool GetInspectRoi()
        {
            return m_RoiInspect;
        }


        public override void UpdateConfigData() //참고 : 오버라이드,, 파트 콜
        {
            if (Owner is DieUnloader)
            {
                DieUnloader dieUnloader = Owner as DieUnloader;
                if (dieUnloader != null)
                {
                    if (XyCalibratorRecipe.IlluminationDataSet == null)
                        XyCalibratorRecipe.IlluminationDataSet = new IlluminationDataSet(Name);
                    XyCalibratorRecipe.IlluminationDataSet.SetIlluminationChannel(dieUnloader.Config.ListIlluminationChannel);

                }

            }
            else if (Owner is DieLoader)
            {
                DieLoader dieloader = Owner as DieLoader;
                if (dieloader != null)
                {
                    if (XyCalibratorRecipe == null)
                        XyCalibratorRecipe = new ColletXYPositionCalibratorRecipe(this);

                    if (XyCalibratorRecipe.IlluminationDataSet == null)
                        XyCalibratorRecipe.IlluminationDataSet = new IlluminationDataSet(Name);
                    XyCalibratorRecipe.IlluminationDataSet.SetIlluminationChannel(dieloader.Config.ListIlluminationChannel);
                    XYCalibratoeConfig = dieloader.Config.XYCalibratorConfig;
                }
            }
            else if (Owner is DieTransfer)
            {
                DieTransfer dieTransfer = Owner as DieTransfer;
                if (dieTransfer != null)
                {
                    //?
                    if (XyCalibratorRecipe.IlluminationDataSet == null)
                        XyCalibratorRecipe.IlluminationDataSet = new IlluminationDataSet(Name);
                    XyCalibratorRecipe.IlluminationDataSet.SetIlluminationChannel(dieTransfer.Config.ListIlluminationChannel);
                    XYCalibratoeConfig = dieTransfer.Config.XYCalibratorConfig;
                }
            }
        }
        public override void UpdateRecipeData()
        {
            if (Owner is DieUnloader)
            {
                DieUnloader dieUnloader = Owner as DieUnloader;
                if (dieUnloader != null)
                {
                    if (dieUnloader.Recipe.ColletXYPositionCalibratorRecipe != null)
                    {
                        this.XyCalibratorRecipe = dieUnloader.Recipe.ColletXYPositionCalibratorRecipe;
                        if (this.XyCalibratorRecipe.IlluminationDataSet != null)
                        {
                            this.XyCalibratorRecipe = dieUnloader.Recipe.ColletXYPositionCalibratorRecipe;
                            IlluminationData = this.XyCalibratorRecipe.IlluminationDataSet;
                        }
                        else
                        {
                            dieUnloader.Recipe.ColletXYPositionCalibratorRecipe = this.XyCalibratorRecipe;
                            this.XyCalibratorRecipe.IlluminationDataSet = new IlluminationDataSet(Name);
                        }
                    }
                }
            }
            else if (Owner is DieLoader)
            {
                DieLoader dieLoader = Owner as DieLoader;
                if (dieLoader != null)
                {
                    if (dieLoader.Recipe.ColletXYPositionCalibratorRecipe != null)
                    {
                        this.XyCalibratorRecipe = dieLoader.Recipe.ColletXYPositionCalibratorRecipe;
                        if (this.XyCalibratorRecipe.IlluminationDataSet != null)
                        {
                            this.XyCalibratorRecipe = dieLoader.Recipe.ColletXYPositionCalibratorRecipe;
                            IlluminationData = this.XyCalibratorRecipe.IlluminationDataSet;
                        }
                        else
                        {
                            dieLoader.Recipe.ColletXYPositionCalibratorRecipe = this.XyCalibratorRecipe;
                            this.XyCalibratorRecipe.IlluminationDataSet = new IlluminationDataSet(Name);
                        }
                    }
                }
            }
            else if (Owner is DieTransfer)
            {
                DieTransfer dieTransfer = Owner as DieTransfer;
                if (dieTransfer != null)
                {
                    if (dieTransfer.Recipe.XYCalibratorRecipe != null)
                    {
                        this.XyCalibratorRecipe = dieTransfer.Recipe.XYCalibratorRecipe;
                        if (this.XyCalibratorRecipe.IlluminationDataSet != null)
                        {
                            this.XyCalibratorRecipe = dieTransfer.Recipe.XYCalibratorRecipe;
                            IlluminationData = this.XyCalibratorRecipe.IlluminationDataSet;
                        }
                        else
                        {
                            dieTransfer.Recipe.XYCalibratorRecipe = this.XyCalibratorRecipe;
                            this.XyCalibratorRecipe.IlluminationDataSet = new IlluminationDataSet(Name);
                        }
                    }
                }
            }
        }
    }
    #region ColletFindCenterResult

    [Serializable]
    public class ColletFindCenterResult
    {
        #region Field
        private XyCoordinate m_ResultPosition;
        private double m_Acircularity;
        private BlobResult m_BlobResult;
        private PointD m_CenterPoint;
        private PointD m_MeasurePoint;
        #endregion

        #region Constructor
        public ColletFindCenterResult(XyCoordinate resultPosition, BlobResult blobResult, double acircularity, PointD centerPoint)
        {
            this.ResultPosition = resultPosition;
            this.Acircularity = acircularity;
            this.BlobResult = blobResult;
            this.CenterPoint = centerPoint;
        }
        #endregion

        #region Property

        public XyCoordinate ResultPosition
        {
            get { return this.m_ResultPosition; }
            set { this.m_ResultPosition = value; }
        }

        public double Acircularity
        {
            get { return this.m_Acircularity; }
            set { this.m_Acircularity = value; }
        }

        public BlobResult BlobResult
        {
            get { return this.m_BlobResult; }
            set { this.m_BlobResult = value; }
        }

        public PointD CenterPoint
        {
            get { return this.m_CenterPoint; }
            set { this.m_CenterPoint = value; }
        }

        public PointD MeasurePoint
        {
            get { return this.m_MeasurePoint; }
            set { this.m_MeasurePoint = value; }
        }
        #endregion
    }

    [Serializable]
    public class ColletFindCenterResultCollection : Collection<ColletFindCenterResult>
    {
        #region Constructor
        public ColletFindCenterResultCollection() : base() { }

        public ColletFindCenterResultCollection(IList<ColletFindCenterResult> list) : base(list) { }
        #endregion
    }
    #endregion



    [Serializable]
    public class BlobVisionColletXyPositionCalibratorConfigurationBody
    {
        #region Field
        private int m_Velocity;
        private PathParameter m_pathGenratorParameter;

        private bool m_UseFinePathGenerator;
        #endregion

        #region Constructor
        public BlobVisionColletXyPositionCalibratorConfigurationBody()
        {
        }
        #endregion

        #region Property
        public int Velocity
        {
            get { return this.m_Velocity; }
            set { this.m_Velocity = value; }
        }

        public PathParameter pathGenratorParameter
        {
            get { return this.m_pathGenratorParameter; }
            set { this.m_pathGenratorParameter = value; }
        }

        public bool UseFinePathGenerator
        {
            get { return this.m_UseFinePathGenerator; }
            set { this.m_UseFinePathGenerator = value; }
        }
        #endregion

        #region qConfiguration Members
        protected void SetDefaultValues()
        {

            this.Velocity = 80;

            this.UseFinePathGenerator = false;
        }
        #endregion
    }
    #region KeyXyztCoordinatePositionPair
    [Serializable]
    public class KeyXyztCoordinatePositionPair
    {
        #region Field
        private object m_Key;
        private XyztCoordinate m_Value;
        private int m_VelocityPercent;
        private bool m_Deleted;
        #endregion

        #region Constructor
        public KeyXyztCoordinatePositionPair(object key)
        {
            if (key != null)
                this.Key = key;
            this.Value = new XyztCoordinate();
            this.VelocityPercent = 100;
            this.Deleted = false;
        }
        public KeyXyztCoordinatePositionPair() : this(null) { }
        #endregion

        #region Property
        /// <summary>
        /// 위치 정보에 대한 키를 가져오거나 설정한다.
        /// </summary>
        public object Key
        {
            get { return this.m_Key; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Key");
                if (value is Enum == false)
                    throw new ArgumentException("Key must be an enumerated type.", "Key");
                this.m_Key = value;
            }
        }

        /// <summary>
        /// 위치 정보를 가져오거나 설정한다.
        /// </summary>
        public XyztCoordinate Value
        {
            get { return this.m_Value; }
            set { this.m_Value = value; }
        }

        /// <summary>
        /// 이동시 사용할 속도의 percent를 가져오거나 설정한다.
        /// </summary>
        public int VelocityPercent
        {
            get { return this.m_VelocityPercent; }
            set
            {
                if (value <= 0 || 100 < value)
                    throw new ArgumentOutOfRangeException("VelocityPercent");
                this.m_VelocityPercent = value;
            }
        }

        /// <summary>
        /// 삭제된 항목인지 여부를 가져오거나 설정한다.
        /// 삭제된 경우는 화면에서 자동으로 삭제되도록 한다.
        /// </summary>
        public bool Deleted
        {
            get { return this.m_Deleted; }
            set { this.m_Deleted = value; }
        }
        #endregion

        #region Method

        #endregion

    }
    #endregion
}
