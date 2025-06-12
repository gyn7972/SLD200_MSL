using System;
using System.Collections.Generic;
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
        public double DisplacementZ { get; set; }

        // Socket 얼라인 결과
        public bool IsSocketAligned { get; set; }
        public double SocketRotationCenterX { get; set; }
        public double SocketRotationCenterY { get; set; }
        public double SocketOffsetX { get; set; }
        public double SocketOffsetY { get; set; }
        public double SocketTheta { get; set; }

        // GoldPowder 얼라인 결과
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

        public bool IsUsedInThisLayer { get; set; } = false;

        public bool IsSelected { get; set; } = false;

        public void Reset()
        {
            DisplacementZ = 0;
            IsSocketAligned = false;
            SocketRotationCenterX = SocketRotationCenterY = 0;
            SocketOffsetX = SocketOffsetY = SocketTheta = 0;
            IsGoldPowderAligned = false;
            GoldRotationCenterX = GoldRotationCenterY = 0;
            GoldOffsetX = GoldOffsetY = GoldTheta = 0;
            IsDrilled = false;
            IsSuccess = false;
            IsUsedInThisLayer = false;
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
        public int CycleTimer_NGSocketCount { get; set; } = 0;
        //  workStage 가공시간 계산을 위해 사용되는 변수
        public CycleTimer CycleTimer_LaserDrilling { get; set; } = new CycleTimer();
    
        public void ResetAll()
        {
            foreach (var layer in LayerList)
                layer.Reset();
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
                                IsDrilled = baseSocket.IsDrilled,
                                IsSuccess = baseSocket.IsSuccess,
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
    }
}
