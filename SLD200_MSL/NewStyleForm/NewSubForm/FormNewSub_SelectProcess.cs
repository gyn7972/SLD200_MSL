using QMC.Common;
using QMC.Common.Global;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QMC.Common.Equipment;
using static QMC.Common.Part;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_SelectProcess : Form
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

        public FormNewSub_SelectProcess()
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


            workStage.ActionDrillingProcessManagerSelectedUpdated += OnDrillingDataUpdated;
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
            if (drillingProcessManager != null && drillingProcessManager.HasChanged())
            {
                this.Invalidate(); // 화면 다시 그리기
                this.Refresh();
                //this.Update();
            }

        }

        private void OnDrillingDataUpdated(DrillingProcessManager manager)
        {
            return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => OnDrillingDataUpdated(manager)));
                return;
            }

            // UI 스레드에서 안전하게 실행
            LoadDrillingManager(manager);
            this.Invalidate(); // 화면 다시 그리기
            this.Refresh();
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

        private void CreateSocketButtons(int socketCount)
        {
            tableLayoutPanelSockets.Controls.Clear();

            // 소켓 수에 맞는 가장 근접한 정사각형 그리드 계산
            int columnCount = (int)Math.Ceiling(Math.Sqrt(socketCount));
            int rowCount = (int)Math.Ceiling((double)socketCount / columnCount);

            tableLayoutPanelSockets.ColumnCount = columnCount;
            tableLayoutPanelSockets.RowCount = rowCount;
            tableLayoutPanelSockets.ColumnStyles.Clear();
            tableLayoutPanelSockets.RowStyles.Clear();

            for (int i = 0; i < columnCount; i++)
                tableLayoutPanelSockets.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columnCount));
            for (int i = 0; i < rowCount; i++)
                tableLayoutPanelSockets.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rowCount));


            for (int i = 0; i < socketCount; i++)
            {
                var socket = selectedLayer.SocketList[i];
                var btn = new Button
                {
                    Text = "", // 글자 안 쓰고 그리기에서 그림
                    Dock = DockStyle.Fill,
                    Tag = i,
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btn.FlatAppearance.BorderSize = 0;

                // 커스텀 그림
                btn.Paint += (s, pe) =>
                {
                    DrawSocketButtonStyle(pe.Graphics, btn.ClientRectangle, socket);
                };

                // 툴팁
                //socketToolTip.SetToolTip(btn, $"소켓 {socket.SocketNumber}\n" +
                //                            $"선택: {(socket.IsSelected ? "O" : "X")}, " +
                //                            $"가공완료: {(socket.IsDrilled ? "O" : "X")}, 성공: {(socket.IsSuccess ? "O" : "X")}");
                //var target = selectedLayer?.SocketList.FirstOrDefault(s => s.SocketNumber == socket.SocketNumber);
                //if (target != null)
                //{
                //    socketToolTip.SetToolTip(btn, $"소켓 {target.SocketNumber}\n" +
                //        $"선택: {(target.IsSelected ? "O" : "X")}, " +
                //        $"가공완료: {(target.IsDrilled ? "O" : "X")}, 성공: {(target.IsSuccess ? "O" : "X")}");
                //}
                var statusText = new StringBuilder();
                statusText.AppendLine($"소켓 {socket.SocketNumber + 1}");

                foreach (var layer in drillingProcessManager.LayerList)
                {
                    var sckt = layer.SocketList.FirstOrDefault(s => s.SocketNumber == socket.SocketNumber);
                    if (sckt == null)
                        continue;

                    statusText.AppendLine($"[{layer.LayerType}] 선택: {(sckt.IsSelected ? "O" : "X")}, 완료: {(sckt.IsDrilled ? "O" : "X")}, 성공: {(sckt.IsSuccess ? "O" : "X")}");
                }
                socketToolTip.SetToolTip(btn, statusText.ToString());


                // 좌클릭 동작
                btn.Click += (s, e) =>
                {
                    int socketNo = socket.SocketNumber;

                    // 해당 소켓이 모든 레이어에서 선택되었는지 확인
                    bool isFullySelected = drillingProcessManager.LayerList
                        .All(layer => layer.SocketList.Any(sckt => sckt.SocketNumber == socketNo && sckt.IsSelected));

                    if (!isFullySelected)
                    {
                        // 처음 선택: 모든 레이어에서 선택
                        foreach (var layer in drillingProcessManager.LayerList)
                        {
                            foreach (var sckt in layer.SocketList)
                            {
                                if (sckt.SocketNumber == socketNo)
                                    sckt.IsSelected = true;
                            }
                        }
                    }
                    else
                    {
                        // 토글: 현재 선택된 레이어만 반전
                        var targetSocket = selectedLayer.SocketList.FirstOrDefault(sckt => sckt.SocketNumber == socketNo);
                        if (targetSocket != null)
                            targetSocket.IsSelected = !targetSocket.IsSelected;
                    }

                    CreateSocketButtons(selectedLayer.SocketList.Count);
                    LoadLayerList();
                };
                //btn.Click += (s, e) =>
                //{
                //    int socketNo = socket.SocketNumber;
                //    bool nextState = !socket.IsSelected;
                //    foreach (var layer in drillingProcessManager.LayerList)
                //    {
                //        foreach (var sckt in layer.SocketList)
                //        {
                //            if (sckt.SocketNumber == socketNo)
                //                sckt.IsSelected = nextState;
                //        }
                //    }
                //    CreateSocketButtons(socketCount);
                //    LoadLayerList();
                //};

                // 우클릭 메뉴
                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("이 소켓만 선택", null, (s, e) =>
                {
                    int socketNo = socket.SocketNumber;
                    foreach (var layer in drillingProcessManager.LayerList)
                    {
                        foreach (var sckt in layer.SocketList)
                            sckt.IsSelected = (sckt.SocketNumber == socketNo);
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
                            if (sckt.SocketNumber == socketNo)
                                sckt.IsSelected = false;
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
            if (selectedLayer == null)
                return SystemColors.Control;

            var target = selectedLayer.SocketList.FirstOrDefault(s => s.SocketNumber == socket.SocketNumber);
            if (target == null)
                return SystemColors.Control;

            if (target.IsDrilled && target.IsSuccess)
                return Color.LightBlue;
            if (target.IsDrilled && !target.IsSuccess)
                return Color.IndianRed;
            return target.IsSelected ? Color.LightGreen : SystemColors.Control;
        }

        private void ButtonProcessAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var layer in drillingProcessManager.LayerList)
                {
                    foreach (var socket in layer.SocketList)
                        socket.IsSelected = true;
                }
                this.Refresh();

                Log.Write("ModuleStatus", "전체 소켓이 선택됨.");

                string msg = "모든 소켓이 선택되었습니다.\r\n" +
                             "장비를 시작하면 전체 가공됩니다.";

                var mb = new MessageBoxOk();
                mb.ShowDialog("Information!", msg);

                if (Equipment.AutoRunStatus)
                    return;

                Log.Write("SLD-200", Equipment.User_Name, "Button Click", "선택 가공 버튼");

                if (workStage.CheckAllInterlock(out msg) == false)
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Error", msg);
                    return;
                }

                if (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None)
                {
                    Equipment.LaserDrillingCycStop_Reservation = false;
                    workStage.m_bLaserDrilling_SocketStopped = false;
                    Equipment.SocketStopped = false;

                    SelectRunEnable_New = true;
                    workStage.m_nDrillingWork_Group_Count = 0;
                    workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.Start;
                    workStage.m_LaserDrillingWork_Start = true;
                    workStage.m_ProductAlign_Start = true;
                    WorkStartTick = Environment.TickCount;
                }
                else
                {
                    var mb1 = new MessageBoxOk();
                    mb1.ShowDialog("Error", "Reset 후 실행 바랍니다.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
                Log.Write("ModuleStatus", $"전체 가공 선택 중 오류: {ex.Message}");
            }
        }

        private void ButtonProcessSelected_Click(object sender, EventArgs e)
        {
            var selectedPerLayer = drillingProcessManager.LayerList
                .Where(layer => layer.LayerType != LayerType.LAYER_FIDUCIAL && layer.LayerType != LayerType.LAYER_PREALIGN)
                .Select(layer => new
                {
                    LayerName = layer.LayerName,
                    LayerType = layer.LayerType,
                    Sockets = layer.SocketList
                                .Where(socket => socket.IsSelected)
                                .Select(socket => socket.SocketNumber + 1)  // 1-based 번호로 표시
                                .OrderBy(n => n)
                                .ToList()
                })
                .Where(x => x.Sockets.Count > 0)
                .ToList();

            if (selectedPerLayer.Count == 0)
            {
                MessageBox.Show("선택된 소켓이 없습니다.", "선택 가공 실패", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"총 {selectedPerLayer.Sum(x => x.Sockets.Count)}개의 소켓이 선택되었습니다.");
            sb.AppendLine("선택된 소켓은 각 레이어에서 다음과 같이 가공됩니다:");
            sb.AppendLine();

            foreach (var entry in selectedPerLayer)
            {
                sb.AppendLine($"[{entry.LayerType}] → 소켓: {string.Join(", ", entry.Sockets)}");
            }

            Log.Write("ModuleStatus", $"선택 가공 준비 완료 - {selectedPerLayer.Count}개 레이어");

            var mb = new MessageBoxOk();
            mb.ShowDialog("Information!", sb.ToString());


            if (Equipment.AutoRunStatus)
                return;

            Log.Write("SLD-200", Equipment.User_Name, "Button Click", "선택 가공 버튼");

            string msg = "";
            if (workStage.CheckAllInterlock(out msg) == false)
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Error", msg);
                return;
            }

            if (workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None)
            {
                Equipment.LaserDrillingCycStop_Reservation = false;
                workStage.m_bLaserDrilling_SocketStopped = false;
                Equipment.SocketStopped = false;

                SelectRunEnable_New = true;
                workStage.m_nDrillingWork_Group_Count = 0;
                workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.Start;
                workStage.m_LaserDrillingWork_Start = true;
                workStage.m_ProductAlign_Start = true;
                WorkStartTick = Environment.TickCount;

            }
            else
            {
                var mb1 = new MessageBoxOk();
                mb1.ShowDialog("Error", "Reset 후 실행 바랍니다.");
                return;
            }

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

        private void DrawSocketButtonStyle(Graphics g, Rectangle rect, SocketProcessData socket)
        {
            var layerMap = new Dictionary<LayerType, Rectangle>
    {
        { LayerType.LAYER_DRILLING, GetTopLeftQuad(rect) },
        { LayerType.LAYER_THRUHOLE, GetTopRightQuad(rect) },
        { LayerType.LAYER_OUTLINE, GetBottomLeftQuad(rect) },
        { LayerType.LAYER_MARKING, GetBottomRightQuad(rect) }
    };

            var textMap = new Dictionary<LayerType, string>
    {
        { LayerType.LAYER_DRILLING, "H" },
        { LayerType.LAYER_THRUHOLE, "T" },
        { LayerType.LAYER_OUTLINE, "O" },
        { LayerType.LAYER_MARKING, "M" }
    };

            foreach (var kvp in layerMap)
            {
                LayerType type = kvp.Key;
                Rectangle subRect = kvp.Value;

                var layer = drillingProcessManager.LayerList.FirstOrDefault(l => l.LayerType == type);
                if (layer == null)
                    continue;

                var sckt = layer.SocketList.FirstOrDefault(s => s.SocketNumber == socket.SocketNumber);
                if (sckt == null)
                    continue;

                // 선택된 레이어만 색으로 표시
                Brush brush = Brushes.White;  // 기본 흰색
                if (selectedLayer != null && selectedLayer.LayerType == type)
                {
                    brush = sckt.IsSelected ? Brushes.LightGreen : Brushes.LightGray;
                }

                g.FillRectangle(brush, subRect);
                g.DrawRectangle(Pens.Black, subRect);

                if (textMap.TryGetValue(type, out string label))
                    DrawCenteredText(g, subRect, label);
            }

            // 중앙 소켓 번호
            string socketNumberText = (socket.SocketNumber + 1).ToString();
            using (Font font = new Font("Tahoma", rect.Height / 4f, FontStyle.Bold))
            using (StringFormat format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(socketNumberText, font, Brushes.Black, rect, format);
            }
        }


        private Rectangle GetTopLeftQuad(Rectangle rect) =>
            new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height / 2);

        private Rectangle GetTopRightQuad(Rectangle rect) =>
            new Rectangle(rect.X + rect.Width / 2, rect.Y, rect.Width / 2, rect.Height / 2);

        private Rectangle GetBottomLeftQuad(Rectangle rect) =>
            new Rectangle(rect.X, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2);

        private Rectangle GetBottomRightQuad(Rectangle rect) =>
            new Rectangle(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2);

        private void DrawCenteredText(Graphics g, Rectangle rect, string text)
        {
            using (Font font = new Font("Tahoma", rect.Height / 5f, FontStyle.Bold))
            using (StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                g.DrawString(text, font, Brushes.Black, rect, format);
            }
        }

        private Brush GetBrushBySocketStatus(SocketProcessData socket)
        {
            if (selectedLayer == null)
                return Brushes.LightGray;

            var matchingSocket = selectedLayer.SocketList.FirstOrDefault(s => s.SocketNumber == socket.SocketNumber);
            if (matchingSocket == null)
                return Brushes.LightGray;

            return matchingSocket.IsSelected ? Brushes.LightGreen : Brushes.LightGray;

            //if (socket.IsSelected)
            //    return Brushes.LightGreen;
            //else
            //    return Brushes.LightGray;
        }

        private Brush GetLayerBrush(LayerType type, SocketProcessData socket)
        {
            var isSelected = socket.IsSelected;
            var isActiveLayer = selectedLayer != null && selectedLayer.LayerType == type;

            if (!socket.IsDrilled)
                return isActiveLayer ? Brushes.LightGray : Brushes.WhiteSmoke;

            if (socket.IsDrilled && !socket.IsSuccess)
                return isActiveLayer ? Brushes.Gold : Brushes.MistyRose;

            return isActiveLayer ? Brushes.LightGreen : Brushes.Honeydew;
        }

        private void buttonProcessStop_Click(object sender, EventArgs e)
        {
            var mb = new MessageBoxYesNo();
            if (DialogResult.Yes != mb.ShowDialog("Question ?", "가공을 중지하시겠습니까?\r\n\r\n[레이저도 Off 됩니다.]"))
                return;

            Equipment.MachineStop_byUser = true;
            WorkStartTick = 0;
            WorkStartTick_Outline = 0;
            WorkStartTick_Thruhole = 0;
            WorkStartTick_Drilling = 0;

            Equipment.LaserDrillingCycStop_Reservation = false;
            workStage.m_nLaserDrilling_MainStep = (int)WorkStage.LaserDrilling_Step.None;
            workStage.m_nFindAlignMark_Step = (int)WorkStage.FindAlignMark_Step.None;
            workStage.m_nSocketAlign_MainStep = (int)WorkStage.SocketAlign_Step.None;

            // 장비 정지 시 그냥 정지 시킨다.
            workStage.m_ScannerCameraOffsetSequence.Reset();
            workStage.scannerCompensator.SetRunStatus(Part.RunStatus.Stop);
            workStage.m_ScannerCameraOffsetSequence.m_MainTick_Start = false;
            workStage.m_bSensorRequestPending = false;   // 요청 보냄
            workStage.m_bSensorResponseReady = false;    // 응답 받음

            workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.X, 2000);
            workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Y, 2000);
            workStage.MC_Func.MC_MotorStop((int)WorkStageParameter.AxisAjinEnum.Z, 2000);

            workStage.laser.Rtc.CtlAbort();             //  실행중인 리스트 명령(busy 상태를)을 강제 종료
            Thread.Sleep(2000);
            workStage.laser.Rtc.CtlReset();             //  에러 해제

            Equipment.SelectRunEnable_New = false; //  수동 가공 시작

            //Main 정지 버튼
            {
                Equipment.Loader_LPort_Pause = true;        //  장비 Stop 시 Pause
                Equipment.Loader_RPort_Pause = true;        //  장비 Stop 시 Pause

                Equipment.SelectRunEnable_New = false;
                Equipment.AutoRunStatus = false;        // 자동운전중
                Equipment.AutoManualStatus = false;     // Auto / Manual 상태 유/무 
                workStage.SetRunStatus(RunStatus.Stop);

                Equipment.ProcessingData_Parsing_byLoader = false;

                workStage.m_nSelectedSocket_Index = -1;
                workStage.m_nSocketAlign_StartIndex = -1;
                Equipment.SelectedSocketStartMode = (int)SelectedSocketStartModeList.All;
                
                workStage._isMainWorkRunning = false;
                workStage._isLaserDrillingWorkRunning = false;
                loader._isLoaderWorkRunning = false;
                unloader._isUnloaderWorkRunning = false;

                // 아래 변수가 자동운전 Tick 돌리는 변수임.
                workStage.m_MainWork_Start = false;
                //workStage.m_LaserDrillingWork_Start = false;              //  Laser Drilling Cycle 은 바로 Stop 하지 않고, 가공중이던 영역이 완료되면 Stop 하도록 예약을 걸어둔다.
                Equipment.LaserDrillingCycStop_Reservation = true;          //  Stop 예약
                workStage.m_ProductAlign_Start = false;
                workStage.m_SubWork_Start = false;
                loader.m_LoaderWork_Start = false;
                unloader.m_UnloaderWork_Start = false;

                // 장비 정지 시 그냥 정지 시킨다.
                workStage.m_ScannerCameraOffsetSequence.Reset();
                workStage.scannerCompensator.SetRunStatus(Part.RunStatus.Stop);
                workStage.m_ScannerCameraOffsetSequence.m_MainTick_Start = false;

                workStage.m_bSensorRequestPending = false;   // 요청 보냄
                workStage.m_bSensorResponseReady = false;    // 응답 받음

                loader.ClearSemiAutoRequest();
                workStage.ClearSemiAutoRequest();
                unloader.ClearSemiAutoRequest();

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //
                //  장비 운전 정지 시점의 모든 상태 데이터 저장
                //
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //  Loader 상태
                loader.m_nLD_RESTORE_Transfer_Step = loader.m_nLoader_Transfer_Step;
                loader.m_nLD_RESTORE_Transfer_MoveType = loader.m_nLoaderTransferMoveType;
                loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete;               //  Work Stage 에 Module Put Down 완료 여부
                loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker0_Complete;               //  Stacker0 에서 Module Pick Up 완료 여부
                loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromStacker1_Complete;               //  Stacker1 에서 Module Pick Up 완료 여부
                loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePickUpfromMAligner_Complete;               //  M-Aligner 에서 Module Pick Up 완료 여부
                loader.m_bLD_RESTORE_AUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoMAligner_Complete;                 //  M-Aligner 에 Module Put Down 완료 여부
                loader.m_nLD_RESTORE_MainWork_Cycle_Step = workStage.m_nMainWork_Step;                                                                                              //  Main Work Cycle Step
                loader.m_nLD_RESTORE_DryRun_Cycle_Step = workStage.m_nDryRun_Step;                                                                                                  //  Dry Run Cycle Step
                loader.m_nLD_RESTORE_LaserDrilling_Cycle_Step = workStage.m_nLaserDrilling_MainStep;                                                                                //  Laser Drilling Cycle Step
                loader.m_bLD_RESTORE_MainWork_Cycle_Complete = workStage.m_bMainWorkCycle_Complete;                                                                                 //  Main Work Cycle 완료 여부
                loader.m_bLD_RESTORE_Transfer_fromStacker0_Module_PickUp_Complete_Flag = loader.m_bLD_Transfer_fromStacker0_Module_PickUp_Complete_Flag;                            //  Stacker0 에서 Module Pick Up 완료 여부
                loader.m_bLD_RESTORE_Transfer_fromStacker1_Module_PickUp_Complete_Flag = loader.m_bLD_Transfer_fromStacker1_Module_PickUp_Complete_Flag;                            //  Stacker1 에서 Module Pick Up 완료 여부
                loader.m_bLD_RESTORE_Transfer_fromMAligner_Module_PickUp_Complete_Flag = loader.m_bLD_Transfer_fromMAligner_Module_PickUp_Complete_Flag;                            //  M-Aligner 에서 Module Pick Up 완료 여부
                loader.m_bLD_RESTORE_Transfer_toMAligner_Module_PutDown_Complete_Flag = loader.m_bLD_Transfer_toMAligner_Module_PutDown_Complete_Flag;                              //  M-Aligner 에 Module Put Down 완료 여부
                loader.m_bLD_RESTORE_Transfer_toWorkStage_Module_PutDown_Complete_Flag = loader.m_bLD_Transfer_toWorkStage_Module_PutDown_Complete_Flag;                            //  Work Stage 에 Module Put Down 완료 여부

                //  Unloader 상태
                unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete = unloader.m_bAUTORUN_Unloader_Transfer_ModulePickUpfromWorkStage_Complete;     //  Work Stage 에서 Module Pick Up 완료 여부
                unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete = unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker0_Complete;         //  Stacker0 에 Module Put Down 완료 여부
                unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete = unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoStacker1_Complete;         //  Stacker1 에 Module Put Down 완료 여부
                unloader.m_bUL_RESTORE_AUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete = unloader.m_bAUTORUN_Unloader_Transfer_ModulePutDowntoNG_Complete;                     //  NG-Port 에 Module Put Down 완료 여부        
                unloader.m_nUL_RESTORE_Transfer_Step = unloader.m_nUnloader_Transfer_Step;                                                                                          //  Unloader Transfer Step
                unloader.m_nUL_RESTORE_Transfer_MoveType = unloader.m_nUnloaderTransferMoveType;                                                                                    //  Unloader Transfer Move Type
                unloader.m_bUL_RESTORE_Transfer_fromWorkStage_Module_PickUp_Complete_Flag = unloader.m_bUL_Transfer_fromWorkStage_Module_PickUp_Complete_Flag;                      //  Unloader 가 Work Stage 에서 Module Pick Up 완료 여부
                unloader.m_bUL_RESTORE_LD_Transfer_toWorkStage_Module_PutDown_Complete = loader.m_bAUTORUN_Loader_Transfer_ModulePutDowntoWorkStage_Complete;                       //  Loader 가 Work Stage 에 Module Put Down 완료 여부
                unloader.m_nUL_RESTORE_MainWork_Cycle_Step = workStage.m_nMainWork_Step;                                                                                            //  Main Work Cycle Step
                unloader.m_nUL_RESTORE_DryRun_Cycle_Step = workStage.m_nDryRun_Step;                                                                                                //  Dry Run Cycle Step
                unloader.m_nUL_RESTORE_LaserDrilling_Cycle_Step = workStage.m_nLaserDrilling_MainStep;                                                                              //  Laser Drilling Cycle Step
                unloader.m_bUL_RESTORE_MainWork_Cycle_Complete = workStage.m_bMainWorkCycle_Complete;                                                                               //  Main Work Cycle 완료 여부
                unloader.m_nUL_RESTORE_MainWork_Cycle_ResultOKNG = workStage.m_nMainWorkCycle_ResultOKNG;                                                                           //  Main Work Cycle 결과 (OK, NG) : OK 인 경우에만 R-Port 로 가져감
                unloader.m_bUL_RESTORE_MainWorkCycle_ResultOK_toRPort = workStage.m_bMainWorkCycle_ResultOK_toRPort;

                workStage.DrillingManager.CycleTimer_LaserDrilling.End();   // 현재 사이클 정지 : 정지 버튼 눌렀을때도 정지하고 다시 해야지.
                string strPath = "D:\\SLD-200_Parameter\\CycleTime.ini";
                workStage.DrillingManager.CycleTimer_LaserDrilling.SaveToIni("LaserDrilling", strPath);
            }
        }
    }
}
