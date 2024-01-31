using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Core.Laser
{
    #region 열거형 타입 정의
    /// <summary>
    /// RTC5 상태에 대한 열거형 데이타
    /// </summary>
    public enum RtcStatus
    {
        /// <summary>
        /// 가공중
        /// </summary>
        Busy,
        /// <summary>
        /// 가공중이 아님
        /// </summary>
        NotBusy,
        /// <summary>
        /// 리스트 버퍼1 동작중(가공중)
        /// </summary>
        List1Busy,
        /// <summary>
        /// 리스트 버퍼2 동작중(가공중)
        /// </summary>
        List2Busy,
        /// <summary>
        /// 에러 없음
        /// </summary>
        NoError,
        /// <summary>
        /// 중지됨
        /// </summary>
        Aborted,
        /// <summary>
        /// X,Y 갈바노 메터 위치 오류 응답 상태
        /// </summary>
        PositionAckOK,
        /// <summary>
        /// 스캐너 전원 상태
        /// </summary>
        PowerOK,
        /// <summary>
        /// 스캐너 동작 가능 온도 도달 여부
        /// </summary>
        TempOK
    };

    /// <summary>
    /// 레이저 모드 열거형
    /// </summary>
    public enum LaserMode
    {
        /// <summary>
        /// Co2 모드
        /// </summary>
        Co2 = 0,
        /// <summary>
        /// YAG1 모드
        /// </summary>
        Yag1,
        /// <summary>
        /// YAG2 모드
        /// </summary>
        Yag2,
        /// <summary>
        /// YAG3 모드
        /// </summary>
        Yag3,
        /// <summary>
        /// Mode 4 모드
        /// </summary>
        Mode4,
        /// <summary>
        /// YAG5 모드
        /// </summary>
        Yag5,
        /// <summary>
        /// Mode 6 모드
        /// </summary>
        Mode6
    };
    /// <summary>
    /// Laser 1,2 및 LaserOn 핀의 시그널 레벨 설정용
    /// </summary>
    public enum SignalLevel
    {
        /// <summary>
        /// Active Low Signal Level
        /// </summary>
        ActiveLow,
        /// <summary>
        /// Active High Signal Level
        /// </summary>
        ActiveHigh
    };

    /// </summary>
    public enum ExtensionChannel 
    { 
        None,
        ExtDI8O8, 
        ExtDO16, 
        ExtDI2O2, 
        ExtAO1, 
        ExtAO2 
    };

    public enum AutomaticLaserControl
    {
        None = 0,
        /// <summary>
        /// 스캐너의 FOV 내에서 반지름 위치(Raduis) 에 따른 보상
        /// </summary>
        PositionDependent, 
        /// <summary>
        /// 스캐너 속도에 따른 보상
        /// </summary>
        SpeedDependent,
        /// <summary>
        /// 시작과 끝 구간 (예를 들어 직선 구간) 에 대한 선형 보상
        /// </summary>
        VectorDefined,
        /// <summary>
        /// 외부 엔코더 입력에 따른 보상? (MOTF)
        /// </summary>
        EncoderSpeedDependent
    };
    #endregion

    /// <summary>
    /// 스캔랩 RTC 인터페이스
    /// </summary>
    public interface IRtc : IDisposable
    {
        #region 공개 속성
        /// <summary>
        /// RTC 카드의 식별자 (0,1,2...)
        /// </summary>
        uint Index { get; }

        /// <summary>
        /// 이름
        /// </summary>
        string Name { get; }

        /// <summary>
        /// KFactor = bits/mm
        /// </summary>
        double KFactor { get; }
        /// <summary>
        /// 스캐너의 논리적인 크기 (mm)
        /// </summary>
        double Fov { get; }
        /// <summary>
        /// 보정 파일 이름 (correction 폴더에서의 상대적 경로)
        /// </summary>
        string CorrectionFile { get; } 
        /// <summary>
        /// 3x3 행렬 스택
        /// Push/Pop 을 통해 변환행렬을 누적 
        /// 최종 변환 행렬 = M(오래된 push 행렬) * ... * M (최신 push 행렬)
        /// </summary>
        MatrixStack MatrixStack { get; }
        #endregion

        bool Initialize(double kFactor, 
            string correctionFileName, 
            LaserMode laserMode = LaserMode.Yag1, SignalLevel signalLevel = SignalLevel.ActiveHigh);

        #region 컨트롤 명령
        /// <summary>
        /// 스캐너 보정 파일 로드(변경)
        /// </summary>
        /// <param name="correctionFileName">보정 파일 이름 (correction 폴더가 루트 디렉토리)</param>
        /// <returns></returns>
        bool CtlLoadCorrectionFile(string correctionFileName);
        /// <summary>
        /// 수동 레이저 출사 시작
        /// (호출전 주파수, 펄스폭 등 지정 필요)
        /// </summary>
        /// <returns></returns>
        bool CtlLaserOn();
        /// <summary>
        /// 수동 레이저 출사 정지
        /// </summary>
        /// <returns></returns>
        bool CtlLaserOff();
        /// <summary>
        /// 스캐너 위치 이동
        /// </summary>
        /// <param name="x">mm</param>
        /// <param name="y">mm</param>
        /// <returns></returns>
        bool CtlMove(double x, double y);
        /// <summary>
        /// 레이저 타이밍(주파수, 펄스폭) 설정
        /// </summary>
        /// <param name="frequency">Hz</param>
        /// <param name="pulseWidth">usec</param>
        /// <returns></returns>
        bool CtlFrequency(double frequency, double pulseWidth);
        /// <summary>
        /// 레이저및 스캐너의 지연시간 설정
        /// </summary>
        /// <param name="laserOn">usec</param>
        /// <param name="laserOff">usec</param>
        /// <param name="scannerJump">usec</param>
        /// <param name="scannerMark">usec</param>
        /// <param name="scannerPolygon">usec</param>
        /// <returns></returns>
        bool CtlDelay(double laserOn, double laserOff, double scannerJump, double scannerMark, double scannerPolygon);
        /// <summary>
        /// 스캐너 속도 설정
        /// </summary>
        /// <param name="jump">mm/s</param>
        /// <param name="mark">mm/s</param>
        /// <returns></returns>
        bool CtlSpeed(double jump, double mark);
        /// <summary>
        /// 레이저 소스측의 파워 출력을 변경
        /// </summary>
        /// <param name="powerXFactor">파워 제어에 사용하는 RTC 출력 인터페이스</param>
        /// <param name="powerXValue">출력 값</param>
        /// <returns></returns>
        bool CtlLaserControl(PowerXFactor powerXFactor, double powerXValue);
        /// <summary>
        /// RTC 확장채널에 데이타 쓰기
        /// </summary>
        /// <typeparam name="T">데이타 타입(ExtDO16, ExtDI8O8, ExtDI2O2 : uint, ExtAO1, ExtAO2 : double)</typeparam>
        /// <param name="ch">확장 채널</param>
        /// <param name="value">값</param>
        /// <returns></returns>
        bool CtlWriteData<T>(ExtensionChannel ch, T value);
        /// <summary>
        /// RTC 카드 상태 조회
        /// </summary>
        /// <param name="status">Rtc 상태 열거형 타입</param>
        /// <returns></returns>
        bool CtlGetStatus(RtcStatus status);
        /// <summary>
        /// 실행중인 리스트 명령을 중단
        /// </summary>
        /// <returns></returns>
        bool CtlAbort();
        /// <summary>
        /// 중단된 상태를 해제
        /// </summary>
        /// <returns></returns>
        bool CtlReset();
        #endregion

        #region 리스트 명령
        /// <summary>
        /// 리스트 명령 버퍼 초기화
        /// 이후의 모든 리스트 명령들은 버퍼에 기록됨고 동시에 현재 리스트 버퍼에의 개수가 4000 명령이 초과되면, 리스트 명령이 자동으로 시작된다.
        /// 리스트 명령이 시작되었다는것은 레이저 가공이 시작됨을 의미하며, 리스트 명령이 출력중이더라도 다음번 4000 명령을 두번째 버퍼에 넣어놓을수있다.
        /// 만약 첫번째 리스트 버퍼가 실행중이고, 두번째 리스트 버퍼가 가득찼다면, 이후 리스트 명령들은 Block 처리되며, 
        /// 이를 해결하기 위해서는 리스트 명령을 삽입하는 측에서는 독립적인 가공 데이타 삽입용 쓰레드 등의 비동기 처리가 필요하다.
        /// </summary>
        /// <returns></returns>
        bool ListBegin();
        /// <summary>
        /// 리스트 버퍼에 레이저 주파수, 펄스폭 명령
        /// </summary>
        /// <param name="frequency">Hz</param>
        /// <param name="pulseWidth">usec</param>
        /// <returns></returns>
        bool ListFrequency(double frequency, double pulseWidth);
        /// <summary>
        /// 리스트 버퍼에 레이저, 스캐너 지연값 명령
        /// </summary>
        /// <param name="laserOn">usec</param>
        /// <param name="laserOff">usec</param>
        /// <param name="scannerJump">usec</param>
        /// <param name="scannerMark">usec</param>
        /// <param name="scannerPolygon">usec</param>
        /// <returns></returns>
        bool ListDelay(double laserOn, double laserOff, double scannerJump, double scannerMark, double scannerPolygon);
        /// <summary>
        /// 리스트 버퍼에 스캐너 속도 명령
        /// </summary>
        /// <param name="jump">mm/s</param>
        /// <param name="mark">mm/s</param>
        /// <returns></returns>
        bool ListSpeed(double jump, double mark);
        /// <summary>
        /// 리스트 버퍼에 스캐너 지정위치로 점프
        /// </summary>
        /// <param name="x">mm</param>
        /// <param name="y">mm</param>
        /// <returns></returns>
        bool ListJumpTo(double x, double y, double weight=1.0);
        /// <summary>
        /// 리스트 버퍼에 스캐너 지정위치로 마크
        /// </summary>
        /// <param name="x">mm</param>
        /// <param name="y">mm</param>
        /// <returns></returns>
        bool ListMarkTo(double x, double y, double weight=1.0);
        /// <summary>
        /// 리스트 버퍼에 스캐너 지정 위치를 중심으로 지정된 회전각도만큼 회전
        /// (호의 시작위치는 마지막 위치가 기준임)
        /// </summary>
        /// <param name="cx">rotate center x (mm)</param>
        /// <param name="cy">rotate center y (mm)</param>
        /// <param name="sweepAngle">degree ( CCW:+, CW:-) </param>
        /// <returns></returns>
        bool ListArc(double cx, double cy, double sweepAngle, double weight=1.0);
        /// <summary>
        /// Pixel Raster Operation
        /// 몇개의 픽셀을 어떤 주기로 어떤 방향으로 가공할지를 미리 준비
        /// (시작점은 현재 스캐너 위치 : ListJump 를 호출한후에 사용 한다던지)
        /// </summary>
        /// <param name="usec">매 픽셀당 주기 (usec)</param>
        /// <param name="ext">매 픽셀당 출력강도를 제어할 채널 (None, ExtAO1, ExtAO2 중 선택) </param>
        /// <param name="dx">매 픽셀당 가로 방향 이동거리 (mm)</param>
        /// <param name="dy">매 픽셀당 세로 방향 이동거리 (mm)</param>
        /// <param name="pixelCount">pixel counts per line</param>
        /// <returns></returns>
        bool ListPixelLine(double usec, ExtensionChannel ext, double dx, double dy, uint pixelCount);
        /// <summary>
        /// Pixel Raster Operation 
        /// 하나의 픽셀에 대한 출력 리스트 명령
        /// </summary>
        /// <param name="usec">한 픽셀에 대한 펄스 출력 시간(ListPixelLine 에서 지정한 주기보다는 당연히 작아야 한다) </param>
        /// <param name="voltage">출력 채널중 아날로그를 사용할 경우 Voltage 값 (0~10V) </param>
        /// <returns></returns>
        bool ListPixel(double usec, double voltage = 0.0f);
        /// <summary>
        /// 리스트 명령으로 확장 채널에 데이타 쓰끼
        /// </summary>
        /// <typeparam name="T">데이타 타입</typeparam>
        /// <param name="ch">확장 채널</param>
        /// <param name="value">데이타 값</param>
        /// <returns></returns>
        bool ListWriteData<T>(ExtensionChannel ch, T value);
        
        /// <summary>
        /// 리스트 명령으로 레이저 파워 제어용 명령쓰기
        /// </summary>
        /// <param name="powerXFactor">파워 변경 방법</param>
        /// <param name="powerXValue">파워 값</param>
        /// <returns></returns>
        bool ListLaserControl(PowerXFactor powerXFactor, double powerXValue);
        
        /// <summary>
        /// 리스트 버퍼 명령 기록 완료
        /// </summary>
        /// <returns></returns>
        bool ListEnd();
        /// <summary>
        /// 리스트 버퍼에 남아 있는 모든 명령 실행
        /// </summary>
        /// <param name="busyWait">완료될때까지 대기 여부 (대기시 블럭 처리됨)</param>
        /// <returns></returns>
        bool ListExecute(bool busyWait = true);

        /// <summary>
        /// 사각형 그리기.
        /// </summary>
        /// <param name="lengthX"> X 길이 mm </param>
        /// <param name="lengthY"> Y 길이 mm </param>
        /// <returns></returns>
        bool DrawRec(double lengthX, double lengthY);

        /// <summary>
        /// Grid 그리기
        /// </summary>
        /// <param name="cntx"> row 수 </param>
        /// <param name="cnty"> col 수 </param>
        /// <returns></returns>
        bool DrawCalGrid(int cntx, int cnty);

        /// <summary>
        /// X 그리기
        /// </summary>
        /// <param name="coordx"> x pos </param>
        /// <param name="coordy"> y pos </param>
        /// <param name="size"> mark size </param>
        /// <returns></returns>
        bool DrawMarkX(double coordx, double coordy, double size);

        bool DrawTestLine();

        #endregion
    }
}
