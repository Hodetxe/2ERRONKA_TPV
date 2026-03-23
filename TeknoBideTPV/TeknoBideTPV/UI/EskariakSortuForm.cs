using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TeknoBideTPV.DTOak;
using TeknoBideTPV.Zerbitzuak;
using System.Drawing;
using TeknoBideTPV.UI.Styles;

namespace TeknoBideTPV.UI
{
    public partial class EskariakSortuForm : Form
    {
        private readonly ApiZerbitzua api = new ApiZerbitzua();
        private List<EskariaProduktuaSortuDto> produktuakEskarian = new();
        private List<ProduktuaDto> produktuak = new();
        private List<ErreserbaDto> erreserbak = new();

        private Button botoiAktiboa = null;

        private readonly Color KoloreNormala = TPVEstiloa.Koloreak.Primary;
        private readonly Color KoloreHover = TPVEstiloa.Koloreak.PrimaryDark;
        private readonly Color KoloreAktiboa = TPVEstiloa.Koloreak.TextTitle;

        private Form _AurrekoPantaila;
        private EskariaDto _eskariaEditatzeko;

        public EskariakSortuForm(Form AurrekoPantaila)
        {
            InitializeComponent();
            TPVEstiloaFinkoa.Prestatu(this);

            //minimizatu maximizatu eta itxi botoiak ezkutatu
            this.ControlBox = false;
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            _AurrekoPantaila = AurrekoPantaila;
            dgv_EskariaProduktua.CellClick += dgv_EskariaProduktua_CellClick;

            PrestatuFooter();
        }

        public EskariakSortuForm(Form AurrekoPantaila, EskariaDto eskaria) : this(AurrekoPantaila)
        {
            _eskariaEditatzeko = eskaria;
        }

        private async void EskariakSortuForm_Load(object sender, EventArgs e)
        {
            EstilatuKontrolak();
            erreserbak = await api.ErreserbakLortuAsync();
            cbo_Erreserba.DataSource = erreserbak;

            dgv_EskariaProduktua.EnableHeadersVisualStyles = false;
            dgv_EskariaProduktua.ColumnHeadersDefaultCellStyle.BackColor = TPVEstiloa.Koloreak.Primary;
            dgv_EskariaProduktua.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv_EskariaProduktua.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

            dgv_EskariaProduktua.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgv_EskariaProduktua.DefaultCellStyle.BackColor = Color.White;
            dgv_EskariaProduktua.DefaultCellStyle.ForeColor = Color.Black;
            dgv_EskariaProduktua.DefaultCellStyle.SelectionBackColor = TPVEstiloa.Koloreak.PrimaryDark;
            dgv_EskariaProduktua.DefaultCellStyle.SelectionForeColor = Color.White;

            dgv_EskariaProduktua.AlternatingRowsDefaultCellStyle.BackColor = TPVEstiloa.Koloreak.Background;
            dgv_EskariaProduktua.GridColor = Color.LightGray;
            dgv_EskariaProduktua.RowHeadersVisible = false;


            cbo_Erreserba.DisplayMember = "BezeroIzena";
            cbo_Erreserba.ValueMember = "Id";

            produktuak = await api.LortuProduktuakAsync();

            if (produktuak.Count == 0)
            {
                MessageBox.Show("Ez da produkturik aurkitu.");
                return;
            }

            SortuProduktuMotaBotoiak();
            ErakutsiProduktuak(produktuak);

            headerControl_EskariakSortu.Izena = "TXAPELA";
            headerControl_EskariakSortu.Titulo = "ESKARIA SORTU";
            headerControl_EskariakSortu.Erabiltzailea = SesioZerbitzua.Izena;
            headerControl_EskariakSortu.DataOrdua = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm");

            if (_eskariaEditatzeko != null)
            {
                headerControl_EskariakSortu.Titulo = "ESKARIA EDITATU";
                btn_SortuEskaria.Text = "GORDE";
                
                cbo_Erreserba.SelectedValue = _eskariaEditatzeko.ErreserbaId;
                cbo_Erreserba.Enabled = false;

                produktuakEskarian = _eskariaEditatzeko.Produktuak.Select(p => new EskariaProduktuaSortuDto
                {
                    ProduktuaId = p.ProduktuaId,
                    Kantitatea = p.Kantitatea,
                    Prezioa = p.Prezioa
                }).ToList();

                EskariakGridEguneratu();
                EguneratuBotoiak();
            }
        }

