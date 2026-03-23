using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TeknoBideTPV.DTOak;
using TeknoBideTPV.UI.Styles;
using TeknoBideTPV.Zerbitzuak;
using System.Linq;

namespace TeknoBideTPV.UI
{
    public partial class EskariakForm : Form
    {
        private readonly ApiZerbitzua _api = new ApiZerbitzua();
        private Form _AurrekoPantaila;

        private Panel overlayPanel;

        private List<EskariaDto> _eskariakGuztiak = new List<EskariaDto>();
        private string _filtroa = "Guztiak";
        private bool _beharDituEguneraketa = false;
        private int _currentCardWidth = 400;

        public EskariakForm(Form AurrekoPantaila)
        {
            InitializeComponent();

            this.ControlBox = false;
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            _AurrekoPantaila = AurrekoPantaila;

            DoubleBufferingAktibatu(flp_Eskariak);

            this.Shown += EskariakForm_Shown;
            this.Activated += EskariakForm_Activated;
            this.Resize += (s, e) => BirarraztuTxartelak();

            PrestatuFooter();
            PrestatuFiltroak();
        }

        private void PrestatuFiltroak()
        {
            btn_Guztiak.Click += (s, e) => EzarriFiltroa("Guztiak", btn_Guztiak);
            bnt_Bidaliak.Click += (s, e) => EzarriFiltroa("Bidalita", bnt_Bidaliak);
            btn_Prest.Click += (s, e) => EzarriFiltroa("Prest", btn_Prest);
            btn_Zerbitzatuak.Click += (s, e) => EzarriFiltroa("Zerbitzatuta", btn_Zerbitzatuak);
        }

        private void EzarriFiltroa(string filtroa, Button botoia)
        {
            _filtroa = filtroa;

            Button[] filtroBotoiak = { btn_Guztiak, bnt_Bidaliak, btn_Prest, btn_Zerbitzatuak };
            foreach (var btn in filtroBotoiak)
            {
                btn.BackColor = TPVEstiloa.Koloreak.Primary;
            }
            botoia.BackColor = TPVEstiloa.Koloreak.PrimaryDark;

            ErakutsiEskariak();
        }

        private async void EskariakForm_Activated(object sender, EventArgs e)
        {
             if (_eskariakGuztiak == null || _eskariakGuztiak.Count == 0 || _beharDituEguneraketa)
             {
                 var eskariak = await _api.LortuEskariakAsync();
                 KargatuEskariak(eskariak);
                 _beharDituEguneraketa = false;
             }
        }

        public void BehartuEguneraketa()
        {
            _beharDituEguneraketa = true;
        }

        private void PrestatuFooter()
        {
            footerControl_Eskariak.AtzeraTestua = "Atzera";

            footerControl_Eskariak.AtzeraClick += (s, e) =>
            {
                _AurrekoPantaila.Show();
                this.Close();
            };
        }

        private void DoubleBufferingAktibatu(Control kontrola)
        {
            typeof(Control).InvokeMember(
                "DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.NonPublic,
                null,
                kontrola,
                new object[] { true }
            );
        }

        private async void EskariakForm_Shown(object sender, EventArgs e)
        {
            TPVEstiloaFinkoa.Prestatu(this);
            TPVEstiloaFinkoa.Aplikatu(this);
            EstilatuKontrolak();

            overlayPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = this.BackColor,
                Visible = true
            };

            this.Controls.Add(overlayPanel);
            overlayPanel.BringToFront();

            EzarriEskariakLayout();

            var eskariak = await _api.LortuEskariakAsync();
            KargatuEskariak(eskariak);

            headerControl_Eskariak.Izena = "TXAPELA";
            headerControl_Eskariak.Titulo = "ESKARIAK";
            headerControl_Eskariak.Erabiltzailea = SesioZerbitzua.Izena;
            headerControl_Eskariak.DataOrdua = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm");

            overlayPanel.Visible = false;
            this.Controls.Remove(overlayPanel);
            overlayPanel.Dispose();

            BirarraztuTxartelak();
        }

        private void EstilatuKontrolak()
        {
            this.BackColor = TPVEstiloa.Koloreak.Background;
            pnl_EdukiaEskariak.BackColor = TPVEstiloa.Koloreak.Background;
            flp_Filtroak.BackColor = TPVEstiloa.Koloreak.Background;
            flp_Eskariak.BackColor = TPVEstiloa.Koloreak.Background;

            Button[] filtroBotoiak = { btn_Guztiak, bnt_Bidaliak, btn_Prest, btn_Zerbitzatuak };
            foreach (var btn in filtroBotoiak)
            {
                btn.BackColor = TPVEstiloa.Koloreak.Primary;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            }
            btn_Guztiak.BackColor = TPVEstiloa.Koloreak.PrimaryDark;
        }

