using System;
using System.IO;
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

        public void Dispose()
        {
            //Dispose(true);
            // 관리되는 리소스 해제
            if (Rtc != null && Rtc is IDisposable dRtc)
                dRtc.Dispose();

            laser = null;
            Rtc = null;
            Rtc3D = null;

            // 비관리 리소스 해제 (필요시 여기에 작성)
            IsInitialized = false;


            GC.SuppressFinalize(this); // Finalizer 호출 방지
        }

        /// <summary>
        /// 3D 초기화: 보정파일 적용, 기본 파라미터 세팅, 레이저 연결
        /// </summary>
        public bool Initialize3D(
            float fovMm,
            LaserMode laserMode,
            string correctionFilePath,
            int freqHz = 50_000,
            int pulseWidthUs = 2,
            float jumpSpeed = 500f,
            float markSpeed = 500f,
            int delayLaserOn = 10, int delayLaserOff = 100, int delayJump = 200, int delayMark = 200, int delayPolygon = 0)
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Rtc 인스턴스가 준비되지 않았습니다.");

            if (!File.Exists(correctionFilePath))
                throw new FileNotFoundException("3D 보정 파일이 없습니다.", correctionFilePath);

            // k = 2^20 / FOV (RTC5/6)
            float kfactor = (float)Math.Pow(2, 20) / (float)Math.Max(1e-6, fovMm);

            bool ok = true;
            ok &= Rtc.Initialize(kfactor, laserMode, correctionFilePath);
            ok &= Rtc.CtlFrequency(freqHz, pulseWidthUs);
            ok &= Rtc.CtlSpeed(jumpSpeed, markSpeed);
            ok &= Rtc.CtlDelay(delayLaserOn, delayLaserOff, delayJump, delayMark, delayPolygon);

            if (laser == null)
                laser = new LaserVirtual(0, "virtual", 20);

            laser.Rtc = Rtc;
            ok &= laser.Initialize();
            ok &= laser.CtlPower(2.0f);

            // 3D 안전 초기화
            Rtc3D = Rtc as IRtc3D ?? throw new InvalidCastException("IRtc3D 캐스팅 실패(3D 미지원/비활성).");
            Rtc3D.CtlZOffset(0);
            Rtc3D.CtlZDefocus(0);

            return ok;
        }

        /// <summary>
        /// (옵션) 3D 보정파일을 지정 슬롯에 로드/선택 (기종별 권장 슬롯 사용)
        /// </summary>
        public bool LoadAndSelect3DCorrection(string ctPath)
        {
            return true;
            //if (!File.Exists(ctPath))
            //    throw new FileNotFoundException("보정 파일 없음", ctPath);

            //CorrectionTableIndex target =
            //    //Rtc.RtcType switch
            //    //{
            //    //    RtcType.Rt c4 => CorrectionTableIndex.Table2,
            //    //    RtcType.Rtc5 => CorrectionTableIndex.Table4,
            //    //    RtcType.Rtc6 => CorrectionTableIndex.Table8,
            //    //    _ => CorrectionTableIndex.Table1,
            //    //};

            //bool ok = true;
            //ok &= Rtc.CtlLoadCorrectionFile(target, ctPath);
            //ok &= Rtc.CtlSelectCorrection(target, target);
            //return ok;
        }


        public bool MoveZAbsolute(float zMm)
        {
            // (주의) XY=0,0 으로 튈 수 있는 절대 이동. 공정 중에는 CtlZOffset/Defocus 사용 권장
            return Rtc3D.CtlMove(new Vector3(0, 0, zMm));
        }

        //실제 가공면 높이 보정 개념
        //표면 높이, 평탄도, plane 차이 보정
        public bool SetZOffset(float zOffset, bool bScale = true)
        {
            float mm = zOffset;
            float value = (mm * 10) * -1; 
            if(bScale == false)
            {
                value = zOffset * -1;
            }

            return Rtc3D.CtlZOffset(value);
        }

        // 초점 깊이 보정 개념. Z Offset과 달리 실제 Z 위치는 안 바뀌고,
        // 초점 품질/크기 조정용. 표면 상태에 따른 품질 보정이나 Z Table 보정에도 활용 가능
        public bool SetZDefocus(float zOffset, bool bScale = true)
        {
            float mm = zOffset;
            float value = (mm * 10) * -1;
            if (bScale == false)
            {
                value = zOffset * -1;
            }

            return Rtc3D.CtlZDefocus(value);
        }

        public float GetCurrentZOffset(bool bScale = true)
        {
            float mm = Rtc3D.ZOffset;
            float value = (mm / 10) * -1;
            if (bScale == false)
            {
                value = Rtc3D.ZOffset * -1;
            }

            return value;
        }
        public float GetCurrentZDefocus(bool bScale = true)
        {
            float mm = Rtc3D.ZDefocus;
            float value = (mm / 10) * -1;
            if (bScale == false)
            {
                value = Rtc3D.ZDefocus * -1;
            }

            return value;
        }

        //Z = A·X + B·Y + C로 보정. 보정 계수는 correction file (ct5)에서 추출하거나 수동 입력 가능
        public bool LoadZTable(float coefA, float coefB, float coefC)
        {
            return Rtc3D.CtlLoadZTable(coefA, coefB, coefC);
        }

        public bool ReadZTableFromCorrectionFile(string filePath, out double coefA, out double coefB, out double coefC)
        {
            return Rtc3D.CtlCorrectionReadABC(filePath, out coefA, out coefB, out coefC);
        }

        public bool WriteZTableToCorrectionFile(string filePath, double coefA, double coefB, double coefC)
        {
            return Rtc3D.CtlCorrectionWriteABC(filePath, coefA, coefB, coefC);
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
        // 반지름 1mm, 높이 0.05mm, 회전 2바퀴
        public void MarkHelix(int revolutions = 2, float radius = 1, float heightPerRev = 0.05f)
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

                    //float z = currentZ / (angle / 360.0f);

                    // Z는 각도에 따라 선형 증가 <- GPT의 제안
                    float t = angle / 360.0f;              // 0..1
                    float z = currentZ + heightPerRev * t; // 다음 회전 끝까지 선형 증가
                    success &= Rtc3D.ListMark3D((float)x, (float)y, z);
                }
            }

            Rtc3D.ListJump3D(0, 0, 0);

            success &= Rtc.ListEnd();
            
            if (success)
                Rtc.ListExecute(false);
        }

        // 헬리컬 드릴링: 회전당 pitch만큼 Z 변화(+재질 구간별 동일 Z 추가회전)
        // 위에꺼랑 비교해서 Test해보자.
        //public void MarkHelix(int revolutions = 2, float radius = 10, float heightPerRev = 1.0f)
        //{
        //    // thickness = revolutions * heightPerRev, pitchPerTurn = +heightPerRev (상승) 또는 -heightPerRev(하강)
        //    float thickness = revolutions * Math.Abs(heightPerRev);
        //    float pitchPerTurn = heightPerRev; // 하강하려면 음수로 넘겨주세요.
        //    bool success = Rtc.ListBegin(laser, ListType.Auto);
        //    if (success)
        //        success &= HelicalDrill(0, 0, radius, thickness, pitchPerTurn, 5f /*angle step*/);
        //    success &= Rtc.ListEnd();
        //    if (success) Rtc.ListExecute(false);
        //}

        /// <summary>
        /// 현재 Stage 위치에서 Scanner (0,0) 기준으로 십자 마킹 수행
        /// 외부에서 Z 또는 XY 이동 후 이 함수 호출
        /// </summary>
        public void MarkCrossAtCenter(float z, float lineLength = 1)
        {
            if (!IsInitialized)
                return;

            bool success = Rtc.ListBegin(laser, ListType.Auto);
            success &= Rtc3D.ListJump3D(-lineLength, 0, z);
            success &= Rtc3D.ListMark3D(lineLength, 0, z);
            success &= Rtc3D.ListJump3D(0, -lineLength, z);
            success &= Rtc3D.ListMark3D(0, lineLength, z);

            Rtc3D.ListJump3D(0, 0, 0); // 복귀

            success &= Rtc.ListEnd();
            
            if (success)
                Rtc.ListExecute(false);
        }

        public void MarkCalibrationGridAtFixedZ(float z, int gridCols = 15, int gridRows = 15, float pitch = 2.0f, float crossSize = 0.5f)
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

            // 리스트 안에서 복귀
            success &= Rtc3D.ListJump3D(0, 0, 0);

            success &= Rtc.ListEnd();

            if (success)
            {
                Rtc.ListExecute(false);
                Console.WriteLine($"[Z={z:F3} mm] Grid 마킹 완료 ({gridCols}x{gridRows})");
            }
        }

        // 고정 Z에서 원 1바퀴 (ListArc3D 미제공 대비)
        public bool MarkCircleAtZ(float cx, float cy, float r, float z, float angleStepDeg = 5f, float startAngleDeg = 0f)
        {
            bool ok = true;
            double r0 = startAngleDeg * Math.PI / 180.0;
            ok &= Rtc3D.ListJump3D(cx + r * (float)Math.Cos(r0), cy + r * (float)Math.Sin(r0), z);
            for (float a = startAngleDeg + angleStepDeg; a <= startAngleDeg + 360f && ok; a += angleStepDeg)
            {
                double rad = a * Math.PI / 180.0;
                ok &= Rtc3D.ListMark3D(cx + r * (float)Math.Cos(rad), cy + r * (float)Math.Sin(rad), z);
            }
            return ok;
        }

        // 헬리컬 드릴링: 회전당 pitch만큼 Z 변화(+재질 구간별 동일 Z 추가회전)
        public bool HelicalDrill(float cx, float cy, float radius,
                                 float thickness, float pitchPerTurn,  // 하강은 음수 예:-0.05f
                                 float angleStepDeg = 5f,
                                 Func<float, int> passesAtDepth = null, // zTop -> 동일 Z에서 총 회전수(기본 1)
                                 int finishTurnsAtBottom = 0,
                                 float startAngleDeg = 0f)
        {
            bool ok = true;
            int turns = (int)Math.Ceiling(Math.Abs(thickness / pitchPerTurn));
            float sign = Math.Sign(pitchPerTurn);
            float z0 = 0f;

            // 시작점 점프
            double r0 = startAngleDeg * Math.PI / 180.0;
            ok &= Rtc3D.ListJump3D(cx + radius * (float)Math.Cos(r0), cy + radius * (float)Math.Sin(r0), z0);
            if (!ok) return false;

            for (int t = 0; t < turns && ok; t++)
            {
                float zStart = z0 + sign * (t * Math.Abs(pitchPerTurn));
                float zEnd = zStart + sign * Math.Abs(pitchPerTurn);

                // 한 바퀴: Z 선형 보간
                for (float a = startAngleDeg + angleStepDeg; a <= startAngleDeg + 360f && ok; a += angleStepDeg)
                {
                    float lerp = (a - startAngleDeg) / 360f;       // 0..1
                    float z = zStart + (zEnd - zStart) * lerp;
                    double rad = a * Math.PI / 180.0;
                    ok &= Rtc3D.ListMark3D(cx + radius * (float)Math.Cos(rad),
                                           cy + radius * (float)Math.Sin(rad), z);
                }
                if (!ok) break;

                // 동일 Z에서 추가 회전(재질 경화 구간)
                int extraTurnsAtSameZ = Math.Max(0, (passesAtDepth?.Invoke(zStart) ?? 1) - 1);
                for (int k = 0; k < extraTurnsAtSameZ && ok; k++)
                    ok &= MarkCircleAtZ(cx, cy, radius, zEnd, angleStepDeg, startAngleDeg);
            }
            if (!ok) return false;

            // 바닥 마무리
            float zBottom = z0 + sign * (turns * Math.Abs(pitchPerTurn));
            for (int i = 0; i < finishTurnsAtBottom && ok; i++)
                ok &= MarkCircleAtZ(cx, cy, radius, zBottom, angleStepDeg, startAngleDeg);

            // 선택: 원점 포커스 복귀
            ok &= Rtc3D.ListJump3D(0, 0, 0);
            return ok;
        }

        // ---------------------------------------------------------------------
        // ListArc3D 유틸: 센터/스윕 기반 순수 원호 가공
        //  - 양(+) 스윕: 반시계(CCW), 음(-) 스윕: 시계(CW)
        //  - startAngleDeg 기준 시작점으로 Jump 후, 스윕 각도만큼 원호 마크
        //  - 리스트 라이프사이클은 관리하지 않음(호출 전 Rtc.ListBegin 필요)
        // ---------------------------------------------------------------------

        /// <summary>
        /// 센터/스윕 기반 원호 (단일 스윕).
        /// cx,cy,cz: 원의 중심(mm), radius: 반지름(mm),
        /// startAngleDeg: 시작각(도), sweepAngleDeg: 스윕각(도, +CCW / -CW)
        /// </summary>
        ///리스트 안에서 원호 한 번:
        //사용 예시:
        //Rtc.ListBegin(laser, ListType.Auto);
        //ListZOffset(0); // 필요시
        //MarkArc3D_CenterSweep(cx:0, cy:0, cz:0, radius:0.5f, startAngleDeg:45f, sweepAngleDeg:180f); // 45°→225° CCW
        //Rtc3D.ListJump3D(0,0,0); // 복귀(선택)
        //Rtc.ListEnd();
        //Rtc.ListExecute(false);
        public bool MarkArc3D_CenterSweep(
            float cx, float cy, float cz,
            float radius,
            float startAngleDeg,
            float sweepAngleDeg,
            float rampFactor = 1.0f)
        {
            bool ok = true;

            // 시작점 계산
            double sr = startAngleDeg * Math.PI / 180.0;
            float sx = cx + radius * (float)Math.Cos(sr);
            float sy = cy + radius * (float)Math.Sin(sr);

            // 시작점 점프 → 스윕
            ok &= Rtc3D.ListJump3D(sx, sy, cz);
            ok &= Rtc3D.ListArc3D(cx, cy, cz, sweepAngleDeg, rampFactor);

            return ok;
        }

        /// <summary>
        /// 완전한 원(360도) 1회 마킹. (+360 → CCW 한바퀴, -360 → CW 한바퀴)
        /// </summary>
        /// 완전 원(한 바퀴)
        ///  //사용 예시:
        //Rtc.ListBegin(laser, ListType.Auto);
        //MarkCircle3D_Center(0,0,0, radius:0.1f, startAngleDeg:0f, ccw:true);
        //Rtc3D.ListJump3D(0,0,0);
        //Rtc.ListEnd();
        //Rtc.ListExecute(false);
        public bool MarkCircle3D_Center(
            float cx, float cy, float cz,
            float radius,
            float startAngleDeg = 0f,
            bool ccw = true,
            float rampFactor = 1.0f)
        {
            float sweep = ccw ? 360f : -360f;
            return MarkArc3D_CenterSweep(cx, cy, cz, radius, startAngleDeg, sweep, rampFactor);
        }

        /// <summary>
        /// 스윕이 360도를 넘거나 작은 세그먼트로 쪼개고 싶을 때 사용.
        /// 예: 총 720도(두 바퀴)를 120도 단위로 6세그먼트 가공 등
        /// </summary>
        /// 두 바퀴(720°)를 120° 세그먼트로 쪼개서:
        ///// Rtc.ListBegin(laser, ListType.Auto);
        //MarkArc3D_CenterSweepSegmented(0,0,0, radius:0.1f, startAngleDeg:0f, totalSweepDeg:720f, segmentDeg:120f);
        //Rtc3D.ListJump3D(0,0,0);
        //Rtc.ListEnd();
        //Rtc.ListExecute(false);
        public bool MarkArc3D_CenterSweepSegmented(
            float cx, float cy, float cz,
            float radius,
            float startAngleDeg,
            float totalSweepDeg,
            float segmentDeg = 120f,         // 세그먼트 각도(절대값). 90~180 권장
            float rampFactor = 1.0f)
        {
            if (segmentDeg <= 0f) segmentDeg = 120f;

            bool ok = true;

            // 방향 보존
            float dir = Math.Sign(totalSweepDeg) == 0 ? 1f : Math.Sign(totalSweepDeg);
            float remain = Math.Abs(totalSweepDeg);

            // 시작점 점프
            double sr = startAngleDeg * Math.PI / 180.0;
            float sx = cx + radius * (float)Math.Cos(sr);
            float sy = cy + radius * (float)Math.Sin(sr);
            ok &= Rtc3D.ListJump3D(sx, sy, cz);
            if (!ok) return false;

            // 세그먼트 반복
            float currentAngle = startAngleDeg;
            while (remain > 1e-6f && ok)
            {
                float step = Math.Min(remain, segmentDeg) * dir;
                ok &= Rtc3D.ListArc3D(cx, cy, cz, step, rampFactor);

                currentAngle += step;          // 실제론 하드웨어가 누적각 관리
                remain -= Math.Abs(step);
            }
            return ok;
        }

        /// <summary>
        /// 동일 원호를 n회 반복(동일 센터/반경/시작각).
        /// 예: 같은 위치에서 물성 때문에 2~3회 더 태우는 경우.
        /// </summary>
        /// 같은 깊이에서 3회 반복(재질 같은 위치 중복 터치):
        ///// Rtc.ListBegin(laser, ListType.Auto);
        //MarkArc3D_CenterSweepRepeat(0,0,0, radius:0.1f, startAngleDeg:0f, sweepAngleDeg:360f, repeatCount:3);
        //Rtc3D.ListJump3D(0,0,0);
        //Rtc.ListEnd();
        //Rtc.ListExecute(false);
        public bool MarkArc3D_CenterSweepRepeat(
            float cx, float cy, float cz,
            float radius,
            float startAngleDeg,
            float sweepAngleDeg,
            int repeatCount,
            float rampFactor = 1.0f)
        {
            if (repeatCount < 1) repeatCount = 1;

            bool ok = true;

            // 시작점 한 번만 점프
            double sr = startAngleDeg * Math.PI / 180.0;
            float sx = cx + radius * (float)Math.Cos(sr);
            float sy = cy + radius * (float)Math.Sin(sr);
            ok &= Rtc3D.ListJump3D(sx, sy, cz);

            // 동일 스윕 반복
            for (int i = 0; i < repeatCount && ok; i++)
                ok &= Rtc3D.ListArc3D(cx, cy, cz, sweepAngleDeg, rampFactor);

            return ok;
        }



    }
}
