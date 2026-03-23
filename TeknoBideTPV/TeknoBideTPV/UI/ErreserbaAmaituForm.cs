using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeknoBideTPV.DTOak;
using TeknoBideTPV.Zerbitzuak;
using TeknoBideTPV.UI.Controls;
using TeknoBideTPV.UI.Styles;

namespace TeknoBideTPV.UI
{
    public partial class ErreserbaAmaituForm : Form
    {
        private Form _AurrekoPantaila;
        private readonly ApiZerbitzua _api = new ApiZerbitzua();
        private int _langileaId;

        private ErreserbaDto? _hautatutakoErreserba;
        private double _guztira;
        private bool _eskudiruaAukeratuta;

        public ErreserbaAmaituForm(Form AurrekoPantaila)
        {
            InitializeComponent();
            TPVEstiloa.PantailarenEskalatuaHasi(this);

            dgv_Eskariak.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv_Eskariak.RowTemplate.Height = 60;
            dgv_Eskariak.Font = new Font("Segoe UI", 22);
            dgv_Eskariak.DefaultCellStyle.Font = new Font("Segoe UI", 22);
            dgv_Eskariak.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            dgv_Eskariak.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_Eskariak.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_Eskariak.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv_Eskariak.MultiSelect = false;
            dgv_Eskariak.ReadOnly = true;

            lbl_Guztira.AutoSize = false;
            lbl_Guztira.Size = new Size(650, 80);
            lbl_Guztira.TextAlign = ContentAlignment.MiddleRight;

            lbl_Itzulia.AutoSize = false;
            lbl_Itzulia.Size = new Size(300, 60);
            lbl_Itzulia.TextAlign = ContentAlignment.MiddleRight;

            this.ControlBox = false;
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            _AurrekoPantaila = AurrekoPantaila;
            _langileaId = SesioZerbitzua.LangileaId;

            this.Load += ErreserbaAmaituForm_Load;

            cmb_Erreserbak.SelectedIndexChanged += cmb_Erreserbak_SelectedIndexChanged;
            btn_Eskudirua.Click += btn_Eskudirua_Click;
            btn_Txartela.Click += btn_Txartela_Click;
            txt_JasoDenDirua.TextChanged += txt_JasoDenDirua_TextChanged;
            btn_Ordaindu.Click += btn_Ordaindu_Click;

            txt_JasoDenDirua.Enabled = false;
            lbl_Itzulia.Text = "0,00 €";
            lbl_Guztira.Text = "0,00 €";

            PrestatuFooter();
        }

        private async void ErreserbaAmaituForm_Load(object sender, EventArgs e)
        {
            EstilatuKontrolak();

            headerControl_ErreserbaAmaitu.Izena = "TXAPELA";
            headerControl_ErreserbaAmaitu.Titulo = "ERRESERBA AMAITU";
            headerControl_ErreserbaAmaitu.Erabiltzailea = SesioZerbitzua.Izena;
            headerControl_ErreserbaAmaitu.DataOrdua = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm");

            await KargatuErreserbakAsync();
        }

        private void PrestatuFooter()
        {
            footerControl_ErreserbaAmaitu.AtzeraTestua = "Atzera";

            footerControl_ErreserbaAmaitu.AtzeraClick += (s, e) =>
            {
                _AurrekoPantaila.Show();
                this.Close();
            };
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            TPVEstiloa.EskalatuaAplikatu(this);
            ZabaleraAjustatu();
            
            dgv_Eskariak.DefaultCellStyle.Font = new Font("Segoe UI", 22);
            dgv_Eskariak.RowTemplate.Height = 60; 
            foreach (DataGridViewRow row in dgv_Eskariak.Rows)
            {
                row.Height = 60;
            }
        }
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ZabaleraAjustatu();
        }
        
