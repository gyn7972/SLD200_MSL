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
    public class LoaderParameterConfig
    {
        protected List<ZzxzxyPositionData> m_LoaderPosition;

        //  Loader 장비에서 사용되는 위치
        public enum PositionLoader
        {
            R_Port_Ready,                       //  우측 Stacker 가 내려간 위치
            R_Port_Top,                         //  우측 Stacker 가 올라간 위치
            L_Port_Ready,                       //  좌측 Stacker 가 내려간 위치
            L_Port_Top,                         //  좌측 Stacker 가 올라간 위치
            Transfer_To_R_Port,                 //  Transfer 가 우측 Stacker 의 Module Pickup 위치로 이동
            Transfer_To_L_Port,                 //  Transfer 가 좌측 Stacker 의 Module Pickup 위치로 이동
            Transfer_To_M_Aligner,              //  Transfer 가 Aligner 의 Module Pickup & Putdown 위치로 이동
            Transfer_To_WorkStage,              //  Transfer 가 Work Stage 의 Module Putdown 위치로 이동
            M_Aligner_Open,                     //  Aligner 가 Open 된 위치
            M_Aligner_Close,                    //  Aligner 가 Close 된 위치
            M_Aligner_Gap_100mm,                //  Aligner 의 간격이 100mm 일 때 위치 (Module 크기에 따라 계산해서 사용하기 위한 기준 위치값)  


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

        public ZzxzxyPositionDataCollection LoaderPositions { set; get; }

        #region Laser/ Scanner Parameter
        [Browsable(false)]
        public List<ZzxzxyPositionData> ConfigLoaderPositions
        {
            get { return m_LoaderPosition; }
            set { m_LoaderPosition = value; }
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

        [Category("[01] Loader"),
            Description("얼라인 - 비전 허용 오차 (Theta Deg, °)"),
            DisplayName("얼라인 - 비전 허용 오차 (Theta Deg, °)")]
        public double Align_Vision_Allowable_Angle { set; get; }

        [Category("[01] Loader"),
            Description("얼라인 - 비전 허용 오차 (XY, mm)"),
            DisplayName("얼라인 - 비전 허용 오차 (XY, mm)")]
        public double Align_Vision_Allowable_XY { set; get; }

        [Category("[01] Loader"),
            Description("얼라인 위치 평균 계산 - 프로브 카드의 얼라인 마크 검출 시, 이 회수만큼 마크를 찾은 후 평균 위치값을 사용한다.\r\n[default 0 : 1회]"),
            DisplayName("얼라인 위치 평균 계산 - 프로브 카드 얼라인 마크 측정 회수")]
        public int ProbeCard_AlignMarkCount_forAverage { set; get; }

        [Category("[01] Loader"),
            Description("얼라인 위치 평균 계산 - 웨이퍼의 얼라인 마크 검출 시, 이 회수만큼 마크를 찾은 후 평균 위치값을 사용한다.\r\n[default 0 : 1회]"),
            DisplayName("얼라인 위치 평균 계산 - 웨이퍼 얼라인 마크 측정 회수")]
        public int Wafer_AlignMarkCount_forAverage { set; get; }

        [Category("[01] Loader"),
            Description("얼라인 후 오차 확인 - 비전 허용 오차 (XY)\r\n\r\n(프로브 핀 위치 기준 웨이퍼 허용 오차 범위"),
            DisplayName("얼라인 후 오차 확인 - 비전 허용 오차 (XY)")]
        public double AlignError_Vision_Allowable_XY { set; get; }

        #endregion



        #region Constructor
        public LoaderParameterConfig()
        {
            Init();

            //Scanner_KFactor = 1000.0;
        }
        //public List<ZPositionData> Positions { set; get; }
        #endregion

        protected void Init()
        {
            if (LoaderPositions == null)
                LoaderPositions = new ZzxzxyPositionDataCollection();

            m_LoaderPosition = new List<ZzxzxyPositionData>();
            m_LoaderPosition.Clear();

            LoaderPositions.Clear();



            foreach (PositionLoader key in Enum.GetValues(typeof(PositionLoader)))
            {
                ZzxzxyPositionData positionBase = new ZzxzxyPositionData();

                positionBase.Name = key.ToString();
                m_LoaderPosition.Add(positionBase);
                LoaderPositions.Add(positionBase);

                ZzxzxyPositionData positionTarget = new ZzxzxyPositionData();

                //positionTarget.Name = key.ToString();                     //  Offset 부분까지 Name 이 보이면 Position 별 구분이 잘 안됨.
                positionTarget.Type = TargetType.Offset;
                m_LoaderPosition.Add(positionTarget);
                LoaderPositions.Add(positionTarget);
            }
        }

        public List<string> GetLoaderPositionList()
        {
            List<string> ret = new List<string>();
            foreach (ZzxzxyPositionData position in m_LoaderPosition)
            {
                if (position.Type == TargetType.Base)
                    ret.Add(position.Name);
            }
            return ret;
        }

        public int SetData(int nStartIndex, SettingParameterCollection parameters)
        {
            int i = nStartIndex;
            foreach (ZzxzxyPositionData pos in m_LoaderPosition)
            {
                //  Loader Stacker Z0
                if (i >= parameters.Count)
                    return i;
                pos.Z0 = parameters[i++].DoubleValue;

                //  Loader Stacker Z1
                if (i >= parameters.Count)
                    return i;
                pos.Z1 = parameters[i++].DoubleValue;

                //  Loader Transfer X
                if (i >= parameters.Count)
                    return i;
                pos.TR_X = parameters[i++].DoubleValue;

                //  Loader Transfer Z
                if (i >= parameters.Count)
                    return i;
                pos.TR_Z = parameters[i++].DoubleValue;

                //  Loader Align X
                if (i >= parameters.Count)
                    return i;
                pos.ALN_X = parameters[i++].DoubleValue;

                //  Loader Align Y
                if (i >= parameters.Count)
                    return i;
                pos.ALN_Y = parameters[i++].DoubleValue;
            }

            return i;
        }

        public SettingParameterCollection GetData()
        {
            SettingParameterCollection parameters = new SettingParameterCollection();

            foreach (ZzxzxyPositionData pos in m_LoaderPosition)
            {
                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Z0;
                    param.Tag = LoaderParameter.MotionKey.Z0.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.Z1;
                    param.Tag = LoaderParameter.MotionKey.Z1.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.TR_X;
                    param.Tag = LoaderParameter.MotionKey.TR_X.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.TR_Z;
                    param.Tag = LoaderParameter.MotionKey.TR_Z.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.ALN_X;
                    param.Tag = LoaderParameter.MotionKey.ALN_X.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }

                {
                    SettingParameter param = new SettingParameter(pos.Name, DataType.Double);
                    param.DoubleValue = pos.ALN_Y;
                    param.Tag = LoaderParameter.MotionKey.ALN_Y.ToString();
                    param.Spare = pos.Type.ToString();
                    parameters.Add(param);
                }
            }

            return parameters;
        }
    }
}
