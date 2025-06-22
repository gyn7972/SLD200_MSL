using QMC.Common;
using QMC.Common.Global;
using QMC.Common.Modules;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static QMC.Common.Equipment;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_ModuleMonitor : Form
    {
        private DrillingProcessManager drillingProcessManager;
        private bool _isDrawing = false;

        static WorkStage workStage;
        static Loader loader;
        static Unloader unloader;
        static Vision vision;
        static Bds bds;

        private System.Windows.Forms.Timer timerModuleMonitor;
        private bool _isRunning_ModuleMonitor = false;

        public FormNewSub_ModuleMonitor()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.DoubleBuffered = true;

            foreach (Module module in Equipment.Modules)
            {
                if (module.Name == "WorkStage") workStage = module as WorkStage;
                if (module.Name == "Loader") loader = module as Loader;
                if (module.Name == "Unloader") unloader = module as Unloader;
                if (module.Name == "Vision") vision = module as Vision;
                if (module.Name == "BDS") bds = module as Bds;
            }

            timerModuleMonitor = new System.Windows.Forms.Timer
            {
                Interval = 100
            };
            timerModuleMonitor.Tick += TimerModuleStatus_Tick;
            timerModuleMonitor.Start();

            //this.Paint += FormNewSub_ModuleMonitor_Paint;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                return;
            }
            base.OnFormClosing(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_isDrawing || drillingProcessManager == null || drillingProcessManager.LayerList == null)
                return;

            _isDrawing = true;

            try
            {
                Graphics g = e.Graphics;
                g.Clear(Color.White);

                int totalSockets = drillingProcessManager.GetMaxSocketCountPerLayer(true);
                if (totalSockets == 0)
                    return;

                // ▶ 패널 내부 전체 크기 기준
                int panelWidth = this.ClientSize.Width;
                int panelHeight = this.ClientSize.Height;
                int margin = 5;
                int spacing = 5;

                // ▶ 그리드 개수 계산 (최대한 정사각형에 가깝게)
                int columnCount = (int)Math.Ceiling(Math.Sqrt(totalSockets));
                int rowCount = (int)Math.Ceiling(totalSockets / (double)columnCount);

                // ▶ 소켓 크기 계산 (전체 공간 내에서 spacing 포함하여 자동 조절)
                int totalSpacingX = (columnCount - 1) * spacing + 2 * margin;
                int totalSpacingY = (rowCount - 1) * spacing + 2 * margin;
                int socketSize = Math.Min(
                    (panelWidth - totalSpacingX) / columnCount,
                    (panelHeight - totalSpacingY) / rowCount
                );

                // ▶ 그리기 시작 위치
                int x = margin;
                int y = margin;

                for (int i = 0; i < totalSockets; i++)
                {
                    Rectangle socketRect = new Rectangle(x, y, socketSize, socketSize);
                    g.DrawRectangle(Pens.Black, socketRect);

                    DrawLayerQuadInSocket(g, socketRect, i);
                    DrawSocketIndex(g, socketRect, i + 1);

                    x += socketSize + spacing;
                    if ((i + 1) % columnCount == 0)
                    {
                        x = margin;
                        y += socketSize + spacing;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write("ModuleMonitor", $"그리기 중 예외: {ex.Message}");
            }
            finally
            {
                _isDrawing = false;
            }
            //for (int i = 0; i < drillingProcessManager.GetMaxSocketCountPerLayer(); i++)
            //{
            //    Rectangle socketRect = GetSocketRect(i); // 좌표 생성
            //    DrawLayerQuadInSocket(e.Graphics, socketRect, i);
            //}
        }

        public void DisposeSemiAutoResources()
        {
            if (timerModuleMonitor != null)
            {
                timerModuleMonitor.Stop();
                timerModuleMonitor.Tick -= TimerModuleStatus_Tick;
                timerModuleMonitor.Dispose();
                timerModuleMonitor = null;
            }
        }

        private void TimerModuleStatus_Tick(object sender, EventArgs e)
        {
            if (_isRunning_ModuleMonitor)
                return;

            try
            {
                _isRunning_ModuleMonitor = true;
                Timer_ModuleStatusRun();
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                _isRunning_ModuleMonitor = false;
            }
        }

        private void Timer_ModuleStatusRun()
        {
            if (drillingProcessManager != null && drillingProcessManager.HasChanged())
            {
                this.Invalidate(); // 화면 다시 그리기
                this.Refresh();
                //this.Update();
            }
        }

        public void LoadDrillingManager(DrillingProcessManager manager)
        {
            this.drillingProcessManager = manager;
        }

        private void FormNewSub_ModuleMonitor_Paint(object sender, PaintEventArgs e)
        {
            if (_isDrawing || drillingProcessManager == null || drillingProcessManager.LayerList == null)
                return;

            _isDrawing = true;

            try
            {
                Graphics g = e.Graphics;
                g.Clear(Color.White);

                int totalSockets = drillingProcessManager.GetMaxSocketCountPerLayer(true);
                if (totalSockets == 0)
                    return;

                // 패널 내부 전체 크기 기준
                int panelWidth = this.ClientSize.Width;
                int panelHeight = this.ClientSize.Height;
                int margin = 5;
                int spacing = 5;

                // 그리드 개수 계산 (최대한 정사각형에 가깝게)
                int columnCount = (int)Math.Ceiling(Math.Sqrt(totalSockets));
                int rowCount = (int)Math.Ceiling(totalSockets / (double)columnCount);

                // 소켓 크기 계산 (전체 공간 내에서 spacing 포함하여 자동 조절)
                int totalSpacingX = (columnCount - 1) * spacing + 2 * margin;
                int totalSpacingY = (rowCount - 1) * spacing + 2 * margin;
                int socketSize = Math.Min(
                    (panelWidth - totalSpacingX) / columnCount,
                    (panelHeight - totalSpacingY) / rowCount
                );

                // 그리기 시작 위치
                int x = margin;
                int y = margin;

                for (int i = 0; i < totalSockets; i++)
                {
                    Rectangle socketRect = new Rectangle(x, y, socketSize, socketSize);
                    g.DrawRectangle(Pens.Black, socketRect);

                    DrawLayerQuadInSocket(g, socketRect, i);
                    DrawSocketIndex(g, socketRect, i + 1);

                    x += socketSize + spacing;
                    if ((i + 1) % columnCount == 0)
                    {
                        x = margin;
                        y += socketSize + spacing;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write("ModuleMonitor", $"그리기 중 예외: {ex.Message}");
            }
            finally
            {
                _isDrawing = false;
            }
        }



        private void DrawLayerQuadInSocket(Graphics g, Rectangle socketRect, int socketIndex)
        {
            try
            {
                var layerMap = new Dictionary<LayerType, Rectangle>
                {
                    { LayerType.LAYER_DRILLING, GetTopLeftQuad(socketRect) },
                    { LayerType.LAYER_THRUHOLE, GetTopRightQuad(socketRect) },
                    { LayerType.LAYER_OUTLINE, GetBottomLeftQuad(socketRect) },
                    { LayerType.LAYER_MARKING, GetBottomRightQuad(socketRect) },
                };

                foreach (var layer in drillingProcessManager.LayerList)
                {
                    if (!layerMap.ContainsKey(layer.LayerType))
                        continue;

                    if (socketIndex >= layer.SocketList.Count)
                        continue;

                    var socket = layer.SocketList[socketIndex];
                    var rect = layerMap[layer.LayerType];
                    g.FillRectangle(GetBrushBySocketStatus(socket), rect);
                    g.DrawRectangle(Pens.Black, rect);
                }
            }
            catch (Exception ex)
            {
                Log.Write("ModuleMonitor", $"DrawLayerQuadInSocket 예외: {ex.Message}");
            }
        }

        private Brush GetBrushBySocketStatus(SocketProcessData socket)
        {
            if (!socket.IsDrilled)
                return Brushes.LightGray;
            if (!socket.IsSuccess)
                return Brushes.Gold;
            return Brushes.LightGreen;
        }

        private Rectangle GetTopLeftQuad(Rectangle rect) => new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height / 2);
        private Rectangle GetTopRightQuad(Rectangle rect) => new Rectangle(rect.X + rect.Width / 2, rect.Y, rect.Width / 2, rect.Height / 2);
        private Rectangle GetBottomLeftQuad(Rectangle rect) => new Rectangle(rect.X, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2);
        private Rectangle GetBottomRightQuad(Rectangle rect) => new Rectangle(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, rect.Width / 2, rect.Height / 2);

        private void DrawSocketIndex(Graphics g, Rectangle rect, int socketNumber)
        {
            string text = socketNumber.ToString();
            using (Font font = new Font("Tahoma", rect.Height / 5f, FontStyle.Bold))
            using (StringFormat format = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            })
            {
                g.DrawString(text, font, Brushes.Black, rect, format);
            }
        }

        private Rectangle GetSocketRect(int index)
        {
            int totalSockets = drillingProcessManager.GetMaxSocketCountPerLayer(true);

            int margin = 20;
            int spacing = 10;

            int panelWidth = this.ClientSize.Width - margin * 2;
            int panelHeight = this.ClientSize.Height - margin * 2;

            int columnCount = (int)Math.Ceiling(Math.Sqrt(totalSockets));
            int rowCount = (int)Math.Ceiling(totalSockets / (double)columnCount);

            // socket 크기 자동 계산 (정사각형 기준)
            int socketSizeX = (panelWidth - (columnCount - 1) * spacing) / columnCount;
            int socketSizeY = (panelHeight - (rowCount - 1) * spacing) / rowCount;
            int socketSize = Math.Min(socketSizeX, socketSizeY);

            // 실제 위치 계산
            int row = index / columnCount;
            int col = index % columnCount;

            int x = margin + col * (socketSize + spacing);
            int y = margin + row * (socketSize + spacing);

            return new Rectangle(x, y, socketSize, socketSize);
        }






    }
}