        private void ZabaleraAjustatu()
        {
            int gap = 40;
            int margin = 40;

            // Calcular espacio vertical disponible entre Header y Footer
            int topPos = margin;
            int bottomLimit = this.ClientSize.Height - margin;

            // Intentar obtener límites de header y footer si existen
            if (headerControl_ErreserbaAmaitu != null)
                topPos = headerControl_ErreserbaAmaitu.Bottom + 10; // Pequeño margen extra
            
            if (footerControl_ErreserbaAmaitu != null)
                bottomLimit = footerControl_ErreserbaAmaitu.Top - 10;

            int availableHeight = bottomLimit - topPos;
            if (availableHeight < 0) availableHeight = 0;

            int totalAvailableWidth = this.ClientSize.Width - (2 * margin) - gap;

            if (totalAvailableWidth > 0)
            {
                // Aumentamos el panel de pago al 50%
                int eskariakWidth = (int)(totalAvailableWidth * 0.50);
                int ordainketaWidth = totalAvailableWidth - eskariakWidth;

                pnl_Eskariak.Left = margin;
                pnl_Eskariak.Top = topPos;
                pnl_Eskariak.Width = eskariakWidth;
                pnl_Eskariak.Height = availableHeight;
                
                dgv_Eskariak.Width = pnl_Eskariak.Width;
                dgv_Eskariak.Height = pnl_Eskariak.Height;

                pnl_Ordainketa.Left = pnl_Eskariak.Right + gap;
                pnl_Ordainketa.Top = topPos;
                pnl_Ordainketa.Width = ordainketaWidth;
                pnl_Ordainketa.Height = availableHeight; 

                // Ajustar controles dentro de pnl_Ordainketa
                AjustarKontrolakOrdainketa(ordainketaWidth, pnl_Ordainketa.Height);
            }

            // Ajustar label Guztira (existente)
            int margenLbl = 20;
            lbl_Guztira.AutoSize = false;
            int availableLbl = pnl_Ordainketa.ClientSize.Width - lbl_Guztira.Left - margenLbl;
            if (availableLbl > 0)
            {
                lbl_Guztira.Width = availableLbl;
            }
        }

        private void AjustarKontrolakOrdainketa(int panelWidth, int panelHeight)
        {
            int innerMargin = 8;
            int fullWidth = panelWidth - (2 * innerMargin);

            // Reducir alturas para asegurar que caben en 700px y permitir mayor separación
            int btnHeight = 70;      
            int btnPayHeight = 80;   
            int lblTotalHeight = 60; 
            int txtHeight = 50;      
            
            if (btn_Eskudirua != null) btn_Eskudirua.Height = btnHeight;
            if (btn_Txartela != null) btn_Txartela.Height = btnHeight;
            if (btn_Ordaindu != null) btn_Ordaindu.Height = btnPayHeight;
            if (lbl_Guztira != null) lbl_Guztira.Height = lblTotalHeight;
            if (lbl_Itzulia != null) lbl_Itzulia.Height = lblTotalHeight; // Usamos la misma altura que Guztira para consistencia
            if (txt_JasoDenDirua != null) txt_JasoDenDirua.Height = txtHeight;
            if (cmb_Erreserbak != null) cmb_Erreserbak.Height = 45; // Mantener altura combo

            if (cmb_Erreserbak != null) cmb_Erreserbak.Width = fullWidth;

            if (btn_Eskudirua != null && btn_Txartela != null)
            {
                int buttonGap = 20;
                int halfWidth = (fullWidth - buttonGap) / 2;
                btn_Eskudirua.Width = halfWidth;
                btn_Txartela.Width = halfWidth;
                btn_Txartela.Left = btn_Eskudirua.Right + buttonGap;
            }

            if (txt_JasoDenDirua != null)
                txt_JasoDenDirua.Width = (int)(fullWidth * 0.5);

            if (btn_Ordaindu != null)
            {
                int ordainduWidth = (int)(fullWidth * 0.6);
                btn_Ordaindu.Width = ordainduWidth;
                btn_Ordaindu.Left = innerMargin + ((fullWidth - ordainduWidth) / 2);
            }

            // Distribución Vertical
            // Definimos grupos de controles y su altura aproximada
            // Grupo 1: Seleccion
            int h1 = lbl_ErreserbaAukeratu.Height + cmb_Erreserbak.Height;
            // Grupo 2: Total
            int h2 = lbl_GuztiraIzenburua.Height + lbl_Guztira.Height;
            // Grupo 3: Metodo
            int h3 = lbl_OrdainketaMetodoa.Height + btn_Eskudirua.Height;
            // Grupo 4: Recibido
            int h4 = lbl_JasoDenDirua.Height + txt_JasoDenDirua.Height;
            // Grupo 5: Vuelta
            int h5 = lbl_ItzuliIzenburua.Height + lbl_Itzulia.Height;
            // Grupo 6: Boton Pagar
            int h6 = btn_Ordaindu.Height;

            int totalContentHeight = h1 + h2 + h3 + h4 + h5 + h6;
            int availableSpace = panelHeight - totalContentHeight;
            
            // Distribuimos el espacio disponible entre los huecos (7 huecos: antes del 1, entre grupos, despues del 6)
            int gapCount = 5; 
            // Permitir que el gap sea pequeño si hace falta, pero intentar mantener al menos 4px
            int gapSize = 6;
            
            if (availableSpace > 0)
            {
                gapSize = availableSpace / (gapCount + 2);
                // Limitar el gap máximo para que no queden muy separados si sobra mucho espacio
                if (gapSize > 20) gapSize = 20; 
                if (gapSize < 4) gapSize = 4;
            }
            else
            {
                // Si no hay espacio, usar un gap mínimo y permitir desbordamiento o scroll (si lo hubiera)
                gapSize = 2;
            }

            int currentY = gapSize;

            // Grupo 1
            lbl_ErreserbaAukeratu.Top = currentY;
            cmb_Erreserbak.Top = lbl_ErreserbaAukeratu.Bottom; 
            currentY = cmb_Erreserbak.Bottom + gapSize;

            // Grupo 2
            lbl_GuztiraIzenburua.Top = currentY;
            lbl_Guztira.Top = lbl_GuztiraIzenburua.Bottom;
            currentY = lbl_Guztira.Bottom + gapSize;

            // Grupo 3
            lbl_OrdainketaMetodoa.Top = currentY;
            btn_Eskudirua.Top = lbl_OrdainketaMetodoa.Bottom;
            btn_Txartela.Top = btn_Eskudirua.Top;
            currentY = btn_Eskudirua.Bottom + gapSize;

            // Grupo 4
            lbl_JasoDenDirua.Top = currentY;
            txt_JasoDenDirua.Top = lbl_JasoDenDirua.Bottom;
            currentY = txt_JasoDenDirua.Bottom + gapSize;

            // Grupo 5
            lbl_ItzuliIzenburua.Top = currentY;
            lbl_Itzulia.Top = lbl_ItzuliIzenburua.Bottom;
            currentY = lbl_Itzulia.Bottom + gapSize;

            // Grupo 6
            btn_Ordaindu.Top = currentY;
        }

