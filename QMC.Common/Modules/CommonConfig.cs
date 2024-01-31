using QMC.Common.Hmi;
using QMC.Common.Vision.Optics.Leesos;
using QMC.Common.Opticon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Modules
{
    #region CommonConfig
    [Serializable]
    public class CommonConfig
    {
        [Category("Illuminator")]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public DigitalIlluminatorConfig IlluminatorConfig { set; get; }

        /*[Category("Barcode")]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public OpticonBarcodeReaderConfig BarcodeReaderConfig { set; get; }*/

        public CommonConfig()
        {
            Init();
        }

        protected MesInfo m_MesInfo;
        public MesInfo MesInfo
        {
            set { m_MesInfo = value; }
            get { return m_MesInfo; }
        }
        public bool UseMesInterlock { get; set; }
        public void Init()
        {
            if (IlluminatorConfig == null)
                IlluminatorConfig = new DigitalIlluminatorConfig();


            //if (m_MesInfo == null)
            //    m_MesInfo = new MesInfo();
            //if (BarcodeReaderConfig == null)
            //    BarcodeReaderConfig = new OpticonBarcodeReaderConfig();
        }
    }
    #endregion
}
