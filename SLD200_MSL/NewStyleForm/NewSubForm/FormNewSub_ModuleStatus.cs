using QMC.Common;
using QMC.Common.Global;
using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Equipment;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_ModuleStatus : Form
    {
        private DrillingProcessManager drillingProcessManager;
        private LayerProcessData selectedLayer;
        private ToolTip socketToolTip = new ToolTip();

        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        private System.Windows.Forms.Timer timerModuleStatus;
        private bool _isRunning_ModuleStatus = false;

        private int selectedSocketRow = -1;
        private int selectedSocketColumn = -1;

        private Label label_SocketInfoSummary;
        private ListView listView_LayerDetails;

        private Timer timerStatusUpdate;

        public FormNewSub_ModuleStatus()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

            ModuleCollection m_collectionModules;
            m_collectionModules = Equipment.Modules;
            foreach (Module module in m_collectionModules)
            {
                if (module.Name == "WorkStage") workStage = module as WorkStage;
                if (module.Name == "Loader") loader = module as Loader;
                if (module.Name == "Unloader") unloader = module as Unloader;
                if (module.Name == "Vision") vision = module as Vision;
                if (module.Name == "BDS") bds = module as Bds;
            }

            timerModuleStatus = new System.Windows.Forms.Timer();
            timerModuleStatus.Interval = 100;
            timerModuleStatus.Tick += TimerModuleStatus_Tick;
            timerModuleStatus.Start();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                // 사용자가 닫기(X 버튼) 누른 경우 → 숨기기만 하고 종료 안 함
                e.Cancel = true;
                this.Hide();
                return;
            }

            // 그 외 종료 (Application.Exit 등) → 정식 해제
            base.OnFormClosing(e);
        }

        public void DisposeSemiAutoResources()
        {
            if (timerModuleStatus != null)
            {
                timerModuleStatus.Stop();
                timerModuleStatus.Tick -= TimerModuleStatus_Tick;
                timerModuleStatus.Dispose();
                timerModuleStatus = null;
            }

            // 필요 시 다른 모듈 정리도 여기에
        }

        private void TimerModuleStatus_Tick(object sender, EventArgs e)
        {
            if (_isRunning_ModuleStatus)
                return;

            try
            {
                _isRunning_ModuleStatus = true;
                Timer_ModuleStatusRun();
            }
            catch (Exception ex)
            {
                // 로그 남기기
                Log.Write(ex);
                _isRunning_ModuleStatus = false;
            }
            finally
            {
                _isRunning_ModuleStatus = false;
            }
        }

        private void Timer_ModuleStatusRun()
        {
            // 실행할 작업들을 여기에 구현.

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
        }

        private void LoadLayerList()
        {
            try
            {
                listViewLayers.Items.Clear();

                foreach (var layer in drillingProcessManager.LayerList)
                {
                    // Fiducial, PreAlign 레이어는 표시하지 않음
                    if (layer.LayerType == LayerType.LAYER_FIDUCIAL || layer.LayerType == LayerType.LAYER_PREALIGN)
                        continue;

                    var activeSockets = layer.SocketList.Count(s => s.IsSelected);
                    var item = new ListViewItem(layer.LayerName);
                    item.SubItems.Add(layer.LayerType.ToString());
                    item.SubItems.Add(layer.SocketList.Count.ToString());
                    item.SubItems.Add(activeSockets.ToString());
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
                var socket = selectedLayer.SocketList[i];
                var btn = new Button
                {
                    Text = $"S{i + 1}",
                    Dock = DockStyle.Fill,
                    Tag = i,
                    BackColor = GetSocketColor(socket)
                };

                // 툴팁
                socketToolTip.SetToolTip(btn, $"소켓 {socket.SocketNumber}\n" +
                                            $"선택: {(socket.IsSelected ? "O" : "X")}, " +
                                            $"가공완료: {(socket.IsDrilled ? "O" : "X")}, 성공: {(socket.IsSuccess ? "O" : "X")}");

                // 좌클릭: 현재 소켓 번호 토글 (모든 레이어에 반영)
                btn.Click += (s, e) =>
                {
                    int socketNo = socket.SocketNumber;
                    bool nextState = !socket.IsSelected;

                    foreach (var layer in drillingProcessManager.LayerList)
                    {
                        foreach (var sckt in layer.SocketList)
                        {
                            if (sckt.SocketNumber == socketNo)
                                sckt.IsSelected = nextState;
                        }
                    }

                    CreateSocketButtons(socketCount);
                    LoadLayerList();
                };

                // 우클릭: Context 메뉴
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("이 소켓만 선택", null, (s, e) =>
                {
                    int socketNo = socket.SocketNumber;

                    foreach (var layer in drillingProcessManager.LayerList)
                    {
                        foreach (var sckt in layer.SocketList)
                        {
                            sckt.IsSelected = (sckt.SocketNumber == socketNo);
                        }
                    }

                    CreateSocketButtons(socketCount);
                    LoadLayerList();
                });

                contextMenu.Items.Add("선택 해제", null, (s, e) =>
                {
                    int socketNo = socket.SocketNumber;

                    foreach (var layer in drillingProcessManager.LayerList)
                    {
                        foreach (var sckt in layer.SocketList)
                        {
                            if (sckt.SocketNumber == socketNo)
                                sckt.IsSelected = false;
                        }
                    }

                    CreateSocketButtons(socketCount);
                    LoadLayerList();
                });

                btn.ContextMenuStrip = contextMenu;

                int row = i / columnCount;
                int col = i % columnCount;
                tableLayoutPanelSockets.Controls.Add(btn, col, row);
            }
        }


        private Color GetSocketColor(SocketProcessData socket)
        {
            if (socket.IsDrilled && socket.IsSuccess)
                return Color.LightBlue;
            if (socket.IsDrilled && !socket.IsSuccess)
                return Color.IndianRed;
            return socket.IsSelected ? Color.LightGreen : SystemColors.Control;
        }

        private void ButtonProcessAll_Click(object sender, EventArgs e)
        {
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
            if (selectedLayer == null)
            {
                MessageBox.Show("레이어를 먼저 선택해주세요.", "선택 가공 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Log.Write("ModuleStatus", $"선택 가공 준비 완료 - Layer: {selectedLayer.LayerName}");
            MessageBox.Show($"[{selectedLayer.LayerName}] 레이어의 선택된 소켓만 가공됩니다.\n장비를 시작하면 해당 소켓만 실행됩니다.", "선택 가공 준비 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

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

        public void CopySelectionFromLayer(LayerProcessData sourceLayer, LayerProcessData targetLayer)
        {
            for (int i = 0; i < Math.Min(sourceLayer.SocketList.Count, targetLayer.SocketList.Count); i++)
            {
                targetLayer.SocketList[i].IsSelected = sourceLayer.SocketList[i].IsSelected;
            }
            Log.Write("ModuleStatus", $"레이어 [{sourceLayer.LayerName}] 선택 상태가 [{targetLayer.LayerName}]에 복사됨.");
        }
    }
}
