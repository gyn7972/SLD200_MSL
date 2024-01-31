namespace CWA150SA_Onsemi300
{
    partial class WeighCells
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
            this.groupBoxWeighCells = new System.Windows.Forms.GroupBox();
            this.LabelWeighValueUnit = new CWA150SA_Onsemi300.BaseLabel();
            this.textBoxWeighValue = new System.Windows.Forms.TextBox();
            this.btnWeighCellsZeroSet = new System.Windows.Forms.Button();
            this.btnWeighCellsReset = new System.Windows.Forms.Button();
            this.groupBoxWeighCells.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxWeighCells
            // 
            this.groupBoxWeighCells.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(38)))), ((int)(((byte)(38)))));
            this.groupBoxWeighCells.Controls.Add(this.LabelWeighValueUnit);
            this.groupBoxWeighCells.Controls.Add(this.textBoxWeighValue);
            this.groupBoxWeighCells.Controls.Add(this.btnWeighCellsZeroSet);
            this.groupBoxWeighCells.Controls.Add(this.btnWeighCellsReset);
            this.groupBoxWeighCells.ForeColor = System.Drawing.Color.White;
            this.groupBoxWeighCells.Location = new System.Drawing.Point(3, 3);
            this.groupBoxWeighCells.Name = "groupBoxWeighCells";
            this.groupBoxWeighCells.Size = new System.Drawing.Size(371, 166);
            this.groupBoxWeighCells.TabIndex = 11;
            this.groupBoxWeighCells.TabStop = false;
            this.groupBoxWeighCells.Text = " Weigh Cells ";
            // 
            // LabelWeighValueUnit
            // 
            this.LabelWeighValueUnit.AutoSize = true;
            this.LabelWeighValueUnit.Font = new System.Drawing.Font("Arial", 12F);
            this.LabelWeighValueUnit.ForeColor = System.Drawing.Color.White;
            this.LabelWeighValueUnit.Location = new System.Drawing.Point(308, 37);
            this.LabelWeighValueUnit.Name = "LabelWeighValueUnit";
            this.LabelWeighValueUnit.Size = new System.Drawing.Size(23, 16);
            this.LabelWeighValueUnit.TabIndex = 11;
            this.LabelWeighValueUnit.Text = "㎎";
            // 
            // textBoxWeighValue
            // 
            this.textBoxWeighValue.Location = new System.Drawing.Point(193, 32);
            this.textBoxWeighValue.Name = "textBoxWeighValue";
            this.textBoxWeighValue.Size = new System.Drawing.Size(109, 21);
            this.textBoxWeighValue.TabIndex = 10;
            this.textBoxWeighValue.Text = "0.0";
            this.textBoxWeighValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnWeighCellsZeroSet
            // 
            this.btnWeighCellsZeroSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnWeighCellsZeroSet.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWeighCellsZeroSet.ForeColor = System.Drawing.Color.White;
            this.btnWeighCellsZeroSet.Location = new System.Drawing.Point(193, 74);
            this.btnWeighCellsZeroSet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnWeighCellsZeroSet.Name = "btnWeighCellsZeroSet";
            this.btnWeighCellsZeroSet.Size = new System.Drawing.Size(164, 38);
            this.btnWeighCellsZeroSet.TabIndex = 9;
            this.btnWeighCellsZeroSet.Text = "Zero Set";
            this.btnWeighCellsZeroSet.UseVisualStyleBackColor = false;
            // 
            // btnWeighCellsReset
            // 
            this.btnWeighCellsReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnWeighCellsReset.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnWeighCellsReset.ForeColor = System.Drawing.Color.White;
            this.btnWeighCellsReset.Location = new System.Drawing.Point(15, 74);
            this.btnWeighCellsReset.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnWeighCellsReset.Name = "btnWeighCellsReset";
            this.btnWeighCellsReset.Size = new System.Drawing.Size(164, 38);
            this.btnWeighCellsReset.TabIndex = 8;
            this.btnWeighCellsReset.Text = "Reset";
            this.btnWeighCellsReset.UseVisualStyleBackColor = false;
            // 
            // WeighCells
            // 
            this.Controls.Add(this.groupBoxWeighCells);
            this.Name = "WeighCells";
            this.Size = new System.Drawing.Size(377, 172);
            this.groupBoxWeighCells.ResumeLayout(false);
            this.groupBoxWeighCells.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxWeighCells;
        private System.Windows.Forms.Button btnWeighCellsZeroSet;
        private System.Windows.Forms.Button btnWeighCellsReset;
        private System.Windows.Forms.TextBox textBoxWeighValue;
        private BaseLabel LabelWeighValueUnit;
    }
}
