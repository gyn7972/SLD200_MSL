using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Vision.Cameras;

namespace QMC.Common.Vision.EureSys
{
    [Serializable]
    public enum ParamMultiCamCameraConfigKey
    {
        Channel,
        ChannelActive,
        AcquisitionFinished,
        ExposureFinished,
        GrabNewImageEventEnable,
        SettingFrameCount,
        CamFilePath,
        //Connector,
        ColorFormat,
        TrigLine,
        TrigMode,
        NextTrigMode,
        //BoardTopology,
        BoardIndex,
        SurfaceCount,
    }
    [Serializable]
    public class MultiCamCameraConfig : CameraConfig
    {
        public int m_Channel;
        public bool m_ChannelActive;
        public bool m_AcquisitionFinished;
        public bool m_ExposureFinished;
        public bool m_GrabNewImageEventEnable;
        public MultiCamCamera.FrameNoPerSequence m_SettingFrameCount;
      
        [DefaultValue(0)]
        public int Channel
        {
            get { return this.m_Channel; }
            set { this.m_Channel = value; }
        }

        /// <summary>
        /// EureSys Camera의 Channel Active Status 정보
        /// </summary>
		[DefaultValue(false)]
        public bool ChannelActive
        {
            get { return this.m_ChannelActive; }
            private set { this.m_ChannelActive = value; }
        }

        /// <summary>
        /// EureSys Camera의 Acquisition 완료 Status 정보
        /// </summary>
		[DefaultValue(false)]
        public bool AcquisitionFinished
        {
            get { return this.m_AcquisitionFinished; }
            private set
            {
                if (this.m_AcquisitionFinished == value) return;
                //this.WriteLog(LogLevel.Lowest, string.Format("AcquisitionFinished Value :{0} -> {1}", m_AcquisitionFinished, value));
                Console.WriteLine(string.Format("AcquisitionFinished Value :{0} -> {1}", m_AcquisitionFinished, value));
                this.m_AcquisitionFinished = value;
            }
        }

        /// <summary>
        /// EureSys Camera의 Exposure 완료 Status 정보
        /// </summary>
		[DefaultValue(false)]
        public bool ExposureFinished
        {
            get { return this.m_ExposureFinished; }
            private set
            {
                if (this.m_ExposureFinished == value) return;
                //this.WriteLog(LogLevel.Lowest, string.Format("ExposureFinished Value :{0} -> {1}", m_ExposureFinished, value));
                Console.WriteLine(string.Format("ExposureFinished Value :{0} -> {1}", m_ExposureFinished, value));
                this.m_ExposureFinished = value;
            }
        }

        /// <summary>
        /// GrabNewImageEvent 사용 여부에 대해서 가져오거나 설정한다.
        /// </summary>
        public bool GrabNewImageEventEnable
        {
            get { return this.m_GrabNewImageEventEnable; }
            set { this.m_GrabNewImageEventEnable = value; }
        }

        public MultiCamCamera.FrameNoPerSequence SettingFrameCount
        {
            get { return this.m_SettingFrameCount; }
            private set { this.m_SettingFrameCount = value; }
        }


        [Category("Channel")]
        public string CamFilePath
        {
            get;
            set;
        }

        [Category("Channel")]
        public Enum Connector
        {
            get;
            set;
        }

        [Category("Channel")]
        public MultiCamCamera.ColorFormats ColorFormat
        {
            get;
            set;
        }

        [Category("Channel")]
        public MultiCamCamera.TriggerLine TrigLine
        {
            get;
            set;
        }
        [Category("Channel")]
        public MultiCamCamera.TriggerMode TrigMode { set; get; }
        [Category("Channel")]
        public MultiCamCamera.NextTriggerMode NextTrigMode
        {
            get;
            set;
        }
        [Category("Board")]
        public Enum BoardTopology { set; get; }
        [Category("Board")]
        public uint BoardIndex { set; get; }
        [Category("Channel")]
        public int SurfaceCount
        {
            get;
            set;
        }

        public MultiCamCameraConfig()
        {
            this.GrabNewImageEventEnable = true;
            this.SettingFrameCount = MultiCamCamera.FrameNoPerSequence.One;
            this.BoardIndex = 0;
            this.ColorFormat = MultiCamCamera.ColorFormats.Y8;                    

            this.TrigLine = MultiCamCamera.TriggerLine.Nom;
            this.TrigMode = MultiCamCamera.TriggerMode.Immediate;
            this.NextTrigMode = MultiCamCamera.NextTriggerMode.Combined;
            this.SurfaceCount = 3;

        }

