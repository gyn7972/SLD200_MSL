using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMC.Common.Global;
using SLD200_MSL;

namespace QMC.Common
{
    public class SocketInfo
    {
        public int SocketNumber { get; set; }
        public bool InspectionResult { get; set; }
        public string AdditionalInfo { get; set; }

        public List<LayerInfo> Layers { get; set; }

        public AlignData AlignData { get; set; } = new AlignData();

        public SocketInfo(int socketNumber)
        {
            SocketNumber = socketNumber;
            InspectionResult = false;
            AdditionalInfo = string.Empty;
            Layers = new List<LayerInfo>();
            AlignData = new AlignData();
        }

        public void AddLayer(string name, int number)
        {
            if (!Layers.Any(l => l.LayerNumber == number))
                Layers.Add(new LayerInfo(name, number));
        }

        public LayerInfo GetLayer(string name)
        {
            return Layers.FirstOrDefault(l => l.LayerName == name);
        }

        public void Reset()
        {
            InspectionResult = false;
            AdditionalInfo = string.Empty;
            AlignData = new AlignData();
            foreach (var layer in Layers)
                layer.ResetAreas();
        }
    }
}
