namespace SLD200.NewStyleForm.NewSubForm
{
    partial class FormNewSub_ModuleStatus
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.GroupBox groupBoxModuleStatus;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSockets;
        private System.Windows.Forms.ListView listViewLayers;
        private System.Windows.Forms.Button buttonProcessAll;
        private System.Windows.Forms.Button buttonProcessSelected;
        private System.Windows.Forms.ColumnHeader columnHeaderLayer;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderSocketCount;
        private System.Windows.Forms.ColumnHeader columnHeaderUsable;

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
            this.groupBoxModuleStatus = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelSockets = new System.Windows.Forms.TableLayoutPanel();
            this.listViewLayers = new System.Windows.Forms.ListView();
            this.columnHeaderLayer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSocketCount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderUsable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonProcessAll = new System.Windows.Forms.Button();
            this.buttonProcessSelected = new System.Windows.Forms.Button();
            this.groupBoxModuleStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxModuleStatus
            // 
            this.groupBoxModuleStatus.Controls.Add(this.tableLayoutPanelSockets);
            this.groupBoxModuleStatus.Controls.Add(this.listViewLayers);
            this.groupBoxModuleStatus.Controls.Add(this.buttonProcessAll);
            this.groupBoxModuleStatus.Controls.Add(this.buttonProcessSelected);
            this.groupBoxModuleStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxModuleStatus.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.groupBoxModuleStatus.Location = new System.Drawing.Point(0, 0);
            this.groupBoxModuleStatus.Name = "groupBoxModuleStatus";
            this.groupBoxModuleStatus.Size = new System.Drawing.Size(734, 291);
            this.groupBoxModuleStatus.TabIndex = 0;
            this.groupBoxModuleStatus.TabStop = false;
            this.groupBoxModuleStatus.Text = "Module Status";
            // 
            // tableLayoutPanelSockets
            // 
            this.tableLayoutPanelSockets.ColumnCount = 3;
            this.tableLayoutPanelSockets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSockets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSockets.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 160F));
            this.tableLayoutPanelSockets.Location = new System.Drawing.Point(20, 40);
            this.tableLayoutPanelSockets.Name = "tableLayoutPanelSockets";
            this.tableLayoutPanelSockets.RowCount = 3;
            this.tableLayoutPanelSockets.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSockets.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSockets.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelSockets.Size = new System.Drawing.Size(200, 200);
            this.tableLayoutPanelSockets.TabIndex = 0;
            // 
            // listViewLayers
            // 
            this.listViewLayers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderLayer,
            this.columnHeaderType,
            this.columnHeaderSocketCount,
            this.columnHeaderUsable});
            this.listViewLayers.FullRowSelect = true;
            this.listViewLayers.HideSelection = false;
            this.listViewLayers.Location = new System.Drawing.Point(240, 40);
            this.listViewLayers.Name = "listViewLayers";
            this.listViewLayers.Size = new System.Drawing.Size(482, 200);
            this.listViewLayers.TabIndex = 1;
            this.listViewLayers.UseCompatibleStateImageBehavior = false;
            this.listViewLayers.View = System.Windows.Forms.View.Details;
            this.listViewLayers.SelectedIndexChanged += new System.EventHandler(this.ListViewLayers_SelectedIndexChanged);
            // 
            // columnHeaderLayer
            // 
            this.columnHeaderLayer.Text = "Layer";
            this.columnHeaderLayer.Width = 102;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 115;
            // 
            // columnHeaderSocketCount
            // 
            this.columnHeaderSocketCount.Text = "Sockets";
            this.columnHeaderSocketCount.Width = 100;
            // 
            // columnHeaderUsable
            // 
            this.columnHeaderUsable.Text = "Usable";
            this.columnHeaderUsable.Width = 154;
            // 
            // buttonProcessAll
            // 
            this.buttonProcessAll.Location = new System.Drawing.Point(20, 250);
            this.buttonProcessAll.Name = "buttonProcessAll";
            this.buttonProcessAll.Size = new System.Drawing.Size(90, 30);
            this.buttonProcessAll.TabIndex = 2;
            this.buttonProcessAll.Text = "전체 가공";
            this.buttonProcessAll.UseVisualStyleBackColor = true;
            this.buttonProcessAll.Click += new System.EventHandler(this.ButtonProcessAll_Click);
            // 
            // buttonProcessSelected
            // 
            this.buttonProcessSelected.Location = new System.Drawing.Point(130, 250);
            this.buttonProcessSelected.Name = "buttonProcessSelected";
            this.buttonProcessSelected.Size = new System.Drawing.Size(90, 30);
            this.buttonProcessSelected.TabIndex = 3;
            this.buttonProcessSelected.Text = "선택 가공";
            this.buttonProcessSelected.UseVisualStyleBackColor = true;
            this.buttonProcessSelected.Click += new System.EventHandler(this.ButtonProcessSelected_Click);
            // 
            // FormNewSub_ModuleStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 291);
            this.Controls.Add(this.groupBoxModuleStatus);
            this.Name = "FormNewSub_ModuleStatus";
            this.groupBoxModuleStatus.ResumeLayout(false);
            this.ResumeLayout(false);

            this.label_SocketInfoSummary = new System.Windows.Forms.Label();
            this.label_SocketInfoSummary.Location = new System.Drawing.Point(20, 410);
            this.label_SocketInfoSummary.Size = new System.Drawing.Size(760, 25);
            this.label_SocketInfoSummary.Text = "Socket 정보 요약";
            this.Controls.Add(this.label_SocketInfoSummary);

            this.listView_LayerDetails = new System.Windows.Forms.ListView();
            this.listView_LayerDetails.Location = new System.Drawing.Point(20, 440);
            this.listView_LayerDetails.Size = new System.Drawing.Size(760, 120);
            this.listView_LayerDetails.View = System.Windows.Forms.View.Details;
            this.listView_LayerDetails.Columns.Add("Layer", 100);
            this.listView_LayerDetails.Columns.Add("Status", 100);
            this.listView_LayerDetails.Columns.Add("Alignment Score", 150);
            this.listView_LayerDetails.Columns.Add("Z Offset", 100);
            this.listView_LayerDetails.Columns.Add("측정값", 100);
            this.Controls.Add(this.listView_LayerDetails);

        }

        #endregion
    }
}