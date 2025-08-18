namespace SLD200.NewStyleForm.NewSubForm
{
    partial class FormNewSub_ScannerCal3D
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
            this.pictureBox_Correction3DRtcForm = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Correction3DRtcForm)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Correction3DRtcForm
            // 
            this.pictureBox_Correction3DRtcForm.Location = new System.Drawing.Point(12, 12);
            this.pictureBox_Correction3DRtcForm.Name = "pictureBox_Correction3DRtcForm";
            this.pictureBox_Correction3DRtcForm.Size = new System.Drawing.Size(760, 435);
            this.pictureBox_Correction3DRtcForm.TabIndex = 0;
            this.pictureBox_Correction3DRtcForm.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(739, 426);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // FormNewSub_ScannerCal3D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 461);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox_Correction3DRtcForm);
            this.Name = "FormNewSub_ScannerCal3D";
            this.Text = "FormNewSub_ScannerCal3D";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Correction3DRtcForm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Correction3DRtcForm;
        private System.Windows.Forms.Button button1;
    }
}