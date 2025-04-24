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
using QMC.Common.Modules;

namespace QMC.Common.Parts
{
    //internal class SpiralLabScanner : Part
    public class SpiralLabScanner : Part
    {
        #region Define
        #endregion

        #region Property
        public string ScannerName { get; set; }
        //public ISpiralLabScanner _owner;
        public Rtc6 rtc { get; set; }
        public LaserVirtual laser { set; get; }
        #endregion

        public override int Create()
        {
            
            return base.Create();
        }

        public SpiralLabScanner(string strName)
            : base(strName)
        {
            ScannerName = strName;
        }

        public bool DrawCalibrationCrosses(int rows, int cols, float pitchX, float pitchY, double markLength = 0.5)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Rows and columns must be greater than zero.");

            bool bRtn = false;

            float crossSize = (float)Equipment.Scanner_Calibration_CrossMarkLength; //1.0f; //(float)markLength; 

            if (Equipment.Machine_LaserType_CO2)
            {
                float fFrequency = (float)Equipment.Scanner_Calibration_LaserFrequency;
                float fPulseWidth = (float)Equipment.Scanner_Calibration_LaserPulseWidth;    //2.6f;

                if (fFrequency / 2 <= fPulseWidth)
                    fPulseWidth = fFrequency / 2;
                if (fFrequency <= 0) fFrequency = 0f;
                if (fPulseWidth <= 0) fPulseWidth = 0f;

                if (!rtc.CtlFrequency(fFrequency, fPulseWidth))
                {
                    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Frequency 설정 실패");
                    return false;
                }
            }

            float fJumpSpeed = (float)Equipment.Scanner_Calibration_LaserJumpSpeed;
            float fMarkSpeed = (float)Equipment.Scanner_Calibration_LaserMarkSpeed;
            if (fJumpSpeed <= 0) fJumpSpeed = 0;
            if (fMarkSpeed <= 0) fMarkSpeed = 0;

            if (!rtc.CtlSpeed(fJumpSpeed, fMarkSpeed))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Speed 설정 실패");
                return false;
            }

            float fLaserOnDelay = (float)Equipment.Scanner_Calibration_LaserOnDelay;
            float fLaserOffDelay = (float)Equipment.Scanner_Calibration_LaserOffDelay;
            float fMarkDelay = (float)Equipment.Scanner_Calibration_MarkDelay;
            float fJumpDelay = (float)Equipment.Scanner_Calibration_JumpDelay;
            float fPolygonDelay = (float)Equipment.Scanner_Calibration_PolygonDelay;
            if (fLaserOnDelay <= 0) fLaserOnDelay = 0;
            if (fLaserOffDelay <= 0) fLaserOffDelay = 0;
            if (fMarkDelay <= 0) fMarkDelay = 0;
            if (fJumpDelay <= 0) fJumpDelay = 200;
            if (fPolygonDelay <= 0) fPolygonDelay = 0;

