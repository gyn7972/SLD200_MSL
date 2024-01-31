using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    [Serializable]
    public class TurretConfig
    {
        [Serializable]
        public enum MotionKey
        {
            Turret,
        }
        public List<ColletZOffset> ColletZOffsets { get; set; }
        public int TurretArmCount { set; get; }
        public int RetryCount { set; get; }
        public List<int> CurrentTurretPosition { set; get; }

        public int TurretMovementAngle { get; set; }

        public List<string> listTurretPositionProperty { set; get; }
        public TurretConfig()
        {
            TurretArmCount = 6;
            RetryCount = 1;
            ColletZOffsets = new List<ColletZOffset>();
            CurrentTurretPosition = new List<int>();
            TurretMovementAngle = 60;
            listTurretPositionProperty = new List<string>();
        }
    }
}
