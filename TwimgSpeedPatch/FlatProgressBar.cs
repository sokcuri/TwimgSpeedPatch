using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TwimgSpeedPatch
{
    [DesignerCategory("CODE")]
    public class FlatProgressBar : ProgressBar
    {
        public FlatProgressBar()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            this.ProgressbarColor = Color.Red;
        }

        private Brush m_brush;
        private Color m_progressbarColor;
        public Color ProgressbarColor
        {
            get => this.m_progressbarColor;
            set
            {
                this.m_brush?.Dispose();
                this.m_brush = new SolidBrush(value);
                this.m_progressbarColor = value;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec = e.ClipRectangle;

            rec.Width = (int)(rec.Width * ((double)this.Value / this.Maximum)) - 4;

            if (ProgressBarRenderer.IsSupported)
                ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);

            rec.Height = rec.Height - 4;
            e.Graphics.FillRectangle(this.m_brush, 2, 2, rec.Width, rec.Height);
        }
    }
}
