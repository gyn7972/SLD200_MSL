namespace QMC.Common.UI
{
    partial class PatternMatchingRecipeControl
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
            this.baseGroupBoxAlignRecipe = new BaseGroupBox();
            this.baseGroupBoxParameter = new BaseGroupBox();
            this.ToggleButtonUseMaskImage = new BaseToggleButton();
            this.checkBoxDupCheck = new System.Windows.Forms.CheckBox();
            this.LabelMinScore = new BaseLabel();
            this.LabelMaxInstance = new BaseLabel();
            this.LabelTolerance = new BaseLabel();
            this.TextMinScore = new BaseTextBox();
            this.TextMaxInstance = new BaseTextBox();
            this.TextBoxTolerance = new BaseTextBox();
            this.baseGroupBoxTrainImage = new BaseGroupBox();
            this.pictureBoxTrainImage = new System.Windows.Forms.PictureBox();
            this.basePropertyGridParameter = new BasePropertyGrid();
            this.baseGroupBoxAlignRecipe.SuspendLayout();
            this.baseGroupBoxParameter.SuspendLayout();
            this.baseGroupBoxTrainImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrainImage)).BeginInit();
            this.SuspendLayout();
            // 
            // baseGroupBoxAlignRecipe
            // 
            this.baseGroupBoxAlignRecipe.Controls.Add(this.baseGroupBoxParameter);
            this.baseGroupBoxAlignRecipe.Controls.Add(this.baseGroupBoxTrainImage);
            this.baseGroupBoxAlignRecipe.Controls.Add(this.basePropertyGridParameter);
            this.baseGroupBoxAlignRecipe.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxAlignRecipe.Location = new System.Drawing.Point(5, 0);
            this.baseGroupBoxAlignRecipe.Name = "baseGroupBoxAlignRecipe";
            this.baseGroupBoxAlignRecipe.Size = new System.Drawing.Size(587, 486);
            this.baseGroupBoxAlignRecipe.TabIndex = 1;
            this.baseGroupBoxAlignRecipe.TabStop = false;
            this.baseGroupBoxAlignRecipe.Text = "Aligner Parameter";
            // 
            // baseGroupBoxParameter
            // 
            this.baseGroupBoxParameter.Controls.Add(this.ToggleButtonUseMaskImage);
            this.baseGroupBoxParameter.Controls.Add(this.checkBoxDupCheck);
            this.baseGroupBoxParameter.Controls.Add(this.LabelMinScore);
            this.baseGroupBoxParameter.Controls.Add(this.LabelMaxInstance);
            this.baseGroupBoxParameter.Controls.Add(this.LabelTolerance);
            this.baseGroupBoxParameter.Controls.Add(this.TextMinScore);
            this.baseGroupBoxParameter.Controls.Add(this.TextMaxInstance);
            this.baseGroupBoxParameter.Controls.Add(this.TextBoxTolerance);
            this.baseGroupBoxParameter.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxParameter.Location = new System.Drawing.Point(6, 295);
            this.baseGroupBoxParameter.Name = "baseGroupBoxParameter";
            this.baseGroupBoxParameter.Size = new System.Drawing.Size(300, 180);
            this.baseGroupBoxParameter.TabIndex = 3;
            this.baseGroupBoxParameter.TabStop = false;
            this.baseGroupBoxParameter.Text = "Pattern Matching Parameter";
            // 
            // ToggleButtonUseMaskImage
            // 
            this.ToggleButtonUseMaskImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.ToggleButtonUseMaskImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ToggleButtonUseMaskImage.Font = new System.Drawing.Font("굴림", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ToggleButtonUseMaskImage.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.ToggleButtonUseMaskImage.Location = new System.Drawing.Point(194, 144);
            this.ToggleButtonUseMaskImage.Name = "ToggleButtonUseMaskImage";
            this.ToggleButtonUseMaskImage.Size = new System.Drawing.Size(100, 30);
            this.ToggleButtonUseMaskImage.TabIndex = 7;
            this.ToggleButtonUseMaskImage.Text = "Use MaskImage";
            this.ToggleButtonUseMaskImage.UseVisualStyleBackColor = false;
            this.ToggleButtonUseMaskImage.Click += new System.EventHandler(this.baseToggleButton_Click);
            // 
            // checkBoxDupCheck
            // 
            this.checkBoxDupCheck.AutoSize = true;
            this.checkBoxDupCheck.Location = new System.Drawing.Point(166, 117);
            this.checkBoxDupCheck.Name = "checkBoxDupCheck";
            this.checkBoxDupCheck.Size = new System.Drawing.Size(116, 16);
            this.checkBoxDupCheck.TabIndex = 6;
            this.checkBoxDupCheck.Text = "Duplicate Check";
            this.checkBoxDupCheck.UseVisualStyleBackColor = true;
            // 
            // LabelMinScore
            // 
            this.LabelMinScore.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.LabelMinScore.ForeColor = System.Drawing.Color.White;
            this.LabelMinScore.Location = new System.Drawing.Point(6, 91);
            this.LabelMinScore.Name = "LabelMinScore";
            this.LabelMinScore.Size = new System.Drawing.Size(182, 16);
            this.LabelMinScore.TabIndex = 5;
            this.LabelMinScore.Text = "Min Score :";
            this.LabelMinScore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelMaxInstance
            // 
            this.LabelMaxInstance.Font = new System.Drawing.Font("돋움", 11F, System.Drawing.FontStyle.Bold);
            this.LabelMaxInstance.ForeColor = System.Drawing.Color.White;
            this.LabelMaxInstance.Location = new System.Drawing.Point(6, 60);
            this.LabelMaxInstance.Name = "LabelMaxInstance";
            this.LabelMaxInstance.Size = new System.Drawing.Size(182, 23);
            this.LabelMaxInstance.TabIndex = 4;
            this.LabelMaxInstance.Text = "Max Instance [ea] :";
            this.LabelMaxInstance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LabelTolerance
            // 
            this.LabelTolerance.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
            this.LabelTolerance.ForeColor = System.Drawing.Color.White;
            this.LabelTolerance.Location = new System.Drawing.Point(7, 26);
            this.LabelTolerance.Name = "LabelTolerance";
            this.LabelTolerance.Size = new System.Drawing.Size(182, 23);
            this.LabelTolerance.TabIndex = 3;
            this.LabelTolerance.Text = "Angle Tolerance [Degree] :";
            this.LabelTolerance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextMinScore
            // 
            this.TextMinScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.TextMinScore.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextMinScore.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextMinScore.ForeColor = System.Drawing.Color.White;
            this.TextMinScore.Location = new System.Drawing.Point(194, 91);
            this.TextMinScore.Name = "TextMinScore";
            this.TextMinScore.Size = new System.Drawing.Size(100, 17);
            this.TextMinScore.TabIndex = 2;
            this.TextMinScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TextMinScore.TextChanged += new System.EventHandler(this.ChangeParametersMinScore);
            // 
            // TextMaxInstance
            // 
            this.TextMaxInstance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.TextMaxInstance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextMaxInstance.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextMaxInstance.ForeColor = System.Drawing.Color.White;
            this.TextMaxInstance.Location = new System.Drawing.Point(194, 60);
            this.TextMaxInstance.Name = "TextMaxInstance";
            this.TextMaxInstance.Size = new System.Drawing.Size(100, 17);
            this.TextMaxInstance.TabIndex = 1;
            this.TextMaxInstance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TextMaxInstance.TextChanged += new System.EventHandler(this.ChangeParametersMaxInstnce);
            // 
            // TextBoxTolerance
            // 
            this.TextBoxTolerance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.TextBoxTolerance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBoxTolerance.Font = new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.TextBoxTolerance.ForeColor = System.Drawing.Color.White;
            this.TextBoxTolerance.Location = new System.Drawing.Point(194, 29);
            this.TextBoxTolerance.Name = "TextBoxTolerance";
            this.TextBoxTolerance.Size = new System.Drawing.Size(100, 17);
            this.TextBoxTolerance.TabIndex = 0;
            this.TextBoxTolerance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TextBoxTolerance.TextChanged += new System.EventHandler(this.ChangeParametersTolerance);
            // 
            // baseGroupBoxTrainImage
            // 
            this.baseGroupBoxTrainImage.Controls.Add(this.pictureBoxTrainImage);
            this.baseGroupBoxTrainImage.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxTrainImage.Location = new System.Drawing.Point(6, 20);
            this.baseGroupBoxTrainImage.Name = "baseGroupBoxTrainImage";
            this.baseGroupBoxTrainImage.Size = new System.Drawing.Size(300, 260);
            this.baseGroupBoxTrainImage.TabIndex = 2;
            this.baseGroupBoxTrainImage.TabStop = false;
            this.baseGroupBoxTrainImage.Text = "Train Image";
            // 
            // pictureBoxTrainImage
            // 
            this.pictureBoxTrainImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBoxTrainImage.Location = new System.Drawing.Point(3, 17);
            this.pictureBoxTrainImage.Name = "pictureBoxTrainImage";
            this.pictureBoxTrainImage.Size = new System.Drawing.Size(293, 236);
            this.pictureBoxTrainImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxTrainImage.TabIndex = 0;
            this.pictureBoxTrainImage.TabStop = false;
            // 
            // basePropertyGridParameter
            // 
            this.basePropertyGridParameter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.CategoryForeColor = System.Drawing.Color.Black;
            this.basePropertyGridParameter.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.basePropertyGridParameter.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.basePropertyGridParameter.Location = new System.Drawing.Point(312, 20);
            this.basePropertyGridParameter.Name = "basePropertyGridParameter";
            this.basePropertyGridParameter.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.Size = new System.Drawing.Size(269, 460);
            this.basePropertyGridParameter.TabIndex = 1;
            this.basePropertyGridParameter.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.basePropertyGridParameter.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridParameter.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.basePropertyGridParameter_PropertyValueChanged);
            // 
            // AlignerRecipeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxAlignRecipe);
            this.Name = "AlignerRecipeControl";
            this.Size = new System.Drawing.Size(595, 490);
            this.baseGroupBoxAlignRecipe.ResumeLayout(false);
            this.baseGroupBoxParameter.ResumeLayout(false);
            this.baseGroupBoxParameter.PerformLayout();
            this.baseGroupBoxTrainImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTrainImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private BaseGroupBox baseGroupBoxAlignRecipe;
        private BasePropertyGrid basePropertyGridParameter;
        private BaseGroupBox baseGroupBoxParameter;
        private BaseToggleButton ToggleButtonUseMaskImage;
        private System.Windows.Forms.CheckBox checkBoxDupCheck;
        private BaseLabel LabelMinScore;
        private BaseLabel LabelMaxInstance;
        private BaseLabel LabelTolerance;
        private BaseTextBox TextMinScore;
        private BaseTextBox TextMaxInstance;
        private BaseTextBox TextBoxTolerance;
        private BaseGroupBox baseGroupBoxTrainImage;
        private System.Windows.Forms.PictureBox pictureBoxTrainImage;
    }
}
