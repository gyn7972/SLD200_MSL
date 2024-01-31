using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class DieLoadSubstrate
    {
        #region Define
        [Serializable]
        public class ArrangementSpecification
        {
            #region Field
            /// <summary>
            /// Path Motion Parameter의 이름을 저장한다.
            /// </summary>
            private string m_Name;
            //private PathGeneratorParameter m_PathGeneratorParameter;
            #endregion

            #region Constructor
            public ArrangementSpecification()
            {

            }
            #endregion

            #region Property
            public string Name
            {
                get { return this.m_Name; }
                set { this.m_Name = value; }
            }

            //public PathGeneratorParameter PathGeneratorParameter
            //{
            //    get { return this.m_PathGeneratorParameter; }
            //    set { this.m_PathGeneratorParameter = value; }
            //}
            #endregion

            #region Method
            //internal int CreatePathGenerator(out TwoDimensionPathGenerator generator)
            //{
            //    int ret = 0;

            //    if ((ret = OnCreatePathGenerator(out generator)) != 0) return ret;

            //    return ret;
            //}

            //protected virtual int OnCreatePathGenerator(out TwoDimensionPathGenerator generator)
            //{
            //    int ret = 0;

            //    generator = null;
            //    // ((TwoDimensionPathGeneratorParameter)this.PathGeneratorParameter).CenterCoordinate = new XyCoordinate(0, 0);
            //    if ((ret = ((TwoDimensionPathGeneratorParameter)this.PathGeneratorParameter).CreatePathGenerator(out generator)) != 0) return ret;


            //    TwoDimensionPathGeneratorParameter parameter = generator.GetParameter();
            //    if (parameter is RectangleZigzagTwoDimensionPathGeneratorParameter)
            //    {
            //        ((RectangleZigzagTwoDimensionPathGeneratorParameter)parameter).CenterCoordinate = new XyCoordinate(0, 0);
            //    }
            //    else if (parameter is RectangleStripeTwoDimensionPathGeneratorParameter)
            //    {
            //        ((RectangleStripeTwoDimensionPathGeneratorParameter)parameter).CenterCoordinate = new XyCoordinate(0, 0);
            //    }
            //    if ((ret = generator.Generate(parameter)) != 0) return ret;
            //    return ret;
            //}
            #endregion

            //#region ISupportEditor
            //public abstract Control GetEditor(PathGeneratorParameterSpecificationKeyedCollection specification);
            //#endregion
        }

        [Serializable]
        public class ArrangementSpecificationCollection : Collection<ArrangementSpecification>
        {
        }

        [Serializable]
        public class SubMaterialSpecification
        {
            #region Field
            private SizeD m_Size;
            private SizeD m_Pitch;
            #endregion

            #region Constructor
            public SubMaterialSpecification()
            {
                this.Size = new SizeD();
                this.Pitch = new SizeD();
            }
            #endregion

            #region Property
            /// <summary>
            /// Substrate가 가진 SubMaterial의 크기를 지정하거나 가져온다
            /// </summary>
            public SizeD Size
            {
                get { return this.m_Size; }
                set { this.m_Size = value; }
            }

            public SizeD Pitch
            {
                get { return this.m_Pitch; }
                set { this.m_Pitch = value; }
            }
            #endregion
        }

        [Serializable]
        public class Specification
        {
            #region Field
            private SizeD m_WorkingArea;
            private RectangleD m_rectangleWorkingArea;
            private bool m_rectangleWorkingAreaSetCompleted;
            #endregion

            #region Constructor
            public Specification()
            {
                this.WorkingArea = new SizeD();
                this.rectangleWorkingArea = new RectangleD();
                this.rectangleWorkingAreaSetCompleted = false;
            }
            #endregion

            #region Property
            public SizeD WorkingArea
            {
                get { return this.m_WorkingArea; }
                set { this.m_WorkingArea = value; }
            }

            public RectangleD rectangleWorkingArea
            {
                get { return this.m_rectangleWorkingArea; }
                set { this.m_rectangleWorkingArea = value; }
            }

            public bool rectangleWorkingAreaSetCompleted
            {
                get { return this.m_rectangleWorkingAreaSetCompleted; }
                set { this.m_rectangleWorkingAreaSetCompleted = value; }
            }
            #endregion
        }

        /// <summary>
        /// Die를 공급하면서 발생하는 정보(등)을 정의한다.
        /// </summary>
        [Serializable]
        public class ProcessInformation
        {
            #region Field
            private XyCoordinate m_LatestPosition;
            private bool m_AvailableLatestPosition;
            private bool m_FirstScanDieEnqueueCompleted;
            #endregion

            #region Constructor
            public ProcessInformation()
            {
                this.LatestPosition = new XyCoordinate();
                this.AvailableLatestPosition = false;
                this.FirstScanDieEnqueueCompleted = false;
            }
            #endregion

            #region Property
            /// <summary>
            /// 가장 최근 공정 위치를 가져오거나 설정한다.
            /// </summary>
            public XyCoordinate LatestPosition
            {
                get { return this.m_LatestPosition; }
                set { this.m_LatestPosition = value; }
            }

            /// <summary>
            /// LatestPosition이 유효한지 여부를 가져오거나 설정한다.
            /// </summary>
            public bool AvailableLatestPosition
            {
                get { return this.m_AvailableLatestPosition; }
                set { this.m_AvailableLatestPosition = value; }
            }

            public bool FirstScanDieEnqueueCompleted
            {
                get { return this.m_FirstScanDieEnqueueCompleted; }
                set { this.m_FirstScanDieEnqueueCompleted = value; }
            }
            #endregion
        }

        #endregion

        #region Field
        private Specification m_SubstrateSpecification;

        private DieLoadSubstrate.ArrangementSpecification m_Arrangement;
        //private DieLoadSubstrateLoadStateMachine m_LoadStateMachine;

        private SubMaterialSpecification m_DieSpecification;

        //private TwoDimensionPathGenerator m_DieLoadPathGenerator;
        private ProcessInformation m_LoadInformation;
        public string ObjId { set; get; }
        #endregion

        #region Constructor
        public DieLoadSubstrate(string objId)
        {
            ObjId = objId;
            // This code will be called after SetDefaultValues()
        }
        public DieLoadSubstrate() : this("") { }
        #endregion

        #region Property
        /// <summary>
        /// WorkingArea와 같은 사양 데이터를 가져오거나 설정한다.
        /// </summary>
        public Specification SubstrateSpecification
        {
            get { return this.m_SubstrateSpecification; }
            set { this.m_SubstrateSpecification = value; }
        }

        /// <summary>
        /// PathGenerator와 같이 정렬이 필요한 데이터를 가져오거나 설정한다.
        /// </summary>
        public DieLoadSubstrate.ArrangementSpecification Arrangement
        {
            get { return this.m_Arrangement; }
            protected set { this.m_Arrangement = value; }
        }

        /// <summary>
        /// Die 공급 상태를 관리하는 상태 머신을 가져온다.
        /// </summary>
        //public DieLoadSubstrateLoadStateMachine LoadStateMachine
        //{
        //    get { return this.m_LoadStateMachine; }
        //    protected set { this.m_LoadStateMachine = value; }
        //}

        /// <summary>
        /// Die Size, Pitch와 같은 Die의 사양 데이터를 가져온다
        /// </summary>
        public SubMaterialSpecification DieSpecification
        {
            get { return this.m_DieSpecification; }
            protected set { this.m_DieSpecification = value; }
        }

        /// <summary>
        /// 작업 경로를 가져오거나 설정한다.
        /// </summary>
        //public TwoDimensionPathGenerator DieLoadPathGenerator
        //{
        //    get { return this.m_DieLoadPathGenerator; }
        //    private set { this.m_DieLoadPathGenerator = value; }
        //}

        /// <summary>
        /// Die 공급 진행 정보를 가져온다.
        /// </summary>
        public ProcessInformation LoadInformation
        {
            get { return this.m_LoadInformation; }
            private set { this.m_LoadInformation = value; }
        }
        #endregion

        #region Method
        public int ApplyArrangement(DieLoadSubstrate.ArrangementSpecification specification)
        {
            int ret = 0;

            if ((ret = this.OnApplyArrangement(specification)) != 0) return ret;
            this.Arrangement = specification;

            return ret;
        }
        protected virtual int OnApplyArrangement(DieLoadSubstrate.ArrangementSpecification specification)
        {
            int ret = 0;
            //TwoDimensionPathGenerator generator = null;

            //if (specification == null)
            //    throw new ArgumentNullException("specification");

            //if ((ret = specification.CreatePathGenerator(out generator)) != 0) return ret;
            //if ((generator as TwoDimensionPathGenerator) == null)
            //    throw new Exception("generator is null");

            //this.DieLoadPathGenerator = generator;
            //Log.Write("LoadPathGenerator", string.Format("LoadSubstrate Genrator PathCount {0}", this.DieLoadPathGenerator.Paths.Count));
            return ret;
        }

        /// <summary>
        /// 주어진 address에 해당하는 Load position을 반환한다.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public int GetLoadPosition(object address, ref XyCoordinate position)
        {
            int ret = 0;

            if ((ret = this.OnGetLoadPosition(address, ref position)) != 0) return ret;

            return ret;
        }

        public int GetNextLoadPosition(ref XyCoordinate position)
        {
            int ret = 0;
            object address = null;

            if ((ret = this.GetNextLoadAddress(out address)) != 0) return ret;
            if ((ret = this.GetLoadPosition(address, ref position)) != 0) return ret;
            return ret;
        }
        protected virtual int OnGetLoadPosition(object address, ref XyCoordinate position)
        {
            int ret = 0;

            return ret;
        }

        public int GetNextLoadAddress(out object address)
        {
            int ret = 0;
            if ((ret = this.OnGetNextLoadAddress(out address)) != 0) return ret;

            return ret;
        }

        protected virtual int OnGetNextLoadAddress(out object address)
        {
            int ret = 0;
            address = null;
            return ret;
        }

        public int GetCurrentLoadAddress(out object address)
        {
            int ret = 0;
            if ((ret = this.OnGetCurrentLoadAddress(out address)) != 0) return ret;
            return ret;
        }

        protected virtual int OnGetCurrentLoadAddress(out object address)
        {
            int ret = 0;
            address = null;
            return ret;
        }

        protected virtual int OnSetObjId(string objId)
        {
            int ret = 0;

            ObjId = objId;

            return ret;
        }
        public int SetObjId(string objId)
        {
            int ret = 0;

            if ((ret = this.OnSetObjId(objId)) != 0) return ret;

            return ret;
        }
        #endregion

        #region Material Members
        //protected override void SetDefaultValues()
        //{
        //    base.SetDefaultValues();

        //    this.SubstrateSpecification = new Specification();
        //    this.LoadStateMachine = new DieLoadSubstrateLoadStateMachine(this);
        //    this.LoadStateMachine.Activate();

        //    this.LoadInformation = new DieLoadSubstrate.ProcessInformation();
        //}
        #endregion
    }
}
