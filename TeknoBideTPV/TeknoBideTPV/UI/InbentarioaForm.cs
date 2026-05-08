using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeknoBideTPV.DTOak;
using TeknoBideTPV.UI.Controls;
using TeknoBideTPV.UI.Styles;
using TeknoBideTPV.Zerbitzuak;

namespace TeknoBideTPV.UI
{
    public class InbentarioaForm : Form
    {
        private readonly ApiZerbitzua _api = new ApiZerbitzua();
        private readonly Form _aurrekoPantaila;

        private readonly HeaderControl _header = new HeaderControl();
        private readonly FooterControl _footer = new FooterControl();
        private readonly DataGridView _dgv = new DataGridView();
        private readonly Button _btnGorde = new Button();
        private readonly Button _btnBerritu = new Button();
        private readonly TextBox _txtOdooUrl = new TextBox();
        private readonly TextBox _txtOdooToken = new TextBox();
        private readonly Button _btnEzarpenakGorde = new Button();
        private readonly Label _lblOdooUrl = new Label();
        private readonly Label _lblOdooToken = new Label();

        private List<ProduktuaDto> _produktuak = new List<ProduktuaDto>();
        private readonly Dictionary<int, int> _stockHasierakoa = new Dictionary<int, int>();

        public InbentarioaForm(Form aurrekoPantaila)
        {
            _aurrekoPantaila = aurrekoPantaila;

            this.ControlBox = false;
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1200, 700);

            this.BackColor = TPVEstiloa.Koloreak.Background;
            this.Load += InbentarioaForm_Load;

            _header.Dock = DockStyle.Top;
            _header.Height = 150;
            _header.Izena = "TXAPELA";
            _header.Titulo = "INBENTARIOA";
            _header.Erabiltzailea = SesioZerbitzua.Izena;
            _header.DataOrdua = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm");

            _footer.Dock = DockStyle.Bottom;
            _footer.Height = 100;
            _footer.AtzeraTestua = "Atzera";
            _footer.AtzeraClick += (_, __) =>
            {
                _aurrekoPantaila.Show();
                this.Close();
            };

            _dgv.Dock = DockStyle.Fill;
            _dgv.AllowUserToAddRows = false;
            _dgv.AllowUserToDeleteRows = false;
            _dgv.MultiSelect = false;
            _dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _dgv.AutoGenerateColumns = false;

