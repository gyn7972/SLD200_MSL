using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Global
{
    // Socket 단위 가공 정보
    public class SocketProcessData
    {
        public int SocketNumber { get; set; }
        public double DisplacementX { get; set; }
        public double DisplacementY { get; set; }

        public bool IsPreAligned { get; set; }
        public bool IsSocketAligned { get; set; }
        public PointD AlignedPosition { get; set; } = new PointD();

        public bool IsDrilled { get; set; }
        public bool IsSuccess { get; set; }

        public double AlignOffsetX { get; set; }
        public double AlignOffsetY { get; set; }
        public double AlignTheta { get; set; }

        // 해당 Layer에서 실제 가공 대상 소켓인지 여부
        public bool IsUsedInThisLayer { get; set; } = false;

        public void Reset()
        {
            DisplacementX = 0;
            DisplacementY = 0;
            IsPreAligned = false;
            IsSocketAligned = false;
            IsDrilled = false;
            IsSuccess = false;
            AlignOffsetX = 0;
            AlignOffsetY = 0;
            AlignTheta = 0;
            AlignedPosition = new PointD();
            IsUsedInThisLayer = false;
        }
    }

    // Layer 단위 가공 정보
    public class LayerProcessData
    {
        public string LayerName { get; set; }
        public List<SocketProcessData> SocketList { get; set; } = new List<SocketProcessData>();

        public void Reset()
        {
            foreach (var socket in SocketList)
                socket.Reset();
        }

        public SocketProcessData GetSocket(int socketNumber)
        {
            return SocketList.FirstOrDefault(s => s.SocketNumber == socketNumber);
        }
    }

    // 전체 공정 관리
    public class DrillingProcessManager
    {
        public List<LayerProcessData> LayerList { get; set; } = new List<LayerProcessData>();

        public void ResetAll()
        {
            foreach (var layer in LayerList)
                layer.Reset();
        }

        public LayerProcessData GetLayer(string layerName)
        {
            return LayerList.FirstOrDefault(l => l.LayerName == layerName);
        }

        public SocketProcessData GetSocket(string layerName, int socketNumber)
        {
            var layer = GetLayer(layerName);
            return layer?.GetSocket(socketNumber);
        }

        /// <summary>
        /// LayerList를 초기화합니다.
        /// </summary>
        public void InitDrillingManagerFromDrawing(List<string> layerNames, int socketCount)
        {
            LayerList.Clear();

            foreach (var name in layerNames)
            {
                var layer = new LayerProcessData { LayerName = name };
                for (int i = 0; i < socketCount; i++)
                {
                    layer.SocketList.Add(new SocketProcessData { SocketNumber = i });
                }
                LayerList.Add(layer);
            }
        }
    }

}
