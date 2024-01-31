using QMC.Common.Hmi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    #region RevisionRecipe
    public class RevisionRecipe
    {
        #region Field
        private double m_dXSpec;
        private double m_dYSpec;
        private double m_dAngleTolerance;
        private int m_dSleep;
        private int m_dCutCount;
        private int m_dAvgCount;
        #endregion

        #region Property
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
        public IlluminationDataSet IlluminationBottomDataSet { set; get; }
        [Browsable(false)]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public IlluminationDataSet IlluminationMounterDataSet { set; get; }

        [Browsable(false)]
        [TypeConverter(typeof(NormalExpandableObjectConverter))]
        public PatternMatchingParameters PatternMatchingParameter { set; get; }
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

        public bool m_UseColletCenter;
        [Category("Collet Parameter")]
        public bool UseColletCenter
        {
            get
            {
                return m_UseColletCenter;
            }
            set
            {
                m_UseColletCenter = value;
            }
        }
        public double XSpec
        {
            get { return m_dXSpec; }
            set { m_dXSpec = value; }
        }
        public double YSpec
        {
            set { m_dYSpec = value; }
            get { return m_dYSpec; }
        }
        public double AngleTolerance
        {
            get { return m_dAngleTolerance; }
            set { m_dAngleTolerance = value; }
        }
        public int Sleep
        {
            get { return m_dSleep; }
            set { m_dSleep = value; }
        }
        public int CutCount
        {
            get { return m_dCutCount; }
            set { m_dCutCount = value; }
        }
        public int AvgCount
        {
            set { m_dAvgCount = value; }
            get { return m_dAvgCount; }
        }

        private int m_ThresHold;
        public int Threshold
        {
            get { return m_ThresHold; }
            set { m_ThresHold = value; }
        }
        #endregion

        #region Consttructor
        public RevisionRecipe(Part part)
        {
            this.InitData(part);
        }
        #endregion

        #region Method
        public void InitData(Part part)
        {
            if (PatternMatchingParameter == null)
                PatternMatchingParameter = new PatternMatchingParameters();

            if (TrainRoiStartLocation == null)
                TrainRoiStartLocation = new Point();

            if (TrainRoiEndLocation == null)
                TrainRoiEndLocation = new Point();

            if (InspectRoiStartLocation == null)
                InspectRoiStartLocation = new Point();

            if (InspectRoiEndLocation == null)
                InspectRoiEndLocation = new Point();

            if (IlluminationBottomDataSet == null)
                IlluminationBottomDataSet = new IlluminationDataSet(part.Name +"_Bottom");

            if (IlluminationMounterDataSet == null)
                IlluminationMounterDataSet = new IlluminationDataSet(part.Name + "_Mounter");

            //ColletSize = 0;
            //ChipThres = 0;
            //EdgeAvgCount = 0;
            //ColletTolerenceX = 0;
            //ColletTolerenceX = 0;
            //HighPassFilterOffset = 0;
            //TartgetSizeWidth = 0;
            //TartgetSizeHight = 0;
            SaveFaildImage = false;
            SaveRevisionImage = false;
            //ShowEnableInspectionImage = false;
            UsePatternMatching = false;
        }
        #endregion
    }
    #endregion

}
