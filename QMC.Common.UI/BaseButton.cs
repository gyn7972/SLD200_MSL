using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Part;

namespace QMC.Common.UI
{
    public class BaseButton : Button
    {
        protected FormBaseConfiguration m_Configuration;
        protected UserAuthority m_Authority;
        protected RunStatus m_EnableStatus;
        public BaseButton()
        {
            m_Configuration = new FormBaseConfiguration();    
            this.BackColor = Color.FromArgb(78,78,78);
            this.FlatStyle = FlatStyle.Flat;
            this.ForeColor = Color.WhiteSmoke;
            this.Size = m_Configuration.ButtonSize;
            // this.FlatStyle = FlatStyle.Standard;
            //this.Click += BaseButton_Click;
            m_Authority = UserAuthority.User;
            m_EnableStatus = RunStatus.Stop;
        }

        private void BaseButton_Click(object sender, EventArgs e)
        {
            BaseButton baseButton = sender as BaseButton;
            this.BackColor = Color.FromArgb(78, 78, 78);
            this.FlatStyle = FlatStyle.Flat;
            this.ForeColor = Color.WhiteSmoke;
            baseButton.BackColor = Color.FromArgb(200,200,200);
            this.TextAlign = ContentAlignment.BottomCenter;
            
        }

        public void SetUserAuthority(UserAuthority authority)
        {
            m_Authority = authority;
        }

        public void SetUser(UserInfo user)
        {
            if(user.Authority >= m_Authority)
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled = false;
            }
        }

        public void SetRunStatus(RunStatus status)
        {
            if(status >= m_EnableStatus)
            {
                this.Enabled = true;
            }
            else
            {
                this.Enabled= false;
            }
        }
       
        
    }

    
}
