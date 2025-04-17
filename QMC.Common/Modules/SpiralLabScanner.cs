using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static QMC.Common.Equipment;
using static QMC.Common.Modules.WorkStage;
using SpiralLab.Sirius;
using LaserVirtual = SpiralLab.Sirius.LaserVirtual;
using QMC.Common.Parts;
using QMC.Common.VisionPart;

namespace QMC.Common.Modules
{
    internal class SpiralLabScanner
    {
        
    static WorkStage workStage;
        public LaserVirtual laser { set; get; }
        public SpiralLabScanner()
        {
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }

        }

        public void Module_Allocation()
        {
            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                //if (module.Name == "WorkStage")
                if (module.Name == "WorkStage")
                {
                    workStage = module as WorkStage;
                }
            }
        }

        /// <summary>
        /// 스캐너 초기화
        /// </summary>
        private void InitializeScanner()
        {
            try
            {
                //_rtc.Initialize()

                //// RTC 초기화 (예: RTC5 사용)
                //_rtc = new Rtc6(0, "RTC5ETH", "192.168.1.100", 10000);
                //_rtc.Initialize();
                //Console.WriteLine("Scanner initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing scanner: {ex.Message}");
            }
        }

        /// <summary>
        /// 우측 상단 기준(0,0)으로 레이저를 중심에서 발진하여 십자가를 찍는 함수
        /// </summary>
        public void DrawCalibrationCrosses(int rows, int cols, float pitchX, float pitchY)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Rows and columns must be greater than zero.");

            float crossSize = 1.0f; // 1mm 십자가

            //float fFrequency = 50_000.0f;
            //float fPulseWidth = 2.0f;
            //workStage.rtc.CtlFrequency(fFrequency, fPulseWidth);

            float fJumpSpeed = 1000.0f;
            float fMarkSpeed = 200.0f;
            workStage.rtc.CtlSpeed(fJumpSpeed, fMarkSpeed);

            float fLaserOnDelay = 10.0f;
            float fLaserOffDelay = 100.0f;
            float fMarkDelay = 200.0f;
            float fJumpDelay = 200.0f;
            float fPolygonDelay = 0.0f;
            workStage.rtc.CtlDelay(fLaserOnDelay, fLaserOffDelay, fMarkDelay, fJumpDelay, fPolygonDelay);

            //float fLaserPower = 2.0f;
            //workStage.laser.CtlPower(fLaserPower);

            workStage.rtc.ListBegin(laser, ListType.Single);

            // 중심 기준 좌표로 시작점 계산
            float startX = -((cols - 1) * pitchX) / 2.0f;
            float startY = -((rows - 1) * pitchY) / 2.0f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float centerX = startX + col * pitchX;
                    float centerY = startY + row * pitchY;

                    DrawCross(centerX, centerY, crossSize);
                }
            }

            workStage.rtc.ListEnd();
            workStage.rtc.ListExecute();
        }


        /// <summary>
        /// 주어진 중심 좌표에 1mm 크기의 십자가를 그리는 함수
        /// </summary>
        /// <param name="centerX">십자가 중심의 X 좌표</param>
        /// <param name="centerY">십자가 중심의 Y 좌표</param>
        /// <param name="size">십자가의 크기 (mm)</param>
        private void DrawCross(float centerX, float centerY, float size)
        {
            float halfSize = size / 2;

            // 가로선 그리기
            workStage.rtc.ListJump(centerX - halfSize, centerY);
            workStage.rtc.ListMark(centerX + halfSize, centerY);

            // 세로선 그리기
            workStage.rtc.ListJump(centerX, centerY - halfSize);
            workStage.rtc.ListMark(centerX, centerY + halfSize);
        }

        /// <summary>
        /// 동작 완료 여부를 확인하는 함수
        /// </summary>
        /// <returns>동작이 완료되었으면 true, 그렇지 않으면 false</returns>
        public bool IsOperationComplete()
        {
            try
            {
                // IRtc의 IsBusy 메서드 또는 유사한 메서드를 사용
                return !workStage.rtc.CtlGetStatus(RtcStatus.Busy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking operation status: {ex.Message}");
                return false;
            }
        }





    }
}
