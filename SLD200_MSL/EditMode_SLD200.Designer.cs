namespace SLD200_MSL
{
    partial class EditMode_SLD200
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnTest_RTC6 = new System.Windows.Forms.Button();
            this.btnScannerOffset_Set = new System.Windows.Forms.Button();
            this.tb_ScannerOffset_X = new System.Windows.Forms.TextBox();
            this.lbl_ScannerOffset_X = new System.Windows.Forms.Label();
            this.lbl_ScannerOffset_Y = new System.Windows.Forms.Label();
            this.tb_ScannerOffset_Y = new System.Windows.Forms.TextBox();
            this.tb_ScannerOffset_Angle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SiriusEditor = new SpiralLab.Sirius.SiriusEditorForm();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // btnTest_RTC6
            // 
            this.btnTest_RTC6.BackColor = System.Drawing.Color.DarkGray;
            this.btnTest_RTC6.FlatAppearance.BorderColor = System.Drawing.Color.DarkRed;
            this.btnTest_RTC6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnTest_RTC6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnTest_RTC6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest_RTC6.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTest_RTC6.ForeColor = System.Drawing.Color.DarkRed;
            this.btnTest_RTC6.Location = new System.Drawing.Point(1598, 16);
            this.btnTest_RTC6.Name = "btnTest_RTC6";
            this.btnTest_RTC6.Size = new System.Drawing.Size(130, 66);
            this.btnTest_RTC6.TabIndex = 60;
            this.btnTest_RTC6.Text = "RTC6";
            this.btnTest_RTC6.UseVisualStyleBackColor = false;
            this.btnTest_RTC6.Click += new System.EventHandler(this.btnTest_RTC6_Click);
            // 
            // btnScannerOffset_Set
            // 
            this.btnScannerOffset_Set.BackColor = System.Drawing.Color.DarkGray;
            this.btnScannerOffset_Set.FlatAppearance.BorderColor = System.Drawing.Color.DarkRed;
            this.btnScannerOffset_Set.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnScannerOffset_Set.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnScannerOffset_Set.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScannerOffset_Set.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnScannerOffset_Set.ForeColor = System.Drawing.Color.DarkRed;
            this.btnScannerOffset_Set.Location = new System.Drawing.Point(1737, 116);
            this.btnScannerOffset_Set.Name = "btnScannerOffset_Set";
            this.btnScannerOffset_Set.Size = new System.Drawing.Size(130, 36);
            this.btnScannerOffset_Set.TabIndex = 66;
            this.btnScannerOffset_Set.Text = "Set Scanner Offset\r\n(RTC6 Mode Only)";
            this.btnScannerOffset_Set.UseVisualStyleBackColor = false;
            this.btnScannerOffset_Set.Click += new System.EventHandler(this.btnScannerOffset_Set_Click);
            // 
            // tb_ScannerOffset_X
            // 
            this.tb_ScannerOffset_X.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_ScannerOffset_X.Location = new System.Drawing.Point(1659, 88);
            this.tb_ScannerOffset_X.Name = "tb_ScannerOffset_X";
            this.tb_ScannerOffset_X.Size = new System.Drawing.Size(64, 26);
            this.tb_ScannerOffset_X.TabIndex = 65;
            this.tb_ScannerOffset_X.Text = "0";
            // 
            // lbl_ScannerOffset_X
            // 
            this.lbl_ScannerOffset_X.AutoSize = true;
            this.lbl_ScannerOffset_X.ForeColor = System.Drawing.Color.White;
            this.lbl_ScannerOffset_X.Location = new System.Drawing.Point(1580, 95);
            this.lbl_ScannerOffset_X.Name = "lbl_ScannerOffset_X";
            this.lbl_ScannerOffset_X.Size = new System.Drawing.Size(75, 12);
            this.lbl_ScannerOffset_X.TabIndex = 64;
            this.lbl_ScannerOffset_X.Text = "X Offset (㎜)";
            // 
            // lbl_ScannerOffset_Y
            // 
            this.lbl_ScannerOffset_Y.AutoSize = true;
            this.lbl_ScannerOffset_Y.ForeColor = System.Drawing.Color.White;
            this.lbl_ScannerOffset_Y.Location = new System.Drawing.Point(1580, 122);
            this.lbl_ScannerOffset_Y.Name = "lbl_ScannerOffset_Y";
            this.lbl_ScannerOffset_Y.Size = new System.Drawing.Size(75, 12);
            this.lbl_ScannerOffset_Y.TabIndex = 67;
            this.lbl_ScannerOffset_Y.Text = "Y Offset (㎜)";
            // 
            // tb_ScannerOffset_Y
            // 
            this.tb_ScannerOffset_Y.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_ScannerOffset_Y.Location = new System.Drawing.Point(1659, 115);
            this.tb_ScannerOffset_Y.Name = "tb_ScannerOffset_Y";
            this.tb_ScannerOffset_Y.Size = new System.Drawing.Size(64, 26);
            this.tb_ScannerOffset_Y.TabIndex = 68;
            this.tb_ScannerOffset_Y.Text = "0";
            // 
            // tb_ScannerOffset_Angle
            // 
            this.tb_ScannerOffset_Angle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tb_ScannerOffset_Angle.Location = new System.Drawing.Point(1814, 88);
            this.tb_ScannerOffset_Angle.Name = "tb_ScannerOffset_Angle";
            this.tb_ScannerOffset_Angle.Size = new System.Drawing.Size(82, 26);
            this.tb_ScannerOffset_Angle.TabIndex = 87;
            this.tb_ScannerOffset_Angle.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(1735, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 12);
            this.label1.TabIndex = 86;
            this.label1.Text = "Angle Offset";
            // 
            // SiriusEditor
            // 
            this.SiriusEditor.AliasName = "NoName";
            this.SiriusEditor.AllowDrop = true;
            this.SiriusEditor.BackColor = System.Drawing.SystemColors.Control;
            this.SiriusEditor.Document = null;
            this.SiriusEditor.EnablePens = true;
            this.SiriusEditor.FileName = "NoName";
            this.SiriusEditor.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SiriusEditor.HidePropertyGrid = false;
            this.SiriusEditor.Index = ((uint)(0u));
            this.SiriusEditor.Laser = null;
            this.SiriusEditor.Location = new System.Drawing.Point(3, 3);
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
            this.SiriusEditor.Size = new System.Drawing.Size(1506, 764);
            this.SiriusEditor.TabIndex = 88;
            // 
            // EditMode_SLD200
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.Controls.Add(this.SiriusEditor);
            this.Controls.Add(this.tb_ScannerOffset_Angle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_ScannerOffset_Y);
            this.Controls.Add(this.lbl_ScannerOffset_Y);
            this.Controls.Add(this.btnScannerOffset_Set);
            this.Controls.Add(this.tb_ScannerOffset_X);
            this.Controls.Add(this.lbl_ScannerOffset_X);
            this.Controls.Add(this.btnTest_RTC6);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "EditMode_SLD200";
            this.Size = new System.Drawing.Size(1910, 770);
            this.Load += new System.EventHandler(this.EditMode_SLD200_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private BaseLabel lblJobFile_List;
        private System.Windows.Forms.Button btnTest_RTC6;
        private BaseLabel lblScannerMode_Select;
        private System.Windows.Forms.Button btnScannerOffset_Set;
        private System.Windows.Forms.TextBox tb_ScannerOffset_X;
        private System.Windows.Forms.Label lbl_ScannerOffset_X;
        private System.Windows.Forms.Label lbl_ScannerOffset_Y;
        private System.Windows.Forms.TextBox tb_ScannerOffset_Y;
        private BaseLabel baseLabel_RepRate;
        private System.Windows.Forms.TextBox tb_ScannerOffset_Angle;
        private System.Windows.Forms.Label label1;
        private SpiralLab.Sirius.SiriusEditorForm SiriusEditor;
    }
}
