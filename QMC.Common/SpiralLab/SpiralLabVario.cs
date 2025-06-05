using System;
using System.Numerics;
using SpiralLab.Sirius;

namespace QMC.Common.Parts
{
    public class SpiralLabVario : Part
    {
        public bool IsInitialized { get; private set; } = false;

        #region Properties
        public IRtc Rtc { get; private set; }
        public IRtc3D Rtc3D { get; private set; }
        public LaserVirtual laser { set; get; }
        public string ScannerName { get; private set; }
        #endregion

        public float fSetZOffset { get; set; }
        public float fSetZDecalOffset { get; set; }

        public SpiralLabVario(string name, IRtc rtcInstance) : base(name)
        {
            this.ScannerName = name;
            this.Rtc = rtcInstance ?? throw new ArgumentNullException(nameof(rtcInstance));
            this.Rtc3D = rtcInstance as IRtc3D ?? throw new InvalidCastException("IRtc 인스턴스는 IRtc3D를 구현해야 합니다.");

            IsInitialized = true;
        }

        public override int Create()
        {
            return base.Create();
        }

        public bool MoveZAbsolute(float zMm)
        {
            return Rtc3D.CtlMove(new Vector3(0, 0, zMm));
        }

        public bool SetZOffset(float zOffset)
        {
            return Rtc3D.CtlZOffset(zOffset);
        }

        public bool SetZDefocus(float zDefocus)
        {
            return Rtc3D.CtlZDefocus(zDefocus);
        }

        public float GetCurrentZOffset() => Rtc3D.ZOffset;
        public float GetCurrentZDefocus() => Rtc3D.ZDefocus;

        //Z = A·X + B·Y + C로 보정. 보정 계수는 correction file (ct5)에서 추출하거나 수동 입력 가능
        public bool LoadZTable(float coefA, float coefB, float coefC)
        {
            return Rtc3D.CtlLoadZTable(coefA, coefB, coefC);
        }

        public bool ReadZTableFromCorrectionFile(string filePath, out double coefA, out double coefB, out double coefC)
        {
            return Rtc3D.CtlCorrectionReadABC(filePath, out coefA, out coefB, out coefC);
        }

        //매뉴얼에 명시: 동시에 사용 시 상호 덮어쓰기됨 → 한 번에 하나만 유효
        //CtlZOffset / ListZOffset
        //실제 가공 시 Z 위치 자체를 보정함.주로 표면 오차 보정이나 테이블 경사 보정용.Z Table 기반 가공에도 필수
        public void ListZOffset(float zOffset)
        {
            Rtc3D.ListZOffset(zOffset);
        }

        //CtlZDefocus / ListZDefocus
        //초점 깊이만 조정. Spot 크기나 품질 조정을 위해 사용. Z 위치는 안 바뀜
        public void ListZDefocus(float zDefocus)
        {
            Rtc3D.ListZDefocus(zDefocus);
        }

        //Z축 포함 3D 가공 지원. 보정이 적용된 Z 기준 좌표 사용
        public void ListJump3D(float x, float y, float z, float rampFactor = 1.0f)
        {
            //Z Offset 기반
            Rtc3D.ListJump3D(x, y, z, rampFactor);
        }

        //Z축 포함 3D 가공 지원. 보정이 적용된 Z 기준 좌표 사용
        public void ListMark3D(float x, float y, float z, float rampFactor = 1.0f)
        {
            //Z Offset 기반
            Rtc3D.ListMark3D(x, y, z, rampFactor);
        }

        public void ListArc3D(float cx, float cy, float cz, float sweepAngle, float rampFactor = 1.0f)
        {
            //Z Offset 기반
            Rtc3D.ListArc3D(cx, cy, cz, sweepAngle, rampFactor);
        }

        public void PrintStatus()
        {
            Console.WriteLine($"[Rtc3D] ZOffset : {Rtc3D.ZOffset:F3} mm");
            Console.WriteLine($"[Rtc3D] ZDefocus : {Rtc3D.ZDefocus:F3} mm");
            Console.WriteLine($"[Rtc3D] KZFactor : {Rtc3D.KZFactor:F3} bits/mm");
            Console.WriteLine($"[Rtc3D] Focal Length : {Rtc3D.FLength:F3} mm");
        }

        public void ResetAll()
        {
            SetZOffset(0);
            SetZDefocus(0);
            Rtc.CtlReset();
        }

        public void LaserOnDuringMs(int durationMs = 5000)
        {
            if (Rtc.ListBegin(laser, ListType.Auto) &&
                Rtc.ListLaserOn(durationMs) &&
                Rtc.ListEnd())
            {
                Rtc.ListExecute(false); // 비동기 실행
            }
        }

