using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Core.Laser
{
    /// <summary>
    /// 파워메터 인터페이스
    /// </summary>
    public interface IPowerMeter
    {
        /// <summary>
        /// 식별 번호
        /// </summary>
        int Index { get; }

        /// <summary>
        /// 이름
        /// </summary>
        string Name { get; }

        bool IsReady { get; }
        bool IsBusy { get; }
        bool IsError { get; }

        /// <summary>
        /// 초기화
        /// </summary>
        /// <returns></returns>
        bool Initialze(int portNo);

        /// <summary>
        /// 측정시작 (측정 파라메터는 생성자에서 전달)
        /// </summary>
        /// <returns></returns>
        bool Start();

        /// <summary>
        /// 측정 최근 값
        /// </summary>
        /// <param name="watt"></param>
        /// <returns></returns>
        bool Power(out double watt);

        /// <summary>
        /// 측정 종료
        /// </summary>
        /// <returns></returns>
        bool Stop();
    }
}
