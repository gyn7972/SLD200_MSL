using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Forms.VisualStyles;

namespace QMC.Core.Mmi
{
    public class ToggleButton : Button
    {
        System.Windows.Forms.Timer timer;

        private bool mouseFlag = false;

        private Rectangle buttonRectangle;
        private Rectangle ledRectangle;
        private Rectangle fontRectangle;

        #region Public Custom Properties
        [Browsable(true)]
        [Category("etc")]
        [Description("Focus Color")]
        public Color FocusColor { get; set; }

        [Browsable(true)]
        [Category("etc")]
        [Description("On Color")]
        public Color OnColor { get; set; }


        [Browsable(true)]
        [Category("etc")]
        [Description("Off Color")]
        public Color OffColor { get; set; }

        [Browsable(true)]
        [Category("etc")]
        [Description("Check")]
        public bool Checked 
        { 
            get{ return check; }
            set
            {
                check = value;
                this.Invalidate();
            }
        }
        private bool check;

        [Browsable(true)]
        [Category("etc")]
        [Description("padding")]
        public short padding { get; set; }

        [Browsable(true)]
        [Category("gradient")]
        [Description("gradient")]
        public bool GradientEnable 
        { 
            get { return this.gradientEnable; }
            set
            { 
                this.gradientEnable = value;
                this.Invalidate();
            }
        }
        private bool gradientEnable;

        [Browsable(true)]
        [Category("gradient")]
        [Description("gradient Color1")]
        public Color GColor1 { get; set; }

        [Browsable(true)]
        [Category("gradient")]
        [Description("gradient Color2")]
        public Color GColor2 { get; set; }

        [Browsable(true)]
        [Category("gradient")]
        [Description("gradient Color3")]
        public Color GColor3 { get; set; }

        [Browsable(true)]
        [Category("gradient")]
        [Description("gradient Color4")]
        public Color GColor4 { get; set; }

        [Browsable(true)]
        [Category("Led Button")]
        [Description("LedButtonEnable")]
        public bool LedButtonEnable 
        {
            get { return this.ledbtnEnable; }
            set
            {
                this.ledbtnEnable = value;
                this.Invalidate();
            }
        }
        private bool ledbtnEnable;

        [Browsable(true)]
        [Category("Led Button")]
        [Description("Led Width")]
        public int LedWidth { get; set; }

        [Browsable(true)]
        [Category("Led Button")]
        [Description("Led Flicker")]
        public bool LedFlicker
        {
            get {  return timer.Enabled;   }
            set {  timer.Enabled = value;  }
        }

        [Browsable(true), DefaultValue(100)]
        [Category("Led Button")]
        [Description("Led Flicker Time")]
        public int LedFlickerTimer
        {
            get {   return timer.Interval;   }
            set {   timer.Interval = value;  }
        }

        #endregion

        #region Contructor
        public ToggleButton()
        {
            // Control styles
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);

            this.DoubleBuffered = true;

            this.timer = new Timer { Interval = 100 };
            this.timer.Tick += Timer_Tick;

            this.FocusColor = Color.Orange;
            this.OnColor = Color.DimGray;
            this.OffColor = Color.Silver;
            this.Checked = false;
            this.padding = 2;
            this.GColor1 = Color.White;
            this.GColor2 = Color.LightGray;
            this.GColor3 = Color.DarkGray;
            this.GColor4 = Color.White;
            this.LedWidth = 10;

            this.MinimumSize = new Size(120, 50);
            this.Margin = new Padding();
        }

