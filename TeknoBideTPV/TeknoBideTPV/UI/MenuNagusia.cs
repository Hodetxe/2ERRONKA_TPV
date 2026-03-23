using System;
using System.Drawing;
using System.Windows.Forms;
using TeknoBideTPV.UI.Controls;
using TeknoBideTPV.UI.Styles;
using TeknoBideTPV.Zerbitzuak;

namespace TeknoBideTPV.UI
{
    public partial class MenuNagusia : Form
    {
        public MenuNagusia()
        {
            InitializeComponent();

            this.Controls.SetChildIndex(tlp_Menua, 0);
            this.Controls.SetChildIndex(footerControl_MenuNagusia, 2);

            foreach (Control c in tlp_Menua.Controls)
            {
                if (c is Button btn)
                {
                    btn.Dock = DockStyle.Fill;
                    btn.Margin = new Padding(10);
                }
            }

            PrestatuMenuBotoia(btn_EskariakIkusi, Properties.Resources.ico_eskariak_ikusi);
            PrestatuMenuBotoia(btn_EskariakSortu, Properties.Resources.ico_eskaria_sortu);
            PrestatuMenuBotoia(btn_ErreserbakIkusi, Properties.Resources.ico_erreserbak_ikusi);
            PrestatuMenuBotoia(btn_ErreserbaSortu, Properties.Resources.ico_erreserba_sortu);
            PrestatuMenuBotoia(btn_ErreserbaAmaitu, Properties.Resources.ico_erreserba_amaitu);
            PrestatuMenuBotoia(btn_MahaiakKudeatu, Properties.Resources.ico_mahaiak_kudeatu);
            PrestatuMenuBotoia(btn_Txostenak, Properties.Resources.ico_txostenak);
            PrestatuMenuBotoia(btn_Ezarpenak, Properties.Resources.ico_ezarpenak);

            //minimizatu maximizatu eta itxi botoiak ezkutatu
            this.ControlBox = false;
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.Load += MenuNagusia_Load;
            this.Shown += MenuNagusia_Shown;

            PrestatuFooter();
        }

        private void MenuNagusia_Shown(object sender, EventArgs e)
        {
            TPVEstiloaFinkoa.Prestatu(this);
            TPVEstiloaFinkoa.Aplikatu(this);
        }

        private async void MenuNagusia_Load(object sender, EventArgs e)
        {
            this.BackColor = TPVEstiloa.Koloreak.Background;

            headerControl_Menua.Izena = "TXAPELA";
            headerControl_Menua.Titulo = "MENU NAGUSIA";
            headerControl_Menua.Erabiltzailea = SesioZerbitzua.Izena;
            headerControl_Menua.DataOrdua = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm");
        }

        private void PrestatuFooter()
        {
            footerControl_MenuNagusia.AtzeraTestua = "Atera";

            footerControl_MenuNagusia.AtzeraClick += (s, e) =>
            {
                SesioZerbitzua.Logout();

                var login = new LoginForm();
                login.Show();

                this.Close();
            };
        }

        private void PrestatuMenuBotoia(Button btn, Image ikonoOriginala)
        {
            btn.TextAlign = ContentAlignment.BottomCenter;
            btn.ImageAlign = ContentAlignment.TopCenter;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1; 
            btn.FlatAppearance.BorderColor = TPVEstiloa.Koloreak.Primary; 
            btn.FlatAppearance.MouseOverBackColor = TPVEstiloa.Koloreak.Secondary; 
            btn.FlatAppearance.MouseDownBackColor = TPVEstiloa.Koloreak.Primary; 

            btn.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            btn.BackColor = Color.White;
            btn.ForeColor = TPVEstiloa.Koloreak.TextTitle; 

            btn.Tag = ikonoOriginala;

            btn.Padding = new Padding(10, 20, 10, 10);

            btn.Resize += (s, e) => IkonoenTamainaAjustatu((Button)s);
            IkonoenTamainaAjustatu(btn);
        }

        private void IkonoenTamainaAjustatu(Button btn)
        {
            if (btn.Tag is not Image original) return;

            int ikonoZabalera = (int)(btn.Width * 0.6);
            int ikonoAltuera = (int)(btn.Height * 0.6);

            if (ikonoZabalera <= 0 || ikonoAltuera <= 0) return;

            if (btn.Image != null && !ReferenceEquals(btn.Image, original))
            {
                btn.Image.Dispose();
            }

            var eskalatua = new Bitmap(original, new Size(ikonoZabalera, ikonoAltuera));
            btn.Image = eskalatua;
        }

        private void btn_EskariakIkusi_Click(object sender, EventArgs e)
        {
            var EskariakForm = new EskariakForm(this);
            this.Hide();
            EskariakForm.Show();
        }

        private void btn_EskariakSortu_Click(object sender, EventArgs e)
        {
            var EskariakSortuForm = new EskariakSortuForm(this);
            this.Hide();
            EskariakSortuForm.Show();
        }

        private void btn_ErreserbakIkusi_Click(object sender, EventArgs e)
        {
            var ErreserbakForm = new ErreserbakForm(this);
            this.Hide();
            ErreserbakForm.Show();
        }

        private void btn_ErreserbaSortu_Click(object sender, EventArgs e)
        {
            var ErreserbakSortuForm = new ErreserbakSortuForm(this);
            this.Hide();
            ErreserbakSortuForm.Show();
        }

        private void btn_ErreserbaAmaitu_Click(object sender, EventArgs e)
        {
            var ErreserbaAmaituForm = new ErreserbaAmaituForm(this);
            this.Hide();
            ErreserbaAmaituForm.Show();
        }

        private void btn_MahaiakKudeatu_Click(object sender, EventArgs e)
        {
            var MahaiakKudeatuForm = new MahaiakKudeatuForm(this);
            this.Hide();
            MahaiakKudeatuForm.Show();
        }

        private void btn_Txostenak_Click(object sender, EventArgs e)
        {
        }

        private void btn_Ezarpenak_Click(object sender, EventArgs e)
        {
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            SesioZerbitzua.Logout();

            var login = new LoginForm();
            login.Show();

            this.Close();
        }
    }
}
