namespace SLD200_MSL
{
    partial class FormSetRoi
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
            this.groupBoxLocation = new System.Windows.Forms.GroupBox();
            this.baseButtonMoveDown = new SLD200_MSL.BaseButton();
            this.baseButtonMoveRight = new SLD200_MSL.BaseButton();
            this.baseButtonMoveLeft = new SLD200_MSL.BaseButton();
            this.baseButtonMoveCenter = new SLD200_MSL.BaseButton();
            this.baseButtonMoveUp = new SLD200_MSL.BaseButton();
            this.groupBoxSize = new System.Windows.Forms.GroupBox();
            this.baseButtonFullSize = new SLD200_MSL.BaseButton();
            this.baseButtonRoiXSizeDown = new SLD200_MSL.BaseButton();
            this.baseButtonRoiXSizeUp = new SLD200_MSL.BaseButton();
            this.baseButtonRoiYSizeDown = new SLD200_MSL.BaseButton();
            this.baseButtonRoiYSizeUp = new SLD200_MSL.BaseButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.baseButtonSave = new SLD200_MSL.BaseButton();
            this.baseTextBoxHeight = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxWidth = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxCenterY = new SLD200_MSL.BaseTextBox();
            this.baseTextBoxCenterX = new SLD200_MSL.BaseTextBox();
            this.baseLabelHeight = new SLD200_MSL.BaseLabel();
            this.baseLabelWidth = new SLD200_MSL.BaseLabel();
            this.baseLabelCenterY = new SLD200_MSL.BaseLabel();
            this.baseLabelCenterX = new SLD200_MSL.BaseLabel();
            this.baseTextBoxSizeToke = new SLD200_MSL.BaseTextBox();
            this.baseLabelSizeToke = new SLD200_MSL.BaseLabel();
            this.baseTextBoxMoveToke = new SLD200_MSL.BaseTextBox();
            this.baseLabelMoveToke = new SLD200_MSL.BaseLabel();
            this.groupBoxLocation.SuspendLayout();
            this.groupBoxSize.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxLocation
            // 
            this.groupBoxLocation.Controls.Add(this.baseTextBoxMoveToke);
            this.groupBoxLocation.Controls.Add(this.baseLabelMoveToke);
            this.groupBoxLocation.Controls.Add(this.baseButtonMoveDown);
            this.groupBoxLocation.Controls.Add(this.baseButtonMoveRight);
            this.groupBoxLocation.Controls.Add(this.baseButtonMoveLeft);
            this.groupBoxLocation.Controls.Add(this.baseButtonMoveCenter);
            this.groupBoxLocation.Controls.Add(this.baseButtonMoveUp);
            this.groupBoxLocation.Location = new System.Drawing.Point(6, 19);
            this.groupBoxLocation.Name = "groupBoxLocation";
            this.groupBoxLocation.Size = new System.Drawing.Size(202, 219);
            this.groupBoxLocation.TabIndex = 0;
            this.groupBoxLocation.TabStop = false;
            this.groupBoxLocation.Text = "Location";
            // 
            // baseButtonMoveDown
            // 
            this.baseButtonMoveDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveDown.BackgroundImage = global::SLD200.Properties.Resources.Down;
            this.baseButtonMoveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseButtonMoveDown.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonMoveDown.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonMoveDown.Location = new System.Drawing.Point(73, 122);
            this.baseButtonMoveDown.Name = "baseButtonMoveDown";
            this.baseButtonMoveDown.Size = new System.Drawing.Size(48, 45);
            this.baseButtonMoveDown.TabIndex = 4;
            this.baseButtonMoveDown.UseVisualStyleBackColor = false;
            this.baseButtonMoveDown.Click += new System.EventHandler(this.baseButtonLocation_Click);
            // 
            // baseButtonMoveRight
            // 
            this.baseButtonMoveRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveRight.BackgroundImage = global::SLD200.Properties.Resources.Right;
            this.baseButtonMoveRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseButtonMoveRight.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonMoveRight.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonMoveRight.Location = new System.Drawing.Point(127, 71);
            this.baseButtonMoveRight.Name = "baseButtonMoveRight";
            this.baseButtonMoveRight.Size = new System.Drawing.Size(48, 45);
            this.baseButtonMoveRight.TabIndex = 3;
            this.baseButtonMoveRight.UseVisualStyleBackColor = false;
            this.baseButtonMoveRight.Click += new System.EventHandler(this.baseButtonLocation_Click);
            // 
            // baseButtonMoveLeft
            // 
            this.baseButtonMoveLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveLeft.BackgroundImage = global::SLD200.Properties.Resources.Left;
            this.baseButtonMoveLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseButtonMoveLeft.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonMoveLeft.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonMoveLeft.Location = new System.Drawing.Point(19, 71);
            this.baseButtonMoveLeft.Name = "baseButtonMoveLeft";
            this.baseButtonMoveLeft.Size = new System.Drawing.Size(48, 45);
            this.baseButtonMoveLeft.TabIndex = 2;
            this.baseButtonMoveLeft.UseVisualStyleBackColor = false;
            this.baseButtonMoveLeft.Click += new System.EventHandler(this.baseButtonLocation_Click);
            // 
            // baseButtonMoveCenter
            // 
            this.baseButtonMoveCenter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveCenter.BackgroundImage = global::SLD200.Properties.Resources._New_CenterNormal;
            this.baseButtonMoveCenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseButtonMoveCenter.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveCenter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonMoveCenter.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonMoveCenter.Location = new System.Drawing.Point(73, 71);
            this.baseButtonMoveCenter.Name = "baseButtonMoveCenter";
            this.baseButtonMoveCenter.Size = new System.Drawing.Size(48, 45);
            this.baseButtonMoveCenter.TabIndex = 1;
            this.baseButtonMoveCenter.UseVisualStyleBackColor = false;
            this.baseButtonMoveCenter.Click += new System.EventHandler(this.baseButtonLocation_Click);
            // 
            // baseButtonMoveUp
            // 
            this.baseButtonMoveUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveUp.BackgroundImage = global::SLD200.Properties.Resources.Up;
            this.baseButtonMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseButtonMoveUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonMoveUp.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonMoveUp.Location = new System.Drawing.Point(73, 20);
            this.baseButtonMoveUp.Name = "baseButtonMoveUp";
            this.baseButtonMoveUp.Size = new System.Drawing.Size(48, 45);
            this.baseButtonMoveUp.TabIndex = 0;
            this.baseButtonMoveUp.UseVisualStyleBackColor = false;
            this.baseButtonMoveUp.Click += new System.EventHandler(this.baseButtonLocation_Click);
            // 
            // groupBoxSize
            // 
            this.groupBoxSize.Controls.Add(this.baseTextBoxSizeToke);
            this.groupBoxSize.Controls.Add(this.baseLabelSizeToke);
            this.groupBoxSize.Controls.Add(this.baseButtonFullSize);
            this.groupBoxSize.Controls.Add(this.baseButtonRoiXSizeDown);
            this.groupBoxSize.Controls.Add(this.baseButtonRoiXSizeUp);
            this.groupBoxSize.Controls.Add(this.baseButtonRoiYSizeDown);
            this.groupBoxSize.Controls.Add(this.baseButtonRoiYSizeUp);
            this.groupBoxSize.Location = new System.Drawing.Point(214, 19);
            this.groupBoxSize.Name = "groupBoxSize";
            this.groupBoxSize.Size = new System.Drawing.Size(189, 219);
            this.groupBoxSize.TabIndex = 1;
            this.groupBoxSize.TabStop = false;
            this.groupBoxSize.Text = "Size";
            // 
            // baseButtonFullSize
            // 
            this.baseButtonFullSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonFullSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonFullSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonFullSize.Location = new System.Drawing.Point(16, 132);
            this.baseButtonFullSize.Name = "baseButtonFullSize";
            this.baseButtonFullSize.Size = new System.Drawing.Size(162, 52);
            this.baseButtonFullSize.TabIndex = 9;
            this.baseButtonFullSize.Text = "FullSize";
            this.baseButtonFullSize.UseVisualStyleBackColor = false;
            this.baseButtonFullSize.Click += new System.EventHandler(this.baseButtonSize_Click);
            // 
            // baseButtonRoiXSizeDown
            // 
            this.baseButtonRoiXSizeDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonRoiXSizeDown.BackgroundImage = global::SLD200.Properties.Resources._New_WidthSizeDownNormal;
            this.baseButtonRoiXSizeDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseButtonRoiXSizeDown.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonRoiXSizeDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRoiXSizeDown.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRoiXSizeDown.Location = new System.Drawing.Point(100, 16);
            this.baseButtonRoiXSizeDown.Name = "baseButtonRoiXSizeDown";
            this.baseButtonRoiXSizeDown.Size = new System.Drawing.Size(78, 52);
            this.baseButtonRoiXSizeDown.TabIndex = 8;
            this.baseButtonRoiXSizeDown.UseVisualStyleBackColor = false;
            this.baseButtonRoiXSizeDown.Click += new System.EventHandler(this.baseButtonSize_Click);
            // 
            // baseButtonRoiXSizeUp
            // 
            this.baseButtonRoiXSizeUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonRoiXSizeUp.BackgroundImage = global::SLD200.Properties.Resources._New_WidthSizeUpNormal;
            this.baseButtonRoiXSizeUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseButtonRoiXSizeUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonRoiXSizeUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRoiXSizeUp.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRoiXSizeUp.Location = new System.Drawing.Point(16, 16);
            this.baseButtonRoiXSizeUp.Name = "baseButtonRoiXSizeUp";
            this.baseButtonRoiXSizeUp.Size = new System.Drawing.Size(78, 52);
            this.baseButtonRoiXSizeUp.TabIndex = 7;
            this.baseButtonRoiXSizeUp.UseVisualStyleBackColor = false;
            this.baseButtonRoiXSizeUp.Click += new System.EventHandler(this.baseButtonSize_Click);
            // 
            // baseButtonRoiYSizeDown
            // 
            this.baseButtonRoiYSizeDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonRoiYSizeDown.BackgroundImage = global::SLD200.Properties.Resources._New_HeightSizeDownNormal;
            this.baseButtonRoiYSizeDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseButtonRoiYSizeDown.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonRoiYSizeDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRoiYSizeDown.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRoiYSizeDown.Location = new System.Drawing.Point(100, 74);
            this.baseButtonRoiYSizeDown.Name = "baseButtonRoiYSizeDown";
            this.baseButtonRoiYSizeDown.Size = new System.Drawing.Size(78, 52);
            this.baseButtonRoiYSizeDown.TabIndex = 6;
            this.baseButtonRoiYSizeDown.UseVisualStyleBackColor = false;
            this.baseButtonRoiYSizeDown.Click += new System.EventHandler(this.baseButtonSize_Click);
            // 
            // baseButtonRoiYSizeUp
            // 
            this.baseButtonRoiYSizeUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonRoiYSizeUp.BackgroundImage = global::SLD200.Properties.Resources._New_HeightSizeUpNormal;
            this.baseButtonRoiYSizeUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.baseButtonRoiYSizeUp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.baseButtonRoiYSizeUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonRoiYSizeUp.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonRoiYSizeUp.Location = new System.Drawing.Point(16, 74);
            this.baseButtonRoiYSizeUp.Name = "baseButtonRoiYSizeUp";
            this.baseButtonRoiYSizeUp.Size = new System.Drawing.Size(78, 52);
            this.baseButtonRoiYSizeUp.TabIndex = 5;
            this.baseButtonRoiYSizeUp.UseVisualStyleBackColor = false;
            this.baseButtonRoiYSizeUp.Click += new System.EventHandler(this.baseButtonSize_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.baseButtonSave);
            this.groupBox1.Controls.Add(this.baseTextBoxHeight);
            this.groupBox1.Controls.Add(this.baseTextBoxWidth);
            this.groupBox1.Controls.Add(this.baseTextBoxCenterY);
            this.groupBox1.Controls.Add(this.baseTextBoxCenterX);
            this.groupBox1.Controls.Add(this.baseLabelHeight);
            this.groupBox1.Controls.Add(this.baseLabelWidth);
            this.groupBox1.Controls.Add(this.baseLabelCenterY);
            this.groupBox1.Controls.Add(this.baseLabelCenterX);
            this.groupBox1.Controls.Add(this.groupBoxSize);
            this.groupBox1.Controls.Add(this.groupBoxLocation);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(410, 364);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // baseButtonSave
            // 
            this.baseButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSave.Location = new System.Drawing.Point(314, 302);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(78, 56);
            this.baseButtonSave.TabIndex = 16;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = false;
            this.baseButtonSave.Click += new System.EventHandler(this.baseButtonSave_Click);
            // 
            // baseTextBoxHeight
            // 
            this.baseTextBoxHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxHeight.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxHeight.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxHeight.Location = new System.Drawing.Point(320, 273);
            this.baseTextBoxHeight.Multiline = true;
            this.baseTextBoxHeight.Name = "baseTextBoxHeight";
            this.baseTextBoxHeight.Size = new System.Drawing.Size(72, 20);
            this.baseTextBoxHeight.TabIndex = 15;
            this.baseTextBoxHeight.Text = "0";
            // 
            // baseTextBoxWidth
            // 
            this.baseTextBoxWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxWidth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxWidth.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxWidth.Location = new System.Drawing.Point(320, 244);
            this.baseTextBoxWidth.Multiline = true;
            this.baseTextBoxWidth.Name = "baseTextBoxWidth";
            this.baseTextBoxWidth.Size = new System.Drawing.Size(72, 20);
            this.baseTextBoxWidth.TabIndex = 14;
            this.baseTextBoxWidth.Text = "0";
            // 
            // baseTextBoxCenterY
            // 
            this.baseTextBoxCenterY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxCenterY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCenterY.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxCenterY.Location = new System.Drawing.Point(121, 278);
            this.baseTextBoxCenterY.Multiline = true;
            this.baseTextBoxCenterY.Name = "baseTextBoxCenterY";
            this.baseTextBoxCenterY.Size = new System.Drawing.Size(81, 20);
            this.baseTextBoxCenterY.TabIndex = 13;
            this.baseTextBoxCenterY.Text = "0";
            // 
            // baseTextBoxCenterX
            // 
            this.baseTextBoxCenterX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxCenterX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxCenterX.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxCenterX.Location = new System.Drawing.Point(121, 241);
            this.baseTextBoxCenterX.Multiline = true;
            this.baseTextBoxCenterX.Name = "baseTextBoxCenterX";
            this.baseTextBoxCenterX.Size = new System.Drawing.Size(81, 20);
            this.baseTextBoxCenterX.TabIndex = 12;
            this.baseTextBoxCenterX.Text = "0";
            // 
            // baseLabelHeight
            // 
            this.baseLabelHeight.AutoSize = true;
            this.baseLabelHeight.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelHeight.ForeColor = System.Drawing.Color.White;
            this.baseLabelHeight.Location = new System.Drawing.Point(238, 269);
            this.baseLabelHeight.Name = "baseLabelHeight";
            this.baseLabelHeight.Size = new System.Drawing.Size(70, 16);
            this.baseLabelHeight.TabIndex = 9;
            this.baseLabelHeight.Text = "Height :";
            // 
            // baseLabelWidth
            // 
            this.baseLabelWidth.AutoSize = true;
            this.baseLabelWidth.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelWidth.ForeColor = System.Drawing.Color.White;
            this.baseLabelWidth.Location = new System.Drawing.Point(244, 240);
            this.baseLabelWidth.Name = "baseLabelWidth";
            this.baseLabelWidth.Size = new System.Drawing.Size(64, 16);
            this.baseLabelWidth.TabIndex = 8;
            this.baseLabelWidth.Text = "Width :";
            // 
            // baseLabelCenterY
            // 
            this.baseLabelCenterY.AutoSize = true;
            this.baseLabelCenterY.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelCenterY.ForeColor = System.Drawing.Color.White;
            this.baseLabelCenterY.Location = new System.Drawing.Point(22, 278);
            this.baseLabelCenterY.Name = "baseLabelCenterY";
            this.baseLabelCenterY.Size = new System.Drawing.Size(84, 16);
            this.baseLabelCenterY.TabIndex = 7;
            this.baseLabelCenterY.Text = "CenterY :";
            // 
            // baseLabelCenterX
            // 
            this.baseLabelCenterX.AutoSize = true;
            this.baseLabelCenterX.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelCenterX.ForeColor = System.Drawing.Color.White;
            this.baseLabelCenterX.Location = new System.Drawing.Point(22, 241);
            this.baseLabelCenterX.Name = "baseLabelCenterX";
            this.baseLabelCenterX.Size = new System.Drawing.Size(85, 16);
            this.baseLabelCenterX.TabIndex = 6;
            this.baseLabelCenterX.Text = "CenterX :";
            // 
            // baseTextBoxSizeToke
            // 
            this.baseTextBoxSizeToke.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxSizeToke.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxSizeToke.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxSizeToke.Location = new System.Drawing.Point(106, 193);
            this.baseTextBoxSizeToke.Multiline = true;
            this.baseTextBoxSizeToke.Name = "baseTextBoxSizeToke";
            this.baseTextBoxSizeToke.Size = new System.Drawing.Size(72, 20);
            this.baseTextBoxSizeToke.TabIndex = 11;
            this.baseTextBoxSizeToke.Text = "0";
            // 
            // baseLabelSizeToke
            // 
            this.baseLabelSizeToke.AutoSize = true;
            this.baseLabelSizeToke.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelSizeToke.ForeColor = System.Drawing.Color.White;
            this.baseLabelSizeToke.Location = new System.Drawing.Point(13, 193);
            this.baseLabelSizeToke.Name = "baseLabelSizeToke";
            this.baseLabelSizeToke.Size = new System.Drawing.Size(82, 16);
            this.baseLabelSizeToke.TabIndex = 10;
            this.baseLabelSizeToke.Text = "SizeToke";
            // 
            // baseTextBoxMoveToke
            // 
            this.baseTextBoxMoveToke.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBoxMoveToke.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBoxMoveToke.ForeColor = System.Drawing.Color.White;
            this.baseTextBoxMoveToke.Location = new System.Drawing.Point(115, 185);
            this.baseTextBoxMoveToke.Multiline = true;
            this.baseTextBoxMoveToke.Name = "baseTextBoxMoveToke";
            this.baseTextBoxMoveToke.Size = new System.Drawing.Size(81, 20);
            this.baseTextBoxMoveToke.TabIndex = 6;
            this.baseTextBoxMoveToke.Text = "0";
            // 
            // baseLabelMoveToke
            // 
            this.baseLabelMoveToke.AutoSize = true;
            this.baseLabelMoveToke.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelMoveToke.ForeColor = System.Drawing.Color.White;
            this.baseLabelMoveToke.Location = new System.Drawing.Point(6, 191);
            this.baseLabelMoveToke.Name = "baseLabelMoveToke";
            this.baseLabelMoveToke.Size = new System.Drawing.Size(91, 16);
            this.baseLabelMoveToke.TabIndex = 5;
            this.baseLabelMoveToke.Text = "MoveToke";
            // 
            // FormSetRoi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 365);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormSetRoi";
            this.groupBoxLocation.ResumeLayout(false);
            this.groupBoxLocation.PerformLayout();
            this.groupBoxSize.ResumeLayout(false);
            this.groupBoxSize.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxLocation;
        private System.Windows.Forms.GroupBox groupBoxSize;
        private BaseButton baseButtonMoveDown;
        private BaseButton baseButtonMoveRight;
        private BaseButton baseButtonMoveLeft;
        private BaseButton baseButtonMoveCenter;
        private BaseButton baseButtonMoveUp;
        private BaseButton baseButtonFullSize;
        private BaseButton baseButtonRoiXSizeDown;
        private BaseButton baseButtonRoiXSizeUp;
        private BaseButton baseButtonRoiYSizeDown;
        private BaseButton baseButtonRoiYSizeUp;
        private BaseTextBox baseTextBoxMoveToke;
        private BaseLabel baseLabelMoveToke;
        private BaseTextBox baseTextBoxSizeToke;
        private BaseLabel baseLabelSizeToke;
        private System.Windows.Forms.GroupBox groupBox1;
        private BaseButton baseButtonSave;
        private BaseTextBox baseTextBoxHeight;
        private BaseTextBox baseTextBoxWidth;
        private BaseTextBox baseTextBoxCenterY;
        private BaseTextBox baseTextBoxCenterX;
        private BaseLabel baseLabelHeight;
        private BaseLabel baseLabelWidth;
        private BaseLabel baseLabelCenterY;
        private BaseLabel baseLabelCenterX;
    }
}
