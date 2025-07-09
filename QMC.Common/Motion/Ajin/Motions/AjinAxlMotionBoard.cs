
using QMC.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.Motion.Ajin.Motions
{
    [Serializable]
    public class AjinAxlMotionBoard : MotionBoard
    {
        public AjinAxlMotionBoard() : base()
        {
            Configuration.BoardType = MotionBoardType.Ajin;
        }
        protected override int OnClose()
        {
            int ret = 0;

            foreach (AjinAxlAxis axis in Axes)
            {
                axis.Close();
            }

            AXL.Close();

            return ret;
        }

        protected override int OnOpen()
        {
            bool m_bCurrentLaserType = Equipment.Machine_LaserType_CO2;
            bool m_bRet = true;
            string strParameterFile = "SLD-200.mot";
            string strFIle = "";
            StringBuilder temp = new StringBuilder(255);

            strFIle = ConfigManager.GetConfigPath() + "\\Machine Option (Do not delete or modify).ini";

            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("Machine Option 파일이 없습니다.\r\n\r\n[Default 값(CO₂)으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }
            else
            {
                //  Laser Type                                                                            //  True : CO₂,    False : UV
                NativeMethods.GetPrivateProfileString("Machine_Option", "Laser_Type", "True", temp, 255, strFIle);
                m_bCurrentLaserType = temp.ToString() == "False" ? false : true;

                if (m_bCurrentLaserType)        //  CO₂
                {
                    strParameterFile = string.Format("D:\\SLD-200_Parameter\\SLD-200C.mot");
                }
                else                            //  UV
                {
                    strParameterFile = string.Format("D:\\SLD-200_Parameter\\SLD-200U.mot");
                }



            }

            int ret = 0;
            bool bIsOpen = AXL.IsOpened();
            ret = AXL.Open();

            bIsOpen = AXL.IsOpened();

            if (ret == 0)
            {
                //ret = (int)AXM.AxmMotLoadParaAll("D:\\SLD-200_Parameter\\SLD-200.mot");
                ret = (int)AXM.AxmMotLoadParaAll(strParameterFile);
                bIsOpen = AXL.IsOpened();
            }

            //if(!bIsOpen)
            //{
            //    ret = (int)AXM.AxmMotLoadParaAll(ConfigManager.GetAjinMotorParameterFile());
            //}


            return ret;
        }

        public override int Load(FileStream fs)
        {
            int ret = 0;
            MotionBoardConfiguration configuration = new MotionBoardConfiguration();
            if ((ret = SaveManager.BinaryDeserialize<MotionBoardConfiguration>(fs, out configuration)) != 0) return ret;
            this.Configuration = configuration;
            int nAxisCount = this.Configuration.AxisCount;

            for (int i = 0; i < nAxisCount; i++)
            {
                AjinAxlAxis axis = new AjinAxlAxis();
                axis.Load(fs);
                axis.Board = this;
                this.AddAxis(axis);
            }

            return ret;
        }

        public override int Load(MotionBoardConfiguration configuration, FileStream fs)
        {
            int ret = 0;
            try
            {
                if (configuration != null)
                {

                    this.Configuration = configuration;
                    int nAxisCount = this.Configuration.AxisCount;

                    for (int i = 0; i < nAxisCount; i++)
                    {
                        AjinAxlAxis axis = new AjinAxlAxis();
                        axis.Load(fs);
                        axis.Board = this;
                        this.AddAxis(axis);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(this.Name, $"Load() : {ex.Message}");
            }

            return ret;
        }
    }
}
