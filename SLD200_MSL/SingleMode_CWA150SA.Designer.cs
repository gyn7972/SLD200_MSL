using System.Windows.Forms;

namespace SLD200_MSL
{
    partial class SingleMode_CWA150SA
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
            this.tabPage_2DMapping = new System.Windows.Forms.TabPage();
            this.SuspendLayout();
            // 
            // tabPage_2DMapping
            // 
            this.tabPage_2DMapping.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tabPage_2DMapping.Location = new System.Drawing.Point(4, 42);
            this.tabPage_2DMapping.Name = "tabPage_2DMapping";
            this.tabPage_2DMapping.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_2DMapping.Size = new System.Drawing.Size(1253, 658);
            this.tabPage_2DMapping.TabIndex = 4;
            this.tabPage_2DMapping.Text = "2D Mapping";
            this.tabPage_2DMapping.UseVisualStyleBackColor = true;
            // 
            // SingleMode_CWA150SA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SingleMode_CWA150SA";
            this.Size = new System.Drawing.Size(1910, 770);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage_2DMapping;
    }
}
