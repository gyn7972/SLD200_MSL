namespace CWA150SA_Onsemi300
{
    partial class FormMaintEquipment
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
            this.components = new System.ComponentModel.Container();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.flowLayoutPanelModuleList = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxMaintEquipMent = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.flowLayoutPanelModuleList);
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // flowLayoutPanelModuleList
            // 
            this.flowLayoutPanelModuleList.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelModuleList.Name = "flowLayoutPanelModuleList";
            this.flowLayoutPanelModuleList.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanelModuleList.TabIndex = 0;
            // 
            // groupBoxMaintEquipMent
            // 
            this.groupBoxMaintEquipMent.Location = new System.Drawing.Point(479, 71);
            this.groupBoxMaintEquipMent.Name = "groupBoxMaintEquipMent";
            this.groupBoxMaintEquipMent.Size = new System.Drawing.Size(107, 242);
            this.groupBoxMaintEquipMent.TabIndex = 2;
            this.groupBoxMaintEquipMent.TabStop = false;
            // 
            // FormMaintEquipment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBoxMaintEquipMent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMaintEquipment";
            this.Text = "FormMaintEquipment";
            this.Controls.SetChildIndex(this.groupBoxMaintEquipMent, 0);
            this.Controls.SetChildIndex(this.panelContent, 0);
            this.panelContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelModuleList;
        private System.Windows.Forms.GroupBox groupBoxMaintEquipMent;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}