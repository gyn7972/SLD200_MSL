namespace QMC.Common.UI
{
    partial class TrainImageControl
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
            this.groupBoxTrainImage = new System.Windows.Forms.GroupBox();
            this.pictureBoxTrainImage = new TrainPictureBox();
            this.baseToggleButtonAvg = new BaseToggleButton();
            this.baseButtonTrain = new BaseButton();
            this.groupBoxTrainImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTrainImage
            // 
            this.groupBoxTrainImage.Controls.Add(this.pictureBoxTrainImage);
            this.groupBoxTrainImage.Controls.Add(this.baseToggleButtonAvg);
            this.groupBoxTrainImage.Controls.Add(this.baseButtonTrain);
            this.groupBoxTrainImage.ForeColor = System.Drawing.Color.White;
            this.groupBoxTrainImage.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTrainImage.Name = "groupBoxTrainImage";
            this.groupBoxTrainImage.Size = new System.Drawing.Size(300, 177);
            this.groupBoxTrainImage.TabIndex = 1;
            this.groupBoxTrainImage.TabStop = false;
            this.groupBoxTrainImage.Text = "Train Image";
            // 
            // pictureBoxTrainImage
            // 
            this.pictureBoxTrainImage.Location = new System.Drawing.Point(4, 14);
            this.pictureBoxTrainImage.Name = "pictureBoxTrainImage";
            this.pictureBoxTrainImage.Size = new System.Drawing.Size(184, 157);
            this.pictureBoxTrainImage.TabIndex = 3;
            // 
            // baseToggleButtonAvg
            // 
            this.baseToggleButtonAvg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonAvg.FlatAppearance.BorderSize = 0;
            this.baseToggleButtonAvg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonAvg.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonAvg.Location = new System.Drawing.Point(194, 105);
            this.baseToggleButtonAvg.Name = "baseToggleButtonAvg";
            this.baseToggleButtonAvg.Size = new System.Drawing.Size(100, 30);
            this.baseToggleButtonAvg.TabIndex = 2;
            this.baseToggleButtonAvg.Text = "Avg";
            this.baseToggleButtonAvg.UseVisualStyleBackColor = false;
            this.baseToggleButtonAvg.Click += new System.EventHandler(this.baseToggleButtonAvg_Click);
            // 
            // baseButtonTrain
            // 
            this.baseButtonTrain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonTrain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonTrain.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonTrain.Location = new System.Drawing.Point(194, 141);
            this.baseButtonTrain.Name = "baseButtonTrain";
            this.baseButtonTrain.Size = new System.Drawing.Size(100, 30);
            this.baseButtonTrain.TabIndex = 0;
            this.baseButtonTrain.Text = "Train";
            this.baseButtonTrain.UseVisualStyleBackColor = false;
            this.baseButtonTrain.Click += new System.EventHandler(this.baseButtonTrain_Click);
            // 
            // TrainImageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTrainImage);
            this.Name = "TrainImageControl";
            this.Size = new System.Drawing.Size(300, 177);
            this.groupBoxTrainImage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private BaseButton baseButtonTrain;
        private System.Windows.Forms.GroupBox groupBoxTrainImage;
        private BaseToggleButton baseToggleButtonAvg;
        private TrainPictureBox pictureBoxTrainImage;
    }
}