        // 주어진 zHeight 높이에서 정사각형 형태의 마킹을 수행합니다.
        // 정사각형의 한 변 길이는 halfSize* 2 (기본 20mm).
        // 마킹 도중 Z축은 고정되어 있으며, XY 평면 상에서만 이동합니다.
        // 시작점으로 이동(-halfSize, +halfSize, zHeight)
        // 시계방향으로 4개의 점을 마킹
        // 다시 중심(0,0,0)으로 점프(복귀)
        public void MarkSquareAtZ(float zHeight, float halfSize = 10)
        {
            if (Rtc.ListBegin(laser, ListType.Auto))
            {
                ListJump3D(-halfSize, halfSize, zHeight);
                ListMark3D(halfSize, halfSize, zHeight);
                ListJump3D(halfSize, -halfSize, zHeight);
                ListJump3D(-halfSize, -halfSize, zHeight);
                ListJump3D(-halfSize, halfSize, zHeight);
                ListJump3D(0, 0, 0);
                Rtc.ListEnd();
                Rtc.ListExecute(false);
            }
        }

        // 나선형(헬릭스) 형태의 마킹을 수행합니다.
        // revolutions 만큼 회전하며 Z축이 점진적으로 상승합니다.
        // XY는 원 궤적을 따라 회전하고, Z는 각도에 따라 선형 상승합니다.
        // 반지름 10mm, 높이 1mm, 회전 2바퀴
        public void MarkHelix(int revolutions = 2, float radius = 10, float heightPerRev = 1.0f)
        {
            bool success = Rtc.ListBegin(laser, ListType.Auto);
            for (int i = 0; i < revolutions && success; i++)
            {
                float currentZ = i * heightPerRev;
                success &= Rtc3D.ListJump3D(radius, 0, currentZ);
                for (float angle = 10; angle < 360 && success; angle += 10)
                {
                    double x = radius * Math.Sin(angle * Math.PI / 180.0);
                    double y = radius * Math.Cos(angle * Math.PI / 180.0);
                    float z = currentZ / (angle / 360.0f);
                    success &= Rtc3D.ListMark3D((float)x, (float)y, z);
                }
            }
            success &= Rtc.ListEnd();
            Rtc3D.ListJump3D(0, 0, 0);
            if (success)
                Rtc.ListExecute(false);
        }

        /// <summary>
        /// 현재 Stage 위치에서 Scanner (0,0) 기준으로 십자 마킹 수행
        /// 외부에서 Z 또는 XY 이동 후 이 함수 호출
        /// </summary>
        public void MarkCrossAtCenter(float z, float lineLength = 10)
        {
            if (!IsInitialized)
                return;

            bool success = Rtc.ListBegin(laser, ListType.Auto);
            success &= Rtc3D.ListJump3D(-lineLength, 0, z);
            success &= Rtc3D.ListMark3D(lineLength, 0, z);
            success &= Rtc3D.ListJump3D(0, -lineLength, z);
            success &= Rtc3D.ListMark3D(0, lineLength, z);
            success &= Rtc.ListEnd();
            Rtc3D.ListJump3D(0, 0, 0); // 복귀

            if (success)
                Rtc.ListExecute(false);
        }

        public void MarkCalibrationGridAtFixedZ(float z, int gridCols = 15, int gridRows = 15, float pitch = 5.0f, float crossSize = 0.5f)
        {
            if (!IsInitialized || Rtc == null || Rtc3D == null)
                return;

            if (gridCols <= 0 || gridRows <= 0)
            {
                Console.WriteLine("[MarkCalibrationGridAtFixedZ] Invalid grid size");
                return;
            }

            float halfWidth = (gridCols - 1) / 2f * pitch;
            float halfHeight = (gridRows - 1) / 2f * pitch;

            bool success = Rtc.ListBegin(laser, ListType.Auto);

            for (int col = 0; col < gridCols && success; col++)
            {
                for (int row = 0; row < gridRows && success; row++)
                {
                    float x = -halfWidth + col * pitch;
                    float y = -halfHeight + row * pitch;

                    // 십자 마킹 (가로 + 세로)
                    success &= Rtc3D.ListJump3D(x - crossSize, y, z);
                    success &= Rtc3D.ListMark3D(x + crossSize, y, z);
                    success &= Rtc3D.ListJump3D(x, y - crossSize, z);
                    success &= Rtc3D.ListMark3D(x, y + crossSize, z);
                }
            }

            success &= Rtc.ListEnd();
            Rtc3D.ListJump3D(0, 0, 0);  // 복귀

            if (success)
            {
                Rtc.ListExecute(false);
                Console.WriteLine($"[Z={z:F3} mm] Grid 마킹 완료 ({gridCols}x{gridRows})");
            }
        }

    }
}
