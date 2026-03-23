using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using TeknoBideTPV.DTOak;
using TeknoBideTPV.Zerbitzuak;
using TeknoBideTPV.UI.Styles;

namespace TeknoBideTPV.UI
{
    public partial class ErreserbakEditatuForm : Form
    {
        private Form _AurrekoPantaila;
        private readonly ApiZerbitzua _api = new ApiZerbitzua();
        private int _langileaId;
        private ErreserbaDto _erreserba;
        private bool _hasierakoaGordeta = false;

        public ErreserbakEditatuForm(Form AurrekoPantaila, ErreserbaDto erreserba)
        {
            InitializeComponent();
            TPVEstiloaFinkoa.Prestatu(this);
            _hasierakoaGordeta = true;

            this.ControlBox = false;
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            _AurrekoPantaila = AurrekoPantaila;
            _erreserba = erreserba;
            _langileaId = SesioZerbitzua.LangileaId;

            this.Load += ErreserbakEditatuForm_Load;
            PrestatuFooter();
        }

        private async void ErreserbakEditatuForm_Load(object sender, EventArgs e)
        {
            EstilatuKontrolak();

            OrduakEzarri();
            await KargatuMahaiak();

            BindeatuDatuak();

            headerControl_EskariakEditatu.Izena = "TXAPELA";
            headerControl_EskariakEditatu.Titulo = "ERRESERBA EDITATU";
            headerControl_EskariakEditatu.Erabiltzailea = SesioZerbitzua.Izena;
            headerControl_EskariakEditatu.DataOrdua = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm");
        }

        private async Task KargatuMahaiak()
        {
            var mahaiak = await _api.MahaiakLortuAsync();
            
            cmb_Mahaiak.DataSource = mahaiak;
            cmb_Mahaiak.DisplayMember = "Zenbakia";
            cmb_Mahaiak.ValueMember = "Id";

            cmb_Mahaiak.SelectedIndexChanged += Cmb_Mahaiak_SelectedIndexChanged;
            
            cmb_Mahaiak.Format += (s, ev) =>
            {
                if (ev.ListItem is MahaiaDto mahaia)
                {
                    ev.Value = $"{mahaia.Zenbakia} (Max: {mahaia.PertsonaKopurua} perts.)";
                }
            };

            if (cmb_Mahaiak.SelectedItem != null)
            {
                Cmb_Mahaiak_SelectedIndexChanged(null, null);
            }
        }

