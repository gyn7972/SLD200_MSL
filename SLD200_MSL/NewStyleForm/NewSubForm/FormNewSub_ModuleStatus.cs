using QMC.Common;
using QMC.Common.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_ModuleStatus : Form
    {
        private DrillingProcessManager drillingProcessManager;
        private LayerProcessData selectedLayer;

        public FormNewSub_ModuleStatus()
        {
            InitializeComponent();
        }

        public void LoadDrillingManager(DrillingProcessManager manager)
        {
            if (manager == null || manager.LayerList == null || manager.LayerList.Count == 0)
            {
                Log.Write("ModuleStatus", "DrillingProcessManager가 비어있습니다.");
                return;
            }

            this.drillingProcessManager = manager;

            LoadLayerList();
            listViewLayers.Items[0].Selected = true; // 기본 선택
            //CreateSocketButtons(GetMaxSocketCount());
        }

        private void LoadLayerList()
        {
            try
            {
                listViewLayers.Items.Clear();

                foreach (var layer in drillingProcessManager.LayerList)
                {
                    var item = new ListViewItem(layer.LayerName);
                    item.SubItems.Add(layer.LayerType.ToString());
                    item.SubItems.Add(layer.SocketList.Count.ToString());
                    item.SubItems.Add(layer.SocketList.Any(s => s.IsUsedInThisLayer) ? "O" : "X");
                    item.Tag = layer;

                    listViewLayers.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Log.Write("ModuleStatus", $"Layer 목록 로딩 중 오류: {ex.Message}");
                MessageBox.Show("레이어 목록 로딩 중 오류가 발생했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListViewLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewLayers.SelectedItems.Count == 0 || drillingProcessManager == null)
                return;

            selectedLayer = listViewLayers.SelectedItems[0].Tag as LayerProcessData;

            if (selectedLayer != null)
            {
                Log.Write("ModuleStatus", $"레이어 선택됨: {selectedLayer.LayerName}");
                CreateSocketButtons(selectedLayer.SocketList.Count);
            }
        }

        private int GetMaxSocketCount()
        {
            return drillingProcessManager.LayerList.Max(l => l.SocketList.Count);
        }

        private void CreateSocketButtons(int socketCount, int columnCount = 6)
        {
            tableLayoutPanelSockets.Controls.Clear();
            tableLayoutPanelSockets.ColumnCount = columnCount;
            tableLayoutPanelSockets.RowCount = (int)Math.Ceiling(socketCount / (double)columnCount);
            tableLayoutPanelSockets.ColumnStyles.Clear();
            tableLayoutPanelSockets.RowStyles.Clear();

            for (int i = 0; i < columnCount; i++)
                tableLayoutPanelSockets.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columnCount));
            for (int i = 0; i < tableLayoutPanelSockets.RowCount; i++)
                tableLayoutPanelSockets.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / tableLayoutPanelSockets.RowCount));

            for (int i = 0; i < socketCount; i++)
            {
                var btn = new Button
                {
                    Text = $"S{i + 1}",
                    Dock = DockStyle.Fill,
                    Tag = i,
                    BackColor = selectedLayer.SocketList[i].IsSelected ? Color.LightGreen : SystemColors.Control
                };

                btn.Click += (s, e) =>
                {
                    int index = (int)((Button)s).Tag;
                    var socket = selectedLayer.SocketList[index];
                    socket.IsSelected = !socket.IsSelected;
                    btn.BackColor = socket.IsSelected ? Color.LightGreen : SystemColors.Control;

                    Log.Write("ModuleStatus", $"소켓 {socket.SocketNumber} 선택 상태: {socket.IsSelected}");
                };

                int row = i / columnCount;
                int col = i % columnCount;
                tableLayoutPanelSockets.Controls.Add(btn, col, row);
            }
        }


        private void ButtonProcessAll_Click(object sender, EventArgs e)
        {
            // 전체 소켓 가공 로직
            try
            {
                foreach (var layer in drillingProcessManager.LayerList)
                {
                    foreach (var socket in layer.SocketList)
                    {
                        socket.IsSelected = true;
                    }
                }

                Log.Write("ModuleStatus", "전체 소켓이 선택됨.");
                MessageBox.Show("모든 소켓이 선택되었습니다.\n장비를 시작하면 전체 가공됩니다.", "전체 가공 준비 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Write("ModuleStatus", $"전체 가공 선택 중 오류: {ex.Message}");
                MessageBox.Show("전체 가공 선택 중 오류가 발생했습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonProcessSelected_Click(object sender, EventArgs e)
        {
            // 선택 소켓만 가공 로직
            if (selectedLayer == null)
            {
                MessageBox.Show("레이어를 먼저 선택해주세요.", "선택 가공 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Log.Write("ModuleStatus", $"선택 가공 준비 완료 - Layer: {selectedLayer.LayerName}");
            MessageBox.Show($"[{selectedLayer.LayerName}] 레이어의 선택된 소켓만 가공됩니다.\n장비를 시작하면 해당 소켓만 실행됩니다.", "선택 가공 준비 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 이 함수는 실제 가공 장비 로직에서 호출됩니다
        public void ProcessSelectedSockets()
        {
            int processedCount = 0;

            foreach (var layer in drillingProcessManager.LayerList)
            {
                foreach (var socket in layer.SocketList)
                {
                    if (socket.IsSelected)
                    {
                        socket.IsDrilled = true;
                        socket.IsSuccess = true;

                        Log.Write("Drill", $"[가공됨] Layer: {layer.LayerName}, Socket: {socket.SocketNumber}");
                        processedCount++;
                    }
                }
            }

            Log.Write("Drill", $"총 {processedCount}개 소켓 가공 완료");
        }
    }
}
