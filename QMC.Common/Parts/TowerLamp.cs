using QMC.Common.Modules;
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

            return ret;
        }        
    }
}
