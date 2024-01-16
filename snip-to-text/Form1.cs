namespace snip_to_text
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Button b = new Button();

            b.Location = new Point(0, 0);
            b.Height = 30;
            b.Width = 250;

            b.Text = "snip";

            b.Click += new EventHandler(B_Click);

            Controls.Add(b);

        }

        private void B_Click(object sender, EventArgs e) {
            var bmp = Snipper.Snip();
            if (bmp != null ) {
                bmp.Save("image.png", System.Drawing.Imaging.ImageFormat.Png);
                System.Windows.Forms.Application.Exit();
            }
        }

    }
}
