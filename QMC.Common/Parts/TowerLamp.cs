using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class TowerLamp : Part
    {
        public enum DioPointKey
        {
            Output_TowerLamp_Red,
            Output_TowerLamp_Yellow,
            Output_TowerLamp_Green,
            Output_Buzzer,


            //  LED Bar Lamp Bar

            /// #1 호기 - 시작
            ///
            Output_Lamp0_Red,                   //  Y007
            Output_Lamp0_Green,                 //  Y008
            Output_Lamp0_Blue,                  //  Y009
            ///
            /// #1 호기 - 끝


            /// #2 ~ #6 호기 - 시작
            ///
            Output_Lamp4,                       //  Y007
            Output_Lamp5,                       //  Y008
            ///
            /// #2 ~ #6 호기 - 끝


            Output_Lamp1_Red,                   //  Y010
            Output_Lamp1_Green,                 //  Y011
            Output_Lamp1_Blue,                  //  Y012
            Output_Lamp2_Red,                   //  Y013
            Output_Lamp2_Green,                 //  Y014
            Output_Lamp2_Blue,                  //  Y015
            Output_Lamp3_Red,                   //  Y016
            Output_Lamp3_Green,                 //  Y017
            Output_Lamp3_Blue,                  //  Y018
        }

        public TowerLamp(string strName) : base(strName)
        {
            if (m_dicDioPoints == null)
                m_dicDioPoints = new Dictionary<string, DioPoint>();

            m_dicDioPoints.Clear();

            foreach (DioPointKey key in Enum.GetValues(typeof(DioPointKey)))
            {
                m_dicDioPoints.Add(key.ToString(), null);
            }
        }

        public virtual int Is_Red_On()
        {
            int ret = 0;
            DioPoint dioLamp_Red = m_dicDioPoints[DioPointKey.Output_TowerLamp_Red.ToString()];

            if (dioLamp_Red == null)
            {
                return ret;
            }

            ret = (int)dioLamp_Red.GetValue();

            return ret;
        }

        public virtual int Red_On()
        {
            int ret = 0;
            DioPoint dioLamp_Red = m_dicDioPoints[DioPointKey.Output_TowerLamp_Red.ToString()];

            if (dioLamp_Red == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Red.Write(DioValue.On)) != 0)
            {
                return ret;
            }
            return ret;
        }

        public virtual int Red_Off()
        {
            int ret = 0;
            DioPoint dioLamp_Red = m_dicDioPoints[DioPointKey.Output_TowerLamp_Red.ToString()];

            if (dioLamp_Red == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Red.Write(DioValue.Off)) != 0)
            {
                return ret;
            }
            return ret;
        }

        public virtual int Is_Yellow_On()
        {
            int ret = 0;
            DioPoint dioLamp_Yellow = m_dicDioPoints[DioPointKey.Output_TowerLamp_Yellow.ToString()];

            if (dioLamp_Yellow == null)
            {
                return ret;
            }

            ret = (int)dioLamp_Yellow.GetValue();

            return ret;
        }

        public virtual int Yellow_On()
        {
            int ret = 0;
            DioPoint dioLamp_Yellow = m_dicDioPoints[DioPointKey.Output_TowerLamp_Yellow.ToString()];

            if (dioLamp_Yellow == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Yellow.Write(DioValue.On)) != 0)
            {
                return ret;
            }
            return ret;
        }

        public virtual int Yellow_Off()
        {
            int ret = 0;
            DioPoint dioLamp_Yellow = m_dicDioPoints[DioPointKey.Output_TowerLamp_Yellow.ToString()];

            if (dioLamp_Yellow == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Yellow.Write(DioValue.Off)) != 0)
            {
                return ret;
            }
            return ret;
        }

        public virtual int Is_Green_On()
        {
            int ret = 0;
            DioPoint dioLamp_Green = m_dicDioPoints[DioPointKey.Output_TowerLamp_Green.ToString()];

            if (dioLamp_Green != null)
                ret = (int)dioLamp_Green.GetValue();

            return ret;
        }

        public virtual int Green_On()
        {
            int ret = 0;
            DioPoint dioLamp_Green = m_dicDioPoints[DioPointKey.Output_TowerLamp_Green.ToString()];

            if ((ret = dioLamp_Green.Write(DioValue.On)) != 0)
            {
                return ret;
            }
            return ret;
        }

        public virtual int Green_Off()
        {
            int ret = 0;
            DioPoint dioLamp_Green = m_dicDioPoints[DioPointKey.Output_TowerLamp_Green.ToString()];

            if (dioLamp_Green == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Green.Write(DioValue.Off)) != 0)
            {
                return ret;
            }
            return ret;
        }

        public virtual int Is_Buzzer_On()
        {
            int ret = 0;
            DioPoint dioBuzzer = m_dicDioPoints[DioPointKey.Output_Buzzer.ToString()];

            if (dioBuzzer == null)
            {
                return ret;
            }

            ret = (int)dioBuzzer.GetValue();

            return ret;
        }

        public virtual int Buzzer_On()
        {
            int ret = 0;
            DioPoint dioBuzzer = m_dicDioPoints[DioPointKey.Output_Buzzer.ToString()];

            if (dioBuzzer == null)
            {
                return ret;
            }


            if ((ret = dioBuzzer.Write(DioValue.On)) != 0)
            {
                return ret;
            }
            return ret;
        }

        public virtual int Buzzer_Off()
        {
            int ret = 0;
            DioPoint dioBuzzer = m_dicDioPoints[DioPointKey.Output_Buzzer.ToString()];

            if (dioBuzzer == null)
            {
                return ret;
            }

            if ((ret = dioBuzzer.Write(DioValue.Off)) != 0)
            {
                return ret;
            }
            return ret;
        }

        public bool IsGreen()
        {
            bool ret = false;
            DioPoint dioGreen = m_dicDioPoints[DioPointKey.Output_TowerLamp_Green.ToString()];
            if (dioGreen != null)
            {
                DioValue Green = dioGreen.GetValue();

                if (Green == DioValue.On)
                {
                    ret = true;
                }
            }

            return ret;
        }

        public bool IsRed()
        {
            bool ret = false;
            DioPoint dioRed = m_dicDioPoints[DioPointKey.Output_TowerLamp_Red.ToString()];
            if (dioRed != null)
            {
                DioValue Red = dioRed.GetValue();

                if (Red == DioValue.On)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool IsYellow()
        {
            bool ret = false;
            DioPoint dioYellow = m_dicDioPoints[DioPointKey.Output_TowerLamp_Yellow.ToString()];
            if (dioYellow != null)
            {
                DioValue Yellow = dioYellow.GetValue();

                if (Yellow == DioValue.On)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool IsBuzzer()
        {
            bool ret = false;
            DioPoint dioBuzzer = m_dicDioPoints[DioPointKey.Output_Buzzer.ToString()];
            if (dioBuzzer != null)
            {
                DioValue Buzzer = dioBuzzer.GetValue();

                if (Buzzer == DioValue.On)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public virtual int AllLamp_Off()
        {
            int ret = 0;

            if (IsGreen())
            {
                Green_Off();
            }

            if (IsRed())
            {
                Red_Off();
            }

            if (IsYellow())
            {
                Yellow_Off();
            }


            //  LED Bar
            if (IsLedBar_Red())
            {
                LedBar_Red_Off();
            }

            if (IsLedBar_Green())
            {
                LedBar_Green_Off();
            }

            if (IsLedBar_Blue())
            {
                LedBar_Blue_Off();
            }

            return ret;
        }

        /// <summary>
        /// 
        /// LED Bar Lamp
        /// 
        /// </summary>
        /// 
        public virtual int Is_LedBar_Red_On()
        {
            int ret = 0;
            DioPoint dioLamp_Red = m_dicDioPoints[DioPointKey.Output_Lamp1_Red.ToString()];

            if (dioLamp_Red == null)
            {
                return ret;
            }

            ret = (int)dioLamp_Red.GetValue();

            return ret;
        }

        public virtual int LedBar_Red_On()
        {
            int ret = 0;
            DioPoint dioLamp_Red1 = m_dicDioPoints[DioPointKey.Output_Lamp1_Red.ToString()];
            DioPoint dioLamp_Red2 = m_dicDioPoints[DioPointKey.Output_Lamp2_Red.ToString()];
            DioPoint dioLamp_Red3 = m_dicDioPoints[DioPointKey.Output_Lamp3_Red.ToString()];

            if (dioLamp_Red1 == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Red1.Write(DioValue.On)) != 0)
            {
                return ret;
            }

            //  1 번째 채널 Write 성공이면 나머지도 Write
            dioLamp_Red2.Write(DioValue.On);
            dioLamp_Red3.Write(DioValue.On);

            return ret;
        }

        public virtual int LedBar_Red_Off()
        {
            int ret = 0;
            DioPoint dioLamp_Red1 = m_dicDioPoints[DioPointKey.Output_Lamp1_Red.ToString()];
            DioPoint dioLamp_Red2 = m_dicDioPoints[DioPointKey.Output_Lamp2_Red.ToString()];
            DioPoint dioLamp_Red3 = m_dicDioPoints[DioPointKey.Output_Lamp3_Red.ToString()];

            if (dioLamp_Red1 == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Red1.Write(DioValue.Off)) != 0)
            {
                return ret;
            }

            //  1 번째 채널 Write 성공이면 나머지도 Write
            dioLamp_Red2.Write(DioValue.Off);
            dioLamp_Red3.Write(DioValue.Off);

            return ret;
        }

        public virtual int Is_LedBar_Green_On()
        {
            int ret = 0;
            DioPoint dioLamp_Green = m_dicDioPoints[DioPointKey.Output_Lamp1_Green.ToString()];

            if (dioLamp_Green == null)
            {
                return ret;
            }

            ret = (int)dioLamp_Green.GetValue();

            return ret;
        }

        public virtual int LedBar_Green_On()
        {
            int ret = 0;
            DioPoint dioLamp_Green1 = m_dicDioPoints[DioPointKey.Output_Lamp1_Green.ToString()];
            DioPoint dioLamp_Green2 = m_dicDioPoints[DioPointKey.Output_Lamp2_Green.ToString()];
            DioPoint dioLamp_Green3 = m_dicDioPoints[DioPointKey.Output_Lamp3_Green.ToString()];

            if (dioLamp_Green1 == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Green1.Write(DioValue.On)) != 0)
            {
                return ret;
            }

            //  1 번째 채널 Write 성공이면 나머지도 Write
            dioLamp_Green2.Write(DioValue.On);
            dioLamp_Green3.Write(DioValue.On);

            return ret;
        }

        public virtual int LedBar_Green_Off()
        {
            int ret = 0;
            DioPoint dioLamp_Green1 = m_dicDioPoints[DioPointKey.Output_Lamp1_Green.ToString()];
            DioPoint dioLamp_Green2 = m_dicDioPoints[DioPointKey.Output_Lamp2_Green.ToString()];
            DioPoint dioLamp_Green3 = m_dicDioPoints[DioPointKey.Output_Lamp3_Green.ToString()];

            if (dioLamp_Green1 == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Green1.Write(DioValue.Off)) != 0)
            {
                return ret;
            }

            //  1 번째 채널 Write 성공이면 나머지도 Write
            dioLamp_Green2.Write(DioValue.Off);
            dioLamp_Green3.Write(DioValue.Off);

            return ret;
        }

        public virtual int Is_LedBar_Blue_On()
        {
            int ret = 0;
            DioPoint dioLamp_Blue = m_dicDioPoints[DioPointKey.Output_Lamp1_Blue.ToString()];

            if (dioLamp_Blue == null)
            {
                return ret;
            }

            ret = (int)dioLamp_Blue.GetValue();

            return ret;
        }

        public virtual int LedBar_Blue_On()
        {
            int ret = 0;
            DioPoint dioLamp_Blue1 = m_dicDioPoints[DioPointKey.Output_Lamp1_Blue.ToString()];
            DioPoint dioLamp_Blue2 = m_dicDioPoints[DioPointKey.Output_Lamp2_Blue.ToString()];
            DioPoint dioLamp_Blue3 = m_dicDioPoints[DioPointKey.Output_Lamp3_Blue.ToString()];

            if (dioLamp_Blue1 == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Blue1.Write(DioValue.On)) != 0)
            {
                return ret;
            }

            //  1 번째 채널 Write 성공이면 나머지도 Write
            dioLamp_Blue2.Write(DioValue.On);
            dioLamp_Blue3.Write(DioValue.On);

            return ret;
        }

        public virtual int LedBar_Blue_Off()
        {
            int ret = 0;
            DioPoint dioLamp_Blue1 = m_dicDioPoints[DioPointKey.Output_Lamp1_Blue.ToString()];
            DioPoint dioLamp_Blue2 = m_dicDioPoints[DioPointKey.Output_Lamp2_Blue.ToString()];
            DioPoint dioLamp_Blue3 = m_dicDioPoints[DioPointKey.Output_Lamp3_Blue.ToString()];

            if (dioLamp_Blue1 == null)
            {
                return ret;
            }

            if ((ret = dioLamp_Blue1.Write(DioValue.Off)) != 0)
            {
                return ret;
            }

            //  1 번째 채널 Write 성공이면 나머지도 Write
            dioLamp_Blue2.Write(DioValue.Off);
            dioLamp_Blue3.Write(DioValue.Off);

            return ret;
        }

        public bool IsLedBar_Red()
        {
            bool ret = false;
            DioPoint dioRed1 = m_dicDioPoints[DioPointKey.Output_Lamp1_Red.ToString()];
            if (dioRed1 != null)
            {
                DioValue Red = dioRed1.GetValue();

                if (Red == DioValue.On)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool IsLedBar_Green()
        {
            bool ret = false;
            DioPoint dioGreen1 = m_dicDioPoints[DioPointKey.Output_Lamp1_Green.ToString()];
            if (dioGreen1 != null)
            {
                DioValue Green = dioGreen1.GetValue();

                if (Green == DioValue.On)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool IsLedBar_Blue()
        {
            bool ret = false;
            DioPoint dioBlue1 = m_dicDioPoints[DioPointKey.Output_Lamp1_Blue.ToString()];
            if (dioBlue1 != null)
            {
                DioValue Blue = dioBlue1.GetValue();

                if (Blue == DioValue.On)
                {
                    ret = true;
                }
            }
            return ret;
        }


        public virtual int Lamp0_On()
        {
            int ret = 0;
            DioPoint dioLamp0 = m_dicDioPoints[DioPointKey.Output_Lamp4.ToString()];

            if (dioLamp0 == null)
            {
                return ret;
            }

            if ((ret = dioLamp0.Write(DioValue.On)) != 0)
            {
                return ret;
            }

            return ret;
        }

        public virtual int Lamp0_Off()
        {
            int ret = 0;
            DioPoint dioLamp0 = m_dicDioPoints[DioPointKey.Output_Lamp4.ToString()];

            if (dioLamp0 == null)
            {
                return ret;
            }

            if ((ret = dioLamp0.Write(DioValue.Off)) != 0)
            {
                return ret;
            }

            return ret;
        }

        public virtual int Lamp1_On()
        {
            int ret = 0;
            DioPoint dioLamp1 = m_dicDioPoints[DioPointKey.Output_Lamp5.ToString()];

            if (dioLamp1 == null)
            {
                return ret;
            }

            if ((ret = dioLamp1.Write(DioValue.On)) != 0)
            {
                return ret;
            }

            return ret;
        }

        public virtual int Lamp1_Off()
        {
            int ret = 0;
            DioPoint dioLamp1 = m_dicDioPoints[DioPointKey.Output_Lamp5.ToString()];

            if (dioLamp1 == null)
            {
                return ret;
            }

            if ((ret = dioLamp1.Write(DioValue.Off)) != 0)
            {
                return ret;
            }

            return ret;
        }

        public bool IsLamp0()
        {
            bool ret = false;
            DioPoint dioLamp = m_dicDioPoints[DioPointKey.Output_Lamp4.ToString()];
            if (dioLamp != null)
            {
                DioValue Blue = dioLamp.GetValue();

                if (Blue == DioValue.On)
                {
                    ret = true;
                }
            }
            return ret;
        }

        public bool IsLamp1()
        {
            bool ret = false;
            DioPoint dioLamp = m_dicDioPoints[DioPointKey.Output_Lamp5.ToString()];
            if (dioLamp != null)
            {
                DioValue Blue = dioLamp.GetValue();

                if (Blue == DioValue.On)
                {
                    ret = true;
                }
            }
            return ret;
        }
    }
}
