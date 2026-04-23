using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using TeknoBideTPV.Zerbitzuak;

namespace TeknoBideTPV.UI
{
    public partial class LoginForm : Form
    {
        private bool _hasierakoaGordeta = false;
        private TableLayoutPanel? tlp_EgituraNagusia;
        private Panel? pnl_EzkerEdukiontzia;
        private Panel? pnl_EskuinaEdukiontzia;
        private PanelBiribildua? pnl_SaioHasieraTxartela;
        private FlowLayoutPanel? flp_SaioHasieraEdukia;
        private Label? lbl_Izenburua;
        private Label? lbl_Azpititulua;
        private readonly float _izenburuPuntuak = 36F;
        private readonly float _etiketaPuntuak = 18F;
        private readonly float _sarreraPuntuak = 20F;
        private readonly float _botoiPuntuak = 22F;
        private readonly float _azpitituluPuntuak = 14F;

        public LoginForm()
        {
            InitializeComponent();

            LogoaKargatu();

            PantailaOsoaEzarri();

            this.BackColor = TPVEstiloa.Koloreak.Background;
            
            btn_Sartu.BackColor = TPVEstiloa.Koloreak.Primary;
            btn_Sartu.ForeColor = TPVEstiloa.Koloreak.White;
            btn_Sartu.FlatStyle = FlatStyle.Flat;
            btn_Sartu.FlatAppearance.BorderSize = 0;
            
            lbl_Erabiltzailea.ForeColor = TPVEstiloa.Koloreak.TextPrimary;
            lbl_Pasahitza.ForeColor = TPVEstiloa.Koloreak.TextPrimary;

            txt_Erabiltzailea.BorderStyle = BorderStyle.FixedSingle;
            txt_Pasahitza.BorderStyle = BorderStyle.FixedSingle;

            this.WindowState = FormWindowState.Maximized;

            this.Shown += LoginForm_Shown;
            this.Resize += LoginForm_Resize;

            this.MaximizeBox = false;
            img_Logoa.SizeMode = PictureBoxSizeMode.Zoom;

            SaioHasieraTxartelaPrestatu();
            this.AcceptButton = btn_Sartu;

            TPVEstiloa.ProfesionalizatuKontrolak(this);
        }

        private void LogoaKargatu()
        {
            if (img_Logoa == null)
                return;

            img_Logoa.Image?.Dispose();
            img_Logoa.Image = null;

            var bideZerrenda = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "NovaFrameLabs.png"),
                Path.Combine(AppContext.BaseDirectory, "Assets", "NovaFrameLabs.png"),
                Path.Combine(AppContext.BaseDirectory, "Resources", "NovaFrameLabs.png")
            };

            foreach (var bidea in bideZerrenda)
            {
                if (!File.Exists(bidea))
                    continue;

                using var fitxategiJarioa = new FileStream(bidea, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var irudia = Image.FromStream(fitxategiJarioa);
                img_Logoa.Image?.Dispose();
                img_Logoa.Image = new Bitmap(irudia);
                return;
            }
        }

        private void PantailaOsoaEzarri()
        {
            StartPosition = FormStartPosition.Manual;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            Bounds = Screen.PrimaryScreen.Bounds;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            PantailaOsoaEzarri();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            var laukia = this.ClientRectangle;
            if (laukia.Width <= 0 || laukia.Height <= 0)
            {
                base.OnPaintBackground(e);
                return;
            }

            using var brotxa = new LinearGradientBrush(
                laukia,
                TPVEstiloa.Koloreak.LoginGradienteaArgia,
                TPVEstiloa.Koloreak.LoginGradienteaIluna,
                LinearGradientMode.Vertical);
            e.Graphics.FillRectangle(brotxa, laukia);
        }

