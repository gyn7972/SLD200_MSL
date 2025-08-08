using QMC.Common.Modules;
using QMC.Common.Vision;
using QMC.Common.Vision.Cameras;
using QMC.Common.Vision.Cognex;
using QMC.Common.Vision.Matrox.Tools;
using QMC.Common.Vision.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public class PatternMatchingVisionPart : VisionPart
    {
        #region Field
        protected VisionProPatternMatchingVisionTool m_PatternMatchingTool;
        protected VisionProRoiVisionTool m_RoiTrain;
        protected VisionProRoiVisionTool m_RoiInspect;
        #endregion

        #region Property
        public VisionImage TestImage { set; get; }
        public VisionImage TrainImage { set; get; }
        public bool Simulated { set; get; }
        #endregion

        #region Constructor
        public PatternMatchingVisionPart(string strName) : base(strName)
        {
            m_PatternMatchingTool = new VisionProPatternMatchingVisionTool();
            m_RoiTrain = new VisionProRoiVisionTool();
            m_RoiInspect = new VisionProRoiVisionTool();

            TestImage = new VisionImage();
            TrainImage = new VisionImage();
        }
        #endregion

        #region Method
        public PatternMatchingResult GetResult()
        {
            return m_PatternMatchingTool.Result;
        }
        public RoiVisionTool GetTrainRoi()
        {
            return m_RoiTrain;
        }
        public RoiVisionTool GetInspectRoi()
        {
            return m_RoiInspect;
        }

        public int OnTrain(Point startPoint, Point endPoint, PatternMatchingParameters parameter, IlluminationDataSet illuminationData)
        {
            int ret = 0;
            VisionImage image;

            // Todo 장비에서 무조건 주석할것!
            //Simulated = true;
            if (Simulated)
            {
                image = TestImage;
            }
            else
            {
                if (Illuminator != null)
                {
                    if ((ret = OnSetIllumination(illuminationData, true)) != 0) return ret;
                }
                if ((ret = Camera.GrabSync(Purpose.Processing, out image)) != 0) return ret;
            }

            m_RoiTrain.Parameter.StartLocation = startPoint;
            m_RoiTrain.Parameter.EndLocation = endPoint;

            TrainImage = image.CutVisionImage(m_RoiTrain.Parameter.StartLocation, m_RoiTrain.Parameter.EndLocation);

            if (parameter != null)
            {
                parameter.TrainImage = TrainImage;
            }

            return ret;
        }

        public int OnSearch(Point startRoiPoint, Point endRoiPoint, PatternMatchingParameters parameter, IlluminationDataSet illuminationData)
        {
            int ret = 0;

            VisionImage image;
            VisionImage inputImage = null;

            //Simulated = true;
            if (Simulated)
            {
                image = TestImage;
            }
            else
            {
                if (Illuminator != null)
                {
                    if ((ret = OnSetIllumination(illuminationData, true)) != 0) return ret;
                }
                if ((ret = Camera.GrabSync(Purpose.Processing, out image)) != 0) return ret;
            }

            // 1. 폴더 생성 (날짜 기준)
            string dateFolder = DateTime.Now.ToString("yyyyMMdd");
            string baseDir = Path.Combine("d:\\TempCrossImage", dateFolder);
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);

            // 2. 초기 원본 이미지 저장
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");  // ex: 20250612_154512_123
            //string rawImagePath = Path.Combine(baseDir, $"CrossImage_{timestamp}.bmp");
            string rawImagePath = Path.Combine(baseDir, $"CrossImage_{timestamp}");
            image.Save(rawImagePath, VisionImage.FileFilter.jpg);

            m_PatternMatchingTool.Parameter.AngleTolerance = new RangeD(parameter.MinTolerance, parameter.MaxTolerance);            
            m_PatternMatchingTool.Parameter.DuplicateChecked = parameter.DuplicateChecked;
            m_PatternMatchingTool.Parameter.MaxInstance = parameter.MaxInstance;
            m_PatternMatchingTool.Parameter.MinScore = parameter.MinScore;
            m_PatternMatchingTool.Parameter.MaskRegion = parameter.MaskRegion;
            m_PatternMatchingTool.Parameter.UseMaskImage = parameter.UseMaskImage;
            TrainImage = parameter.TrainImage;
            m_RoiInspect.Parameter.StartLocation = startRoiPoint;
            m_RoiInspect.Parameter.EndLocation = endRoiPoint;
            m_PatternMatchingTool.SubTools.InputImage = TrainImage;
            m_RoiTrain.Parameter.IsFull = true;
            m_RoiInspect.InputImage = image;
            m_RoiInspect.SubTools.InputImage = TrainImage;
            m_RoiInspect.Parameter.IsFull = false;

            if ((ret = m_RoiInspect.Run()) != 0) return ret;
            m_PatternMatchingTool.InputImage = m_RoiInspect.OutputImage;

            if ((ret = m_PatternMatchingTool.Run()) != 0) return ret;

            if (m_PatternMatchingTool.Result.Values.Count <= 0)
            {
                //Log.Write(this.Name, "VisionCalibrator Fail");
                return ret;
            }

            return ret;
        }

        public int OnSearch(VisionImage image, Point startRoiPoint, Point endRoiPoint, PatternMatchingParameters parameter, IlluminationDataSet illuminationData , bool bLearn = true)
        {
            int ret = 0;

            //VisionImage image;
            VisionImage inputImage = null;

            if (Simulated)
            {
                image = TestImage;
            }
            else if (image != null)
            {

            }
            else
            {
                //if ((ret = Camera.GrabQuickly(this.Camera, out image)) != 0) return ret;
                if (image == null)
                {
                    if (Illuminator != null)
                    {
                        if ((ret = OnSetIllumination(illuminationData, true)) != 0) return ret;
                    }
                    if ((ret = Camera.GrabSync(Purpose.Processing, out image)) != 0) return ret;
                }
            }

            m_PatternMatchingTool.Parameter.AngleTolerance = new RangeD(parameter.MinTolerance, parameter.MaxTolerance);
            m_PatternMatchingTool.Parameter.DuplicateChecked = parameter.DuplicateChecked;
            m_PatternMatchingTool.Parameter.MaxInstance = parameter.MaxInstance;
            m_PatternMatchingTool.Parameter.MinScore = parameter.MinScore;
            m_PatternMatchingTool.Parameter.MaskRegion = parameter.MaskRegion;
            m_PatternMatchingTool.Parameter.UseMaskImage = parameter.UseMaskImage;
            TrainImage = parameter.TrainImage;

            m_RoiInspect.Parameter.StartLocation = startRoiPoint;
            m_RoiInspect.Parameter.EndLocation = endRoiPoint;

            m_PatternMatchingTool.SubTools.InputImage = TrainImage;

            m_RoiTrain.Parameter.IsFull = true;

            m_RoiInspect.InputImage = image;
            m_RoiInspect.Parameter.IsFull = false;

            if ((ret = m_RoiInspect.Run()) != 0) return ret;
            m_PatternMatchingTool.InputImage = m_RoiInspect.OutputImage;

            if (bLearn)
            {

                if ((ret = m_PatternMatchingTool.Run()) != 0) return ret;
            }
            else
            {
                m_PatternMatchingTool.SetValue(m_PatternMatchingTool.InputImage, bLearn);
                ret = m_PatternMatchingTool.GetValue();
            }

            if (m_PatternMatchingTool.Result.Values.Count <= 0)
            {
                //Log.Write(this.Name, "VisionCalibrator Fail");
                return ret;
            }

            return ret;
        }
        #endregion

        #region VisionPart Members
        public override int Create()
        {
            int ret = 0;

            if (m_PatternMatchingTool == null)
                m_PatternMatchingTool = new VisionProPatternMatchingVisionTool(this.Name);
            if (m_RoiTrain == null)
                m_RoiTrain = new VisionProRoiVisionTool(this.Name);
            if (m_RoiInspect == null)
                m_RoiInspect = new VisionProRoiVisionTool(this.Name);

            //m_PatternMatchingTool.Prepare();
            m_PatternMatchingTool.SubTools.Clear();
            m_PatternMatchingTool.SubTools.Add(m_RoiTrain);

            return ret;
        }

        public override void Close()
        {
            base.Close();
        }
        #endregion
    }
}
