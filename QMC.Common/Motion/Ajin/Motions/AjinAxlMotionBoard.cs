
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            int ret = 0;
            bool bIsOpen = AXL.IsOpened();
            ret = AXL.Open();

            bIsOpen = AXL.IsOpened();

            if (ret == 0)
            {
                ret = (int)AXM.AxmMotLoadParaAll("D:\\CWA-150SA_Parameter\\CWA-150SA.mot");
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
