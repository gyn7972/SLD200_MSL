using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Matrox.MatroxImagingLibrary;

using QMC.Common.Vision.Tools;

namespace QMC.Common.Vision.Matrox.Tools
{
    #region MILPatternMatchingVisionTool
    public class MILPatternMatchingVisionTool : PatternMatchingVisionTool
    {
        #region Field
        private VisionImage m_LatestImage;
        private MIL_ID m_MILInputImage;
        private MIL_ID m_MILTrainImage;
        private MIL_ID milResult;
        private MIL_ID milImage;
        private MIL_ID milTrainImage;
        private MIL_ID model;
        #endregion

        #region Constructor
        public MILPatternMatchingVisionTool(string name) : base(name)
        {
            this.Parameter = new MILPatternMatchingVisionToolParameter();
            this.m_MILInputImage = MIL.M_NULL;
            this.m_MILTrainImage = MIL.M_NULL;
        }

        public MILPatternMatchingVisionTool() : this("") { }
        #endregion

        #region Property
        public MIL_ID MILInputImage
        {
            get { return this.m_MILInputImage; }
            set { this.m_MILInputImage = value; }
        }

        public MIL_ID MILTrainImage
        {
            get { return this.m_MILTrainImage; }
            set { this.m_MILTrainImage = value; }
        }
        #endregion

        #region Method
        private int SetValue(VisionImage image)
        {
            int ret = 0;

            MILCustomizedVisionImage milVisionImage = null;
            milImage = MIL.M_NULL;

            this.m_LatestImage = image;

            milVisionImage = image.CustomizedData as MILCustomizedVisionImage;

            milImage = (MIL_ID)milVisionImage.Image;

            if (milVisionImage != null)
            {
                if (milVisionImage.Region == null)
                {

                }
                else
                {
                    MIL.MbufSetRegion(milImage, milImage, MIL.M_DEFAULT, MIL.M_RASTERIZE, MIL.M_DEFAULT);
                }
            }

            this.MILInputImage = milImage;

            if ((ret = this.OnLearn()) != 0) return ret;

            return ret;
        }
        private int GetValue()
        {
            int ret = 0;
            int count = 0;
            PatternMatchingResult result = new PatternMatchingResult(this.Name);
            PatternMatchingResult.PatternMatchingResultValue resultValue;
            double angle = 0.0;
            milResult = MIL.M_NULL;
            MIL_ID milSystem = MSystem.Instance.MilSystem;

            if (this.Parameter.MaxInstance == -1)
                this.Parameter.MaxInstance = 1;

            double[] xResult = new double[this.Parameter.MaxInstance];
            double[] yResult = new double[this.Parameter.MaxInstance];
            double[] angleResult = new double[this.Parameter.MaxInstance];
            double[] scoreResult = new double[this.Parameter.MaxInstance];
            double[] scaleResult = new double[this.Parameter.MaxInstance];

            try
            {
                // Preprocess the model.
                MIL.MpatPreprocModel(this.MILInputImage, this.MILTrainImage, MIL.M_DEFAULT);

                MIL.MpatAllocResult(milSystem, (MIL_INT)this.Parameter.MaxInstance, ref milResult);

                // Dummy first call for bench measure purpose only (bench stabilization, cache effect, etc...). This first call is NOT required by the application.
                MIL.MpatFindModel(this.MILInputImage, this.MILTrainImage, milResult);

                // If one model was found above the acceptance threshold.
                if (MIL.MpatGetNumber(milResult) == 1L)
                {
                    // Read results and draw a box around the model occurrence.
                    MIL.MpatGetResult(milResult, MIL.M_POSITION_X, xResult);
                    MIL.MpatGetResult(milResult, MIL.M_POSITION_Y, yResult);
                    MIL.MpatGetResult(milResult, MIL.M_ANGLE, angleResult);
                    MIL.MpatGetResult(milResult, MIL.M_SCORE, scoreResult);


                    resultValue = new PatternMatchingResult.PatternMatchingResultValue();
                    resultValue.X = xResult[0];
                    resultValue.Y = yResult[0];

                    if (angleResult[0] >= 0.0 && angleResult[0] < 180.0)
                    {
                        resultValue.R = -angleResult[0];
                    }
                    else if (angleResult[0] > 180.0)
                    {
                        resultValue.R = 360.0 - angleResult[0];
                    }

                    resultValue.Score = scoreResult[0];
                    //if (resultValue.R < this.Parameter.AngleTolerance.Maximum)
                    {
                        result.Values.Add(resultValue);
                    }
                }
                else if (MIL.MpatGetNumber(milResult) > 1L)
                {
                    MIL.MpatGetResult(milResult, MIL.M_POSITION_X, xResult);
                    MIL.MpatGetResult(milResult, MIL.M_POSITION_Y, yResult);
                    MIL.MpatGetResult(milResult, MIL.M_ANGLE, angleResult);
                    MIL.MpatGetResult(milResult, MIL.M_SCORE, scoreResult);

                    for (int i = 0; i < MIL.MpatGetNumber(milResult); i++)
                    {
                        resultValue = new PatternMatchingResult.PatternMatchingResultValue();
                        resultValue.X = xResult[i];
                        resultValue.Y = yResult[i];

                        if (angleResult[i] >= 0.0 && angleResult[i] < 180.0)
                        {
                            resultValue.R = -angleResult[i];
                        }
                        else if (angleResult[i] > 180.0)
                        {
                            resultValue.R = 360.0 - angleResult[i];
                        }

                        resultValue.Score = scoreResult[i];
                        // if (resultValue.R < this.Parameter.AngleTolerance.Maximum)
                        {
                            result.Values.Add(resultValue);
                        }
                    }
                }
                else
                {
                }
            }
            finally
            {
                this.Result = result;

                this.SortingResult();

                this.ConfirmDuplication(new System.Drawing.Size(this.SubTools.OutputImage.Header.Width, this.SubTools.OutputImage.Header.Height));

                for (int i = 0; i < this.Result.Values.Count; i++)
                {
                    VisionToolLog.Write(this, string.Format("X[{0}] : {1}", i, this.Result.Values[i].X));
                    VisionToolLog.Write(this, string.Format("Y[{0}] : {1}", i, this.Result.Values[i].Y));
                    VisionToolLog.Write(this, string.Format("R[{0}] : {1}", i, this.Result.Values[i].R));
                    VisionToolLog.Write(this, string.Format("Score[{0}] : {1}", i, this.Result.Values[i].Score));
                }

                //TODO : ProcessingTime 및 ResultMessage 추가.
                //result.ProcessingTime = this.Tool.RunStatus.ProcessingTime;
                //VisionToolLog.Write(this, string.Format("ProcessingTime : {0}", this.Result.ProcessingTime));
                //result.ResultMessage = this.Tool.RunStatus.Message;
                //VisionToolLog.Write(this, string.Format("ResultMessage : {0}", this.Result.ResultMessage));

                if (((MILCustomizedVisionImage)this.SubTools.OutputImage.CustomizedData).Region == null)
                {
                    this.RotateMatchPoint(new System.Drawing.Size(this.SubTools.OutputImage.Header.Width, this.SubTools.OutputImage.Header.Height));
                }
                else
                {
                    Rect? roi = ((MILCustomizedVisionImage)this.SubTools.OutputImage.CustomizedData).Region;
                    this.RotateMatchPoint(new System.Drawing.Size((int)roi.Value.Width, (int)roi.Value.Height));
                }
            }

            return ret;
        }
        #endregion

