using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TeknoBideTPV.DTOak;
using TeknoBideTPV.Zerbitzuak;
using TeknoBideTPV.UI.Styles;

namespace TeknoBideTPV.UI
{
    public partial class MahaiakKudeatuForm : Form
    {
        private readonly ApiZerbitzua _api = new ApiZerbitzua();
        private Form _AurrekoPantaila;

        private int? _mahaiaIdAktiboa = null;

        private Panel? _panelAukeratua = null;

        public MahaiakKudeatuForm(Form AurrekoPantaila)
        {
            InitializeComponent();
            TPVEstiloa.PantailarenEskalatuaHasi(this);

            this.ControlBox = false;
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            _AurrekoPantaila = AurrekoPantaila;
            this.Load += MahaiakKudeatuForm_Load;

            PrestatuFooter();
            PrestatuKokapenak();
            PrestatuHasierakoEgoera();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            TPVEstiloa.EskalatuaAplikatu(this);
            DistribuzioaAjustatu();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            DistribuzioaAjustatu();
        }

        private void DistribuzioaAjustatu()
        {
            int margina = 30; 
            
            
            btn_Garbitu.Left = pnl_Ezaugarriak.Width - btn_Garbitu.Width - 20;
            btn_Garbitu.Height = Math.Max(btn_Garbitu.Height, 42);

            if (lbl_Izenburua.Right + 20 > btn_Garbitu.Left)
            {
                 btn_Garbitu.Top = lbl_Izenburua.Bottom + 10;
                 btn_Garbitu.Left = pnl_Ezaugarriak.Width - btn_Garbitu.Width - 20;
            }
            else
            {
                 btn_Garbitu.Top = 20;
            }
            
            int startY = Math.Max(lbl_Izenburua.Bottom, btn_Garbitu.Bottom) + 40;

            int maxLabelRight = 0;
            if (lbl_Izena.Right > maxLabelRight) maxLabelRight = lbl_Izena.Right;
            if (lbl_PertsonaKopurua.Right > maxLabelRight) maxLabelRight = lbl_PertsonaKopurua.Right;
            if (lbl_Kokapena.Right > maxLabelRight) maxLabelRight = lbl_Kokapena.Right;
            
            int inputLeft = maxLabelRight + 20;
            
            int availableWidth = pnl_Ezaugarriak.Width - inputLeft - 20;

            int newWidth = Math.Max(300, availableWidth); 

            if (inputLeft + newWidth > pnl_Ezaugarriak.Width - 20)
                newWidth = pnl_Ezaugarriak.Width - inputLeft - 20;

            txt_Zenbakia.Width = newWidth;
            nud_PertsonaKopurua.Width = newWidth;
            cmb_Kokapena.Width = newWidth;

            txt_Zenbakia.Left = inputLeft;
            nud_PertsonaKopurua.Left = inputLeft;
            cmb_Kokapena.Left = inputLeft;

            AlignRow(lbl_Izena, txt_Zenbakia, startY);
            startY += Math.Max(lbl_Izena.Height, txt_Zenbakia.Height) + margina;

            AlignRow(lbl_PertsonaKopurua, nud_PertsonaKopurua, startY);
            startY += Math.Max(lbl_PertsonaKopurua.Height, nud_PertsonaKopurua.Height) + margina;

            AlignRow(lbl_Kokapena, cmb_Kokapena, startY);
            startY += Math.Max(lbl_Kokapena.Height, cmb_Kokapena.Height) + margina;

            int groupLeft = Math.Min(lbl_Izena.Left, Math.Min(lbl_PertsonaKopurua.Left, lbl_Kokapena.Left));
            int groupRight = inputLeft + newWidth;
            int groupWidth = Math.Max(100, groupRight - groupLeft);
            int gap = 20;
            int halfWidth = Math.Max(80, (groupWidth - gap) / 2);

            btn_Gorde.Top = startY + 10;
            btn_Ezabatu.Top = startY + 10;
            btn_Gorde.Left = groupLeft;
            btn_Gorde.Width = halfWidth;
            btn_Ezabatu.Left = btn_Gorde.Right + 20;
            btn_Ezabatu.Width = halfWidth;

            btn_MahaiBerria.Top = btn_Gorde.Bottom + 20;
            btn_MahaiBerria.Left = groupLeft;
            btn_MahaiBerria.Width = groupWidth;
        }

        private void AlignRow(Control label, Control input, int topY)
        {
            int maxHeight = Math.Max(label.Height, input.Height);
            int centerY = topY + (maxHeight / 2);

            label.Top = centerY - (label.Height / 2);
            input.Top = centerY - (input.Height / 2);
        }

