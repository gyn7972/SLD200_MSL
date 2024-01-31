namespace CWA150SA_Onsemi300
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
            this.groupBoxTrainImage = new System.Windows.Forms.GroupBox();
            this.pictureBoxTrainImage = new System.Windows.Forms.PictureBox();
            this.baseButtonTrain = new CWA150SA_Onsemi300.BaseButton();
            this.groupBoxTrainImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrainImage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxTrainImage
            // 
            this.groupBoxTrainImage.Controls.Add(this.pictureBoxTrainImage);
            this.groupBoxTrainImage.Controls.Add(this.baseButtonTrain);
            this.groupBoxTrainImage.ForeColor = System.Drawing.Color.White;
            this.groupBoxTrainImage.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTrainImage.Name = "groupBoxTrainImage";
            this.groupBoxTrainImage.Size = new System.Drawing.Size(304, 216);
            this.groupBoxTrainImage.TabIndex = 1;
            this.groupBoxTrainImage.TabStop = false;
            this.groupBoxTrainImage.Text = " Train Image ";
            // 
            // pictureBoxTrainImage
            // 
            this.pictureBoxTrainImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.pictureBoxTrainImage.Location = new System.Drawing.Point(9, 25);
            this.pictureBoxTrainImage.Name = "pictureBoxTrainImage";
            this.pictureBoxTrainImage.Size = new System.Drawing.Size(186, 180);
            this.pictureBoxTrainImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxTrainImage.TabIndex = 1;
            this.pictureBoxTrainImage.TabStop = false;
            // 
            // baseButtonTrain
            // 
            this.baseButtonTrain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonTrain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonTrain.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonTrain.Location = new System.Drawing.Point(203, 139);
            this.baseButtonTrain.Name = "baseButtonTrain";
            this.baseButtonTrain.Size = new System.Drawing.Size(91, 66);
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
            this.Size = new System.Drawing.Size(307, 218);
            this.groupBoxTrainImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrainImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseButton baseButtonTrain;
        private System.Windows.Forms.GroupBox groupBoxTrainImage;
        private System.Windows.Forms.PictureBox pictureBoxTrainImage;
    }
}
