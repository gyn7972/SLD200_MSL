using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QMC.Common
{
    public static class ProcessManager
    {
        public static List<LayerInfo> Layers { get; private set; } = new List<LayerInfo>();
        public static string StateFilePath = "D:\\process_state.txt"; // 저장 경로

        public static int nHole1Layer_Index = 0;

        // 초기화 (초기 생성)
        public static void Init()
        {
            nHole1Layer_Index = 0;

            Layers.Clear();
        }

        public static LayerInfo AddLayer(string layerName, int layerNumber)
        {
            var layer = new LayerInfo(layerName, layerNumber);
            Layers.Add(layer);
            return layer;
        }

        public static void Reset()
        {
            foreach (var layer in Layers)
                layer.ResetSockets();
        }

        public static LayerInfo GetLayer(string layerName)
        {
            return Layers.FirstOrDefault(l => l.LayerName == layerName);
        }

        public static LayerInfo GetLayer(int layerNumber)
        {
            return Layers.FirstOrDefault(l => l.LayerNumber == layerNumber);
        }

        // 특정 Layer의 소켓 결과 설정 || 
        //추가: 아직 가공되지 않은 첫 위치 반환
        public static (string layerName, int socketIndex)? GetFirstUnprocessedPosition()
        {
            foreach (var layer in Layers)
            {
                for (int j = 0; j < layer.Sockets.Count; j++)
                {
                    if (!layer.Sockets[j].InspectionResult)
                        return (layer.LayerName, j);
                }
            }
            return null;
        }

        //전체 미가공 소켓 수 계산
        public static int GetAllUnprocessedCount()
        {
            return Layers.Sum(layer => layer.Sockets.Count(s => !s.InspectionResult));
        }
        //public static int GetAllUnprocessedCount() =>
        //Layers.Sum(layer => layer.Sockets.Count(s => !s.InspectionResult));

        //특정 Layer 가공률(%) 계산
        public static double GetLayerProgress(string layerName)
        {
            var layer = GetLayer(layerName);
            if (layer == null || layer.Sockets.Count == 0)
                return 0;

            int done = layer.Sockets.Count(s => s.InspectionResult);
            return (done / (double)layer.Sockets.Count) * 100.0;
        }

        //상태 저장
        public static void SaveStateToFile()
        {
            var sb = new StringBuilder();
            foreach (var layer in Layers)
            {
                sb.AppendLine($"[Layer:{layer.LayerName}]");
                foreach (var socket in layer.Sockets)
                    sb.AppendLine($"Socket_{socket.SocketNumber} = {socket.InspectionResult.ToString().ToLower()}");
                sb.AppendLine();
            }
            File.WriteAllText(StateFilePath, sb.ToString());
        }

        public static void LoadStateFromFile()
        {
            if (!File.Exists(StateFilePath))
                return;

            string currentLayerName = null;
            foreach (var line in File.ReadAllLines(StateFilePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.StartsWith("[Layer:"))
                {
                    currentLayerName = line.Replace("[Layer:", "").Replace("]", "").Trim();
                }
                else if (currentLayerName != null && line.StartsWith("Socket_"))
                {
                    var parts = line.Split('=');
                    int socketNum = int.Parse(parts[0].Trim().Replace("Socket_", ""));
                    bool result = parts[1].Trim().ToLower() == "true";

                    var layer = GetLayer(currentLayerName);
                    var socket = layer?.Sockets.FirstOrDefault(s => s.SocketNumber == socketNum);
                    if (socket != null)
                        socket.InspectionResult = result;
                }
            }
        }
    }
}
