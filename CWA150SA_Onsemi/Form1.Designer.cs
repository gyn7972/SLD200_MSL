
namespace CWA150SA_Onsemi300
{
    partial class Form1
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
            this.textBoxBoardCount = new System.Windows.Forms.TextBox();
            this.textBoxInputCount = new System.Windows.Forms.TextBox();
            this.textBoxOutputCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxReadIndex = new System.Windows.Forms.TextBox();
            this.textBoxTargetX = new System.Windows.Forms.TextBox();
            this.textBoxTargetY = new System.Windows.Forms.TextBox();
            this.buttonStartCamera = new System.Windows.Forms.Button();
            this.buttonStopCamera = new System.Windows.Forms.Button();
            this.propertyGridCamera = new System.Windows.Forms.PropertyGrid();
            this.buttonCrossLine = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.buttonIlOpen = new System.Windows.Forms.Button();
            this.buttonIlClose = new System.Windows.Forms.Button();
            this.textBoxVolume = new System.Windows.Forms.TextBox();
            this.buttonTurnOn = new System.Windows.Forms.Button();
            this.buttonSetBright = new System.Windows.Forms.Button();
            this.buttonLog = new System.Windows.Forms.Button();
            this.textBoxClass = new System.Windows.Forms.TextBox();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.textBoxMessage = new System.Windows.Forms.TextBox();
            this.buttonLogTest2 = new System.Windows.Forms.Button();
            this.buttonMaterialCreate = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxBoardCount
            // 
            this.textBoxBoardCount.Location = new System.Drawing.Point(362, 88);
            this.textBoxBoardCount.Name = "textBoxBoardCount";
            this.textBoxBoardCount.Size = new System.Drawing.Size(100, 21);
            this.textBoxBoardCount.TabIndex = 1;
            // 
            // textBoxInputCount
            // 
            this.textBoxInputCount.Location = new System.Drawing.Point(362, 132);
            this.textBoxInputCount.Name = "textBoxInputCount";
            this.textBoxInputCount.Size = new System.Drawing.Size(100, 21);
            this.textBoxInputCount.TabIndex = 2;
            // 
            // textBoxOutputCount
            // 
            this.textBoxOutputCount.Location = new System.Drawing.Point(362, 159);
            this.textBoxOutputCount.Name = "textBoxOutputCount";
            this.textBoxOutputCount.Size = new System.Drawing.Size(100, 21);
            this.textBoxOutputCount.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(271, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "Board Count";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(231, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Input Module Count";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 162);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Output Module Count";
            // 
            // textBoxReadIndex
            // 
            this.textBoxReadIndex.Location = new System.Drawing.Point(362, 230);
            this.textBoxReadIndex.Name = "textBoxReadIndex";
            this.textBoxReadIndex.Size = new System.Drawing.Size(100, 21);
            this.textBoxReadIndex.TabIndex = 8;
            // 
            // textBoxTargetX
            // 
            this.textBoxTargetX.Location = new System.Drawing.Point(534, 241);
            this.textBoxTargetX.Name = "textBoxTargetX";
            this.textBoxTargetX.Size = new System.Drawing.Size(100, 21);
            this.textBoxTargetX.TabIndex = 19;
            // 
            // textBoxTargetY
            // 
            this.textBoxTargetY.Location = new System.Drawing.Point(640, 241);
            this.textBoxTargetY.Name = "textBoxTargetY";
            this.textBoxTargetY.Size = new System.Drawing.Size(100, 21);
            this.textBoxTargetY.TabIndex = 20;
            // 
            // buttonStartCamera
            // 
            this.buttonStartCamera.Location = new System.Drawing.Point(895, 371);
            this.buttonStartCamera.Name = "buttonStartCamera";
            this.buttonStartCamera.Size = new System.Drawing.Size(118, 63);
            this.buttonStartCamera.TabIndex = 22;
            this.buttonStartCamera.Text = "Start";
            this.buttonStartCamera.UseVisualStyleBackColor = true;
            this.buttonStartCamera.Click += new System.EventHandler(this.buttonStartCamera_Click);
            // 
            // buttonStopCamera
            // 
            this.buttonStopCamera.Location = new System.Drawing.Point(1019, 371);
            this.buttonStopCamera.Name = "buttonStopCamera";
            this.buttonStopCamera.Size = new System.Drawing.Size(118, 63);
            this.buttonStopCamera.TabIndex = 23;
            this.buttonStopCamera.Text = "Stop";
            this.buttonStopCamera.UseVisualStyleBackColor = true;
            this.buttonStopCamera.Click += new System.EventHandler(this.buttonStopCamera_Click);
            // 
            // propertyGridCamera
            // 
            this.propertyGridCamera.Location = new System.Drawing.Point(949, 12);
            this.propertyGridCamera.Name = "propertyGridCamera";
            this.propertyGridCamera.Size = new System.Drawing.Size(286, 302);
            this.propertyGridCamera.TabIndex = 24;
            // 
            // buttonCrossLine
            // 
            this.buttonCrossLine.Location = new System.Drawing.Point(1160, 371);
            this.buttonCrossLine.Name = "buttonCrossLine";
            this.buttonCrossLine.Size = new System.Drawing.Size(75, 23);
            this.buttonCrossLine.TabIndex = 25;
            this.buttonCrossLine.Text = "CrossLine";
            this.buttonCrossLine.UseVisualStyleBackColor = true;
            this.buttonCrossLine.Click += new System.EventHandler(this.buttonCrossLine_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(779, 424);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(286, 343);
            this.propertyGrid1.TabIndex = 26;
            // 
            // buttonIlOpen
            // 
            this.buttonIlOpen.Location = new System.Drawing.Point(1128, 501);
            this.buttonIlOpen.Name = "buttonIlOpen";
            this.buttonIlOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonIlOpen.TabIndex = 27;
            this.buttonIlOpen.Text = "Open";
            this.buttonIlOpen.UseVisualStyleBackColor = true;
            this.buttonIlOpen.Click += new System.EventHandler(this.buttonIlOpen_Click);
            // 
            // buttonIlClose
            // 
            this.buttonIlClose.Location = new System.Drawing.Point(1128, 541);
            this.buttonIlClose.Name = "buttonIlClose";
            this.buttonIlClose.Size = new System.Drawing.Size(75, 23);
            this.buttonIlClose.TabIndex = 28;
            this.buttonIlClose.Text = "Close";
            this.buttonIlClose.UseVisualStyleBackColor = true;
            this.buttonIlClose.Click += new System.EventHandler(this.buttonIlClose_Click);
            // 
            // textBoxVolume
            // 
            this.textBoxVolume.Location = new System.Drawing.Point(1103, 645);
            this.textBoxVolume.Name = "textBoxVolume";
            this.textBoxVolume.Size = new System.Drawing.Size(100, 21);
            this.textBoxVolume.TabIndex = 29;
            // 
            // buttonTurnOn
            // 
            this.buttonTurnOn.Location = new System.Drawing.Point(1128, 616);
            this.buttonTurnOn.Name = "buttonTurnOn";
            this.buttonTurnOn.Size = new System.Drawing.Size(97, 23);
            this.buttonTurnOn.TabIndex = 30;
            this.buttonTurnOn.Text = "Turn On/Off";
            this.buttonTurnOn.UseVisualStyleBackColor = true;
            this.buttonTurnOn.Click += new System.EventHandler(this.buttonTurnOn_Click);
            // 
            // buttonSetBright
            // 
            this.buttonSetBright.Location = new System.Drawing.Point(1128, 672);
            this.buttonSetBright.Name = "buttonSetBright";
            this.buttonSetBright.Size = new System.Drawing.Size(97, 23);
            this.buttonSetBright.TabIndex = 31;
            this.buttonSetBright.Text = "Set Bright";
            this.buttonSetBright.UseVisualStyleBackColor = true;
            this.buttonSetBright.Click += new System.EventHandler(this.buttonSetBright_Click);
            // 
            // buttonLog
            // 
            this.buttonLog.Location = new System.Drawing.Point(335, 341);
            this.buttonLog.Name = "buttonLog";
            this.buttonLog.Size = new System.Drawing.Size(92, 28);
            this.buttonLog.TabIndex = 32;
            this.buttonLog.Text = "Log Test";
            this.buttonLog.UseVisualStyleBackColor = true;
            this.buttonLog.Click += new System.EventHandler(this.buttonLog_Click);
            // 
            // textBoxClass
            // 
            this.textBoxClass.Location = new System.Drawing.Point(202, 292);
            this.textBoxClass.Name = "textBoxClass";
            this.textBoxClass.Size = new System.Drawing.Size(100, 21);
            this.textBoxClass.TabIndex = 33;
            // 
            // textBoxSource
            // 
            this.textBoxSource.Location = new System.Drawing.Point(202, 319);
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.Size = new System.Drawing.Size(100, 21);
            this.textBoxSource.TabIndex = 34;
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.Location = new System.Drawing.Point(202, 346);
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.Size = new System.Drawing.Size(100, 21);
            this.textBoxMessage.TabIndex = 35;
            // 
            // buttonLogTest2
            // 
            this.buttonLogTest2.Location = new System.Drawing.Point(335, 375);
            this.buttonLogTest2.Name = "buttonLogTest2";
            this.buttonLogTest2.Size = new System.Drawing.Size(92, 28);
            this.buttonLogTest2.TabIndex = 36;
            this.buttonLogTest2.Text = "Log Test2";
            this.buttonLogTest2.UseVisualStyleBackColor = true;
            this.buttonLogTest2.Click += new System.EventHandler(this.buttonLogTest2_Click);
            // 
            // buttonMaterialCreate
            // 
            this.buttonMaterialCreate.Location = new System.Drawing.Point(479, 444);
            this.buttonMaterialCreate.Name = "buttonMaterialCreate";
            this.buttonMaterialCreate.Size = new System.Drawing.Size(81, 57);
            this.buttonMaterialCreate.TabIndex = 37;
            this.buttonMaterialCreate.Text = "Create Material ";
            this.buttonMaterialCreate.UseVisualStyleBackColor = true;
            this.buttonMaterialCreate.Click += new System.EventHandler(this.buttonMaterialCreate_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(661, 337);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(79, 48);
            this.button1.TabIndex = 38;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(651, 424);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 45);
            this.button2.TabIndex = 39;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Location = new System.Drawing.Point(26, 424);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(286, 343);
            this.propertyGrid2.TabIndex = 41;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(383, 532);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(107, 42);
            this.button3.TabIndex = 42;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(403, 597);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(114, 48);
            this.button4.TabIndex = 43;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(404, 676);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(130, 46);
            this.button5.TabIndex = 44;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 795);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.propertyGrid2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonMaterialCreate);
            this.Controls.Add(this.buttonLogTest2);
            this.Controls.Add(this.textBoxMessage);
            this.Controls.Add(this.textBoxSource);
            this.Controls.Add(this.textBoxClass);
            this.Controls.Add(this.buttonLog);
            this.Controls.Add(this.buttonSetBright);
            this.Controls.Add(this.buttonTurnOn);
            this.Controls.Add(this.textBoxVolume);
            this.Controls.Add(this.buttonIlClose);
            this.Controls.Add(this.buttonIlOpen);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonCrossLine);
            this.Controls.Add(this.propertyGridCamera);
            this.Controls.Add(this.buttonStopCamera);
            this.Controls.Add(this.buttonStartCamera);
            this.Controls.Add(this.textBoxTargetY);
            this.Controls.Add(this.textBoxTargetX);
            this.Controls.Add(this.textBoxReadIndex);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxOutputCount);
            this.Controls.Add(this.textBoxInputCount);
            this.Controls.Add(this.textBoxBoardCount);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxBoardCount;
        private System.Windows.Forms.TextBox textBoxInputCount;
        private System.Windows.Forms.TextBox textBoxOutputCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxReadIndex;
        private System.Windows.Forms.TextBox textBoxTargetX;
        private System.Windows.Forms.TextBox textBoxTargetY;
        //private QMC.Common.Hmi.VisionImageViewer visionImageViewer1;
        private System.Windows.Forms.Button buttonStartCamera;
        private System.Windows.Forms.Button buttonStopCamera;
        private System.Windows.Forms.PropertyGrid propertyGridCamera;
        private System.Windows.Forms.Button buttonCrossLine;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button buttonIlOpen;
        private System.Windows.Forms.Button buttonIlClose;
        private System.Windows.Forms.TextBox textBoxVolume;
        private System.Windows.Forms.Button buttonTurnOn;
        private System.Windows.Forms.Button buttonSetBright;
        private System.Windows.Forms.Button buttonLog;
        private System.Windows.Forms.TextBox textBoxClass;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.TextBox textBoxMessage;
        private System.Windows.Forms.Button buttonLogTest2;
        private System.Windows.Forms.Button buttonMaterialCreate;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}