        private void EstilatuKontrolak()
        {
            this.BackColor = TPVEstiloa.Koloreak.Background;
            pnl_Eskariak.BackColor = TPVEstiloa.Koloreak.Background;
            pnl_Ordainketa.BackColor = TPVEstiloa.Koloreak.Background;

            Label[] labels =
            {
                lbl_ErreserbaAukeratu,
                lbl_GuztiraIzenburua,
                lbl_Guztira,
                lbl_OrdainketaMetodoa,
                lbl_JasoDenDirua,
                lbl_ItzuliIzenburua,
                lbl_Itzulia
            };

            foreach (var lbl in labels)
            {
                lbl.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            }

            txt_JasoDenDirua.BackColor = Color.White;
            cmb_Erreserbak.BackColor = Color.White;

            dgv_Eskariak.BackgroundColor = TPVEstiloa.Koloreak.Background;
            dgv_Eskariak.BorderStyle = BorderStyle.None;
            dgv_Eskariak.ColumnHeadersDefaultCellStyle.BackColor = TPVEstiloa.Koloreak.Primary;
            dgv_Eskariak.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv_Eskariak.EnableHeadersVisualStyles = false;
            dgv_Eskariak.DefaultCellStyle.BackColor = Color.White;
            dgv_Eskariak.DefaultCellStyle.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            dgv_Eskariak.DefaultCellStyle.SelectionBackColor = TPVEstiloa.Koloreak.Secondary;
            dgv_Eskariak.DefaultCellStyle.SelectionForeColor = TPVEstiloa.Koloreak.TextTitle;

            Button[] botoiak = { btn_Eskudirua, btn_Txartela, btn_Ordaindu };
            foreach (var btn in botoiak)
            {
                btn.BackColor = TPVEstiloa.Koloreak.Primary;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
            }
        }

        private async Task KargatuErreserbakAsync()
        {
            var erreserbak = await _api.ErreserbakLortuAsync();

            var aukeragarriak = erreserbak
                .Where(e => e.Ordainduta == 0)
                .ToList();

            cmb_Erreserbak.DisplayMember = "BezeroIzena";
            cmb_Erreserbak.ValueMember = "Id";
            cmb_Erreserbak.DataSource = aukeragarriak;
        }