        private void PrestatuFooter()
        {
            footerControl_EskariakSortu.AtzeraTestua = "Atzera";

            footerControl_EskariakSortu.AtzeraClick += (s, e) =>
            {
                _AurrekoPantaila.Show();
                this.Close();
            };
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            TPVEstiloaFinkoa.Aplikatu(this);
            DistribuzioaAjustatu();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            DistribuzioaAjustatu();
        }

        private void DistribuzioaAjustatu()
        {
            if (cbo_Erreserba.Left < lbl_AukeratutakoErreserba.Right + 20)
            {
                cbo_Erreserba.Left = lbl_AukeratutakoErreserba.Right + 20;
            }

            int availableComboWidth = this.ClientSize.Width - cbo_Erreserba.Left - 50;
            int desiredComboWidth = Math.Max(cbo_Erreserba.Width, Math.Min(availableComboWidth, 400));
            cbo_Erreserba.Width = desiredComboWidth;
            cbo_Erreserba.DropDownWidth = Math.Max(cbo_Erreserba.DropDownWidth, desiredComboWidth + 100);

            int rightMargin = 50;
            int newWidth = this.ClientSize.Width - dgv_EskariaProduktua.Left - rightMargin;
            if (newWidth > dgv_EskariaProduktua.Width)
            {
                dgv_EskariaProduktua.Width = newWidth;
            }

            int bottomLimit = (footerControl_EskariakSortu != null) ? footerControl_EskariakSortu.Top : (this.ClientSize.Height - 100);
            
            btn_SortuEskaria.Top = bottomLimit - btn_SortuEskaria.Height - 30;
            btn_SortuEskaria.Left = this.ClientSize.Width - btn_SortuEskaria.Width - rightMargin;

            txt_PrezioTotala.Top = btn_SortuEskaria.Top - txt_PrezioTotala.Height - 20;
            lbl_PrezioTotala.Top = txt_PrezioTotala.Top; 

            txt_PrezioTotala.Left = btn_SortuEskaria.Right - txt_PrezioTotala.Width;
            lbl_PrezioTotala.Left = txt_PrezioTotala.Left - lbl_PrezioTotala.Width - 20;

            int availableHeight = txt_PrezioTotala.Top - dgv_EskariaProduktua.Top - 20;
            if (availableHeight > dgv_EskariaProduktua.Height)
            {
                dgv_EskariaProduktua.Height = availableHeight;
            }
        }

        private void EstilatuKontrolak()
        {
            this.BackColor = TPVEstiloa.Koloreak.Background;
            flp_Produktuak.BackColor = TPVEstiloa.Koloreak.Background;
            flp_ProduktuMotak.BackColor = TPVEstiloa.Koloreak.Background;
            pnl_ProduktuMotak.BackColor = TPVEstiloa.Koloreak.Background;

            lbl_AukeratutakoErreserba.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            lbl_PrezioTotala.ForeColor = TPVEstiloa.Koloreak.TextTitle;

            btn_SortuEskaria.BackColor = TPVEstiloa.Koloreak.Primary;
            btn_SortuEskaria.ForeColor = Color.White;
            btn_SortuEskaria.FlatStyle = FlatStyle.Flat;
            btn_SortuEskaria.FlatAppearance.BorderSize = 0;

            cbo_Erreserba.BackColor = Color.White;
            txt_PrezioTotala.BackColor = Color.White;
        }

        private Button SortuTpvBotoia(string testua)
        {
            var btn = new Button
            {
                Text = testua,
                AutoSize = true,
                BackColor = KoloreNormala,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Padding = new Padding(25, 15, 25, 15),
                Margin = new Padding(8)
            };

            btn.FlatAppearance.BorderSize = 0;

            btn.MouseEnter += (s, e) =>
            {
                if (btn != botoiAktiboa)
                    btn.BackColor = KoloreHover;
            };

            btn.MouseLeave += (s, e) =>
            {
                if (btn != botoiAktiboa)
                    btn.BackColor = KoloreNormala;
            };

            TPVEstiloaFinkoa.EguneratuKontrola(btn, this);

            return btn;
        }

