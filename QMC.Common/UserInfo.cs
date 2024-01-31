using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common
{
    public enum UserAuthority
    {
        User,
        Administrator,
        Developer,
    }
    public class UserInfo
    {
        public UserAuthority Authority { set; get; }
        public string ID { set; get; }
        public string Name { set; get; } 
        public DateTime LogInTime { set; get; }
    }
}
