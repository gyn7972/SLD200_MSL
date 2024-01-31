using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Keithley.SourceMeter
{
    [Serializable]
    public class KeithleySMURecipe
    {
        #region Field
        private string m_ScriptName;
        //private int m_SubstrateCount;
        private string m_ScriptPath;
        private double m_Vf1;
        private double m_Vf2;
        private RangeD m_NTC;
        private double m_ReceiveTimeout;
        private bool m_UseInspectNTC;
        private bool m_UseInspectVf1;
        private bool m_UseInspectVf2;

        private double m_Vf1MeasureTime;
        private double m_Vf2MeasureTime;
        private double m_NTCMeasureTime;

        #endregion

        #region Constructor
        public KeithleySMURecipe(Part part)
        {
            this.ScriptName = string.Empty;
            //this.SubstrateCount = 0;
            this.ScriptPath = string.Empty;
            this.Vf1 = 0.0;
            this.Vf2 = 0.0;
            this.NTC = new RangeD(0, 0);
            this.ReceiveTimeout = 10000.0;
            this.m_UseInspectNTC = false;
            this.m_UseInspectVf1 = true;
            this.m_UseInspectVf2 = true;
            this.m_Vf1MeasureTime = 0.0;
            this.m_Vf2MeasureTime = 0.0;
            this.m_NTCMeasureTime = 0.0;

            Init(part);
        }
        #endregion

        #region Property
        [Category("Source Meter")]
        [Description("Load Script Name")]
        public string ScriptName
        {
            get { return this.m_ScriptName; }
            set { this.m_ScriptName = value; }
        }

        //[Category("Source Meter")]
        //[Description("Substrate Count")]
        //public int SubstrateCount
        //{
        //    get { return this.m_SubstrateCount; }
        //    set { this.m_SubstrateCount = value; }
        //}

        [Category("Source Meter")]
        [Description("Load Script Path")]
        public string ScriptPath
        {
            get { return this.m_ScriptPath; }
            set { this.m_ScriptPath = value; }
        }

        [Category("Source Meter")]
        [Description("Source Data ReceiveTimeout (㎳)")]
        public double ReceiveTimeout
        {
            get { return this.m_ReceiveTimeout; }
            set { this.m_ReceiveTimeout = value; }
        }

        [Category("Range")]
        [Description("Vf1 Range")]
        public double Vf1
        {
            get { return this.m_Vf1; }
            set { this.m_Vf1 = value; }
        }

        [Category("Range")]
        [Description("Vf2 Range")]
        public double Vf2
        {
            get { return this.m_Vf2; }
            set { this.m_Vf2 = value; }
        }

        [Category("Range")]
        [Description("NTC Range")]
        public RangeD NTC
        {
            get { return this.m_NTC; }
            set { this.m_NTC = value; }
        }

        [Category("Range")]
        [Description("Inspect Use NTC")]
        public bool UseInspectNTC
        {
            get { return this.m_UseInspectNTC; }
            set { this.m_UseInspectNTC = value; }
        }

        [Category("Range")]
        [Description("Inspect Use Vf1")]
        public bool UseInspectVf1
        {
            get { return this.m_UseInspectVf1; }
            set { this.m_UseInspectVf1 = value; }
        }

        [Category("Range")]
        [Description("Inspect Use Vf2")]
        public bool UseInspectVf2
        {
            get { return this.m_UseInspectVf2; }
            set { this.m_UseInspectVf2 = value; }
        }

        [Category("Time")]
        [Description("Vf1 Inspect Delay TIme.")]
        public double Vf1MeasureTime
        {
            get { return this.m_Vf1MeasureTime; }
            set { this.m_Vf1MeasureTime = value; }
        }

        [Category("Time")]
        [Description("Vf2 Inspect Delay TIme.")]
        public double Vf2MeasureTime
        {
            get { return this.m_Vf2MeasureTime; }
            set { this.m_Vf2MeasureTime = value; }
        }

        [Category("Time")]
        [Description("NTC Inspect Delay TIme.")]
        public double NTCMeasureTime
        {
            get { return this.m_NTCMeasureTime; }
            set { this.m_NTCMeasureTime = value; }
        }
        #endregion

        #region Method
        public void Init(Part part)
        {

        }
        #endregion

    }
}
