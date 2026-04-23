using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TeknoBideTPV.UI.Controls;
using TeknoBideTPV.UI.Styles;
using TeknoBideTPV.Zerbitzuak;

namespace TeknoBideTPV.UI
{
    public partial class MenuNagusia : Form
    {
        private const int _eguraldiMinutuak = 60;
        private static readonly EguraldiZerbitzua _eguraldiZerbitzua = new EguraldiZerbitzua(cacheDenbora: TimeSpan.FromMinutes(_eguraldiMinutuak));
        private static EguraldiEmaitza? _eguraldiCache;
        private static DateTime _eguraldiAzkenEguneraketa;
        private System.Windows.Forms.Timer? _eguraldiTimer;
        private System.Windows.Forms.Timer? _orduaTimer;

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

            btn_Txostenak.Text = "EGURALDIA";

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

            TPVEstiloa.ProfesionalizatuKontrolak(this);

            OrduaTimerraPrestatu();
            EguraldiTimerraPrestatu();
            await EguraldiaBerrituAsync();
        }

        private void OrduaTimerraPrestatu()
        {
            if (_orduaTimer != null)
                return;

            _orduaTimer = new System.Windows.Forms.Timer();
            _orduaTimer.Interval = 10 * 1000;
            _orduaTimer.Tick += (_, __) =>
            {
                try
                {
                    headerControl_Menua.DataOrdua = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm");
                }
                catch
                {
                }
            };
            _orduaTimer.Start();

            this.Disposed -= MenuNagusia_Disposed;
            this.Disposed += MenuNagusia_Disposed;
        }

        private void EguraldiTimerraPrestatu()
        {
            if (_eguraldiTimer != null)
                return;

            _eguraldiTimer = new System.Windows.Forms.Timer();
            _eguraldiTimer.Interval = 60 * 60 * 1000;
            _eguraldiTimer.Tick += async (_, __) => await EguraldiaBerrituAsync();
            _eguraldiTimer.Start();

            this.Disposed -= MenuNagusia_Disposed;
            this.Disposed += MenuNagusia_Disposed;
        }

        private void MenuNagusia_Disposed(object? sender, EventArgs e)
        {
            if (_eguraldiTimer != null)
            {
                _eguraldiTimer.Stop();
                _eguraldiTimer.Dispose();
                _eguraldiTimer = null;
            }

            if (_orduaTimer != null)
            {
                _orduaTimer.Stop();
                _orduaTimer.Dispose();
                _orduaTimer = null;
            }
        }

        private async System.Threading.Tasks.Task EguraldiaBerrituAsync()
        {
            try
            {
                var emaitza = await _eguraldiZerbitzua.LortuEmaitzaAsync(true);
                if (emaitza != null)
                {
                    _eguraldiCache = emaitza;
                    _eguraldiAzkenEguneraketa = DateTime.Now;
                }
            }
            catch
            {
            }
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
            TPVEstiloa.ProfesionalizatuKontrolak(btn);
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
            EguraldiaIrekiAsync();
        }

        private async void EguraldiaIrekiAsync()
        {
            try
            {
                btn_Txostenak.Enabled = false;

                if (_eguraldiCache == null)
                    await EguraldiaBerrituAsync();

                var emaitza = _eguraldiCache;
                if (emaitza == null)
                {
                    MessageBox.Show(
                        "Ezin izan da eguraldiaren XML-a irakurri. Egiaztatu TPV_EGURALDI_XML aldagaia edo internet konexioa.",
                        "Eguraldia",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                EguraldiAsteaErakutsi(emaitza);
            }
            catch
            {
            }
            finally
            {
                btn_Txostenak.Enabled = true;
            }
        }

        private void EguraldiAsteaErakutsi(EguraldiEmaitza emaitza)
        {
            using var f = new Form();
            f.Text = "Eguraldia (astea)";
            f.StartPosition = FormStartPosition.CenterParent;
            f.Width = 900;
            f.Height = 600;

            string GoiburuaTestua()
            {
                var azken = _eguraldiAzkenEguneraketa == default ? DateTime.Now : _eguraldiAzkenEguneraketa;
                var duela = DateTime.Now - azken;
                var min = (int)Math.Max(0, Math.Floor(duela.TotalMinutes));
                var hurrengoa = azken.AddMinutes(_eguraldiMinutuak);
                return $"{emaitza.Info}\nAzken eguneraketa: {azken:yyyy-MM-dd HH:mm} (duela {min} min) | Hurrengoa: {hurrengoa:HH:mm}";
            }

            var lbl = new Label
            {
                Dock = DockStyle.Top,
                Height = 80,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(16, 10, 16, 10),
                Text = GoiburuaTestua()
            };

            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true
            };

            var data = emaitza.Astea.Select(x => new
            {
                Eguna = EuskaraEguna(x.Data),
                Data = x.Data.ToString("yyyy-MM-dd"),
                Tenperatura = x.Tenperatura ?? "--",
                Egoera = x.Egoera ?? "--",
                Haizea = x.Haizea ?? "--"
            }).ToList();

            dgv.DataSource = data;
            TPVEstiloa.EstilatuDataGridView(dgv);

            f.Controls.Add(dgv);
            f.Controls.Add(lbl);

            TPVEstiloa.ProfesionalizatuKontrolak(f);
            TPVEstiloa.ProfesionalizatuKontrolak(lbl);

            var t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            t.Tick += (_, __) => { try { lbl.Text = GoiburuaTestua(); } catch { } };
            f.FormClosed += (_, __) => { try { t.Stop(); t.Dispose(); } catch { } };
            t.Start();

            f.ShowDialog();
        }

        private static string EuskaraEguna(DateTime data)
        {
            return data.DayOfWeek switch
            {
                DayOfWeek.Monday => "Astelehena",
                DayOfWeek.Tuesday => "Asteartea",
                DayOfWeek.Wednesday => "Asteazkena",
                DayOfWeek.Thursday => "Osteguna",
                DayOfWeek.Friday => "Ostirala",
                DayOfWeek.Saturday => "Larunbata",
                DayOfWeek.Sunday => "Igandea",
                _ => "Eguna"
            };
        }

        private void btn_Ezarpenak_Click(object sender, EventArgs e)
        {
            var inbentarioa = new InbentarioaForm(this);
            this.Hide();
            inbentarioa.Show();
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
