using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class SmtUnit
    {
        public enum UnitWorkStateKey
        {
            None,
            Work,
            Complete
        }
        public enum ResultKey
        {
            None,
            Ok,
            Ng,
        }
        public string ObjID { get; set; }
        public string CarrierID { get; set; }
        public bool UseWork { get; set; }
        public ResultKey Result { get; set; }
        public bool IsBarcode { get; set; }
        public UnitWorkStateKey WorkStatus { get; set; }
        public XytCoordinate EtchingPosition { set; get; }
        public object MeasureData { get; set; }

        public DateTime ProcessTime { get; set; }
        public SmtUnit()
        {
            Init();
        }
        private void Init()
        {
            EtchingPosition = new XytCoordinate();
            Result = ResultKey.Ok;
        }

        public XytCoordinate GetEtchingPosition(XyCoordinate offset)
        {
            XytCoordinate coordinate = this.EtchingPosition;
            coordinate.X += offset.X;
            coordinate.Y += offset.Y;
            return coordinate;
        }
    }
}
