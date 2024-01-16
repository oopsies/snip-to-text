using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snip_to_text {
    public partial class Snipper : Form {

        public Image Image { get; set; }

        private Rectangle rcSelect;
        private Point pointStart;

        public Snipper(Image screenShot) {
            InitializeComponent();
            this.BackgroundImage = screenShot;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;
        }

        public static Image Snip() {

            var rc = Screen.PrimaryScreen.Bounds;

            using (Bitmap bmp = new Bitmap(rc.Width, rc.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb)) {
                using (Graphics gr = Graphics.FromImage(bmp)) {
                    gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                }
                using (var snipper = new Snipper(bmp)) {
                    if (snipper.ShowDialog() ==  DialogResult.OK) {
                        return snipper.Image;
                    }
                }
                return null;
            }

        }

        protected override void OnMouseDown(MouseEventArgs e) {

            // get starting position of snip
            if (e.Button != MouseButtons.Left) {
                return;
            }

            pointStart = e.Location;
            rcSelect = new Rectangle(e.Location, new Size(0, 0));
            this.Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            
            //resize area while moving mouse
            if (e.Button != MouseButtons.Left) {
                return;
            }

            int x1 = Math.Min(e.X, pointStart.X);
            int x2 = Math.Max(e.X, pointStart.X);
            int y1 = Math.Min(e.Y, pointStart.Y);
            int y2 = Math.Max(e.Y, pointStart.Y);

            rcSelect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            this.Invalidate();

        }


        protected override void OnMouseUp(MouseEventArgs e) {

            //finalize the snip selection when mouse is released
            if (rcSelect.Width <= 0 || rcSelect.Height <= 0) {  
                return;
            }

            Image = new Bitmap(rcSelect.Width, rcSelect.Height);
            using (Graphics graphics = Graphics.FromImage(Image)) {
                graphics.DrawImage(this.BackgroundImage, new Rectangle(0, 0, Image.Width, Image.Height), rcSelect, GraphicsUnit.Pixel);
            }
            DialogResult = DialogResult.OK;

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            
            //cancel snip on esc
            if (keyData == Keys.Escape) {
                this.DialogResult = DialogResult.Cancel;
            }
            return base.ProcessCmdKey(ref msg, keyData);


        }



    }

}
