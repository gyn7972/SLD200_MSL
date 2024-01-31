namespace QMC.Common.UI
{
    partial class ModuleButton
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
            this.buttonModule = new System.Windows.Forms.Button();
            this.labelModuleStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonModule
            // 
            this.buttonModule.Location = new System.Drawing.Point(3, 3);
            this.buttonModule.Name = "buttonModule";
            this.buttonModule.Size = new System.Drawing.Size(120, 120);
            this.buttonModule.TabIndex = 0;
            this.buttonModule.UseVisualStyleBackColor = true;
            this.buttonModule.Click += new System.EventHandler(this.buttonModule_Click);
            // 
            // labelModuleStatus
            // 
            this.labelModuleStatus.Location = new System.Drawing.Point(9, 97);
            this.labelModuleStatus.Name = "labelModuleStatus";
            this.labelModuleStatus.Size = new System.Drawing.Size(20, 20);
            this.labelModuleStatus.TabIndex = 1;
            // 
            // ModuleButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelModuleStatus);
            this.Controls.Add(this.buttonModule);
            this.Name = "ModuleButton";
            this.Size = new System.Drawing.Size(127, 127);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonModule;
        private System.Windows.Forms.Label labelModuleStatus;
    }
}
