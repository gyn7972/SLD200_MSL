namespace SLD200.NewStyleForm.NewSubForm
{
    partial class FormNewSub_SemiAuto
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnSemiAutoStart;
        private System.Windows.Forms.Button btnSemiAutoNext;
        private System.Windows.Forms.Button btnSemiAutoStop;
        private System.Windows.Forms.ComboBox cboSemiAutoStep;
        private System.Windows.Forms.Label lblCurrentStep;
        private System.Windows.Forms.ComboBox cboStackerStep;
        private System.Windows.Forms.Button btnRunStackerStep;
        private System.Windows.Forms.CheckBox chkDetailAuto;

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
            this.btnSemiAutoStart = new System.Windows.Forms.Button();
            this.btnSemiAutoNext = new System.Windows.Forms.Button();
            this.btnSemiAutoStop = new System.Windows.Forms.Button();
            this.cboSemiAutoStep = new System.Windows.Forms.ComboBox();
            this.lblCurrentStep = new System.Windows.Forms.Label();
            this.cboStackerStep = new System.Windows.Forms.ComboBox();
            this.btnRunStackerStep = new System.Windows.Forms.Button();
            this.chkDetailAuto = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSemiAutoStart
            // 
            this.btnSemiAutoStart.Location = new System.Drawing.Point(30, 30);
            this.btnSemiAutoStart.Size = new System.Drawing.Size(100, 30);
            this.btnSemiAutoStart.Text = "Start";
            this.btnSemiAutoStart.Click += new System.EventHandler(this.btnSemiAutoStart_Click);
            // 
            // btnSemiAutoNext
            // 
            this.btnSemiAutoNext.Location = new System.Drawing.Point(140, 30);
            this.btnSemiAutoNext.Size = new System.Drawing.Size(100, 30);
            this.btnSemiAutoNext.Text = "Next Step";
            this.btnSemiAutoNext.Click += new System.EventHandler(this.btnSemiAutoNext_Click);
            // 
            // btnSemiAutoStop
            // 
            this.btnSemiAutoStop.Location = new System.Drawing.Point(250, 30);
            this.btnSemiAutoStop.Size = new System.Drawing.Size(100, 30);
            this.btnSemiAutoStop.Text = "Stop";
            this.btnSemiAutoStop.Click += new System.EventHandler(this.btnSemiAutoStop_Click);
            // 
            // cboSemiAutoStep
            // 
            this.cboSemiAutoStep.Location = new System.Drawing.Point(30, 70);
            this.cboSemiAutoStep.Size = new System.Drawing.Size(320, 24);
            this.cboSemiAutoStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // lblCurrentStep
            // 
            this.lblCurrentStep.Location = new System.Drawing.Point(30, 110);
            this.lblCurrentStep.Size = new System.Drawing.Size(500, 23);
            this.lblCurrentStep.Text = "Current Step: None";
            // 
            // cboStackerStep
            // 
            this.cboStackerStep.Location = new System.Drawing.Point(30, 145);
            this.cboStackerStep.Size = new System.Drawing.Size(320, 24);
            this.cboStackerStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            // 
            // btnRunStackerStep
            // 
            this.btnRunStackerStep.Location = new System.Drawing.Point(370, 145);
            this.btnRunStackerStep.Size = new System.Drawing.Size(100, 30);
            this.btnRunStackerStep.Text = "Run Step";
            this.btnRunStackerStep.Click += new System.EventHandler(this.btnRunStackerStep_Click);
            // 
            // chkDetailAuto
            // 
            this.chkDetailAuto.Location = new System.Drawing.Point(370, 30);
            this.chkDetailAuto.Size = new System.Drawing.Size(150, 30);
            this.chkDetailAuto.Text = "Stacker0 Detail Auto";
            this.chkDetailAuto.CheckedChanged += new System.EventHandler(this.ChkDetailAuto_CheckedChanged);
            // 
            // FormNewSub_SemiAuto
            // 
            this.ClientSize = new System.Drawing.Size(800, 208);
            this.Controls.Add(this.btnSemiAutoStart);
            this.Controls.Add(this.btnSemiAutoNext);
            this.Controls.Add(this.btnSemiAutoStop);
            this.Controls.Add(this.cboSemiAutoStep);
            this.Controls.Add(this.lblCurrentStep);
            this.Controls.Add(this.cboStackerStep);
            this.Controls.Add(this.btnRunStackerStep);
            this.Controls.Add(this.chkDetailAuto);
            this.Text = "Loader Semi-Auto Panel";
            this.ResumeLayout(false);

        }

        #endregion
    }
}