using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using WControls.Utils;
using WControls.Drawable;

namespace WControls
{
    public enum ControlShape
    {
        Rect,
        RoundedRect,
        Circular
    }

    [ToolboxItem(false)]
    public partial class BaseWControl : UserControl
    {
        /// <summary>
        /// Always transparent
        /// </summary>
        [Browsable(false)]
        public override Color BackColor
        {
            get
            {
                return Color.Transparent;
            }
            set { }
        }

        private Color GradientColorTrans
        {
            get
            {
                return Color.FromArgb(0xcc, GradientColor);
            }
        }

        private Color TopHalfColor
        {
            get
            {
                return Color.FromArgb(0x10, GlossColor);
            }
        }

        #region Public Properties

        /// <summary>
        /// The shape of the control
        /// </summary>
        [Description("The shape of the control")]
        [DefaultValue(typeof(ControlShape), "RoundedRect")]
        [Category("Appearance")]
        public virtual ControlShape Shape
        {
            get { return m_shape; }
            set
            {
                m_shape = value;
                this.RecalculatePaths();
                this.Invalidate(true);
            }
        }

        /// <summary>
        /// The background color of the control
        /// </summary>
        [Description("The background color of the control")]
        [DefaultValue(typeof(Color), "DimGray")]
        [Category("Appearance")]
        public virtual Color BackgroundColor
        {
            get { return m_bgColor; }
            set
            {
                m_bgColor = value;
                this.Invalidate(true);
            }
        }

        /// <summary>
        /// The color of the control's gloss effect
        /// </summary>
        [Description("The color of the control\'s gloss effect")]
        [DefaultValue(typeof(Color), "White")]
        [Category("Appearance")]
        public virtual Color GlossColor
        {
            get { return m_glossColor; }
            set
            {
                m_glossColor = value;
                this.Invalidate(true);
            }
        }

        /// <summary>
        /// The color of the control's gradient effect
        /// </summary>
        [Description("The color of the control\'s gradient effect")]
        [DefaultValue(typeof(Color), "White")]
        [Category("Appearance")]
        public virtual Color GradientColor
        {
            get { return m_gradColor; }
            set
            {
                m_gradColor = value;
                this.Invalidate(true);
            }
        }

        /// <summary>
        /// Whether or not to show the control's gloss effect
        /// </summary>
        [Description("Whether or not to show the control\'s gloss effect")]
        [DefaultValue(true)]
        [Category("Appearance")]
        public virtual bool ShowGloss
        {
            get { return m_bShowGloss; }
            set
            {
                m_bShowGloss = value;
                this.Invalidate(true);
            }
        }

        /// <summary>
        /// Whether or not to show the control's gradient effect
        /// </summary>
        [Description("Whether or not to show the control\'s gradient effect")]
        [DefaultValue(true)]
        [Category("Appearance")]
        public virtual bool ShowGradient
        {
            get { return m_bShowGrad; }
            set
            {
                m_bShowGrad = value;
                this.Invalidate(true);
            }
        }

        #endregion

        //for accessors
        private Color m_bgColor = Color.DimGray;
        private Color m_glossColor = Color.White;
        private Color m_gradColor = Color.White;
        private ControlShape m_shape = ControlShape.RoundedRect;
        private bool m_bShowGloss = true;
        private bool m_bShowGrad = true;

        //internals
        private GraphicsPath m_bgPath;
        private GraphicsPath m_glossPath;
        private bool m_bHandleGloss = false;

        private BaseWControl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint, true);

            Shape = ControlShape.RoundedRect;
            BackgroundColor = Color.DimGray;
            GradientColor = Color.White;
            GlossColor = Color.White;
            ShowGloss = true;
            ShowGradient = true;

            m_bHandleGloss = true;

            RecalculatePaths();
        }

        public BaseWControl(bool bGlossDrawHandledByBase)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint, true);

            Shape = ControlShape.RoundedRect;
            BackgroundColor = Color.DimGray;
            GradientColor = Color.White;
            GlossColor = Color.White;
            ShowGloss = true;
            ShowGradient = true;

            m_bHandleGloss = bGlossDrawHandledByBase;

            RecalculatePaths();
        }

        protected void Invalidate(IDrawable control)
        {
            Invalidate(control.GetRedrawRegion());
        }

        private void RecalculatePaths()
        {
            DisposePaths();
            m_bgPath = GraphicsHelper.GetGraphicsPath(ClientRectangle, Shape);
            m_glossPath = GraphicsHelper.Get3DShinePath(ClientRectangle, Shape);
        }

        protected virtual void OnPaintGloss(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (ShowGloss)
            {
                using (Brush shineBrush = new SolidBrush(TopHalfColor))
                {
                    g.FillPath(shineBrush, m_glossPath);
                }
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //dont draw the transparent bg in the region we are going to paint anyways
            GraphicsState state = e.Graphics.Save();

            if (m_bgPath != null)
            {
                e.Graphics.Clip.Exclude(m_bgPath);
            }
            base.OnPaintBackground(e);
            e.Graphics.Restore(state);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (Brush bgBrush = new SolidBrush(BackgroundColor))
            using (Brush gradBrush = GraphicsHelper.GetGradBrush(ClientRectangle, Shape, GradientColorTrans))
            {
                if (m_bgPath != null)
                {
                    e.Graphics.FillPath(bgBrush, m_bgPath);

                    if (ShowGradient)
                    {
                        e.Graphics.FillPath(gradBrush, m_bgPath);
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (m_bHandleGloss)
            {
                OnPaintGloss(e.Graphics);
            }

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            RecalculatePaths();
            base.OnResize(e);
        }

        #region Disposing

        private void DisposePaths()
        {
            if (m_bgPath != null)
            {
                m_bgPath.Dispose();
                m_bgPath = null;
            }
            if (m_glossPath != null)
            {
                m_glossPath.Dispose();
                m_glossPath = null;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                DisposePaths();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
