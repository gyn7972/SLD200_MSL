using ACS.SPiiPlusNET;
using QMC.Common.Motion.Ajin.Motions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Motion.ACS.Motions
{
    #region MyRegACSSPiiPlusMotionBoardion
    [Serializable]
    public class ACSSPiiPlusMotionBoard : MotionBoard
    {
        #region Constructor
        public ACSSPiiPlusMotionBoard() : base()
        {
            Api = new Api();
            Configuration.BoardType = MotionBoardType.ACS;
        }
        #endregion

        #region Property
        public static Api Api { get; set; }
        #endregion

        #region MotionBoard Members
        protected override int OnClose()
        {
            int ret = 0;

            foreach (MotionAxis axis in Axes)
            {
                axis.Close();
            }

            Api.CloseComm();
            //AXL.Close();

            return ret;
        }

        protected override int OnOpen()
        {
            int ret = 0;

            try
            {
                if (this.Configuration != null)
                {
                    if (this.Configuration.Address != null)
                    {
                        Api.OpenCommEthernetTCP(
                         this.Configuration.Address.ToString(),                             // IP Address (Default : 10.0.0.100)
                         this.Configuration.Port//Convert.ToInt32(txtPort.Text.Trim())    // TCP/IP Port nubmer (default : 701)
                         );

                        bool bIsOpen = Api.IsConnected;
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Write(this.Name, String.Format($"{ex.Message}"));
            }


            //bool bIsOpen = AXL.IsOpened();
            //ret = AXL.Open();

            //bIsOpen = AXL.IsOpened();

            //if (ret == 0)
            //{
            //    ret = (int)AXM.AxmMotLoadParaAll("D:\\SLD-100.mot");
            //    bIsOpen = AXL.IsOpened();
            //}

            ////if(!bIsOpen)
            ////{
            ////    ret = (int)AXM.AxmMotLoadParaAll(ConfigManager.GetAjinMotorParameterFile());
            ////}

            return ret;
        }

        public override int Load(FileStream fs)
        {
            int ret = 0;
            MotionBoardConfiguration configuration = new MotionBoardConfiguration();
            if ((ret = SaveManager.BinaryDeserialize<MotionBoardConfiguration>(fs, out configuration)) != 0) return ret;
            int nAxisCount = this.Configuration.AxisCount;

            for (int i = 0; i < nAxisCount; i++)
            {
                ACSSPiiPlusAxis axis = new ACSSPiiPlusAxis();
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
                        ACSSPiiPlusAxis axis = new ACSSPiiPlusAxis();
                        //AjinAxlAxis axis = new AjinAxlAxis();
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
        #endregion
    }
    #endregion
}