        private void SortuProduktuMotaBotoiak()
        {
            flp_ProduktuMotak.Controls.Clear();

            var motak = produktuak
                .Select(p => p.Mota?.Trim().ToLower())
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Distinct()
                .OrderBy(m => m)
                .ToList();

            if (motak.Count == 0)
            {
                MessageBox.Show("Ez da produktu motarik aurkitu.");
                return;
            }

            var btnGuztiak = SortuTpvBotoia("Guztiak");

            btnGuztiak.Click += (s, e) =>
            {
                if (botoiAktiboa != null)
                    botoiAktiboa.BackColor = KoloreNormala;

                btnGuztiak.BackColor = KoloreAktiboa;
                botoiAktiboa = btnGuztiak;

                ErakutsiProduktuak(produktuak);
            };

            flp_ProduktuMotak.Controls.Add(btnGuztiak);

            foreach (var mota in motak)
            {
                var btn = SortuTpvBotoia(char.ToUpper(mota[0]) + mota.Substring(1));

                btn.Click += (s, e) =>
                {
                    if (botoiAktiboa != null)
                        botoiAktiboa.BackColor = KoloreNormala;

                    btn.BackColor = KoloreAktiboa;
                    botoiAktiboa = btn;

                    var filtratuak = produktuak
                        .Where(p => p.Mota.Equals(mota, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    ErakutsiProduktuak(filtratuak);
                };

                flp_ProduktuMotak.Controls.Add(btn);
            }

            btnGuztiak.PerformClick();
        }

        private void ErakutsiProduktuak(List<ProduktuaDto> zerrenda)
        {
            flp_Produktuak.Controls.Clear();

            foreach (var p in zerrenda)
            {
                var btn = new Button
                {
                    Text = $"{p.Izena}\n{p.Prezioa:0.00}€",
                    Tag = p,
                    Width = 150,
                    Height = 80,
                    BackColor = TPVEstiloa.Koloreak.Baieztatu,
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Margin = new Padding(5)
                };

                btn.Click += ProduktuaGehitu_Click;
                flp_Produktuak.Controls.Add(btn);
                TPVEstiloaFinkoa.EguneratuKontrola(btn, this);
            }
            EguneratuBotoiak();
        }

        private void EguneratuBotoiak()
        {
            foreach (Control c in flp_Produktuak.Controls)
            {
                if (c is Button btn && btn.Tag is ProduktuaDto p)
                {
                    var inCart = produktuakEskarian.FirstOrDefault(x => x.ProduktuaId == p.Id);
                    int quantityInCart = inCart?.Kantitatea ?? 0;

                    if (p.Stock <= quantityInCart)
                    {
                        btn.Enabled = false;
                        btn.BackColor = Color.Gray;
                    }
                    else
                    {
                        btn.Enabled = true;
                        btn.BackColor = TPVEstiloa.Koloreak.Baieztatu;
                    }
                }
            }
        }

        private void ProduktuaGehitu_Click(object sender, EventArgs e)
        {
            var produktua = (ProduktuaDto)((Button)sender).Tag;
            var lehendik = produktuakEskarian.FirstOrDefault(x => x.ProduktuaId == produktua.Id);

            if (lehendik != null)
            {
                lehendik.Kantitatea++;
                lehendik.Prezioa = lehendik.Kantitatea * produktua.Prezioa;
            }
            else
            {
                produktuakEskarian.Add(new EskariaProduktuaSortuDto
                {
                    ProduktuaId = produktua.Id,
                    Kantitatea = 1,
                    Prezioa = produktua.Prezioa
                });
            }

            EskariakGridEguneratu();
            EguneratuBotoiak();
        }

        private void EskariakGridEguneratu()
        {
            dgv_EskariaProduktua.Columns.Clear();

            var data = produktuakEskarian.Select(p => new
            {
                ProduktuaIzena = produktuak.FirstOrDefault(x => x.Id == p.ProduktuaId)?.Izena ?? "Desconocido",
                Kantitatea = p.Kantitatea,
                ProduktuakPrezioaBakarka = p.Prezioa / p.Kantitatea,
                ProduktuakPrezioaGuztira = p.Prezioa
            }).ToList();

            dgv_EskariaProduktua.AutoGenerateColumns = false;
            dgv_EskariaProduktua.DataSource = data;
            dgv_EskariaProduktua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv_EskariaProduktua.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProduktuaIzena",
                HeaderText = "Produktua",
                Name = "ProduktuaIzena"
            });

            dgv_EskariaProduktua.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Kantitatea",
                HeaderText = "Kantitatea",
                Name = "Kantitatea"
            });

