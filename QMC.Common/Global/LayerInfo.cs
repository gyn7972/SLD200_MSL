using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public class LayerInfo
    {
        private List<SocketInfo> sockets;

        public string LayerName { get; set; }             // 레이어 이름
        public int LayerNumber { get; set; }               // 레이어 번호
        public List<SocketInfo> Sockets { get => sockets; set => sockets = value; }      // 소켓 리스트

        public LayerInfo(string name, int number)
        {
            LayerName = name;
            LayerNumber = number;
            Sockets = new List<SocketInfo>();
        }

        // 소켓 추가
        public void AddSocket(int socketNumber)
        {
            if (!Sockets.Any(s => s.SocketNumber == socketNumber))
                Sockets.Add(new SocketInfo(socketNumber));
        }

        // 소켓 결과 설정
        public void SetSocketResult(int socketNumber, bool result, string info = "")
        {
            var socket = Sockets.FirstOrDefault(s => s.SocketNumber == socketNumber);
            if (socket != null)
            {
                socket.InspectionResult = result;
                socket.AdditionalInfo = info;
            }
        }

        // 소켓 전체 초기화
        public void ResetSockets()
        {
            foreach (var socket in Sockets)
            {
                socket.Reset();
            }
        }
    }
}
