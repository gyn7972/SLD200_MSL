using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Vision.EureSys
{
    [Serializable]
    public class GrabLinkMultiCamCameraConfig : MultiCamCameraConfig
    {
        #region ConstructConfiguration

        [Category("Channel")]
        public new GrabLinkMultiCamCamera.eConnector Connector
        {
            get { return (GrabLinkMultiCamCamera.eConnector)base.Connector; }
            set { base.Connector = value; }
        }

        [Category("Board")]
        public new GrabLinkMultiCamCamera.eBoardTopology BoardTopology
        {
            get { return (GrabLinkMultiCamCamera.eBoardTopology)base.BoardTopology; }
            set { base.BoardTopology = value; }
        }

        [Category("Board")]
        public GrabLinkMultiCamCamera.eTapConfiguration TapConfiguration
        {
            get;
            set;
        }

        [Category("Board")]
        public GrabLinkMultiCamCamera.eTapGeometry TapGeometry
        {
            get;
            set;
        }
        #endregion

        public GrabLinkMultiCamCameraConfig()
        {
            this.Connector = GrabLinkMultiCamCamera.eConnector.M;
            this.BoardTopology = GrabLinkMultiCamCamera.eBoardTopology.MONO;
            this.TapConfiguration = GrabLinkMultiCamCamera.eTapConfiguration.BASE_1T10;
            this.TapGeometry = GrabLinkMultiCamCamera.eTapGeometry.A1X_1Y;
        }
    }
}
