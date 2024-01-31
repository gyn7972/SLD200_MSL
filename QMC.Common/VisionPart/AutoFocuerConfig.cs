using QMC.Common.Hmi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.VisionPart
{
    [Serializable]
    public enum FocusingDirection
    {
        Plus,
        Minus,
    }
    [Serializable]
    public class AutoFocuserConfig
    {
        public double FocusMargin { set; get; }
        public int FocusCount { set; get; }
        public FocusingDirection Direction { set; get; }
        [Browsable(false)]
        public double CenterPosition { set; get; }
        [Browsable(false)]
        public bool EnabledMoveBestFocusPosition { set; get; }
        [Browsable(false)]
        public bool EnabledSecondStep { set; get; }
        public double MinimumPitch { set; get; }
        [Browsable(false)]
        public double FocusPosition { set; get; }
        [Browsable(false)]
        public AutoFocusResult Result { get; set; }
        public double FocusStartPosition { set; get; }

        public int Threshold { get; set; }

        public int BackGroundThreshold { get; set; }

        public AutoFocuserConfig()
        {
            FocusMargin = 0.05;
            Direction = FocusingDirection.Plus;
            FocusCount = 50;
            CenterPosition = 0;
            FocusStartPosition = 6;
            EnabledMoveBestFocusPosition = true;
            if (Result == null)
            {
                //Result = new List<AutoFocusResult>();
                //for (int i = 0; i < 6; i++)
                //{
                //    AutoFocusResult result = new AutoFocusResult(0, 0, null, 0);
                //    Result.Add(result);
                //}

                AutoFocusResult result = new AutoFocusResult(0, 0, null, 0);
                Result = result;
            }

            Init();
        }

        public void Init()
        {
            //if (FocusPosition == null)
            //{
            //    FocusPosition = new List<double>();
            //    for (int i = 0; i < 6; i++)
            //    {
            //        double dValue = 0.0;
            //        FocusPosition.Add(dValue);
            //    }

                
            //}
            FocusPosition = 0.0;
            Threshold = 100;
            BackGroundThreshold = 100;
        }
    }
}
