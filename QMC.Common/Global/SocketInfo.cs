using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLD200_MSL;

namespace QMC.Common
{
    public class SocketInfo
    {
        public int SocketNumber { get; set; }            // 소켓 번호
        public bool InspectionResult { get; set; }        // 검사 결과 (OK/NG)
        public string AdditionalInfo { get; set; }        // 추가 설명

        public SocketInfo(int socketNumber)
        {
            SocketNumber = socketNumber;
            InspectionResult = false;
            AdditionalInfo = string.Empty;
        }

        public void Reset()
        {
            InspectionResult = false;
            AdditionalInfo = string.Empty;
        }
    }
}
