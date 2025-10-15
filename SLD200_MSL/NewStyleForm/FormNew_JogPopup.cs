using QMC.Common;
using QMC.Common.Modules;
using QMC.Common.Parts;
using QMC.Common.VisionPart;
using QMC.Core;
using SharpGL;
using SLD200.NewStyleForm.NewSubForm;
using SpiralLab.Sirius; // Sirius Document 접근
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

namespace SLD200.NewStyleForm
{
    public partial class FormNew_JogPopup : Form
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        private bool m_bInitialized = false;
        private System.Windows.Forms.Timer timer_Status;
        Loader loader;
        WorkStage workStage;
        Unloader unloader;
        private FormNewSub_JogPopup_Loader userform_Loader;
        private FormNewSub_JogPopup_Stage userform_Stage;
        private FormNewSub_JogPopup_Unloader userform_Unloader;
        private TabPage SelectedTabPage = null;
        private ContextMenuStrip _viewerContextMenu;
        private System.Drawing.Point _lastMouseDownPoint;
        private System.Drawing.Point _mouseDownLocation;   // Main 과 동일한 멤버 추가
        private int _selectedSocketIndex = -1;   // 선택 소켓 인덱스
        private double _viewerScale_mmPerPixel = 0.01; // 임시 스케일
        private bool _pendingDocSync = false;
        private bool _viewerHooked = false;

        // 필드 추가
        private bool _viewerInitialized = false;
        private IView _cachedView;
        private static readonly System.Reflection.MethodInfo _miGetDoc =
            typeof(Equipment).GetMethod("GetEqpSiriusViewerDocument", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);


