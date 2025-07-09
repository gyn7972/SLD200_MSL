using System.Drawing;
using System.Windows.Forms;

namespace SLD200_MSL
{
    partial class FormNew_KeyPad
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label_MinValue = new System.Windows.Forms.Label();
            this.button_Apply = new System.Windows.Forms.Button();
            this.label_MaxValue = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_NumPad = new System.Windows.Forms.Label();
            this.button_PlusMinus = new System.Windows.Forms.Button();
            this.button_Clear = new System.Windows.Forms.Button();
            this.button_BackSpace = new System.Windows.Forms.Button();
            this.button_Num_9 = new System.Windows.Forms.Button();
            this.button_Num_8 = new System.Windows.Forms.Button();
            this.button_Num_7 = new System.Windows.Forms.Button();
            this.button_Num_6 = new System.Windows.Forms.Button();
            this.button_Num_5 = new System.Windows.Forms.Button();
            this.button_Num_4 = new System.Windows.Forms.Button();
            this.button_Num_3 = new System.Windows.Forms.Button();
            this.button_Num_2 = new System.Windows.Forms.Button();
            this.button_Num_1 = new System.Windows.Forms.Button();
            this.button_Num_Dot = new System.Windows.Forms.Button();
            this.button_Num_0 = new System.Windows.Forms.Button();
            this.button_Result = new System.Windows.Forms.Button();
            this.button_Divide = new System.Windows.Forms.Button();
            this.button_Multiply = new System.Windows.Forms.Button();
            this.button_Minus = new System.Windows.Forms.Button();
            this.button_Plus = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.button_Cancel);
            this.panel1.Controls.Add(this.label_MinValue);
            this.panel1.Controls.Add(this.button_Apply);
            this.panel1.Controls.Add(this.label_MaxValue);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label_NumPad);
            this.panel1.Controls.Add(this.button_PlusMinus);
            this.panel1.Controls.Add(this.button_Clear);
            this.panel1.Controls.Add(this.button_BackSpace);
            this.panel1.Controls.Add(this.button_Num_9);
            this.panel1.Controls.Add(this.button_Num_8);
            this.panel1.Controls.Add(this.button_Num_7);
            this.panel1.Controls.Add(this.button_Num_6);
            this.panel1.Controls.Add(this.button_Num_5);
            this.panel1.Controls.Add(this.button_Num_4);
            this.panel1.Controls.Add(this.button_Num_3);
            this.panel1.Controls.Add(this.button_Num_2);
            this.panel1.Controls.Add(this.button_Num_1);
            this.panel1.Controls.Add(this.button_Num_Dot);
            this.panel1.Controls.Add(this.button_Num_0);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(559, 355);
            this.panel1.TabIndex = 0;
            // 
            // button_Cancel
            // 
            this.button_Cancel.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Cancel.Location = new System.Drawing.Point(369, 280);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(178, 63);
            this.button_Cancel.TabIndex = 20;
            this.button_Cancel.Text = "Cancel";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // label_MinValue
            // 
            this.label_MinValue.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label_MinValue.ForeColor = System.Drawing.Color.Red;
            this.label_MinValue.Location = new System.Drawing.Point(429, 37);
            this.label_MinValue.Name = "label_MinValue";
            this.label_MinValue.Size = new System.Drawing.Size(96, 27);
            this.label_MinValue.TabIndex = 23;
            this.label_MinValue.Text = "0";
            this.label_MinValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_Apply
            // 
            this.button_Apply.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Apply.Location = new System.Drawing.Point(369, 213);
            this.button_Apply.Name = "button_Apply";
            this.button_Apply.Size = new System.Drawing.Size(178, 63);
            this.button_Apply.TabIndex = 19;
            this.button_Apply.Text = "Apply";
            this.button_Apply.UseVisualStyleBackColor = true;
            this.button_Apply.Click += new System.EventHandler(this.button_Apply_Click);
            // 
            // label_MaxValue
            // 
            this.label_MaxValue.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label_MaxValue.ForeColor = System.Drawing.Color.Red;
            this.label_MaxValue.Location = new System.Drawing.Point(429, 9);
            this.label_MaxValue.Name = "label_MaxValue";
            this.label_MaxValue.Size = new System.Drawing.Size(96, 27);
            this.label_MaxValue.TabIndex = 22;
            this.label_MaxValue.Text = "100,000";
            this.label_MaxValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(366, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 27);
            this.label3.TabIndex = 21;
            this.label3.Text = "Min :";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(366, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 27);
            this.label2.TabIndex = 20;
            this.label2.Text = "Max :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_NumPad
            // 
            this.label_NumPad.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label_NumPad.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_NumPad.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_NumPad.ForeColor = System.Drawing.Color.Lime;
            this.label_NumPad.Location = new System.Drawing.Point(9, 9);
            this.label_NumPad.Name = "label_NumPad";
            this.label_NumPad.Size = new System.Drawing.Size(351, 63);
            this.label_NumPad.TabIndex = 19;
            this.label_NumPad.Text = "label1";
            this.label_NumPad.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_PlusMinus
            // 
            this.button_PlusMinus.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_PlusMinus.Location = new System.Drawing.Point(369, 146);
            this.button_PlusMinus.Name = "button_PlusMinus";
            this.button_PlusMinus.Size = new System.Drawing.Size(178, 63);
            this.button_PlusMinus.TabIndex = 13;
            this.button_PlusMinus.Text = "+ / -";
            this.button_PlusMinus.UseVisualStyleBackColor = true;
            this.button_PlusMinus.Click += new System.EventHandler(this.button_PlusMinus_Click);
            // 
            // button_Clear
            // 
            this.button_Clear.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Clear.Location = new System.Drawing.Point(461, 77);
            this.button_Clear.Name = "button_Clear";
            this.button_Clear.Size = new System.Drawing.Size(86, 63);
            this.button_Clear.TabIndex = 12;
            this.button_Clear.Text = "C";
            this.button_Clear.UseVisualStyleBackColor = true;
            this.button_Clear.Click += new System.EventHandler(this.button_Clear_Click);
            // 
            // button_BackSpace
            // 
            this.button_BackSpace.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_BackSpace.Location = new System.Drawing.Point(369, 77);
            this.button_BackSpace.Name = "button_BackSpace";
            this.button_BackSpace.Size = new System.Drawing.Size(86, 63);
            this.button_BackSpace.TabIndex = 11;
            this.button_BackSpace.Text = "BS";
            this.button_BackSpace.UseVisualStyleBackColor = true;
            this.button_BackSpace.Click += new System.EventHandler(this.button_BackSpace_Click);
            // 
            // button_Num_9
            // 
            this.button_Num_9.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_9.Location = new System.Drawing.Point(248, 77);
            this.button_Num_9.Name = "button_Num_9";
            this.button_Num_9.Size = new System.Drawing.Size(112, 63);
            this.button_Num_9.TabIndex = 10;
            this.button_Num_9.Text = "9";
            this.button_Num_9.UseVisualStyleBackColor = true;
            this.button_Num_9.Click += new System.EventHandler(this.button_Num_9_Click);
            // 
            // button_Num_8
            // 
            this.button_Num_8.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_8.Location = new System.Drawing.Point(128, 77);
            this.button_Num_8.Name = "button_Num_8";
            this.button_Num_8.Size = new System.Drawing.Size(112, 63);
            this.button_Num_8.TabIndex = 9;
            this.button_Num_8.Text = "8";
            this.button_Num_8.UseVisualStyleBackColor = true;
            this.button_Num_8.Click += new System.EventHandler(this.button_Num_8_Click);
            // 
            // button_Num_7
            // 
            this.button_Num_7.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_7.Location = new System.Drawing.Point(9, 77);
            this.button_Num_7.Name = "button_Num_7";
            this.button_Num_7.Size = new System.Drawing.Size(112, 63);
            this.button_Num_7.TabIndex = 8;
            this.button_Num_7.Text = "7";
            this.button_Num_7.UseVisualStyleBackColor = true;
            this.button_Num_7.Click += new System.EventHandler(this.button_Num_7_Click);
            // 
            // button_Num_6
            // 
            this.button_Num_6.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_6.Location = new System.Drawing.Point(248, 145);
            this.button_Num_6.Name = "button_Num_6";
            this.button_Num_6.Size = new System.Drawing.Size(112, 63);
            this.button_Num_6.TabIndex = 7;
            this.button_Num_6.Text = "6";
            this.button_Num_6.UseVisualStyleBackColor = true;
            this.button_Num_6.Click += new System.EventHandler(this.button_Num_6_Click);
            // 
            // button_Num_5
            // 
            this.button_Num_5.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_5.Location = new System.Drawing.Point(128, 145);
            this.button_Num_5.Name = "button_Num_5";
            this.button_Num_5.Size = new System.Drawing.Size(112, 63);
            this.button_Num_5.TabIndex = 6;
            this.button_Num_5.Text = "5";
            this.button_Num_5.UseVisualStyleBackColor = true;
            this.button_Num_5.Click += new System.EventHandler(this.button_Num_5_Click);
            // 
            // button_Num_4
            // 
            this.button_Num_4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_4.Location = new System.Drawing.Point(9, 145);
            this.button_Num_4.Name = "button_Num_4";
            this.button_Num_4.Size = new System.Drawing.Size(112, 63);
            this.button_Num_4.TabIndex = 5;
            this.button_Num_4.Text = "4";
            this.button_Num_4.UseVisualStyleBackColor = true;
            this.button_Num_4.Click += new System.EventHandler(this.button_Num_4_Click);
            // 
            // button_Num_3
            // 
            this.button_Num_3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_3.Location = new System.Drawing.Point(248, 213);
            this.button_Num_3.Name = "button_Num_3";
            this.button_Num_3.Size = new System.Drawing.Size(112, 63);
            this.button_Num_3.TabIndex = 4;
            this.button_Num_3.Text = "3";
            this.button_Num_3.UseVisualStyleBackColor = true;
            this.button_Num_3.Click += new System.EventHandler(this.button_Num_3_Click);
            // 
            // button_Num_2
            // 
            this.button_Num_2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_2.Location = new System.Drawing.Point(128, 213);
            this.button_Num_2.Name = "button_Num_2";
            this.button_Num_2.Size = new System.Drawing.Size(112, 63);
            this.button_Num_2.TabIndex = 3;
            this.button_Num_2.Text = "2";
            this.button_Num_2.UseVisualStyleBackColor = true;
            this.button_Num_2.Click += new System.EventHandler(this.button_Num_2_Click);
            // 
            // button_Num_1
            // 
            this.button_Num_1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_1.Location = new System.Drawing.Point(9, 213);
            this.button_Num_1.Name = "button_Num_1";
            this.button_Num_1.Size = new System.Drawing.Size(112, 63);
            this.button_Num_1.TabIndex = 2;
            this.button_Num_1.Text = "1";
            this.button_Num_1.UseVisualStyleBackColor = true;
            this.button_Num_1.Click += new System.EventHandler(this.button_Num_1_Click);
            // 
            // button_Num_Dot
            // 
            this.button_Num_Dot.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_Dot.Location = new System.Drawing.Point(248, 281);
            this.button_Num_Dot.Name = "button_Num_Dot";
            this.button_Num_Dot.Size = new System.Drawing.Size(112, 63);
            this.button_Num_Dot.TabIndex = 1;
            this.button_Num_Dot.Text = ".";
            this.button_Num_Dot.UseVisualStyleBackColor = true;
            this.button_Num_Dot.Click += new System.EventHandler(this.button_Num_Dot_Click);
            // 
            // button_Num_0
            // 
            this.button_Num_0.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Num_0.Location = new System.Drawing.Point(9, 281);
            this.button_Num_0.Name = "button_Num_0";
            this.button_Num_0.Size = new System.Drawing.Size(232, 63);
            this.button_Num_0.TabIndex = 0;
            this.button_Num_0.Text = "0";
            this.button_Num_0.UseVisualStyleBackColor = true;
            this.button_Num_0.Click += new System.EventHandler(this.button_Num_0_Click);
            // 
            // button_Result
            // 
            this.button_Result.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Result.Location = new System.Drawing.Point(575, 294);
            this.button_Result.Name = "button_Result";
            this.button_Result.Size = new System.Drawing.Size(24, 63);
            this.button_Result.TabIndex = 18;
            this.button_Result.Text = "=";
            this.button_Result.UseVisualStyleBackColor = true;
            this.button_Result.Visible = false;
            this.button_Result.Click += new System.EventHandler(this.button_Result_Click);
            // 
            // button_Divide
            // 
            this.button_Divide.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Divide.Location = new System.Drawing.Point(575, 20);
            this.button_Divide.Name = "button_Divide";
            this.button_Divide.Size = new System.Drawing.Size(24, 63);
            this.button_Divide.TabIndex = 17;
            this.button_Divide.Text = "/";
            this.button_Divide.UseVisualStyleBackColor = true;
            this.button_Divide.Visible = false;
            this.button_Divide.Click += new System.EventHandler(this.button_Divide_Click);
            // 
            // button_Multiply
            // 
            this.button_Multiply.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Multiply.Location = new System.Drawing.Point(575, 226);
            this.button_Multiply.Name = "button_Multiply";
            this.button_Multiply.Size = new System.Drawing.Size(24, 63);
            this.button_Multiply.TabIndex = 16;
            this.button_Multiply.Text = "*";
            this.button_Multiply.UseVisualStyleBackColor = true;
            this.button_Multiply.Visible = false;
            this.button_Multiply.Click += new System.EventHandler(this.button_Multiply_Click);
            // 
            // button_Minus
            // 
            this.button_Minus.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Minus.Location = new System.Drawing.Point(575, 89);
            this.button_Minus.Name = "button_Minus";
            this.button_Minus.Size = new System.Drawing.Size(24, 63);
            this.button_Minus.TabIndex = 15;
            this.button_Minus.Text = "-";
            this.button_Minus.UseVisualStyleBackColor = true;
            this.button_Minus.Visible = false;
            this.button_Minus.Click += new System.EventHandler(this.button_Minus_Click);
            // 
            // button_Plus
            // 
            this.button_Plus.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.button_Plus.Location = new System.Drawing.Point(575, 158);
            this.button_Plus.Name = "button_Plus";
            this.button_Plus.Size = new System.Drawing.Size(24, 63);
            this.button_Plus.TabIndex = 14;
            this.button_Plus.Text = "+";
            this.button_Plus.UseVisualStyleBackColor = true;
            this.button_Plus.Visible = false;
            this.button_Plus.Click += new System.EventHandler(this.button_Plus_Click);
            // 
            // FormNew_KeyPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 373);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_Plus);
            this.Controls.Add(this.button_Minus);
            this.Controls.Add(this.button_Result);
            this.Controls.Add(this.button_Multiply);
            this.Controls.Add(this.button_Divide);
            this.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_KeyPad";
            this.Text = "The Data Entry Keypad Dialog";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Button button_Num_0;
        private Button button_Result;
        private Button button_Divide;
        private Button button_Multiply;
        private Button button_Minus;
        private Button button_Plus;
        private Button button_PlusMinus;
        private Button button_Clear;
        private Button button_BackSpace;
        private Button button_Num_9;
        private Button button_Num_8;
        private Button button_Num_7;
        private Button button_Num_6;
        private Button button_Num_5;
        private Button button_Num_4;
        private Button button_Num_3;
        private Button button_Num_2;
        private Button button_Num_1;
        private Button button_Num_Dot;
        private Label label2;
        public Label label_NumPad;
        private Label label3;
        private Label label_MinValue;
        private Label label_MaxValue;
        private Button button_Apply;
        private Button button_Cancel;
    }
}