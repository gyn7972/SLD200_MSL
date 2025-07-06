using QMC.Common;
using QMC.Common.Recipe;
using SLD200.NewStyleForm.NewSubForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200.NewStyleForm
{
    public partial class FormNew_CalFilePopup : Form
    {
        private bool m_bFormVisible = false; // 실제 Show 상태 여부
        private bool m_bInitialized = false;

        private System.Windows.Forms.Timer timer_Status;

        public FormNew_CalFilePopup()
        {
            InitializeComponent();

            this.Load += FormNew_CalFilePopup_Load; // 여기서 Load 이벤트 연결
            this.FormClosing += FormNew_CalFilePopup_FormClosing; // 추가
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // 엔터 또는 스페이스 키 눌렀을 때 무시
            if (keyData == Keys.Enter || keyData == Keys.Space)
                return true;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void FormNew_CalFilePopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // 폼이 종료되지 않도록 방지
                OnHide();
                this.Hide();     // 대신 숨긴다
            }
        }

        private void FormNew_CalFilePopup_Load(object sender, EventArgs e)
        {
            if (m_bInitialized)
                return;

            // 타이머 초기화
            timer_Status = new System.Windows.Forms.Timer();
            timer_Status.Interval = 200; // 200ms 주기
            timer_Status.Tick += Timer_Status_Tick;
            timer_Status.Start();

            InitializeDataGridView();
            RefreshGrid(); // 초기 로딩 시 DataGridView 표시

            m_bInitialized = true;
        }

        private void Timer_Status_Tick(object sender, EventArgs e)
        {
            try
            {
                // 타이머 중복 호출 방지
                timer_Status.Enabled = false;

                var current = Equipment.stConfigScannerCalData.CurrentCalFile;
                if (current != null)
                {
                    label_CalFilePopup_NearestCalFile_Disp.Text = $"[Auto] OffsetZ: {current.OffsetZ_mm} mm\n{current.CalFilePath}";
                }

            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            finally
            {
                timer_Status.Enabled = true;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (!this.Created)
                return;

            if (this.Visible && !m_bFormVisible)
            {
                m_bFormVisible = true;
                OnShow();
                timer_Status.Start();
            }
            else if (!this.Visible && m_bFormVisible)
            {
                m_bFormVisible = false;
                OnHide();
                timer_Status.Stop();
            }
        }

        private void OnShow()
        {
            LoadRecipe();
            timer_Status.Start();
            RefreshGrid();
        }
        private void OnHide()
        {
            timer_Status.Stop();
        }

        private void InitializeDataGridView()
        {
            dataGridView_CalFilePopup_CalFiles.AutoGenerateColumns = false;
            dataGridView_CalFilePopup_CalFiles.AllowUserToAddRows = false;
            dataGridView_CalFilePopup_CalFiles.AllowUserToResizeColumns = false;

            dataGridView_CalFilePopup_CalFiles.Columns.Clear();

            var colIndex = new DataGridViewTextBoxColumn
            {
                Name = "Index",
                HeaderText = "Index",
                DataPropertyName = "Index",
                Width = 50,
                ReadOnly = true
            };
            var colOffsetZ = new DataGridViewTextBoxColumn
            {
                Name = "OffsetZ_mm",
                HeaderText = "OffsetZ_mm",
                DataPropertyName = "OffsetZ_mm",
                Width = 100,
                ReadOnly = true
            };
            var colPath = new DataGridViewTextBoxColumn
            {
                Name = "CalFilePath",
                HeaderText = "CalFilePath",
                DataPropertyName = "CalFilePath",
                Width = 440,
                ReadOnly = true
            };

            dataGridView_CalFilePopup_CalFiles.Columns.Add(colIndex);
            dataGridView_CalFilePopup_CalFiles.Columns.Add(colOffsetZ);
            dataGridView_CalFilePopup_CalFiles.Columns.Add(colPath);
        }

        private void RefreshGrid()
        {
            dataGridView_CalFilePopup_CalFiles.DataSource = null;
            dataGridView_CalFilePopup_CalFiles.DataSource = new BindingList<ScannerCalManager.CalibrationFileInfo>(Equipment.stConfigScannerCalData.ZCalFileList);
        }

        private void Button_CalFilePopup_Open_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Correction Files|*.ctb;*.ct5";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = ofd.FileName;
                }
            }
        }

        private void Button_CalFilePopup_Add_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox_CalFilePopup_OffsetZ.Text, out double offsetZ))
            {
                string filePath = textBox1.Text;
                if (!string.IsNullOrEmpty(filePath))
                {
                    Equipment.stConfigScannerCalData.AddCalFile(offsetZ, filePath);
                    RefreshGrid();
                }
            }
        }

        private void Button_CalFilePopup_Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView_CalFilePopup_CalFiles.SelectedRows.Count > 0)
            {
                var row = dataGridView_CalFilePopup_CalFiles.SelectedRows[0];
                var info = row.DataBoundItem as ScannerCalManager.CalibrationFileInfo;

                if (info != null)
                {
                    Equipment.stConfigScannerCalData.RemoveCalFile(info.Index);

                    // 인덱스 재정렬
                    ReIndexCalFiles();

                    RefreshGrid();
                }
            }
        }

        private void ReIndexCalFiles()
        {
            for (int i = 0; i < Equipment.stConfigScannerCalData.ZCalFileList.Count; i++)
            {
                Equipment.stConfigScannerCalData.ZCalFileList[i].Index = (i+1);
            }
        }

        private void Button_CalFilePopup_Save_Click(object sender, EventArgs e)
        {
            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\ScannerCalFile(Do not delete or modify).ini";
            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("ScannerCalFile 파일이 없습니다.\r\n\r\n[Default값(CO₂)으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            if (File.Exists(strFIle) == false)
            {
                File.Create(strFIle);
                //return;

                MessageBox.Show("ScannerCalFile 파일을 생성하였습니다. 다시 시도하십시오.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Equipment.stConfigScannerCalData.SaveToIni(strFIle);
        }

        private bool LoadRecipe()
        {
            string strFIle = "";
            strFIle = ConfigManager.GetConfigPath() + "\\ScannerCalFile(Do not delete or modify).ini";
            if (File.Exists(strFIle) == false)
            {
                MessageBox.Show("ScannerCalFile 파일이 없습니다.\r\n\r\n[Default값(CO₂)으로 설정됩니다.]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //return false;
            }

            Equipment.stConfigScannerCalData.LoadFromIni(strFIle);
            RefreshGrid();

            return true;
        }

        private void Button_CalFilePopup_Load_Click(object sender, EventArgs e)
        {
            if(!LoadRecipe())
            {
                MessageBox.Show("Recipe Load Fail.");
                return;
            }
        }

        private void Button_CalFilePopup_GetFile_Click(object sender, EventArgs e)
        {
            if (double.TryParse(textBox_CalFilePopup_CurrentOffsetZ.Text, out double currentZ))
            {
                var nearest = Equipment.stConfigScannerCalData.GetNearestCalFile(currentZ);
                if (nearest != null)
                {
                    label_CalFilePopup_NearestCalFile_Disp.Text = $"OffsetZ: {nearest.OffsetZ_mm} μm\nFile: {nearest.CalFilePath}";
                }
                else
                {
                    label_CalFilePopup_NearestCalFile_Disp.Text = "No suitable Cal file found.";
                }
            }
        }

    }
}
