using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Vision.EureSys
{
    [Serializable]
    public enum ParamGrabLinkMultiCamCameraConfigKey
    {
        Connector,
        BoardTopology,
        TapConfiguration,
        TapGeometry,

    }
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

        public override ListParam ToListParam()
        {

            ListParam listParam = base.ToListParam();
            ParamGroup group = new ParamGroup();
            group.Name = this.GetType().Name;
            {
                Param param = new Param();
                param.SetParam(nameof(Connector), Param.DisplayTypeKey.Combobox, Connector, Param.ValueTypeKey.Int, group.Name);
                param.SelectValues.Clear();
                foreach (Enum e in Enum.GetValues(typeof(GrabLinkMultiCamCamera.eConnector)))
                {
                    param.SelectValues.Add(e.ToString());
                }
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(BoardTopology), Param.DisplayTypeKey.Combobox, BoardTopology, Param.ValueTypeKey.Int, group.Name);
                param.SelectValues.Clear();
                foreach (Enum e in Enum.GetValues(typeof(GrabLinkMultiCamCamera.eBoardTopology)))
                {
                    param.SelectValues.Add(e.ToString());
                }
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(TapConfiguration), Param.DisplayTypeKey.Combobox, TapConfiguration, Param.ValueTypeKey.Int, group.Name);
                param.SelectValues.Clear();
                foreach (Enum e in Enum.GetValues(typeof(GrabLinkMultiCamCamera.eTapConfiguration)))
                {
                    param.SelectValues.Add(e.ToString());
                }
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(TapGeometry), Param.DisplayTypeKey.Combobox, TapGeometry, Param.ValueTypeKey.Int, group.Name);
                param.SelectValues.Clear();
                foreach (Enum e in Enum.GetValues(typeof(GrabLinkMultiCamCamera.eTapGeometry)))
                {
                    param.SelectValues.Add(e.ToString());
                }
                group.AddParam(param);
            }
            listParam.SetGroup(group);
            return listParam;
        }

        public override void SetParam(ListParam listParam)
        {
            base.SetParam(listParam);

            ParamGroup group = listParam.GetGroup(this.GetType().Name);
            if(group != null)
            {
                Param param = null;

                param = group.GetParam((int)ParamGrabLinkMultiCamCameraConfigKey.Connector);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        Connector = (GrabLinkMultiCamCamera.eConnector)value;
                    }
                }


                param = group.GetParam((int)ParamGrabLinkMultiCamCameraConfigKey.BoardTopology);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        BoardTopology = (GrabLinkMultiCamCamera.eBoardTopology)value;
                    }
                }

                param = group.GetParam((int)ParamGrabLinkMultiCamCameraConfigKey.TapConfiguration);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        TapConfiguration = (GrabLinkMultiCamCamera.eTapConfiguration)value;
                    }
                }

                param = group.GetParam((int)ParamGrabLinkMultiCamCameraConfigKey.TapGeometry);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        TapGeometry = (GrabLinkMultiCamCamera.eTapGeometry)value;
                    }
                }
            }
        }
    }
}
