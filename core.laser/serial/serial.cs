using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Core.Laser
{
    public interface ISerial : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        uint Index { get; }

        /// <summary>
        /// 이름
        /// </summary>
        string Name { get; }

        bool IsReady { get; }

        bool IsBusy { get; }

        bool IsError { get; }

        bool Initialize(int port, int baudrate);

        bool Write(string data);

        string Read();

    }
}
