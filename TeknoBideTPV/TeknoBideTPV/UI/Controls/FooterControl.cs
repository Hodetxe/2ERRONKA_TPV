using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TeknoBideTPV.Txata;
using TeknoBideTPV.UI.Styles;

namespace TeknoBideTPV.UI.Controls
{
    public partial class FooterControl : UserControl
    {
        public event EventHandler AtzeraClick;
        public event EventHandler TxataClick;

        public FooterControl()
        {
            InitializeComponent();

            btn_Atzera.Tag = Properties.Resources.ico_atzera;

            btn_Atzera.TextAlign = ContentAlignment.MiddleRight;
            btn_Atzera.ImageAlign = ContentAlignment.MiddleLeft;
            btn_Atzera.Padding = new Padding(10, 0, 10, 0);

            btn_Atzera.Region = new Region(IskinakRedondeatu(btn_Atzera.ClientRectangle, 20));
            btn_Atzera.Resize += (s, e) =>
            {
                btn_Atzera.Region = new Region(IskinakRedondeatu(btn_Atzera.ClientRectangle, 20));
                PosizionatuAtzera();
            };

            btn_Atzera.Resize += (s, e) => EskalatuIkonoa((Button)s);

            this.Resize += (s, e) => PosizionatuAtzera();

            EskalatuIkonoa(btn_Atzera);

            btn_Txata.Region = new Region(IskinakRedondeatu(btn_Txata.ClientRectangle, 20));
            btn_Txata.Resize += (s, e) =>
            {
                btn_Txata.Region = new Region(IskinakRedondeatu(btn_Txata.ClientRectangle, 20));
            };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.BackColor = TPVEstiloa.Koloreak.Primary;
            this.ForeColor = TPVEstiloa.Koloreak.TextTitle;
        }

        public string TxataTestua
        {
            get => btn_Txata.Text;
            set => btn_Txata.Text = value;
        }

        public string AtzeraTestua
        {
            get => btn_Atzera.Text;
            set => btn_Atzera.Text = value;
        }

        private void btn_Atzera_Click(object sender, EventArgs e)
        {
            AtzeraClick?.Invoke(this, EventArgs.Empty);
        }

        private void btn_Txata_Click(object sender, EventArgs e)
        {
            Form f = new Form(); 
            f.Text = "Txata"; 
            f.Size = new Size(615, 650); 
            var txat = new TxatPantaila(); 
            txat.Dock = DockStyle.Fill; 
            f.Controls.Add(txat); 
            f.ShowDialog(); 
            TxataClick?.Invoke(this, EventArgs.Empty);
        }

        private void PosizionatuAtzera()
        {
            if (btn_Atzera == null) return;
            btn_Atzera.Location = new Point(
                this.Width - btn_Atzera.Width - 20,
                (this.Height - btn_Atzera.Height) / 2
            );
        }

        private void EskalatuIkonoa(Button btn)
        {
            if (btn.Tag is not Image original) return;

            int ikonoAltuera = (int)(btn.Height * 0.6);
            int ikonoZabalera = ikonoAltuera;

            if (ikonoZabalera <= 0 || ikonoAltuera <= 0) return;

            if (btn.Image != null && !ReferenceEquals(btn.Image, original))
                btn.Image.Dispose();

            btn.Image = new Bitmap(original, new Size(ikonoZabalera, ikonoAltuera));
        }

        private GraphicsPath IskinakRedondeatu(Rectangle rect, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
