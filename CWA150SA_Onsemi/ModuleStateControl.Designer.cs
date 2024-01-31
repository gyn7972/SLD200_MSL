namespace CWA150SA_Onsemi300
{
    partial class ModuleStateControl
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
            this.groupBoxStateControl = new System.Windows.Forms.GroupBox();
            this.baseLabelService = new CWA150SA_Onsemi300.BaseLabel();
            this.baseLabelBehavior = new CWA150SA_Onsemi300.BaseLabel();
            this.textBoxModuleService = new System.Windows.Forms.TextBox();
            this.textBoxModuleBehevior = new System.Windows.Forms.TextBox();
            this.baseButtonInitialize = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonStop = new CWA150SA_Onsemi300.BaseButton();
            this.baseGroupBoxPartInit = new CWA150SA_Onsemi300.BaseGroupBox();
            this.groupBoxStateControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxStateControl
            // 
            this.groupBoxStateControl.Controls.Add(this.baseLabelService);
            this.groupBoxStateControl.Controls.Add(this.baseLabelBehavior);
            this.groupBoxStateControl.Controls.Add(this.textBoxModuleService);
            this.groupBoxStateControl.Controls.Add(this.textBoxModuleBehevior);
            this.groupBoxStateControl.Controls.Add(this.baseButtonInitialize);
            this.groupBoxStateControl.Controls.Add(this.baseButtonStop);
            this.groupBoxStateControl.ForeColor = System.Drawing.Color.White;
            this.groupBoxStateControl.Location = new System.Drawing.Point(0, 4);
            this.groupBoxStateControl.Name = "groupBoxStateControl";
            this.groupBoxStateControl.Size = new System.Drawing.Size(168, 196);
            this.groupBoxStateControl.TabIndex = 0;
            this.groupBoxStateControl.TabStop = false;
            this.groupBoxStateControl.Text = " State ";
            // 
            // baseLabelService
            // 
            this.baseLabelService.AutoSize = true;
            this.baseLabelService.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelService.ForeColor = System.Drawing.Color.White;
            this.baseLabelService.Location = new System.Drawing.Point(9, 69);
            this.baseLabelService.Name = "baseLabelService";
            this.baseLabelService.Size = new System.Drawing.Size(63, 15);
            this.baseLabelService.TabIndex = 7;
            this.baseLabelService.Text = "Service";
            // 
            // baseLabelBehavior
            // 
            this.baseLabelBehavior.AutoSize = true;
            this.baseLabelBehavior.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.baseLabelBehavior.ForeColor = System.Drawing.Color.White;
            this.baseLabelBehavior.Location = new System.Drawing.Point(9, 17);
            this.baseLabelBehavior.Name = "baseLabelBehavior";
            this.baseLabelBehavior.Size = new System.Drawing.Size(73, 15);
            this.baseLabelBehavior.TabIndex = 6;
            this.baseLabelBehavior.Text = "Behavior";
            // 
            // textBoxModuleService
            // 
            this.textBoxModuleService.Location = new System.Drawing.Point(9, 88);
            this.textBoxModuleService.Name = "textBoxModuleService";
            this.textBoxModuleService.Size = new System.Drawing.Size(150, 21);
            this.textBoxModuleService.TabIndex = 5;
            // 
            // textBoxModuleBehevior
            // 
            this.textBoxModuleBehevior.Location = new System.Drawing.Point(9, 36);
            this.textBoxModuleBehevior.Name = "textBoxModuleBehevior";
            this.textBoxModuleBehevior.Size = new System.Drawing.Size(150, 21);
            this.textBoxModuleBehevior.TabIndex = 4;
            // 
            // baseButtonInitialize
            // 
            this.baseButtonInitialize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonInitialize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonInitialize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonInitialize.Location = new System.Drawing.Point(9, 121);
            this.baseButtonInitialize.Name = "baseButtonInitialize";
            this.baseButtonInitialize.Size = new System.Drawing.Size(150, 30);
            this.baseButtonInitialize.TabIndex = 3;
            this.baseButtonInitialize.Text = "Initialize";
            this.baseButtonInitialize.UseVisualStyleBackColor = false;
            this.baseButtonInitialize.Click += new System.EventHandler(this.baseButtonInitialize_Click);
            // 
            // baseButtonStop
            // 
            this.baseButtonStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonStop.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonStop.Location = new System.Drawing.Point(9, 155);
            this.baseButtonStop.Name = "baseButtonStop";
            this.baseButtonStop.Size = new System.Drawing.Size(150, 30);
            this.baseButtonStop.TabIndex = 2;
            this.baseButtonStop.Text = "Stop";
            this.baseButtonStop.UseVisualStyleBackColor = false;
            this.baseButtonStop.Click += new System.EventHandler(this.baseButtonStop_Click);
            // 
            // baseGroupBoxPartInit
            // 
            this.baseGroupBoxPartInit.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxPartInit.Location = new System.Drawing.Point(0, 217);
            this.baseGroupBoxPartInit.Name = "baseGroupBoxPartInit";
            this.baseGroupBoxPartInit.Size = new System.Drawing.Size(168, 233);
            this.baseGroupBoxPartInit.TabIndex = 2;
            this.baseGroupBoxPartInit.TabStop = false;
            this.baseGroupBoxPartInit.Text = " Part Initialize ";
            // 
            // ModuleStateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxPartInit);
            this.Controls.Add(this.groupBoxStateControl);
            this.Name = "ModuleStateControl";
            this.Size = new System.Drawing.Size(169, 453);
            this.groupBoxStateControl.ResumeLayout(false);
            this.groupBoxStateControl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxStateControl;
        private BaseLabel baseLabelService;
        private BaseLabel baseLabelBehavior;
        private System.Windows.Forms.TextBox textBoxModuleService;
        private System.Windows.Forms.TextBox textBoxModuleBehevior;
        private BaseButton baseButtonInitialize;
        private BaseButton baseButtonStop;
        protected System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButton;
        private BaseGroupBox baseGroupBoxPartInit;
    }
}
