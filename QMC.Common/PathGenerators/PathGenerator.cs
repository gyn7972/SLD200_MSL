using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace QMC.Common.PathGenerators
{
    #region PathGenerator
    public abstract class PathGenerator
    {
        #region Define
        /// <summary>
        /// Path가 생성되는 방법
        /// </summary>
        [Serializable]
        public enum PathType
        {
            /// <summary>
            /// 방향 전환되는 경로만 생성
            /// </summary>
            Continuous,
            /// <summary>
            /// Step 별로 경로를 생성
            /// </summary>
            StepByStep,
        }

        /// <summary>
        /// Path를 생성하기 위한 기준
        /// </summary>
        [Serializable]
        public enum ReferenceType
        {
            // 점을 기준으로 Path를 생성.
            Point,
            // 면적을 기준으로 Path를 생성.
            Area
        }

        [Serializable]
        public class PathReadOnlyCollection<TPosition> : ReadOnlyCollection<TPosition>,
            IPathList<TPosition>
        {
            #region Field
            private int m_CurrentPathIndex;

            #endregion

            #region Constructor
            public PathReadOnlyCollection(IList<TPosition> list) : base(list)
            {
                this.CurrentPathIndex = 0;
            }
            public PathReadOnlyCollection(TPosition position) : this(new TPosition[] { position }) { }
            public PathReadOnlyCollection() : this(new TPosition[0]) { }
            #endregion

            #region Property
            public int CurrentPathIndex
            {
                get { return this.m_CurrentPathIndex; }
                private set { this.m_CurrentPathIndex = value; }
            }
            #endregion

            #region Method
            public void SetCurrentPathIndex(int pathindex)
            {
                this.CurrentPathIndex = pathindex;
            }
            #endregion

            #region IPathList<TPosition> Members
            /// <summary>
            /// 다음 위치의 경로로 이동한다.
            /// </summary>
            public virtual TPosition MoveNext()
            {
                if (this.Count == 0)
                    throw new ArgumentOutOfRangeException("Path");

                if (this.CurrentPathIndex < this.Count)
                    this.CurrentPathIndex++;

                if (this.CurrentPathIndex < this.Count)
                    return this[this.CurrentPathIndex];
                else
                    return this[this.Count - 1];
            }

            /// <summary>
            /// 이전 위치의 경로로 이동한다.
            /// </summary>
            public TPosition MovePrevious()
            {
                if (this.Count == 0 || --this.CurrentPathIndex < 0)
                    throw new ArgumentOutOfRangeException("Path");

                return this[this.CurrentPathIndex];
            }

            /// <summary>
            /// 처음 위치의 경로로 이동한다.
            /// </summary>
            public TPosition MoveFirst()
            {
                if (this.Count == 0)
                    throw new ArgumentOutOfRangeException("Path");

                this.CurrentPathIndex = 0;

                return this[this.CurrentPathIndex];
            }

            /// <summary>
            /// 마지막 위치의 경로로 이동한다.
            /// </summary>
            public TPosition MoveLast()
            {
                if (this.Count == 0)
                    throw new ArgumentOutOfRangeException("Path");

                this.CurrentPathIndex = this.Count - 1;

                return this[CurrentPathIndex];
            }

            /// <summary>
            /// 처음 위치의 경로를 준다.
            /// </summary>
            public TPosition GetFirst()
            {
                if (this.Count == 0)
                    throw new ArgumentOutOfRangeException("Path");

                return this[0];
            }

            /// <summary>
            /// 마지막 위치의 경로를 준다.
            /// </summary>
            public TPosition GetLast()
            {
                if (this.Count == 0)
                    throw new ArgumentOutOfRangeException("Path");

                return this[this.Count - 1];
            }

            /// <summary>
            /// 현재 위치의 경로를 준다.
            /// </summary>
            public TPosition GetCurrent()
            {
                if (this.Count == 0)
                    throw new ArgumentOutOfRangeException("Path");

                return this[this.CurrentPathIndex];
            }

            /// <summary>
            /// 생성된 경로의 작업을 완료 유/무를 판단한다.
            /// </summary>
            /// <returns>CurrentPathIndex가 생성된 PathCount와 동일하면 ture값을 리턴한다.</returns>
            public bool IsEndOfList()
            {
                bool ret = false;

                if (this.Count <= this.CurrentPathIndex)
                    return true;

                return ret;
            }
            #endregion

            #region IPathList Members
            object IPathList.MoveNext()
            {
                return this.MoveNext();
            }

            object IPathList.MovePrevious()
            {
                return this.MovePrevious();
            }

            object IPathList.MoveFirst()
            {
                return this.MoveFirst();
            }

            object IPathList.MoveLast()
            {
                return this.MoveLast();
            }

            object IPathList.GetFirst()
            {
                return this.GetFirst();
            }

            object IPathList.GetLast()
            {
                return this.GetLast();
            }

            object IPathList.GetCurrent()
            {
                return this.GetLast();
            }
            #endregion
        }
        #endregion

        #region Field
        private PathGeneratorParameter m_Parameter;
        private IPathList m_Paths;
        private string m_Name;
        #endregion

        #region Constructor
        public PathGenerator(string name)
        {
            //this.Parameter = Activator.CreateInstance<PathGeneratorParameter>();

            this.Name = name;
        }
        public PathGenerator() : this("") { }
        #endregion

        #region Property
        /// <summary>
        /// 생성된 Path들을 가져온다.
        /// </summary>
        public IPathList Paths
        {
            get { return this.m_Paths; }
            protected set { this.m_Paths = value; }
        }

        protected PathGeneratorParameter Parameter
        {
            get { return this.m_Parameter; }
            set { this.m_Parameter = value; }
        }

        /// <summary>
        /// Path Generator의 이름을 가져온다.
        /// </summary>
        public string Name
        {
            get { return this.m_Name; }
            private set { this.m_Name = value; }
        }
        #endregion

        #region Method
        /// <summary>
        /// Motion의 경로를 생성한다.
        /// </summary>
        /// <param name="paramter"></param>
        /// <returns></returns>
        public int Generate(PathGeneratorParameter paramter)
        {
            int ret = 0;

            this.m_Parameter = paramter;

            if ((ret = this.OnGenerate(paramter)) != 0) return ret;

            return ret;
        }

        protected abstract int OnGenerate(PathGeneratorParameter paramter);

        /// <summary>
        /// Path 생성시 사용할 Paramter를 가져온다.
        /// </summary>
        //public PathGeneratorParameter GetParameter()
        //{
        //    return CopyUtility.GetDeepCopy(this.m_Parameter) as PathGeneratorParameter;
        //}
        #endregion
    }
    #endregion

    #region PathGeneratorParameter
    [Serializable]
    public abstract class PathGeneratorParameter
    {
        #region Field
        public const string DefaultName = "Default";

        private PathGenerator.PathType m_PathType;
        private PathGenerator.ReferenceType m_ReferenceType;
        private SizeD m_ObjectSize;
        private bool m_InvertedX;
        private bool m_InvertedY;
        #endregion

        #region Constructor
        public PathGeneratorParameter()
        {
            this.PathType = PathGenerator.PathType.StepByStep;
            this.ReferenceType = PathGenerator.ReferenceType.Point;
            this.ObjectSize = new SizeD(0, 0);
            this.InvertedX = false;
            this.InvertedY = false;
        }
        #endregion

        #region Property
        /// <summary>
        /// 생성될 경로의 Type를 가져오거나 설정한다.
        /// </summary>
        public PathGenerator.PathType PathType
        {
            get { return this.m_PathType; }
            set { this.m_PathType = value; }
        }

        /// <summary>
        /// 생성될 경로의 기준을 가져오거나 설정한다.
        /// </summary>
        public PathGenerator.ReferenceType ReferenceType
        {
            get { return this.m_ReferenceType; }
            set { this.m_ReferenceType = value; }
        }

        /// <summary>
        /// Object의 Size를 가져오거나 설정한다. ( FOV, Laser Beam Size 등 )
        /// </summary>
        public SizeD ObjectSize
        {
            get { return this.m_ObjectSize; }
            set { this.m_ObjectSize = value; }
        }

        public bool InvertedX
        {
            get { return this.m_InvertedX; }
            set { this.m_InvertedX = value; }
        }

        public bool InvertedY
        {
            get { return this.m_InvertedY; }
            set { this.m_InvertedY = value; }
        }
        #endregion

        #region Method
        /// <summary>
        /// 지원하는 Path Generator의 형식을 가져온다.
        /// </summary>
        /// <returns></returns>
        protected abstract Type OnGetPathGeneratorType();

        public int CreatePathGenerator(string name, out PathGenerator generator)
        {
            int ret = 0;

            generator = Activator.CreateInstance(this.OnGetPathGeneratorType(), name) as PathGenerator;
            // To Do: parameter가 완료되지 않은 상태에서 호출하면 generate를 할 수 없어서 실패를 리턴한다.
            generator.Generate(this);

            return ret;
        }

        public int CreatePathGenerator(out PathGenerator generator)
        {
            return this.CreatePathGenerator(PathGeneratorParameter.DefaultName, out generator);
        }

        /// <summary>
        /// PathGenerator Parameter의 기본 컨트롤을 가져온다.
        /// </summary>
        /// <returns></returns>
        //   public PathGeneratorControl GetControl()
        //   {
        //       return this.OnGetControl(this);
        //   }

        // protected abstract PathGeneratorControl OnGetControl(PathGeneratorParameter parameter);
        #endregion

        #region Object Members
        public override string ToString()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine("PathGeneratorParameter");
            sw.WriteLine("\tPathType = {0}", this.PathType);
            sw.WriteLine("\tReferenceType = {0}", this.ReferenceType);
            sw.WriteLine("\tObjectSize = {0}", this.ObjectSize);

            return sw.ToString();
        }
        #endregion
    }
    #endregion

    #region PathGeneratorCollection
    public class PathGeneratorCollection : Collection<PathGenerator>
    {

    }
    #endregion

    #region IPathList
    public interface IPathList
    {
        object MoveNext();
        object MovePrevious();

        object MoveFirst();
        object MoveLast();

        object GetFirst();
        object GetLast();

        object GetCurrent();

        bool IsEndOfList();

        int Count { get; }
    }

    public interface IPathList<TPosition> : IPathList
    {
        new TPosition MoveNext();
        new TPosition MovePrevious();

        new TPosition MoveFirst();
        new TPosition MoveLast();

        new TPosition GetFirst();
        new TPosition GetLast();

        new TPosition GetCurrent();
    }
    #endregion

    [Serializable]
    public class PathGeneratorParameterSpecification
    {
        #region Field
        private string m_Name;
        #endregion

        #region Constructor
        public PathGeneratorParameterSpecification()
        {
            this.Name = "";
        }
        #endregion

        #region Property
        public string Name
        {
            get { return this.m_Name; }
            set { this.m_Name = value; }
        }


        #endregion

        #region Method
        /// <summary>
        /// PathGeneratorParameter instance를 생성하여 반환한다
        /// </summary>
        /// <returns></returns>

        #endregion
    }

    [Serializable]
    public class PathGeneratorParameterSpecificationKeyedCollection : KeyedCollection<string, PathGeneratorParameterSpecification>
    {
        public PathGeneratorParameterSpecificationKeyedCollection()
        {
        }

        protected override string GetKeyForItem(PathGeneratorParameterSpecification item)
        {
            return item.Name;
        }
    }
}
