using QMC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.Equipment;

namespace QMC.Common.Recipe
{
    public class ScannerCalManager
    {
        public class CalibrationFileInfo
        {
            public int Index { get; set; }
            public double OffsetZ_mm { get; set; }
            public string CalFilePath { get; set; }
        }
        public List<CalibrationFileInfo> ZCalFileList { get; private set; } = new List<CalibrationFileInfo>();
        public CalibrationFileInfo CurrentCalFile { get; private set; } = null;
        public void AddCalFile(double offsetZ_mm, string path)
        {
            int newIndex = ZCalFileList.Count > 0 ? ZCalFileList.Max(f => f.Index) + 1 : 1;
            ZCalFileList.Add(new CalibrationFileInfo
            {
                Index = newIndex,
                OffsetZ_mm = offsetZ_mm,
                CalFilePath = path
            });

            // 1. Index 1번 항목 고정
            var firstItem = ZCalFileList.FirstOrDefault(x => x.Index == 1);
            var restItems = ZCalFileList.Where(x => x.Index != 1)
                                         .OrderByDescending(x => x.OffsetZ_mm)
                                         .ToList();

            // 2. 나머지 항목은 2번부터 다시 인덱싱
            for (int i = 0; i < restItems.Count; i++)
            {
                restItems[i].Index = i + 2;
            }

            // 3. 리스트 재조합
            ZCalFileList = new List<CalibrationFileInfo>();
            if (firstItem != null)
                ZCalFileList.Add(firstItem);

            ZCalFileList.AddRange(restItems);
        }

        public void RemoveCalFile(int index)
        {
            ZCalFileList.RemoveAll(f => f.Index == index);
        }

        public CalibrationFileInfo GetNearestCalFile(double currentZ_um, double threshold_um = 100.0)
        {
            if (ZCalFileList == null || ZCalFileList.Count == 0)
                return null;

            var nearest = ZCalFileList
                .Where(c => Math.Abs(c.OffsetZ_mm - currentZ_um) <= threshold_um)
                .OrderBy(c => Math.Abs(c.OffsetZ_mm - currentZ_um))
                .FirstOrDefault();

            CurrentCalFile = nearest; // 현재 Cal 파일로 저장
            return nearest;
        }

        public void SaveToIni(string path)
        {
            NativeMethods.WritePrivateProfileString("ZCalFile", "Count", ZCalFileList.Count.ToString(), path);

            for (int i = 0; i < ZCalFileList.Count; i++)
            {
                var item = ZCalFileList[i];
                NativeMethods.WritePrivateProfileString("ZCalFile", $"Index{i}", item.Index.ToString(), path);
                NativeMethods.WritePrivateProfileString("ZCalFile", $"OffsetZ{i}", item.OffsetZ_mm.ToString(), path);
                NativeMethods.WritePrivateProfileString("ZCalFile", $"CalFilePath{i}", item.CalFilePath, path);
            }
        }

        public void LoadFromIni(string path)
        {
            ZCalFileList.Clear();
            StringBuilder sb = new StringBuilder(255);

            NativeMethods.GetPrivateProfileString("ZCalFile", "Count", "0", sb, sb.Capacity, path);
            int count = Equipment.ToInt(sb.ToString());

            if (count == 0)
            {
                CalibrationFileInfo info = new CalibrationFileInfo();
                info.Index = 1;
                info.OffsetZ_mm = 0.0;
                if(Machine_LaserType_CO2)
                    info.CalFilePath = "D:\\SLD-200_Parameter\\Cor_200C.ct5";
                else
                    info.CalFilePath = "D:\\SLD-200_Parameter\\Cor_200U.ct5";

                ZCalFileList.Add(info);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    CalibrationFileInfo info = new CalibrationFileInfo();

                    NativeMethods.GetPrivateProfileString("ZCalFile", $"Index{i}", "0", sb, sb.Capacity, path);
                    info.Index = Equipment.ToInt(sb.ToString());

                    NativeMethods.GetPrivateProfileString("ZCalFile", $"OffsetZ{i}", "0", sb, sb.Capacity, path);
                    info.OffsetZ_mm = Equipment.ToDouble(sb.ToString());

                    NativeMethods.GetPrivateProfileString("ZCalFile", $"CalFilePath{i}", "", sb, sb.Capacity, path);
                    info.CalFilePath = sb.ToString();

                    ZCalFileList.Add(info);
                }
            }
        }
    }
}
