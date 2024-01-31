using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public delegate void ChangedTurretIndex();
    public class Turret : MotionPart
    {
        public enum MotionKey
        {
            Turret,
        }
        
        public enum PoistionKey
        {
            LoadZ,
            RevisionPart,
            Empty1,
            UnloadZ,
            Empty2,
            TrashCan,
        }

        protected List<int> m_ListTurretPositions;

        public event ChangedTurretIndex OnChangedTurretIndex;
        public TurretConfig Config { get; set; }
        public int TurretArmCount 
        { 
            set
            {
                Config.TurretArmCount = value;
            }
            get
            {
                return Config.TurretArmCount;
            }
        }
        public int TurretMovementAngle
        {
            get
            {
                return Config.TurretMovementAngle;
            }
            set
            {
                Config.TurretMovementAngle = value;
            }
        }
        public List<Collet> Collets { get; set; }

        public List<string> TurretPositions
        {
            get { return Config.listTurretPositionProperty; }
            set { Config.listTurretPositionProperty = value; }
        }

        public List<int> TurretArmPositions 
        { 
            get
            {
                double dPosition = GetCurrentCommandPosition(MotionKey.Turret.ToString());
                int nArm0Index = (int)dPosition / TurretMovementAngle;
                int nArm = 0;
                for(int i = 0; i < TurretArmCount; i++)
                {
                    nArm = (i - nArm0Index + TurretArmCount) % TurretArmCount + 1;
                    if (i >= m_ListTurretPositions.Count)
                        m_ListTurretPositions.Add(nArm);
                    else
                        m_ListTurretPositions[i] = nArm;
                }

                return m_ListTurretPositions;
            }
        }

        public Turret(string strName) : base(strName)
        {
            Config = new TurretConfig();
            TurretArmCount = 6;

            m_ListTurretPositions = new List<int>();
        }

        public override int Create()
        {
            int ret = base.Create();

            m_dicAxes.Clear();
            foreach (MotionKey key in Enum.GetValues(typeof(MotionKey)))
            {
                m_dicAxes.Add(key.ToString(), null);
            }

            m_dicAxisDisplayType.Clear();
            m_dicAxisDisplayType.Add(MotionKey.Turret.ToString(), DisplayAxisType.Theta);

            return ret;
        }



        public override void Close()
        {
            base.Close();
        }

        public override void UpdateConfigData() //참고 : Override
        {
            DieTransfer dieTransfer = Owner as DieTransfer;
            if (Owner != null) 
            {
                Config = dieTransfer.Config.TurretConfig;
                if (Config.ColletZOffsets == null) //참고 : 저장안될때.. Data null인지 꼭 확인
                    Config.ColletZOffsets = new List<ColletZOffset>();

                if (Config.CurrentTurretPosition == null)
                    Config.CurrentTurretPosition = new List<int>();

                if (Config.listTurretPositionProperty == null)
                { 
                    Config.listTurretPositionProperty = new List<string>();
                
                }
                IntitTurretProperty();
            }
        }

        public void IntitTurretProperty()
        {
            string[] strNameArr = new string[] { "LoadZ", "RevisionPart", "Empty", "UnloadZ", "Empty", "AirBlowDie" };
            int[] nIndexArr = new int[] { 1, 2, 3, 4, 5, 6 };
            TurretPositions.Clear();
            for (int i = 0; i < TurretArmCount; i++)
            {
                string tempProperty = "";
                tempProperty = strNameArr[i];
                TurretPositions.Add(tempProperty);
            }
        }

        public int MoveNext()
        {
            int ret = 0;

            double dStepAngle = 0;
            if (IsWorkingPosition())
            {
                dStepAngle = Config.TurretMovementAngle;
            }
            else
            {
                dStepAngle = Config.TurretMovementAngle / 2;                
            }
            string strMotionKey = MotionKey.Turret.ToString();
            Dictionary<string, MovingProjection> projections = new Dictionary<string, MovingProjection>();
            MovingProjection projection = GetDefaultMovingProjection(strMotionKey);
            if (projection != null)
            {
                projection.Position = GetCurrentCommandPosition(strMotionKey) + dStepAngle;
                projections.Add(strMotionKey, projection);

                if((ret = Move(projections)) != 0) return ret;

                if (OnChangedTurretIndex != null)
                    OnChangedTurretIndex();
            }


            return ret;
        }

        public int MoveWorking(bool bOn)
        {
            int ret = 0;

            double dStepAngle = Config.TurretMovementAngle / 2;
            string strMotionKey = MotionKey.Turret.ToString();
            Dictionary<string, MovingProjection> projections = new Dictionary<string, MovingProjection>();
            MovingProjection projection = GetDefaultMovingProjection(strMotionKey);
            if (projection != null)
            {
                if (bOn)
                {
                    projection.Position = GetCurrentCommandPosition(strMotionKey) - dStepAngle;
                }
                else
                {
                    projection.Position = GetCurrentCommandPosition(strMotionKey) + dStepAngle;
                }
                
                projections.Add(strMotionKey, projection);

                if ((ret = Move(projections)) != 0) return ret;
            }

            return ret;
        }

        public bool IsWorkingPosition()
        {
            bool ret = false;
            string strMotionKey = MotionKey.Turret.ToString();
            double dStepAngle = Config.TurretMovementAngle;
            double dCurrentPosition = GetCurrentCommandPosition(strMotionKey);

            if(dCurrentPosition % dStepAngle <= 1)
            {
                ret = true;
            }


            return ret;
        }

        public List<int> ColletList()
        {
            List<int> list = new List<int>();



            return list;
        }

        protected override int OnAfterMove(Dictionary<string, MovingProjection> dicMovingProjection)
        {
            string strMotionKey = MotionKey.Turret.ToString();
            double dPosition = GetCurrentActualPosition(strMotionKey);
            if (Math.Abs(dPosition - 360) <= 0.01)
            {
                double dZeroPosition = dPosition - 360;
                SetPosition(dZeroPosition, strMotionKey);
            }
            return 0;
        }

        public void ReadCurrentTurretPosition()
        {
            //Axes["Turret"].Motor.ActualPosition = 120;
            //double dActualPos_T_Axis = Axes["Turret"].Motor.ActualPosition;
            //if (dActualPos_T_Axis >= 0 && dActualPos_T_Axis < 60 )
            //{
            //    int[] arr = new int[] { 1, 2, 3, 4, 5, 6 };
            //    List<int> listTemp = new List<int>(arr);
            //    Config.CurrentTurretPosition = listTemp;
            //}
            //else if (dActualPos_T_Axis >= 60 && dActualPos_T_Axis < 120)
            //{
            //    int[] arr = new int[] { 6, 1, 2, 3, 4, 5 };
            //    List<int> listTemp = new List<int>(arr);
            //    Config.CurrentTurretPosition = listTemp;
            //}
            //else if (dActualPos_T_Axis >= 120 && dActualPos_T_Axis < 180)
            //{
            //    int[] arr = new int[] { 5, 6, 1, 2, 3, 4 };
            //    List<int> listTemp = new List<int>(arr);
            //    Config.CurrentTurretPosition = listTemp;
            //}
            //else if (dActualPos_T_Axis >= 180 && dActualPos_T_Axis < 240)
            //{
            //    int[] arr = new int[] { 4, 5, 6, 1, 2, 3 };
            //    List<int> listTemp = new List<int>(arr);
            //    Config.CurrentTurretPosition = listTemp;
            //}
            //else if (dActualPos_T_Axis >= 240 && dActualPos_T_Axis < 300)
            //{
            //    int[] arr = new int[] { 3, 4, 5, 6, 1, 2 };
            //    List<int> listTemp = new List<int>(arr);
            //    Config.CurrentTurretPosition = listTemp;
            //}
            //else if (dActualPos_T_Axis >= 300 && dActualPos_T_Axis < 360)
            //{
            //    int[] arr = new int[] { 2, 3, 4, 5, 6, 1 };
            //    List<int> listTemp = new List<int>(arr);
            //    Config.CurrentTurretPosition = listTemp;
            //}
            //else { }
        }

        public int GetColletIndex(PoistionKey position)
        {
            int index = -1;
            int nNo = (int)position;
            index = TurretArmPositions[nNo];

            return index;
        }

        public int Clamp(int ColletNo)
        {
            int result = 0;
            int nIndex = ColletNo - 1;
            if (Collets != null && Collets.Count == TurretArmCount && nIndex < Collets.Count)
            {
                result = Collets[nIndex].Hold();
            }

            return result;
        }

        public int UnClamp(int ColletNo)
        {
            int result = 0;
            int nIndex = ColletNo - 1;
            if (Collets != null && Collets.Count == TurretArmCount && nIndex < Collets.Count)
            {
                result = Collets[nIndex].Release();
            }

            return result;
        }

        public bool IsClamp(int ColletNo)
        {
            bool ret = false;
            int nIndex = ColletNo - 1;
            if (Collets != null && Collets.Count == TurretArmCount && nIndex < Collets.Count)
            {
                ret = Collets[nIndex].IsHold();
            }

            return ret;
        }


    }
}