            _dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Izena",
                HeaderText = "Produktua",
                Name = "Izena",
                ReadOnly = true
            });

            _dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Mota",
                HeaderText = "Mota",
                Name = "Mota",
                ReadOnly = true
            });

            _dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Prezioa",
                HeaderText = "Prezioa (€)",
                Name = "Prezioa",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "0.00" }
            });

            _dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Stock",
                HeaderText = "Stock",
                Name = "Stock",
                ReadOnly = false
            });

            _btnGorde.Text = "GORDE";
            _btnGorde.BackColor = TPVEstiloa.Koloreak.Primary;
            _btnGorde.ForeColor = TPVEstiloa.Koloreak.White;
            _btnGorde.FlatStyle = FlatStyle.Flat;
            _btnGorde.FlatAppearance.BorderSize = 0;
            _btnGorde.Width = 220;
            _btnGorde.Height = 54;
            _btnGorde.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnGorde.Click += async (_, __) => await GordeAldaketakAsync();

            _btnBerritu.Text = "BERRITU";
            _btnBerritu.BackColor = TPVEstiloa.Koloreak.Secondary;
            _btnBerritu.ForeColor = TPVEstiloa.Koloreak.White;
            _btnBerritu.FlatStyle = FlatStyle.Flat;
            _btnBerritu.FlatAppearance.BorderSize = 0;
            _btnBerritu.Width = 220;
            _btnBerritu.Height = 54;
            _btnBerritu.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnBerritu.Click += async (_, __) => await KargatuProduktuakAsync();

            var pnlGoikoEkintzak = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = TPVEstiloa.Koloreak.Background,
                Padding = new Padding(16, 10, 16, 10)
            };

            _lblOdooUrl.Text = "Odoo URL";
            _lblOdooUrl.AutoSize = true;
            _lblOdooUrl.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            _lblOdooUrl.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _lblOdooUrl.Left = 16;
            _lblOdooUrl.Top = 10;

            _txtOdooUrl.Width = 520;
            _txtOdooUrl.Height = 34;
            _txtOdooUrl.Left = 16;
            _txtOdooUrl.Top = _lblOdooUrl.Bottom + 6;
            _txtOdooUrl.BackColor = Color.White;

            _lblOdooToken.Text = "Odoo Token";
            _lblOdooToken.AutoSize = true;
            _lblOdooToken.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            _lblOdooToken.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            _lblOdooToken.Left = _txtOdooUrl.Right + 18;
            _lblOdooToken.Top = 10;

            _txtOdooToken.Width = 420;
            _txtOdooToken.Height = 34;
            _txtOdooToken.Left = _lblOdooToken.Left;
            _txtOdooToken.Top = _lblOdooToken.Bottom + 6;
            _txtOdooToken.BackColor = Color.White;
            _txtOdooToken.UseSystemPasswordChar = true;

            _btnEzarpenakGorde.Text = "ODOO GORDE";
            _btnEzarpenakGorde.BackColor = TPVEstiloa.Koloreak.Primary;
            _btnEzarpenakGorde.ForeColor = TPVEstiloa.Koloreak.White;
            _btnEzarpenakGorde.FlatStyle = FlatStyle.Flat;
            _btnEzarpenakGorde.FlatAppearance.BorderSize = 0;
            _btnEzarpenakGorde.Width = 220;
            _btnEzarpenakGorde.Height = 54;
            _btnEzarpenakGorde.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            _btnEzarpenakGorde.Top = _txtOdooUrl.Bottom + 16;
            _btnEzarpenakGorde.Click += (_, __) => GordeOdooEzarpenak();

            pnlGoikoEkintzak.Controls.Add(_btnGorde);
            pnlGoikoEkintzak.Controls.Add(_btnBerritu);
            pnlGoikoEkintzak.Controls.Add(_lblOdooUrl);
            pnlGoikoEkintzak.Controls.Add(_txtOdooUrl);
            pnlGoikoEkintzak.Controls.Add(_lblOdooToken);
            pnlGoikoEkintzak.Controls.Add(_txtOdooToken);
            pnlGoikoEkintzak.Controls.Add(_btnEzarpenakGorde);

            this.Controls.Add(_dgv);
            this.Controls.Add(pnlGoikoEkintzak);
            this.Controls.Add(_footer);
            this.Controls.Add(_header);

            this.Shown += (_, __) =>
            {
                TPVEstiloaFinkoa.Prestatu(this);
                TPVEstiloaFinkoa.Aplikatu(this);
            };
        }

        private async void InbentarioaForm_Load(object sender, EventArgs e)
        {
            TPVEstiloa.EstilatuDataGridView(_dgv);
            TPVEstiloa.ProfesionalizatuKontrolak(this);
            KokatuBotoiak();
            KargatuOdooEzarpenak();
            await KargatuProduktuakAsync();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            KokatuBotoiak();
        }

        private void KokatuBotoiak()
        {
            int right = this.ClientSize.Width - 16;
            _btnGorde.Left = right - _btnGorde.Width;
            _btnGorde.Top = 10;

            _btnBerritu.Left = _btnGorde.Left - 12 - _btnBerritu.Width;
            _btnBerritu.Top = 10;

            _btnEzarpenakGorde.Left = right - _btnEzarpenakGorde.Width;
            _btnEzarpenakGorde.Top = 78;

            var maxWidth = Math.Max(300, _btnBerritu.Left - 32);
            _txtOdooUrl.Width = Math.Min(520, maxWidth);
            _txtOdooToken.Left = _txtOdooUrl.Right + 18;
            _lblOdooToken.Left = _txtOdooToken.Left;

            var tokenMax = Math.Max(220, _btnEzarpenakGorde.Left - _txtOdooToken.Left - 16);
            _txtOdooToken.Width = Math.Min(520, tokenMax);
        }

        private void KargatuOdooEzarpenak()
        {
            var ezarpenak = TpvEzarpenakZerbitzua.Kargatu();
            _txtOdooUrl.Text = string.IsNullOrWhiteSpace(ezarpenak.OdooBaseUrl) ? "http://192.168.10.5:8069" : ezarpenak.OdooBaseUrl;
            _txtOdooToken.Text = ezarpenak.OdooToken ?? string.Empty;
        }

        private void GordeOdooEzarpenak()
        {
            var url = (_txtOdooUrl.Text ?? string.Empty).Trim().TrimEnd('/');
            var token = (_txtOdooToken.Text ?? string.Empty).Trim();

            if (url.Length == 0)
            {
                MessageBox.Show("Sartu Odoo URL bat (adib.: http://192.168.10.5:8069).", "Ezarpenak",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || (uri.Scheme != "http" && uri.Scheme != "https"))
            {
                MessageBox.Show("Odoo URL ez da zuzena. Adib.: http://192.168.10.5:8069", "Ezarpenak",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ezarpenak = new TpvEzarpenak
            {
                OdooBaseUrl = url,
                OdooToken = token.Length == 0 ? null : token
            };

            var ok = TpvEzarpenakZerbitzua.Gorde(ezarpenak);
            if (!ok)
            {
                MessageBox.Show("Ezin izan da ezarpenak gorde.", "Ezarpenak",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Odoo ezarpenak gorde dira. Itxi eta berriz ireki pantaila deskontuak probatzeko.", "Ezarpenak",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task KargatuProduktuakAsync()
        {
            _produktuak = await _api.LortuProduktuakAsync();
            _stockHasierakoa.Clear();
            foreach (var p in _produktuak)
                _stockHasierakoa[p.Id] = p.Stock;

            _dgv.DataSource = null;
            _dgv.DataSource = _produktuak;
        }

        private async Task GordeAldaketakAsync()
        {
            _dgv.EndEdit();

            var aldatuak = _produktuak
                .Where(p => _stockHasierakoa.TryGetValue(p.Id, out var hasiera) && hasiera != p.Stock)
                .ToList();

            if (aldatuak.Count == 0)
            {
                MessageBox.Show("Ez dago aldaketarik gordetzeko.", "Inbentarioa",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var p in aldatuak)
            {
                if (p.Stock < 0)
                {
                    MessageBox.Show("Stock-a ezin da negatiboa izan.", "Errorea",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            bool guztiakOndo = true;
            foreach (var p in aldatuak)
            {
                bool ok = await _api.EguneratuProduktuaStockAsync(p.Id, p.Stock);
                if (!ok)
                {
                    guztiakOndo = false;
                    break;
                }
            }

            if (!guztiakOndo)
            {
                MessageBox.Show("Errorea stock-a eguneratzean.", "Errorea",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Inbentarioa ondo gorde da.", "Inbentarioa",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            await KargatuProduktuakAsync();
        }
    }
}