        private void Cmb_Mahaiak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Mahaiak.SelectedItem is MahaiaDto mahaia)
            {
                nud_PertsonaKopurua.Maximum = mahaia.PertsonaKopurua;
                if (nud_PertsonaKopurua.Value > mahaia.PertsonaKopurua)
                {
                    nud_PertsonaKopurua.Value = mahaia.PertsonaKopurua;
                }
            }
        }

        private void PrestatuFooter()
        {
            footerControl_EskariakEditatu.AtzeraTestua = "Atzera";

            footerControl_EskariakEditatu.AtzeraClick += (s, e) =>
            {
                _AurrekoPantaila.Show();
                this.Close();
            };
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            TPVEstiloaFinkoa.Aplikatu(this);

            if (nud_PertsonaKopurua.Top < lbl_PertsonaKopurua.Bottom + 5)
            {
                nud_PertsonaKopurua.Top = lbl_PertsonaKopurua.Bottom + 5;
            }
            nud_PertsonaKopurua.Left = txt_BezeroIzena.Left;

            DistribuzioaAjustatu();
        }

        private void EstilatuKontrolak()
        {
            this.BackColor = TPVEstiloa.Koloreak.Background;
            pnl_EzkerrekoPanela.BackColor = TPVEstiloa.Koloreak.Background;
            pnl_EskubikoPanela.BackColor = TPVEstiloa.Koloreak.Background;

            Label[] labels = { lbl_BezeroIzena, lbl_Telefonoa, lbl_PertsonaKopurua, lbl_Eguna, lbl_Ordua, lbl_Mahaia };
            foreach (var lbl in labels)
            {
                lbl.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            }

            Control[] inputs = { txt_BezeroIzena, txt_Telefonoa, nud_PertsonaKopurua, dtp_Eguna, cmb_Ordua, cmb_Mahaiak };
            foreach (var input in inputs)
            {
                input.BackColor = Color.White;
            }

            btn_Gorde.BackColor = TPVEstiloa.Koloreak.Primary;
            btn_Gorde.ForeColor = Color.White;
            btn_Gorde.FlatStyle = FlatStyle.Flat;
            btn_Gorde.FlatAppearance.BorderSize = 0;
        }

        private void DistribuzioaAjustatu()
        {
            int top = headerControl_EskariakEditatu.Height;
            int bottom = footerControl_EskariakEditatu.Height;
            int availH = this.ClientSize.Height - top - bottom;
            int availW = this.ClientSize.Width;

            int marginX = Math.Max(20, (int)(availW * 0.05));
            int gapX = Math.Max(20, (int)(availW * 0.04));
            int panelW = Math.Max(300, (availW - marginX * 2 - gapX) / 2);
            int panelH = Math.Max(300, (int)(availH * 0.68));
            int y = top + Math.Max(0, (availH - panelH) / 2);

            pnl_EzkerrekoPanela.SetBounds(marginX, y, panelW, panelH);
            pnl_EskubikoPanela.SetBounds(marginX + panelW + gapX, y, panelW, panelH);

            int btnMarginX = Math.Max(20, (int)(panelW * 0.03));
            int btnMarginY = Math.Max(20, (int)(panelH * 0.03));
            btn_Gorde.Location = new Point(
                pnl_EskubikoPanela.Right - btn_Gorde.Width - btnMarginX,
                pnl_EskubikoPanela.Bottom - btn_Gorde.Height - btnMarginY
            );

            this.Resize -= Form_Resize_Reajuste;
            this.Resize += Form_Resize_Reajuste;
        }

        private void Form_Resize_Reajuste(object sender, EventArgs e)
        {
            DistribuzioaAjustatu();
        }

        private void OrduakEzarri()
        {
            cmb_Ordua.Items.Clear();

            TimeSpan bazkariHasiera = new TimeSpan(12, 30, 0);
            TimeSpan bazkariAmaiera = new TimeSpan(15, 30, 0);

            TimeSpan afariHasiera = new TimeSpan(20, 0, 0);
            TimeSpan afariAmaiera = new TimeSpan(23, 0, 0);

            TimeSpan denboraTartea = new TimeSpan(0, 30, 0);

            for (TimeSpan ordua = bazkariHasiera; ordua <= bazkariAmaiera; ordua += denboraTartea)
            {
                cmb_Ordua.Items.Add(ordua.ToString(@"hh\:mm"));
            }

            for (TimeSpan ordua = afariHasiera; ordua <= afariAmaiera; ordua += denboraTartea)
            {
                cmb_Ordua.Items.Add(ordua.ToString(@"hh\:mm"));
            }
        }

        private void BindeatuDatuak()
        {
            txt_BezeroIzena.Text = _erreserba.BezeroIzena;
            txt_Telefonoa.Text = _erreserba.Telefonoa;
            dtp_Eguna.Value = _erreserba.EgunaOrdua.Date;

            if (_erreserba.MahaiakId > 0)
            {
                cmb_Mahaiak.SelectedValue = _erreserba.MahaiakId;
            }
            
            if (_erreserba.PertsonaKopurua > nud_PertsonaKopurua.Maximum)
            {
                nud_PertsonaKopurua.Maximum = _erreserba.PertsonaKopurua;
            }
            nud_PertsonaKopurua.Value = _erreserba.PertsonaKopurua;

            string timeStr = _erreserba.EgunaOrdua.ToString("HH:mm");
            if (cmb_Ordua.Items.Contains(timeStr))
            {
                cmb_Ordua.SelectedItem = timeStr;
            }
            else
            {
                cmb_Ordua.Text = timeStr;
            }
        }

        private async void btn_Gorde_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_BezeroIzena.Text))
            {
                MessageBox.Show("Bezero izena bete behar da.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txt_Telefonoa.Text))
            {
                MessageBox.Show("Telefonoa bete behar da.");
                return;
            }

            if (cmb_Mahaiak.SelectedItem == null)
            {
                MessageBox.Show("Aukeratu mahaia.");
                return;
            }

            var mahaia = (MahaiaDto)cmb_Mahaiak.SelectedItem;

            if (!Regex.IsMatch(txt_Telefonoa.Text, @"^\d{9}$"))
            {
                MessageBox.Show("Telefonoak 9 zenbaki izan behar ditu.");
                return;
            }

            TimeSpan AukeratutakoOrdua;
            if (cmb_Ordua.SelectedItem != null)
                AukeratutakoOrdua = TimeSpan.Parse(cmb_Ordua.SelectedItem.ToString());
            else
            {
                if (!TimeSpan.TryParse(cmb_Ordua.Text, out AukeratutakoOrdua))
                {
                    MessageBox.Show("Ordu formatua ez da zuzena.");
                    return;
                }
            }

            var dto = new ErreserbaSortuDto
            {
                BezeroIzena = txt_BezeroIzena.Text.Trim(),
                Telefonoa = txt_Telefonoa.Text.Trim(),
                PertsonaKopurua = (int)nud_PertsonaKopurua.Value,
                EgunaOrdua = dtp_Eguna.Value.Date + AukeratutakoOrdua,
                PrezioTotala = _erreserba.PrezioTotala,
                FakturaRuta = _erreserba.FakturaRuta,
                LangileaId = _langileaId,
                MahaiakId = mahaia.Id
            };

            bool ondo = await _api.UpdateErreserbaAsync(_erreserba.Id, dto);

            if (ondo)
            {
                MessageBox.Show("Erreserba eguneratuta!");
                _AurrekoPantaila.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Errorea erreserba eguneratzean.");
            }
        }
    }
}
