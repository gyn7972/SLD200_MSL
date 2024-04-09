namespace QMC.Common.UI
{
    partial class SearchResultControl
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
            this.baseButtonSearch = new QMC.Common.UI.BaseButton();
            this.groupBoxSearchResult = new System.Windows.Forms.GroupBox();
            this.baseGroupBoxPatternMatching = new QMC.Common.UI.BaseGroupBox();
            this.baseTextBoxMinScore = new QMC.Common.UI.BaseTextBox();
            this.baseTextBoxMaxInstance = new QMC.Common.UI.BaseTextBox();
            this.baseTextBoxAngleTolerance = new QMC.Common.UI.BaseTextBox();
            this.baseLabelMinScore = new QMC.Common.UI.BaseLabel();
            this.baseLabelMaxInstance = new QMC.Common.UI.BaseLabel();
            this.baseLabelAngleTolerance = new QMC.Common.UI.BaseLabel();
            this.baseToggleButtonUseMaskImage = new QMC.Common.UI.BaseToggleButton();
            this.baseToggleButtonDuplicateCheck = new QMC.Common.UI.BaseToggleButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.radioButtonMilimiter = new System.Windows.Forms.RadioButton();
            this.radioButtonPixel = new System.Windows.Forms.RadioButton();
            this.groupBoxPosition = new System.Windows.Forms.GroupBox();
            this.baseLabelT = new QMC.Common.UI.BaseLabel();
            this.baseLabelY = new QMC.Common.UI.BaseLabel();
            this.baseLabelX = new QMC.Common.UI.BaseLabel();
            this.baseTextBoxT = new QMC.Common.UI.BaseTextBox();
            this.baseTextBoxY = new QMC.Common.UI.BaseTextBox();
            this.baseTextBoxX = new QMC.Common.UI.BaseTextBox();
            this.groupBoxSearchResult.SuspendLayout();
            this.baseGroupBoxPatternMatching.SuspendLayout();
            this.groupBoxPosition.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseButtonSearch
            // 
            this.baseButtonSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSearch.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSearch.Location = new System.Drawing.Point(219, 139);
            this.baseButtonSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseButtonSearch.Name = "baseButtonSearch";
            this.baseButtonSearch.Size = new System.Drawing.Size(114, 38);
            this.baseButtonSearch.TabIndex = 1;
            this.baseButtonSearch.Text = "Search";
            this.baseButtonSearch.UseVisualStyleBackColor = false;
            this.baseButtonSearch.Click += new System.EventHandler(this.baseButtonSearch_Click);
            // 
            // groupBoxSearchResult
            // 
            this.groupBoxSearchResult.Controls.Add(this.tabControl1);
            this.groupBoxSearchResult.Controls.Add(this.baseGroupBoxPatternMatching);
            this.groupBoxSearchResult.ForeColor = System.Drawing.Color.White;
            this.groupBoxSearchResult.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSearchResult.Name = "groupBoxSearchResult";
            this.groupBoxSearchResult.Size = new System.Drawing.Size(301, 139);
            this.groupBoxSearchResult.TabIndex = 2;
            this.groupBoxSearchResult.TabStop = false;
            this.groupBoxSearchResult.Text = "Search Result";
            // 
            // baseGroupBoxPatternMatching
            // 
            this.baseGroupBoxPatternMatching.Controls.Add(this.baseTextBoxMinScore);
            this.baseGroupBoxPatternMatching.Controls.Add(this.baseTextBoxMaxInstance);
            this.baseGroupBoxPatternMatching.Controls.Add(this.baseTextBoxAngleTolerance);
            this.baseGroupBoxPatternMatching.Controls.Add(this.baseLabelMinScore);
            this.baseGroupBoxPatternMatching.Controls.Add(this.baseLabelMaxInstance);
            this.baseGroupBoxPatternMatching.Controls.Add(this.baseLabelAngleTolerance);
            this.baseGroupBoxPatternMatching.Controls.Add(this.baseToggleButtonUseMaskImage);
            this.baseGroupBoxPatternMatching.Controls.Add(this.baseToggleButtonDuplicateCheck);
            this.baseGroupBoxPatternMatching.ForeColor = System.Drawing.Color.Black;
            this.baseGroupBoxPatternMatching.Location = new System.Drawing.Point(5, 14);
            this.baseGroupBoxPatternMatching.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxPatternMatching.Name = "baseGroupBoxPatternMatching";
            this.baseGroupBoxPatternMatching.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseGroupBoxPatternMatching.Size = new System.Drawing.Size(280, 109);
            this.baseGroupBoxPatternMatching.TabIndex = 19;
            this.baseGroupBoxPatternMatching.TabStop = false;
            this.baseGroupBoxPatternMatching.Text = "Pattern Matching Parameter";
            // 
            // baseTextBoxMinScore
            // 
            this.baseTextBoxMinScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxMinScore.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxMinScore.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxMinScore.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxMinScore.Location = new System.Drawing.Point(185, 46);
            this.baseTextBoxMinScore.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxMinScore.Name = "baseTextBoxMinScore";
            this.baseTextBoxMinScore.Size = new System.Drawing.Size(88, 13);
            this.baseTextBoxMinScore.TabIndex = 7;
            // 
            // baseTextBoxMaxInstance
            // 
            this.baseTextBoxMaxInstance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxMaxInstance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxMaxInstance.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxMaxInstance.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxMaxInstance.Location = new System.Drawing.Point(185, 31);
            this.baseTextBoxMaxInstance.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxMaxInstance.Name = "baseTextBoxMaxInstance";
            this.baseTextBoxMaxInstance.Size = new System.Drawing.Size(88, 13);
            this.baseTextBoxMaxInstance.TabIndex = 6;
            // 
            // baseTextBoxAngleTolerance
            // 
            this.baseTextBoxAngleTolerance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxAngleTolerance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxAngleTolerance.Font = new System.Drawing.Font("굴림", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxAngleTolerance.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxAngleTolerance.Location = new System.Drawing.Point(185, 16);
            this.baseTextBoxAngleTolerance.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseTextBoxAngleTolerance.Name = "baseTextBoxAngleTolerance";
            this.baseTextBoxAngleTolerance.Size = new System.Drawing.Size(88, 13);
            this.baseTextBoxAngleTolerance.TabIndex = 5;
            // 
            // baseLabelMinScore
            // 
            this.baseLabelMinScore.AutoSize = true;
            this.baseLabelMinScore.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabelMinScore.ForeColor = System.Drawing.Color.Black;
            this.baseLabelMinScore.Location = new System.Drawing.Point(92, 48);
            this.baseLabelMinScore.Name = "baseLabelMinScore";
            this.baseLabelMinScore.Size = new System.Drawing.Size(74, 11);
            this.baseLabelMinScore.TabIndex = 4;
            this.baseLabelMinScore.Text = "MinScore :";
            // 
            // baseLabelMaxInstance
            // 
            this.baseLabelMaxInstance.AutoSize = true;
            this.baseLabelMaxInstance.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabelMaxInstance.ForeColor = System.Drawing.Color.Black;
            this.baseLabelMaxInstance.Location = new System.Drawing.Point(33, 33);
            this.baseLabelMaxInstance.Name = "baseLabelMaxInstance";
            this.baseLabelMaxInstance.Size = new System.Drawing.Size(133, 11);
            this.baseLabelMaxInstance.TabIndex = 3;
            this.baseLabelMaxInstance.Text = "Max Instance [EA] :";
            // 
            // baseLabelAngleTolerance
            // 
            this.baseLabelAngleTolerance.AutoSize = true;
            this.baseLabelAngleTolerance.Font = new System.Drawing.Font("돋움", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabelAngleTolerance.ForeColor = System.Drawing.Color.Black;
            this.baseLabelAngleTolerance.Location = new System.Drawing.Point(5, 18);
            this.baseLabelAngleTolerance.Name = "baseLabelAngleTolerance";
            this.baseLabelAngleTolerance.Size = new System.Drawing.Size(161, 11);
            this.baseLabelAngleTolerance.TabIndex = 2;
            this.baseLabelAngleTolerance.Text = "Angle Tolerance [Deg] :";
            // 
            // baseToggleButtonUseMaskImage
            // 
            this.baseToggleButtonUseMaskImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonUseMaskImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonUseMaskImage.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonUseMaskImage.Location = new System.Drawing.Point(185, 62);
            this.baseToggleButtonUseMaskImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonUseMaskImage.Name = "baseToggleButtonUseMaskImage";
            this.baseToggleButtonUseMaskImage.Size = new System.Drawing.Size(88, 24);
            this.baseToggleButtonUseMaskImage.TabIndex = 1;
            this.baseToggleButtonUseMaskImage.Text = "Use Mask Image";
            this.baseToggleButtonUseMaskImage.UseVisualStyleBackColor = false;
            this.baseToggleButtonUseMaskImage.Click += new System.EventHandler(this.baseToggleButtonUseMaskImage_Click);
            // 
            // baseToggleButtonDuplicateCheck
            // 
            this.baseToggleButtonDuplicateCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonDuplicateCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonDuplicateCheck.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonDuplicateCheck.Location = new System.Drawing.Point(83, 62);
            this.baseToggleButtonDuplicateCheck.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.baseToggleButtonDuplicateCheck.Name = "baseToggleButtonDuplicateCheck";
            this.baseToggleButtonDuplicateCheck.Size = new System.Drawing.Size(88, 24);
            this.baseToggleButtonDuplicateCheck.TabIndex = 0;
            this.baseToggleButtonDuplicateCheck.Text = "Duplicate Check";
            this.baseToggleButtonDuplicateCheck.UseVisualStyleBackColor = false;
            this.baseToggleButtonDuplicateCheck.Click += new System.EventHandler(this.baseToggleButtonDuplicateCheck_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(684, 98);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(242, 326);
            this.tabControl1.TabIndex = 18;
            // 
            // radioButtonMilimiter
            // 
            this.radioButtonMilimiter.AutoSize = true;
            this.radioButtonMilimiter.Location = new System.Drawing.Point(219, 114);
            this.radioButtonMilimiter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonMilimiter.Name = "radioButtonMilimiter";
            this.radioButtonMilimiter.Size = new System.Drawing.Size(79, 19);
            this.radioButtonMilimiter.TabIndex = 4;
            this.radioButtonMilimiter.TabStop = true;
            this.radioButtonMilimiter.Text = "Milimeter";
            this.radioButtonMilimiter.UseVisualStyleBackColor = true;
            this.radioButtonMilimiter.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonPixel
            // 
            this.radioButtonPixel.AutoSize = true;
            this.radioButtonPixel.Location = new System.Drawing.Point(219, 85);
            this.radioButtonPixel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioButtonPixel.Name = "radioButtonPixel";
            this.radioButtonPixel.Size = new System.Drawing.Size(61, 19);
            this.radioButtonPixel.TabIndex = 3;
            this.radioButtonPixel.TabStop = true;
            this.radioButtonPixel.Text = "Pixel";
            this.radioButtonPixel.UseVisualStyleBackColor = true;
            this.radioButtonPixel.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // groupBoxPosition
            // 
            this.groupBoxPosition.Controls.Add(this.baseLabelT);
            this.groupBoxPosition.Controls.Add(this.baseLabelY);
            this.groupBoxPosition.Controls.Add(this.baseLabelX);
            this.groupBoxPosition.Controls.Add(this.baseTextBoxT);
            this.groupBoxPosition.Controls.Add(this.baseTextBoxY);
            this.groupBoxPosition.Controls.Add(this.baseTextBoxX);
            this.groupBoxPosition.ForeColor = System.Drawing.Color.Black;
            this.groupBoxPosition.Location = new System.Drawing.Point(7, 76);
            this.groupBoxPosition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxPosition.Name = "groupBoxPosition";
            this.groupBoxPosition.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxPosition.Size = new System.Drawing.Size(206, 116);
            this.groupBoxPosition.TabIndex = 2;
            this.groupBoxPosition.TabStop = false;
            this.groupBoxPosition.Text = "Position";
            // 
            // baseLabelT
            // 
            this.baseLabelT.AutoSize = true;
            this.baseLabelT.Font = new System.Drawing.Font("돋움", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabelT.ForeColor = System.Drawing.Color.Black;
            this.baseLabelT.Location = new System.Drawing.Point(27, 85);
            this.baseLabelT.Name = "baseLabelT";
            this.baseLabelT.Size = new System.Drawing.Size(28, 14);
            this.baseLabelT.TabIndex = 5;
            this.baseLabelT.Text = "T :";
            // 
            // baseLabelY
            // 
            this.baseLabelY.AutoSize = true;
            this.baseLabelY.Font = new System.Drawing.Font("돋움", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabelY.ForeColor = System.Drawing.Color.Black;
            this.baseLabelY.Location = new System.Drawing.Point(27, 54);
            this.baseLabelY.Name = "baseLabelY";
            this.baseLabelY.Size = new System.Drawing.Size(30, 14);
            this.baseLabelY.TabIndex = 4;
            this.baseLabelY.Text = "Y :";
            // 
            // baseLabelX
            // 
            this.baseLabelX.AutoSize = true;
            this.baseLabelX.Font = new System.Drawing.Font("돋움", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabelX.ForeColor = System.Drawing.Color.Black;
            this.baseLabelX.Location = new System.Drawing.Point(27, 25);
            this.baseLabelX.Name = "baseLabelX";
            this.baseLabelX.Size = new System.Drawing.Size(30, 14);
            this.baseLabelX.TabIndex = 3;
            this.baseLabelX.Text = "X :";
            // 
            // baseTextBoxT
            // 
            this.baseTextBoxT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxT.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxT.Location = new System.Drawing.Point(69, 85);
            this.baseTextBoxT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseTextBoxT.Name = "baseTextBoxT";
            this.baseTextBoxT.Size = new System.Drawing.Size(100, 14);
            this.baseTextBoxT.TabIndex = 2;
            // 
            // baseTextBoxY
            // 
            this.baseTextBoxY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxY.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxY.Location = new System.Drawing.Point(69, 54);
            this.baseTextBoxY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseTextBoxY.Name = "baseTextBoxY";
            this.baseTextBoxY.Size = new System.Drawing.Size(100, 14);
            this.baseTextBoxY.TabIndex = 1;
            // 
            // baseTextBoxX
            // 
            this.baseTextBoxX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxX.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxX.Location = new System.Drawing.Point(69, 25);
            this.baseTextBoxX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseTextBoxX.Name = "baseTextBoxX";
            this.baseTextBoxX.Size = new System.Drawing.Size(100, 14);
            this.baseTextBoxX.TabIndex = 0;
            // 
            // SearchResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxSearchResult);
            this.Name = "SearchResultControl";
            this.Size = new System.Drawing.Size(303, 139);
            this.groupBoxSearchResult.ResumeLayout(false);
            this.baseGroupBoxPatternMatching.ResumeLayout(false);
            this.baseGroupBoxPatternMatching.PerformLayout();
            this.groupBoxPosition.ResumeLayout(false);
            this.groupBoxPosition.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseButton baseButtonSearch;
        private System.Windows.Forms.GroupBox groupBoxSearchResult;
        private System.Windows.Forms.GroupBox groupBoxPosition;
        private BaseTextBox baseTextBoxX;
        private BaseLabel baseLabelT;
        private BaseLabel baseLabelY;
        private BaseLabel baseLabelX;
        private BaseTextBox baseTextBoxT;
        private BaseTextBox baseTextBoxY;
        private System.Windows.Forms.RadioButton radioButtonMilimiter;
        private System.Windows.Forms.RadioButton radioButtonPixel;
        private System.Windows.Forms.TabControl tabControl1;
        private BaseGroupBox baseGroupBoxPatternMatching;
        private BaseLabel baseLabelMinScore;
        private BaseLabel baseLabelMaxInstance;
        private BaseLabel baseLabelAngleTolerance;
        private BaseToggleButton baseToggleButtonUseMaskImage;
        private BaseToggleButton baseToggleButtonDuplicateCheck;
        private BaseTextBox baseTextBoxMinScore;
        private BaseTextBox baseTextBoxMaxInstance;
        private BaseTextBox baseTextBoxAngleTolerance;
    }
}
