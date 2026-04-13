using Newtonsoft.Json;
using QMC.Common;
using SpiralLab.Sirius;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SLD200.NewStyleForm.NewSubForm
{
    public partial class FormNewSub_ScannerCal3D : Form
    {
        // 현재 스캐너 보정 파일
        private string m_srcFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "cor_1to1.ct5");
        // 신규 생성 파일
        private string m_targetFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction", "focus_3d.ct5");

        private float m_fieldSize = 105;
        private float m_rowInterval = 2;
        private float m_colInterval = 2;
        private int m_row = 5;
        private int m_col = 5;
        private float m_kfactor = 0;

        private BindingList<PlaneFileItem> m_planeFiles = new BindingList<PlaneFileItem>();

        private Correction3DRtc m_correction3DRtc = null;
        private Correction3DRtcForm m_correction3DRtcForm = null;

        public FormNewSub_ScannerCal3D()
        {
            InitializeComponent();

            InitializeScannerParameter();
            InitializeEmbeddedPreviewForm();
            InitializeGrid();
            InitializeEvent();

            textBoxSourceFile.Text = m_srcFile;
            textBoxTargetFile.Text = m_targetFile;
        }

        #region Init

        private void InitializeScannerParameter()
        {
            if (Equipment.Machine_LaserType_CO2)
            {
                float fov = 72.5f;
                float kfactor = (float)Math.Pow(2, 20) / fov;

                if (kfactor == 0)
                    kfactor = 14461.43448275862f;

                m_fieldSize = fov;
                m_kfactor = kfactor;
            }

            if (m_kfactor <= 0)
                m_kfactor = (float)Math.Pow(2, 20) / m_fieldSize;
        }

        private void InitializeEmbeddedPreviewForm()
        {
            // FocusCalibration은 정적 함수이지만
            // 기존 UI preview form을 유지하려면 dummy Correction3DRtc 생성만 해둠
            m_correction3DRtc = new Correction3DRtc(
                m_kfactor,
                m_row,
                m_col,
                m_rowInterval,
                1.0f,
                -1.0f,
                m_srcFile,
                m_targetFile);

            m_correction3DRtcForm = new Correction3DRtcForm(m_correction3DRtc);
            m_correction3DRtcForm.TopLevel = false;
            m_correction3DRtcForm.FormBorderStyle = FormBorderStyle.None;
            m_correction3DRtcForm.Dock = DockStyle.Fill;
            panelPreviewHost.Controls.Add(m_correction3DRtcForm);
            m_correction3DRtcForm.Show();
        }

        private void InitializeEvent()
        {
            buttonBrowseSource.Click += ButtonBrowseSource_Click;
            buttonBrowseTarget.Click += ButtonBrowseTarget_Click;
            buttonAddFiles.Click += ButtonAddFiles_Click;
            buttonRemove.Click += ButtonRemove_Click;
            buttonAutoFillZ.Click += ButtonAutoFillZ_Click;
            buttonSortByZ.Click += ButtonSortByZ_Click;
            buttonValidate.Click += ButtonValidate_Click;
            buttonSave.Click += ButtonSave_Click;

            dataGridViewPlaneFiles.SelectionChanged += DataGridViewPlaneFiles_SelectionChanged;
            dataGridViewPlaneFiles.CellEndEdit += DataGridViewPlaneFiles_CellEndEdit;
            dataGridViewPlaneFiles.CellFormatting += DataGridViewPlaneFiles_CellFormatting;
            dataGridViewPlaneFiles.CurrentCellDirtyStateChanged += DataGridViewPlaneFiles_CurrentCellDirtyStateChanged;
        }

        private void InitializeGrid()
        {
            dataGridViewPlaneFiles.AutoGenerateColumns = false;
            dataGridViewPlaneFiles.AllowUserToAddRows = false;
            dataGridViewPlaneFiles.AllowUserToDeleteRows = false;
            dataGridViewPlaneFiles.AllowUserToResizeRows = false;
            dataGridViewPlaneFiles.MultiSelect = false;
            dataGridViewPlaneFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlaneFiles.RowHeadersVisible = false;

            dataGridViewPlaneFiles.Columns.Clear();

            dataGridViewPlaneFiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "No",
                DataPropertyName = nameof(PlaneFileItem.No),
                Width = 40,
                ReadOnly = true
            });

            dataGridViewPlaneFiles.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Use",
                DataPropertyName = nameof(PlaneFileItem.Use),
                Width = 45
            });

            dataGridViewPlaneFiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "File Name",
                DataPropertyName = nameof(PlaneFileItem.FileName),
                Width = 200,
                ReadOnly = true
            });

            dataGridViewPlaneFiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Z(mm)",
                DataPropertyName = nameof(PlaneFileItem.ZValue),
                Width = 70
            });

            dataGridViewPlaneFiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Rows",
                DataPropertyName = nameof(PlaneFileItem.Rows),
                Width = 50,
                ReadOnly = true
            });

            dataGridViewPlaneFiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cols",
                DataPropertyName = nameof(PlaneFileItem.Cols),
                Width = 50,
                ReadOnly = true
            });

            dataGridViewPlaneFiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "PointCnt",
                DataPropertyName = nameof(PlaneFileItem.PointCount),
                Width = 70,
                ReadOnly = true
            });

            dataGridViewPlaneFiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "MinScore",
                DataPropertyName = nameof(PlaneFileItem.MinScore),
                Width = 85,
                ReadOnly = true
            });

            dataGridViewPlaneFiles.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Status",
                DataPropertyName = nameof(PlaneFileItem.Status),
                Width = 80,
                ReadOnly = true
            });

            dataGridViewPlaneFiles.DataSource = m_planeFiles;
        }

        #endregion

        #region JSON Load

        private ZPlaneMeasureFile LoadZPlaneJson(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"파일이 없습니다: {filePath}");

            string json = File.ReadAllText(filePath);
            ZPlaneMeasureFile data = JsonConvert.DeserializeObject<ZPlaneMeasureFile>(json);

            if (data == null)
                throw new Exception($"JSON 파싱 실패: {filePath}");

            if (data.Points == null || data.Points.Count == 0)
                throw new Exception($"측정 포인트가 비어 있음: {filePath}");

            return data;
        }

        private void AddPlaneFile(string filePath)
        {
            if (m_planeFiles.Any(x => string.Equals(x.FilePath, filePath, StringComparison.OrdinalIgnoreCase)))
            {
                WriteLog($"중복 파일 제외: {filePath}");
                return;
            }

            ZPlaneMeasureFile planeData = LoadZPlaneJson(filePath);

            PlaneFileItem item = new PlaneFileItem
            {
                No = m_planeFiles.Count + 1,
                Use = true,
                FileName = Path.GetFileName(filePath),
                FilePath = filePath,
                ZValue = planeData.Z,
                Rows = planeData.Rows,
                Cols = planeData.Cols,
                PitchX = planeData.PitchX,
                PitchY = planeData.PitchY,
                PointCount = planeData.Points.Count,
                MinX = planeData.Points.Min(p => p.RefX),
                MaxX = planeData.Points.Max(p => p.RefX),
                MinY = planeData.Points.Min(p => p.RefY),
                MaxY = planeData.Points.Max(p => p.RefY),
                MinScore = planeData.Points.Min(p => p.Score),
                Status = "Loaded",
                RawData = planeData
            };

            m_planeFiles.Add(item);

            if (checkBoxAutoSort.Checked)
                SortPlaneFilesByZ();

            RefreshPlaneNo();
            RefreshBasePlaneCombo();
            WriteLog($"파일 로드 완료: {item.FileName}");
        }

        #endregion

        #region Button Event

        private void ButtonBrowseSource_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Correction File (*.ct5)|*.ct5|All Files (*.*)|*.*";
                ofd.FileName = textBoxSourceFile.Text;

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                textBoxSourceFile.Text = ofd.FileName;
                m_srcFile = ofd.FileName;
                WriteLog($"Source CT5 변경: {m_srcFile}");
            }
        }

        private void ButtonBrowseTarget_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Correction File (*.ct5)|*.ct5|All Files (*.*)|*.*";
                sfd.FileName = Path.GetFileName(textBoxTargetFile.Text);

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                textBoxTargetFile.Text = sfd.FileName;
                m_targetFile = sfd.FileName;
                WriteLog($"Target CT5 변경: {m_targetFile}");
            }
        }

        private void ButtonAddFiles_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = true;
                ofd.Filter = "Json File (*.json)|*.json|All Files (*.*)|*.*";

                if (ofd.ShowDialog() != DialogResult.OK)
                    return;

                foreach (string filePath in ofd.FileNames)
                {
                    try
                    {
                        AddPlaneFile(filePath);
                    }
                    catch (Exception ex)
                    {
                        WriteLog($"파일 추가 실패: {Path.GetFileName(filePath)} / {ex.Message}");
                    }
                }

                ValidateAllPlaneFiles();
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            if (dataGridViewPlaneFiles.CurrentRow == null)
                return;

            PlaneFileItem item = dataGridViewPlaneFiles.CurrentRow.DataBoundItem as PlaneFileItem;
            if (item == null)
                return;

            m_planeFiles.Remove(item);
            RefreshPlaneNo();
            RefreshBasePlaneCombo();
            ClearSelectedInfo();

            WriteLog($"파일 제거: {item.FileName}");
        }

        private void ButtonAutoFillZ_Click(object sender, EventArgs e)
        {
            foreach (PlaneFileItem item in m_planeFiles)
            {
                double z = ExtractZValueFromFileName(Path.GetFileNameWithoutExtension(item.FileName));
                if (Math.Abs(z) > 0.000001)
                    item.ZValue = (float)z;
            }

            dataGridViewPlaneFiles.Refresh();

            if (checkBoxAutoSort.Checked)
                SortPlaneFilesByZ();

            WriteLog("파일명 기준 Z 자동 입력 완료");
        }

        private void ButtonSortByZ_Click(object sender, EventArgs e)
        {
            SortPlaneFilesByZ();
            WriteLog("Z 기준 정렬 완료");
        }

        private void ButtonValidate_Click(object sender, EventArgs e)
        {
            ValidateAllPlaneFiles();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<PlaneFileItem> usePlanes = m_planeFiles
                    .Where(x => x.Use)
                    .OrderBy(x => x.ZValue)
                    .ToList();

                if (usePlanes.Count < 2)
                    throw new Exception("FocusCalibration은 최소 2개 이상의 plane 파일이 필요합니다.");

                ValidateAllPlaneFiles();

                List<PlaneFileItem> invalids = usePlanes.Where(x => x.Status != "OK").ToList();
                if (invalids.Count > 0)
                    throw new Exception("검증 실패 상태의 plane 파일이 있습니다.");

                List<ZPlaneMeasureFile> rawPlanes = usePlanes
                    .Select(x => ClonePlaneWithUiZ(x))
                    .ToList();

                ScannerFocusCalibrationBuilder builder = new ScannerFocusCalibrationBuilder();
                ScannerFocusCalibrationBuilder.BuildResult result = builder.Build(
                    rawPlanes,
                    textBoxSourceFile.Text,
                    textBoxTargetFile.Text,
                    m_kfactor,
                    null);

                if (!result.Success)
                    throw new Exception(result.Message);

                WriteLog(result.Message);
                WriteAggregateSummary(result.Aggregates);

                MessageBox.Show("FocusCalibration 완료", "완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                WriteLog($"Build 실패: {ex.Message}");
                MessageBox.Show(ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Grid Event

        private void DataGridViewPlaneFiles_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewPlaneFiles.CurrentRow == null)
                return;

            PlaneFileItem item = dataGridViewPlaneFiles.CurrentRow.DataBoundItem as PlaneFileItem;
            if (item == null)
                return;

            ShowSelectedInfo(item);
        }

        private void DataGridViewPlaneFiles_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (checkBoxAutoSort.Checked)
                SortPlaneFilesByZ();

            RefreshPlaneNo();
            ValidateAllPlaneFiles();
        }

        private void DataGridViewPlaneFiles_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewPlaneFiles.IsCurrentCellDirty)
                dataGridViewPlaneFiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void DataGridViewPlaneFiles_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewPlaneFiles.Columns[e.ColumnIndex].DataPropertyName != nameof(PlaneFileItem.Status))
                return;

            if (e.Value == null)
                return;

            string status = e.Value.ToString();

            if (status == "OK")
            {
                e.CellStyle.ForeColor = Color.Green;
                e.CellStyle.Font = new Font(dataGridViewPlaneFiles.Font, FontStyle.Bold);
            }
            else if (status == "Warning")
            {
                e.CellStyle.ForeColor = Color.DarkOrange;
                e.CellStyle.Font = new Font(dataGridViewPlaneFiles.Font, FontStyle.Bold);
            }
            else
            {
                e.CellStyle.ForeColor = Color.Red;
                e.CellStyle.Font = new Font(dataGridViewPlaneFiles.Font, FontStyle.Bold);
            }
        }

        #endregion

        #region Validate / UI

        private void ValidateAllPlaneFiles()
        {
            if (m_planeFiles.Count == 0)
                return;

            PlaneFileItem refItem = m_planeFiles.FirstOrDefault(x => x.Use) ?? m_planeFiles[0];

            Dictionary<string, GridMeasurePoint> refMap = refItem.RawData.Points
                .ToDictionary(p => GetKey(p.IndexX, p.IndexY), p => p);

            HashSet<double> duplicatedZ = new HashSet<double>(
                m_planeFiles.Where(x => x.Use)
                           .GroupBy(x => Math.Round(x.ZValue, 6))
                           .Where(g => g.Count() > 1)
                           .Select(g => g.Key));

            foreach (PlaneFileItem item in m_planeFiles)
            {
                item.Status = "OK";
                item.StatusMessage = string.Empty;

                if (item.RawData == null || item.RawData.Points == null || item.RawData.Points.Count == 0)
                {
                    item.Status = "Error";
                    item.StatusMessage = "포인트 없음";
                    continue;
                }

                if (item.RawData.Rows != refItem.RawData.Rows ||
                    item.RawData.Cols != refItem.RawData.Cols ||
                    item.RawData.Points.Count != refItem.RawData.Points.Count)
                {
                    item.Status = "Error";
                    item.StatusMessage = "Rows/Cols/Count mismatch";
                    continue;
                }

                if (Math.Abs(item.RawData.PitchX - refItem.RawData.PitchX) > 0.000001f ||
                    Math.Abs(item.RawData.PitchY - refItem.RawData.PitchY) > 0.000001f)
                {
                    item.Status = "Error";
                    item.StatusMessage = "Pitch mismatch";
                    continue;
                }

                bool indexMatch = true;
                foreach (GridMeasurePoint pt in item.RawData.Points)
                {
                    if (!refMap.TryGetValue(GetKey(pt.IndexX, pt.IndexY), out GridMeasurePoint refPt))
                    {
                        indexMatch = false;
                        break;
                    }

                    if (Math.Abs(refPt.RefX - pt.RefX) > 0.000001f ||
                        Math.Abs(refPt.RefY - pt.RefY) > 0.000001f)
                    {
                        indexMatch = false;
                        break;
                    }
                }

                if (!indexMatch)
                {
                    item.Status = "Error";
                    item.StatusMessage = "Index/RefXY mismatch";
                    continue;
                }

                if (checkBoxDuplicateCheck.Checked && duplicatedZ.Contains(Math.Round(item.ZValue, 6)))
                {
                    item.Status = "Error";
                    item.StatusMessage = "Duplicated Z";
                    continue;
                }

                item.Status = "OK";
            }

            dataGridViewPlaneFiles.Refresh();

            if (dataGridViewPlaneFiles.CurrentRow != null)
            {
                PlaneFileItem current = dataGridViewPlaneFiles.CurrentRow.DataBoundItem as PlaneFileItem;
                if (current != null)
                    ShowSelectedInfo(current);
            }

            WriteLog("Plane 파일 검증 완료");
        }

        private void RefreshPlaneNo()
        {
            for (int i = 0; i < m_planeFiles.Count; i++)
                m_planeFiles[i].No = i + 1;

            dataGridViewPlaneFiles.Refresh();
        }

        private void RefreshBasePlaneCombo()
        {
            comboBoxBasePlane.Items.Clear();

            foreach (PlaneFileItem item in m_planeFiles)
                comboBoxBasePlane.Items.Add(item.FileName);

            if (comboBoxBasePlane.Items.Count > 0 && comboBoxBasePlane.SelectedIndex < 0)
                comboBoxBasePlane.SelectedIndex = 0;
        }

        private void SortPlaneFilesByZ()
        {
            List<PlaneFileItem> sorted = m_planeFiles.OrderBy(x => x.ZValue).ToList();

            m_planeFiles.RaiseListChangedEvents = false;
            m_planeFiles.Clear();
            foreach (PlaneFileItem item in sorted)
                m_planeFiles.Add(item);
            m_planeFiles.RaiseListChangedEvents = true;
            m_planeFiles.ResetBindings();

            RefreshPlaneNo();
            RefreshBasePlaneCombo();
        }

        private void ShowSelectedInfo(PlaneFileItem item)
        {
            labelSelectedFilePath.Text = item.FilePath;
            labelSelectedMinX.Text = item.MinX.ToString("0.000");
            labelSelectedMaxX.Text = item.MaxX.ToString("0.000");
            labelSelectedMinY.Text = item.MinY.ToString("0.000");
            labelSelectedMaxY.Text = item.MaxY.ToString("0.000");
            labelSelectedPointCount.Text = item.PointCount.ToString();

            bool ok = item.Status == "OK";
            labelSelectedGridMatch.Text = ok ? "OK" : "NG";
            labelSelectedGridMatch.ForeColor = ok ? Color.Green : Color.Red;
        }

        private void ClearSelectedInfo()
        {
            labelSelectedFilePath.Text = "-";
            labelSelectedMinX.Text = "-";
            labelSelectedMaxX.Text = "-";
            labelSelectedMinY.Text = "-";
            labelSelectedMaxY.Text = "-";
            labelSelectedPointCount.Text = "-";
            labelSelectedGridMatch.Text = "-";
            labelSelectedGridMatch.ForeColor = Color.Black;
        }

        private void WriteLog(string message)
        {
            richTextBoxLog.AppendText($"{DateTime.Now:HH:mm:ss} {message}{Environment.NewLine}");
            richTextBoxLog.ScrollToCaret();
        }

        private void WriteAggregateSummary(List<FocusPointAggregate> aggregates)
        {
            if (aggregates == null || aggregates.Count == 0)
                return;

            float minZ = aggregates.Min(x => x.BestZ);
            float maxZ = aggregates.Max(x => x.BestZ);
            float avgZ = aggregates.Average(x => x.BestZ);

            WriteLog($"Aggregate Count : {aggregates.Count}");
            WriteLog($"Best Z Range    : {minZ:0.000} ~ {maxZ:0.000}");
            WriteLog($"Best Z Average  : {avgZ:0.000}");
        }

        #endregion

        #region Helper

        private ZPlaneMeasureFile ClonePlaneWithUiZ(PlaneFileItem item)
        {
            return new ZPlaneMeasureFile
            {
                Z = item.ZValue,
                Rows = item.RawData.Rows,
                Cols = item.RawData.Cols,
                PitchX = item.RawData.PitchX,
                PitchY = item.RawData.PitchY,
                Points = item.RawData.Points.Select(p => new GridMeasurePoint
                {
                    IndexX = p.IndexX,
                    IndexY = p.IndexY,
                    RefX = p.RefX,
                    RefY = p.RefY,
                    MeasuredX = p.MeasuredX,
                    MeasuredY = p.MeasuredY,
                    Score = p.Score
                }).ToList()
            };
        }

        private string GetKey(int ix, int iy)
        {
            return $"{ix}_{iy}";
        }

        private double ExtractZValueFromFileName(string fileNameWithoutExt)
        {
            Match m1 = Regex.Match(fileNameWithoutExt, @"Z(?<z>[+-]?\d+(\.\d+)?)", RegexOptions.IgnoreCase);
            if (m1.Success)
                return Convert.ToDouble(m1.Groups["z"].Value);

            Match m2 = Regex.Match(fileNameWithoutExt, @"(?<sign>[PM])(?<num>\d+)", RegexOptions.IgnoreCase);
            if (m2.Success)
            {
                string sign = m2.Groups["sign"].Value.ToUpper();
                double num = Convert.ToDouble(m2.Groups["num"].Value) / 100.0;
                if (sign == "M")
                    num *= -1.0;
                return num;
            }

            return 0.0;
        }

        private void Build3DFromJsonFiles(List<string> jsonFiles)
        {
            if (jsonFiles == null || jsonFiles.Count < 2)
                throw new Exception("최소 2개 이상의 Z plane JSON 파일이 필요합니다.");

            var planes = new List<ZPlaneMeasureFile>();
            foreach (var f in jsonFiles)
                planes.Add(LoadZPlaneJson(f));

            // 검증: rows/cols/pitch 동일해야 함
            var p0 = planes[0];
            foreach (var p in planes)
            {
                if (p.Rows != p0.Rows || p.Cols != p0.Cols)
                    throw new Exception("Rows/Cols가 서로 다릅니다.");
                if (Math.Abs(p.PitchX - p0.PitchX) > 1e-6 || Math.Abs(p.PitchY - p0.PitchY) > 1e-6)
                    throw new Exception("PitchX/PitchY가 서로 다릅니다.");
            }

            // (IndexX,IndexY)별 best Z 계산
            var focusInput = new List<Tuple<float, float, float>>();

            var keys = p0.Points.Select(x => $"{x.IndexX}_{x.IndexY}").Distinct().ToList();

            foreach (var key in keys)
            {
                GridMeasurePoint bestPt = null;
                float bestZ = 0f;
                float bestScore = float.MaxValue;

                foreach (var plane in planes)
                {
                    var pt = plane.Points.FirstOrDefault(x => $"{x.IndexX}_{x.IndexY}" == key);
                    if (pt == null) continue;

                    if (pt.Score < bestScore)
                    {
                        bestScore = pt.Score;
                        bestPt = pt;
                        bestZ = plane.Z;
                    }
                }

                if (bestPt == null)
                    throw new Exception($"포인트 누락: {key}");

                focusInput.Add(Tuple.Create(bestPt.RefX, bestPt.RefY, bestZ));
            }

            Calibration3DReturnCode rc;
            bool ok = Correction3DRtc.FocusCalibration(
                focusInput.ToArray(),
                m_srcFile,       // source ct5
                null,            // readme 없으면 null
                m_kfactor,
                m_targetFile,    // output ct5
                out rc);

            if (!ok)
                throw new Exception($"FocusCalibration 실패: {rc}");
        }


        #endregion
    }

    public class PlaneFileItem
    {
        public int No { get; set; }
        public bool Use { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public float ZValue { get; set; }

        public int Rows { get; set; }
        public int Cols { get; set; }
        public float PitchX { get; set; }
        public float PitchY { get; set; }

        public int PointCount { get; set; }
        public float MinX { get; set; }
        public float MaxX { get; set; }
        public float MinY { get; set; }
        public float MaxY { get; set; }
        public float MinScore { get; set; }

        public string Status { get; set; }
        public string StatusMessage { get; set; }

        public ZPlaneMeasureFile RawData { get; set; }
    }
}