        private void EzarriEskariakLayout()
        {
            flp_Eskariak.Dock = DockStyle.Fill;
            flp_Eskariak.FlowDirection = FlowDirection.LeftToRight;
            flp_Eskariak.WrapContents = true;
            flp_Eskariak.AutoScroll = true;
            flp_Eskariak.Padding = new Padding(10);
        }

        private void KargatuEskariak(List<EskariaDto> eskariak)
        {
            _eskariakGuztiak = eskariak;
            ErakutsiEskariak();
        }

        private void ErakutsiEskariak()
        {
            flp_Eskariak.SuspendLayout();
            flp_Eskariak.Controls.Clear();

            var erakustekoak = _eskariakGuztiak;
            if (_filtroa != "Guztiak")
            {
                erakustekoak = _eskariakGuztiak.Where(e => e.Egoera == _filtroa).ToList();
            }

            foreach (var eskaria in erakustekoak)
            {
                Panel panel = SortuEskariaPanel(eskaria);
                flp_Eskariak.Controls.Add(panel);
            }

            flp_Eskariak.ResumeLayout();
            flp_Eskariak.PerformLayout();
        }

        private Panel SortuEskariaPanel(EskariaDto eskaria)
        {
            Panel pnl_Eskaria = new Panel
            {
                Width = 400,
                Height = 280,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(8),
                Padding = new Padding(10)
            };

            Label lbl_Mahaia = new Label
            {
                Text = $"Mahai: {eskaria.MahaiaZenbakia}",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(300, 25),
                ForeColor = TPVEstiloa.Koloreak.TextTitle
            };

            Label lbl_Egoera = new Label
            {
                Text = $"Egoera: {eskaria.Egoera}",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, 40),
                Size = new Size(200, 25),
                ForeColor = TPVEstiloa.Koloreak.TextTitle
            };

            FlowLayoutPanel flp_Produktuak = new FlowLayoutPanel
            {
                Location = new Point(10, 80),
                Size = new Size(380, 110),
                MaximumSize = new Size(380, 9999),
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false
            };

            foreach (var p in eskaria.Produktuak)
            {
                Label lblProd = new Label
                {
                    Text = $"{p.ProduktuaIzena}   x{p.Kantitatea}   {p.Prezioa:0.00} €",
                    Font = new Font("Segoe UI", 10),
                    Size = new Size(340, 25),
                    ForeColor = TPVEstiloa.Koloreak.TextTitle
                };

                flp_Produktuak.Controls.Add(lblProd);
            }

            Label lbl_Prezioa = new Label
            {
                Text = $"Prezioa: {eskaria.Prezioa:0.00} €",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(180, 190),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.TopRight,
                ForeColor = TPVEstiloa.Koloreak.TextTitle
            };

            int botoiZabalera = 110;
            int botoiAltuera = 30;
            int espazioa = 20;
            int totalZabalera = (botoiZabalera * 3) + (espazioa * 2);
            int xHasiera = (pnl_Eskaria.Width - totalZabalera) / 2;
            int yPos = 225;

