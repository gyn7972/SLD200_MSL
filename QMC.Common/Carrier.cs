using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class Carrier
    {
        public enum CarrierWorkStateKey
        {
            None,
            Work,
            Complete
        }


        public SmtUnit[] ArrSmtUnit = {};
        public string ID { get; set; }
        public string RepresentationID { get; set; }
        public int UnitCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StartTime_2 { get; set; }
        public CarrierWorkStateKey CurrentState { get; set; }

        public Carrier()
        {
            Init();
        }
       
        private void Init()
        {
            ID = DateTime.Now.ToString("yyyyMMddhhmmssfff");
            UnitCount = 10;
            ArrSmtUnit = new SmtUnit[UnitCount];

            CurrentState = CarrierWorkStateKey.None;
            //Obj Data Initialize
            StartTime = DateTime.Now;
            StartTime_2 = DateTime.Now;
            for (int i = 0; i < UnitCount; i++)
            {
                SmtUnit unit = new SmtUnit();
                unit.CarrierID = ID;
                unit.UseWork = true;
                unit.IsBarcode = false;
                unit.WorkStatus = SmtUnit.UnitWorkStateKey.None;
                unit.ObjID = ID +"0"+i.ToString();
                //unit.RivetDepth = 0;
                //unit.RivetForce = 0;
                unit.Result = SmtUnit.ResultKey.Ok;
                ArrSmtUnit[i] = unit;
            }

            RepresentationID = ArrSmtUnit[0].ObjID;
        }

        public int GetRemainCount()
        {
            int count = 0;

            foreach(SmtUnit unit in ArrSmtUnit)
            {
                if(unit != null)
                    count++;
            }

            return count;
        }

        public bool IsFull()
        {
            bool ret = true;

            foreach (SmtUnit unit in ArrSmtUnit)
            {
                if(unit == null)
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }
        public bool IsEmpty()
        {
            int nCount = 0;
            bool ret = false;
            for (int i = 0; i < UnitCount; i++)
            {
                if (ArrSmtUnit[i] == null)
                {
                    nCount++; 
                }
            }
            if(nCount == UnitCount)
            {
                ret = true;
            }
            return ret;
        }
        public int Pop(out SmtUnit unit)
        {
            int nIndex = 0;
            unit = null;
            for(int i = 0; i < UnitCount; i++)
            {
                if(ArrSmtUnit[i] != null)
                {
                    unit = ArrSmtUnit[i];
                    ArrSmtUnit[i] = null;
                    nIndex = i;
                    break;
                }
            }
            return nIndex;
        }

        public int GetCarrierEmptyIndex()
        {
            int nIndex = -1;
            for (int i = 0; i < UnitCount; i++)
            {
                if (ArrSmtUnit[i] == null)
                {
                    nIndex = i;
                    break;
                }
            }
            return nIndex;
        }
        public int AddUnitData(SmtUnit unitData, int nloadeIndex)
        {
            int ret = 0;
            if (ArrSmtUnit.Count() > nloadeIndex && ArrSmtUnit[nloadeIndex] == null)
            {
                ArrSmtUnit[nloadeIndex] = unitData;
            }
            else
            {
                ret = -1;
            }
            return ret;
        }
        public int GetRemainEmptyCount()
        {
            int count = 0;

            foreach (SmtUnit unit in ArrSmtUnit)
            {
                if (unit == null)
                    count++;
            }

            return count;
        }
        public int GetUnitIndex()
        {
            int nIndex = 0;

            for (int i = 0; i < UnitCount; i++)
            {
                if (ArrSmtUnit[i] != null)
                {
                    nIndex = i;
                    break;
                }
            }

            return nIndex;
        }
        public bool IsUseWork(int nIndex)
        {
            bool bRet = false;
            bRet = ArrSmtUnit[nIndex].UseWork;
            return bRet;
        }

        public void AllOK()
        {
            for (int i = 0; i < UnitCount; i++)
            {
                ArrSmtUnit[i].Result = SmtUnit.ResultKey.Ok;
            }
        }

        public DateTime GetCarrierStartTime()
        {
            return StartTime;
        }

        public List<UnitStatus> GetUnitStatuses()
        {
            List<UnitStatus> unitStatuses = new List<UnitStatus>();
            foreach (SmtUnit unit in ArrSmtUnit) 
            {
                unitStatuses.Add(new UnitStatus(unit.ObjID, unit.Result == SmtUnit.ResultKey.Ok ? 0 : 1));
            }
            return unitStatuses;
        }

        public void SetUnitStatus(List<UnitStatus> unitStatuses)
        {
            int i = 0;
            foreach(UnitStatus unit in unitStatuses)
            {
                if(i < ArrSmtUnit.Length)
                {
                    ArrSmtUnit[i].ObjID = unit.Barcode;
                    ArrSmtUnit[i].UseWork = (unit.Status == 0);
                    ArrSmtUnit[i].Result = (unit.Status == 0) ? SmtUnit.ResultKey.Ok : SmtUnit.ResultKey.Ng;
                    i++;
                }                
            }

        }

    }
}
