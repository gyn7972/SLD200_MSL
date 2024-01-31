using QMC.Common.Vision.Cameras;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Vision.EureSys
{
    [Serializable]
    public class ServerGrabLinkMultiCamCamera : GrabLinkMultiCamCamera
    {
        #region Field
        public SharedMemoryServer SharedMemoryServer;
        private VisionImage m_RecentGrabImage;
        #endregion

        #region Constructor
        public ServerGrabLinkMultiCamCamera(string strName)
            : base(strName)
        {
            this.RecentGrabImage = null;
        }

        public ServerGrabLinkMultiCamCamera() : this("ServerGrabLinkMultiCamCamera") { }
        #endregion

        public VisionImage RecentGrabImage
        {
            get { return this.m_RecentGrabImage; }
            set { this.m_RecentGrabImage = value; }
        }

        #region GrabLinkMultiCamCamera
        protected override int OnOpen()
        {
            int ret = 0;

            ret = base.OnOpen();

            return ret;
        }

        protected override int OnClose()
        {
            return base.OnClose();
        }

        public override void Close()
        {
            base.Close();
        }

        public override int Initialize()
        {
            int ret = 0;

            if ((ret = base.Initialize()) != 0)
            {
                return ret;
            }

            if (this.SharedMemoryServer == null)
                this.SharedMemoryServer = new SharedMemoryServer(this.Name);

            this.SharedMemoryServer.OnExposeRequest -= SharedMemoryServer_OnExposeRequest;
            this.SharedMemoryServer.OnGrabRequest -= SharedMemoryServer_OnGrabRequest;
            this.SharedMemoryServer.OnReadoutRequest -= SharedMemoryServer_OnReadoutRequest;

            this.SharedMemoryServer.OnExposeRequest += SharedMemoryServer_OnExposeRequest;
            this.SharedMemoryServer.OnGrabRequest += SharedMemoryServer_OnGrabRequest;
            this.SharedMemoryServer.OnReadoutRequest += SharedMemoryServer_OnReadoutRequest;

            return ret;
        }
        #endregion

        #region SharedMemoryServer Event Handler
        private void SharedMemoryServer_OnExposeRequest()
        {
            if (this.Opened == false)
                return;

            this.Expose();
        }

        private void SharedMemoryServer_OnGrabRequest(out Bitmap bitmap)
        {
            VisionImage image = null;
            bitmap = null;

            if (this.Opened == false)
                return;

            this.GrabSync(out image);

            if (image != null)
            {
                this.RecentGrabImage = image;
                bitmap = image.GetImage() as Bitmap;
            }
        }

        private void SharedMemoryServer_OnReadoutRequest(out Bitmap bitmap)
        {
            VisionImage image = null;
            bitmap = null;

            if (this.Opened == false)
                return;

            this.Readout(out image);

            if (image != null)
            {
                this.RecentGrabImage = image;
                bitmap = image.GetImage() as Bitmap;
            }
        }
        #endregion
    }
}
