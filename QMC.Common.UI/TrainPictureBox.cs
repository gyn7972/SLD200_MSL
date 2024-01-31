using QMC.Common.Hmi;
using QMC.Common.Vision;
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

namespace QMC.Common.UI
{
    public delegate void ImageChangedEventHandler();
    public partial class TrainPictureBox : UserControl
    {
        [Serializable]
        public enum MenuItems
        {
            [Abbreviation("Image Save")]
            ImageSave,
            [Abbreviation("Image Load")]
            ImageLoad,
        }
        private SaveFileDialog saveFileDialog = null;
        private OpenFileDialog openFileDialog = null;
        public ImageChangedEventHandler ImageChanged;
        public TrainPictureBox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            this.ContextMenuStrip = new ContextMenuStrip();
            OnCreateControl();
        }

        public VisionImage GetImage()
        {
            VisionImage image = new VisionImage();
            image = pictureBox1.Image;
            return image;
        }

        public void SetImage(Image image)
        {
            if (image != null)
                this.pictureBox1.Image = image;
        }
        public void ResizeControl(int nX, int nY)
        {
            this.pictureBox1.Size = new Size(nX, nY);
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Right)
            {
                this.ContextMenuStrip.Show(e.X, e.Y);
            }
        }

        protected override void OnCreateControl()
        {
            MenuItems[] items = (MenuItems[])Enum.GetValues(typeof(MenuItems));
            ToolStripMenuItem item = null;

            base.OnCreateControl();

            // Design mode 
            if (this.DesignMode) return;

            #region Initial

            #endregion


            #region Menu Strip
            this.ContextMenuStrip = new ContextMenuStrip();
            for (int i = 0; i < items.Length; i++)
            {
                item = new ToolStripMenuItem();
                item.Text = AbbreviationAttribute.GetAbbreviation(items[i]);
                item.Name = items[i].ToString();
                item.Click += Item_Click;

                this.ContextMenuStrip.Items.Add(item);

                if (items[i] == MenuItems.ImageSave || items[i] == MenuItems.ImageLoad)
                {
                    this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                }
            }
            #endregion
        }

        private void Item_Click(object sender, EventArgs e)
        {
            ToolStripItem item = sender as ToolStripItem;
            StringBuilder builder = null;
            VisionImage.FileFilter[] filter = null;

            if (item.Name == MenuItems.ImageSave.ToString())
            {
                if (this.pictureBox1.Image == null)
                {
                    MessageBox.Show("InputImage is not exist");
                    return;
                }
                saveFileDialog = new SaveFileDialog();
                builder = new StringBuilder();
                filter = (VisionImage.FileFilter[])Enum.GetValues(typeof(VisionImage.FileFilter));

                for (int i = 0; i < filter.Length; i++)
                {
                    builder.Append(".");
                    builder.Append(filter[i].ToString());
                    builder.Append("|*.");
                    builder.Append(filter[i].ToString());

                    if (filter.Length - 1 == i) continue;
                    builder.Append("|");
                }

                saveFileDialog.Filter = builder.ToString();

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {

                        this.pictureBox1.Image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(new Form { TopMost = true }, ex.Message);
                    }
                }
            }
            else if (item.Name == MenuItems.ImageLoad.ToString())
            {
                this.pictureBox1.Image = null;
                openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                if (File.Exists(openFileDialog.FileName) == false)
                {
                    return;
                }
                this.pictureBox1.Load(openFileDialog.FileName);
                if (ImageChanged != null)
                {
                    ImageChanged();
                }
            }
        }

        private void TrainPictureBox_SizeChanged(object sender, EventArgs e)
        {
            this.pictureBox1.Size = this.Size;
        }
    }
}
