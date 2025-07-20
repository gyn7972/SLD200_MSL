using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common;
using static QMC.Common.Equipment;

namespace QMC.Common.Global
{
    public class SocketProcessData
    {
        public int SocketNumber { get; set; }

        // 변위센서 측정 유무 / 결과
        public bool IsSocketDisplacement { get; set; }
        public double DisplacementZ { get; set; }

        // Socket 얼라인 유무 / 결과
        public bool IsSocketAligned { get; set; }
        public double SocketRotationCenterX { get; set; }
        public double SocketRotationCenterY { get; set; }
        public double SocketOffsetX { get; set; }
        public double SocketOffsetY { get; set; }
        public double SocketTheta { get; set; }

        // GoldPowder 얼라인 유무 / 결과
        public bool IsGoldPowderAligned { get; set; }
        public double GoldRotationCenterX { get; set; }
        public double GoldRotationCenterY { get; set; }
        public double GoldOffsetX { get; set; }
        public double GoldOffsetY { get; set; }
        public double GoldTheta { get; set; }


        //1. 기준 정의
        //구분 조건
        //미진행 IsDrilled == false
        //진행중 IsDrilled == true && IsSuccess == false
        //완료 IsDrilled == true && IsSuccess == true
        public bool IsDrilled { get; set; }
        public bool IsSuccess { get; set; }

        public bool IsUsedInThisLayer { get; set; } = true;

        public bool IsSelected { get; set; } = false;

        public void Reset()
        {
            IsSocketDisplacement = false;
            DisplacementZ = 0;
            IsSocketAligned = false;
            SocketRotationCenterX = SocketRotationCenterY = 0;
            SocketOffsetX = SocketOffsetY = SocketTheta = 0;
            IsGoldPowderAligned = false;
            GoldRotationCenterX = GoldRotationCenterY = 0;
            GoldOffsetX = GoldOffsetY = GoldTheta = 0;
            IsDrilled = false;
            IsSuccess = false;
            IsUsedInThisLayer = true;
            IsSelected = false;
        }
    }

    public class LayerProcessData
    {
        public string LayerName { get; set; }
        public int LayerNumber { get; set; }
        public Equipment.LayerList LayerEnum { get; set; }
        public Equipment.LayerType LayerType { get; set; }

        public bool IsPreAligned { get; set; }
        public double PreAlignRotationCenterX { get; set; }
        public double PreAlignRotationCenterY { get; set; }
        public double PreAlignOffsetX { get; set; }
        public double PreAlignOffsetY { get; set; }
        public double PreAlignTheta { get; set; }

        public List<SocketProcessData> SocketList { get; set; } = new List<SocketProcessData>();

        public bool IsSelected { get; set; } = false;

        public void Reset()
        {
            IsPreAligned = false;
            PreAlignRotationCenterX = PreAlignRotationCenterY = 0;
            PreAlignOffsetX = PreAlignOffsetY = PreAlignTheta = 0;

            foreach (var socket in SocketList)
                socket.Reset();

            IsSelected = false;
        }

        public SocketProcessData GetSocket(int socketNumber)
        {
            //return SocketList.FirstOrDefault(s => s.SocketNumber == socketNumber);
            if (SocketList == null)
            {
                //Log.Write("LayerProcessData", $"GetSocket 실패 - SocketList가 null입니다. 요청된 Socket: {socketNumber}");
                return null;
            }

            if (SocketList.Count == 0)
            {
                //Log.Write("LayerProcessData", $"GetSocket 실패 - SocketList가 비어 있습니다. 요청된 Socket: {socketNumber}");
                return null;
            }

            var socket = SocketList.FirstOrDefault(s => s.SocketNumber == socketNumber);
            if (socket == null)
            {
                //Log.Write("LayerProcessData", $"GetSocket 실패 - Socket {socketNumber} 이(가) 존재하지 않습니다.");
                return null;
            }

            return socket;
        }
    }

    public class DrillingProcessManager
    {
        public List<LayerProcessData> LayerList { get; set; } = new List<LayerProcessData>();

        // --- 추가된 변수들 ---
        public int CycleTimer_TargetModuleCount { get; set; } = 0;
        public int CycleTimer_DoneModuleCount { get; set; } = 0;
        public int CycleTimer_NGModuleCount { get; set; } = 0;
        public int CycleTimer_DoneSocketCount { get; set; } = 0;
        public int CycleTimer_NGSocketCount { get; set; } = 0;
        //  workStage 가공시간 계산을 위해 사용되는 변수
        public CycleTimer CycleTimer_LaserDrilling { get; set; } = new CycleTimer();

