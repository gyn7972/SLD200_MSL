using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWA150SA_Onsemi300
{
    public class BaseToggleButton : BaseButton
    {
        protected bool m_IsClicked;
        public BaseToggleButton()
        {
            m_IsClicked = false;
        }

        public void UpdateToggleStatus(bool bOn)
        {
            m_IsClicked = bOn;
            if (bOn)
            {
                this.BackColor = Color.GreenYellow;
                this.ForeColor = Color.FromArgb(78, 78, 78); 
            }
            else
            {
                this.BackColor = Color.FromArgb(78, 78, 78);
                this.ForeColor = Color.WhiteSmoke;
            }
        }

        public void SetButtonText(string strText)
        {
            this.Text = strText;
        }

        public bool GetButtonStatus()
        {
            return m_IsClicked;
        }
    }
}
