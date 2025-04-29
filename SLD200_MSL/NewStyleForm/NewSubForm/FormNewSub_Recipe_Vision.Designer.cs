namespace SLD200.NewStyleForm.NewSubForm
{
    partial class FormNewSub_Recipe_Vision
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
            this.components = new System.ComponentModel.Container();
            this.button_RecipeVision_CameraStop = new System.Windows.Forms.Button();
            this.checkBox_RecipeVision_CirclePos = new System.Windows.Forms.CheckBox();
            this.label_RecipeVision_CirclePosY = new System.Windows.Forms.Label();
            this.button_RecipeVision_CameraLive = new System.Windows.Forms.Button();
            this.label_RecipeVision_CirclePosX = new System.Windows.Forms.Label();
            this.button_RecipeVision_Vision_Save = new System.Windows.Forms.Button();
            this.groupBox_RecipeVision_Illumination = new System.Windows.Forms.GroupBox();
            this.radioButton_RecipeVision_Light_Red = new System.Windows.Forms.RadioButton();
            this.radioButton_RecipeVision_Light_IR = new System.Windows.Forms.RadioButton();
            this.baseLabel_RecipeVision_Max = new SLD200_MSL.BaseLabel();
            this.baseLabel_RecipeVision_Min = new SLD200_MSL.BaseLabel();
            this.hScrollBar_RecipeVision_Illuminator = new System.Windows.Forms.HScrollBar();
            this.button_RecipeVision_Illumin_value = new System.Windows.Forms.Button();
            this.textBox_RecipeVision_IlluminationValue = new System.Windows.Forms.TextBox();
            this.button_RecipeVision_Inspect = new System.Windows.Forms.Button();
            this.button_RecipeVision_Train = new System.Windows.Forms.Button();
            this.groupBox_RecipeVision_MarkMatching = new System.Windows.Forms.GroupBox();
            this.radioButton_RecipeVision_Blob = new System.Windows.Forms.RadioButton();
            this.radioButton_RecipeVision_Pattern = new System.Windows.Forms.RadioButton();
            this.groupBox_RecipeVision_MarkType = new System.Windows.Forms.GroupBox();
            this.radioButton_RecipeVision_Circle = new System.Windows.Forms.RadioButton();
            this.radioButton_RecipeVision_Cross = new System.Windows.Forms.RadioButton();
            this.groupBox_RecipeVision_SearchResult = new SLD200_MSL.WATGroupBox();
            this.tabControl_RecipeVision_SearchResult = new System.Windows.Forms.TabControl();
            this.Parameter = new System.Windows.Forms.TabPage();
            this.basetextBox_RecipeVision_MinScore = new SLD200_MSL.BaseTextBox();
            this.baseLabel_RecipeVision_AngleTolerance = new SLD200_MSL.BaseLabel();
            this.basetextBox_RecipeVision_MaxInstance = new SLD200_MSL.BaseTextBox();
            this.baseToggleButton_RecipeVision_DuplicateCheck = new SLD200_MSL.BaseToggleButton();
            this.basetextBox_RecipeVision_AngleTolerance = new SLD200_MSL.BaseTextBox();
            this.baseToggleButton_RecipeVision_UseMaskImage = new SLD200_MSL.BaseToggleButton();
            this.baseLabel_RecipeVision_MinScore = new SLD200_MSL.BaseLabel();
            this.baseLabel_RecipeVision_MaxInstance = new SLD200_MSL.BaseLabel();
            this.Result = new System.Windows.Forms.TabPage();
            this.button_RecipeVision_Search = new System.Windows.Forms.Button();
            this.radioButton_RecipeVision_mm = new System.Windows.Forms.RadioButton();
            this.radioButton_RecipeVision_Pixel = new System.Windows.Forms.RadioButton();
            this.baseTextBox_RecipeVision_PositionT = new SLD200_MSL.BaseTextBox();
            this.baseTextBox_RecipeVision_PositionY = new SLD200_MSL.BaseTextBox();
            this.baseLabel_RecipeVision_PositionT = new SLD200_MSL.BaseLabel();
            this.baseLabel_RecipeVision_PositionY = new SLD200_MSL.BaseLabel();
            this.baseLabel_RecipeVision_PositionX = new SLD200_MSL.BaseLabel();
            this.baseTextBox_RecipeVision_PositionX = new SLD200_MSL.BaseTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.groupBox_RecipeVision_TrainImage = new SLD200_MSL.WATGroupBox();
            this.pictureBox_RecipeVision_TrainImage = new System.Windows.Forms.PictureBox();
            this.button_RecipeVision_Train_Set = new SLD200_MSL.BaseButton();
            this.ImageViewer_RecipeVision_Rows = new QMC.Common.Hmi.VisionImageViewer();
            this.ImageViewer_RecipeVision_highs = new QMC.Common.Hmi.VisionImageViewer();
            this.groupBox_RecipeVision_Illumination.SuspendLayout();
            this.groupBox_RecipeVision_MarkMatching.SuspendLayout();
            this.groupBox_RecipeVision_MarkType.SuspendLayout();
            this.groupBox_RecipeVision_SearchResult.SuspendLayout();
            this.tabControl_RecipeVision_SearchResult.SuspendLayout();
            this.Parameter.SuspendLayout();
            this.Result.SuspendLayout();
            this.groupBox_RecipeVision_TrainImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_RecipeVision_TrainImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer_RecipeVision_Rows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer_RecipeVision_highs)).BeginInit();
            this.SuspendLayout();
            // 
            // button_RecipeVision_CameraStop
            // 
            this.button_RecipeVision_CameraStop.Location = new System.Drawing.Point(368, 287);
            this.button_RecipeVision_CameraStop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_RecipeVision_CameraStop.Name = "button_RecipeVision_CameraStop";
            this.button_RecipeVision_CameraStop.Size = new System.Drawing.Size(52, 18);
            this.button_RecipeVision_CameraStop.TabIndex = 199;
            this.button_RecipeVision_CameraStop.Text = "STOP";
            this.button_RecipeVision_CameraStop.UseVisualStyleBackColor = true;
            this.button_RecipeVision_CameraStop.Click += new System.EventHandler(this.button_RecipeVision_CameraStop_Click);
            // 
            // checkBox_RecipeVision_CirclePos
            // 
            this.checkBox_RecipeVision_CirclePos.AutoSize = true;
            this.checkBox_RecipeVision_CirclePos.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_RecipeVision_CirclePos.Location = new System.Drawing.Point(826, 367);
            this.checkBox_RecipeVision_CirclePos.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBox_RecipeVision_CirclePos.Name = "checkBox_RecipeVision_CirclePos";
            this.checkBox_RecipeVision_CirclePos.Size = new System.Drawing.Size(87, 20);
            this.checkBox_RecipeVision_CirclePos.TabIndex = 198;
            this.checkBox_RecipeVision_CirclePos.Text = "Circle X,Y";
            this.checkBox_RecipeVision_CirclePos.UseVisualStyleBackColor = true;
            this.checkBox_RecipeVision_CirclePos.CheckedChanged += new System.EventHandler(this.checkBox_RecipeVision_CirclePos_CheckedChanged);
            // 
            // label_RecipeVision_CirclePosY
            // 
            this.label_RecipeVision_CirclePosY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_RecipeVision_CirclePosY.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label_RecipeVision_CirclePosY.Location = new System.Drawing.Point(826, 404);
            this.label_RecipeVision_CirclePosY.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.label_RecipeVision_CirclePosY.Name = "label_RecipeVision_CirclePosY";
            this.label_RecipeVision_CirclePosY.Size = new System.Drawing.Size(78, 17);
            this.label_RecipeVision_CirclePosY.TabIndex = 188;
            this.label_RecipeVision_CirclePosY.Text = "--";
            this.label_RecipeVision_CirclePosY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_RecipeVision_CameraLive
            // 
            this.button_RecipeVision_CameraLive.Location = new System.Drawing.Point(312, 287);
            this.button_RecipeVision_CameraLive.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_RecipeVision_CameraLive.Name = "button_RecipeVision_CameraLive";
            this.button_RecipeVision_CameraLive.Size = new System.Drawing.Size(52, 18);
            this.button_RecipeVision_CameraLive.TabIndex = 197;
            this.button_RecipeVision_CameraLive.Text = "LIVE";
            this.button_RecipeVision_CameraLive.UseVisualStyleBackColor = true;
            this.button_RecipeVision_CameraLive.Click += new System.EventHandler(this.button_RecipeVision_CameraLive_Click);
            // 
            // label_RecipeVision_CirclePosX
            // 
            this.label_RecipeVision_CirclePosX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_RecipeVision_CirclePosX.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label_RecipeVision_CirclePosX.Location = new System.Drawing.Point(826, 383);
            this.label_RecipeVision_CirclePosX.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.label_RecipeVision_CirclePosX.Name = "label_RecipeVision_CirclePosX";
            this.label_RecipeVision_CirclePosX.Size = new System.Drawing.Size(78, 17);
            this.label_RecipeVision_CirclePosX.TabIndex = 187;
            this.label_RecipeVision_CirclePosX.Text = "--";
            this.label_RecipeVision_CirclePosX.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button_RecipeVision_Vision_Save
            // 
            this.button_RecipeVision_Vision_Save.Location = new System.Drawing.Point(825, 429);
            this.button_RecipeVision_Vision_Save.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_RecipeVision_Vision_Save.Name = "button_RecipeVision_Vision_Save";
            this.button_RecipeVision_Vision_Save.Size = new System.Drawing.Size(79, 34);
            this.button_RecipeVision_Vision_Save.TabIndex = 196;
            this.button_RecipeVision_Vision_Save.Text = "SAVE";
            this.button_RecipeVision_Vision_Save.UseVisualStyleBackColor = true;
            this.button_RecipeVision_Vision_Save.Click += new System.EventHandler(this.button_RecipeVision_Vision_Save_Click);
            // 
            // groupBox_RecipeVision_Illumination
            // 
            this.groupBox_RecipeVision_Illumination.Controls.Add(this.radioButton_RecipeVision_Light_Red);
            this.groupBox_RecipeVision_Illumination.Controls.Add(this.radioButton_RecipeVision_Light_IR);
            this.groupBox_RecipeVision_Illumination.Controls.Add(this.baseLabel_RecipeVision_Max);
            this.groupBox_RecipeVision_Illumination.Controls.Add(this.baseLabel_RecipeVision_Min);
            this.groupBox_RecipeVision_Illumination.Controls.Add(this.hScrollBar_RecipeVision_Illuminator);
            this.groupBox_RecipeVision_Illumination.Controls.Add(this.button_RecipeVision_Illumin_value);
            this.groupBox_RecipeVision_Illumination.Controls.Add(this.textBox_RecipeVision_IlluminationValue);
            this.groupBox_RecipeVision_Illumination.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox_RecipeVision_Illumination.Location = new System.Drawing.Point(606, 289);
            this.groupBox_RecipeVision_Illumination.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_RecipeVision_Illumination.Name = "groupBox_RecipeVision_Illumination";
            this.groupBox_RecipeVision_Illumination.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox_RecipeVision_Illumination.Size = new System.Drawing.Size(214, 69);
            this.groupBox_RecipeVision_Illumination.TabIndex = 195;
            this.groupBox_RecipeVision_Illumination.TabStop = false;
            this.groupBox_RecipeVision_Illumination.Text = " Illumination Brightness (%) ";
            // 
            // radioButton_RecipeVision_Light_Red
            // 
            this.radioButton_RecipeVision_Light_Red.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_RecipeVision_Light_Red.Location = new System.Drawing.Point(43, 17);
            this.radioButton_RecipeVision_Light_Red.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_RecipeVision_Light_Red.Name = "radioButton_RecipeVision_Light_Red";
            this.radioButton_RecipeVision_Light_Red.Size = new System.Drawing.Size(39, 18);
            this.radioButton_RecipeVision_Light_Red.TabIndex = 34;
            this.radioButton_RecipeVision_Light_Red.Text = "Red";
            this.radioButton_RecipeVision_Light_Red.UseVisualStyleBackColor = true;
            this.radioButton_RecipeVision_Light_Red.CheckedChanged += new System.EventHandler(this.radioButton_RecipeVision_Light_Red_CheckedChanged);
            // 
            // radioButton_RecipeVision_Light_IR
            // 
            this.radioButton_RecipeVision_Light_IR.Checked = true;
            this.radioButton_RecipeVision_Light_IR.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_RecipeVision_Light_IR.Location = new System.Drawing.Point(7, 17);
            this.radioButton_RecipeVision_Light_IR.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_RecipeVision_Light_IR.Name = "radioButton_RecipeVision_Light_IR";
            this.radioButton_RecipeVision_Light_IR.Size = new System.Drawing.Size(31, 18);
            this.radioButton_RecipeVision_Light_IR.TabIndex = 33;
            this.radioButton_RecipeVision_Light_IR.TabStop = true;
            this.radioButton_RecipeVision_Light_IR.Text = "IR";
            this.radioButton_RecipeVision_Light_IR.UseVisualStyleBackColor = true;
            this.radioButton_RecipeVision_Light_IR.CheckedChanged += new System.EventHandler(this.radioButton_RecipeVision_Light_IR_CheckedChanged);
            // 
            // baseLabel_RecipeVision_Max
            // 
            this.baseLabel_RecipeVision_Max.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabel_RecipeVision_Max.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_RecipeVision_Max.Location = new System.Drawing.Point(157, 45);
            this.baseLabel_RecipeVision_Max.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.baseLabel_RecipeVision_Max.Name = "baseLabel_RecipeVision_Max";
            this.baseLabel_RecipeVision_Max.Size = new System.Drawing.Size(38, 15);
            this.baseLabel_RecipeVision_Max.TabIndex = 32;
            this.baseLabel_RecipeVision_Max.Text = "255";
            this.baseLabel_RecipeVision_Max.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // baseLabel_RecipeVision_Min
            // 
            this.baseLabel_RecipeVision_Min.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.baseLabel_RecipeVision_Min.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_RecipeVision_Min.Location = new System.Drawing.Point(11, 45);
            this.baseLabel_RecipeVision_Min.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.baseLabel_RecipeVision_Min.Name = "baseLabel_RecipeVision_Min";
            this.baseLabel_RecipeVision_Min.Size = new System.Drawing.Size(15, 15);
            this.baseLabel_RecipeVision_Min.TabIndex = 31;
            this.baseLabel_RecipeVision_Min.Text = "0";
            // 
            // hScrollBar_RecipeVision_Illuminator
            // 
            this.hScrollBar_RecipeVision_Illuminator.Location = new System.Drawing.Point(33, 42);
            this.hScrollBar_RecipeVision_Illuminator.Name = "hScrollBar_RecipeVision_Illuminator";
            this.hScrollBar_RecipeVision_Illuminator.Size = new System.Drawing.Size(126, 26);
            this.hScrollBar_RecipeVision_Illuminator.TabIndex = 30;
            // 
            // button_RecipeVision_Illumin_value
            // 
            this.button_RecipeVision_Illumin_value.Font = new System.Drawing.Font("Tahoma", 10F);
            this.button_RecipeVision_Illumin_value.Location = new System.Drawing.Point(164, 17);
            this.button_RecipeVision_Illumin_value.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_RecipeVision_Illumin_value.Name = "button_RecipeVision_Illumin_value";
            this.button_RecipeVision_Illumin_value.Size = new System.Drawing.Size(28, 18);
            this.button_RecipeVision_Illumin_value.TabIndex = 28;
            this.button_RecipeVision_Illumin_value.Text = "#";
            this.button_RecipeVision_Illumin_value.UseVisualStyleBackColor = true;
            // 
            // textBox_RecipeVision_IlluminationValue
            // 
            this.textBox_RecipeVision_IlluminationValue.Font = new System.Drawing.Font("Tahoma", 10F);
            this.textBox_RecipeVision_IlluminationValue.Location = new System.Drawing.Point(90, 18);
            this.textBox_RecipeVision_IlluminationValue.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBox_RecipeVision_IlluminationValue.Name = "textBox_RecipeVision_IlluminationValue";
            this.textBox_RecipeVision_IlluminationValue.Size = new System.Drawing.Size(71, 24);
            this.textBox_RecipeVision_IlluminationValue.TabIndex = 27;
            this.textBox_RecipeVision_IlluminationValue.Text = "000";
            this.textBox_RecipeVision_IlluminationValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button_RecipeVision_Inspect
            // 
            this.button_RecipeVision_Inspect.Location = new System.Drawing.Point(312, 432);
            this.button_RecipeVision_Inspect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_RecipeVision_Inspect.Name = "button_RecipeVision_Inspect";
            this.button_RecipeVision_Inspect.Size = new System.Drawing.Size(134, 34);
            this.button_RecipeVision_Inspect.TabIndex = 192;
            this.button_RecipeVision_Inspect.Text = "Inspect";
            this.button_RecipeVision_Inspect.UseVisualStyleBackColor = true;
            this.button_RecipeVision_Inspect.Click += new System.EventHandler(this.button_RecipeVision_Inspect_Click);
            // 
            // button_RecipeVision_Train
            // 
            this.button_RecipeVision_Train.Location = new System.Drawing.Point(312, 391);
            this.button_RecipeVision_Train.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_RecipeVision_Train.Name = "button_RecipeVision_Train";
            this.button_RecipeVision_Train.Size = new System.Drawing.Size(134, 37);
            this.button_RecipeVision_Train.TabIndex = 191;
            this.button_RecipeVision_Train.Text = "Train";
            this.button_RecipeVision_Train.UseVisualStyleBackColor = true;
            this.button_RecipeVision_Train.Click += new System.EventHandler(this.button_RecipeVision_Train_Click);
            // 
            // groupBox_RecipeVision_MarkMatching
            // 
            this.groupBox_RecipeVision_MarkMatching.Controls.Add(this.radioButton_RecipeVision_Blob);
            this.groupBox_RecipeVision_MarkMatching.Controls.Add(this.radioButton_RecipeVision_Pattern);
            this.groupBox_RecipeVision_MarkMatching.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox_RecipeVision_MarkMatching.Location = new System.Drawing.Point(312, 351);
            this.groupBox_RecipeVision_MarkMatching.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_RecipeVision_MarkMatching.Name = "groupBox_RecipeVision_MarkMatching";
            this.groupBox_RecipeVision_MarkMatching.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox_RecipeVision_MarkMatching.Size = new System.Drawing.Size(134, 37);
            this.groupBox_RecipeVision_MarkMatching.TabIndex = 190;
            this.groupBox_RecipeVision_MarkMatching.TabStop = false;
            this.groupBox_RecipeVision_MarkMatching.Text = "MarkMatching";
            // 
            // radioButton_RecipeVision_Blob
            // 
            this.radioButton_RecipeVision_Blob.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_RecipeVision_Blob.Location = new System.Drawing.Point(66, 17);
            this.radioButton_RecipeVision_Blob.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_RecipeVision_Blob.Name = "radioButton_RecipeVision_Blob";
            this.radioButton_RecipeVision_Blob.Size = new System.Drawing.Size(60, 18);
            this.radioButton_RecipeVision_Blob.TabIndex = 6;
            this.radioButton_RecipeVision_Blob.Text = "Blob";
            this.radioButton_RecipeVision_Blob.UseVisualStyleBackColor = true;
            this.radioButton_RecipeVision_Blob.CheckedChanged += new System.EventHandler(this.radioButton_RecipeVision_Blob_CheckedChanged);
            // 
            // radioButton_RecipeVision_Pattern
            // 
            this.radioButton_RecipeVision_Pattern.Checked = true;
            this.radioButton_RecipeVision_Pattern.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_RecipeVision_Pattern.Location = new System.Drawing.Point(9, 17);
            this.radioButton_RecipeVision_Pattern.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_RecipeVision_Pattern.Name = "radioButton_RecipeVision_Pattern";
            this.radioButton_RecipeVision_Pattern.Size = new System.Drawing.Size(60, 18);
            this.radioButton_RecipeVision_Pattern.TabIndex = 5;
            this.radioButton_RecipeVision_Pattern.TabStop = true;
            this.radioButton_RecipeVision_Pattern.Text = "Pattern";
            this.radioButton_RecipeVision_Pattern.UseVisualStyleBackColor = true;
            this.radioButton_RecipeVision_Pattern.CheckedChanged += new System.EventHandler(this.radioButton_RecipeVision_Pattern_CheckedChanged);
            // 
            // groupBox_RecipeVision_MarkType
            // 
            this.groupBox_RecipeVision_MarkType.Controls.Add(this.radioButton_RecipeVision_Circle);
            this.groupBox_RecipeVision_MarkType.Controls.Add(this.radioButton_RecipeVision_Cross);
            this.groupBox_RecipeVision_MarkType.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBox_RecipeVision_MarkType.Location = new System.Drawing.Point(312, 308);
            this.groupBox_RecipeVision_MarkType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox_RecipeVision_MarkType.Name = "groupBox_RecipeVision_MarkType";
            this.groupBox_RecipeVision_MarkType.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox_RecipeVision_MarkType.Size = new System.Drawing.Size(134, 41);
            this.groupBox_RecipeVision_MarkType.TabIndex = 189;
            this.groupBox_RecipeVision_MarkType.TabStop = false;
            this.groupBox_RecipeVision_MarkType.Text = "MarkType";
            // 
            // radioButton_RecipeVision_Circle
            // 
            this.radioButton_RecipeVision_Circle.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_RecipeVision_Circle.Location = new System.Drawing.Point(66, 17);
            this.radioButton_RecipeVision_Circle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_RecipeVision_Circle.Name = "radioButton_RecipeVision_Circle";
            this.radioButton_RecipeVision_Circle.Size = new System.Drawing.Size(60, 18);
            this.radioButton_RecipeVision_Circle.TabIndex = 6;
            this.radioButton_RecipeVision_Circle.Text = "Circle";
            this.radioButton_RecipeVision_Circle.UseVisualStyleBackColor = true;
            this.radioButton_RecipeVision_Circle.CheckedChanged += new System.EventHandler(this.radioButton_RecipeVision_Circle_CheckedChanged);
            // 
            // radioButton_RecipeVision_Cross
            // 
            this.radioButton_RecipeVision_Cross.Checked = true;
            this.radioButton_RecipeVision_Cross.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_RecipeVision_Cross.Location = new System.Drawing.Point(9, 17);
            this.radioButton_RecipeVision_Cross.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_RecipeVision_Cross.Name = "radioButton_RecipeVision_Cross";
            this.radioButton_RecipeVision_Cross.Size = new System.Drawing.Size(60, 18);
            this.radioButton_RecipeVision_Cross.TabIndex = 5;
            this.radioButton_RecipeVision_Cross.TabStop = true;
            this.radioButton_RecipeVision_Cross.Text = "Cross";
            this.radioButton_RecipeVision_Cross.UseVisualStyleBackColor = true;
            this.radioButton_RecipeVision_Cross.CheckedChanged += new System.EventHandler(this.radioButton_RecipeVision_Cross_CheckedChanged);
            // 
            // groupBox_RecipeVision_SearchResult
            // 
            this.groupBox_RecipeVision_SearchResult.BorderColor = System.Drawing.Color.Black;
            this.groupBox_RecipeVision_SearchResult.Controls.Add(this.tabControl_RecipeVision_SearchResult);
            this.groupBox_RecipeVision_SearchResult.Controls.Add(this.tabControl1);
            this.groupBox_RecipeVision_SearchResult.Font = new System.Drawing.Font("Tahoma", 9F);
            this.groupBox_RecipeVision_SearchResult.ForeColor = System.Drawing.Color.Black;
            this.groupBox_RecipeVision_SearchResult.Location = new System.Drawing.Point(606, 363);
            this.groupBox_RecipeVision_SearchResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_RecipeVision_SearchResult.Name = "groupBox_RecipeVision_SearchResult";
            this.groupBox_RecipeVision_SearchResult.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_RecipeVision_SearchResult.Size = new System.Drawing.Size(214, 102);
            this.groupBox_RecipeVision_SearchResult.TabIndex = 194;
            this.groupBox_RecipeVision_SearchResult.TabStop = false;
            this.groupBox_RecipeVision_SearchResult.Text = " Search Result ";
            // 
            // tabControl_RecipeVision_SearchResult
            // 
            this.tabControl_RecipeVision_SearchResult.Controls.Add(this.Parameter);
            this.tabControl_RecipeVision_SearchResult.Controls.Add(this.Result);
            this.tabControl_RecipeVision_SearchResult.Location = new System.Drawing.Point(4, 11);
            this.tabControl_RecipeVision_SearchResult.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tabControl_RecipeVision_SearchResult.Name = "tabControl_RecipeVision_SearchResult";
            this.tabControl_RecipeVision_SearchResult.SelectedIndex = 0;
            this.tabControl_RecipeVision_SearchResult.Size = new System.Drawing.Size(206, 89);
            this.tabControl_RecipeVision_SearchResult.TabIndex = 167;
            // 
            // Parameter
            // 
            this.Parameter.Controls.Add(this.basetextBox_RecipeVision_MinScore);
            this.Parameter.Controls.Add(this.baseLabel_RecipeVision_AngleTolerance);
            this.Parameter.Controls.Add(this.basetextBox_RecipeVision_MaxInstance);
            this.Parameter.Controls.Add(this.baseToggleButton_RecipeVision_DuplicateCheck);
            this.Parameter.Controls.Add(this.basetextBox_RecipeVision_AngleTolerance);
            this.Parameter.Controls.Add(this.baseToggleButton_RecipeVision_UseMaskImage);
            this.Parameter.Controls.Add(this.baseLabel_RecipeVision_MinScore);
            this.Parameter.Controls.Add(this.baseLabel_RecipeVision_MaxInstance);
            this.Parameter.Location = new System.Drawing.Point(4, 23);
            this.Parameter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Parameter.Name = "Parameter";
            this.Parameter.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Parameter.Size = new System.Drawing.Size(198, 62);
            this.Parameter.TabIndex = 0;
            this.Parameter.Text = "Parameter";
            this.Parameter.UseVisualStyleBackColor = true;
            // 
            // basetextBox_RecipeVision_MinScore
            // 
            this.basetextBox_RecipeVision_MinScore.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.basetextBox_RecipeVision_MinScore.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.basetextBox_RecipeVision_MinScore.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.basetextBox_RecipeVision_MinScore.ForeColor = System.Drawing.Color.White;
            this.basetextBox_RecipeVision_MinScore.Location = new System.Drawing.Point(130, 29);
            this.basetextBox_RecipeVision_MinScore.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.basetextBox_RecipeVision_MinScore.Name = "basetextBox_RecipeVision_MinScore";
            this.basetextBox_RecipeVision_MinScore.Size = new System.Drawing.Size(62, 13);
            this.basetextBox_RecipeVision_MinScore.TabIndex = 7;
            // 
            // baseLabel_RecipeVision_AngleTolerance
            // 
            this.baseLabel_RecipeVision_AngleTolerance.AutoSize = true;
            this.baseLabel_RecipeVision_AngleTolerance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel_RecipeVision_AngleTolerance.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_RecipeVision_AngleTolerance.Location = new System.Drawing.Point(4, 10);
            this.baseLabel_RecipeVision_AngleTolerance.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.baseLabel_RecipeVision_AngleTolerance.Name = "baseLabel_RecipeVision_AngleTolerance";
            this.baseLabel_RecipeVision_AngleTolerance.Size = new System.Drawing.Size(139, 13);
            this.baseLabel_RecipeVision_AngleTolerance.TabIndex = 2;
            this.baseLabel_RecipeVision_AngleTolerance.Text = "Angle Tolerance [Deg] :";
            // 
            // basetextBox_RecipeVision_MaxInstance
            // 
            this.basetextBox_RecipeVision_MaxInstance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.basetextBox_RecipeVision_MaxInstance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.basetextBox_RecipeVision_MaxInstance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.basetextBox_RecipeVision_MaxInstance.ForeColor = System.Drawing.Color.White;
            this.basetextBox_RecipeVision_MaxInstance.Location = new System.Drawing.Point(130, 19);
            this.basetextBox_RecipeVision_MaxInstance.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.basetextBox_RecipeVision_MaxInstance.Name = "basetextBox_RecipeVision_MaxInstance";
            this.basetextBox_RecipeVision_MaxInstance.Size = new System.Drawing.Size(62, 13);
            this.basetextBox_RecipeVision_MaxInstance.TabIndex = 6;
            // 
            // baseToggleButton_RecipeVision_DuplicateCheck
            // 
            this.baseToggleButton_RecipeVision_DuplicateCheck.BackColor = System.Drawing.Color.White;
            this.baseToggleButton_RecipeVision_DuplicateCheck.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseToggleButton_RecipeVision_DuplicateCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton_RecipeVision_DuplicateCheck.ForeColor = System.Drawing.Color.Black;
            this.baseToggleButton_RecipeVision_DuplicateCheck.Location = new System.Drawing.Point(59, 45);
            this.baseToggleButton_RecipeVision_DuplicateCheck.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.baseToggleButton_RecipeVision_DuplicateCheck.Name = "baseToggleButton_RecipeVision_DuplicateCheck";
            this.baseToggleButton_RecipeVision_DuplicateCheck.Size = new System.Drawing.Size(62, 16);
            this.baseToggleButton_RecipeVision_DuplicateCheck.TabIndex = 0;
            this.baseToggleButton_RecipeVision_DuplicateCheck.Text = "Duplicate Check";
            this.baseToggleButton_RecipeVision_DuplicateCheck.UseVisualStyleBackColor = false;
            this.baseToggleButton_RecipeVision_DuplicateCheck.Click += new System.EventHandler(this.baseToggleButton_RecipeVision_DuplicateCheck_Click);
            // 
            // basetextBox_RecipeVision_AngleTolerance
            // 
            this.basetextBox_RecipeVision_AngleTolerance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.basetextBox_RecipeVision_AngleTolerance.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.basetextBox_RecipeVision_AngleTolerance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.basetextBox_RecipeVision_AngleTolerance.ForeColor = System.Drawing.Color.White;
            this.basetextBox_RecipeVision_AngleTolerance.Location = new System.Drawing.Point(130, 9);
            this.basetextBox_RecipeVision_AngleTolerance.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.basetextBox_RecipeVision_AngleTolerance.Name = "basetextBox_RecipeVision_AngleTolerance";
            this.basetextBox_RecipeVision_AngleTolerance.Size = new System.Drawing.Size(62, 13);
            this.basetextBox_RecipeVision_AngleTolerance.TabIndex = 5;
            // 
            // baseToggleButton_RecipeVision_UseMaskImage
            // 
            this.baseToggleButton_RecipeVision_UseMaskImage.BackColor = System.Drawing.Color.White;
            this.baseToggleButton_RecipeVision_UseMaskImage.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.baseToggleButton_RecipeVision_UseMaskImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseToggleButton_RecipeVision_UseMaskImage.ForeColor = System.Drawing.Color.Black;
            this.baseToggleButton_RecipeVision_UseMaskImage.Location = new System.Drawing.Point(130, 45);
            this.baseToggleButton_RecipeVision_UseMaskImage.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.baseToggleButton_RecipeVision_UseMaskImage.Name = "baseToggleButton_RecipeVision_UseMaskImage";
            this.baseToggleButton_RecipeVision_UseMaskImage.Size = new System.Drawing.Size(62, 16);
            this.baseToggleButton_RecipeVision_UseMaskImage.TabIndex = 1;
            this.baseToggleButton_RecipeVision_UseMaskImage.Text = "Use Mask Image";
            this.baseToggleButton_RecipeVision_UseMaskImage.UseVisualStyleBackColor = false;
            this.baseToggleButton_RecipeVision_UseMaskImage.Click += new System.EventHandler(this.baseToggleButton_RecipeVision_UseMaskImage_Click);
            // 
            // baseLabel_RecipeVision_MinScore
            // 
            this.baseLabel_RecipeVision_MinScore.AutoSize = true;
            this.baseLabel_RecipeVision_MinScore.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel_RecipeVision_MinScore.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_RecipeVision_MinScore.Location = new System.Drawing.Point(56, 30);
            this.baseLabel_RecipeVision_MinScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.baseLabel_RecipeVision_MinScore.Name = "baseLabel_RecipeVision_MinScore";
            this.baseLabel_RecipeVision_MinScore.Size = new System.Drawing.Size(65, 13);
            this.baseLabel_RecipeVision_MinScore.TabIndex = 4;
            this.baseLabel_RecipeVision_MinScore.Text = "MinScore :";
            // 
            // baseLabel_RecipeVision_MaxInstance
            // 
            this.baseLabel_RecipeVision_MaxInstance.AutoSize = true;
            this.baseLabel_RecipeVision_MaxInstance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel_RecipeVision_MaxInstance.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_RecipeVision_MaxInstance.Location = new System.Drawing.Point(20, 20);
            this.baseLabel_RecipeVision_MaxInstance.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.baseLabel_RecipeVision_MaxInstance.Name = "baseLabel_RecipeVision_MaxInstance";
            this.baseLabel_RecipeVision_MaxInstance.Size = new System.Drawing.Size(117, 13);
            this.baseLabel_RecipeVision_MaxInstance.TabIndex = 3;
            this.baseLabel_RecipeVision_MaxInstance.Text = "Max Instance [EA] :";
            // 
            // Result
            // 
            this.Result.Controls.Add(this.button_RecipeVision_Search);
            this.Result.Controls.Add(this.radioButton_RecipeVision_mm);
            this.Result.Controls.Add(this.radioButton_RecipeVision_Pixel);
            this.Result.Controls.Add(this.baseTextBox_RecipeVision_PositionT);
            this.Result.Controls.Add(this.baseTextBox_RecipeVision_PositionY);
            this.Result.Controls.Add(this.baseLabel_RecipeVision_PositionT);
            this.Result.Controls.Add(this.baseLabel_RecipeVision_PositionY);
            this.Result.Controls.Add(this.baseLabel_RecipeVision_PositionX);
            this.Result.Controls.Add(this.baseTextBox_RecipeVision_PositionX);
            this.Result.Location = new System.Drawing.Point(4, 23);
            this.Result.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Result.Name = "Result";
            this.Result.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Result.Size = new System.Drawing.Size(198, 62);
            this.Result.TabIndex = 1;
            this.Result.Text = "Result";
            this.Result.UseVisualStyleBackColor = true;
            // 
            // button_RecipeVision_Search
            // 
            this.button_RecipeVision_Search.Location = new System.Drawing.Point(115, 48);
            this.button_RecipeVision_Search.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_RecipeVision_Search.Name = "button_RecipeVision_Search";
            this.button_RecipeVision_Search.Size = new System.Drawing.Size(81, 17);
            this.button_RecipeVision_Search.TabIndex = 14;
            this.button_RecipeVision_Search.Text = "Search";
            this.button_RecipeVision_Search.UseVisualStyleBackColor = true;
            this.button_RecipeVision_Search.Click += new System.EventHandler(this.button_RecipeVision_Search_Click);
            // 
            // radioButton_RecipeVision_mm
            // 
            this.radioButton_RecipeVision_mm.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_RecipeVision_mm.Location = new System.Drawing.Point(125, 29);
            this.radioButton_RecipeVision_mm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_RecipeVision_mm.Name = "radioButton_RecipeVision_mm";
            this.radioButton_RecipeVision_mm.Size = new System.Drawing.Size(60, 18);
            this.radioButton_RecipeVision_mm.TabIndex = 13;
            this.radioButton_RecipeVision_mm.Text = "mm";
            this.radioButton_RecipeVision_mm.UseVisualStyleBackColor = true;
            this.radioButton_RecipeVision_mm.CheckedChanged += new System.EventHandler(this.radioButton_RecipeVision_mm_CheckedChanged);
            // 
            // radioButton_RecipeVision_Pixel
            // 
            this.radioButton_RecipeVision_Pixel.Checked = true;
            this.radioButton_RecipeVision_Pixel.Font = new System.Drawing.Font("Tahoma", 10F);
            this.radioButton_RecipeVision_Pixel.Location = new System.Drawing.Point(125, 9);
            this.radioButton_RecipeVision_Pixel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.radioButton_RecipeVision_Pixel.Name = "radioButton_RecipeVision_Pixel";
            this.radioButton_RecipeVision_Pixel.Size = new System.Drawing.Size(60, 18);
            this.radioButton_RecipeVision_Pixel.TabIndex = 12;
            this.radioButton_RecipeVision_Pixel.TabStop = true;
            this.radioButton_RecipeVision_Pixel.Text = "Pixel";
            this.radioButton_RecipeVision_Pixel.UseVisualStyleBackColor = true;
            this.radioButton_RecipeVision_Pixel.CheckedChanged += new System.EventHandler(this.radioButton_RecipeVision_Pixel_CheckedChanged);
            // 
            // baseTextBox_RecipeVision_PositionT
            // 
            this.baseTextBox_RecipeVision_PositionT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBox_RecipeVision_PositionT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBox_RecipeVision_PositionT.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBox_RecipeVision_PositionT.ForeColor = System.Drawing.Color.White;
            this.baseTextBox_RecipeVision_PositionT.Location = new System.Drawing.Point(54, 36);
            this.baseTextBox_RecipeVision_PositionT.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.baseTextBox_RecipeVision_PositionT.Name = "baseTextBox_RecipeVision_PositionT";
            this.baseTextBox_RecipeVision_PositionT.Size = new System.Drawing.Size(62, 13);
            this.baseTextBox_RecipeVision_PositionT.TabIndex = 11;
            // 
            // baseTextBox_RecipeVision_PositionY
            // 
            this.baseTextBox_RecipeVision_PositionY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBox_RecipeVision_PositionY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBox_RecipeVision_PositionY.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBox_RecipeVision_PositionY.ForeColor = System.Drawing.Color.White;
            this.baseTextBox_RecipeVision_PositionY.Location = new System.Drawing.Point(54, 21);
            this.baseTextBox_RecipeVision_PositionY.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.baseTextBox_RecipeVision_PositionY.Name = "baseTextBox_RecipeVision_PositionY";
            this.baseTextBox_RecipeVision_PositionY.Size = new System.Drawing.Size(62, 13);
            this.baseTextBox_RecipeVision_PositionY.TabIndex = 10;
            // 
            // baseLabel_RecipeVision_PositionT
            // 
            this.baseLabel_RecipeVision_PositionT.AutoSize = true;
            this.baseLabel_RecipeVision_PositionT.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel_RecipeVision_PositionT.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_RecipeVision_PositionT.Location = new System.Drawing.Point(4, 36);
            this.baseLabel_RecipeVision_PositionT.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.baseLabel_RecipeVision_PositionT.Name = "baseLabel_RecipeVision_PositionT";
            this.baseLabel_RecipeVision_PositionT.Size = new System.Drawing.Size(65, 13);
            this.baseLabel_RecipeVision_PositionT.TabIndex = 9;
            this.baseLabel_RecipeVision_PositionT.Text = "PositionT :";
            // 
            // baseLabel_RecipeVision_PositionY
            // 
            this.baseLabel_RecipeVision_PositionY.AutoSize = true;
            this.baseLabel_RecipeVision_PositionY.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel_RecipeVision_PositionY.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_RecipeVision_PositionY.Location = new System.Drawing.Point(4, 21);
            this.baseLabel_RecipeVision_PositionY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.baseLabel_RecipeVision_PositionY.Name = "baseLabel_RecipeVision_PositionY";
            this.baseLabel_RecipeVision_PositionY.Size = new System.Drawing.Size(65, 13);
            this.baseLabel_RecipeVision_PositionY.TabIndex = 8;
            this.baseLabel_RecipeVision_PositionY.Text = "PositionY :";
            // 
            // baseLabel_RecipeVision_PositionX
            // 
            this.baseLabel_RecipeVision_PositionX.AutoSize = true;
            this.baseLabel_RecipeVision_PositionX.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.baseLabel_RecipeVision_PositionX.ForeColor = System.Drawing.Color.Black;
            this.baseLabel_RecipeVision_PositionX.Location = new System.Drawing.Point(4, 9);
            this.baseLabel_RecipeVision_PositionX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.baseLabel_RecipeVision_PositionX.Name = "baseLabel_RecipeVision_PositionX";
            this.baseLabel_RecipeVision_PositionX.Size = new System.Drawing.Size(65, 13);
            this.baseLabel_RecipeVision_PositionX.TabIndex = 6;
            this.baseLabel_RecipeVision_PositionX.Text = "PositionX :";
            // 
            // baseTextBox_RecipeVision_PositionX
            // 
            this.baseTextBox_RecipeVision_PositionX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseTextBox_RecipeVision_PositionX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.baseTextBox_RecipeVision_PositionX.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseTextBox_RecipeVision_PositionX.ForeColor = System.Drawing.Color.White;
            this.baseTextBox_RecipeVision_PositionX.Location = new System.Drawing.Point(54, 9);
            this.baseTextBox_RecipeVision_PositionX.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.baseTextBox_RecipeVision_PositionX.Name = "baseTextBox_RecipeVision_PositionX";
            this.baseTextBox_RecipeVision_PositionX.Size = new System.Drawing.Size(62, 13);
            this.baseTextBox_RecipeVision_PositionX.TabIndex = 7;
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(479, 65);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(169, 217);
            this.tabControl1.TabIndex = 18;
            // 
            // groupBox_RecipeVision_TrainImage
            // 
            this.groupBox_RecipeVision_TrainImage.BorderColor = System.Drawing.Color.Black;
            this.groupBox_RecipeVision_TrainImage.Controls.Add(this.pictureBox_RecipeVision_TrainImage);
            this.groupBox_RecipeVision_TrainImage.Controls.Add(this.button_RecipeVision_Train_Set);
            this.groupBox_RecipeVision_TrainImage.Font = new System.Drawing.Font("Tahoma", 9F);
            this.groupBox_RecipeVision_TrainImage.ForeColor = System.Drawing.Color.Black;
            this.groupBox_RecipeVision_TrainImage.Location = new System.Drawing.Point(452, 289);
            this.groupBox_RecipeVision_TrainImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_RecipeVision_TrainImage.Name = "groupBox_RecipeVision_TrainImage";
            this.groupBox_RecipeVision_TrainImage.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox_RecipeVision_TrainImage.Size = new System.Drawing.Size(146, 177);
            this.groupBox_RecipeVision_TrainImage.TabIndex = 193;
            this.groupBox_RecipeVision_TrainImage.TabStop = false;
            this.groupBox_RecipeVision_TrainImage.Text = " Train Image ";
            // 
            // pictureBox_RecipeVision_TrainImage
            // 
            this.pictureBox_RecipeVision_TrainImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.pictureBox_RecipeVision_TrainImage.Location = new System.Drawing.Point(7, 14);
            this.pictureBox_RecipeVision_TrainImage.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_RecipeVision_TrainImage.Name = "pictureBox_RecipeVision_TrainImage";
            this.pictureBox_RecipeVision_TrainImage.Size = new System.Drawing.Size(130, 120);
            this.pictureBox_RecipeVision_TrainImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_RecipeVision_TrainImage.TabIndex = 1;
            this.pictureBox_RecipeVision_TrainImage.TabStop = false;
            // 
            // button_RecipeVision_Train_Set
            // 
            this.button_RecipeVision_Train_Set.BackColor = System.Drawing.Color.White;
            this.button_RecipeVision_Train_Set.FlatAppearance.BorderColor = System.Drawing.Color.Aqua;
            this.button_RecipeVision_Train_Set.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_RecipeVision_Train_Set.Font = new System.Drawing.Font("Tahoma", 9F);
            this.button_RecipeVision_Train_Set.ForeColor = System.Drawing.Color.Black;
            this.button_RecipeVision_Train_Set.Location = new System.Drawing.Point(10, 141);
            this.button_RecipeVision_Train_Set.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button_RecipeVision_Train_Set.Name = "button_RecipeVision_Train_Set";
            this.button_RecipeVision_Train_Set.Size = new System.Drawing.Size(126, 29);
            this.button_RecipeVision_Train_Set.TabIndex = 0;
            this.button_RecipeVision_Train_Set.Text = "Train";
            this.button_RecipeVision_Train_Set.UseVisualStyleBackColor = false;
            this.button_RecipeVision_Train_Set.Click += new System.EventHandler(this.button_RecipeVision_Train_Set_Click);
            // 
            // ImageViewer_RecipeVision_Rows
            // 
            this.ImageViewer_RecipeVision_Rows.BackColor = System.Drawing.Color.Black;
            this.ImageViewer_RecipeVision_Rows.Camera = null;
            this.ImageViewer_RecipeVision_Rows.CameraSwitch = null;
            this.ImageViewer_RecipeVision_Rows.FrameRate = 1D;
            this.ImageViewer_RecipeVision_Rows.InputImage = null;
            this.ImageViewer_RecipeVision_Rows.IsViewCustomizedImage = false;
            this.ImageViewer_RecipeVision_Rows.Location = new System.Drawing.Point(1, 235);
            this.ImageViewer_RecipeVision_Rows.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ImageViewer_RecipeVision_Rows.Name = "ImageViewer_RecipeVision_Rows";
            this.ImageViewer_RecipeVision_Rows.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.ImageViewer_RecipeVision_Rows.Simulated = false;
            this.ImageViewer_RecipeVision_Rows.Size = new System.Drawing.Size(301, 227);
            this.ImageViewer_RecipeVision_Rows.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageViewer_RecipeVision_Rows.TabIndex = 186;
            this.ImageViewer_RecipeVision_Rows.TabStop = false;
            this.ImageViewer_RecipeVision_Rows.UpdateDelayTime = 160;
            this.ImageViewer_RecipeVision_Rows.VisibleCrossLine = true;
            // 
            // ImageViewer_RecipeVision_highs
            // 
            this.ImageViewer_RecipeVision_highs.BackColor = System.Drawing.Color.Black;
            this.ImageViewer_RecipeVision_highs.Camera = null;
            this.ImageViewer_RecipeVision_highs.CameraSwitch = null;
            this.ImageViewer_RecipeVision_highs.FrameRate = 1D;
            this.ImageViewer_RecipeVision_highs.InputImage = null;
            this.ImageViewer_RecipeVision_highs.IsViewCustomizedImage = false;
            this.ImageViewer_RecipeVision_highs.Location = new System.Drawing.Point(1, 5);
            this.ImageViewer_RecipeVision_highs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ImageViewer_RecipeVision_highs.Name = "ImageViewer_RecipeVision_highs";
            this.ImageViewer_RecipeVision_highs.OperatingType = QMC.Common.Hmi.VisionImageViewer.OperatingTypes.Center;
            this.ImageViewer_RecipeVision_highs.Simulated = false;
            this.ImageViewer_RecipeVision_highs.Size = new System.Drawing.Size(301, 227);
            this.ImageViewer_RecipeVision_highs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageViewer_RecipeVision_highs.TabIndex = 185;
            this.ImageViewer_RecipeVision_highs.TabStop = false;
            this.ImageViewer_RecipeVision_highs.UpdateDelayTime = 160;
            this.ImageViewer_RecipeVision_highs.VisibleCrossLine = true;
            // 
            // FormNewSub_Recipe_Vision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_RecipeVision_CameraStop);
            this.Controls.Add(this.checkBox_RecipeVision_CirclePos);
            this.Controls.Add(this.label_RecipeVision_CirclePosY);
            this.Controls.Add(this.button_RecipeVision_CameraLive);
            this.Controls.Add(this.label_RecipeVision_CirclePosX);
            this.Controls.Add(this.button_RecipeVision_Vision_Save);
            this.Controls.Add(this.groupBox_RecipeVision_Illumination);
            this.Controls.Add(this.button_RecipeVision_Inspect);
            this.Controls.Add(this.button_RecipeVision_Train);
            this.Controls.Add(this.groupBox_RecipeVision_MarkMatching);
            this.Controls.Add(this.groupBox_RecipeVision_MarkType);
            this.Controls.Add(this.groupBox_RecipeVision_SearchResult);
            this.Controls.Add(this.groupBox_RecipeVision_TrainImage);
            this.Controls.Add(this.ImageViewer_RecipeVision_Rows);
            this.Controls.Add(this.ImageViewer_RecipeVision_highs);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormNewSub_Recipe_Vision";
            this.Size = new System.Drawing.Size(1200, 470);
            this.groupBox_RecipeVision_Illumination.ResumeLayout(false);
            this.groupBox_RecipeVision_Illumination.PerformLayout();
            this.groupBox_RecipeVision_MarkMatching.ResumeLayout(false);
            this.groupBox_RecipeVision_MarkType.ResumeLayout(false);
            this.groupBox_RecipeVision_SearchResult.ResumeLayout(false);
            this.tabControl_RecipeVision_SearchResult.ResumeLayout(false);
            this.Parameter.ResumeLayout(false);
            this.Parameter.PerformLayout();
            this.Result.ResumeLayout(false);
            this.Result.PerformLayout();
            this.groupBox_RecipeVision_TrainImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_RecipeVision_TrainImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer_RecipeVision_Rows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageViewer_RecipeVision_highs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_RecipeVision_CameraStop;
        private System.Windows.Forms.CheckBox checkBox_RecipeVision_CirclePos;
        private System.Windows.Forms.Label label_RecipeVision_CirclePosY;
        private System.Windows.Forms.Button button_RecipeVision_CameraLive;
        private System.Windows.Forms.Label label_RecipeVision_CirclePosX;
        private System.Windows.Forms.Button button_RecipeVision_Vision_Save;
        private System.Windows.Forms.GroupBox groupBox_RecipeVision_Illumination;
        private System.Windows.Forms.RadioButton radioButton_RecipeVision_Light_Red;
        private System.Windows.Forms.RadioButton radioButton_RecipeVision_Light_IR;
        private SLD200_MSL.BaseLabel baseLabel_RecipeVision_Max;
        private SLD200_MSL.BaseLabel baseLabel_RecipeVision_Min;
        private System.Windows.Forms.HScrollBar hScrollBar_RecipeVision_Illuminator;
        private System.Windows.Forms.Button button_RecipeVision_Illumin_value;
        private System.Windows.Forms.TextBox textBox_RecipeVision_IlluminationValue;
        private System.Windows.Forms.Button button_RecipeVision_Inspect;
        private System.Windows.Forms.Button button_RecipeVision_Train;
        private System.Windows.Forms.GroupBox groupBox_RecipeVision_MarkMatching;
        private System.Windows.Forms.RadioButton radioButton_RecipeVision_Blob;
        private System.Windows.Forms.RadioButton radioButton_RecipeVision_Pattern;
        private System.Windows.Forms.GroupBox groupBox_RecipeVision_MarkType;
        private System.Windows.Forms.RadioButton radioButton_RecipeVision_Circle;
        private System.Windows.Forms.RadioButton radioButton_RecipeVision_Cross;
        private SLD200_MSL.WATGroupBox groupBox_RecipeVision_SearchResult;
        private System.Windows.Forms.TabControl tabControl_RecipeVision_SearchResult;
        private System.Windows.Forms.TabPage Parameter;
        private SLD200_MSL.BaseTextBox basetextBox_RecipeVision_MinScore;
        private SLD200_MSL.BaseLabel baseLabel_RecipeVision_AngleTolerance;
        private SLD200_MSL.BaseTextBox basetextBox_RecipeVision_MaxInstance;
        private SLD200_MSL.BaseToggleButton baseToggleButton_RecipeVision_DuplicateCheck;
        private SLD200_MSL.BaseTextBox basetextBox_RecipeVision_AngleTolerance;
        private SLD200_MSL.BaseToggleButton baseToggleButton_RecipeVision_UseMaskImage;
        private SLD200_MSL.BaseLabel baseLabel_RecipeVision_MinScore;
        private SLD200_MSL.BaseLabel baseLabel_RecipeVision_MaxInstance;
        private System.Windows.Forms.TabPage Result;
        private System.Windows.Forms.Button button_RecipeVision_Search;
        private System.Windows.Forms.RadioButton radioButton_RecipeVision_mm;
        private System.Windows.Forms.RadioButton radioButton_RecipeVision_Pixel;
        private SLD200_MSL.BaseTextBox baseTextBox_RecipeVision_PositionT;
        private SLD200_MSL.BaseTextBox baseTextBox_RecipeVision_PositionY;
        private SLD200_MSL.BaseLabel baseLabel_RecipeVision_PositionT;
        private SLD200_MSL.BaseLabel baseLabel_RecipeVision_PositionY;
        private SLD200_MSL.BaseLabel baseLabel_RecipeVision_PositionX;
        private SLD200_MSL.BaseTextBox baseTextBox_RecipeVision_PositionX;
        private System.Windows.Forms.TabControl tabControl1;
        private SLD200_MSL.WATGroupBox groupBox_RecipeVision_TrainImage;
        private System.Windows.Forms.PictureBox pictureBox_RecipeVision_TrainImage;
        private SLD200_MSL.BaseButton button_RecipeVision_Train_Set;
        private QMC.Common.Hmi.VisionImageViewer ImageViewer_RecipeVision_Rows;
        private QMC.Common.Hmi.VisionImageViewer ImageViewer_RecipeVision_highs;
    }
}