        private async void MahaiakKudeatuForm_Load(object sender, EventArgs e)
        {
            EstilatuKontrolak();

            headerControl_MahaiakKudeatu.Izena = "TXAPELA";
            headerControl_MahaiakKudeatu.Titulo = "MAHAIAK KUDEATU";
            headerControl_MahaiakKudeatu.Erabiltzailea = SesioZerbitzua.Izena;
            headerControl_MahaiakKudeatu.DataOrdua = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm");

            if (!SesioZerbitzua.Gerentea)
            {
                btn_Gorde.Enabled = false;
                btn_Ezabatu.Enabled = false;
                btn_MahaiBerria.Enabled = false;
            }

            await KargatuMahaiakAsync();
        }

        private void EstilatuKontrolak()
        {
            this.BackColor = TPVEstiloa.Koloreak.Background;
            pnl_Edukia.BackColor = TPVEstiloa.Koloreak.Background;
            pnl_Ezaugarriak.BackColor = TPVEstiloa.Koloreak.Background;

            Label[] labels = { lbl_Izena, lbl_Izenburua, lbl_PertsonaKopurua, lbl_Kokapena };
            foreach (var lbl in labels)
            {
                lbl.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            }

            btn_Gorde.BackColor = TPVEstiloa.Koloreak.Primary;
            btn_Gorde.ForeColor = TPVEstiloa.Koloreak.White;
            btn_Gorde.FlatStyle = FlatStyle.Flat;
            btn_Gorde.FlatAppearance.BorderSize = 0;

            btn_MahaiBerria.BackColor = TPVEstiloa.Koloreak.Primary;
            btn_MahaiBerria.ForeColor = TPVEstiloa.Koloreak.White;
            btn_MahaiBerria.FlatStyle = FlatStyle.Flat;
            btn_MahaiBerria.FlatAppearance.BorderSize = 0;

            btn_Ezabatu.BackColor = TPVEstiloa.Koloreak.Error;
            btn_Ezabatu.ForeColor = TPVEstiloa.Koloreak.White;
            btn_Ezabatu.FlatStyle = FlatStyle.Flat;
            btn_Ezabatu.FlatAppearance.BorderSize = 0;

            btn_Garbitu.BackColor = TPVEstiloa.Koloreak.Secondary;
            btn_Garbitu.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            btn_Garbitu.FlatStyle = FlatStyle.Flat;
            btn_Garbitu.FlatAppearance.BorderSize = 0;
        }

        private void PrestatuFooter()
        {
            footerControl_MahaiakKudeatu.AtzeraTestua = "Atzera";

            footerControl_MahaiakKudeatu.AtzeraClick += (s, e) =>
            {
                _AurrekoPantaila.Show();
                this.Close();
            };
        }

        private void PrestatuKokapenak()
        {
            cmb_Kokapena.Items.AddRange(new[]
            {
                "Barneko gunea",
                "Kanpoko terraza",
                "Leiho ondoan",
                "Areto nagusia"
            });
        }

        private void PrestatuHasierakoEgoera()
        {
            txt_Zenbakia.ReadOnly = true;
            cmb_Kokapena.Enabled = false;
            nud_PertsonaKopurua.Enabled = false;
        }

        private async Task KargatuMahaiakAsync()
        {
            flp_Mahaiak.Controls.Clear();

            var mahaiak = await _api.MahaiakLortuAsync();

            foreach (var mahaia in mahaiak)
            {
                var kontrola = SortuMahaiaBanaka(mahaia);
                flp_Mahaiak.Controls.Add(kontrola);
            }
        }

        private void PanelariEsatiloaEman(Panel p)
        {
            p.BackColor = Color.White;
            p.BorderStyle = BorderStyle.None;
        }

        private void EstilatuPanelAukeratua(Panel p)
        {
            p.BackColor = TPVEstiloa.Koloreak.Secondary;
            p.BorderStyle = BorderStyle.FixedSingle;
        }

        private Control SortuMahaiaBanaka(MahaiaDto mahaia)
        {
            var panel = new Panel
            {
                Width = 180,
                Height = 140,
                Margin = new Padding(10),
                Cursor = Cursors.Hand,
                Tag = mahaia,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            panel.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            };

            var lblZenbakia = new Label
            {
                Text = $"🪑 Mahai {mahaia.Zenbakia}",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 40,
                ForeColor = TPVEstiloa.Koloreak.TextTitle
            };

            var lblPertsonak = new Label
            {
                Text = $"👥 {mahaia.PertsonaKopurua} pertsona",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 30,
                ForeColor = TPVEstiloa.Koloreak.TextTitle
            };

            var lblKokapena = new Label
            {
                Text = $"📍 {mahaia.Kokapena}",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = TPVEstiloa.Koloreak.TextNormal
            };

            panel.Controls.Add(lblKokapena);
            panel.Controls.Add(lblPertsonak);
            panel.Controls.Add(lblZenbakia);

            panel.Click += MahaiaAukeratu;
            lblZenbakia.Click += MahaiaAukeratu;
            lblPertsonak.Click += MahaiaAukeratu;
            lblKokapena.Click += MahaiaAukeratu;

            return panel;
        }


