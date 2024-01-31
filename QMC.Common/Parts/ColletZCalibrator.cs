using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Parts
{
    public class ColletZCalibrator : Part
    {
        public Turret turret { get; set; }
        public LoadZ Mover { set; get; }
        public Collet m_Collet { set; get; }
        public int m_nRevisionColletIndex{ set; get; }
        private double m_dResult { set; get; }


        public ColletZCalibrator(string strName) : base(strName)
        {
            m_nRevisionColletIndex = 1;
        }
        public double GetResultData()
        {
            double dResult = 0;

            dResult = m_dResult;

            return dResult;
        }
        private double GetActualPosition()
        {
            double dPosition = 0;
            Mover.Axes["Z"].GetActualPosition(ref dPosition);
           
            return dPosition;
        }
        private double GetPich()
        {
            double dPitch = 0;
            int nCount = GetPichCount();
            dPitch = Mover.LoadZConfig.m_listCalibrationpitch[nCount - 1].pitchZ;
            return dPitch;
        }

        private int GetPichCount()
        {
            int nCount = Mover.LoadZConfig.m_listCalibrationpitch.Count;
            return nCount;
        }
        public void OnRun()
        {
            Collet collet = null;//new Collet();
            double dRecive = 1;
            // pitch Control의 제일 마지막 pitch를 Load
            double dPitch = GetPich();
            // 현재 Z 값 얻어오고
            double dActualPos = GetActualPosition();
            double dTargetPos = 0;
            while (true)
            {
                //현재값 확인
                dActualPos = GetActualPosition();
                dTargetPos = dActualPos - dPitch;
                //현재값 - Pitch만큼 이동(하강)..  ReciveData가 0이 될때까지 pitch만큼씩 하강하다가
                Mover.MovePosition(dActualPos - dPitch);
                //하강 후 ReciveData 확인
                dRecive = ReciveData(collet);
                //현재 TargetPos를 현재 Collet[Index]의 OffsetZ로 저장...
                
                if (dRecive == 0)
                {
                    m_dResult = dTargetPos;
                    turret.Config.ColletZOffsets[m_nRevisionColletIndex].dOffsetZ = m_dResult; // OffsetList에 Result 저장
                    //Recive가 0이 되면 한 pitch 상승
                    Mover.MovePosition(dActualPos + dPitch);
                    break;
                }
            }
        
        }
        
        public double ReciveData(Collet collet)
        {
            double dSencerValue = 0;

            if(collet != null)
            {
                if(collet.GetPresentValue(ref dSencerValue) != 0)
                {
                    return -1;
                }
            }

            return dSencerValue;
        }
    }
}
