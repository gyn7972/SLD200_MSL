using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    //
    // 요약:
    //     Homing 방법을 정의합니다.
    [Serializable]
    public enum HomingMethod
    {
        //
        // 요약:
        //     즉시 Homing을 완료합니다.
        None = 0,
        //
        // 요약:
        //     홈 센서를 기준으로 Homing합니다.
        HomeSensor = 1,
        //
        // 요약:
        //     Negative 센서를 기준으로 Homing합니다.
        NegativeSensor = 2,
        //
        // 요약:
        //     Positive 센서를 기준으로 Homing합니다.
        PositiveSensor = 3,
        //
        // 요약:
        //     인덱스를 기준으로 Homing합니다.
        Index = 4
    }
    [Serializable]
    [TypeConverter(typeof(HomingSpecificationConverter))]
    public class HomingSpecification
    {
        //
        // 요약:
        //     속도를 가져오거나 설정합니다.
        public double Velocity { get; set; }
        //
        // 요약:
        //     가속도를 가져오거나 설정합니다.
        public double Acceleration { get; set; }
        //
        // 요약:
        //     감속도를 가져오거나 설정합니다.
        public double Deceleration { get; set; }
        //
        // 요약:
        //     Negative 방향으로 움직일 수 있는 최대 거리를 가져오거나 설정합니다.
        public double NegativePosition { get; set; }
        //
        // 요약:
        //     Positive 방향으로 움직일 수 있는 최대 거리를 가져오거나 설정합니다.
        public double PositivePosition { get; set; }
        //
        // 요약:
        //     sensor를 이용하여 homing후 이동하고자 하는 거리를 가져오거나 설정한다.
        public double EscapeDistance { get; set; }
        //
        // 요약:
        //     homing 완료후 설정하고자 하는 위치를 가져오거나 설정한다.
        public double HomePosition { get; set; }
        //
        // 요약:
        //     Homing 방법을 가져오거나 설정합니다.
        [DefaultValue(HomingMethod.NegativeSensor)]
        public HomingMethod Method { get; set; }
        //
        // 요약:
        //     Precise Search를 수행할지 여부를 가져오거나 설정한다
        [DefaultValue(false)]
        public bool EnablePreciseSearch { get; set; }
        //
        // 요약:
        //     Precise Search시 사용하는 속도의 percent를 가져오거나 설정한다
        [DefaultValue(10)]
        public int PreciseSearchVelocityPercent { get; set; }
        //
        // 요약:
        //     Homing과정에서 index검사를 수행할지 여부를 가져오거나 설정한다
        [DefaultValue(false)]
        public bool EnableIndexSearch { get; set; }

        //
        // 요약:
        //     HomingSpecification의 DeepCopy본을 반환합니다.
        //
        // 반환 값:
        //     복사된 HomingSpecification 개체입니다.
        public virtual HomingSpecification GetDeepCopy()
        {
            HomingSpecification HomingSpecification = new HomingSpecification();
            HomingSpecification.Acceleration = this.Acceleration;
            HomingSpecification.Deceleration = this.Deceleration;
            HomingSpecification.EnableIndexSearch = this.EnableIndexSearch;
            HomingSpecification.EnablePreciseSearch = this.EnablePreciseSearch;
            HomingSpecification.EscapeDistance = this.EscapeDistance;
            HomingSpecification.HomePosition = this.HomePosition;
            HomingSpecification.Method = this.Method;
            HomingSpecification.NegativePosition = this.NegativePosition;
            HomingSpecification.PositivePosition = this.PositivePosition;
            HomingSpecification.PreciseSearchVelocityPercent = this.PreciseSearchVelocityPercent;
            HomingSpecification.Velocity = this.Velocity;
            
            return HomingSpecification;
        }
        //
        // 요약:
        //     기본값들이 설정된 Trajectory를 가져옵니다.
        //
        // 반환 값:
        //     Trajectory 개체입니다.
        //public virtual Trajectory GetDefaultTrajectory();
        //
        // 요약:
        //     재정의 되었습니다.
        //
        // 반환 값:
        //     개체를 나타내는 문자열입니다.
        //public string ToString()
        //{
        //    string strValue = "";

        //    return strValue;
        //}
    }
}
