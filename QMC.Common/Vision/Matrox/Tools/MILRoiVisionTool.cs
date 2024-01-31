using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using QMC.Common.Vision.Tools;

namespace QMC.Common.Vision.Matrox.Tools
{
    #region MILRoiVisionTool
    public class MILRoiVisionTool : RoiVisionTool
    {

        #region Field

        #endregion

        #region Constructor
        public MILRoiVisionTool(string name) : base(name)
        {
            this.Parameter = new MILRoiVisionToolParameter(name);
        }
        public MILRoiVisionTool() : this("") { }
        #endregion

        #region Property

        #endregion

        #region Method
        private int SetValue(VisionImage visionImage)
        {
            int ret = 0;
            MILCustomizedVisionImage milVisionImage = visionImage.CustomizedData as MILCustomizedVisionImage;

            //CogRectangle parameter = null;
            RoiResult result = new RoiResult(this.Name);
            StopWatch timer = new StopWatch();

            try
            {
                timer.Start();

                if (this.Parameter.IsFull == true)
                {
                    milVisionImage.Region = null;
                }
                else
                {
                    Rect parameter = new Rect();
                    parameter.X = this.Parameter.StartLocation.X;
                    parameter.Y = this.Parameter.StartLocation.Y;
                    VisionToolLog.Write(this, string.Format("X : {0}, Y : {1}", parameter.X, parameter.Y));
                    parameter.Width = this.Parameter.Size.Width;
                    parameter.Height = this.Parameter.Size.Height;
                    VisionToolLog.Write(this, string.Format("Width : {0}, Height : {1}", parameter.Width, parameter.Height));
                    VisionToolLog.Write(this, string.Format("Full : {0}", this.Parameter.IsFull));
                    milVisionImage.Region = parameter;
                }

                timer.Stop();
                result.ProcessingTime = timer.Elapsed.Milliseconds;
                VisionToolLog.Write(this, string.Format("ProcessingTime : {0}", result.ProcessingTime));
            }
            finally
            {
                timer.Stop();
                this.Result = result;
            }
            return ret;
        }
        #endregion

        #region VisionTool Members
        public new MILRoiVisionToolParameter Parameter
        {
            get { return base.Parameter as MILRoiVisionToolParameter; }
            set { base.Parameter = value; }
        }

        protected override int OnCheckedLicense()
        {
            int ret = 0;
            return ret;
        }

        protected override int OnPrepare()
        {
            int ret = 0;
            this.Result = new RoiResult(this.Name);
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

            this.OutputImage = this.InputImage;
            return ret;
        }
        #endregion

        #region IDisposable Member
        protected override void OnDispose()
        {
        }
        #endregion
    }
    #endregion

    #region MILRoiVisionToolParameter
    [Serializable]
    public class MILRoiVisionToolParameter : RoiVisionToolParameter
    {
        #region Field
        #endregion

        #region Constructor
        public MILRoiVisionToolParameter(string name) : base(name)
        {
        }
        public MILRoiVisionToolParameter() : this("") { }
        #endregion

        #region Property
        #endregion
    }
    #endregion
}