        private bool _hasChanged = false;

        public void ResetAll()
        {
            //foreach (var layer in LayerList)
            //    layer.Reset();

            //MarkAsChanged();
            foreach (var layer in LayerList)
            {
                foreach (var socket in layer.SocketList)
                {
                    socket.IsSocketDisplacement = false;
                    socket.IsSocketAligned = false;
                    socket.IsDrilled = false;
                    socket.IsSuccess = false;
                    socket.IsGoldPowderAligned = false;
                }
            }
            MarkAsChanged();
        }

        public LayerProcessData GetLayer(string layerName)
        {
            //return LayerList.FirstOrDefault(l => l.LayerName == layerName);
            if (string.IsNullOrEmpty(layerName))
            {
                Log.Write("DrillStatus", "GetLayer 실패 - 입력된 layerName이 null 또는 빈 문자열입니다.");
                return null;
            }

            if (LayerList == null)
            {
                Log.Write("DrillStatus", $"GetLayer 실패 - LayerList가 null입니다. 요청된 LayerName: {layerName}");
                return null;
            }

            if (LayerList.Count == 0)
            {
                Log.Write("DrillStatus", $"GetLayer 실패 - LayerList가 비어 있습니다. 요청된 LayerName: {layerName}");
                return null;
            }

            var layer = LayerList.FirstOrDefault(l => l.LayerName == layerName);
            if (layer == null)
            {
                Log.Write("DrillStatus", $"GetLayer 실패 - '{layerName}' 이름의 Layer가 존재하지 않습니다.");
            }

            return layer;
        }

        public LayerProcessData GetLayer(Equipment.LayerList layerEnum)
        {
            // catch (Exception ex)로 빠진다.
            //return LayerList.FirstOrDefault(l => l.LayerEnum == layerEnum);
            if (LayerList == null)
            {
                //Log.Write("DrillStatus", "LayerList가 null입니다. 초기화되지 않았습니다.");
                return null;
            }

            if (LayerList.Count == 0)
            {
                //Log.Write("DrillStatus", $"LayerList에 데이터가 없습니다. 요청된 LayerEnum: {layerEnum}");
                return null;
            }

            var layer = LayerList.FirstOrDefault(l => l.LayerEnum == layerEnum);
            if (layer == null)
            {
                Log.Write("DrillStatus", $"GetLayer 실패 - 요청한 LayerEnum({layerEnum}) 이 존재하지 않습니다.");
            }

            return layer;
        }

        public SocketProcessData GetSocket(string layerName, int socketNumber)
        {
            var layer = GetLayer(layerName);
            return layer?.GetSocket(socketNumber);
        }

        public SocketProcessData GetSocket(Equipment.LayerList layerEnum, int socketNumber)
        {
            var layer = GetLayer(layerEnum);
            return layer?.GetSocket(socketNumber);
        }

