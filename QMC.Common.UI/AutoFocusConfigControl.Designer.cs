using System.Windows.Forms;

namespace QMC.Common.UI
{
    partial class AutoFocusConfigControl
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
            this.baseGroupBox1 = new BaseGroupBox();
            this.PropertyGridAutoFocus = new BasePropertyGrid();
            this.baseGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // baseGroupBox1
            // 
            this.baseGroupBox1.Controls.Add(this.PropertyGridAutoFocus);
            this.baseGroupBox1.ForeColor = System.Drawing.Color.White;
            this.baseGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.baseGroupBox1.Name = "baseGroupBox1";
            this.baseGroupBox1.Size = new System.Drawing.Size(263, 378);
            this.baseGroupBox1.TabIndex = 0;
            this.baseGroupBox1.TabStop = false;
            this.baseGroupBox1.Text = "AutoFocus";
            // 
            // PropertyGridAutoFocus
            // 
            this.PropertyGridAutoFocus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.PropertyGridAutoFocus.CategoryForeColor = System.Drawing.Color.Black;
            this.PropertyGridAutoFocus.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.PropertyGridAutoFocus.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.PropertyGridAutoFocus.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.PropertyGridAutoFocus.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.PropertyGridAutoFocus.Location = new System.Drawing.Point(6, 20);
            this.PropertyGridAutoFocus.Name = "PropertyGridAutoFocus";
            this.PropertyGridAutoFocus.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.PropertyGridAutoFocus.Size = new System.Drawing.Size(251, 352);
            this.PropertyGridAutoFocus.TabIndex = 1;
            this.PropertyGridAutoFocus.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.PropertyGridAutoFocus.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            // 
            // AutoFocusConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.baseGroupBox1);
            this.Name = "AutoFocusConfigControl";
            this.Size = new System.Drawing.Size(263, 378);
            this.baseGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

        }

        #endregion

        private BaseGroupBox baseGroupBox1;
        private BasePropertyGrid PropertyGridAutoFocus;
    }
}
