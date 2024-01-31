using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Opticon
{
    public class OpticonBarcodeReaderCommunicator : SerialComm
    {
        [Serializable]
        public enum OpticonCommands
        {
            Start,
            Stop,
        }

        public int RetryCount { set; get; }
        public const byte Stx = 0x1b;
        public const byte Etx = 0x0d;

        public string GetCommandString(OpticonCommands commands)
        {
            string commandString = "";
            switch(commands)
            {
                case OpticonCommands.Start:
                    commandString = "Z";
                    break;
                case OpticonCommands.Stop:
                    commandString = "Y";
                    break;
                default:
                    break;
            }
            return commandString;
        }

        public int Send(OpticonCommands commands)
        {
            int ret = 0;
           
            string message = this.GetCommandString(commands);

            if ((ret = this.SendFrame(message, Stx, Etx)) != 0) return ret;

            return ret;
        }

        public virtual int SendAndReceive(OpticonCommands commands, out string response)
        {
            int ret = 0;
            response = string.Empty;
            bool etxDouble = false;
            int retryCount = 0;

            for (retryCount = 0; retryCount < 5 /*this.RetryCount*/; retryCount++)
            {
                if ((ret = this.Send(commands)) != 0) continue;

                if ((ret = ReceiveFrame(out response, "\r")) != 0) continue;
            }

            if (string.IsNullOrEmpty(response) == true)
            {
                ret = -1;
            }

            return ret;
        }

    }
}
