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
        public static string StateFilePath = "process_state.txt"; // 저장 경로

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

        public static LayerInfo GetLayer(int layerNumber)
        {
            return Layers.FirstOrDefault(l => l.LayerNumber == layerNumber);
        }

        // 특정 Layer의 소켓 결과 설정 || 
        //추가: 아직 가공되지 않은 첫 위치 반환
        public static (int layerIndex, int socketIndex)? GetFirstUnprocessedPosition()
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                for (int j = 0; j < layer.Sockets.Count; j++)
                {
                    if (!layer.Sockets[j].InspectionResult)
                        return (i, j);
                }
            }
            return null;
        }

        //전체 미가공 소켓 수 계산
        public static int GetAllUnprocessedCount()
        {
            return Layers.Sum(layer => layer.Sockets.Count(s => !s.InspectionResult));
        }

        //특정 Layer 가공률(%) 계산
        public static double GetLayerProgress(int layerNumber)
        {
            var layer = GetLayer(layerNumber);
            if (layer == null || layer.Sockets.Count == 0)
                return 0;

            int total = layer.Sockets.Count;
            int done = layer.Sockets.Count(s => s.InspectionResult);
            return (done / (double)total) * 100.0;
        }

        //상태 저장
        public static void SaveStateToFile()
        {
            var sb = new StringBuilder();

            foreach (var layer in Layers)
            {
                sb.AppendLine($"[Layer_{layer.LayerNumber}]");
                foreach (var socket in layer.Sockets)
                {
                    sb.AppendLine($"Socket_{socket.SocketNumber} = {socket.InspectionResult.ToString().ToLower()}");
                }
                sb.AppendLine();
            }

            File.WriteAllText(StateFilePath, sb.ToString());
        }

        //상태 복원
        public static void LoadStateFromFile()
        {
            if (!File.Exists(StateFilePath))
                return;

            var lines = File.ReadAllLines(StateFilePath);
            LayerInfo currentLayer = null;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                if (line.StartsWith("[Layer_"))
                {
                    int num = int.Parse(line.Replace("[Layer_", "").Replace("]", ""));
                    currentLayer = GetLayer(num);
                }
                else if (line.StartsWith("Socket_") && currentLayer != null)
                {
                    var parts = line.Split('=');
                    var socketNum = int.Parse(parts[0].Trim().Replace("Socket_", ""));
                    bool value = parts[1].Trim().ToLower() == "true";

                    var socket = currentLayer.Sockets.FirstOrDefault(s => s.SocketNumber == socketNum);
                    if (socket != null)
                        socket.InspectionResult = value;
                }
            }
        }
    }
}
