namespace QMC.Common.UI
{
    partial class RoiListControl
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
            this.groupBoxRoiList = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelButton = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxRoiList.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxRoiList
            // 
            this.groupBoxRoiList.Controls.Add(this.flowLayoutPanelButton);
            this.groupBoxRoiList.ForeColor = System.Drawing.Color.White;
            this.groupBoxRoiList.Location = new System.Drawing.Point(0, 0);
            this.groupBoxRoiList.Name = "groupBoxRoiList";
            this.groupBoxRoiList.Size = new System.Drawing.Size(115, 100);
            this.groupBoxRoiList.TabIndex = 0;
            this.groupBoxRoiList.TabStop = false;
            this.groupBoxRoiList.Text = "Roi List";
            // 
            // flowLayoutPanelButton
            // 
            this.flowLayoutPanelButton.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelButton.Location = new System.Drawing.Point(5, 15);
            this.flowLayoutPanelButton.Name = "flowLayoutPanelButton";
            this.flowLayoutPanelButton.Size = new System.Drawing.Size(105, 80);
            this.flowLayoutPanelButton.TabIndex = 0;
            // 
            // RoiListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxRoiList);
            this.Name = "RoiListControl";
            this.Size = new System.Drawing.Size(115, 100);
            this.groupBoxRoiList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxRoiList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButton;
    }
}
