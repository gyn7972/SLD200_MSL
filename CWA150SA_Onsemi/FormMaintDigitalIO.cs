using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QMC.Common;
namespace CWA150SA_Onsemi300
{
    public partial class FormMaintDigitalIO : FormSubContentBase
    {
        public Size m_imagesize = new Size(30, 25);
        //public string m_path = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName;

        public Module m_Module;
        public int m_Index = 0;
        public FormMaintDigitalIO(Module module)
            : base(FormType.Maint.ToString(), " IO List")
        {
            m_Module = module;
            InitializeComponent();
            this.panelContent.Visible = false;
            this.panelContent.Size = new Size();
            //this.panelContent.Location = new Point(Configuration.ContentLocation.X, Configuration.PanelbuttonSize.Height);
            this.Controls.Add(this.baseDataGridViewIO);
            this.baseDataGridViewIO.Location = new Point(Configuration.ContentLocation.X, Configuration.ContentLocation.Y);
            this.baseDataGridViewIO.Size = new Size(Configuration.ContentSize.Width - Configuration.ContentLocation.X - 20, Configuration.FormContentSize.Height - Configuration.PanelSize.Height * 2);
            InitbaseDataGridViewIOColumns();
            timerUpdate.Start();

            InitModule(module);
        }

        public void InitModule(Module module)
        {
            m_Module = module;
            if (m_Module != null)
            {
                DioPointCollection DioPointCollection = m_Module.GetAllDioPoints();
                baseDataGridViewIO.DataSource = DioPointCollection;
            }
        }

        public void ShowDataGridView()
        {
            DioPointCollection DioPointCollection = m_Module.GetAllDioPoints();
            
            for (int i = 0; i < DioPointCollection.Count; i++)
            {
                if (DioPointCollection[i].IoType == IoType.Input)
                {
                    Image img = CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
                    Bitmap imgbitmap = new Bitmap(img);
                    img = resizeImage(imgbitmap, m_imagesize);
                    DataGridViewImageCell cell = baseDataGridViewIO.Rows[i].Cells[m_Index] as DataGridViewImageCell;
                    cell.Value = img;
                    cell.ReadOnly = true;
                }
                else
                {
                    Image img = CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
                    Bitmap imgbitmap = new Bitmap(img);
                    img = resizeImage(imgbitmap, m_imagesize);
                    DataGridViewImageCell cell = baseDataGridViewIO.Rows[i].Cells[m_Index] as DataGridViewImageCell;
                    cell.Value = img;
                    cell.ReadOnly = false;
                }
            }
        }
        public static Image resizeImage(Image image, Size size)
        {
            return (Image)new Bitmap(image, size);
        }

        private void InitbaseDataGridViewIOColumns()
        {
            baseDataGridViewIO.Columns.Clear();
            baseDataGridViewIO.AutoGenerateColumns = false;
            baseDataGridViewIO.DataSource = null;
            
            {
                DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
                Image img = CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
                imageColumn.Image = img;
                imageColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
                imageColumn.Resizable = DataGridViewTriState.False;
                m_Index = baseDataGridViewIO.Columns.Add(imageColumn);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Name";
                column.Name = "Name";

                baseDataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Locator";
                column.Name = "Locator";
                baseDataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "Address";
                column.Name = "Address";
                baseDataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "IoType";
                column.Name = "Type";
                baseDataGridViewIO.Columns.Add(column);
            }
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn();
                column.DataPropertyName = "ModuleNo";
                column.Name = "IOModule";
                baseDataGridViewIO.Columns.Add(column);
            }
            baseDataGridViewIO.CellDoubleClick += BaseDataGridViewIO_DoubleClick;

            //DioPointCollection DioPointCollection = m_Module.GetAllDioPoints();
            //baseDataGridViewIO.DataSource = DioPointCollection;
        }

        private void BaseDataGridViewIO_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == m_Index)
            {
                DioPoint dioPoint = baseDataGridViewIO.Rows[e.RowIndex].DataBoundItem as DioPoint;
                DataGridViewImageCell cell = baseDataGridViewIO.Rows[e.RowIndex].Cells[m_Index] as DataGridViewImageCell;


                if (dioPoint != null)
                {
                    if (baseDataGridViewIO.Rows[e.RowIndex].Cells["Type"].Value.ToString() != IoType.Input.ToString())
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

        private void FormMaintDigitalIO_Load(object sender, EventArgs e)
        {
            ShowDataGridView();
        }

        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            timerUpdate.Stop();
            for (int i = 0; i < baseDataGridViewIO.Rows.Count; i++)
            {
                DioPoint dioPoint = baseDataGridViewIO.Rows[i].DataBoundItem as DioPoint;
                if (dioPoint.IoType == IoType.Input)
                {
                    if (dioPoint.GetValue() == DioValue.On)
                    {
                        //Image img = Image.FromFile(m_path + "\\Resources\\DioEllipseOff.png");
                        Image img = CWA150SA_Onsemi.Properties.Resources.DioEllipseOn;
                        Bitmap imgbitmap = new Bitmap(img);
                        img = resizeImage(imgbitmap, m_imagesize);
                        baseDataGridViewIO.Rows[i].Cells[0].Value = img;
                    }
                    else if (dioPoint.GetValue() == DioValue.Off)
                    {
                        //Image img = Image.FromFile(m_path + "\\Resources\\DioEllipseOn.png");
                        Image img = CWA150SA_Onsemi.Properties.Resources.DioEllipseOff;
                        Bitmap imgbitmap = new Bitmap(img);
                        img = resizeImage(imgbitmap, m_imagesize);
                        baseDataGridViewIO.Rows[i].Cells[0].Value = img;
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
                        //Image img = Image.FromFile(m_path + "\\Resources\\DioRectangleOff.png");
                        Image img = CWA150SA_Onsemi.Properties.Resources.DioRectangleOn;
                        Bitmap imgbitmap = new Bitmap(img);
                        img = resizeImage(imgbitmap, m_imagesize);
                        baseDataGridViewIO.Rows[i].Cells[0].Value = img;
                    }
                    else if (dioPoint.GetValue() == DioValue.Off)
                    {
                        //Image img = Image.FromFile(m_path + "\\Resources\\DioRectangleOn.png");
                        Image img = CWA150SA_Onsemi.Properties.Resources.DioRectangleOff;
                        Bitmap imgbitmap = new Bitmap(img);
                        img = resizeImage(imgbitmap, m_imagesize);
                        baseDataGridViewIO.Rows[i].Cells[0].Value = img;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            timerUpdate.Start();
        }
    }
}