        private async void cmb_Erreserbak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Erreserbak.SelectedItem is ErreserbaDto erreserba)
            {
                _hautatutakoErreserba = erreserba;
                await KargatuEskariakEtaGuztiraAsync(erreserba.Id);
            }
        }

        private async Task KargatuEskariakEtaGuztiraAsync(int erreserbaId)
        {
            var eskariak = await _api.LortuEskariakAsync();
            var eskariakErreserba = eskariak
                .Where(e => e.ErreserbaId == erreserbaId)
                .ToList();

            var produktuak = eskariakErreserba
                .SelectMany(e => e.Produktuak)
                .ToList();

            dgv_Eskariak.DataSource = produktuak;

            if (dgv_Eskariak.Columns.Contains("Id"))
                dgv_Eskariak.Columns["Id"].Visible = false;

            if (dgv_Eskariak.Columns.Contains("EskariaId"))
                dgv_Eskariak.Columns["EskariaId"].Visible = false;

            if (dgv_Eskariak.Columns.Contains("ProduktuaId"))
                dgv_Eskariak.Columns["ProduktuaId"].Visible = false;

            if (dgv_Eskariak.Columns.Contains("ProduktuaIzena"))
                dgv_Eskariak.Columns["ProduktuaIzena"].HeaderText = "Produktua";

            if (dgv_Eskariak.Columns.Contains("Kantitatea"))
                dgv_Eskariak.Columns["Kantitatea"].HeaderText = "Kantitatea";

            if (dgv_Eskariak.Columns.Contains("Prezioa"))
                dgv_Eskariak.Columns["Prezioa"].HeaderText = "Prezioa (€)";

            dgv_Eskariak.RowHeadersVisible = false;

            double guztira = produktuak.Sum(p => p.Kantitatea * p.Prezioa);
            _guztira = guztira;
            lbl_Guztira.Text = _guztira.ToString("0.00 €");

            txt_JasoDenDirua.Text = "";
            lbl_Itzulia.Text = "0,00 €";
        }

        private void btn_Eskudirua_Click(object sender, EventArgs e)
        {
            _eskudiruaAukeratuta = true;
            txt_JasoDenDirua.Enabled = true;
            txt_JasoDenDirua.Focus();

            btn_Eskudirua.BackColor = Color.LightGreen;
            btn_Txartela.BackColor = SystemColors.Control;

            KalkulatuItzulia();
        }

        private void btn_Txartela_Click(object sender, EventArgs e)
        {
            _eskudiruaAukeratuta = false;
            txt_JasoDenDirua.Enabled = false;
            txt_JasoDenDirua.Text = "";
            lbl_Itzulia.Text = "0,00 €";

            btn_Txartela.BackColor = Color.LightGreen;
            btn_Eskudirua.BackColor = SystemColors.Control;
        }

        private void txt_JasoDenDirua_TextChanged(object sender, EventArgs e)
        {
            if (_eskudiruaAukeratuta)
                KalkulatuItzulia();
        }

        private void KalkulatuItzulia()
        {
            if (!double.TryParse(txt_JasoDenDirua.Text.Replace("€", "").Trim(),
                                 NumberStyles.Number,
                                 CultureInfo.CurrentCulture,
                                 out var jasotakoa))
            {
                lbl_Itzulia.Text = "0,00 €";
                return;
            }

            var itzulia = jasotakoa - _guztira;
            lbl_Itzulia.Text = itzulia < 0 ? "0,00 €" : itzulia.ToString("0.00 €");
        }

        private async void btn_Ordaindu_Click(object sender, EventArgs e)
        {
            if (_hautatutakoErreserba == null)
            {
                MessageBox.Show("Lehenengo erreserba bat aukeratu.", "Abisua",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ordainketaModua = _eskudiruaAukeratuta ? "Eskudirua" : "Txartela";

            double jasotakoa = 0;
            double itzulia = 0;

            if (_eskudiruaAukeratuta)
            {
                if (!double.TryParse(txt_JasoDenDirua.Text.Replace("€", "").Trim(),
                                     NumberStyles.Number,
                                     CultureInfo.CurrentCulture,
                                     out jasotakoa))
                {
                    MessageBox.Show("Sartu jasotako dirua zuzen.", "Errorea",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (jasotakoa < _guztira)
                {
                    MessageBox.Show("Jasotako dirua ez da nahikoa.", "Errorea",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                itzulia = jasotakoa - _guztira;
            }

            var dto = new ErreserbaOrdainduDto
            {
                ErreserbaId = _hautatutakoErreserba.Id,
                Guztira = _guztira,
                Jasotakoa = jasotakoa,
                Itzulia = itzulia,
                LangileaId = _langileaId,
                OrdainketaModua = ordainketaModua
            };

            bool ok = await _api.OrdainduErreserbaAsync(dto);

            if (!ok)
            {
                MessageBox.Show("Errorea erreserba ordaintzean.", "Errorea",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Erreserba ongi ordaindu da.", "Informazioa",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            _AurrekoPantaila.Show();
            this.Close();
        }
    }
}