        public void InitDrillingManagerFromDrawing(Dictionary<string, int> layerSocketCounts)
        {
            LayerList.Clear();

            foreach (var kvp in layerSocketCounts)
            {
                string layerName = kvp.Key;
                int socketCount = kvp.Value;
                int layerNumber = ParseLayerNumber(layerName);

                Equipment.LayerList layerEnum = Equipment.LayerList.PreAlign;
                Equipment.LayerType layerType = Equipment.LayerType.LAYER_PREALIGN;

                if (layerName.StartsWith("Hole") && layerNumber >= 1 && layerNumber <= 50)
                {
                    layerEnum = (Equipment.LayerList)(layerNumber - 1);
                    layerType = Equipment.LayerType.LAYER_DRILLING;
                }
                else if (layerName == "Outline")
                {
                    layerEnum = Equipment.LayerList.Outline;
                    layerType = Equipment.LayerType.LAYER_OUTLINE;
                }
                else if (layerName == "Thruhole")
                {
                    layerEnum = Equipment.LayerList.Thruhole;
                    layerType = Equipment.LayerType.LAYER_THRUHOLE;
                }
                else if (layerName == "Marking")
                {
                    layerEnum = Equipment.LayerList.Marking;
                    layerType = Equipment.LayerType.LAYER_MARKING;
                }
                else if (layerName == "PreAlign")
                {
                    layerEnum = Equipment.LayerList.PreAlign;
                    layerType = Equipment.LayerType.LAYER_PREALIGN;
                }
                else if (layerName == "Fiducial")
                {
                    layerEnum = Equipment.LayerList.Fiducial;
                    layerType = Equipment.LayerType.LAYER_FIDUCIAL;
                }

                var layer = new LayerProcessData
                {
                    LayerName = layerName,
                    LayerNumber = layerNumber,
                    LayerEnum = layerEnum,
                    LayerType = layerType
                };

                if (layerEnum != Equipment.LayerList.Hole1 && layerEnum.ToString().StartsWith("Hole"))
                {
                    // Hole1 구조 복사
                    var baseLayer = LayerList.FirstOrDefault(l => l.LayerEnum == Equipment.LayerList.Hole1);
                    if (baseLayer != null)
                    {
                        foreach (var baseSocket in baseLayer.SocketList)
                        {
                            layer.SocketList.Add(new SocketProcessData
                            {
                                SocketNumber = baseSocket.SocketNumber,
                                DisplacementZ = baseSocket.DisplacementZ,
                                SocketRotationCenterX = baseSocket.SocketRotationCenterX,
                                SocketRotationCenterY = baseSocket.SocketRotationCenterY,
                                SocketOffsetX = baseSocket.SocketOffsetX,
                                SocketOffsetY = baseSocket.SocketOffsetY,
                                SocketTheta = baseSocket.SocketTheta,
                                IsSocketAligned = baseSocket.IsSocketAligned,
                                IsGoldPowderAligned = baseSocket.IsGoldPowderAligned,
                                GoldRotationCenterX = baseSocket.GoldRotationCenterX,
                                GoldRotationCenterY = baseSocket.GoldRotationCenterY,
                                GoldOffsetX = baseSocket.GoldOffsetX,
                                GoldOffsetY = baseSocket.GoldOffsetY,
                                GoldTheta = baseSocket.GoldTheta,
                                //IsDrilled = baseSocket.IsDrilled,
                                //IsSuccess = baseSocket.IsSuccess,
                                IsDrilled = false,
                                IsSuccess = false,
                                IsUsedInThisLayer = baseSocket.IsUsedInThisLayer
                            });
                        }
                    }
                    else
                    {
                        // Hole1이 없으면 기본 소켓 생성
                        for (int i = 0; i < socketCount; i++)
                            layer.SocketList.Add(new SocketProcessData { SocketNumber = i });
                    }
                }
                else
                {
                    // 일반 레이어 (Marking 등): 직접 생성
                    for (int i = 0; i < socketCount; i++)
                        layer.SocketList.Add(new SocketProcessData { SocketNumber = i });
                }

                LayerList.Add(layer);
                Log.Write("DrillStatus", $"Layer 추가됨: {layer.LayerName} (Enum={layerEnum}, Type={layerType}) - 소켓 {layer.SocketList.Count}개");
            }

            MarkAsChanged();
        }

        private int ParseLayerNumber(string layerName)
        {
            if (layerName.StartsWith("Hole") && int.TryParse(layerName.Substring(4), out int num))
                return num;

            return -1;
        }

        public class LayerDrillingStatus
        {
            public int NotStarted { get; set; }
            public int InProgress { get; set; }
            public int Completed { get; set; }

            public override string ToString()
            {
                return $"미진행: {NotStarted}개, 진행중: {InProgress}개, 완료: {Completed}개";
            }
        }

        /// <summary>
        /// 모든 Layer에 존재하는 소켓 수의 총합을 반환
        /// IsUsedInThisLayer == true 인 경우만 포함하려면 filter 파라미터 활용
        /// </summary>
        public int GetTotalSocketCount(bool onlyUsedSockets = false)
        {
            int count = 0;

            foreach (var layer in LayerList)
            {
                if (onlyUsedSockets)
                    count += layer.SocketList.Count(s => s.IsUsedInThisLayer);
                else
                    count += layer.SocketList.Count;
            }

            return count;
        }

