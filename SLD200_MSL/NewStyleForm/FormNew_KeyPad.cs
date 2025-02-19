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
    public partial class FormNew_KeyPad : Form
    {
        public FormNew_KeyPad()
        {
            label_NumPad = new Label();

            label_NumPad.Text = "0";

            InitializeComponent();
        }

        private void button_Num_0_Click(object sender, EventArgs e)
        {
            label_NumPad.Text += "0";
        }

        private void button_Num_1_Click(object sender, EventArgs e)
        {
            //  왼쪽에 0이 있으면 0을 지우고 1을 입력

            if (label_NumPad.Text == "0")
            {
                label_NumPad.Text = "1";
            }
            else
            {
                label_NumPad.Text += "1";
            }
        }

        private void button_Num_2_Click(object sender, EventArgs e)
        {
            //  왼쪽에 0이 있으면 0을 지우고 2를 입력
            if (label_NumPad.Text == "0")
            {
                label_NumPad.Text = "2";
            }
            else
            {
                label_NumPad.Text += "2";
            }   
        }

        private void button_Num_3_Click(object sender, EventArgs e)
        {
            //  왼쪽에 0이 있으면 0을 지우고 3을 입력
            if (label_NumPad.Text == "0")
            {
                label_NumPad.Text = "3";
            }
            else
            {
                label_NumPad.Text += "3";
            }
        }

        private void button_Num_4_Click(object sender, EventArgs e)
        {
            //  왼쪽에 0이 있으면 0을 지우고 4를 입력
            if (label_NumPad.Text == "0")
            {
                label_NumPad.Text = "4";
            }
            else
            {
                label_NumPad.Text += "4";
            }
        }

        private void button_Num_5_Click(object sender, EventArgs e)
        {
            //  왼쪽에 0이 있으면 0을 지우고 5를 입력
            if (label_NumPad.Text == "0")
            {
                label_NumPad.Text = "5";
            }
            else
            {
                label_NumPad.Text += "5";
            }
        }

        private void button_Num_6_Click(object sender, EventArgs e)
        {
            //  왼쪽에 0이 있으면 0을 지우고 6을 입력
            if (label_NumPad.Text == "0")
            {
                label_NumPad.Text = "6";
            }
            else
            {
                label_NumPad.Text += "6";
            }
        }

        private void button_Num_7_Click(object sender, EventArgs e)
        {
            //  왼쪽에 0이 있으면 0을 지우고 7을 입력
            if (label_NumPad.Text == "0")
            {
                label_NumPad.Text = "7";
            }
            else
            {
                label_NumPad.Text += "7";
            }
        }

        private void button_Num_8_Click(object sender, EventArgs e)
        {
            //  왼쪽에 0이 있으면 0을 지우고 8을 입력
            if (label_NumPad.Text == "0")
            {
                label_NumPad.Text = "8";
            }
            else
            {
                label_NumPad.Text += "8";
            }
        }

        private void button_Num_9_Click(object sender, EventArgs e)
        {
            //  왼쪽에 0이 있으면 0을 지우고 9를 입력
            if (label_NumPad.Text == "0")
            {
                label_NumPad.Text = "9";
            }
            else
            {
                label_NumPad.Text += "9";
            }
        }

        private void button_Num_Dot_Click(object sender, EventArgs e)
        {
            //  왼쪽에 아무것도 없으면 0을 붙여서 입력
            if (label_NumPad.Text == "")
            {
                label_NumPad.Text = "0";
            }

            label_NumPad.Text += ".";
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            label_NumPad.Text = "0";
        }

        private void button_BackSpace_Click(object sender, EventArgs e)
        {
            if (label_NumPad.Name.Length > 0)
            {
                label_NumPad.Text = label_NumPad.Text.Substring(0, label_NumPad.Text.Length - 1);
            }
        }
    }
}
