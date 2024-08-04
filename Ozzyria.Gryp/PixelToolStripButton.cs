using System.Drawing.Drawing2D;

namespace Ozzyria.Gryp
{
    internal class PixelToolStripButton: ToolStripButton
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            base.OnPaint(pe);
        }
    }
}
