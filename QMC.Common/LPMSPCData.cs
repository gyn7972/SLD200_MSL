using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class LPMSPCData
    {

        #region Constructor
        public LPMSPCData()
        {
            this.LoactionNO = 0;
            this.CarrierID = "";
            this.UnitID = "";
            this.Rivet1 = 0.0;
            this.Rivet2 = 0.0;
            this.EtchingWidth = 0.0;
            this.EtchingHeight = 0.0;
            this.MountShiftX = 0.0;
            this.MountShiftY = 0.0;
            this.Dispensing = false;
        }
        #endregion

        #region Property
        public DateTime Date { get; set; }
        public int LoactionNO { get; set; }
        public string CarrierID { get; set; }
        public string UnitID { get; set; }
        public double Rivet1 { get; set; }
        public double Rivet2 { get; set; }
        public double EtchingWidth { get; set; }
        public double EtchingHeight { get; set; }
        public double MountShiftX { get; set; }
        public double MountShiftY { get; set; }
        public bool Dispensing { get; set; }
        #endregion

        #region Method
        public string SaveCsvFormat()
        {
            string value = string.Empty;

            value = string.Format($"{this.Date},{this.LoactionNO},{this.CarrierID},{this.UnitID},{Rivet1},{Rivet2},{EtchingWidth},{EtchingHeight},{MountShiftX},{MountShiftY}");

            return value;
        }

        public override string ToString()
        {
            // 항목:데이터@항목:데이터@항목:데이터@항목:데이터@
            //smtunit measuredata에 넣어주기.
            string value = string.Empty;

            value = string.Format($"Rivet1:{Rivet1}@Rivet2:{Rivet2}@Dispensing:{Dispensing}@EtchingWidth:{EtchingWidth}@EtchingHeight:{EtchingHeight}@MounterShiftX:{MountShiftX}@MounterShiftY:{MountShiftY}");

            return value;
        }
        #endregion
    }
}
