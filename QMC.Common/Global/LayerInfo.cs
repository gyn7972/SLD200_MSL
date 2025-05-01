using QMC.Common;
using System.Collections.Generic;
using System.Linq;

namespace QMC.Common
{
    public class LayerInfo
    {
        public string LayerName { get; set; }
        public int LayerNumber { get; set; }
        public List<AreaInfo> Areas { get; set; }

        public LayerInfo(string name, int number)
        {
            LayerName = name;
            LayerNumber = number;
            Areas = new List<AreaInfo>();
        }

        public void AddArea(int areaIndex)
        {
            if (!Areas.Any(a => a.AreaIndex == areaIndex))
                Areas.Add(new AreaInfo(areaIndex));
        }

        public void SetAreaProcessed(int areaIndex, bool result, string note = "")
        {
            var area = Areas.FirstOrDefault(a => a.AreaIndex == areaIndex);
            if (area != null)
            {
                area.IsProcessed = result;
                area.Note = note;
            }
        }

        public void ResetAreas()
        {
            foreach (var area in Areas)
                area.Reset();
        }
    }
}
