using System.Numerics;

namespace QMC.Core.Laser
{
    #region 2차원 스캐너 필드 보정 데이타 구조체 정의
    /// <summary>
    /// 2차원 스캐너 필드 보정 데이타 
    /// </summary>
    public struct CorrectionData2D
    {
        /// <summary>
        /// 논리적인 좌표값
        /// </summary>
        public Vector2 Logical { get; set; }
        /// <summary>
        /// 실제 측정된 좌표값
        /// </summary>
        public Vector2 Measured { get; set; }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="reference">논리적인 좌표값</param>
        /// <param name="measured">실제 측정된 좌표값</param>
        public CorrectionData2D(Vector2 reference, Vector2 measured)
        {
            this.Logical = reference;
            this.Measured = measured;
        }
        /// <summary>
        /// 논리 좌표 문자열 출력
        /// </summary>
        /// <returns></returns>
        public string LogicalToString()
        {
            return $"{Logical.X:F3}, {Logical.Y:F3}";
        }
        /// <summary>
        /// 실측값 문자열 출력
        /// </summary>
        /// <returns></returns>
        public string MeasuredToString()
        {
            return $"{Measured.X:F3}, {Measured.Y:F3}";
        }
    }
    #endregion

    /// <summary>
    /// 스캐너 필드 보정에 대한 결과 이벤트 통지용 델리게이트
    /// </summary>
    /// <param name="sender">IRtcCorrection 인터페이스</param>
    /// <param name="success">변환 성공 여부</param>
    /// <param name="message">변환 로그 메시지</param>
    public delegate void ResultEventHandler(object sender, bool success, string message);

    /// <summary>
    /// RtcCorrection 인터페이스
    /// </summary>
    public interface ICorrection
    {
        #region 공개 속성
        /// <summary>
        /// 결과 통보용 이벤트 핸들러
        /// </summary>
        event ResultEventHandler OnResult;
        /// <summary>
        /// 입력 데이타의 행 개수
        /// </summary>
        int Rows { get; set; }
        /// <summary>
        /// 입력 데이타의 열 개수
        /// </summary>
        int Cols { get; set; }
        /// <summary>
        /// 입력 보정 파일 (correction 폴더에서의 상대적 경로)
        /// </summary>
        string SourceCorrectionFile { get; set; }
        /// <summary>
        /// 출력 보정 파일 (correction 폴더에서의 상대적 경로)
        /// </summary>
        string TargetCorrectionFile { get; set; }
        #endregion

        /// <summary>
        /// 입력 데이타 모두 제거
        /// </summary>
        void Clear();
        /// <summary>
        /// 변환 시작
        /// </summary>
        /// <returns>성공 여부</returns>
        bool Convert();

        /// <summary>
        /// 측정 데이타 입력 (절대 좌표 값)
        /// </summary>
        /// <param name="row">행</param>
        /// <param name="col">열</param>
        /// <param name="reference">기준 좌표(mm)</param>
        /// <param name="absoulte">측정 절대 좌표 (mm)</param>
        /// <returns></returns>
        bool AddAbsolute(int row, int col, Vector2 reference, Vector2 absoulte);

        /// <summary>
        /// 측정 데이타 입력 (상대 좌표값) 
        /// ex) 상대 좌표값 = 비전 오차량 만큼만 입력
        /// </summary>
        /// <param name="row">행</param>
        /// <param name="col">열</param>
        /// <param name="reference">기준 좌표값 (mm)</param>
        /// <param name="relative">측정 상대 좌표 (mm)</param>
        /// <returns></returns>
        bool AddRelative(int row, int col, Vector2 reference, Vector2 relative);
    }
}
