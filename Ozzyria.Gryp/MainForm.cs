using Ozzyria.Gryp.Models.Data;
using System.Reflection;

namespace Ozzyria.Gryp
{
    public partial class MainGrypWindow : Form
    {
        internal Map _map = new Map();

        public MainGrypWindow()
        {
            InitializeComponent();

            // hackity hack to override DoubleBuffered without making custom class
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, viewPortPanel, new object[] { true });
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var mapDialog = new NewMapDialog();
            if (mapDialog.ShowDialog() == DialogResult.OK)
            {
                _map.Width = mapDialog.NewMapResult.Width;
                _map.Height = mapDialog.NewMapResult.Height;
                viewPortPanel.Refresh();
            }
        }

        private void viewPortPanel_Paint(object sender, PaintEventArgs e)
        {
            // Draw background Grid
            var canvasSize = e.ClipRectangle.Size;
            for (var x = 0; x <= canvasSize.Width / 16; x++)
            {
                for (var y = 0; y <= canvasSize.Height / 16; y++)
                {
                    e.Graphics.DrawLine(new Pen(Color.Blue), (x * 16), 0, (x * 16), canvasSize.Height);
                    e.Graphics.DrawLine(new Pen(Color.Blue), 0, (y * 16), canvasSize.Width, (y * 16));
                }
            }

            // TODO infer graphics to draw
            if (_map.Width > 0 && _map.Height > 0)
            {
                var tileImage = new Bitmap(_map.Width * 32, _map.Height * 32);
                using (var graphics = Graphics.FromImage(tileImage))
                {
                    for (var x = 0; x < _map.Width; x++)
                    {

                        for (var y = 0; y < _map.Height; y++)
                        {
                            e.Graphics.DrawRectangle(new Pen(Color.Red), new Rectangle(x * 32, y * 32, 32, 32));
                        }
                    }
                }
                e.Graphics.DrawImage(tileImage, new Point(0, 0));
            }

            // Render Width x Height
            var font = new Font(new FontFamily("Arial"), 12);
            var brush = new SolidBrush(Color.Red);
            e.Graphics.DrawString($"{_map.Width}x{_map.Height}", font, brush, new PointF());
        }

        private void viewPortPanel_Scroll(object sender, ScrollEventArgs e)
        {
            // TODO calculate scale
        }
    }
}
