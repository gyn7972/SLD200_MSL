using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public interface IExecuter
    {
        int Initialize();  //초기화
        int Work();    //동작
        void Stop();    //정지
    }
}
