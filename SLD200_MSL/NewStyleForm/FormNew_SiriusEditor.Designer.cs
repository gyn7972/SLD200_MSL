using System.Drawing;
using System.Windows.Forms;

namespace SLD200_MSL
{
    partial class FormNew_SiriusEditor
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
            this.SiriusEditor = new SpiralLab.Sirius.SiriusEditorForm();
            this.button_DataParsing = new System.Windows.Forms.Button();
            this.button_Rotate = new System.Windows.Forms.Button();
            this.groupBox167 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.button_Test_OffsetAngle_Calc = new System.Windows.Forms.Button();
            this.tb_ScannerOffset_Angle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_ScannerOffset_Y = new System.Windows.Forms.TextBox();
            this.lbl_ScannerOffset_Y = new System.Windows.Forms.Label();
            this.btnScannerOffset_Set = new System.Windows.Forms.Button();
            this.tb_ScannerOffset_X = new System.Windows.Forms.TextBox();
            this.lbl_ScannerOffset_X = new System.Windows.Forms.Label();
            this.tb_SelectSocketNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox167.SuspendLayout();
            this.SuspendLayout();
            // 
            // SiriusEditor
            // 
            this.SiriusEditor.AliasName = "NoName";
            this.SiriusEditor.AllowDrop = true;
            this.SiriusEditor.BackColor = System.Drawing.SystemColors.Control;
            this.SiriusEditor.Document = null;
            this.SiriusEditor.EnablePens = true;
            this.SiriusEditor.FileName = "NoName";
            this.SiriusEditor.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SiriusEditor.HidePropertyGrid = false;
            this.SiriusEditor.Index = ((uint)(0u));
            this.SiriusEditor.Laser = null;
            this.SiriusEditor.Location = new System.Drawing.Point(5, 5);
            this.SiriusEditor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SiriusEditor.Marker = null;
            this.SiriusEditor.Motors = null;
            this.SiriusEditor.MotorZ = null;
            this.SiriusEditor.Name = "SiriusEditor";
            this.SiriusEditor.PowerMap = null;
            this.SiriusEditor.PowerMeter = null;
            this.SiriusEditor.Progress = 0;
            this.SiriusEditor.Rtc = null;
            this.SiriusEditor.RtcExtension1Input = null;
            this.SiriusEditor.RtcExtension1Output = null;
            this.SiriusEditor.RtcExtension2Output = null;
            this.SiriusEditor.RtcPin2Input = null;
            this.SiriusEditor.RtcPin2Output = null;
            this.SiriusEditor.Size = new System.Drawing.Size(1286, 871);
            this.SiriusEditor.TabIndex = 69;
            // 
            // button_DataParsing
            // 
            this.button_DataParsing.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_DataParsing.Location = new System.Drawing.Point(1336, 209);
            this.button_DataParsing.Name = "button_DataParsing";
            this.button_DataParsing.Size = new System.Drawing.Size(141, 60);
            this.button_DataParsing.TabIndex = 2;
            this.button_DataParsing.Text = "Data Parsing";
            this.button_DataParsing.UseVisualStyleBackColor = true;
            this.button_DataParsing.Click += new System.EventHandler(this.button_DataParsing_Click);
            // 
            // button_Rotate
            // 
            this.button_Rotate.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Rotate.Location = new System.Drawing.Point(1324, 558);
            this.button_Rotate.Name = "button_Rotate";
            this.button_Rotate.Size = new System.Drawing.Size(153, 57);
            this.button_Rotate.TabIndex = 2;
            this.button_Rotate.Text = "Data  Select";
            this.button_Rotate.UseVisualStyleBackColor = true;
            this.button_Rotate.Click += new System.EventHandler(this.button_Rotate_Click);
            // 
            // groupBox167
            // 
            this.groupBox167.Controls.Add(this.label3);
            this.groupBox167.Controls.Add(this.label2);
            this.groupBox167.Controls.Add(this.label1);
            this.groupBox167.Controls.Add(this.label21);
            this.groupBox167.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox167.Location = new System.Drawing.Point(1336, 13);
            this.groupBox167.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox167.Name = "groupBox167";
            this.groupBox167.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox167.Size = new System.Drawing.Size(141, 153);
            this.groupBox167.TabIndex = 84;
            this.groupBox167.TabStop = false;
            this.groupBox167.Text = " Available Layer ";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label3.Location = new System.Drawing.Point(11, 100);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 24);
            this.label3.TabIndex = 10;
            this.label3.Text = "- Outline";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label2.Location = new System.Drawing.Point(11, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "- Rect";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(11, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "- Fiducial";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label21.Location = new System.Drawing.Point(11, 25);
            this.label21.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(109, 24);
            this.label21.TabIndex = 7;
            this.label21.Text = "- Hole1 ~ 4";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_Test_OffsetAngle_Calc
            // 
            this.button_Test_OffsetAngle_Calc.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Test_OffsetAngle_Calc.Location = new System.Drawing.Point(1324, 652);
            this.button_Test_OffsetAngle_Calc.Name = "button_Test_OffsetAngle_Calc";
            this.button_Test_OffsetAngle_Calc.Size = new System.Drawing.Size(153, 57);
            this.button_Test_OffsetAngle_Calc.TabIndex = 85;
            this.button_Test_OffsetAngle_Calc.Text = "Test : Angle, Offset Calc.";
            this.button_Test_OffsetAngle_Calc.UseVisualStyleBackColor = true;
            this.button_Test_OffsetAngle_Calc.Click += new System.EventHandler(this.button_Test_OffsetAngle_Calc_Click);
            // 
            // tb_ScannerOffset_Angle
            // 
            this.tb_ScannerOffset_Angle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_ScannerOffset_Angle.Location = new System.Drawing.Point(1400, 789);
            this.tb_ScannerOffset_Angle.Name = "tb_ScannerOffset_Angle";
            this.tb_ScannerOffset_Angle.Size = new System.Drawing.Size(77, 27);
            this.tb_ScannerOffset_Angle.TabIndex = 94;
            this.tb_ScannerOffset_Angle.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(1321, 795);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 16);
            this.label4.TabIndex = 93;
            this.label4.Text = "Angle Offset";
            // 
            // tb_ScannerOffset_Y
            // 
            this.tb_ScannerOffset_Y.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_ScannerOffset_Y.Location = new System.Drawing.Point(1407, 759);
            this.tb_ScannerOffset_Y.Name = "tb_ScannerOffset_Y";
            this.tb_ScannerOffset_Y.Size = new System.Drawing.Size(70, 27);
            this.tb_ScannerOffset_Y.TabIndex = 92;
            this.tb_ScannerOffset_Y.Text = "0";
            // 
            // lbl_ScannerOffset_Y
            // 
            this.lbl_ScannerOffset_Y.AutoSize = true;
            this.lbl_ScannerOffset_Y.ForeColor = System.Drawing.Color.Black;
            this.lbl_ScannerOffset_Y.Location = new System.Drawing.Point(1322, 764);
            this.lbl_ScannerOffset_Y.Name = "lbl_ScannerOffset_Y";
            this.lbl_ScannerOffset_Y.Size = new System.Drawing.Size(81, 16);
            this.lbl_ScannerOffset_Y.TabIndex = 91;
            this.lbl_ScannerOffset_Y.Text = "Y Offset (㎜)";
            // 
            // btnScannerOffset_Set
            // 
            this.btnScannerOffset_Set.BackColor = System.Drawing.Color.White;
            this.btnScannerOffset_Set.FlatAppearance.BorderColor = System.Drawing.Color.DarkRed;
            this.btnScannerOffset_Set.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnScannerOffset_Set.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnScannerOffset_Set.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScannerOffset_Set.ForeColor = System.Drawing.Color.Black;
            this.btnScannerOffset_Set.Location = new System.Drawing.Point(1324, 825);
            this.btnScannerOffset_Set.Name = "btnScannerOffset_Set";
            this.btnScannerOffset_Set.Size = new System.Drawing.Size(153, 44);
            this.btnScannerOffset_Set.TabIndex = 90;
            this.btnScannerOffset_Set.Text = "Set Scanner Offset";
            this.btnScannerOffset_Set.UseVisualStyleBackColor = false;
            this.btnScannerOffset_Set.Click += new System.EventHandler(this.btnScannerOffset_Set_Click);
            // 
            // tb_ScannerOffset_X
            // 
            this.tb_ScannerOffset_X.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_ScannerOffset_X.Location = new System.Drawing.Point(1407, 729);
            this.tb_ScannerOffset_X.Name = "tb_ScannerOffset_X";
            this.tb_ScannerOffset_X.Size = new System.Drawing.Size(70, 27);
            this.tb_ScannerOffset_X.TabIndex = 89;
            this.tb_ScannerOffset_X.Text = "0";
            // 
            // lbl_ScannerOffset_X
            // 
            this.lbl_ScannerOffset_X.AutoSize = true;
            this.lbl_ScannerOffset_X.ForeColor = System.Drawing.Color.Black;
            this.lbl_ScannerOffset_X.Location = new System.Drawing.Point(1322, 734);
            this.lbl_ScannerOffset_X.Name = "lbl_ScannerOffset_X";
            this.lbl_ScannerOffset_X.Size = new System.Drawing.Size(82, 16);
            this.lbl_ScannerOffset_X.TabIndex = 88;
            this.lbl_ScannerOffset_X.Text = "X Offset (㎜)";
            // 
            // tb_SelectSocketNumber
            // 
            this.tb_SelectSocketNumber.Font = new System.Drawing.Font("Tahoma", 10F);
            this.tb_SelectSocketNumber.Location = new System.Drawing.Point(1446, 530);
            this.tb_SelectSocketNumber.Name = "tb_SelectSocketNumber";
            this.tb_SelectSocketNumber.Size = new System.Drawing.Size(31, 24);
            this.tb_SelectSocketNumber.TabIndex = 96;
            this.tb_SelectSocketNumber.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(1323, 534);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 16);
            this.label5.TabIndex = 95;
            this.label5.Text = "Select Socket Num.";
            // 
            // FormNew_SiriusEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 881);
            this.Controls.Add(this.tb_SelectSocketNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_ScannerOffset_Angle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_ScannerOffset_Y);
            this.Controls.Add(this.lbl_ScannerOffset_Y);
            this.Controls.Add(this.btnScannerOffset_Set);
            this.Controls.Add(this.tb_ScannerOffset_X);
            this.Controls.Add(this.lbl_ScannerOffset_X);
            this.Controls.Add(this.button_Test_OffsetAngle_Calc);
            this.Controls.Add(this.groupBox167);
            this.Controls.Add(this.button_Rotate);
            this.Controls.Add(this.button_DataParsing);
            this.Controls.Add(this.SiriusEditor);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNew_SiriusEditor";
            this.Text = "QMC Sirius Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormNew_SiriusEditor_FormClosing);
            this.Shown += new System.EventHandler(this.FormNew_CommunicationTerminal_Shown);
            this.groupBox167.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private SpiralLab.Sirius2.Winforms.UI.SiriusEditorUserControl siriusEditor;
        public SpiralLab.Sirius.SiriusEditorForm SiriusEditor;
        private Button button_DataParsing;
        private Button button_Rotate;
        private GroupBox groupBox167;
        private Label label21;
        private Label label1;
        private Label label3;
        private Label label2;
        private Button button_Test_OffsetAngle_Calc;
        private TextBox tb_ScannerOffset_Angle;
        private Label label4;
        private TextBox tb_ScannerOffset_Y;
        private Label lbl_ScannerOffset_Y;
        private Button btnScannerOffset_Set;
        private TextBox tb_ScannerOffset_X;
        private Label lbl_ScannerOffset_X;
        private TextBox tb_SelectSocketNumber;
        private Label label5;
    }
}