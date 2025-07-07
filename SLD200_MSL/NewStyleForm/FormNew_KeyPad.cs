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
            InitializeComponent();
            label_NumPad.Text = "0";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void AddDigit(string digit)
        {
            if (label_NumPad.Text == "0")
                label_NumPad.Text = digit;
            else
                label_NumPad.Text += digit;
        }

        private void button_Num_0_Click(object sender, EventArgs e) => AddDigit("0");
        private void button_Num_1_Click(object sender, EventArgs e) => AddDigit("1");
        private void button_Num_2_Click(object sender, EventArgs e) => AddDigit("2");
        private void button_Num_3_Click(object sender, EventArgs e) => AddDigit("3");
        private void button_Num_4_Click(object sender, EventArgs e) => AddDigit("4");
        private void button_Num_5_Click(object sender, EventArgs e) => AddDigit("5");
        private void button_Num_6_Click(object sender, EventArgs e) => AddDigit("6");
        private void button_Num_7_Click(object sender, EventArgs e) => AddDigit("7");
        private void button_Num_8_Click(object sender, EventArgs e) => AddDigit("8");
        private void button_Num_9_Click(object sender, EventArgs e) => AddDigit("9");
        private void button_Num_Dot_Click(object sender, EventArgs e)
        {
            if (!label_NumPad.Text.Contains("."))
            {
                if (string.IsNullOrEmpty(label_NumPad.Text))
                    label_NumPad.Text = "0";
                label_NumPad.Text += ".";
            }
        }
        private void button_Clear_Click(object sender, EventArgs e)
        {
            label_NumPad.Text = "0";
        }

        private void button_BackSpace_Click(object sender, EventArgs e)
        {
            if (label_NumPad.Text.Length > 0)
            {
                label_NumPad.Text = label_NumPad.Text.Substring(0, label_NumPad.Text.Length - 1);
                if (label_NumPad.Text == "")
                    label_NumPad.Text = "0";
            }
        }

        private void button_PlusMinus_Click(object sender, EventArgs e)
        {
            if (label_NumPad.Text.StartsWith("-"))
                label_NumPad.Text = label_NumPad.Text.Substring(1);
            else if (label_NumPad.Text != "0")
                label_NumPad.Text = "-" + label_NumPad.Text;
        }

        public double EnteredValue { get; private set; }
        public double MinValue { get; set; } = 0;
        public double MaxValue { get; set; } = 100000;

        private void button_Apply_Click(object sender, EventArgs e)
        {
            if (double.TryParse(label_NumPad.Text, out double result))
            {
                if (result < MinValue || result > MaxValue)
                {
                    MessageBox.Show($"입력값은 {MinValue} ~ {MaxValue} 범위여야 합니다.");
                    return;
                }

                EnteredValue = result;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("유효하지 않은 숫자입니다.");
            }
        }

        private string _expression = "";
        private void button_Plus_Click(object sender, EventArgs e)
        {
            _expression += label_NumPad.Text + "+";
            label_NumPad.Text = "0";
        }

        private void button_Minus_Click(object sender, EventArgs e)
        {

        }

        private void button_Multiply_Click(object sender, EventArgs e)
        {

        }

        private void button_Divide_Click(object sender, EventArgs e)
        {

        }

        private void button_Result_Click(object sender, EventArgs e)
        {
            _expression += label_NumPad.Text;
            try
            {
                var result = new DataTable().Compute(_expression, null);
                label_NumPad.Text = Convert.ToDouble(result).ToString();
                _expression = ""; // 초기화
            }
            catch
            {
                label_NumPad.Text = "Error";
                _expression = "";
            }
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
