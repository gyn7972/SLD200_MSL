using System.Collections.Generic;

using Category = System.String;
using SetX = System.Double;
using MeasureWatt = System.Double;


namespace QMC.Core.Laser
{
    /// <summary>
    /// 파워맵 인터페이스
    /// </summary>
    public interface IPowerMap
    {
        /// <summary>
        /// 식별자
        /// </summary>
        uint Index { get; }

        /// <summary>
        /// 파워 맵 파일 이름
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// X : 레이저 파워 조절 인자(펄스폭(um) / 전압(V) / 펄스폭(%) 등 )
        /// </summary>
        string XName { get; }

        /// <summary>
        /// 파라메터 X값의 간격
        /// </summary>
        double XGap { get; }

        /// <summary>
        /// Category(예:주파수), (X값, 측정값)
        /// </summary>
        Dictionary<Category, Dictionary<SetX, MeasureWatt> > Data { get; }

        /// <summary>
        /// 사용자 정의 데이타
        /// </summary>
        object Tag { get; set; }

        /// <summary>
        /// 시리즈 정보및 룩업테이블 모두 삭제
        /// </summary>
        void Clear();

        /// <summary>
        /// 룩업 테이블 업데이트 
        /// 설정값 : 시리즈 이름과 의존값(x)
        /// 측정값 : 에너지 (Watt)
        /// </summary>
        /// <param name="category">Series Name</param>
        /// <param name="x">레이저 파워 조절 인자(펄스폭(um) / 전압(V) / 펄스폭(%) 등 )</param>
        /// <param name="detectedWatt">측정된 실제 에너지(W)</param>
        /// <returns></returns>
        bool Update(Category category, double x, double detectedWatt);

        /// <summary>
        /// 룩업 테이블 실제값 조회
        /// </summary>
        /// <param name="category">Series Name</param>
        /// <param name="x">레이저 파워 조절 인자(펄스폭(um) / 전압(V) / 펄스폭(%) 등 )</param>
        /// <param name="watt">측정된 실제 에너지(W)</param>
        /// <returns></returns>
        bool Query(Category category, double x, out double watt);

        /// <summary>
        /// 룩업 테이블내의 지정된 시리즈에서의 실제 최대 에너지(W)값
        /// </summary>
        /// <param name="category">Series Name</param>
        /// <param name="detectedWatt">측정된 실제 에너지(W)</param>
        /// <returns></returns>
        bool MaxDetectedPower(Category category, out double detectedWatt);

        /// <summary>
        /// 룩업 테이블내의 지정된 시리즈에서의 X 의 최대, 최소값
        /// </summary>
        /// <param name="category">Series Name</param>
        /// <param name="minX">최소값(레이저 파워 조절 인자)</param>
        /// <param name="maxX">최대값(레이저 파워 조절 인자)</param>
        /// <returns></returns>
        bool MinMaxX(Category category, out double minX, out double maxX);

        /// <summary>
        /// 룩업 테이블을 이용해 지정된 시리즈에서 지정된 에너지(watt)가 출력되기 위한 x 값 추출 (선형방식)
        /// </summary>
        /// <param name="category">Series Name</param>
        /// <param name="targetWatt">출력을 원하는 에너지(W)</param>
        /// <param name="x">계산된 x값 (레이저 파워 조절 인자)</param>
        /// <returns></returns>
        bool Lookup(Category category, double targetWatt, out double x);

    }
}