            dgv_EskariaProduktua.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProduktuakPrezioaBakarka",
                HeaderText = "Prezioa (€)",
                Name = "ProduktuakPrezioaBakarka"
            });

            dgv_EskariaProduktua.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProduktuakPrezioaGuztira",
                HeaderText = "Guztira (€)",
                Name = "ProduktuakPrezioaGuztira"
            });

            dgv_EskariaProduktua.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "btn_ProduktuaKendu",
                HeaderText = "Produktua ezabatu",
                Text = "Ezabatu",
                UseColumnTextForButtonValue = true
            });

            txt_PrezioTotala.Text = produktuakEskarian.Sum(p => p.Prezioa).ToString("0.00");
        }

        private void dgv_EskariaProduktua_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgv_EskariaProduktua.Columns[e.ColumnIndex].Name == "btn_ProduktuaKendu")
            {
                var produktuaIzena = dgv_EskariaProduktua.Rows[e.RowIndex].Cells["ProduktuaIzena"].Value.ToString();
                var produktuaEskarian = produktuakEskarian.FirstOrDefault(p =>
                    produktuak.FirstOrDefault(x => x.Id == p.ProduktuaId)?.Izena == produktuaIzena);

                if (produktuaEskarian != null)
                {
                    if (produktuaEskarian.Kantitatea > 1)
                    {
                        produktuaEskarian.Kantitatea--;
                        produktuaEskarian.Prezioa = produktuaEskarian.Kantitatea *
                            produktuak.First(x => x.Id == produktuaEskarian.ProduktuaId).Prezioa;
                    }
                    else
                    {
                        produktuakEskarian.Remove(produktuaEskarian);
                    }
                }

                EskariakGridEguneratu();
                EguneratuBotoiak();
            }
        }

        private async void btn_SortuEskaria_Click(object sender, EventArgs e)
        {
            if (cbo_Erreserba.SelectedItem == null || produktuakEskarian.Count == 0)
            {
                MessageBox.Show("Aukeratu erreserba eta produktuak lehenik.");
                return;
            }

            var eskaria = new EskariaSortuDto
            {
                ErreserbaId = ((ErreserbaDto)cbo_Erreserba.SelectedItem).Id,
                Prezioa = double.Parse(txt_PrezioTotala.Text),
                Egoera = _eskariaEditatzeko != null ? _eskariaEditatzeko.Egoera : "Bidalita",
                Produktuak = produktuakEskarian
            };

            if (_eskariaEditatzeko != null)
            {
                // EDITATU
                var ondo = await api.EguneratuEskariaAsync(_eskariaEditatzeko.Id, eskaria);
                if (ondo)
                {
                    MessageBox.Show("Eskaria ondo eguneratu da!", "Eskaria Eguneratuta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (_AurrekoPantaila is EskariakForm eskariakForm)
                    {
                        eskariakForm.BehartuEguneraketa();
                    }
                    _AurrekoPantaila.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Errorea eskaria eguneratzean.");
                }
            }
            else
            {
                // SORTU
                var emaitza = await api.SortuEskariaAsync(eskaria);

                if (emaitza != null)
                {
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine($"Eskaria sortuta!");
                    sb.AppendLine($"Prezio Totala: {emaitza.PrezioaTotala:0.00} €");
                    sb.AppendLine("Produktuak:");

                    foreach (var p in emaitza.Produktuak)
                    {
                        sb.AppendLine($"{p.ProduktuaIzena} - {p.Kantitatea} x {p.ProduktuakPrezioaBakarka:0.00}€ = {p.ProduktuakPrezioaGuztira:0.00}€");
                    }

                    MessageBox.Show(sb.ToString(), "Eskaria sortuta", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    produktuakEskarian.Clear();
                    if (produktuakEskarian.Count > 0)
                        EskariakGridEguneratu();
                    else
                        dgv_EskariaProduktua.DataSource = null;

                    EguneratuBotoiak();
                }
                else
                {
                    MessageBox.Show("Errorea eskaria sortzean.");
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
