namespace SLD200_MSL
{
    partial class RevisionParameterControl
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
            this.baseGroupBoxMain = new SLD200_MSL.BaseGroupBox();
            this.baseButtonLoad = new SLD200_MSL.BaseButton();
            this.baseButtonSave = new SLD200_MSL.BaseButton();
            this.baseGroupBoxSaveImage = new SLD200_MSL.BaseGroupBox();
            this.baseToggleButtonSaveRevisionImage = new SLD200_MSL.BaseToggleButton();
            this.baseToggleButtonFailedImageSave = new SLD200_MSL.BaseToggleButton();
            this.baseGroupBoxTarget = new SLD200_MSL.BaseGroupBox();
            this.baseLabel9 = new SLD200_MSL.BaseLabel();
            this.baseLabel8 = new SLD200_MSL.BaseLabel();
            this.baseTextBoxTargetHight = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxTargetWidth = new SLD200_MSL.BaseTextBox();
            this.baseGroupBoxColletParam = new SLD200_MSL.BaseGroupBox();
            this.baseTextBoxFilterOffset = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxToleranceX = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxEdgeAvgCount = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxChipThres = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxColletSize = new SLD200_MSL.BaseTextBox();
            this.baseToggleButtonUsePatternMatching = new SLD200_MSL.BaseToggleButton();
            this.baseToggleButtonShowEdgeImg = new SLD200_MSL.BaseToggleButton();
            this.baseLabel7 = new SLD200_MSL.BaseLabel();
            this.baseLabel6 = new SLD200_MSL.BaseLabel();
            this.baseLabel5 = new SLD200_MSL.BaseLabel();
            this.baseLabel4 = new SLD200_MSL.BaseLabel();
            this.baseLabel3 = new SLD200_MSL.BaseLabel();
            this.baseLabel2 = new SLD200_MSL.BaseLabel();
            this.baseLabel1 = new SLD200_MSL.BaseLabel();
            this.baseButtonRun = new SLD200_MSL.BaseButton();
            this.baseGroupBoxMain.SuspendLayout();
            this.baseGroupBoxSaveImage.SuspendLayout();
            this.baseGroupBoxTarget.SuspendLayout();
            this.baseGroupBoxColletParam.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBoxMain
            // 
            this.baseGroupBoxMain.Controls.Add(this.baseButtonRun);
            this.baseGroupBoxMain.Controls.Add(this.baseButtonLoad);
            this.baseGroupBoxMain.Controls.Add(this.baseButtonSave);
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBoxSaveImage);
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBoxTarget);
            this.baseGroupBoxMain.Controls.Add(this.baseGroupBoxColletParam);
            this.baseGroupBoxMain.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxMain.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBoxMain.Name = "baseGroupBoxMain";
            this.baseGroupBoxMain.Size = new System.Drawing.Size(473, 339);
            this.baseGroupBoxMain.TabIndex = 1;
            this.baseGroupBoxMain.TabStop = false;
            this.baseGroupBoxMain.Text = "Parameter";
            // 
            // baseButtonLoad
            // 
            this.baseButtonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonLoad.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonLoad.Location = new System.Drawing.Point(299, 303);
            this.baseButtonLoad.Name = "baseButtonLoad";
            this.baseButtonLoad.Size = new System.Drawing.Size(144, 30);
            this.baseButtonLoad.TabIndex = 18;
            this.baseButtonLoad.Text = "Load";
            this.baseButtonLoad.UseVisualStyleBackColor = false;
            this.baseButtonLoad.Click += new System.EventHandler(this.baseButtonLoad_Click);
            // 
            // baseButtonSave
            // 
            this.baseButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSave.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSave.Location = new System.Drawing.Point(299, 269);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(144, 30);
            this.baseButtonSave.TabIndex = 17;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = false;
            this.baseButtonSave.Click += new System.EventHandler(this.baseButtonSavaRecipe_Click);
            // 
            // baseGroupBoxSaveImage
            // 
            this.baseGroupBoxSaveImage.Controls.Add(this.baseToggleButtonSaveRevisionImage);
            this.baseGroupBoxSaveImage.Controls.Add(this.baseToggleButtonFailedImageSave);
            this.baseGroupBoxSaveImage.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxSaveImage.Location = new System.Drawing.Point(280, 132);
            this.baseGroupBoxSaveImage.Name = "baseGroupBoxSaveImage";
            this.baseGroupBoxSaveImage.Size = new System.Drawing.Size(182, 100);
            this.baseGroupBoxSaveImage.TabIndex = 16;
            this.baseGroupBoxSaveImage.TabStop = false;
            this.baseGroupBoxSaveImage.Text = "Save Image";
            // 
            // baseToggleButtonSaveRevisionImage
            // 
            this.baseToggleButtonSaveRevisionImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonSaveRevisionImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonSaveRevisionImage.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonSaveRevisionImage.Location = new System.Drawing.Point(19, 57);
            this.baseToggleButtonSaveRevisionImage.Name = "baseToggleButtonSaveRevisionImage";
            this.baseToggleButtonSaveRevisionImage.Size = new System.Drawing.Size(144, 30);
            this.baseToggleButtonSaveRevisionImage.TabIndex = 1;
            this.baseToggleButtonSaveRevisionImage.Text = "Revision Image";
            this.baseToggleButtonSaveRevisionImage.UseVisualStyleBackColor = false;
            this.baseToggleButtonSaveRevisionImage.Click += new System.EventHandler(this.baseToggleButtonSaveRevisionImage_Click);
            // 
            // baseToggleButtonFailedImageSave
            // 
            this.baseToggleButtonFailedImageSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonFailedImageSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonFailedImageSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonFailedImageSave.Location = new System.Drawing.Point(19, 21);
            this.baseToggleButtonFailedImageSave.Name = "baseToggleButtonFailedImageSave";
            this.baseToggleButtonFailedImageSave.Size = new System.Drawing.Size(144, 30);
            this.baseToggleButtonFailedImageSave.TabIndex = 0;
            this.baseToggleButtonFailedImageSave.Text = "Faild Image";
            this.baseToggleButtonFailedImageSave.UseVisualStyleBackColor = false;
            this.baseToggleButtonFailedImageSave.Click += new System.EventHandler(this.baseToggleButtonFailedImageSave_Click);
            // 
            // baseGroupBoxTarget
            // 
            this.baseGroupBoxTarget.Controls.Add(this.baseLabel9);
            this.baseGroupBoxTarget.Controls.Add(this.baseLabel8);
            this.baseGroupBoxTarget.Controls.Add(this.baseTextBoxTargetHight);
            this.baseGroupBoxTarget.Controls.Add(this.baseTextBoxTargetWidth);
            this.baseGroupBoxTarget.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxTarget.Location = new System.Drawing.Point(282, 20);
            this.baseGroupBoxTarget.Name = "baseGroupBoxTarget";
            this.baseGroupBoxTarget.Size = new System.Drawing.Size(182, 106);
            this.baseGroupBoxTarget.TabIndex = 15;
            this.baseGroupBoxTarget.TabStop = false;
            this.baseGroupBoxTarget.Text = "TargetSize";
            // 
            // baseLabel9
            // 
            this.baseLabel9.AutoSize = true;
            this.baseLabel9.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel9.ForeColor = System.Drawing.Color.White;
            this.baseLabel9.Location = new System.Drawing.Point(21, 54);
            this.baseLabel9.Name = "baseLabel9";
            this.baseLabel9.Size = new System.Drawing.Size(48, 11);
            this.baseLabel9.TabIndex = 3;
            this.baseLabel9.Text = "Hight :";
            // 
            // baseLabel8
            // 
            this.baseLabel8.AutoSize = true;
            this.baseLabel8.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel8.ForeColor = System.Drawing.Color.White;
            this.baseLabel8.Location = new System.Drawing.Point(16, 28);
            this.baseLabel8.Name = "baseLabel8";
            this.baseLabel8.Size = new System.Drawing.Size(51, 11);
            this.baseLabel8.TabIndex = 2;
            this.baseLabel8.Text = "Width :";
            // 
            // baseTextBoxTargetHight
            // 
            this.baseTextBoxTargetHight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxTargetHight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxTargetHight.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxTargetHight.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxTargetHight.Location = new System.Drawing.Point(81, 52);
            this.baseTextBoxTargetHight.Name = "baseTextBoxTargetHight";
            this.baseTextBoxTargetHight.Size = new System.Drawing.Size(87, 13);
            this.baseTextBoxTargetHight.TabIndex = 1;
            // 
            // baseTextBoxTargetWidth
            // 
            this.baseTextBoxTargetWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxTargetWidth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxTargetWidth.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxTargetWidth.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxTargetWidth.Location = new System.Drawing.Point(81, 26);
            this.baseTextBoxTargetWidth.Name = "baseTextBoxTargetWidth";
            this.baseTextBoxTargetWidth.Size = new System.Drawing.Size(87, 13);
            this.baseTextBoxTargetWidth.TabIndex = 0;
            // 
            // baseGroupBoxColletParam
            // 
            this.baseGroupBoxColletParam.Controls.Add(this.baseTextBoxFilterOffset);
            this.baseGroupBoxColletParam.Controls.Add(this.baseTextBoxToleranceX);
            this.baseGroupBoxColletParam.Controls.Add(this.baseTextBoxEdgeAvgCount);
            this.baseGroupBoxColletParam.Controls.Add(this.baseTextBoxChipThres);
            this.baseGroupBoxColletParam.Controls.Add(this.baseTextBoxColletSize);
            this.baseGroupBoxColletParam.Controls.Add(this.baseToggleButtonUsePatternMatching);
            this.baseGroupBoxColletParam.Controls.Add(this.baseToggleButtonShowEdgeImg);
            this.baseGroupBoxColletParam.Controls.Add(this.baseLabel7);
            this.baseGroupBoxColletParam.Controls.Add(this.baseLabel6);
            this.baseGroupBoxColletParam.Controls.Add(this.baseLabel5);
            this.baseGroupBoxColletParam.Controls.Add(this.baseLabel4);
            this.baseGroupBoxColletParam.Controls.Add(this.baseLabel3);
            this.baseGroupBoxColletParam.Controls.Add(this.baseLabel2);
            this.baseGroupBoxColletParam.Controls.Add(this.baseLabel1);
            this.baseGroupBoxColletParam.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxColletParam.Location = new System.Drawing.Point(6, 20);
            this.baseGroupBoxColletParam.Name = "baseGroupBoxColletParam";
            this.baseGroupBoxColletParam.Size = new System.Drawing.Size(268, 309);
            this.baseGroupBoxColletParam.TabIndex = 14;
            this.baseGroupBoxColletParam.TabStop = false;
            this.baseGroupBoxColletParam.Text = "Collet";
            // 
            // baseTextBoxFilterOffset
            // 
            this.baseTextBoxFilterOffset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxFilterOffset.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxFilterOffset.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxFilterOffset.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxFilterOffset.Location = new System.Drawing.Point(172, 180);
            this.baseTextBoxFilterOffset.Name = "baseTextBoxFilterOffset";
            this.baseTextBoxFilterOffset.Size = new System.Drawing.Size(87, 13);
            this.baseTextBoxFilterOffset.TabIndex = 13;
            // 
            // baseTextBoxToleranceX
            // 
            this.baseTextBoxToleranceX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxToleranceX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxToleranceX.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxToleranceX.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxToleranceX.Location = new System.Drawing.Point(172, 130);
            this.baseTextBoxToleranceX.Name = "baseTextBoxToleranceX";
            this.baseTextBoxToleranceX.Size = new System.Drawing.Size(87, 13);
            this.baseTextBoxToleranceX.TabIndex = 12;
            // 
            // baseTextBoxEdgeAvgCount
            // 
            this.baseTextBoxEdgeAvgCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxEdgeAvgCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxEdgeAvgCount.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxEdgeAvgCount.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxEdgeAvgCount.Location = new System.Drawing.Point(172, 78);
            this.baseTextBoxEdgeAvgCount.Name = "baseTextBoxEdgeAvgCount";
            this.baseTextBoxEdgeAvgCount.Size = new System.Drawing.Size(87, 13);
            this.baseTextBoxEdgeAvgCount.TabIndex = 11;
            // 
            // baseTextBoxChipThres
            // 
            this.baseTextBoxChipThres.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxChipThres.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxChipThres.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxChipThres.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxChipThres.Location = new System.Drawing.Point(172, 52);
            this.baseTextBoxChipThres.Name = "baseTextBoxChipThres";
            this.baseTextBoxChipThres.Size = new System.Drawing.Size(87, 13);
            this.baseTextBoxChipThres.TabIndex = 10;
            // 
            // baseTextBoxColletSize
            // 
            this.baseTextBoxColletSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxColletSize.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxColletSize.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBoxColletSize.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxColletSize.Location = new System.Drawing.Point(172, 26);
            this.baseTextBoxColletSize.Name = "baseTextBoxColletSize";
            this.baseTextBoxColletSize.Size = new System.Drawing.Size(87, 13);
            this.baseTextBoxColletSize.TabIndex = 9;
            // 
            // baseToggleButtonUsePatternMatching
            // 
            this.baseToggleButtonUsePatternMatching.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonUsePatternMatching.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonUsePatternMatching.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonUsePatternMatching.Location = new System.Drawing.Point(7, 270);
            this.baseToggleButtonUsePatternMatching.Name = "baseToggleButtonUsePatternMatching";
            this.baseToggleButtonUsePatternMatching.Size = new System.Drawing.Size(252, 30);
            this.baseToggleButtonUsePatternMatching.TabIndex = 8;
            this.baseToggleButtonUsePatternMatching.Text = "Use Pattern Matching";
            this.baseToggleButtonUsePatternMatching.UseVisualStyleBackColor = false;
            this.baseToggleButtonUsePatternMatching.Click += new System.EventHandler(this.baseToggleButtonUsePatternMatching_Click);
            // 
            // baseToggleButtonShowEdgeImg
            // 
            this.baseToggleButtonShowEdgeImg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseToggleButtonShowEdgeImg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButtonShowEdgeImg.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseToggleButtonShowEdgeImg.Location = new System.Drawing.Point(6, 235);
            this.baseToggleButtonShowEdgeImg.Name = "baseToggleButtonShowEdgeImg";
            this.baseToggleButtonShowEdgeImg.Size = new System.Drawing.Size(253, 30);
            this.baseToggleButtonShowEdgeImg.TabIndex = 7;
            this.baseToggleButtonShowEdgeImg.Text = "Show Edeg Inspection Image";
            this.baseToggleButtonShowEdgeImg.UseVisualStyleBackColor = false;
            this.baseToggleButtonShowEdgeImg.Click += new System.EventHandler(this.baseToggleButtonShowEdgeImg_Click);
            // 
            // baseLabel7
            // 
            this.baseLabel7.AutoSize = true;
            this.baseLabel7.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel7.ForeColor = System.Drawing.Color.White;
            this.baseLabel7.Location = new System.Drawing.Point(104, 182);
            this.baseLabel7.Name = "baseLabel7";
            this.baseLabel7.Size = new System.Drawing.Size(55, 11);
            this.baseLabel7.TabIndex = 6;
            this.baseLabel7.Text = "Offset :";
            // 
            // baseLabel6
            // 
            this.baseLabel6.AutoSize = true;
            this.baseLabel6.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel6.ForeColor = System.Drawing.Color.White;
            this.baseLabel6.Location = new System.Drawing.Point(65, 132);
            this.baseLabel6.Name = "baseLabel6";
            this.baseLabel6.Size = new System.Drawing.Size(88, 11);
            this.baseLabel6.TabIndex = 5;
            this.baseLabel6.Text = "ToleranceX :";
            // 
            // baseLabel5
            // 
            this.baseLabel5.AutoSize = true;
            this.baseLabel5.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel5.ForeColor = System.Drawing.Color.White;
            this.baseLabel5.Location = new System.Drawing.Point(51, 162);
            this.baseLabel5.Name = "baseLabel5";
            this.baseLabel5.Size = new System.Drawing.Size(106, 11);
            this.baseLabel5.TabIndex = 4;
            this.baseLabel5.Text = "HighPass Filter";
            // 
            // baseLabel4
            // 
            this.baseLabel4.AutoSize = true;
            this.baseLabel4.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel4.ForeColor = System.Drawing.Color.White;
            this.baseLabel4.Location = new System.Drawing.Point(81, 112);
            this.baseLabel4.Name = "baseLabel4";
            this.baseLabel4.Size = new System.Drawing.Size(76, 11);
            this.baseLabel4.TabIndex = 3;
            this.baseLabel4.Text = "Find Collet";
            // 
            // baseLabel3
            // 
            this.baseLabel3.AutoSize = true;
            this.baseLabel3.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel3.ForeColor = System.Drawing.Color.White;
            this.baseLabel3.Location = new System.Drawing.Point(29, 80);
            this.baseLabel3.Name = "baseLabel3";
            this.baseLabel3.Size = new System.Drawing.Size(118, 11);
            this.baseLabel3.TabIndex = 2;
            this.baseLabel3.Text = "Edge Avg Count :";
            // 
            // baseLabel2
            // 
            this.baseLabel2.AutoSize = true;
            this.baseLabel2.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel2.ForeColor = System.Drawing.Color.White;
            this.baseLabel2.Location = new System.Drawing.Point(70, 54);
            this.baseLabel2.Name = "baseLabel2";
            this.baseLabel2.Size = new System.Drawing.Size(85, 11);
            this.baseLabel2.TabIndex = 1;
            this.baseLabel2.Text = "Chip Thres :";
            // 
            // baseLabel1
            // 
            this.baseLabel1.AutoSize = true;
            this.baseLabel1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel1.ForeColor = System.Drawing.Color.White;
            this.baseLabel1.Location = new System.Drawing.Point(68, 28);
            this.baseLabel1.Name = "baseLabel1";
            this.baseLabel1.Size = new System.Drawing.Size(85, 11);
            this.baseLabel1.TabIndex = 0;
            this.baseLabel1.Text = "Collet Size :";
            // 
            // baseButtonRun
            // 
            this.baseButtonRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRun.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseButtonRun.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRun.Location = new System.Drawing.Point(299, 233);
            this.baseButtonRun.Name = "baseButtonRun";
            this.baseButtonRun.Size = new System.Drawing.Size(144, 30);
            this.baseButtonRun.TabIndex = 19;
            this.baseButtonRun.Text = "Run";
            this.baseButtonRun.UseVisualStyleBackColor = false;
            this.baseButtonRun.Click += new System.EventHandler(this.baseButtonRun_Click);
            // 
            // RevisionParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxMain);
            this.Name = "RevisionParameterControl";
            this.Size = new System.Drawing.Size(473, 339);
            this.baseGroupBoxMain.ResumeLayout(false);
            this.baseGroupBoxSaveImage.ResumeLayout(false);
            this.baseGroupBoxTarget.ResumeLayout(false);
            this.baseGroupBoxTarget.PerformLayout();
            this.baseGroupBoxColletParam.ResumeLayout(false);
            this.baseGroupBoxColletParam.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseGroupBox baseGroupBoxMain;
        private BaseButton baseButtonLoad;
        private BaseButton baseButtonSave;
        private BaseGroupBox baseGroupBoxSaveImage;
        private BaseToggleButton baseToggleButtonSaveRevisionImage;
        private BaseToggleButton baseToggleButtonFailedImageSave;
        private BaseGroupBox baseGroupBoxTarget;
        private BaseLabel baseLabel9;
        private BaseLabel baseLabel8;
        private BaseTextBox baseTextBoxTargetHight;
        private BaseTextBox baseTextBoxTargetWidth;
        private BaseGroupBox baseGroupBoxColletParam;
        private BaseTextBox baseTextBoxFilterOffset;
        private BaseTextBox baseTextBoxToleranceX;
        private BaseTextBox baseTextBoxEdgeAvgCount;
        private BaseTextBox baseTextBoxChipThres;
        private BaseTextBox baseTextBoxColletSize;
        private BaseToggleButton baseToggleButtonUsePatternMatching;
        private BaseToggleButton baseToggleButtonShowEdgeImg;
        private BaseLabel baseLabel7;
        private BaseLabel baseLabel6;
        private BaseLabel baseLabel5;
        private BaseLabel baseLabel4;
        private BaseLabel baseLabel3;
        private BaseLabel baseLabel2;
        private BaseLabel baseLabel1;
        private BaseButton baseButtonRun;
    }
}