namespace QMC.Common.UI
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
            this.baseLabelService = new BaseLabel();
            this.baseLabelBehavior = new BaseLabel();
            this.textBoxModuleService = new System.Windows.Forms.TextBox();
            this.textBoxModuleBehevior = new System.Windows.Forms.TextBox();
            this.baseButtonInitialize = new BaseButton();
            this.baseButtonStop = new BaseButton();
            this.baseGroupBoxPartInit = new BaseGroupBox();
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
            this.groupBoxStateControl.Location = new System.Drawing.Point(0, 0);
            this.groupBoxStateControl.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxStateControl.Name = "groupBoxStateControl";
            this.groupBoxStateControl.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBoxStateControl.Size = new System.Drawing.Size(149, 246);
            this.groupBoxStateControl.TabIndex = 0;
            this.groupBoxStateControl.TabStop = false;
            this.groupBoxStateControl.Text = "State";
            // 
            // baseLabelService
            // 
            this.baseLabelService.AutoSize = true;
            this.baseLabelService.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelService.ForeColor = System.Drawing.Color.White;
            this.baseLabelService.Location = new System.Drawing.Point(15, 82);
            this.baseLabelService.Name = "baseLabelService";
            this.baseLabelService.Size = new System.Drawing.Size(79, 20);
            this.baseLabelService.TabIndex = 7;
            this.baseLabelService.Text = "Service";
            // 
            // baseLabelBehavior
            // 
            this.baseLabelBehavior.AutoSize = true;
            this.baseLabelBehavior.Font = new System.Drawing.Font("돋움", 12F, System.Drawing.FontStyle.Bold);
            this.baseLabelBehavior.ForeColor = System.Drawing.Color.White;
            this.baseLabelBehavior.Location = new System.Drawing.Point(15, 18);
            this.baseLabelBehavior.Name = "baseLabelBehavior";
            this.baseLabelBehavior.Size = new System.Drawing.Size(89, 20);
            this.baseLabelBehavior.TabIndex = 6;
            this.baseLabelBehavior.Text = "Behavior";
            // 
            // textBoxModuleService
            // 
            this.textBoxModuleService.Location = new System.Drawing.Point(11, 110);
            this.textBoxModuleService.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxModuleService.Name = "textBoxModuleService";
            this.textBoxModuleService.Size = new System.Drawing.Size(125, 25);
            this.textBoxModuleService.TabIndex = 5;
            // 
            // textBoxModuleBehevior
            // 
            this.textBoxModuleBehevior.Location = new System.Drawing.Point(11, 45);
            this.textBoxModuleBehevior.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxModuleBehevior.Name = "textBoxModuleBehevior";
            this.textBoxModuleBehevior.Size = new System.Drawing.Size(125, 25);
            this.textBoxModuleBehevior.TabIndex = 4;
            // 
            // baseButtonInitialize
            // 
            this.baseButtonInitialize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonInitialize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonInitialize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonInitialize.Location = new System.Drawing.Point(17, 151);
            this.baseButtonInitialize.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseButtonInitialize.Name = "baseButtonInitialize";
            this.baseButtonInitialize.Size = new System.Drawing.Size(114, 38);
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
            this.baseButtonStop.Location = new System.Drawing.Point(17, 199);
            this.baseButtonStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseButtonStop.Name = "baseButtonStop";
            this.baseButtonStop.Size = new System.Drawing.Size(114, 38);
            this.baseButtonStop.TabIndex = 2;
            this.baseButtonStop.Text = "Stop";
            this.baseButtonStop.UseVisualStyleBackColor = false;
            this.baseButtonStop.Click += new System.EventHandler(this.baseButtonStop_Click);
            // 
            // baseGroupBoxPartInit
            // 
            this.baseGroupBoxPartInit.ForeColor = System.Drawing.Color.White;
            this.baseGroupBoxPartInit.Location = new System.Drawing.Point(0, 254);
            this.baseGroupBoxPartInit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseGroupBoxPartInit.Name = "baseGroupBoxPartInit";
            this.baseGroupBoxPartInit.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.baseGroupBoxPartInit.Size = new System.Drawing.Size(149, 292);
            this.baseGroupBoxPartInit.TabIndex = 1;
            this.baseGroupBoxPartInit.TabStop = false;
            this.baseGroupBoxPartInit.Text = "Part Initialize";
            // 
            // ModuleStateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBoxPartInit);
            this.Controls.Add(this.groupBoxStateControl);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ModuleStateControl";
            this.Size = new System.Drawing.Size(149, 550);
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