        ~ToggleButton()
        {
            Dispose(false);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.timer.Enabled = false;
                this.timer.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Methods

        //추후 상속 처리.
        //protected override void
        private void DrawBackGround(Graphics e, Color backColor, Rectangle rectangle)
        {
            e.FillRectangle(new SolidBrush(backColor), rectangle);
        }

        private void DrawBorder(Graphics e)
        {
            RectangleF borderRect = new RectangleF(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);
            var gp = new GraphicsPath();
            gp.AddRectangle(borderRect);
            using (var pen = new Pen(!Focused ? Color.FromArgb(60, Color.Black) : FocusColor, 2))
                e.DrawPath(pen, gp);
        }

        private void DrawHighLight(Graphics e)
        {
            RectangleF highlightRect = new RectangleF( this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);
            highlightRect.Inflate(-2, -2);
            var gp = new GraphicsPath();
            gp.AddRectangle(highlightRect);
            using (var pen = new Pen(Color.FromArgb(60, Color.White), 4))
            {
                e.DrawPath(pen, gp);
            }
        }

        private void DrawLed(Graphics e, Color ledColor, Rectangle rectangle)
        {
            e.FillRectangle(new SolidBrush(ledColor), rectangle);
        }
        
        private void DrawGradiant(Graphics e, Rectangle rectangle)
        {
            LinearGradientBrush lgb = new LinearGradientBrush(rectangle, Color.Black, Color.Black, LinearGradientMode.Vertical);
            ColorBlend cb = new ColorBlend();
            cb.Colors = new Color[] { GColor1, GColor2, GColor3, GColor4 };
            cb.Positions = new Single[] { 0.0F, 0.5F, 0.5F, 1.0F };
            lgb.InterpolationColors = cb;
            e.FillRectangle(lgb, rectangle);
        }

        private void DrawText(Graphics e, Rectangle rectangle)
        {
            SolidBrush drawBrush;
            if (Enabled) 
                drawBrush = new SolidBrush(ForeColor);
            else 
                drawBrush = new SolidBrush(Color.FromArgb(80, ForeColor));

            StringFormat sf = new StringFormat();
            switch (this.TextAlign)
            {
                case System.Drawing.ContentAlignment.TopLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case System.Drawing.ContentAlignment.MiddleLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case System.Drawing.ContentAlignment.BottomLeft:
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
                case System.Drawing.ContentAlignment.TopCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case System.Drawing.ContentAlignment.MiddleCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case System.Drawing.ContentAlignment.BottomCenter:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
                case System.Drawing.ContentAlignment.TopRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Near;
                    break;
                case System.Drawing.ContentAlignment.MiddleRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Center;
                    break;
                case System.Drawing.ContentAlignment.BottomRight:
                    sf.Alignment = StringAlignment.Far;
                    sf.LineAlignment = StringAlignment.Far;
                    break;
            }

            
            e.DrawString(this.Text, Font, drawBrush, rectangle, sf);
        }

        #endregion

        #region Events
        private void Timer_Tick(object sender, EventArgs e)
        {
            this.timer.Enabled = false;
            this.Checked = !this.Checked;
            this.Invalidate();
            this.timer.Enabled = true;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            this.ledRectangle = new Rectangle(
                    this.ClientRectangle.X + padding + Padding.Left,
                    this.ClientRectangle.Y + padding + Padding.Top,
                    this.LedWidth,
                    this.ClientRectangle.Height - (padding * 2) - Padding.Top - Padding.Bottom);

            this.buttonRectangle = new Rectangle(
                    this.ClientRectangle.X + padding + Padding.Left,
                    this.ClientRectangle.Y + padding + Padding.Top,
                    this.ClientRectangle.Width - (padding * 2) - Padding.Left - Padding.Right,
                    this.ClientRectangle.Height - (padding * 2) - Padding.Top - Padding.Bottom);

            if (LedButtonEnable)
            {
                this.fontRectangle = new Rectangle(this.ClientRectangle.X + LedWidth + Padding.Left + padding * 2,
                    this.ClientRectangle.Y + Padding.Top + 1,
                    this.ClientRectangle.Width - (padding * 4) - Padding.Left - Padding.Right - LedWidth,
                    this.ClientRectangle.Height - Padding.Top - Padding.Bottom);
            }
            else
            {
                this.fontRectangle = new Rectangle(this.ClientRectangle.X + Padding.Left + padding * 2,
                    this.ClientRectangle.Y + Padding.Top + 1,
                    this.ClientRectangle.Width - (padding * 4) - Padding.Left - Padding.Right,
                    this.ClientRectangle.Height - Padding.Top - Padding.Bottom);
            }
        }

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

        protected override void OnClick(EventArgs e)
        {
            //this.Checked = !this.Checked;
            this.Enabled = false;
            base.OnClick(e);
            this.Enabled = true;
            this.Focus();

        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            ButtonRenderer.DrawParentBackground(e.Graphics, this.ClientRectangle, this);


            // draw background
            if (LedButtonEnable)
                DrawBackGround(graphics, this.BackColor, this.buttonRectangle);
            else
                DrawBackGround(graphics, Checked ? OnColor : OffColor, this.buttonRectangle);

            // draw background gradient
            if (GradientEnable)
                DrawGradiant(graphics, this.buttonRectangle);

            // draw border
            DrawBorder(graphics);

            // draw led
            if (LedButtonEnable)
                DrawLed(graphics, Checked ? OnColor : OffColor, this.ledRectangle);

            // draw highlight                     
            if (mouseFlag)
                DrawHighLight(graphics);

            // draw text
            if (String.IsNullOrEmpty(this.Text) == false)
            {
                DrawText(graphics, this.fontRectangle);
            }
        }
        #endregion
    }
}
