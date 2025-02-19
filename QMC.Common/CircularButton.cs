using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace QMC.Core.Mmi
{
    public class CircularButton : Button
    {
        #region Public Custom Properties
        [Description("Check"), Category("Circle")]
        public bool Checked 
        { 
            get{ return this.check;} 
            set
            {
                this.check = value;
                this.Invalidate();
            }
        }
        private bool check;

        [Description("Circle Border Size"), Category("Circle")]
        public long BorderWidth { get; set; }

        [Description("Circle Border Color"), Category("Circle")]
        public Color BorderColor { get; set; }

        [Description("On Color"), Category("Circle")]
        public Color OnColor { get; set; }

        [Description("Off Color"), Category("Circle")]
        public Color OffColor { get; set; }


        [Description("Background Hatch Style Enable/Disable"), Category("Circle")]
        public bool HatchEnable { get; set; }

        [Description("Background Hatch Style"), Category("Circle")]
        public HatchStyle Style 
        {
            get { return this.style; }
            set
            { 
                this.style = value;
                this.Invalidate();
            }
        }
        private HatchStyle style;

        [Description("Circle Edge Line margin"), Category("Circle")]
        public long EdgeMargin { get; set; }

        #endregion

        #region Contructor

        private bool mouseFlag = false;

        public CircularButton()
        {
            this.DoubleBuffered = true;
            this.Font = new Font("Tahoma", 15, FontStyle.Bold);
            this.BorderColor = Color.DimGray;
            this.BorderWidth = 2;
            this.BackColor = Color.Transparent;//Color.LimeGreen;
            this.OnColor = Color.Transparent; //Color.Lime;
            this.OffColor = Color.Gray; //Color.Lime;
            this.MinimumSize = new Size(50, 50);
            this.EdgeMargin = 4;
            
        }
        ~CircularButton()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
        #endregion

        #region Methods
        private void DrawCircle(Graphics e)
        {
            using (Brush brush = new SolidBrush(Checked ? this.OnColor : this.OffColor ))
            {
                e.FillEllipse(brush, EdgeMargin, EdgeMargin, (this.Width - EdgeMargin * 2), (this.Height - EdgeMargin * 2));
            }
        }

        private void DrawHatch(Graphics e)
        {
            using (Brush brush = new HatchBrush(this.style, Checked ? this.OnColor : this.OffColor))
            {
                e.FillEllipse(brush, EdgeMargin, EdgeMargin, (this.Width - EdgeMargin * 2), (this.Height - EdgeMargin * 2));
            }
        }

        private void DrawBorder(Graphics e)
        {
            using (Pen pen = new Pen(this.BorderColor, this.BorderWidth))
            {
                e.DrawArc(pen, EdgeMargin, EdgeMargin, (this.Width - EdgeMargin * 2), (this.Height - EdgeMargin * 2), 0, 360);
            }
        }

        private void DrawText(Graphics e)
        {
            using (Brush FontColor = new SolidBrush(this.ForeColor))
            {
                SizeF MS = e.MeasureString(this.Text, this.Font);
                SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(100, this.ForeColor));
                //
                e.DrawString(this.Text,
                    this.Font,
                    FontColor,
                    Convert.ToInt32(Width / 2 - MS.Width / 2),
                    Convert.ToInt32(Height / 2 - MS.Height / 2));
            }
        }
        #endregion

        #region Events
        protected override void OnMouseEnter(EventArgs e)
        {
            mouseFlag = true;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            mouseFlag = false;
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ButtonRenderer.DrawParentBackground(e.Graphics, this.ClientRectangle, this);

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            DrawCircle(e.Graphics);

            if (true == HatchEnable)
                DrawHatch(e.Graphics);

            DrawBorder(e.Graphics);

            DrawText(e.Graphics);

            if (null != this.Image)
            {
                e.Graphics.DrawImage(this.Image, (this.Width - this.Image.Width) / 2, (this.Height - this.Image.Height) / 2);
                return;
            }
        }
        #endregion
    }
}
