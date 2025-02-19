using System.Drawing;
using System.Windows.Forms;

namespace QMC_System_UI
{
    partial class FormNew_CommunicationTerminal
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
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.textBox28 = new System.Windows.Forms.TextBox();
            this.label127 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label53
            // 
            this.label53.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label53.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label53.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label53.ForeColor = System.Drawing.Color.Lime;
            this.label53.Location = new System.Drawing.Point(172, 59);
            this.label53.Margin = new System.Windows.Forms.Padding(6);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(383, 301);
            this.label53.TabIndex = 18;
            this.label53.Text = "TEST";
            // 
            // label52
            // 
            this.label52.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label52.Location = new System.Drawing.Point(15, 57);
            this.label52.Margin = new System.Windows.Forms.Padding(6);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(145, 24);
            this.label52.TabIndex = 17;
            this.label52.Text = "Received Message :";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox28
            // 
            this.textBox28.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox28.Location = new System.Drawing.Point(172, 16);
            this.textBox28.Margin = new System.Windows.Forms.Padding(6);
            this.textBox28.Name = "textBox28";
            this.textBox28.Size = new System.Drawing.Size(383, 24);
            this.textBox28.TabIndex = 24;
            this.textBox28.Text = "000.000";
            // 
            // label127
            // 
            this.label127.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label127.Location = new System.Drawing.Point(15, 15);
            this.label127.Margin = new System.Windows.Forms.Padding(6);
            this.label127.Name = "label127";
            this.label127.Size = new System.Drawing.Size(145, 24);
            this.label127.TabIndex = 23;
            this.label127.Text = "Send Message :";
            this.label127.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(33, 301);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 59);
            this.button1.TabIndex = 25;
            this.button1.Text = "Query";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FormNew_CommunicationTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 371);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox28);
            this.Controls.Add(this.label127);
            this.Controls.Add(this.label53);
            this.Controls.Add(this.label52);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_CommunicationTerminal";
            this.Text = "QMC Communication Terminal";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label53;
        private Label label52;
        private TextBox textBox28;
        private Label label127;
        private Button button1;
    }
}