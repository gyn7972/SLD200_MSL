namespace CWA150SA_Onsemi300
{
    partial class FormWaferTipContactPosition
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_Close = new System.Windows.Forms.Button();
            this.pictureBox_TOP_PAK = new System.Windows.Forms.PictureBox();
            this.pictureBox_TOP_Wafer = new System.Windows.Forms.PictureBox();
            this.pictureBox_MID_PAK = new System.Windows.Forms.PictureBox();
            this.pictureBox_MID_Wafer = new System.Windows.Forms.PictureBox();
            this.pictureBox_BOT_PAK = new System.Windows.Forms.PictureBox();
            this.pictureBox_BOT_Wafer = new System.Windows.Forms.PictureBox();
            this.pictureBox_Empty_Wafer = new System.Windows.Forms.PictureBox();
            this.baseLabel_Wafer = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_PAK = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_Empty = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_BOT = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_MID = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_TOP = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_BOT2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_MID2 = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabel_TOP2 = new CWA150SA_Onsemi300.BaseLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TOP_PAK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TOP_Wafer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MID_PAK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MID_Wafer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_BOT_PAK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_BOT_Wafer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Empty_Wafer)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.LightGray;
            this.btn_Close.FlatAppearance.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.btn_Close.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btn_Close.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btn_Close.Font = new System.Drawing.Font("나눔바른고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Close.ForeColor = System.Drawing.Color.DarkRed;
            this.btn_Close.Location = new System.Drawing.Point(1572, 719);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(270, 130);
            this.btn_Close.TabIndex = 126;
            this.btn_Close.Text = "[ Tip Contact 위치 확인 ]\r\n\r\n창 닫기";
            this.btn_Close.UseVisualStyleBackColor = false;
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // pictureBox_TOP_PAK
            // 
            this.pictureBox_TOP_PAK.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox_TOP_PAK.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_TOP_PAK.Location = new System.Drawing.Point(9, 52);
            this.pictureBox_TOP_PAK.Name = "pictureBox_TOP_PAK";
            this.pictureBox_TOP_PAK.Size = new System.Drawing.Size(440, 368);
            this.pictureBox_TOP_PAK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_TOP_PAK.TabIndex = 127;
            this.pictureBox_TOP_PAK.TabStop = false;
            // 
            // pictureBox_TOP_Wafer
            // 
            this.pictureBox_TOP_Wafer.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox_TOP_Wafer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_TOP_Wafer.Location = new System.Drawing.Point(9, 481);
            this.pictureBox_TOP_Wafer.Name = "pictureBox_TOP_Wafer";
            this.pictureBox_TOP_Wafer.Size = new System.Drawing.Size(440, 368);
            this.pictureBox_TOP_Wafer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_TOP_Wafer.TabIndex = 128;
            this.pictureBox_TOP_Wafer.TabStop = false;
            // 
            // pictureBox_MID_PAK
            // 
            this.pictureBox_MID_PAK.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox_MID_PAK.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_MID_PAK.Location = new System.Drawing.Point(455, 52);
            this.pictureBox_MID_PAK.Name = "pictureBox_MID_PAK";
            this.pictureBox_MID_PAK.Size = new System.Drawing.Size(440, 368);
            this.pictureBox_MID_PAK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_MID_PAK.TabIndex = 129;
            this.pictureBox_MID_PAK.TabStop = false;
            // 
            // pictureBox_MID_Wafer
            // 
            this.pictureBox_MID_Wafer.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox_MID_Wafer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_MID_Wafer.Location = new System.Drawing.Point(455, 481);
            this.pictureBox_MID_Wafer.Name = "pictureBox_MID_Wafer";
            this.pictureBox_MID_Wafer.Size = new System.Drawing.Size(440, 368);
            this.pictureBox_MID_Wafer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_MID_Wafer.TabIndex = 130;
            this.pictureBox_MID_Wafer.TabStop = false;
            // 
            // pictureBox_BOT_PAK
            // 
            this.pictureBox_BOT_PAK.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox_BOT_PAK.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_BOT_PAK.Location = new System.Drawing.Point(901, 52);
            this.pictureBox_BOT_PAK.Name = "pictureBox_BOT_PAK";
            this.pictureBox_BOT_PAK.Size = new System.Drawing.Size(440, 368);
            this.pictureBox_BOT_PAK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_BOT_PAK.TabIndex = 131;
            this.pictureBox_BOT_PAK.TabStop = false;
            // 
            // pictureBox_BOT_Wafer
            // 
            this.pictureBox_BOT_Wafer.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox_BOT_Wafer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_BOT_Wafer.Location = new System.Drawing.Point(901, 481);
            this.pictureBox_BOT_Wafer.Name = "pictureBox_BOT_Wafer";
            this.pictureBox_BOT_Wafer.Size = new System.Drawing.Size(440, 368);
            this.pictureBox_BOT_Wafer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_BOT_Wafer.TabIndex = 132;
            this.pictureBox_BOT_Wafer.TabStop = false;
            // 
            // pictureBox_Empty_Wafer
            // 
            this.pictureBox_Empty_Wafer.BackColor = System.Drawing.Color.DimGray;
            this.pictureBox_Empty_Wafer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox_Empty_Wafer.Location = new System.Drawing.Point(1402, 52);
            this.pictureBox_Empty_Wafer.Name = "pictureBox_Empty_Wafer";
            this.pictureBox_Empty_Wafer.Size = new System.Drawing.Size(440, 368);
            this.pictureBox_Empty_Wafer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Empty_Wafer.TabIndex = 133;
            this.pictureBox_Empty_Wafer.TabStop = false;
            // 
            // baseLabel_Wafer
            // 
            this.baseLabel_Wafer.BackColor = System.Drawing.Color.Yellow;
            this.baseLabel_Wafer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Wafer.Font = new System.Drawing.Font("나눔바른고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Wafer.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_Wafer.Location = new System.Drawing.Point(9, 437);
            this.baseLabel_Wafer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Wafer.Name = "baseLabel_Wafer";
            this.baseLabel_Wafer.Size = new System.Drawing.Size(120, 41);
            this.baseLabel_Wafer.TabIndex = 135;
            this.baseLabel_Wafer.Text = "Wafer";
            this.baseLabel_Wafer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_PAK
            // 
            this.baseLabel_PAK.BackColor = System.Drawing.Color.Yellow;
            this.baseLabel_PAK.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_PAK.Font = new System.Drawing.Font("나눔바른고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_PAK.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_PAK.Location = new System.Drawing.Point(9, 8);
            this.baseLabel_PAK.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_PAK.Name = "baseLabel_PAK";
            this.baseLabel_PAK.Size = new System.Drawing.Size(120, 41);
            this.baseLabel_PAK.TabIndex = 134;
            this.baseLabel_PAK.Text = "PAK";
            this.baseLabel_PAK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_Empty
            // 
            this.baseLabel_Empty.BackColor = System.Drawing.Color.Black;
            this.baseLabel_Empty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_Empty.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_Empty.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_Empty.Location = new System.Drawing.Point(1558, 8);
            this.baseLabel_Empty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_Empty.Name = "baseLabel_Empty";
            this.baseLabel_Empty.Size = new System.Drawing.Size(129, 41);
            this.baseLabel_Empty.TabIndex = 124;
            this.baseLabel_Empty.Text = "[ Empty Chip ]";
            this.baseLabel_Empty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_BOT
            // 
            this.baseLabel_BOT.BackColor = System.Drawing.Color.Black;
            this.baseLabel_BOT.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_BOT.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_BOT.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_BOT.Location = new System.Drawing.Point(1061, 8);
            this.baseLabel_BOT.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_BOT.Name = "baseLabel_BOT";
            this.baseLabel_BOT.Size = new System.Drawing.Size(120, 41);
            this.baseLabel_BOT.TabIndex = 123;
            this.baseLabel_BOT.Text = "[  BOTTOM  ]";
            this.baseLabel_BOT.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_MID
            // 
            this.baseLabel_MID.BackColor = System.Drawing.Color.Black;
            this.baseLabel_MID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_MID.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_MID.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_MID.Location = new System.Drawing.Point(615, 8);
            this.baseLabel_MID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MID.Name = "baseLabel_MID";
            this.baseLabel_MID.Size = new System.Drawing.Size(120, 41);
            this.baseLabel_MID.TabIndex = 122;
            this.baseLabel_MID.Text = "[  MIDDLE  ]";
            this.baseLabel_MID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_TOP
            // 
            this.baseLabel_TOP.BackColor = System.Drawing.Color.Black;
            this.baseLabel_TOP.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_TOP.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_TOP.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_TOP.Location = new System.Drawing.Point(169, 8);
            this.baseLabel_TOP.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_TOP.Name = "baseLabel_TOP";
            this.baseLabel_TOP.Size = new System.Drawing.Size(120, 41);
            this.baseLabel_TOP.TabIndex = 116;
            this.baseLabel_TOP.Text = "[  TOP  ]";
            this.baseLabel_TOP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_BOT2
            // 
            this.baseLabel_BOT2.BackColor = System.Drawing.Color.Black;
            this.baseLabel_BOT2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_BOT2.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_BOT2.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_BOT2.Location = new System.Drawing.Point(1061, 437);
            this.baseLabel_BOT2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_BOT2.Name = "baseLabel_BOT2";
            this.baseLabel_BOT2.Size = new System.Drawing.Size(120, 41);
            this.baseLabel_BOT2.TabIndex = 138;
            this.baseLabel_BOT2.Text = "[  BOTTOM  ]";
            this.baseLabel_BOT2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_MID2
            // 
            this.baseLabel_MID2.BackColor = System.Drawing.Color.Black;
            this.baseLabel_MID2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_MID2.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_MID2.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_MID2.Location = new System.Drawing.Point(615, 437);
            this.baseLabel_MID2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_MID2.Name = "baseLabel_MID2";
            this.baseLabel_MID2.Size = new System.Drawing.Size(120, 41);
            this.baseLabel_MID2.TabIndex = 137;
            this.baseLabel_MID2.Text = "[  MIDDLE  ]";
            this.baseLabel_MID2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // baseLabel_TOP2
            // 
            this.baseLabel_TOP2.BackColor = System.Drawing.Color.Black;
            this.baseLabel_TOP2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.baseLabel_TOP2.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabel_TOP2.ForeColor = System.Drawing.Color.Yellow;
            this.baseLabel_TOP2.Location = new System.Drawing.Point(169, 437);
            this.baseLabel_TOP2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.baseLabel_TOP2.Name = "baseLabel_TOP2";
            this.baseLabel_TOP2.Size = new System.Drawing.Size(120, 41);
            this.baseLabel_TOP2.TabIndex = 136;
            this.baseLabel_TOP2.Text = "[  TOP  ]";
            this.baseLabel_TOP2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormWaferTipContactPosition
            // 
            this.BackColor = System.Drawing.Color.DarkSlateGray;
            this.ClientSize = new System.Drawing.Size(1851, 858);
            this.ControlBox = false;
            this.Controls.Add(this.baseLabel_BOT2);
            this.Controls.Add(this.baseLabel_MID2);
            this.Controls.Add(this.baseLabel_TOP2);
            this.Controls.Add(this.baseLabel_Wafer);
            this.Controls.Add(this.baseLabel_PAK);
            this.Controls.Add(this.pictureBox_Empty_Wafer);
            this.Controls.Add(this.pictureBox_BOT_Wafer);
            this.Controls.Add(this.pictureBox_BOT_PAK);
            this.Controls.Add(this.pictureBox_MID_Wafer);
            this.Controls.Add(this.pictureBox_MID_PAK);
            this.Controls.Add(this.pictureBox_TOP_Wafer);
            this.Controls.Add(this.pictureBox_TOP_PAK);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.baseLabel_Empty);
            this.Controls.Add(this.baseLabel_BOT);
            this.Controls.Add(this.baseLabel_MID);
            this.Controls.Add(this.baseLabel_TOP);
            this.ForeColor = System.Drawing.Color.Coral;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWaferTipContactPosition";
            this.ShowIcon = false;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TOP_PAK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TOP_Wafer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MID_PAK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_MID_Wafer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_BOT_PAK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_BOT_Wafer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Empty_Wafer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseLabel baseLabel_TOP;
        private BaseLabel baseLabel_MID;
        private BaseLabel baseLabel_BOT;
        private BaseLabel baseLabel_Empty;
        private System.Windows.Forms.Button btn_Close;
        private System.Windows.Forms.PictureBox pictureBox_TOP_PAK;
        private System.Windows.Forms.PictureBox pictureBox_TOP_Wafer;
        private System.Windows.Forms.PictureBox pictureBox_MID_PAK;
        private System.Windows.Forms.PictureBox pictureBox_MID_Wafer;
        private System.Windows.Forms.PictureBox pictureBox_BOT_PAK;
        private System.Windows.Forms.PictureBox pictureBox_BOT_Wafer;
        private System.Windows.Forms.PictureBox pictureBox_Empty_Wafer;
        private BaseLabel baseLabel_PAK;
        private BaseLabel baseLabel_Wafer;
        private BaseLabel baseLabel_BOT2;
        private BaseLabel baseLabel_MID2;
        private BaseLabel baseLabel_TOP2;
    }
}

