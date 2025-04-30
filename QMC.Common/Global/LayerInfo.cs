using System.Collections.Generic;
using System.Linq;

namespace QMC.Common
{
    public class LayerInfo
    {
        private List<SocketInfo> sockets;

        public string LayerName { get; set; }
        public int LayerNumber { get; set; }
        public List<SocketInfo> Sockets { get => sockets; set => sockets = value; }

        public LayerInfo(string name, int number)
        {
            LayerName = name;
            LayerNumber = number;
            Sockets = new List<SocketInfo>();
        }

        public void AddSocket(int socketNumber)
        {
            if (!Sockets.Any(s => s.SocketNumber == socketNumber))
                Sockets.Add(new SocketInfo(socketNumber));
        }

        public void SetSocketResult(int socketNumber, bool result, string info = "")
        {
            var socket = Sockets.FirstOrDefault(s => s.SocketNumber == socketNumber);
            if (socket != null)
            {
                socket.InspectionResult = result;
                socket.AdditionalInfo = info;

                // 자동 저장 트리거
                ProcessManager.SaveStateToFile();
            }
        }

        public void ResetSockets()
        {
            foreach (var socket in Sockets)
            {
                socket.Reset();
            }
        }
    }
}
