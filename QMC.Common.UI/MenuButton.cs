using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QMC.Common.UI
{
    public class MenuButton : Button
    {
        protected Image m_imgOnImage;
        protected Image m_imgOffImage;
        private bool m_bButtonState;
        public MenuButton()
        {
            this.BackColor = Color.FromArgb(78, 78, 78);
            this.ImageAlign = ContentAlignment.MiddleCenter;
            this.TextAlign = ContentAlignment.BottomCenter;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 1;
            this.FlatAppearance.BorderColor = Color.FromArgb(78, 78, 78);
            this.BackgroundImageLayout = ImageLayout.Center;
            //this.ImageAlign = ContentAlignment.MiddleCenter;
            //this.TextAlign = ContentAlignment.BottomCenter;
            this.TabStop = false;
        }
        public void SetImage(Image onImage, Image offImage)
        {
            this.m_imgOnImage = onImage;
            this.m_imgOffImage = offImage;
        }

        public void SetButtonState(bool bOn)
        {
            if(bOn)
            {
                this.Image = m_imgOnImage;
                this.ForeColor = Color.SkyBlue;
            }
            else
            {
                this.Image=m_imgOffImage;
                this.ForeColor = Color.White;
            }
            this.m_bButtonState = bOn;
        }

        public bool GetButtonState()
        {
            return m_bButtonState;
        }
    }
}