            if (!rtc.CtlDelay(fLaserOnDelay, fLaserOffDelay, fMarkDelay, fJumpDelay, fPolygonDelay))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Delay 설정 실패");
                return false;
            }

            rtc.ListBegin(laser, ListType.Single);
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

            rtc.ListEnd();
            rtc.ListExecute();

            bRtn = true;
            return bRtn;
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
            rtc.ListJump(centerX - halfSize, centerY);
            rtc.ListMark(centerX + halfSize, centerY);

            // 세로선 그리기
            rtc.ListJump(centerX, centerY - halfSize);
            rtc.ListMark(centerX, centerY + halfSize);
        }

        public bool DrawCalibrationArc(int rows, int cols, float pitchX, float pitchY)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Rows and columns must be greater than zero.");

            bool bRtn = false;

            float crossSize = (float)Equipment.Scanner_Calibration_CrossMarkLength; //1.0f; //(float)markLength; 

            if (Equipment.Machine_LaserType_CO2)
            {
                float fFrequency = (float)Equipment.Scanner_Calibration_LaserFrequency;
                float fPulseWidth = (float)Equipment.Scanner_Calibration_LaserPulseWidth;    //2.6f;

                if (fFrequency / 2 <= fPulseWidth)
                    fPulseWidth = fFrequency / 2;
                if (fFrequency <= 0) fFrequency = 0f;
                if (fPulseWidth <= 0) fPulseWidth = 0f;

                if (!rtc.CtlFrequency(fFrequency, fPulseWidth))
                {
                    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Frequency 설정 실패");
                    return false;
                }
            }

            float fJumpSpeed = (float)Equipment.Scanner_Calibration_LaserJumpSpeed;
            float fMarkSpeed = (float)Equipment.Scanner_Calibration_LaserMarkSpeed;
            if (fJumpSpeed <= 0) fJumpSpeed = 0;
            if (fMarkSpeed <= 0) fMarkSpeed = 0;

            if (!rtc.CtlSpeed(fJumpSpeed, fMarkSpeed))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Speed 설정 실패");
                return false;
            }

            float fLaserOnDelay = (float)Equipment.Scanner_Calibration_LaserOnDelay;
            float fLaserOffDelay = (float)Equipment.Scanner_Calibration_LaserOffDelay;
            float fMarkDelay = (float)Equipment.Scanner_Calibration_MarkDelay;
            float fJumpDelay = (float)Equipment.Scanner_Calibration_JumpDelay;
            float fPolygonDelay = (float)Equipment.Scanner_Calibration_PolygonDelay;
            if (fLaserOnDelay <= 0) fLaserOnDelay = 0;
            if (fLaserOffDelay <= 0) fLaserOffDelay = 0;
            if (fMarkDelay <= 0) fMarkDelay = 0;
            if (fJumpDelay <= 0) fJumpDelay = 200;
            if (fPolygonDelay <= 0) fPolygonDelay = 0;

            if (!rtc.CtlDelay(fLaserOnDelay, fLaserOffDelay, fMarkDelay, fJumpDelay, fPolygonDelay))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Delay 설정 실패");
                return false;
            }

            rtc.ListBegin(laser, ListType.Single);
            // 중심 기준 좌표로 시작점 계산
            float startX = -((cols - 1) * pitchX) / 2.0f;
            float startY = -((rows - 1) * pitchY) / 2.0f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float centerX = startX + col * pitchX;
                    float centerY = startY + row * pitchY;

                    // DrawArc 호출: 중심 좌표와 반지름, 시작 각도, 끝 각도를 전달
                    float radius = crossSize / 2.0f; // 반지름은 crossSize의 절반으로 설정
                    float startAngle = 0.0f;         // 시작 각도 (0도)
                    float endAngle = 360.0f;         // 끝 각도 (360도, 완전한 원)

                    DrawArc(centerX, centerY, radius, startAngle, endAngle);

                    //radius만 가지고 구하기.
                    //PointD pointD = new PointD(centerX, centerY);
                    //PointD startPoint = new PointD(centerX - radius, centerY);
                    //PointD endPoint = new PointD(centerX + radius, centerY);
                    //var angles = CalculateAngles(pointD, startPoint, endPoint);
                    //// DrawArc 호출: 중심 좌표와 반지름, 시작 각도, 끝 각도를 전달
                    //DrawArc(centerX, centerY, radius, (float)angles.startAngle, (float)angles.endAngle);

                }
            }

            rtc.ListEnd();
            rtc.ListExecute();
            bRtn = true;
            return bRtn;
        }

        /// <summary>
        /// FOV기준으로 cal 진행시 사용 함수.
        /// <summary>
        public bool DrawCalibrationArc(float fovWidth, float fovHeight, int rows, int cols, out float pitchX, out float pitchY)
        {
            pitchX = 0;
            pitchY = 0;

            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Rows and columns must be greater than zero.");

            bool bRtn = false;
            float crossSize = (float)Equipment.Scanner_Calibration_CrossMarkLength;  //1.0f; // 각 원의 지름을 1mm로 설정

            // pitch 자동 계산
            pitchX = (cols > 1) ? fovWidth / (cols - 1) : 0;
            pitchY = (rows > 1) ? fovHeight / (rows - 1) : 0;

            if (Equipment.Machine_LaserType_CO2)
            {
                float fFrequency = (float)Equipment.Scanner_Calibration_LaserFrequency;
                float fPulseWidth = (float)Equipment.Scanner_Calibration_LaserPulseWidth;    //2.6f;

                if (fFrequency / 2 <= fPulseWidth)
                    fPulseWidth = fFrequency / 2;
                if (fFrequency <= 0) fFrequency = 5000;
                if (fPulseWidth <= 0) fPulseWidth = 2.6f;

                if (!rtc.CtlFrequency(fFrequency, fPulseWidth))
                {
                    Log.Write("SLD-200", "DrawCalibrationArc", "Laser Frequency 설정 실패");
                    return false;
                }
            }

            float fJumpSpeed = (float)Equipment.Scanner_Calibration_LaserJumpSpeed;
            float fMarkSpeed = (float)Equipment.Scanner_Calibration_LaserMarkSpeed;
            if (fJumpSpeed <= 0) fJumpSpeed = 0;
            if (fMarkSpeed <= 0) fMarkSpeed = 0;

            if (!rtc.CtlSpeed(fJumpSpeed, fMarkSpeed))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Speed 설정 실패");
                return false;
            }

            float fLaserOnDelay = (float)Equipment.Scanner_Calibration_LaserOnDelay;
            float fLaserOffDelay = (float)Equipment.Scanner_Calibration_LaserOffDelay;
            float fMarkDelay = (float)Equipment.Scanner_Calibration_MarkDelay;
            float fJumpDelay = (float)Equipment.Scanner_Calibration_JumpDelay;
            float fPolygonDelay = (float)Equipment.Scanner_Calibration_PolygonDelay;
            if (fLaserOnDelay <= 0) fLaserOnDelay = 0;
            if (fLaserOffDelay <= 0) fLaserOffDelay = 0;
            if (fMarkDelay <= 0) fMarkDelay = 0;
            if (fJumpDelay <= 0) fJumpDelay = 200;
            if (fPolygonDelay <= 0) fPolygonDelay = 0;

            if (!rtc.CtlDelay(fLaserOnDelay, fLaserOffDelay, fMarkDelay, fJumpDelay, fPolygonDelay))
            {
                Log.Write("SLD-200", "DrawCalibrationArc", "Laser Delay 설정 실패");
                return false;
            }

            rtc.ListBegin(laser, ListType.Single);

            float startX = -((cols - 1) * pitchX) / 2.0f;
            float startY = -((rows - 1) * pitchY) / 2.0f;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    float centerX = startX + col * pitchX;
                    float centerY = startY + row * pitchY;

                    float radius = crossSize / 2.0f;
                    DrawArc(centerX, centerY, radius, 0.0f, 360.0f);
                }
            }

            rtc.ListEnd();
            rtc.ListExecute();
            bRtn = true;
            return bRtn;
        }

        private void DrawArc(float centerX, float centerY, float radius, float startAngle, float endAngle)
        {
            if (radius <= 0)
                throw new ArgumentException("Radius must be greater than zero.");

            // 원호의 sweepAngle 계산
            float sweepAngle = endAngle - startAngle;

            // 원호 시작점 계산.
            float startX = centerX + radius * (float)Math.Cos(startAngle * Math.PI / 180.0);
            float startY = centerY + radius * (float)Math.Sin(startAngle * Math.PI / 180.0);

            // 먼저 원호의 시작점으로 점프
            if (!rtc.ListJump(startX, startY))
            {
                throw new InvalidOperationException("Failed to jump to arc start position.");
            }

            // rtc.ListArc 호출
            if (!rtc.ListArc(centerX, centerY, sweepAngle))
            {
                throw new InvalidOperationException("Failed to draw arc using rtc.ListArc.");
            }
        }

        public static (double startAngle, double endAngle) CalculateAngles(PointD center, PointD startPoint, PointD endPoint)
        {
            // 시작 각도 계산
            double startAngle = Math.Atan2(startPoint.Y - center.Y, startPoint.X - center.X) * (180.0 / Math.PI);

            // 끝 각도 계산
            double endAngle = Math.Atan2(endPoint.Y - center.Y, endPoint.X - center.X) * (180.0 / Math.PI);

            // 각도를 0~360 범위로 변환
            if (startAngle < 0) startAngle += 360;
            if (endAngle < 0) endAngle += 360;

            return (startAngle, endAngle);
        }

    }
}