        #region PatternMatchingVisionTool Members
        protected override int OnLearn()
        {
            int ret = 0;
            VisionImage image = null;
            milTrainImage = MIL.M_NULL;
            MILCustomizedVisionImage milVisionImage = null;
            CycleTimer timer = new CycleTimer();
            MIL_ID milSystem = MSystem.Instance.MilSystem;

            image = this.SubTools.OutputImage;
            if (image.CustomizedData == null)
            {
                if ((ret = MILCustomizedVisionImage.Create(ref image)) != 0) return ret;
            }

            milVisionImage = image.CustomizedData as MILCustomizedVisionImage;

            milTrainImage = (MIL_ID)milVisionImage.Image;

            model = MIL.M_NULL;

            MIL.MpatAllocModel(milSystem, milTrainImage, (MIL_INT)0, (MIL_INT)0, (MIL_INT)this.SubTools.OutputImage.Header.Width, (MIL_INT)this.SubTools.OutputImage.Header.Height, MIL.M_NORMALIZED, ref model);

            this.MILTrainImage = model;
            MIL.MpatSetAccuracy(this.MILTrainImage, MIL.M_HIGH);
            MIL.MpatSetSpeed(this.MILTrainImage, MIL.M_MEDIUM);

            MIL.MpatSetAngle(this.MILTrainImage, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);
            MIL.MpatSetAngle(this.MILTrainImage, MIL.M_SEARCH_ANGLE, 0);
            //MIL.MpatSetAngle(this.MILTrainImage, MIL.M_SEARCH_ANGLE, this.Parameter.AngleTolerance.Maximum);
            //if (this.Parameter.AngleTolerance.Maximum != 0)
            //{
            //    MIL.MpatSetAngle(this.MILTrainImage, MIL.M_SEARCH_ANGLE_DELTA_NEG, Math.Abs(this.Parameter.AngleTolerance.Minimum));
            //    MIL.MpatSetAngle(this.MILTrainImage, MIL.M_SEARCH_ANGLE_DELTA_POS, Math.Abs(this.Parameter.AngleTolerance.Maximum));
            //}
            MIL.MpatSetAngle(this.MILTrainImage, MIL.M_SEARCH_ANGLE_ACCURACY, 0.1);
            MIL.MpatSetAngle(this.MILTrainImage, MIL.M_SEARCH_ANGLE_INTERPOLATION_MODE, MIL.M_BICUBIC);

            MIL.MpatSetNumber(this.MILTrainImage, (MIL_INT)this.Parameter.MaxInstance);
            MIL.MpatSetSearchParameter(this.MILTrainImage, MIL.M_COARSE_SEARCH_ACCEPTANCE, this.Parameter.MinScore * 100.0);

            //TODO : MaskImage 추가.
            #region MarkImage 추가
            //MaskImage 추가
            //if (this.Parameter.UseMaskImage == true)
            //{
            //    this.Parameter.Pattern.TrainImageMask = new CogImage8Grey(image.Header.Width, image.Header.Height);

            //    //Train Image Mask Set 255
            //    for (int i = 0; i < this.Parameter.Pattern.TrainImageMask.Height; i++)
            //    {
            //        for (int j = 0; j < this.Parameter.Pattern.TrainImageMask.Width; j++)
            //        {
            //            this.Parameter.Pattern.TrainImageMask.SetPixel(j, i, 255);
            //        }
            //    }

            //    //Train Image Mask Set  0
            //    for (int i = (int)this.Parameter.MaskRegion.Y; i < (int)(this.Parameter.MaskRegion.Y + this.Parameter.MaskRegion.Height); i++)
            //    {
            //        for (int j = (int)this.Parameter.MaskRegion.X; j < (int)(this.Parameter.MaskRegion.X + this.Parameter.MaskRegion.Width); j++)
            //        {
            //            this.Parameter.Pattern.TrainImageMask.SetPixel(j, i, 0);
            //        }
            //    }
            //}
            //Mask Image 사용 안한다면 Mask Region 전부 255로 Set
            //else
            //{
            //    this.Parameter.Pattern.TrainImageMask = new CogImage8Grey(image.Header.Width, image.Header.Height);

            //    for (int i = 0; i < this.Parameter.Pattern.TrainImageMask.Height; i++)
            //    {
            //        for (int j = 0; j < this.Parameter.Pattern.TrainImageMask.Width; j++)
            //        {
            //            this.Parameter.Pattern.TrainImageMask.SetPixel(j, i, 255);
            //        }
            //    }
            //}
            #endregion

            timer.End();

            return ret;
        }
        #endregion

