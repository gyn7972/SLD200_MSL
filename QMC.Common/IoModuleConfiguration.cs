using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;


namespace QMC.Common
{
    [Serializable]

    public class IoModuleConfiguration
    {
        //
        // 요약:
        //     IoModuleConfiguration 클래스의 새 인스턴스를 초기화합니다.
        public IoModuleConfiguration()
        {

        }
        [Browsable(false)]
        public int BoardNo { get; set; }
        //
        // 요약:
        //     번호를 가져오거나 설정합니다.
        [Browsable(true)]
        [DefaultValue(0)]
        public uint No { get; set; }
        //
        // 요약:
        //     모듈을 소유하고 있는 보드의 UID를 가져오거나 설정합니다.
        //
        // 요약:
        //     컴포넌트 UID를 가져오거나 설정합니다.
        [Browsable(false)]
        public string ComponentUid { get; set; }
        //
        // 요약:
        //     입력의 개수를 가져오거나 설정합니다.
        [Browsable(true)]
        [DefaultValue(0)]
        public int InputCount { get; set; }
        //
        // 요약:
        //     출력의 개수를 가져오거나 설정합니다.
        [Browsable(true)]
        [DefaultValue(0)]
        public int OutputCount { get; set; }
        //
        // 요약:
        //     입력 값을 다시 읽어오는 간격을 가져오거나 설정합니다.
        [Browsable(true)]
        [DefaultValue(10)]
        public double RefreshTime { get; set; }
        //
        // 요약:
        //     시물레이션인지 여부를 가져오거나 설정합니다.
        [Browsable(true)]
        [DefaultValue(false)]
        public virtual bool Simulated { get; set; }
        //
        // 요약:
        //     원격 지원 여부를 가져오거나 설정합니다.
        [Browsable(true)]
        [DefaultValue(false)]
        public virtual bool Remoted { get; set; }
        public int PointCount { set; get; }

        protected virtual void SetDefaultValues()
        {
            this.No = 0;
            this.ComponentUid = "";
            this.InputCount = 0;
            this.OutputCount = 0;
            this.RefreshTime = 100;
            this.Remoted = false;
            this.Simulated = false;
            this.BoardNo = 0;
            this.PointCount = 0;
        }


        public int Save(FileStream fs)
        {
            int ret = 0;

            if ((ret = SaveManager.BinarySerialize(fs, this)) != 0) return ret;

            return ret;
        }
    }
}
