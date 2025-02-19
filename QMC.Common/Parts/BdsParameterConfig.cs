using QMC.Common.VisionPart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Windows.Forms;

using SpiralLab.Sirius;

//using OpenTK;
//using OpenTK.Graphics.OpenGL;
//using SpiralLab.Sirius2;
//using SpiralLab.Sirius2.Laser;
//using SpiralLab.Sirius2.PowerMeter;
//using SpiralLab.Sirius2.Scanner;
//using SpiralLab.Sirius2.Scanner.Rtc;
//using SpiralLab.Sirius2.Winforms;
//using SpiralLab.Sirius2.Winforms.Entity;
//using SpiralLab.Sirius2.Winforms.Marker;
//using SpiralLab.Sirius2.Winforms.UI;

namespace QMC.Common.Parts
{
    [Serializable]
    public class BdsParameterConfig
    {
        protected List<YPositionData> m_BdsPosition;

        //  Bds 에서 사용되는 위치
        public enum PositionBds
        {
            None_Mask,                          //  마스크 없는 위치
            Mask_1,                             //  마스크 1 위치
            Mask_2,                             //  마스크 2 위치
            Mask_3,                             //  마스크 3 위치
            Mask_4,                             //  마스크 4 위치


            //Ready,                              //  대기 위치
            //Picker_MovablePos_Z,                //  Loader 가 충돌 없이 움직일 수 있는 위치 (Z축)
            //Picker_PNL_PutDown,                 //  PNL 을 내려놓는 위치 (Work Table)
            //Picker_Stack0_PickUp_Ready,         //  Stacker0 에서 제품 PickUp 대기 위치
            //Picker_Stack0_PickUp,               //  Stacker0 에서 제품 PickUp 위치
            //Picker_Stack1_PickUp_Ready,         //  Stacker1 에서 제품 PickUp 대기 위치
            //Picker_Stack1_PickUp,               //  Stacker1 에서 제품 PickUp 위치
            //Stacker0_Top,                       //  제품이 비어있는 상태 (Stacker0 이 위로 올라간 위치)
            //Stacker0_Bottom,                    //  제품이 가득 찬 상태 (Stacker0 이 아래로 내려간 위치)
            //Stacker1_Top,                       //  제품이 비어있는 상태 (Stacker1 이 위로 올라간 위치)
            //Stacker1_Bottom,                    //  제품이 가득 찬 상태 (Stacker1 이 아래로 내려간 위치)
            //Aligner_Product_PickUp,             //  얼라인 완료된 자재를 PickUp 하는 위치
            //Aligner_Product_PutDown,            //  얼라인을 위해 자재를 내려놓는 위치
            //Aligner_FullOpen,                   //  얼라이너 전체 Open 위치
            //Aligner_Close100mm,                 //  얼라이너 전체 Close 위치
        }

        public YPositionDataCollection BdsPositions { set; get; }

        #region Laser/ Scanner Parameter
        [Browsable(false)]
        public List<YPositionData> ConfigBdsPositions
        {
            get { return m_BdsPosition; }
            set { m_BdsPosition = value; }
        }



        /// <summary>
        /// Loader Parameter
        /// </summary>
        /// 
        //[Category("[01] 웨이퍼 얼라인"),
        //    Description("두번째 얼라인 마크 옵셋 X (mm)"),
        //    DisplayName("공통 - 두번째 얼라인 마크 옵셋 X (mm)")]
        //public double AlignMark_2nd_Offset_X { set; get; }
        
        //[Category("[01] 웨이퍼 얼라인"),
        //    Description("두번째 얼라인 마크 옵셋 Y (mm)"),
        //    DisplayName("공통 - 두번째 얼라인 마크 옵셋 Y (mm)")]
        //public double AlignMark_2nd_Offset_Y { set; get; }

        [Category("[01] BDS"),
            Description("얼라인 - 비전 허용 오차 (Theta Deg, °)"),
            DisplayName("얼라인 - 비전 허용 오차 (Theta Deg, °)")]
        public double Align_Vision_Allowable_Angle { set; get; }

        [Category("[01] BDS"),
            Description("얼라인 - 비전 허용 오차 (XY, mm)"),
            DisplayName("얼라인 - 비전 허용 오차 (XY, mm)")]
        public double Align_Vision_Allowable_XY { set; get; }

        [Category("[01] BDS"),
            Description("얼라인 위치 평균 계산 - 프로브 카드의 얼라인 마크 검출 시, 이 회수만큼 마크를 찾은 후 평균 위치값을 사용한다.\r\n[default 0 : 1회]"),
            DisplayName("얼라인 위치 평균 계산 - 프로브 카드 얼라인 마크 측정 회수")]
        public int ProbeCard_AlignMarkCount_forAverage { set; get; }

        [Category("[01] BDS"),
            Description("얼라인 위치 평균 계산 - 웨이퍼의 얼라인 마크 검출 시, 이 회수만큼 마크를 찾은 후 평균 위치값을 사용한다.\r\n[default 0 : 1회]"),
            DisplayName("얼라인 위치 평균 계산 - 웨이퍼 얼라인 마크 측정 회수")]
        public int Wafer_AlignMarkCount_forAverage { set; get; }

        [Category("[01] BDS"),
            Description("얼라인 후 오차 확인 - 비전 허용 오차 (XY)\r\n\r\n(프로브 핀 위치 기준 웨이퍼 허용 오차 범위"),
            DisplayName("얼라인 후 오차 확인 - 비전 허용 오차 (XY)")]
        public double AlignError_Vision_Allowable_XY { set; get; }

        #endregion



        #region Constructor
        public BdsParameterConfig()
        {
            Init();

            //Scanner_KFactor = 1000.0;
        }
        //public List<ZPositionData> Positions { set; get; }
        #endregion

        protected void Init()
        {
            if (BdsPositions == null)
                BdsPositions = new YPositionDataCollection();

            m_BdsPosition = new List<YPositionData>();
            m_BdsPosition.Clear();

            BdsPositions.Clear();



            foreach (PositionBds key in Enum.GetValues(typeof(PositionBds)))
            {
                YPositionData positionBase = new YPositionData();

                positionBase.Name = key.ToString();
                m_BdsPosition.Add(positionBase);
                BdsPositions.Add(positionBase);

                YPositionData positionTarget = new YPositionData();

                //positionTarget.Name = key.ToString();                     //  Offset 부분까지 Name 이 보이면 Position 별 구분이 잘 안됨.
                positionTarget.Type = TargetType.Offset;
                m_BdsPosition.Add(positionTarget);
                BdsPositions.Add(positionTarget);
            }
        }

        public List<string> GetLoaderPositionList()
        {
            List<string> ret = new List<string>();
            foreach (YPositionData position in m_BdsPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public int SetData(int nStartIndex, SettingParameterCollection parameters)
        {
            int i = nStartIndex;
            foreach (YPositionData pos in m_BdsPosition)
            {
                //  BDS Mask Y
                if (i >= parameters.Count)
                    return i;
                pos.Y = parameters[i++].DoubleValue;
            }

            return i;
        }

        public SettingParameterCollection GetData()
        {
            SettingParameterCollection parameters = new SettingParameterCollection();

            foreach (YPositionData pos in m_BdsPosition)
            {
                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Y;
                    param.Tag = BdsParameter.MotionKey.Y.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }
            }

            return parameters;
        }
    }
}
