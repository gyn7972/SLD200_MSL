using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QMC.Common
{
    public static class ProcessManager
    {
        public static List<SocketInfo> Sockets { get; private set; } = new List<SocketInfo>();
        public static string StateFilePath = "D:\\process_state.txt";

        public class AreaResult
        {
            public int SocketNumber { get; set; }
            public string LayerName { get; set; }
            public int AreaIndex { get; set; }
            public bool IsProcessed { get; set; }
            public string Note { get; set; }
        }

        public static void Init()
        {
            Sockets.Clear();
        }

        public static void Reset()
        {
            foreach (var socket in Sockets)
            {
                socket.InspectionResult = false;
                socket.AdditionalInfo = string.Empty;

                foreach (var layer in socket.Layers)
                {
                    foreach (var area in layer.Areas)
                    {
                        area.IsProcessed = false;
                        area.Note = string.Empty;
                    }
                }
            }

            SaveStateToFile(); // 상태 초기화 후 파일 저장도 포함하면 좋음
        }

        public static SocketInfo GetSocket(int socketNumber)
        {
            var socket = Sockets.FirstOrDefault(s => s.SocketNumber == socketNumber);
            if (socket == null)
            {
                socket = new SocketInfo(socketNumber);
                Sockets.Add(socket);
            }
            return socket;
        }

        public static int GetSocketCount()
        {
            return Sockets.Count;
        }

        public static int GetLayerCount(int socketNumber)
        {
            var socket = Sockets.FirstOrDefault(s => s.SocketNumber == socketNumber);
            return socket?.Layers.Count ?? 0;
        }

        public static int GetAreaCount(int socketNumber, string layerName)
        {
            var socket = Sockets.FirstOrDefault(s => s.SocketNumber == socketNumber);
            var layer = socket?.Layers.FirstOrDefault(l => l.LayerName == layerName);
            return layer?.Areas.Count ?? 0;
        }

        public static bool MarkAreaProcessed(int socketNumber, string layerName, int areaIndex, string note = "")
        {
            var socket = GetSocket(socketNumber);
            var layer = socket.GetLayer(layerName);

            if (layer == null)
            {
                layer = new LayerInfo(layerName, socket.Layers.Count + 1);
                socket.Layers.Add(layer);
            }

            layer.AddArea(areaIndex);
            var area = layer.Areas.FirstOrDefault(a => a.AreaIndex == areaIndex);

            if (area == null)
                return false;

            if (area.IsProcessed)
                return false; // 이미 처리됨

            area.IsProcessed = true;
            area.Note = note;

            SaveStateToFile();
            return true; // 이번에 새로 처리함
        }

        public static AreaResult GetAreaResult(int socketNumber, string layerName, int areaIndex)
        {
            var socket = Sockets.FirstOrDefault(s => s.SocketNumber == socketNumber);
            if (socket == null)
                return null;

            var layer = socket.Layers.FirstOrDefault(l => l.LayerName == layerName);
            if (layer == null)
                return null;

            var area = layer.Areas.FirstOrDefault(a => a.AreaIndex == areaIndex);
            if (area == null)
                return null;

            return new AreaResult
            {
                SocketNumber = socketNumber,
                LayerName = layerName,
                AreaIndex = areaIndex,
                IsProcessed = area.IsProcessed,
                Note = area.Note
            };
        }

        public static (int socketIndex, string layerName, int areaIndex)? GetFirstUnprocessedPosition()
        {
            foreach (var socket in Sockets)
            {
                foreach (var layer in socket.Layers)
                {
                    foreach (var area in layer.Areas)
                    {
                        if (!area.IsProcessed)
                        {
                            return (socket.SocketNumber, layer.LayerName, area.AreaIndex);
                        }
                    }
                }
            }

            return null;
        }

        public static void SaveStateToFile()
        {
            var sb = new StringBuilder();
            foreach (var socket in Sockets)
            {
                sb.AppendLine($"[Socket:{socket.SocketNumber}]");
                foreach (var layer in socket.Layers)
                {
                    sb.AppendLine($"  [Layer:{layer.LayerName}]");
                    foreach (var area in layer.Areas)
                        sb.AppendLine($"    Area_{area.AreaIndex} = {area.IsProcessed.ToString().ToLower()} // {area.Note}");
                }
                sb.AppendLine();
            }
            File.WriteAllText(StateFilePath, sb.ToString());
        }

        public static void LoadStateFromFile()
        {
            if (!File.Exists(StateFilePath))
                return;

            SocketInfo currentSocket = null;
            LayerInfo currentLayer = null;

            foreach (var line in File.ReadAllLines(StateFilePath))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                if (line.StartsWith("[Socket:"))
                {
                    int socketNum = int.Parse(line.Replace("[Socket:", "").Replace("]", "").Trim());
                    currentSocket = GetSocket(socketNum);
                }
                else if (line.Trim().StartsWith("[Layer:"))
                {
                    string layerName = line.Trim().Replace("[Layer:", "").Replace("]", "").Trim();
                    currentLayer = new LayerInfo(layerName, currentSocket.Layers.Count + 1);
                    currentSocket.Layers.Add(currentLayer);
                }
                else if (line.Trim().StartsWith("Area_"))
                {
                    var parts = line.Trim().Split('=');
                    int areaIdx = int.Parse(parts[0].Replace("Area_", "").Trim());
                    bool isDone = parts[1].Trim().ToLower().StartsWith("true");
                    currentLayer.AddArea(areaIdx);
                    currentLayer.SetAreaProcessed(areaIdx, isDone);
                }
            }
        }
    }
}
