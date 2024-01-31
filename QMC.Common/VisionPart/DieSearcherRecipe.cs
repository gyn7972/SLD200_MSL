using QMC.Common.Hmi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common;

namespace QMC.Common.VisionPart
{

    [Serializable]
    public class DieSearcherRecipe
    {
        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }
        [Browsable(false)]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public IlluminationDataSet IlluminationDataSet { set; get; }
        [Browsable(false)]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public PatternMatchingParameters PatternMatchingParameter { set; get; }


        private int m_ColletSize;
        [Category("Collet Parameter")]
        public int ColletSize 
        {
            get
            {
                return m_ColletSize;
            }
            set
            {
                m_ColletSize = value;
            }
        }

        private int m_ChipThres;
        [Category("Collet Parameter")]
        public int ChipThres 
        { 
            get
            {
                return m_ChipThres;
            }
            set
            {
                m_ChipThres = value;
            }
        }

        private int m_EdgeAvgCount;
        [Category("Collet Parameter")]
        public int EdgeAvgCount 
        { 
            get
            {
                return m_EdgeAvgCount;
            }
            set
            {
                m_EdgeAvgCount = value;
            }
        }
        private double m_ColletTolerenceX;
        [Category("Collet Parameter")]
        public double ColletTolerenceX 
        { 
            get
            {
                return m_ColletTolerenceX;
            }
            set
            {
                m_ColletTolerenceX = value;
            }
        }

        private double m_HighPassFilterOffset;
        [Category("Collet Parameter")]
        public double HighPassFilterOffset 
        { 
            get
            {
                return m_HighPassFilterOffset;
            }
            set
            {
                m_HighPassFilterOffset = value;
            }
        }
        public bool m_ShowEnableInspectionImage;
        [Category("Collet Parameter")]
        public bool ShowEnableInspectionImage 
        { 
            get
            {
                return m_ShowEnableInspectionImage;
            }
            set
            {
                m_ShowEnableInspectionImage = value;
            }
        }
        public bool m_UsePatternMatching;
        [Category("Collet Parameter")]
        public bool UsePatternMatching 
        { 
            get
            {
                return m_UsePatternMatching;
            }
            set
            {
                m_UsePatternMatching = value;
            }
        }

        public double m_TartgetSizeWidth;
        [Category("Target Size")]
        public double TartgetSizeWidth 
        { 
            get
            {
                return m_TartgetSizeWidth;
            }
            set
            {
                m_TartgetSizeWidth = value;
            }
        }

        public double m_TartgetSizeHight;
        [Category("Target Size")]
        public double TartgetSizeHight 
        { 
            get
            {
                return m_TartgetSizeHight;
            }
            set
            {
                m_TartgetSizeHight = value;
            }
        }

        public bool m_SaveFaildImage;
        [Category("Image Save")]
        public bool SaveFaildImage 
        {
            get
            {
                return m_SaveFaildImage;
            }
            set
            {
                m_SaveFaildImage = value;
            }
        }
        public bool m_SaveRevisionImage;
        [Category("Image Save")]
        public bool SaveRevisionImage 
        { 
            get
            {
                return m_SaveRevisionImage;
            }
            set
            {
                m_SaveRevisionImage = value;
            }
        }



        public DieSearcherRecipe(Part part)
        {
            InitData(part);
        }
        public void InitData(Part part)
        {
            InspectRoiStartLocation = new Point();
            InspectRoiEndLocation = new Point();
            TrainRoiStartLocation = new Point();
            TrainRoiEndLocation = new Point();
            IlluminationDataSet = new IlluminationDataSet(part.Name);
            if (PatternMatchingParameter == null)
                PatternMatchingParameter = new PatternMatchingParameters();
            ColletSize = 0;
            ChipThres = 0;
            EdgeAvgCount = 0;
            ColletTolerenceX = 0;
            ColletTolerenceX = 0;
            HighPassFilterOffset = 0;
            TartgetSizeWidth = 0;
            TartgetSizeHight = 0;
            SaveFaildImage = false;
            SaveRevisionImage = false;
            ShowEnableInspectionImage = false;
            UsePatternMatching = false;

            
        }
    }
}
