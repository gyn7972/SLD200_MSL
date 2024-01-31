
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMC.Common.Motion.Ajin.Motions
{
    public enum AsyncType
    {
        WithExecute,
        AfterExecute
    }
    [DefaultPropertyAttribute("기타")]
    public class AxisHomeSequenceConfiguration
    {

        #region Field
        [DefaultValue("")]
        public int Order { get; set; } = 0;
        [DefaultValue("")]
        public uint AxisUID { get; set; } = 0;
        [DefaultValue("")]
        public string AxisName { get; set; } = "";
        [DefaultValue("")]
        public AsyncType AsyncType { get; set; } = AsyncType.WithExecute;



        #endregion

        #region Property

      

        #endregion

        public virtual AxisHomeSequenceConfiguration GetDeepCopy()
        {
            AxisHomeSequenceConfiguration AxisHomeSequenceConfiguration = new AxisHomeSequenceConfiguration();
            AxisHomeSequenceConfiguration.Order = this.Order;
            AxisHomeSequenceConfiguration.AxisName = this.AxisName;
            AxisHomeSequenceConfiguration.AxisUID = this.AxisUID;
            AxisHomeSequenceConfiguration.AsyncType = this.AsyncType;

            return AxisHomeSequenceConfiguration;

        }


        public AxisHomeSequenceConfiguration()
        {
        }
    }
    public class AxisHomeSequenceConfigurationColletion : Collection<AxisHomeSequenceConfiguration>
    {

    }


}