        #region VisionTool Members
        public new MILPatternMatchingVisionToolParameter Parameter
        {
            get { return base.Parameter as MILPatternMatchingVisionToolParameter; }
            set { base.Parameter = value; }
        }

        protected override int OnCheckedLicense()
        {
            int ret = 0;
            double typeD = 0.0;

            MIL_ID application = MSystem.Instance.MilApplication;
            MIL.MappInquire(application, MIL.M_LICENSE_MODULES, ref typeD);

            return ret;
        }

        protected override int OnPrepare()
        {
            int ret = 0;
            this.MILTrainImage = MIL.M_NULL;
            this.MILInputImage = MIL.M_NULL;
            this.Result = new PatternMatchingResult(this.Name);
            return ret;
        }

        protected override int OnRun()
        {
            int ret = 0;
            VisionImage image = this.InputImage;

            if (image.CustomizedData == null)
            {
                if ((ret = MILCustomizedVisionImage.Create(ref image)) != 0) return ret;
            }
            if ((ret = this.SetValue(image)) != 0) return ret;
            if ((ret = this.GetValue()) != 0) return ret;

            this.OutputImage = this.InputImage;
            return ret;
        }

        protected override void OnDispose()
        {
            // Check해서 Free (Input, Train)
            if (milResult != MIL.M_NULL)
                MIL.MbufFree(milResult);
            if (milImage != MIL.M_NULL)
                MIL.MbufFree(milImage);
            if (milTrainImage != MIL.M_NULL)
                MIL.MbufFree(milTrainImage);
            if (model != MIL.M_NULL)
                MIL.MbufFree(model);
            if (this.MILInputImage != MIL.M_NULL)
                MIL.MbufFree(this.MILInputImage);
            if (this.MILTrainImage != MIL.M_NULL)
                MIL.MbufFree(this.MILTrainImage);
        }
        #endregion
    }
    #endregion

    #region MILPatternMatchingVisionToolParameter
    [Serializable]
    public class MILPatternMatchingVisionToolParameter : PatternMatchingVisionToolParameter
    {
        #region Field
        #endregion

        #region Constructor
        public MILPatternMatchingVisionToolParameter() : base()
        {
            this.ResultOverlayVisible = true;
            this.MaxInstance = -1;
            this.MinScore = -1;
        }
        #endregion

        #region Property
        #endregion
    }
    #endregion
}