            Button btn_Zerbitzatu = new Button
            {
                Text = "ZERBITZATU",
                Size = new Size(botoiZabalera, botoiAltuera),
                Location = new Point(xHasiera, yPos),
                BackColor = TPVEstiloa.Koloreak.Baieztatu,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btn_Zerbitzatu.FlatAppearance.BorderSize = 0;
            btn_Zerbitzatu.Click += async (s, e) =>
            {
                var dto = new EskariaSortuDto
                {
                    ErreserbaId = eskaria.ErreserbaId,
                    Prezioa = eskaria.Prezioa,
                    Egoera = "Zerbitzatuta",
                    Produktuak = eskaria.Produktuak.Select(p => new EskariaProduktuaSortuDto
                    {
                        ProduktuaId = p.ProduktuaId,
                        Kantitatea = p.Kantitatea,
                        Prezioa = p.Prezioa
                    }).ToList()
                };

                bool ondo = await _api.EguneratuEskariaAsync(eskaria.Id, dto);
                if (ondo)
                {
                    var eskariak = await _api.LortuEskariakAsync();
                    KargatuEskariak(eskariak);
                }
                else
                {
                    MessageBox.Show("Errorea egoera aldatzean.");
                }
            };

            Button btn_Ezabatu = new Button
            {
                Text = "EZABATU",
                Size = new Size(botoiZabalera, botoiAltuera),
                Location = new Point(xHasiera + botoiZabalera + espazioa, yPos),
                BackColor = TPVEstiloa.Koloreak.Error,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btn_Ezabatu.FlatAppearance.BorderSize = 0;
            btn_Ezabatu.Click += async (s, e) =>
            {
                var result = MessageBox.Show("Ziur zaude eskaria ezabatu nahi duzula?", "Ezabatu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    bool ondo = await _api.EzabatuEskariaAsync(eskaria.Id);
                    if (ondo)
                    {
                        var eskariak = await _api.LortuEskariakAsync();
                        KargatuEskariak(eskariak);
                    }
                    else
                    {
                        MessageBox.Show("Errorea ezabatzean.");
                    }
                }
            };

            Button btn_Editatu = new Button
            {
                Text = "EDITATU",
                Size = new Size(botoiZabalera, botoiAltuera),
                Location = new Point(xHasiera + (botoiZabalera + espazioa) * 2, yPos),
                BackColor = TPVEstiloa.Koloreak.Secondary,
                ForeColor = TPVEstiloa.Koloreak.TextTitle,
                FlatStyle = FlatStyle.Flat
            };
            btn_Editatu.FlatAppearance.BorderSize = 0;
            btn_Editatu.Click += (s, e) =>
            {
                var editForm = new EskariakSortuForm(this, eskaria);
                editForm.Show();
                this.Hide();
            };

            pnl_Eskaria.Controls.Add(lbl_Mahaia);
            pnl_Eskaria.Controls.Add(lbl_Egoera);
            pnl_Eskaria.Controls.Add(flp_Produktuak);
            pnl_Eskaria.Controls.Add(lbl_Prezioa);
            pnl_Eskaria.Controls.Add(btn_Zerbitzatu);
            pnl_Eskaria.Controls.Add(btn_Ezabatu);
            pnl_Eskaria.Controls.Add(btn_Editatu);

            EguneratuPanelDiseinua(pnl_Eskaria, _currentCardWidth);

            return pnl_Eskaria;
        }

        private void BirarraztuTxartelak()
        {
            if (flp_Eskariak.ClientSize.Width == 0) return;

            int availableWidth = flp_Eskariak.ClientSize.Width - flp_Eskariak.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth;

            int columns = 3;
            int candidateWidth = (availableWidth / columns) - 16; 

            if (candidateWidth < 380)
            {
                columns = 2;
                candidateWidth = (availableWidth / columns) - 16;
            }

            _currentCardWidth = candidateWidth;

            flp_Eskariak.SuspendLayout();
            foreach (Control c in flp_Eskariak.Controls)
            {
                if (c is Panel p)
                {
                    EguneratuPanelDiseinua(p, _currentCardWidth);
                }
            }
            flp_Eskariak.ResumeLayout(true);
        }

        private void EguneratuPanelDiseinua(Panel p, int width)
        {
            p.Width = width;

            var flp_Prod = p.Controls.OfType<FlowLayoutPanel>().FirstOrDefault();
            var lbl_Prezioa = p.Controls.OfType<Label>().FirstOrDefault(l => l.Text.StartsWith("Prezioa:"));
            var btns = p.Controls.OfType<Button>().ToList();

            if (flp_Prod != null)
            {
                flp_Prod.Width = width - 20; 
                foreach (Control item in flp_Prod.Controls)
                {
                    if (item is Label l) l.Width = flp_Prod.Width - 40;
                }
            }

            if (lbl_Prezioa != null)
            {
                lbl_Prezioa.Location = new Point(width - lbl_Prezioa.Width - 20, 190);
            }

            if (btns.Count == 3)
            {
                var btn_Zerbitzatu = btns.FirstOrDefault(b => b.Text == "ZERBITZATU");
                var btn_Ezabatu = btns.FirstOrDefault(b => b.Text == "EZABATU");
                var btn_Editatu = btns.FirstOrDefault(b => b.Text == "EDITATU");

                if (btn_Zerbitzatu != null && btn_Ezabatu != null && btn_Editatu != null)
                {
                    int botoiZabalera = 110;
                    int espazioa = 20;
                    int totalZabalera = (botoiZabalera * 3) + (espazioa * 2);
                    int xHasiera = (width - totalZabalera) / 2;
                    int yPos = 225;

                    btn_Zerbitzatu.Location = new Point(xHasiera, yPos);
                    btn_Ezabatu.Location = new Point(xHasiera + botoiZabalera + espazioa, yPos);
                    btn_Editatu.Location = new Point(xHasiera + (botoiZabalera + espazioa) * 2, yPos);
                }
            }
        }

        private void btn_Atzera_Click(object sender, EventArgs e)
        {
            _AurrekoPantaila.Show();
            this.Close();
        }
    }
}
