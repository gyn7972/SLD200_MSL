using QMC.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLD200_MSL
{
    public partial class IOListControl : UserControl
    {
        private DioPointCollection m_DioPoint;
        private Image m_InputOnImage;
        private Image m_InputOffImage;
        private Image m_OutputOnImage;
        private Image m_OutputOffImage;
        private int m_Index = 0;
        private Size m_imagesize = new Size(20, 20);
        private string m_strType;
        public IOListControl(DioPointCollection IoPoints, string strType)
        {
            m_DioPoint = IoPoints;
            m_strType = strType;
            SetImage();
            InitializeComponent();
            InitDataGridView();
            //UpdateGridInfo();
            this.timerIO.Interval = 100;
            this.timerIO.Tick += Update;

            this.Load += FormModuleIO_Load;
        }


        private void SetImage()
        {
            m_InputOnImage = SLD200.Properties.Resources.DioEllipseOn;
            Bitmap imgbitmapInputOn = new Bitmap(m_InputOnImage);
            m_InputOnImage = resizeImage(imgbitmapInputOn, m_imagesize);

            m_InputOffImage = SLD200.Properties.Resources.DioEllipseOff;
            Bitmap imgbitmapInputOff = new Bitmap(m_InputOffImage);
            m_InputOffImage = resizeImage(imgbitmapInputOff, m_imagesize);

            m_OutputOnImage = SLD200.Properties.Resources.DioRectangleOn;
            Bitmap imgbitmapOutPutOn = new Bitmap(m_OutputOnImage);
            m_OutputOnImage = resizeImage(imgbitmapOutPutOn, m_imagesize);

            m_OutputOffImage = SLD200.Properties.Resources.DioRectangleOff;
            Bitmap imgbitmapOutPutOff = new Bitmap(m_OutputOffImage);
            m_OutputOffImage = resizeImage(imgbitmapOutPutOff, m_imagesize);


        }
        private void FormModuleIO_Load(object sender, EventArgs e)
        {
            timerIO.Start();
            ShowDataGridView();
        }
        public void ShowDataGridView()
        {
            for (int i = 0; i < m_DioPoint.Count; i++)
            {
                if (m_DioPoint[i].IoType == IoType.Input)
                {
                    DataGridViewImageCell cell = DataGridView.Rows[i].Cells[m_Index] as DataGridViewImageCell;
                    cell.Value = m_InputOffImage;
                    cell.ReadOnly = true;
                }
                else
                {
                    DataGridViewImageCell cell = DataGridView.Rows[i].Cells[m_Index] as DataGridViewImageCell;
                    cell.Value = m_OutputOffImage;
                    cell.ReadOnly = false;
                }
            }
        }
        public void SetControlName(string strName)
        {
            this.GroupBox.Text = strName;
        }

        public void UpdateGridInfo(DioPointCollection dioPoints)
        {
            this.DataGridView.DataSource = null;
            BindingSource IOPoint = new BindingSource();
            IOPoint.DataSource = dioPoints;
            DataGridView.DataSource = IOPoint;
        }

        private void InitDataGridView()
        {
            DataGridView.Columns.Clear();
            DataGridView.AutoGenerateColumns = false;
            if (m_strType == "Input List")
            {
                {
                    DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                    imageColumn.Image = m_InputOffImage;
                    imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    imageColumn.Resizable = DataGridViewTriState.False;
                    m_Index = DataGridView.Columns.Add(imageColumn);
                }
            }
            else
            {
                {
                    DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                    imageColumn.Image = m_OutputOffImage;
                    imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                    imageColumn.Resizable = DataGridViewTriState.False;
                    m_Index = DataGridView.Columns.Add(imageColumn);
                }
            }

            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";
                DataGridView.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "ModuleNo";
                column.Name = "ModuleNo";
                DataGridView.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Address";
                column.Name = "Address";
                DataGridView.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Locator";
                column.Name = "Locator";
                DataGridView.Columns.Add(column);
            }
            this.DataGridView.CellDoubleClick += DataGridViewIO_DoubleClick;            
        }

        private void DataGridViewIO_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == m_Index)
            {
                DioPoint dioPoint = DataGridView.Rows[e.RowIndex].DataBoundItem as DioPoint;
                DataGridViewImageCell cell = DataGridView.Rows[e.RowIndex].Cells[m_Index] as DataGridViewImageCell;


                if (dioPoint != null)
                {
                    if (dioPoint.IoType != IoType.Input)
                    {
                        if (dioPoint.GetValue() == DioValue.On)
                        {
                            dioPoint.Write(DioValue.Off);
                        }
                        else if (dioPoint.GetValue() == DioValue.Off)
                        {
                            dioPoint.Write(DioValue.On);
                        }
                        else
                        {
                            return;
                        }
                    }

                }

            }
        }
        private void Update(object sender, EventArgs e)
        {
            timerIO.Stop();

            //20250722 IO TEST.
            PollingAllInputModules();

            for (int i = 0; i < DataGridView.Rows.Count; i++)
            {
                DioPoint dioPoint = DataGridView.Rows[i].DataBoundItem as DioPoint;
                if (dioPoint.IoType == IoType.Input)
                {
                    if (dioPoint.GetValue() == DioValue.On)
                    {
                        DataGridView.Rows[i].Cells[0].Value = m_InputOnImage;
                    }
                    else if (dioPoint.GetValue() == DioValue.Off)
                    {
                        DataGridView.Rows[i].Cells[0].Value = m_InputOffImage;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (dioPoint.GetValue() == DioValue.On)
                    {                        
                        DataGridView.Rows[i].Cells[0].Value = m_OutputOnImage;
                    }
                    else if (dioPoint.GetValue() == DioValue.Off)
                    {
                        DataGridView.Rows[i].Cells[0].Value = m_OutputOffImage;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            timerIO.Start();
        }


        private static Image resizeImage(Image image, Size size)
        {
            return (Image)new Bitmap(image, size);
        }


        public void PollingAllInputModules()
        {
            var modules = m_DioPoint
                    .Where(p => p.IoType == IoType.Input) // DioPoint의 IoType 사용
                    .Select(p => p.Module as DioModule)   // Module 캐스팅
                    .Where(m => m != null)                // null 필터링
                    .Distinct();                          // 중복 제거

            foreach (var module in modules)
                module.Read(); // 1회만 Read
        }
    }
}
