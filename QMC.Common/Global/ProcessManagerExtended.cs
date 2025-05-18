using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Equipment;

namespace QMC.Common.Global
{
    public class ProcessManagerExtended
    {
        public static List<SocketInfo> Sockets { get; private set; } = new List<SocketInfo>();
        public static string StateFilePath = "D:\\process_state.txt";
        public static PreAlignData PreAlign { get; private set; } = new PreAlignData();

        public class AreaResult
        {
            public int SocketNumber { get; set; }
            public string LayerName { get; set; }
            public int AreaIndex { get; set; }
            public int ProcessStatus { get; set; }
            public string Note { get; set; }
        }

        public static void Init()
        {
            Sockets.Clear();
            PreAlign = new PreAlignData();
        }

        public static void Reset()
        {
            foreach (var socket in Sockets)
                socket.Reset();

            PreAlign = new PreAlignData();
            SaveStateToFile();
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

        public static AlignData GetSocketAlignData(int socketNumber)
        {
            return GetSocket(socketNumber).AlignData;
        }

        public static void SetPreAlign(PointD[] marks, double[] widths, double[] heights)
        {
            PreAlign.FiducialMarks = marks;
            PreAlign.Widths = widths;
            PreAlign.Heights = heights;
            PreAlign.IsCompleted = true;
        }

        public static bool MarkAreaProcessed(int socketNumber, string layerName, int areaIndex, int status, string note = "")
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

            if (area == null || area.ProcessStatus == status)
                return false;

            area.ProcessStatus = status;
            area.Note = note;

            if (layer.Areas.All(a => a.IsProcessed))
            {
                layer.IsProcessed = true;
                layer.ProcessedTime = DateTime.Now;
            }

            SaveStateToFile();
            return true;
        }

        public static AreaResult GetAreaResult(int socketNumber, string layerName, int areaIndex)
        {
            var socket = Sockets.FirstOrDefault(s => s.SocketNumber == socketNumber);
            var layer = socket?.Layers.FirstOrDefault(l => l.LayerName == layerName);
            var area = layer?.Areas.FirstOrDefault(a => a.AreaIndex == areaIndex);

            if (area == null)
                return null;

            return new AreaResult
            {
                SocketNumber = socketNumber,
                LayerName = layerName,
                AreaIndex = areaIndex,
                ProcessStatus = area.ProcessStatus,
                Note = area.Note
            };
        }

        public static (int socketIndex, string layerName, int areaIndex, int nResult)? GetFirstUnprocessedPosition()
        {
            foreach (var socket in Sockets)
            {
                foreach (var layer in socket.Layers)
                {
                    foreach (var area in layer.Areas)
                    {
                        if (!area.IsProcessed)
                            return (socket.SocketNumber, layer.LayerName, area.AreaIndex, area.ProcessStatus);
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
                    sb.AppendLine($"  [Layer:{layer.LayerName}] Processed={layer.IsProcessed} Time={layer.ProcessedTime}");
                    foreach (var area in layer.Areas)
                        sb.AppendLine($"    Area_{area.AreaIndex} = {area.ProcessStatus} // {area.Note}");
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
                    string part = line.Trim().Replace("[Layer:", "").Replace("]", "");
                    //string[] parts = part.Split("Processed=");
                    string[] parts = part.Split(new[] { "Processed=" }, StringSplitOptions.None);
                    string layerName = parts[0].Trim();
                    currentLayer = new LayerInfo(layerName, currentSocket.Layers.Count + 1);
                    currentSocket.Layers.Add(currentLayer);
                }
                else if (line.Trim().StartsWith("Area_"))
                {
                    var parts = line.Trim().Split('=');
                    int areaIdx = int.Parse(parts[0].Replace("Area_", "").Trim());
                    string[] valParts = parts[1].Split(new[] { "//" }, StringSplitOptions.None);
                    int status = int.Parse(valParts[0].Trim());
                    string note = valParts.Length > 1 ? valParts[1].Trim() : "";

                    currentLayer.AddArea(areaIdx);
                    currentLayer.SetAreaProcessed(areaIdx, status, note);
                }
            }
        }
    }

    public class AlignData
    {
        public WorkStage.st4PointAlign_Result SocketAlignResult { get; set; }
        public WorkStage.st4PointAlign_Result? GoldPowderAlignResult { get; set; }
        public bool GoldPowderUsed { get; set; }
        public Dictionary<string, double> HeightMeasureMap { get; set; } = new Dictionary<string, double>();

        public bool IsSocketAligned { get; set; }
        public bool IsHeightMeasured => HeightMeasureMap.Count > 0;
    }

    public class PreAlignData
    {
        public PointD[] FiducialMarks { get; set; } = new PointD[4];
        public double[] Widths { get; set; } = new double[4];
        public double[] Heights { get; set; } = new double[4];
        public bool IsCompleted { get; set; } = false;
    }



}
