using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SLD200_MSL;

namespace QMC.Common
{
    public static class ProcessManager
    {
        public static List<LayerInfo> Layers { get; private set; } = new List<LayerInfo>();

        // 초기화 (초기 생성)
        public static void Init()
        {
            Layers.Clear();
        }

        // Layer 추가
        public static LayerInfo AddLayer(string layerName, int layerNumber)
        {
            var layer = new LayerInfo(layerName, layerNumber);
            Layers.Add(layer);
            return layer;
        }

        // 전체 리셋 (모든 소켓 초기화)
        public static void Reset()
        {
            foreach (var layer in Layers)
            {
                layer.ResetSockets();
            }
        }

        // 특정 Layer 찾기
        public static LayerInfo GetLayer(int layerNumber)
        {
            return Layers.FirstOrDefault(l => l.LayerNumber == layerNumber);
        }
    }
}