        public FormNew_JogPopup()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Load += FormNewSub_JogPopup_Load;
            this.FormClosing += FormNew_JogPopup_FormClosing;
            this.tabControl_JogPopup.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            this.tabControl_JogPopup.SelectedIndex = 1;
            if (this.tabControl_JogPopup.TabPages.Count > 2)
                SelectedTabPage = this.tabControl_JogPopup.TabPages[1];
        }

        private void InitSiriusViewer()
        {
            if (_viewerInitialized)
                return;
            try
            {
                if (_viewerContextMenu == null)
                {
                    _viewerContextMenu = new ContextMenuStrip();
                    _viewerContextMenu.Items.Add("Select Socket", null, ContextMenu_SelectSocket_Click);
                    _viewerContextMenu.Items.Add("이 위치로 이동", null, ContextMenu_MoveToThisPosition_Click);
                    _viewerContextMenu.Items.Add("선택된 중심으로 이동", null, ContextMenu_MoveToSelectedGroupCenter_Click);
                }

                if (_miGetDoc != null)
                {
                    if (_miGetDoc.Invoke(null, null) is IDocument doc && doc != null && SiriusViewer_JogPopup.Document != doc)
                        SiriusViewer_JogPopup.Document = doc;
                }

                HookViewerEvents();

                // 필요 시 즉시 View 확보 (중복 생성 방지)
                if (SiriusViewer_JogPopup.Document != null && SiriusViewer_JogPopup.Document.Views.Count == 0)
                {
                    _cachedView = new ViewDefault(SiriusViewer_JogPopup.Document, SiriusViewer_JogPopup.GLcontrol);
                    SiriusViewer_JogPopup.Document.Views.Add(_cachedView);
                }

                _viewerInitialized = true;
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }

            //기존코드
            {
                //try
                //{
                //    if (_viewerContextMenu == null)
                //    {
                //        _viewerContextMenu = new ContextMenuStrip();
                //        _viewerContextMenu.Items.Add("Select Socket", null, ContextMenu_SelectSocket_Click);
                //        _viewerContextMenu.Items.Add("이 위치로 이동", null, ContextMenu_MoveToThisPosition_Click);
                //        _viewerContextMenu.Items.Add("선택된 중심으로 이동", null, ContextMenu_MoveToSelectedGroupCenter_Click);
                //    }

                //    // 메인에서 공유 Document 가져오기
                //    try
                //    {
                //        var mi = typeof(Equipment).GetMethod("GetEqpSiriusViewerDocument");
                //        if (mi != null)
                //        {
                //            var doc = mi.Invoke(null, null) as IDocument;
                //            if (doc != null && SiriusViewer_JogPopup.Document != doc)
                //                SiriusViewer_JogPopup.Document = doc;
                //        }
                //    }
                //    catch { }

                //    HookViewerEvents();
                //    SiriusViewer_JogPopup.Invalidate();
                //}
                //catch (Exception ex)
                //{
                //    Log.Write(ex);
                //}
            }
        }

        // ===================== 컨텍스트 메뉴 동작 (Main 과 동일 로직 적용) =====================
        private void ContextMenu_MoveToThisPosition_Click(object sender, EventArgs e)
        {
            try
            {
                var doc = this.SiriusViewer_JogPopup.Document;
                if (doc?.Views == null)
                    return;
                if (doc.Views.Count == 0)
                {
                    // View 없으면 하나 생성
                    var viewNew = new ViewDefault(doc, SiriusViewer_JogPopup.GLcontrol);
                    doc.Views.Add(viewNew);
                }
                var view = doc.Views.Last();
                view.Dp2Lp(_mouseDownLocation, out float dX, out float dY);
                var ptReal = new XyzCoordinate(dX, dY, 0);

                if (Equipment.AutoManualStatus == false)
                {
                    if (Equipment._InitDeviceStatus.MotionIo && workStage != null)
                    {
                        var targetPos = workStage.ConvertPointFineCam(ptReal);
                        targetPos = (XyzCoordinate)workStage.ConvertPreAlignData(new XyCoordinate(targetPos.X, targetPos.Y));
                        workStage.MovetoWorkStage_ABS_PositionsXY(new XyCoordinate(targetPos.X, targetPos.Y), Type_Motor_Speed.Process);
                    }
                    else
                    {
                        MessageBox.Show("초기화 되지 않았습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("설비가 Manual 상태가 아닙니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
        private void ContextMenu_MoveToSelectedGroupCenter_Click(object sender, EventArgs e)
        {
            try
            {
                var doc = this.SiriusViewer_JogPopup.Document;
                if (doc == null)
                    return;

                foreach (var layer in doc.Layers)
                {
                    if (!layer.IsMarkerable || layer.Count == 0)
                        continue;

                    foreach (var entity in layer)
                    {
                        if (entity.EntityType == EType.Group)
                        {
                            var group = entity as Group;
                            if (group != null && group.IsSelected)
                            {
                                var ptReal = new XyzCoordinate(group.Location.X, group.Location.Y, 0);
                                if (Equipment.AutoManualStatus == false)
                                {
                                    if (Equipment._InitDeviceStatus.MotionIo && workStage != null)
                                    {
                                        var targetPos = workStage.ConvertPointFineCam(ptReal);
                                        targetPos = (XyzCoordinate)workStage.ConvertPreAlignData(new XyCoordinate(targetPos.X, targetPos.Y));
                                        workStage.MovetoWorkStage_ABS_PositionsXY(new XyCoordinate(targetPos.X, targetPos.Y), Type_Motor_Speed.Process);
                                    }
                                    else
                                    {
                                        MessageBox.Show("초기화 되지 않았습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("설비가 Manual 상태가 아닙니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                return; // 첫 번째 선택 그룹만 처리
                            }
                        }
                        else if (entity.EntityType == EType.Circle)
                        {
                            var circle = entity as SpiralLab.Sirius.Circle; // 명확히 지정
                            if (circle != null && circle.IsSelected)
                            {
                                var ptReal = new XyzCoordinate(circle.Center.X, circle.Center.Y, 0);
                                if (Equipment.AutoManualStatus == false)
                                {
                                    if (Equipment._InitDeviceStatus.MotionIo && workStage != null)
                                    {
                                        var targetPos = workStage.ConvertPointFineCam(ptReal);
                                        targetPos = (XyzCoordinate)workStage.ConvertPreAlignData(new XyCoordinate(targetPos.X, targetPos.Y));
                                        workStage.MovetoWorkStage_ABS_PositionsXY(new XyCoordinate(targetPos.X, targetPos.Y), Type_Motor_Speed.Process);
                                    }
                                    else
                                    {
                                        MessageBox.Show("초기화 되지 않았습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("설비가 Manual 상태가 아닙니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                return; // 첫 번째만 처리
                            }
                        }
                    }
                }
                MessageBox.Show("선택된 그룹이 없습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
        private void ContextMenu_SelectSocket_Click(object sender, EventArgs e)
        {
            try
            {
                var doc = this.SiriusViewer_JogPopup.Document;
                if (doc == null)
                    return;
                foreach (var layer in doc.Layers)
                {
                    if (!layer.IsMarkerable || layer.Count == 0)
                        continue;
                    int idx = 0;
                    if (layer.Name == "Hole1")
                    {
                        foreach (var entity in layer)
                        {
                            if (entity.EntityType == EType.Group)
                            {
                                var group = entity as Group;
                                if (group != null && group.IsSelected)
                                {
                                    _selectedSocketIndex = idx;
                                    if (workStage != null)
                                        workStage.m_nSelectedSocket_Index = idx;
                                    MessageBox.Show($"선택된 Socket Index: {idx + 1}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    SiriusViewer_JogPopup.Invalidate();
                                    return;
                                }
                            }
                            idx++;
                        }
                    }
                }
                _selectedSocketIndex = -1;
                if (workStage != null) workStage.m_nSelectedSocket_Index = -1;
                MessageBox.Show("선택된 Socket이 없습니다.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }
        // ======================================================================

        private void HookViewerEvents()
        {
            if (_viewerHooked)
                return;
            var gl = this.SiriusViewer_JogPopup?.GLcontrol;
            if (gl == null) return;
            gl.MouseDown += ViewerGL_MouseDown;
            gl.MouseDoubleClick += ViewerGL_MouseDoubleClick;
            gl.MouseClick += GLcontrol_MouseClick; // 메인과 동일하게 우클릭 컨텍스트 메뉴
            gl.ContextMenuStrip = _viewerContextMenu;
            this.SiriusViewer_JogPopup.Paint += SiriusViewer_JogPopup_Paint;
            _viewerHooked = true;
        }

        private void ViewerGL_MouseDown(object sender, MouseEventArgs e)
        {
            _lastMouseDownPoint = e.Location;
            if (e.Button == MouseButtons.Left && ModifierKeys == Keys.Control)
            {
                SelectSocketAtPoint(e.Location);
                SiriusViewer_JogPopup.Invalidate();
            }
        }
        private void ViewerGL_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Equipment.AutoManualStatus || Equipment.AutoRunStatus)
                return;
            try
            {
                if (sender is OpenGLControl)
                {
                    var Document = this.SiriusViewer_JogPopup.Document;
                    if (Document.Views.Count == 0)
                    {
                        IView view = new ViewDefault(Document, SiriusViewer_JogPopup.GLcontrol);
                        Document.Views.Add(view);
                    }
                    if (Document.Views.Count > 0)
                    {
                        var view = Document.Views.Last();
                        view.Dp2Lp(e.Location, out float dX, out float dY);
                        XyzCoordinate ptReal = new XyzCoordinate(dX, dY, 0);
                        if (Equipment.AutoManualStatus == false)
                        {
                            if (Equipment._InitDeviceStatus.MotionIo)
                            {
                                XyzCoordinate v = workStage.ConvertPointFineCam(new XyzCoordinate(ptReal.X, ptReal.Y, 0));
                                v = (XyzCoordinate)workStage.ConvertPreAlignData(new XyCoordinate(v.X, v.Y));
                                workStage.MovetoWorkStage_ABS_PositionsXY(new XyCoordinate(v.X, v.Y), Type_Motor_Speed.Process);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("초기화 되지 않았습니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("설비가 Manual 상태가 아닙니다.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
        }

        private void GLcontrol_MouseClick(object sender, MouseEventArgs e)
        {
            if (Equipment.AutoRunStatus) return; // AutoRun 중이면 금지
            if (e.Button == MouseButtons.Right)
            {
                _mouseDownLocation = e.Location;
                if (_viewerContextMenu != null)
                    _viewerContextMenu.Show(SiriusViewer_JogPopup.GLcontrol, e.Location);
            }
        }

        private void SiriusViewer_JogPopup_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (workStage == null || workStage.Main_SocketPositions == null) return;
                if (_selectedSocketIndex < 0 || _selectedSocketIndex >= workStage.Main_SocketPositions.Count) return;
                var pt = workStage.Main_SocketPositions[_selectedSocketIndex];
                var screen = StageToScreen(pt.X, pt.Y);
                int r = 10;
                using (var pen = new Pen(Color.Red, 2))
                    e.Graphics.DrawEllipse(pen, screen.X - r, screen.Y - r, r * 2, r * 2);
            }
            catch { }
        }

        private XyCoordinate ScreenToStage(System.Drawing.Point screenPt)
        {
            double cx = this.SiriusViewer_JogPopup.Width / 2.0;
            double cy = this.SiriusViewer_JogPopup.Height / 2.0;
            return new XyCoordinate
            {
                X = (screenPt.X - cx) * _viewerScale_mmPerPixel,
                Y = (cy - screenPt.Y) * _viewerScale_mmPerPixel
            };
        }
        private PointF StageToScreen(double stageX, double stageY)
        {
            double cx = this.SiriusViewer_JogPopup.Width / 2.0;
            double cy = this.SiriusViewer_JogPopup.Height / 2.0;
            return new PointF((float)(cx + stageX / _viewerScale_mmPerPixel), (float)(cy - stageY / _viewerScale_mmPerPixel));
        }

        private void SelectSocketAtPoint(System.Drawing.Point screenPt)
        {
            if (workStage == null || workStage.Main_SocketPositions == null || workStage.Main_SocketPositions.Count == 0) return;
            try
            {
                var stagePos = ScreenToStage(screenPt);
                double min = double.MaxValue; int idx = -1;
                for (int i = 0; i < workStage.Main_SocketPositions.Count; i++)
                {
                    var sp = workStage.Main_SocketPositions[i];
                    double dx = sp.X - stagePos.X; double dy = sp.Y - stagePos.Y; double d = dx * dx + dy * dy;
                    if (d < min) { min = d; idx = i; }
                }
                if (idx >= 0)
                {
                    _selectedSocketIndex = idx;
                    workStage.m_nSelectedSocket_Index = idx;
                }
            }
            catch (Exception ex) { Log.Write(ex); }
        }

        private void MoveToSelectedSocket()
        {
            if (_selectedSocketIndex < 0) return;
            if (workStage == null || workStage.Main_SocketPositions == null) return;
            if (_selectedSocketIndex >= workStage.Main_SocketPositions.Count) return;
            var pt = workStage.Main_SocketPositions[_selectedSocketIndex];
            MoveStageToStageXY(pt.X, pt.Y);
        }
        private void MoveToSelectedGroupCenter()
        {
            if (workStage == null || workStage.Main_SocketPositions == null || workStage.Main_SocketPositions.Count == 0) return;
            if (_selectedSocketIndex >= 0 && _selectedSocketIndex < workStage.Main_SocketPositions.Count)
            { var pt = workStage.Main_SocketPositions[_selectedSocketIndex]; MoveStageToStageXY(pt.X, pt.Y); return; }
            double sx = 0, sy = 0; foreach (var p in workStage.Main_SocketPositions) { sx += p.X; sy += p.Y; }
            sx /= workStage.Main_SocketPositions.Count; sy /= workStage.Main_SocketPositions.Count; MoveStageToStageXY(sx, sy);
        }


        private void MoveStageToMouse(System.Drawing.Point screenPt)
        {
            try
            {
                if (workStage == null || !Equipment.AjinBoard_Opened) return;
                var st = ScreenToStage(screenPt);
                workStage.MovetoWorkStage_ABS_PositionsXY(st, Type_Motor_Speed.Coarse, 1);
            }
            catch (Exception ex) { Log.Write(ex); }
        }


        private void MoveStageToStageXY(double x, double y)
        { try { if (workStage == null || !Equipment.AjinBoard_Opened) return; workStage.MovetoWorkStage_ABS_PositionsXY(new XyCoordinate { X = x, Y = y }, Type_Motor_Speed.Coarse, 1); } catch (Exception ex) { Log.Write(ex); } }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        { if (keyData == Keys.Enter || keyData == Keys.Space) return true; return base.ProcessCmdKey(ref msg, keyData); }

        private void FormNew_JogPopup_FormClosing(object sender, FormClosingEventArgs e)
        { if (e.CloseReason == CloseReason.UserClosing) { e.Cancel = true; OnHide(); this.Hide(); } }

        private void FormNewSub_JogPopup_Load(object sender, EventArgs e)
        {
            if (m_bInitialized) return;
            ModuleCollection modules = Equipment.Modules;
            foreach (Module m in modules)
            {
                if (m.Name == "WorkStage") workStage = m as WorkStage;
                else if (m.Name == "Loader") loader = m as Loader;
                else if (m.Name == "Unloader") unloader = m as Unloader;
            }
            if (workStage != null)
            { workStage.ActionSiriusViewerRefresy -= OnWorkStageSiriusRefresh; workStage.ActionSiriusViewerRefresy += OnWorkStageSiriusRefresh; }
            InitializeTabs();
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (workStage != null && workStage.IsModuleClose) break;
                    Thread.Sleep(200);
                    Timer_Status_Tick(null, null);
                    if (_pendingDocSync && workStage != null && workStage.m_nLaserDrilling_MainStep == (int)WorkStage.LaserDrilling_Step.None)
                        SyncSiriusDocumentIfNeeded(true);
                }
            });
            m_bInitialized = true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!this.Created) return;
            if (this.Visible && !m_bFormVisible) { m_bFormVisible = true; OnShow(); SyncSiriusDocumentIfNeeded(false); }
            else if (!this.Visible && m_bFormVisible) { m_bFormVisible = false; OnHide(); }
        }

        private void OnShow()
        {
            tabPage_Loader.Select();
            userform_Loader.OnShow();
            InitSiriusViewer();
            SyncSiriusDocumentIfNeeded();
        }
        private void OnHide()
        {
            userform_Loader?.OnHide();
            userform_Stage?.OnHide();
            userform_Unloader?.OnHide();
        }

        private void Timer_Status_Tick(object sender, EventArgs e)
        {
            try
            {
                if (SelectedTabPage == tabPage_Loader) userform_Loader?.UpdateStatus();
                else if (SelectedTabPage == tabPage_Stage) userform_Stage?.UpdateStatus();
                else if (SelectedTabPage == tabPage_Unloader) userform_Unloader?.UpdateStatus();
            }
            catch (Exception ex) { Log.Write(ex); }
        }

        private void InitializeTabs()
        {
            userform_Loader = new FormNewSub_JogPopup_Loader(this);
            userform_Stage = new FormNewSub_JogPopup_Stage(this);
            userform_Unloader = new FormNewSub_JogPopup_Unloader(this);
            userform_Loader.Dock = DockStyle.Fill; userform_Stage.Dock = DockStyle.Fill; userform_Unloader.Dock = DockStyle.Fill;
            tabPage_Loader.Controls.Add(userform_Loader);
            tabPage_Stage.Controls.Add(userform_Stage);
            tabPage_Unloader.Controls.Add(userform_Unloader);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            userform_Loader.OnHide(); userform_Stage.OnHide(); userform_Unloader.OnHide();
            switch (tabControl_JogPopup.SelectedTab.Name)
            {
                case "tabPage_Loader": userform_Loader.OnShow(); break;
                case "tabPage_Stage": userform_Stage.OnShow(); break;
                case "tabPage_Unloader": userform_Unloader.OnShow(); break;
            }
            SelectedTabPage = (sender as TabControl).SelectedTab;
        }

        public interface IJogControlProvider
        { bool IsJogMoveContinuous { get; } bool IsJogMoveStep { get; } double JogStepDistance { get; } Type_Motor_Speed JogSpeedType { get; } }

        public async void button_AxisJog_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button; if (btn == null || btn.Tag == null) return;
            if (Equipment.AjinBoard_Opened)
            {
                var parts = btn.Tag.ToString().Split(','); if (parts.Length != 3) return;
                string unit = parts[0]; string axisName = parts[1]; double direction = Convert.ToDouble(parts[2]);
                int axis = GetAxisIndex(ref unit, axisName); if (axis < 0) return;
                Type_Motor_Speed sp = GetMotorSpeedType(unit);
                if (IsJogMoveContinuous(unit))
                {
                    switch (unit)
                    {
                        case "WorkStage": workStage.MovetoWorkStage_Jog_Positions((WorkStage.nAxis)axis, (int)direction, sp); break;
                        case "Loader": loader.MovetoLoader_Jog_Positions((Loader.nAxis)axis, (int)direction, sp); break;
                        case "Unloader": unloader.MovetoUnloader_Jog_Positions((Unloader.nAxis)axis, (int)direction, sp); break;
                    }
                }
                else if (IsJogMoveStep(unit))
                {
                    double dist = GetStepDistance(unit);
                    switch (unit)
                    {
                        case "WorkStage": workStage.MovetoWorkStage_Rel_Positions((WorkStage.nAxis)axis, dist, (int)direction, sp); break;
                        case "Loader": loader.MovetoLoader_Rel_Positions((Loader.nAxis)axis, dist, (int)direction, sp); break;
                        case "Unloader": unloader.MovetoUnloader_Rel_Positions((Unloader.nAxis)axis, dist, (int)direction, sp); break;
                    }
                }
            }
        }
        public void button_AxisJog_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button; if (btn == null || btn.Tag == null) return; if (!Equipment.AjinBoard_Opened) return;
            var parts = btn.Tag.ToString().Split(','); if (parts.Length < 2) return; string unit = parts[0]; string axisName = parts[1]; int axis = GetAxisIndex(ref unit, axisName); if (axis < 0) return;
            if (IsJogMoveContinuous(unit))
            {
                switch (unit)
                {
                    case "WorkStage": workStage.MC_Func.MC_JogStop(axis); break;
                    case "Loader": loader.MC_Func.MC_JogStop(axis); break;
                    case "Unloader": unloader.MC_Func.MC_JogStop(axis); break;
                }
            }
        }

        private int GetAxisIndex(ref string unit, string axisName)
        { try { if (unit == "WorkStage") return (int)Enum.Parse(typeof(WorkStage.nAxis), axisName); if (unit == "Loader") return (int)Enum.Parse(typeof(Loader.nAxis), axisName); if (unit == "Unloader") return (int)Enum.Parse(typeof(Unloader.nAxis), axisName); } catch (Exception ex) { Log.Write(ex); } return -1; }
        private IJogControlProvider GetJogProviderByUnit(string unit)
        { if (unit == "Loader") return userform_Loader; if (unit == "WorkStage") return userform_Stage; if (unit == "Unloader") return userform_Unloader; return null; }
        private Type_Motor_Speed GetMotorSpeedType(string unit) => GetJogProviderByUnit(unit)?.JogSpeedType ?? Type_Motor_Speed.Fine;
        private bool IsJogMoveContinuous(string unit) => GetJogProviderByUnit(unit)?.IsJogMoveContinuous ?? false;
        private bool IsJogMoveStep(string unit) => GetJogProviderByUnit(unit)?.IsJogMoveStep ?? false;
        private double GetStepDistance(string unit) => GetJogProviderByUnit(unit)?.JogStepDistance ?? 0.0;

        private void SyncSiriusDocumentIfNeeded(bool force = false)
        {
            try
            {
                if (workStage == null) return;
                var doc = Equipment.GetEqpSiriusViewerDocument();
                if (doc == null) return;
                bool busy = workStage.m_nLaserDrilling_MainStep != (int)WorkStage.LaserDrilling_Step.None;
                if (!force && busy) { _pendingDocSync = true; return; }
                if (SiriusViewer_JogPopup.Document != doc) SiriusViewer_JogPopup.Document = doc;
                SiriusViewer_JogPopup.Invalidate();
                _pendingDocSync = false;
            }
            catch (Exception ex) { Log.Write(ex); }
        }

        private void OnWorkStageSiriusRefresh(bool req)
        { if (req) SyncSiriusDocumentIfNeeded(false); }
        public void ForceRefreshDrawing() => SyncSiriusDocumentIfNeeded(true);
    }
}