        private void MahaiaAukeratu(object sender, EventArgs e)
        {
            Control kontrola = sender as Control;

            if (kontrola is Label)
                kontrola = kontrola.Parent;

            var mahaia = kontrola.Tag as MahaiaDto;
            if (mahaia == null) return;

            if (_panelAukeratua != null)
                PanelariEsatiloaEman(_panelAukeratua);

            Panel panel = (Panel)kontrola;
            EstilatuPanelAukeratua(panel);
            _panelAukeratua = panel;

            _mahaiaIdAktiboa = mahaia.Id;

            txt_Zenbakia.Text = mahaia.Zenbakia.ToString();
            nud_PertsonaKopurua.Value = mahaia.PertsonaKopurua;
            for (int i = 0; i < cmb_Kokapena.Items.Count; i++)
            {
                if (string.Equals(cmb_Kokapena.Items[i].ToString(), mahaia.Kokapena, StringComparison.OrdinalIgnoreCase))
                {
                    cmb_Kokapena.SelectedIndex = i;
                    break;
                }
            }

            txt_Zenbakia.ReadOnly = false;
            cmb_Kokapena.Enabled = true;
            nud_PertsonaKopurua.Enabled = true;

        }

        private async void btn_Gorde_Click(object sender, EventArgs e)
        {
            if (_mahaiaIdAktiboa == null)
            {
                MessageBox.Show("Ez dago mahaia aukeratuta.");
                return;
            }

            if (!int.TryParse(txt_Zenbakia.Text, out int zenbakia))
            {
                MessageBox.Show("Zenbakia ez da baliozko zenbaki bat.");
                return;
            }

            var dto = new MahaiaDto
            {
                Id = _mahaiaIdAktiboa.Value,
                Zenbakia = zenbakia,
                PertsonaKopurua = (int)nud_PertsonaKopurua.Value,
                Kokapena = cmb_Kokapena.SelectedItem?.ToString() ?? ""
            };

            var ok = await _api.AktualizatuMahaiaAsync(dto);

            if (ok)
            {
                MessageBox.Show("Mahaia eguneratuta!");
                await KargatuMahaiakAsync();
            }
            else
            {
                MessageBox.Show("Errorea mahaia eguneratzean.");
            }
        }

        private async void btn_Ezabatu_Click(object sender, EventArgs e)
        {
            if (_mahaiaIdAktiboa == null)
            {
                MessageBox.Show("Ez dago mahaia aukeratuta.");
                return;
            }

            var ok = await _api.EzabatuMahaiaAsync(_mahaiaIdAktiboa.Value);

            if (ok)
            {
                MessageBox.Show("Mahaia ezabatuta!");

                _mahaiaIdAktiboa = null;
                txt_Zenbakia.Text = "";
                nud_PertsonaKopurua.Value = 0;
                cmb_Kokapena.SelectedIndex = -1;

                PrestatuHasierakoEgoera();
                await KargatuMahaiakAsync();
            }
            else
            {
                MessageBox.Show("Errorea mahaia ezabatzean.");
            }
        }

        private async void btn_MahaiBerria_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Zenbakia.Text))
            {
                MessageBox.Show("Zenbakia ezin da hutsik egon.");
                return;
            }

            var dto = new MahaiaSortuDto
            {
                Zenbakia = txt_Zenbakia.Text,
                PertsonaKopurua = (int)nud_PertsonaKopurua.Value,
                Kokapena = cmb_Kokapena.SelectedItem?.ToString() ?? ""
            };

            var ok = await _api.SortuMahaiaAsync(dto);

            if (ok)
            {
                MessageBox.Show("Mahaia sortuta!");
                await KargatuMahaiakAsync();
            }
            else
            {
                MessageBox.Show("Errorea mahaia sortzean.");
            }
        }

        private void btn_Garbitu_Click(object sender, EventArgs e)
        {
            _mahaiaIdAktiboa = null;

            txt_Zenbakia.Text = "";
            nud_PertsonaKopurua.Value = 0;
            cmb_Kokapena.SelectedIndex = -1;

            txt_Zenbakia.ReadOnly = false;
            cmb_Kokapena.Enabled = true;
            nud_PertsonaKopurua.Enabled = true;

            if (_panelAukeratua != null)
            {
                PanelariEsatiloaEman(_panelAukeratua);
                _panelAukeratua = null;
            }
        }

    }
}
