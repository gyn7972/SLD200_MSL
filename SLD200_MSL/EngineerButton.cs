using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public partial class EngineerButton : UserControl
    {
        protected bool m_IsClicked = false;
        public FormBaseConfiguration formConfiguration { get; set; }

        public EngineerButton()
        {
            InitializeComponent();
            formConfiguration = new FormBaseConfiguration();
            this.Size = new Size(formConfiguration.ButtonSize.Width + 50, formConfiguration.ButtonSize.Height + 30);
            this.gruopBoxEngineer.Size = new Size(formConfiguration.ButtonSize.Width + 10, formConfiguration.ButtonSize.Height + 25);
            this.gruopBoxEngineer.Controls.Add(this.baseButtonEngineer);
            this.gruopBoxEngineer.ForeColor = Color.White;
            this.baseButtonEngineer.Size = new Size(formConfiguration.ButtonSize.Width, formConfiguration.ButtonSize.Height);
            this.baseButtonEngineer.Location = new Point(5, formConfiguration.ButtonSize.Height - 15);
        }

        
        private void UpdateButtonToggle()
        {
            if (m_IsClicked == true)
            {
                this.baseButtonEngineer.BackColor = Color.FromArgb(78, 78, 78);
                m_IsClicked = false;
            }
            else if (m_IsClicked == false)
            {
                this.baseButtonEngineer.BackColor = Color.FromArgb(40, 40, 40);
                m_IsClicked = true;
            }
        }

        private void baseButton1_Click(object sender, EventArgs e)
        {
            UpdateButtonToggle();
        }


    }
}