        private void SaioHasieraTxartelaPrestatu()
        {
            tlp_EgituraNagusia = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(32),
                BackColor = Color.Transparent
            };
            tlp_EgituraNagusia.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlp_EgituraNagusia.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlp_EgituraNagusia.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            pnl_EzkerEdukiontzia = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };
            pnl_EskuinaEdukiontzia = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(32) };

            tlp_EgituraNagusia.Controls.Add(pnl_EzkerEdukiontzia, 0, 0);
            tlp_EgituraNagusia.Controls.Add(pnl_EskuinaEdukiontzia, 1, 0);

            Controls.Remove(img_Logoa);
            pnl_EskuinaEdukiontzia.Controls.Add(img_Logoa);
            img_Logoa.Dock = DockStyle.Fill;

            Controls.Add(tlp_EgituraNagusia);
            tlp_EgituraNagusia.BringToFront();

            pnl_SaioHasieraTxartela = new PanelBiribildua
            {
                BackColor = TPVEstiloa.Koloreak.White,
                ErtzKolorea = TPVEstiloa.Koloreak.Border,
                ErtzLodiera = 1,
                ErtzErradioa = 18,
                ItzalKolorea = Color.FromArgb(28, 0, 0, 0),
                ItzalDesplazamendua = 8,
                ItzalLausotzea = 18,
                AzentuKolorea = TPVEstiloa.Koloreak.Primary,
                AzentuZabalera = 12,
                Padding = new Padding(42, 34, 36, 34)
            };

            lbl_Izenburua = new Label
            {
                AutoSize = true,
                ForeColor = TPVEstiloa.Koloreak.TextTitle,
                Font = new Font("Segoe UI Semibold", _izenburuPuntuak, FontStyle.Bold, GraphicsUnit.Point, 0),
                Text = "TeknoBide TPV"
            };

            lbl_Azpititulua = new Label
            {
                AutoSize = true,
                ForeColor = TPVEstiloa.Koloreak.TextSecondary,
                Font = new Font("Segoe UI", _azpitituluPuntuak, FontStyle.Regular, GraphicsUnit.Point, 0),
                Text = "Sartu zure datuekin"
            };

            flp_SaioHasieraEdukia = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };

            lbl_Izenburua.Dock = DockStyle.Top;
            lbl_Azpititulua.Dock = DockStyle.Top;
            lbl_Erabiltzailea.Dock = DockStyle.Top;
            txt_Erabiltzailea.Dock = DockStyle.Top;
            lbl_Pasahitza.Dock = DockStyle.Top;
            txt_Pasahitza.Dock = DockStyle.Top;
            btn_Sartu.Dock = DockStyle.Top;

            flp_SaioHasieraEdukia.Controls.Add(lbl_Izenburua);
            flp_SaioHasieraEdukia.Controls.Add(lbl_Azpititulua);
            flp_SaioHasieraEdukia.Controls.Add(lbl_Erabiltzailea);
            flp_SaioHasieraEdukia.Controls.Add(txt_Erabiltzailea);
            flp_SaioHasieraEdukia.Controls.Add(lbl_Pasahitza);
            flp_SaioHasieraEdukia.Controls.Add(txt_Pasahitza);
            flp_SaioHasieraEdukia.Controls.Add(btn_Sartu);

            pnl_SaioHasieraTxartela.Controls.Add(flp_SaioHasieraEdukia);

            pnl_EzkerEdukiontzia.Controls.Add(pnl_SaioHasieraTxartela);
            pnl_SaioHasieraTxartela.BringToFront();

            lbl_Erabiltzailea.Font = new Font("Segoe UI Semibold", _etiketaPuntuak, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Pasahitza.Font = new Font("Segoe UI Semibold", _etiketaPuntuak, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Erabiltzailea.ForeColor = TPVEstiloa.Koloreak.TextPrimary;
            lbl_Pasahitza.ForeColor = TPVEstiloa.Koloreak.TextPrimary;

            txt_Erabiltzailea.Font = new Font("Segoe UI", _sarreraPuntuak, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_Pasahitza.Font = new Font("Segoe UI", _sarreraPuntuak, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_Erabiltzailea.AutoSize = false;
            txt_Pasahitza.AutoSize = false;
            txt_Erabiltzailea.Height = 56;
            txt_Pasahitza.Height = 56;
            txt_Erabiltzailea.BorderStyle = BorderStyle.FixedSingle;
            txt_Pasahitza.BorderStyle = BorderStyle.FixedSingle;
            txt_Erabiltzailea.PlaceholderText = "Langile kodea";
            txt_Pasahitza.PlaceholderText = "Pasahitza";
            txt_Pasahitza.UseSystemPasswordChar = true;

            btn_Sartu.Font = new Font("Segoe UI Semibold", _botoiPuntuak, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Sartu.Height = 72;
            btn_Sartu.BackColor = TPVEstiloa.Koloreak.Primary;
            btn_Sartu.ForeColor = TPVEstiloa.Koloreak.White;
            btn_Sartu.FlatStyle = FlatStyle.Flat;
            btn_Sartu.FlatAppearance.BorderSize = 0;
            btn_Sartu.FlatAppearance.MouseOverBackColor = TPVEstiloa.Koloreak.PrimaryHover;
            btn_Sartu.FlatAppearance.MouseDownBackColor = TPVEstiloa.Koloreak.PrimaryHover;
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            if (!_hasierakoaGordeta)
            {
                TPVEstiloa.PantailarenEskalatuaHasi(this);
                _hasierakoaGordeta = true;
            }
            BeginInvoke(new Action(() =>
            {
                PantailaOsoaEzarri();
                BirarraztuKontrolak();
            }));
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            if (!_hasierakoaGordeta)
                return;

            BirarraztuKontrolak();
        }

        private void BirarraztuKontrolak()
        {
            if (this.ClientSize.Width == 0 || this.ClientSize.Height == 0) return;

            if (pnl_SaioHasieraTxartela == null || lbl_Izenburua == null || lbl_Azpititulua == null || pnl_EzkerEdukiontzia == null || tlp_EgituraNagusia == null || flp_SaioHasieraEdukia == null)
                return;

            bool biZutabe = this.ClientSize.Width >= 1100;
            tlp_EgituraNagusia.ColumnStyles[0].Width = biZutabe ? 50F : 100F;
            tlp_EgituraNagusia.ColumnStyles[1].Width = biZutabe ? 50F : 0F;
            if (pnl_EskuinaEdukiontzia != null) pnl_EskuinaEdukiontzia.Visible = biZutabe;

            float xEskala = pnl_EzkerEdukiontzia.ClientSize.Width / 820f;
            float yEskala = pnl_EzkerEdukiontzia.ClientSize.Height / 720f;
            float eskala = Math.Max(0.95f, Math.Min(1.35f, Math.Min(xEskala, yEskala)));

            lbl_Izenburua.Font = new Font(lbl_Izenburua.Font.FontFamily, _izenburuPuntuak * eskala, lbl_Izenburua.Font.Style);
            lbl_Azpititulua.Font = new Font(lbl_Azpititulua.Font.FontFamily, _azpitituluPuntuak * eskala, lbl_Azpititulua.Font.Style);
            lbl_Erabiltzailea.Font = new Font(lbl_Erabiltzailea.Font.FontFamily, _etiketaPuntuak * eskala, lbl_Erabiltzailea.Font.Style);
            lbl_Pasahitza.Font = new Font(lbl_Pasahitza.Font.FontFamily, _etiketaPuntuak * eskala, lbl_Pasahitza.Font.Style);
            txt_Erabiltzailea.Font = new Font(txt_Erabiltzailea.Font.FontFamily, _sarreraPuntuak * eskala, txt_Erabiltzailea.Font.Style);
            txt_Pasahitza.Font = new Font(txt_Pasahitza.Font.FontFamily, _sarreraPuntuak * eskala, txt_Pasahitza.Font.Style);
            btn_Sartu.Font = new Font(btn_Sartu.Font.FontFamily, _botoiPuntuak * eskala, btn_Sartu.Font.Style);

            int txartelaMarjina = Math.Max(24, (int)(pnl_EzkerEdukiontzia.ClientSize.Width / 18f));
            int txartelaZabalera = Math.Min((int)(pnl_EzkerEdukiontzia.ClientSize.Width - (txartelaMarjina * 2)), biZutabe ? 720 : 640);
            if (txartelaZabalera < 360) txartelaZabalera = 360;

            int paddingH = (int)(36 * eskala);
            int paddingV = (int)(30 * eskala);
            int azentuZabalera = (int)(12 * eskala);
            pnl_SaioHasieraTxartela.AzentuZabalera = azentuZabalera;
            pnl_SaioHasieraTxartela.Padding = new Padding(paddingH + azentuZabalera, paddingV, paddingH, paddingV);

            txt_Erabiltzailea.Height = (int)(56 * eskala);
            txt_Pasahitza.Height = (int)(56 * eskala);
            btn_Sartu.Height = (int)(72 * eskala);

            int edukiaZabalera = txartelaZabalera - pnl_SaioHasieraTxartela.Padding.Left - pnl_SaioHasieraTxartela.Padding.Right;
            if (edukiaZabalera < 260) edukiaZabalera = 260;

            int tarteaTxikia = (int)(8 * eskala);
            int tarteaErtaina = (int)(18 * eskala);
            int tarteaHandia = (int)(26 * eskala);
            int tarteaIzenburu = (int)(22 * eskala);

            lbl_Izenburua.MaximumSize = new Size(edukiaZabalera, 0);
            lbl_Azpititulua.MaximumSize = new Size(edukiaZabalera, 0);
            lbl_Erabiltzailea.MaximumSize = new Size(edukiaZabalera, 0);
            lbl_Pasahitza.MaximumSize = new Size(edukiaZabalera, 0);

            lbl_Izenburua.Margin = new Padding(0, 0, 0, (int)(10 * eskala));
            lbl_Azpititulua.Margin = new Padding(0, 0, 0, tarteaIzenburu);
            lbl_Erabiltzailea.Margin = new Padding(0, 0, 0, tarteaTxikia);
            txt_Erabiltzailea.Margin = new Padding(0, 0, 0, tarteaErtaina);
            lbl_Pasahitza.Margin = new Padding(0, 0, 0, tarteaTxikia);
            txt_Pasahitza.Margin = new Padding(0, 0, 0, tarteaHandia);
            btn_Sartu.Margin = new Padding(0);

            txt_Erabiltzailea.Width = edukiaZabalera;
            txt_Pasahitza.Width = edukiaZabalera;
            btn_Sartu.Width = edukiaZabalera;

            flp_SaioHasieraEdukia.Width = edukiaZabalera;
            flp_SaioHasieraEdukia.PerformLayout();

            int gutxienekoAltuera = (int)(420 * eskala);
            int edukiaAltuera = flp_SaioHasieraEdukia.GetPreferredSize(new Size(edukiaZabalera, 0)).Height;
            int txartelaAltuera = edukiaAltuera + pnl_SaioHasieraTxartela.Padding.Top + pnl_SaioHasieraTxartela.Padding.Bottom + pnl_SaioHasieraTxartela.ItzalDesplazamendua;
            if (txartelaAltuera < gutxienekoAltuera) txartelaAltuera = gutxienekoAltuera;

            pnl_SaioHasieraTxartela.Size = new Size(txartelaZabalera, txartelaAltuera);
            pnl_SaioHasieraTxartela.Location = new Point((pnl_EzkerEdukiontzia.ClientSize.Width - pnl_SaioHasieraTxartela.Width) / 2, (pnl_EzkerEdukiontzia.ClientSize.Height - pnl_SaioHasieraTxartela.Height) / 2);
            pnl_SaioHasieraTxartela.Invalidate();
        }

        private async void btn_Sartu_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txt_Erabiltzailea.Text, out int langileKodea))
            {
                MessageBox.Show("Langile kodea zenbaki bat izan behar da", "Errorea",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string pasahitza = txt_Pasahitza.Text;

            var api = new ApiZerbitzua();
            var erantzuna = await api.LoginAsync(langileKodea, pasahitza);

            if (erantzuna == null)
            {
                MessageBox.Show("Errorea APIarekin konektatzean", "Errorea",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (erantzuna.Ok)
            {
                SesioZerbitzua.LangileaId = erantzuna.Data.Id;
                SesioZerbitzua.Izena = erantzuna.Data.Izena;
                SesioZerbitzua.Gerentea = erantzuna.Data.Gerentea;

                MessageBox.Show(erantzuna.Message, "Sarrera",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                var menuNagusia = new MenuNagusia();
                menuNagusia.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(erantzuna.Message, "Errorea",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private sealed class PanelBiribildua : Panel
        {
            public int ErtzErradioa { get; set; } = 16;
            public int ErtzLodiera { get; set; } = 1;
            public Color ErtzKolorea { get; set; } = Color.Gainsboro;
            public Color ItzalKolorea { get; set; } = Color.FromArgb(30, 0, 0, 0);
            public int ItzalDesplazamendua { get; set; } = 8;
            public int ItzalLausotzea { get; set; } = 18;
            public Color AzentuKolorea { get; set; } = Color.Transparent;
            public int AzentuZabalera { get; set; } = 0;

            public PanelBiribildua()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
                UpdateStyles();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                var itzalLaukia = new Rectangle(ItzalDesplazamendua, ItzalDesplazamendua, Width - ItzalDesplazamendua - 1, Height - ItzalDesplazamendua - 1);
                using (var itzalBidea = LaukiBiribilduaSortu(itzalLaukia, ErtzErradioa))
                using (var itzalBrotxa = new SolidBrush(ItzalKolorea))
                {
                    e.Graphics.FillPath(itzalBrotxa, itzalBidea);
                }

                var laukia = new Rectangle(0, 0, Width - ItzalDesplazamendua - 1, Height - ItzalDesplazamendua - 1);
                using (var bidea = LaukiBiribilduaSortu(laukia, ErtzErradioa))
                using (var betegarriBrotxa = new SolidBrush(BackColor))
                using (var arkatza = new Pen(ErtzKolorea, ErtzLodiera))
                {
                    e.Graphics.FillPath(betegarriBrotxa, bidea);
                    if (AzentuZabalera > 0 && AzentuKolorea.A > 0)
                    {
                        var aurrekoMoztu = e.Graphics.Clip;
                        e.Graphics.SetClip(bidea);
                        using var azentuBrotxa = new SolidBrush(AzentuKolorea);
                        e.Graphics.FillRectangle(azentuBrotxa, laukia.X, laukia.Y, AzentuZabalera, laukia.Height);
                        e.Graphics.Clip = aurrekoMoztu;
                    }
                    e.Graphics.DrawPath(arkatza, bidea);
                }
            }

            private static GraphicsPath LaukiBiribilduaSortu(Rectangle laukia, int erradioa)
            {
                int diametroa = erradioa * 2;
                var bidea = new GraphicsPath();
                bidea.AddArc(laukia.X, laukia.Y, diametroa, diametroa, 180, 90);
                bidea.AddArc(laukia.Right - diametroa, laukia.Y, diametroa, diametroa, 270, 90);
                bidea.AddArc(laukia.Right - diametroa, laukia.Bottom - diametroa, diametroa, diametroa, 0, 90);
                bidea.AddArc(laukia.X, laukia.Bottom - diametroa, diametroa, diametroa, 90, 90);
                bidea.CloseFigure();
                return bidea;
            }
        }
    }
}
