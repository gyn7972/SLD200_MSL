using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace QMC.Core.Laser
namespace QMC.Common.Laser
{
    /// <summary>
    /// 레이저 파워 제어 요소
    /// </summary>
    public enum PowerXFactor
    {
        /// <summary>
        /// 사용자가 직접 제어 (외부 통신 등)
        /// </summary>
        Custom = 0,

        /// <summary>
        /// RTC 확장 아나로그1 포트
        /// </summary>
        Analog1 = 1, //0~10V
        /// <summary>
        /// RTC 확장 아나로그2 포트
        /// </summary>
        Analog2 = 2, //0~10V
        /// <summary>
        /// RTC 확장 8비트 디지털 출력 포트
        /// </summary>
        ExtDO8Bit = 3, //0~255
        /// <summary>
        /// RTC 15핀의 LASER1,2 신호 출력 (펄스폭 변조)
        /// </summary>
        PulseWidth = 4, //usec
        /// <summary>
        /// RTC15핀의 LASER 1,2, 신호 출력 (주파수 변조)
        /// Half Peroid
        /// </summary>
        Frequency = 5, //Hz
        /// <summary>
        /// RTC 확장 16비트 디지털 출력 포트
        /// </summary>
        ExtDO16 = 6, //0~65535
        /// <summary>
        /// 미구현
        /// </summary>
        FocusShift, //+- mm (vario-scan stroke length)
    };

    /// <summary>
    /// laser source interface
    /// </summary>
    //public interface ILaser : IDisposable
    public interface DPSSLaser : IDisposable                    //  Sirius Lib 의 ILaser 와 이름이 동일하여 DPSSLaser 로 변경
    {
        /// <summary>
        /// 식별자
        /// </summary>
        int Index { get; }
        /// <summary>
        /// 이름
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 최대 출력 에너지 (Watt)
        /// </summary>
        float MaxPowerWatt { get; }
        /// <summary>
        /// 레이저 출력을 제어하는 인터페이스 방법
        /// </summary>
        PowerXFactor PowerXFactor { get; }
        /// <summary>
        /// 준비 상태
        /// </summary>
        bool IsReady { get; }
        /// <summary>
        /// 출사중 여부
        /// </summary>
        bool IsBusy { get; }
        /// <summary>
        /// 알람 발생 여부
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// 사용자 정의 데이타
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// 통신 초기화
        /// </summary>
        /// <returns></returns>
        int Initialize();

        /// <summary>
        /// 리셋
        /// </summary>
        /// <returns></returns>
        bool CtlReset();

        /// <summary>
        /// 파워 변경 (RTC 컨트롤 명령 : 즉시)
        /// </summary>
        /// <param name="watt"></param>
        /// <returns></returns>
        bool CtlPower(double watt);

        /// <summary>
        /// 파워 변경 (RTC 리스트 명령 : 리스트에 삽입후 실행) 
        /// </summary>
        /// <param name="watt"></param>
        /// <returns></returns>
        //bool ListPower(double watt); 

        //bool CtlShutterOpen();

        //bool CtlShutterClose();


        //    bool CtlLaserOn(); // laser armed
        //    bool CtlLaserOff(); // laser armed

        //    bool CtlLaserShot(); // laser fire
        //    bool CtlLaserStop(); // laser fire stop

        //}
        bool IsConnected { get; }
        /// <summary>
        /// 셔터 Open/Close 여부
        /// </summary>
        bool IsShutterOpend { get; }
        /// <summary>
        /// GateSignal On/Off 여부
        /// </summary>
        bool IsGateSignalOn { get; }
        /// <summary>
        /// Armed 상태
        /// </summary>
        bool IsArmed { get; }
        /// <summary>
        /// BurstMode On/Off 여부
        /// </summary>
        bool IsBurstModeOn { get; }

        int Connect();

        int SetPower(int power);

        int GetPower(ref int power);

        int SetRepRate(int repRate);

        int GetRepRate(ref int repRate);

        int SetArm();

        int ResetArm();

        int ShutDown();

        int GetStatus(ref int state);
    }
}
