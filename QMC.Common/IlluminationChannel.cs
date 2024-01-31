using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    [Serializable]
    public class IlluminationChannel
    {
        public string ChannelName { set; get; }
        public int Channel { set; get; }
        public double Min { set; get; }
        public double Max { set; get; }
        public int Value { set; get; }
        public IlluminationChannel(string strChannelName)
        {
            ChannelName = strChannelName;
            Channel = 0;
            Min = 0;
            Max = 100;
            Value = 0;
        }
    }
}
