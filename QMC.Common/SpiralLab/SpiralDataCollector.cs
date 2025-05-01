using System;
using System.Collections.Generic;
using System.Linq;

namespace QMC.Common.SpiralLab
{
    public class SpiralDataCollector
    {
        private List<SpialData> spialDataList;

        public SpiralDataCollector()
        {
            spialDataList = new List<SpialData>();
        }

        // SpialData 추가
        public SpialData AddSpialData(SpialData spialData)
        {
            if (spialData == null)
            {
                return null;
            }
                
            if (spialDataList.Contains(spialData))
            {
                return spialDataList.Where(t=> t == spialData).ToList().FirstOrDefault();
                
            }
            spialData.SpiralData_Create(spialData.GetOuterDiameter(), spialData.GetInnerDiameter(), spialData.GetRevolutions(), spialData.GetAngleFactor(), 0, 0);
            spialDataList.Add(spialData);
        }

        // SpialData 삭제
        public bool RemoveSpialData(SpialData spialData)
        {
            return spialDataList.Remove(spialData);
        }

        // 특정 조건에 맞는 SpialData 검색
        public List<SpialData> FindSpialData(Func<SpialData, bool> predicate)
        {
            return spialDataList.Where(predicate).ToList();
        }

        // 모든 SpialData 반환
        public List<SpialData> GetAllSpialData()
        {
            return new List<SpialData>(spialDataList);
        }

        // SpialData 정렬
        public void SortSpialData(Comparison<SpialData> comparison)
        {
            spialDataList.Sort(comparison);
        }

        // SpialData 개수 반환
        public int Count => spialDataList.Count;

        // SpialData 초기화
        public void Clear()
        {
            spialDataList.Clear();
        }
    }
}
