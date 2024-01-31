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
    public class MaterialCompansatorRecipe
    {

        private double m_StartPositionX;
        private double m_StartPositionY;

        private bool m_bInveterX;
        private bool m_bInveterY;

        private XyCoordinateCollection m_MatrialPos;
        private XyCoordinateCollection m_OffsetData;
        public List<MaterialPitch> XGridValue { set; get; }
        public List<MaterialPitch> YGridValue{ set; get; }

        public IlluminationDataList IlluminationDataSets { set; get; }
        public IlluminationDataSet IlluminationDataSet { set; get; }
        [Browsable(false)]
        public Point InspectRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point InspectRoiEndLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiStartLocation { set; get; }
        [Browsable(false)]
        public Point TrainRoiEndLocation { set; get; }

        public PatternMatchingParameters PatternMatchingParameter { set; get; }

        public MaterialCompansatorRecipe(Part part)
        {
            IlluminationDataSets = new IlluminationDataList();
            IlluminationDataSet = new IlluminationDataSet(part.Name);
            InspectRoiStartLocation = new Point();
            InspectRoiEndLocation = new Point();
            TrainRoiStartLocation = new Point();
            TrainRoiEndLocation = new Point();

            Init();

            m_StartPositionX = 0;
            m_StartPositionY = 0;
            m_MatrialPos = new XyCoordinateCollection();

            m_bInveterX = false;
            m_bInveterY = false;

        }
        public void Init()
        {
            if (this.PatternMatchingParameter == null)
                PatternMatchingParameter = new PatternMatchingParameters();
            
            if(XGridValue == null)
            {
                XGridValue = new List<MaterialPitch>();
            }
            if (YGridValue == null)
            {
                YGridValue = new List<MaterialPitch>();
            }

        }


        public bool InvertedX
        {
            get { return m_bInveterX; }
            set { m_bInveterX = value; }
        }
        public bool InvertedY
        {
            get { return m_bInveterY; }
            set { m_bInveterY = value; }
        }
        public XyCoordinateCollection MatrialPos
        {
            get { return m_MatrialPos; }
            set { m_MatrialPos = value; }
        }
    
        public double StartPositionX
        {
            get { return this.m_StartPositionX; }
            set { this.m_StartPositionX = value; }
        }

        public double StartPositionY
        {
            get { return this.m_StartPositionY; }
            set { this.m_StartPositionY = value; }
        }

    }
}
