using Tesseract;

namespace snip_to_text
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //make form invisible
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 0;
            this.Size = new Size(0, 0);

            // run snipping process on startup
            this.Load += new EventHandler(Form1_Load);

        }

        private void Form1_Load(object sender, EventArgs e) {

            // take and save screenshot
            var bmp = Snipper.Snip();
            if (bmp == null) {
                System.Windows.Forms.Application.Exit();
                return;
            } 

            // process image
            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default)) {
                using (var page = engine.Process(PixConverter.ToPix(new Bitmap(bmp)))) {
                    var text = page.GetText();
                    if (text != null && text != "") {
                        Clipboard.SetText(text);
                    }

                    // Console.Out.WriteLine(text);

                }
            }
            System.Windows.Forms.Application.Exit();
        }

    }
}
