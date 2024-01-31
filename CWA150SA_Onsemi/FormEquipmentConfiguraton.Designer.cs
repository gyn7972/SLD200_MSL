namespace CWA150SA_Onsemi300
{
    partial class FormEquipmentConfiguraton
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.treeViewFuctionConfig = new CWA150SA_Onsemi300.BaseTreeView();
            this.propertyGridConfig = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.buttonAxisIoAdd = new CWA150SA_Onsemi300.BaseButton();
            this.buttonAxisIoRemove = new CWA150SA_Onsemi300.BaseButton();
            this.baseDataGridViewAlarm = new CWA150SA_Onsemi300.BaseDataGridView();
            this.baseButtonAlarmAdd = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonAlarmRemove = new CWA150SA_Onsemi300.BaseButton();
            this.basePropertyGridMotion = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.buttonAxisMotionRemove = new CWA150SA_Onsemi300.BaseButton();
            this.buttonAxisMotionAdd = new CWA150SA_Onsemi300.BaseButton();
            this.basePropertyGridIO = new CWA150SA_Onsemi300.BasePropertyGrid();
            this.baseButtonLoad = new CWA150SA_Onsemi300.BaseButton();
            this.baseButtonSave = new CWA150SA_Onsemi300.BaseButton();
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewAlarm)).BeginInit();
            this.SuspendLayout();
            // 
            // panelContent
            // 
            this.panelContent.Location = new System.Drawing.Point(50, 141);
            this.panelContent.Size = new System.Drawing.Size(226, 152);
            // 
            // treeViewFuctionConfig
            // 
            this.treeViewFuctionConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.treeViewFuctionConfig.Font = new System.Drawing.Font("Arial", 16F);
            this.treeViewFuctionConfig.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.treeViewFuctionConfig.Location = new System.Drawing.Point(594, 282);
            this.treeViewFuctionConfig.Name = "treeViewFuctionConfig";
            this.treeViewFuctionConfig.Size = new System.Drawing.Size(66, 56);
            this.treeViewFuctionConfig.TabIndex = 0;
            this.treeViewFuctionConfig.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewConfig_AfterSelect);
            // 
            // propertyGridConfig
            // 
            this.propertyGridConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridConfig.CategoryForeColor = System.Drawing.Color.Black;
            this.propertyGridConfig.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.propertyGridConfig.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridConfig.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridConfig.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.propertyGridConfig.Location = new System.Drawing.Point(620, 158);
            this.propertyGridConfig.Name = "propertyGridConfig";
            this.propertyGridConfig.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridConfig.Size = new System.Drawing.Size(56, 70);
            this.propertyGridConfig.TabIndex = 2;
            this.propertyGridConfig.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.propertyGridConfig.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.propertyGridConfig.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridConfig_PropertyValueChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Location = new System.Drawing.Point(782, 123);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(277, 408);
            this.tabControl1.TabIndex = 18;
            // 
            // buttonAxisIoAdd
            // 
            this.buttonAxisIoAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonAxisIoAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAxisIoAdd.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonAxisIoAdd.Location = new System.Drawing.Point(405, 482);
            this.buttonAxisIoAdd.Name = "buttonAxisIoAdd";
            this.buttonAxisIoAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAxisIoAdd.TabIndex = 25;
            this.buttonAxisIoAdd.Text = "Add";
            this.buttonAxisIoAdd.UseVisualStyleBackColor = true;
            this.buttonAxisIoAdd.Click += new System.EventHandler(this.buttonAxisIoAdd_Click);
            // 
            // buttonAxisIoRemove
            // 
            this.buttonAxisIoRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonAxisIoRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAxisIoRemove.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonAxisIoRemove.Location = new System.Drawing.Point(405, 530);
            this.buttonAxisIoRemove.Name = "buttonAxisIoRemove";
            this.buttonAxisIoRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonAxisIoRemove.TabIndex = 26;
            this.buttonAxisIoRemove.Text = "Remove";
            this.buttonAxisIoRemove.UseVisualStyleBackColor = true;
            this.buttonAxisIoRemove.Click += new System.EventHandler(this.buttonAxisIoRemove_Click);
            // 
            // baseDataGridViewAlarm
            // 
            this.baseDataGridViewAlarm.AllowUserToAddRows = false;
            this.baseDataGridViewAlarm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.baseDataGridViewAlarm.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseDataGridViewAlarm.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewAlarm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.baseDataGridViewAlarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.baseDataGridViewAlarm.DefaultCellStyle = dataGridViewCellStyle2;
            this.baseDataGridViewAlarm.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(3)))), ((int)(((byte)(3)))));
            this.baseDataGridViewAlarm.Location = new System.Drawing.Point(345, 67);
            this.baseDataGridViewAlarm.Name = "baseDataGridViewAlarm";
            this.baseDataGridViewAlarm.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.baseDataGridViewAlarm.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.baseDataGridViewAlarm.RowHeadersVisible = false;
            this.baseDataGridViewAlarm.RowHeadersWidth = 51;
            this.baseDataGridViewAlarm.RowTemplate.Height = 23;
            this.baseDataGridViewAlarm.Size = new System.Drawing.Size(240, 150);
            this.baseDataGridViewAlarm.TabIndex = 27;
            // 
            // baseButtonAlarmAdd
            // 
            this.baseButtonAlarmAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonAlarmAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonAlarmAdd.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonAlarmAdd.Location = new System.Drawing.Point(91, 530);
            this.baseButtonAlarmAdd.Name = "baseButtonAlarmAdd";
            this.baseButtonAlarmAdd.Size = new System.Drawing.Size(75, 23);
            this.baseButtonAlarmAdd.TabIndex = 28;
            this.baseButtonAlarmAdd.Text = "Add";
            this.baseButtonAlarmAdd.UseVisualStyleBackColor = false;
            this.baseButtonAlarmAdd.Click += new System.EventHandler(this.baseButtonAlarmAdd_Click);
            // 
            // baseButtonAlarmRemove
            // 
            this.baseButtonAlarmRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonAlarmRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonAlarmRemove.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonAlarmRemove.Location = new System.Drawing.Point(91, 573);
            this.baseButtonAlarmRemove.Name = "baseButtonAlarmRemove";
            this.baseButtonAlarmRemove.Size = new System.Drawing.Size(75, 23);
            this.baseButtonAlarmRemove.TabIndex = 29;
            this.baseButtonAlarmRemove.Text = "Remove";
            this.baseButtonAlarmRemove.UseVisualStyleBackColor = false;
            this.baseButtonAlarmRemove.Click += new System.EventHandler(this.baseButtonAlarmRemove_Click);
            // 
            // basePropertyGridMotion
            // 
            this.basePropertyGridMotion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridMotion.CategoryForeColor = System.Drawing.Color.Black;
            this.basePropertyGridMotion.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.basePropertyGridMotion.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridMotion.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridMotion.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.basePropertyGridMotion.Location = new System.Drawing.Point(682, 50);
            this.basePropertyGridMotion.Name = "basePropertyGridMotion";
            this.basePropertyGridMotion.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridMotion.Size = new System.Drawing.Size(130, 130);
            this.basePropertyGridMotion.TabIndex = 30;
            this.basePropertyGridMotion.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.basePropertyGridMotion.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            // 
            // buttonAxisMotionRemove
            // 
            this.buttonAxisMotionRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonAxisMotionRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAxisMotionRemove.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonAxisMotionRemove.Location = new System.Drawing.Point(510, 338);
            this.buttonAxisMotionRemove.Name = "buttonAxisMotionRemove";
            this.buttonAxisMotionRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonAxisMotionRemove.TabIndex = 32;
            this.buttonAxisMotionRemove.Text = "Remove";
            this.buttonAxisMotionRemove.UseVisualStyleBackColor = true;
            this.buttonAxisMotionRemove.Click += new System.EventHandler(this.buttonAxisMotionRemove_Click);
            // 
            // buttonAxisMotionAdd
            // 
            this.buttonAxisMotionAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.buttonAxisMotionAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAxisMotionAdd.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.buttonAxisMotionAdd.Location = new System.Drawing.Point(510, 290);
            this.buttonAxisMotionAdd.Name = "buttonAxisMotionAdd";
            this.buttonAxisMotionAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAxisMotionAdd.TabIndex = 31;
            this.buttonAxisMotionAdd.Text = "Add";
            this.buttonAxisMotionAdd.UseVisualStyleBackColor = true;
            this.buttonAxisMotionAdd.Click += new System.EventHandler(this.buttonAxisMotionAdd_Click);
            // 
            // basePropertyGridIO
            // 
            this.basePropertyGridIO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridIO.CategoryForeColor = System.Drawing.Color.Black;
            this.basePropertyGridIO.CategorySplitterColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.basePropertyGridIO.CommandsBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridIO.HelpBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridIO.HelpForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.basePropertyGridIO.Location = new System.Drawing.Point(800, 28);
            this.basePropertyGridIO.Name = "basePropertyGridIO";
            this.basePropertyGridIO.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.basePropertyGridIO.Size = new System.Drawing.Size(130, 130);
            this.basePropertyGridIO.TabIndex = 33;
            this.basePropertyGridIO.ViewBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.basePropertyGridIO.ViewBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            // 
            // baseButtonLoad
            // 
            this.baseButtonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonLoad.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonLoad.Location = new System.Drawing.Point(666, 468);
            this.baseButtonLoad.Name = "baseButtonLoad";
            this.baseButtonLoad.Size = new System.Drawing.Size(75, 23);
            this.baseButtonLoad.TabIndex = 35;
            this.baseButtonLoad.Text = "Load";
            this.baseButtonLoad.UseVisualStyleBackColor = true;
            this.baseButtonLoad.Click += new System.EventHandler(this.ButtonLoadClick);
            // 
            // baseButtonSave
            // 
            this.baseButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(78)))), ((int)(((byte)(78)))));
            this.baseButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.baseButtonSave.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.baseButtonSave.Location = new System.Drawing.Point(666, 367);
            this.baseButtonSave.Name = "baseButtonSave";
            this.baseButtonSave.Size = new System.Drawing.Size(75, 23);
            this.baseButtonSave.TabIndex = 34;
            this.baseButtonSave.Text = "Save";
            this.baseButtonSave.UseVisualStyleBackColor = true;
            this.baseButtonSave.Click += new System.EventHandler(this.ButtonSaveClick);
            // 
            // FormEquipmentConfiguratin
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(1095, 651);
            this.Controls.Add(this.baseButtonLoad);
            this.Controls.Add(this.baseButtonSave);
            this.Controls.Add(this.basePropertyGridIO);
            this.Controls.Add(this.buttonAxisMotionRemove);
            this.Controls.Add(this.buttonAxisMotionAdd);
            this.Controls.Add(this.basePropertyGridMotion);
            this.Controls.Add(this.baseButtonAlarmRemove);
            this.Controls.Add(this.baseButtonAlarmAdd);
            this.Controls.Add(this.baseDataGridViewAlarm);
            this.Controls.Add(this.buttonAxisIoRemove);
            this.Controls.Add(this.buttonAxisIoAdd);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.treeViewFuctionConfig);
            this.Controls.Add(this.propertyGridConfig);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormEquipmentConfiguratin";
            this.Text = "FormEquipmentConfiguratin";
            this.Controls.SetChildIndex(this.propertyGridConfig, 0);
            this.Controls.SetChildIndex(this.treeViewFuctionConfig, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.buttonAxisIoAdd, 0);
            this.Controls.SetChildIndex(this.buttonAxisIoRemove, 0);
            this.Controls.SetChildIndex(this.baseDataGridViewAlarm, 0);
            this.Controls.SetChildIndex(this.baseButtonAlarmAdd, 0);
            this.Controls.SetChildIndex(this.baseButtonAlarmRemove, 0);
            this.Controls.SetChildIndex(this.basePropertyGridMotion, 0);
            this.Controls.SetChildIndex(this.buttonAxisMotionAdd, 0);
            this.Controls.SetChildIndex(this.buttonAxisMotionRemove, 0);
            this.Controls.SetChildIndex(this.basePropertyGridIO, 0);
            this.Controls.SetChildIndex(this.panelContent, 0);
            this.Controls.SetChildIndex(this.baseButtonSave, 0);
            this.Controls.SetChildIndex(this.baseButtonLoad, 0);
            ((System.ComponentModel.ISupportInitialize)(this.baseDataGridViewAlarm)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseTreeView treeViewFuctionConfig;
        private BasePropertyGrid propertyGridConfig;
        private System.Windows.Forms.TabControl tabControl1;
        private BaseButton buttonAxisIoAdd;
        private BaseButton buttonAxisIoRemove;
        private BaseDataGridView baseDataGridViewAlarm;
        private BaseButton baseButtonAlarmAdd;
        private BaseButton baseButtonAlarmRemove;
        private BasePropertyGrid basePropertyGridMotion;
        private BaseButton buttonAxisMotionRemove;
        private BaseButton buttonAxisMotionAdd;
        private BasePropertyGrid basePropertyGridIO;
        private BaseButton baseButtonLoad;
        private BaseButton baseButtonSave;
    }
}