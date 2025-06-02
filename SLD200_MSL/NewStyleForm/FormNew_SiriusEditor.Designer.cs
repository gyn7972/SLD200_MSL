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
            this.SiriusEditor = new SpiralLab.Sirius.QMCSiriusEditorForm();
            this.button_DataParsing = new System.Windows.Forms.Button();
            this.button_Rotate = new System.Windows.Forms.Button();
            this.groupBox167 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
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
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxCorY = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textboxCorX = new System.Windows.Forms.TextBox();
            this.textBox_SiriusEditor_Divided_W = new System.Windows.Forms.TextBox();
            this.label_SiriusEditor_Divided_W = new System.Windows.Forms.Label();
            this.textBox_SiriusEditor_Divided_H = new System.Windows.Forms.TextBox();
            this.label_SiriusEditor_Divided_H = new System.Windows.Forms.Label();
            this.button_SiriusEditor_Divided = new System.Windows.Forms.Button();
            this.checkBox_SiriusEditor_Divided = new System.Windows.Forms.CheckBox();
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
            this.SiriusEditor.OnDocumentSave += new SpiralLab.Sirius.SiriusDocumentSave(this.SiriusEditor_OnDocumentSave);
            this.SiriusEditor.CausesValidationChanged += new System.EventHandler(this.SiriusEditor_CausesValidationChanged);
            // 
            // button_DataParsing
            // 
            this.button_DataParsing.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_DataParsing.Location = new System.Drawing.Point(1300, 234);
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
            this.button_Rotate.Location = new System.Drawing.Point(1324, 521);
            this.button_Rotate.Name = "button_Rotate";
            this.button_Rotate.Size = new System.Drawing.Size(153, 57);
            this.button_Rotate.TabIndex = 2;
            this.button_Rotate.Text = "Data  Select";
            this.button_Rotate.UseVisualStyleBackColor = true;
            this.button_Rotate.Click += new System.EventHandler(this.button_Rotate_Click);
            // 
            // groupBox167
            // 
            this.groupBox167.Controls.Add(this.label9);
            this.groupBox167.Controls.Add(this.label8);
            this.groupBox167.Controls.Add(this.label3);
            this.groupBox167.Controls.Add(this.label2);
            this.groupBox167.Controls.Add(this.label1);
            this.groupBox167.Controls.Add(this.label21);
            this.groupBox167.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox167.Location = new System.Drawing.Point(1300, 13);
            this.groupBox167.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.groupBox167.Name = "groupBox167";
            this.groupBox167.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox167.Size = new System.Drawing.Size(141, 214);
            this.groupBox167.TabIndex = 84;
            this.groupBox167.TabStop = false;
            this.groupBox167.Text = " Available Layer ";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(11, 185);
            this.label9.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(109, 24);
            this.label9.TabIndex = 12;
            this.label9.Text = "- PreAlign";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(11, 57);
            this.label8.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(109, 24);
            this.label8.TabIndex = 11;
            this.label8.Text = "- Thruhole";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(11, 89);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 24);
            this.label3.TabIndex = 10;
            this.label3.Text = "- Outline";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 121);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "- Marking";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 153);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "- Fiducial";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(11, 25);
            this.label21.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(109, 24);
            this.label21.TabIndex = 7;
            this.label21.Text = "- Hole1 ~ 50";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_Test_OffsetAngle_Calc
            // 
            this.button_Test_OffsetAngle_Calc.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_Test_OffsetAngle_Calc.Location = new System.Drawing.Point(1324, 583);
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
            this.tb_SelectSocketNumber.Location = new System.Drawing.Point(1446, 493);
            this.tb_SelectSocketNumber.Name = "tb_SelectSocketNumber";
            this.tb_SelectSocketNumber.Size = new System.Drawing.Size(31, 24);
            this.tb_SelectSocketNumber.TabIndex = 96;
            this.tb_SelectSocketNumber.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(1323, 497);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 16);
            this.label5.TabIndex = 95;
            this.label5.Text = "Select Socket Num.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(1322, 701);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 16);
            this.label6.TabIndex = 88;
            this.label6.Text = "Y Cor (㎜)";
            // 
            // textBoxCorY
            // 
            this.textBoxCorY.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCorY.Location = new System.Drawing.Point(1407, 696);
            this.textBoxCorY.Name = "textBoxCorY";
            this.textBoxCorY.Size = new System.Drawing.Size(70, 27);
            this.textBoxCorY.TabIndex = 89;
            this.textBoxCorY.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(1322, 655);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(68, 16);
            this.label7.TabIndex = 88;
            this.label7.Text = "X Cor (㎜)";
            // 
            // textboxCorX
            // 
            this.textboxCorX.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textboxCorX.Location = new System.Drawing.Point(1407, 650);
            this.textboxCorX.Name = "textboxCorX";
            this.textboxCorX.Size = new System.Drawing.Size(70, 27);
            this.textboxCorX.TabIndex = 89;
            this.textboxCorX.Text = "0";
            // 
            // textBox_SiriusEditor_Divided_W
            // 
            this.textBox_SiriusEditor_Divided_W.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_SiriusEditor_Divided_W.Location = new System.Drawing.Point(1413, 362);
            this.textBox_SiriusEditor_Divided_W.Name = "textBox_SiriusEditor_Divided_W";
            this.textBox_SiriusEditor_Divided_W.Size = new System.Drawing.Size(41, 23);
            this.textBox_SiriusEditor_Divided_W.TabIndex = 98;
            this.textBox_SiriusEditor_Divided_W.Text = "0";
            this.textBox_SiriusEditor_Divided_W.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_SiriusEditor_Divided_W
            // 
            this.label_SiriusEditor_Divided_W.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_SiriusEditor_Divided_W.ForeColor = System.Drawing.Color.Black;
            this.label_SiriusEditor_Divided_W.Location = new System.Drawing.Point(1300, 362);
            this.label_SiriusEditor_Divided_W.Name = "label_SiriusEditor_Divided_W";
            this.label_SiriusEditor_Divided_W.Size = new System.Drawing.Size(110, 23);
            this.label_SiriusEditor_Divided_W.TabIndex = 97;
            this.label_SiriusEditor_Divided_W.Text = "Divided_W (㎜)";
            this.label_SiriusEditor_Divided_W.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBox_SiriusEditor_Divided_H
            // 
            this.textBox_SiriusEditor_Divided_H.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_SiriusEditor_Divided_H.Location = new System.Drawing.Point(1413, 391);
            this.textBox_SiriusEditor_Divided_H.Name = "textBox_SiriusEditor_Divided_H";
            this.textBox_SiriusEditor_Divided_H.Size = new System.Drawing.Size(41, 23);
            this.textBox_SiriusEditor_Divided_H.TabIndex = 100;
            this.textBox_SiriusEditor_Divided_H.Text = "0";
            this.textBox_SiriusEditor_Divided_H.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label_SiriusEditor_Divided_H
            // 
            this.label_SiriusEditor_Divided_H.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_SiriusEditor_Divided_H.ForeColor = System.Drawing.Color.Black;
            this.label_SiriusEditor_Divided_H.Location = new System.Drawing.Point(1300, 391);
            this.label_SiriusEditor_Divided_H.Name = "label_SiriusEditor_Divided_H";
            this.label_SiriusEditor_Divided_H.Size = new System.Drawing.Size(110, 23);
            this.label_SiriusEditor_Divided_H.TabIndex = 99;
            this.label_SiriusEditor_Divided_H.Text = "Divided_H (㎜)";
            this.label_SiriusEditor_Divided_H.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button_SiriusEditor_Divided
            // 
            this.button_SiriusEditor_Divided.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.button_SiriusEditor_Divided.Location = new System.Drawing.Point(1300, 417);
            this.button_SiriusEditor_Divided.Name = "button_SiriusEditor_Divided";
            this.button_SiriusEditor_Divided.Size = new System.Drawing.Size(154, 34);
            this.button_SiriusEditor_Divided.TabIndex = 101;
            this.button_SiriusEditor_Divided.Text = "Divided적용";
            this.button_SiriusEditor_Divided.UseVisualStyleBackColor = true;
            this.button_SiriusEditor_Divided.Click += new System.EventHandler(this.button_SiriusEditor_Divided_Click);
            // 
            // checkBox_SiriusEditor_Divided
            // 
            this.checkBox_SiriusEditor_Divided.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_SiriusEditor_Divided.Location = new System.Drawing.Point(1300, 335);
            this.checkBox_SiriusEditor_Divided.Name = "checkBox_SiriusEditor_Divided";
            this.checkBox_SiriusEditor_Divided.Size = new System.Drawing.Size(110, 23);
            this.checkBox_SiriusEditor_Divided.TabIndex = 102;
            this.checkBox_SiriusEditor_Divided.Text = "Divided Use";
            this.checkBox_SiriusEditor_Divided.UseVisualStyleBackColor = true;
            // 
            // FormNew_SiriusEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1489, 881);
            this.Controls.Add(this.checkBox_SiriusEditor_Divided);
            this.Controls.Add(this.button_SiriusEditor_Divided);
            this.Controls.Add(this.textBox_SiriusEditor_Divided_H);
            this.Controls.Add(this.label_SiriusEditor_Divided_H);
            this.Controls.Add(this.textBox_SiriusEditor_Divided_W);
            this.Controls.Add(this.label_SiriusEditor_Divided_W);
            this.Controls.Add(this.tb_SelectSocketNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tb_ScannerOffset_Angle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_ScannerOffset_Y);
            this.Controls.Add(this.lbl_ScannerOffset_Y);
            this.Controls.Add(this.btnScannerOffset_Set);
            this.Controls.Add(this.textboxCorX);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxCorY);
            this.Controls.Add(this.label6);
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
        public SpiralLab.Sirius.QMCSiriusEditorForm SiriusEditor;
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
        private Label label6;
        private TextBox textBoxCorY;
        private Label label7;
        private TextBox textboxCorX;
        private Label label8;
        private Label label9;
        private TextBox textBox_SiriusEditor_Divided_W;
        private Label label_SiriusEditor_Divided_W;
        private TextBox textBox_SiriusEditor_Divided_H;
        private Label label_SiriusEditor_Divided_H;
        private Button button_SiriusEditor_Divided;
        private CheckBox checkBox_SiriusEditor_Divided;
    }
}