        /// <summary>
        /// 모든 Layer 중 가장 많은 소켓 개수를 가진 Layer의 소켓 수를 반환
        /// onlyUsedSockets == true 인 경우 IsUsedInThisLayer == true 인 소켓만 계산
        /// </summary>
        public int GetMaxSocketCountPerLayer(bool onlyUsedSockets = false)
        {
            int maxCount = 0;

            foreach (var layer in LayerList)
            {
                // 필요한 레이어만 필터링
                if (layer.LayerType != Equipment.LayerType.LAYER_DRILLING &&
                    layer.LayerType != Equipment.LayerType.LAYER_THRUHOLE &&
                    layer.LayerType != Equipment.LayerType.LAYER_OUTLINE &&
                    layer.LayerType != Equipment.LayerType.LAYER_MARKING)
                    continue;

                int count = onlyUsedSockets
                    ? layer.SocketList.Count(s => s.IsUsedInThisLayer)
                    : layer.SocketList.Count;

                if (count > maxCount)
                    maxCount = count;
            }

            return maxCount;
        }

        public LayerDrillingStatus GetLayerDrillingStatus(Equipment.LayerList layerEnum)
        {
            var layer = GetLayer(layerEnum);
            if (layer == null) return new LayerDrillingStatus();

            var status = new LayerDrillingStatus();

            foreach (var socket in layer.SocketList)
            {
                if (!socket.IsDrilled)
                    status.NotStarted++;
                else if (socket.IsDrilled && !socket.IsSuccess)
                    status.InProgress++;
                else if (socket.IsDrilled && socket.IsSuccess)
                    status.Completed++;
            }

            return status;
        }

        public void MarkAsChanged()
        {
            _hasChanged = true;
        }

        public bool HasChanged()
        {
            if (_hasChanged)
            {
                _hasChanged = false; // 자동 리셋
                return true;
            }
            return false;
        }

        public void SaveLotLog(bool bOkNg = true)
        {
            string logFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LotLog");
            if (!Directory.Exists(logFolder))
                Directory.CreateDirectory(logFolder);

            string logFile = Path.Combine(logFolder, $"LotLog_{DateTime.Now:yyyyMMdd}.csv");

            string startTime = CycleTimer_LaserDrilling.ProcessStartTime.ToString("yyyy-MM-dd HH:mm:ss");
            string endTime = CycleTimer_LaserDrilling.ProcessEndTime.ToString("yyyy-MM-dd HH:mm:ss");
            string recipeName = Path.GetFileName(Equipment.Current_Recipe);
            string drawingName = Path.GetFileName(Equipment.Current_DrawingFileName);
            int count = 0;      // 전체 제품 수
            int countNg = 0;    // NG 수
            int markingNumber = Equipment.m_nSerialNumberMarkingCount;  // 넣기에는 또.. 경우의 수가 너무 많다.

            List<string> lines = new List<string>();

            try
            {
                if (File.Exists(logFile))
                {
                    using (FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                            lines.Add(reader.ReadLine());
                    }
                }

                bool isUpdated = false;
                int updatedCount = 0;
                int updatedCountNg = 0;

                if (lines.Count > 0)
                {
                    string lastLine = lines.Last();
                    var parts = lastLine.Split(',');
                    if (parts.Length >= 6 && parts[2] == recipeName)
                    {
                        string originalStartTime = parts[0];
                        if (int.TryParse(parts[4], out int prevCount) && int.TryParse(parts[5], out int prevCountNg))
                        {
                            updatedCount = prevCount + 1;
                            if(bOkNg)
                            {
                                updatedCountNg = prevCountNg;
                            }
                            else
                            {
                                updatedCountNg = prevCountNg + 1;
                            }
                            string updatedLine = $"{originalStartTime},{endTime},{recipeName},{drawingName},{updatedCount},{updatedCountNg}";
                            lines[lines.Count - 1] = updatedLine;
                            isUpdated = true;

                            Log.Write("DrillStatus", $"LOT 로그 저장 완료: Recipe={recipeName}, Count={updatedCount}, NG={updatedCountNg}");
                        }
                    }
                }

                if (!isUpdated)
                {
                    count += 1;
                    if (bOkNg)
                    {
                        countNg = 0;
                    }
                    else
                    {
                        countNg += 1;
                    }

                    string newLine = $"{startTime},{endTime},{recipeName},{drawingName},{count},{countNg}";
                    lines.Add(newLine);

                    Log.Write("DrillStatus", $"LOT 로그 저장 완료: Recipe={recipeName}, Count={count}, NG={countNg}");
                }

                using (FileStream fs = new FileStream(logFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                using (StreamWriter writer = new StreamWriter(fs, new UTF8Encoding(true)))
                {
                    foreach (string line in lines)
                        writer.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Log.Write("DrillStatus", $"LOT 로그 저장 실패: {ex.Message}");
            }
        }

    }
}