        public override ListParam ToListParam()
        {
            ListParam listParam = base.ToListParam();
            ParamGroup group = new ParamGroup();
            group.Name = "MultiCamCameraConfig";
            {
                Param param = new Param();
                param.SetParam(nameof(Channel), Param.DisplayTypeKey.Text, Channel, Param.ValueTypeKey.Int, group.Name);
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(ChannelActive), Param.DisplayTypeKey.CheckBox, ChannelActive, Param.ValueTypeKey.Bool, group.Name);
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(AcquisitionFinished), Param.DisplayTypeKey.CheckBox, AcquisitionFinished, Param.ValueTypeKey.Bool, group.Name);
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(ExposureFinished), Param.DisplayTypeKey.CheckBox, ExposureFinished, Param.ValueTypeKey.Bool, group.Name);
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(GrabNewImageEventEnable), Param.DisplayTypeKey.CheckBox, GrabNewImageEventEnable, Param.ValueTypeKey.Bool, group.Name);
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(SettingFrameCount), Param.DisplayTypeKey.Combobox, SettingFrameCount, Param.ValueTypeKey.Int, group.Name);
                param.SelectValues.Clear();
                foreach (Enum e in Enum.GetValues(typeof(MultiCamCamera.FrameNoPerSequence)))
                {
                    param.SelectValues.Add(e.ToString());
                }
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(CamFilePath), Param.DisplayTypeKey.Text, CamFilePath, Param.ValueTypeKey.String, group.Name);
                group.AddParam(param);
            }
            //{
            //    Param param = new Param();
            //    param.SetParam(nameof(Connector), Param.DisplayTypeKey.Combobox, Connector, Param.ValueTypeKey.Int, group.Name);
            //    param.SelectValues.Clear();
            //    foreach (Enum e in Enum.GetValues(typeof(Enum)))
            //    {
            //        param.SelectValues.Add(e.ToString());
            //    }
            //    group.AddParam(param);
            //}
            {
                Param param = new Param();
                param.SetParam(nameof(ColorFormat), Param.DisplayTypeKey.Combobox, ColorFormat, Param.ValueTypeKey.Int, group.Name);
                param.SelectValues.Clear();
                foreach (Enum e in Enum.GetValues(typeof(MultiCamCamera.ColorFormats)))
                {
                    param.SelectValues.Add(e.ToString());
                }
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(TrigLine), Param.DisplayTypeKey.Combobox, TrigLine, Param.ValueTypeKey.Int, group.Name);
                param.SelectValues.Clear();
                foreach (Enum e in Enum.GetValues(typeof(MultiCamCamera.TriggerLine)))
                {
                    param.SelectValues.Add(e.ToString());
                }
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(TrigMode), Param.DisplayTypeKey.Combobox, TrigMode, Param.ValueTypeKey.Int, group.Name);
                param.SelectValues.Clear();
                foreach (Enum e in Enum.GetValues(typeof(MultiCamCamera.TriggerMode)))
                {
                    param.SelectValues.Add(e.ToString());
                }
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(NextTrigMode), Param.DisplayTypeKey.Combobox, NextTrigMode, Param.ValueTypeKey.Int, group.Name);
                param.SelectValues.Clear();
                foreach (Enum e in Enum.GetValues(typeof(MultiCamCamera.NextTriggerMode)))
                {
                    param.SelectValues.Add(e.ToString());
                }
                group.AddParam(param);
            }
            //{
            //    Param param = new Param();
            //    param.SetParam(nameof(BoardTopology), Param.DisplayTypeKey.Combobox, BoardTopology, Param.ValueTypeKey.Int, group.Name);
            //    param.SelectValues.Clear();
            //    foreach (Enum e in Enum.GetValues(typeof(Enum)))
            //    {
            //        param.SelectValues.Add(e.ToString());
            //    }
            //    group.AddParam(param);
            //}
            {
                Param param = new Param();
                param.SetParam(nameof(BoardIndex), Param.DisplayTypeKey.Text, BoardIndex, Param.ValueTypeKey.Uint, group.Name);
                group.AddParam(param);
            }
            {
                Param param = new Param();
                param.SetParam(nameof(SurfaceCount), Param.DisplayTypeKey.Text, SurfaceCount, Param.ValueTypeKey.Int, group.Name);
                group.AddParam(param);
            }

            listParam.SetGroup(group);
            return listParam;
        }

        public override void SetParam(ListParam listParam)
        {
            base.SetParam(listParam);
            ParamGroup group = listParam.GetGroup("MultiCamCameraConfig");

            if (group != null)
            {
                Param param = null;
                param = group.GetParam((int)ParamMultiCamCameraConfigKey.Channel);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        Channel = value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.ChannelActive);
                if (param != null)
                {
                    bool value = false;
                    if (param.GetBoolValue(ref value))
                    {
                        ChannelActive = value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.AcquisitionFinished);
                if (param != null)
                {
                    bool value = false;
                    if (param.GetBoolValue(ref value))
                    {
                        AcquisitionFinished = value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.ExposureFinished);
                if (param != null)
                {
                    bool value = false;
                    if (param.GetBoolValue(ref value))
                    {
                        ExposureFinished = value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.GrabNewImageEventEnable);
                if (param != null)
                {
                    bool value = false;
                    if (param.GetBoolValue(ref value))
                    {
                        GrabNewImageEventEnable = value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.SettingFrameCount);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        SettingFrameCount = (MultiCamCamera.FrameNoPerSequence)value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.CamFilePath);
                if (param != null)
                {
                    string value = string.Empty;
                    if (param.GetStringValue(ref value))
                    {
                        CamFilePath = value;
                    }
                }

                //param = group.GetParam((int)ParamMultiCamCameraConfigKey.Connector); 질문필요!! 
                //if (param != null)
                //{
                //    int value = 0;
                //    if (param.GetIntValue(ref value))
                //    {
                //        Connector = (Enum)value;
                //    }
                //}

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.ColorFormat);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        ColorFormat = (MultiCamCamera.ColorFormats)value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.TrigLine);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        TrigLine = (MultiCamCamera.TriggerLine)value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.TrigMode);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        TrigMode = (MultiCamCamera.TriggerMode)value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.NextTrigMode);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        NextTrigMode = (MultiCamCamera.NextTriggerMode)value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.BoardIndex);
                if (param != null)
                {
                    uint value = 0;
                    if (param.GetUintValue(ref value))
                    {
                        BoardIndex = value;
                    }
                }

                param = group.GetParam((int)ParamMultiCamCameraConfigKey.SurfaceCount);
                if (param != null)
                {
                    int value = 0;
                    if (param.GetIntValue(ref value))
                    {
                        SurfaceCount = value;
                    }
                }

            }
        }
    }
}
