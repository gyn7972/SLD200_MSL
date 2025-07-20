using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QMC.Common.VisionPart.MCVisionCalibrator;

namespace QMC.Common.Global
{
    public class AxisMap
    {
        /// <summary>
        /// 축 키 정의: 모듈별로 의미 있는 축을 명확히 구분
        /// </summary>
        public enum AxisKey
        {
            Loader_Z0,          //Z0
            Loader_Z1,          //Z1
            Loader_TR_X,        //TR_X 
            Loader_TR_Z,        //TR_Z 
            Loader_ALN_X,       //ALN_X 
            Loader_ALN_Y,       //ALN_Y 

            Unloader_Z0,        //Z0
            Unloader_Z1,        //Z1
            Unloader_TR_X,      //TR_X
            Unloader_TR_Z,      //TR_Z

            Stage_X,            //X
            Stage_Y,            //Y
            Stage_Z,            //Z
            Stage_MASK_Y,       //MASK_Y
        }

        private static Dictionary<AxisKey, int> _axisMap = new Dictionary<AxisKey, int>();

        public static void Init(bool isCo2)
        {
            _axisMap.Clear();

            if (isCo2) // CO₂ (SLD-200C)
            {
                _axisMap[AxisKey.Loader_Z0] = 6;
                _axisMap[AxisKey.Loader_Z1] = 7;
                _axisMap[AxisKey.Loader_TR_X] = 8;
                _axisMap[AxisKey.Loader_TR_Z] = 9;
                _axisMap[AxisKey.Loader_ALN_X] = 1;
                _axisMap[AxisKey.Loader_ALN_Y] = 2;

                _axisMap[AxisKey.Unloader_Z0] = 10;
                _axisMap[AxisKey.Unloader_Z1] = 11;
                _axisMap[AxisKey.Unloader_TR_X] = 12;
                _axisMap[AxisKey.Unloader_TR_Z] = 13;

                _axisMap[AxisKey.Stage_X] = 3;
                _axisMap[AxisKey.Stage_Y] = 4;
                _axisMap[AxisKey.Stage_Z] = 5;
                _axisMap[AxisKey.Stage_MASK_Y] = 0;
            }
            else // UV (SLD-200U)
            {
                _axisMap[AxisKey.Loader_Z0] = 5;
                _axisMap[AxisKey.Loader_Z1] = 6;
                _axisMap[AxisKey.Loader_TR_X] = 7;
                _axisMap[AxisKey.Loader_TR_Z] = 8;
                _axisMap[AxisKey.Loader_ALN_X] = 0;
                _axisMap[AxisKey.Loader_ALN_Y] = 1;

                _axisMap[AxisKey.Unloader_Z0] = 9;
                _axisMap[AxisKey.Unloader_Z1] = 10;
                _axisMap[AxisKey.Unloader_TR_X] = 11;
                _axisMap[AxisKey.Unloader_TR_Z] = 12;

                _axisMap[AxisKey.Stage_X] = 2;
                _axisMap[AxisKey.Stage_Y] = 3;
                _axisMap[AxisKey.Stage_Z] = 4;
                _axisMap[AxisKey.Stage_MASK_Y] = 13;
            }
        }

        public static int Get(AxisKey key)
        {
            if (_axisMap.TryGetValue(key, out int axisNo))
                return axisNo;

            throw new KeyNotFoundException($"축 키가 정의되어 있지 않습니다: {key}");
        }
    }
}
