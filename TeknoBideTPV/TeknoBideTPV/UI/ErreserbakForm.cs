using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TeknoBideTPV.DTOak;
using TeknoBideTPV.UI.Styles;
using TeknoBideTPV.Zerbitzuak;
using System.Diagnostics;
using System.Drawing;

namespace TeknoBideTPV.UI
{
    public partial class ErreserbakForm : Form
    {
        private readonly ApiZerbitzua _api = new ApiZerbitzua();

        private List<ErreserbaDto> _erreserbakOriginalak = new();
        private List<MahaiaDto> mahaiak = new();

        private bool _editatuKolumnaGehituta = false;

        private Form _AurrekoPantaila;

        private bool _hasierakoaGordeta = false;

        public ErreserbakForm(Form AurrekoPantaila)
        {
            InitializeComponent();

            //minimizatu maximizatu eta itxi botoiak ezkutatu
            this.ControlBox = false;
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            dgv_ErreserbakIkusi.CellClick += dgv_ErreserbakIkusi_CellClick;

            _AurrekoPantaila = AurrekoPantaila;
            this.Load += ErreserbakForm_Load;
            this.Shown += ErreserbakForm_Shown;

            PrestatuFooter();
        }
        private void ErreserbakForm_Shown(object sender, EventArgs e)
        {
            if (!_hasierakoaGordeta)
            {
                TPVEstiloaFinkoa.Prestatu(this);
                _hasierakoaGordeta = true;
            }

            TPVEstiloaFinkoa.Aplikatu(this);
            DistribuzioaAjustatu();
        }

        private void DistribuzioaAjustatu()
        {
            int magina = 20;


            if (txt_Bilatu.Left < lbl_Bilatu.Right + magina)
            {
                txt_Bilatu.Left = lbl_Bilatu.Right + magina;
            }

            if (lbl_Eguna.Left < txt_Bilatu.Right + magina)
            {
                lbl_Eguna.Left = txt_Bilatu.Right + magina;
            }

            if (dtp_Eguna.Left < lbl_Eguna.Right + magina)
            {
                dtp_Eguna.Left = lbl_Eguna.Right + magina;
            }

            if (btn_Bilatu.Left < dtp_Eguna.Right + magina)
            {
                btn_Bilatu.Left = dtp_Eguna.Right + magina;
            }

             if (btn_Garbitu.Left < btn_Bilatu.Right + magina)
            {
                btn_Garbitu.Left = btn_Bilatu.Right + magina;
            }

            btn_Garbitu.AutoSize = false;
            int requiredWidth = TextRenderer.MeasureText(btn_Garbitu.Text, btn_Garbitu.Font).Width + 30;
            int availableWidth = pnl_Filtroak.ClientSize.Width - btn_Garbitu.Left - magina;
            if (availableWidth > 0)
            {
                btn_Garbitu.Width = Math.Min(requiredWidth, availableWidth);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            DistribuzioaAjustatu();
        }

        private async void ErreserbakForm_Load(object sender, EventArgs e)
        {
            EstilatuKontrolak();
            await KargatuMahaiak();
            await KargatuErreserbak();
            EstilatuDataGridView();

            headerControl_Erreserbak.Izena = "TXAPELA";
            headerControl_Erreserbak.Titulo = "ERRESERBAK";
            headerControl_Erreserbak.Erabiltzailea = SesioZerbitzua.Izena;
            headerControl_Erreserbak.DataOrdua = DateTime.Now.ToString("dddd, dd MMMM yyyy - HH:mm");
        }

        private void PrestatuFooter()
        {
            footerControl_Erreserbak.AtzeraTestua = "Atzera";

            footerControl_Erreserbak.AtzeraClick += (s, e) =>
            {
                _AurrekoPantaila.Show();
                this.Close();
            };
        }


        private async Task KargatuMahaiak()
        {
            mahaiak = await _api.MahaiakLortuAsync();
        }

        private async Task KargatuErreserbak()
        {
            try
            {
                _erreserbakOriginalak = await _api.ErreserbakLortuAsync();

                foreach (var r in _erreserbakOriginalak)
                {
                    r.MahaiaZenbakia =
                        mahaiak.FirstOrDefault(m => m.Id == r.MahaiakId)?.Zenbakia ?? 0;
                }

                dgv_ErreserbakIkusi.DataSource = _erreserbakOriginalak;

                EzarriKolumnak();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea erreserbak kargatzen: {ex.Message}");
            }
        }

        private void EzarriKolumnak()
        {
            if (dgv_ErreserbakIkusi.Columns.Count == 0)
                return;

            dgv_ErreserbakIkusi.RowHeadersVisible = false;
            dgv_ErreserbakIkusi.Columns["Id"].Visible = false;
            dgv_ErreserbakIkusi.Columns["EgunaOrdua"].Visible = false;
            dgv_ErreserbakIkusi.Columns["FakturaRuta"].Visible = false;
            dgv_ErreserbakIkusi.Columns["MahaiakId"].Visible = false;

            dgv_ErreserbakIkusi.Columns["BezeroIzena"].HeaderText = "Bezeroa";
            dgv_ErreserbakIkusi.Columns["Telefonoa"].HeaderText = "Telefonoa";
            dgv_ErreserbakIkusi.Columns["PertsonaKopurua"].HeaderText = "Kopurua";
            dgv_ErreserbakIkusi.Columns["Eguna"].HeaderText = "Eguna";
            dgv_ErreserbakIkusi.Columns["Ordua"].HeaderText = "Ordua";
            dgv_ErreserbakIkusi.Columns["PrezioTotala"].HeaderText = "Prezioa";
            dgv_ErreserbakIkusi.Columns["LangileaId"].Visible = false;
            dgv_ErreserbakIkusi.Columns["LangileaIzena"].HeaderText = "Langilea";
            dgv_ErreserbakIkusi.Columns["Ordainduta"].Visible = false;
            dgv_ErreserbakIkusi.Columns["OrdaindutaTestua"].HeaderText = "Ordaindua";

            dgv_ErreserbakIkusi.Columns["MahaiaZenbakia"].HeaderText = "Mahaia";

            if (!_editatuKolumnaGehituta)
            {
                var editatuCol = new DataGridViewButtonColumn
                {
                    HeaderText = "Editatu",
                    Text = "Editatu",
                    UseColumnTextForButtonValue = true,
                    Width = 80,
                    Name = "Editatu"
                };
                dgv_ErreserbakIkusi.Columns.Add(editatuCol);

                var tiketaCol = new DataGridViewButtonColumn
                {
                    HeaderText = "Tiketa",
                    Text = "Ikusi",
                    UseColumnTextForButtonValue = true,
                    Width = 80,
                    Name = "Tiketa"
                };
                dgv_ErreserbakIkusi.Columns.Add(tiketaCol);

                var ezabatuCol = new DataGridViewButtonColumn
                {
                    HeaderText = "Ezabatu",
                    Text = "Ezabatu",
                    UseColumnTextForButtonValue = true,
                    Width = 80,
                    Name = "Ezabatu"
                };
                dgv_ErreserbakIkusi.Columns.Add(ezabatuCol);

                _editatuKolumnaGehituta = true;
            }

            foreach (DataGridViewColumn col in dgv_ErreserbakIkusi.Columns)
            {
                col.FillWeight = 100;
            }

            if (dgv_ErreserbakIkusi.Columns.Contains("Editatu"))
                dgv_ErreserbakIkusi.Columns["Editatu"].FillWeight = 60;

            if (dgv_ErreserbakIkusi.Columns.Contains("Tiketa"))
                dgv_ErreserbakIkusi.Columns["Tiketa"].FillWeight = 60;

            if (dgv_ErreserbakIkusi.Columns.Contains("Ezabatu"))
                dgv_ErreserbakIkusi.Columns["Ezabatu"].FillWeight = 60;
        }

        private async void dgv_ErreserbakIkusi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgv_ErreserbakIkusi.Columns[e.ColumnIndex].Name == "Editatu")
            {
                var dto = dgv_ErreserbakIkusi.Rows[e.RowIndex].DataBoundItem as ErreserbaDto;
                if (dto != null)
                {
                    var editForm = new ErreserbakEditatuForm(this, dto);
                    editForm.ShowDialog();
                    await KargatuErreserbak();
                }
            }
            else if (dgv_ErreserbakIkusi.Columns[e.ColumnIndex].Name == "Tiketa")
            {
                var dto = dgv_ErreserbakIkusi.Rows[e.RowIndex].DataBoundItem as ErreserbaDto;
                
                if (dto == null)
                {
                    MessageBox.Show("Errorea: Ezin izan da erreserba identifikatu.");
                    return;
                }

                if (string.IsNullOrEmpty(dto.FakturaRuta))
                {
                    if (dto.Ordainduta == 1)
                    {
                        string baseUrl = ApiZerbitzua.BASE_URL.TrimEnd('/');
                        string url = $"{baseUrl}/api/erreserbak/tiket/{dto.Id}";
                        try
                        {
                            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Errorea tiketa irekitzean: {ex.Message}");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Erreserba hau oraindik ez da ordaindu, beraz ez du tiketarik.");
                    }
                    return;
                }

                {
                    string baseUrl = ApiZerbitzua.BASE_URL.TrimEnd('/');
                    string url = $"{baseUrl}/api/erreserbak/tiket/{dto.Id}";

                    try
                    {
                        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Errorea nabigatzailea irekitzean: {ex.Message}");
                    }
                }
            }
            else if (dgv_ErreserbakIkusi.Columns[e.ColumnIndex].Name == "Ezabatu")
            {
                var dto = dgv_ErreserbakIkusi.Rows[e.RowIndex].DataBoundItem as ErreserbaDto;
                if (dto != null)
                {
                    var result = MessageBox.Show(
                        "Ziur zaude erreserba hau ezabatu nahi duzula? Lotutako eskariak ere ezabatuko dira.",
                        "Erreserba Ezabatu",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        bool ondo = await _api.EzabatuErreserbaAsync(dto.Id);
                        if (ondo)
                        {
                            MessageBox.Show("Erreserba ondo ezabatu da.", "Egina", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            await KargatuErreserbak();
                        }
                        else
                        {
                            MessageBox.Show("Errorea erreserba ezabatzean.", "Errorea", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }


        private void EstilatuDataGridView()
        {
            dgv_ErreserbakIkusi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv_ErreserbakIkusi.BackgroundColor = TPVEstiloa.Koloreak.Background;
            dgv_ErreserbakIkusi.BorderStyle = BorderStyle.None;

            dgv_ErreserbakIkusi.ColumnHeadersDefaultCellStyle.BackColor = TPVEstiloa.Koloreak.Primary;
            dgv_ErreserbakIkusi.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv_ErreserbakIkusi.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgv_ErreserbakIkusi.EnableHeadersVisualStyles = false;

            dgv_ErreserbakIkusi.DefaultCellStyle.BackColor = Color.White;
            dgv_ErreserbakIkusi.DefaultCellStyle.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            dgv_ErreserbakIkusi.DefaultCellStyle.Font = new Font("Segoe UI", 11);
            dgv_ErreserbakIkusi.DefaultCellStyle.SelectionBackColor = TPVEstiloa.Koloreak.Secondary;
            dgv_ErreserbakIkusi.DefaultCellStyle.SelectionForeColor = TPVEstiloa.Koloreak.TextTitle;

            dgv_ErreserbakIkusi.RowTemplate.Height = 40;

            TPVEstiloaFinkoa.EguneratuKontrola(dgv_ErreserbakIkusi, this);
        }

        private void EstilatuKontrolak()
        {
            this.BackColor = TPVEstiloa.Koloreak.Background;
            pnl_Filtroak.BackColor = TPVEstiloa.Koloreak.Background;

            lbl_Bilatu.ForeColor = TPVEstiloa.Koloreak.TextTitle;
            lbl_Eguna.ForeColor = TPVEstiloa.Koloreak.TextTitle;

            txt_Bilatu.BackColor = Color.White;
            dtp_Eguna.BackColor = Color.White;

            Button[] botoiak = { btn_Bilatu, btn_Garbitu };
            foreach (var btn in botoiak)
            {
                btn.BackColor = TPVEstiloa.Koloreak.Primary;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
            }
        }

        private void btn_Bilatu_Click(object sender, EventArgs e)
        {
            FiltratuErreserbak();
        }

        private void FiltratuErreserbak()
        {
            if (_erreserbakOriginalak == null || _erreserbakOriginalak.Count == 0)
                return;

            string testua = txt_Bilatu.Text.Trim().ToLower();
            bool egunaAktibatuta = dtp_Eguna.Checked;
            DateTime eguna = dtp_Eguna.Value.Date;

            IEnumerable<ErreserbaDto> filtratuta = _erreserbakOriginalak;

            if (!string.IsNullOrEmpty(testua))
            {
                filtratuta = filtratuta.Where(r =>
                    r.BezeroIzena != null &&
                    r.BezeroIzena.ToLower().Contains(testua));
            }

            if (egunaAktibatuta)
            {
                filtratuta = filtratuta.Where(r => r.EgunaOrdua.Date == eguna);
            }

            dgv_ErreserbakIkusi.DataSource = filtratuta.ToList();
            EzarriKolumnak();
        }

        private void btn_Garbitu_Click(object sender, EventArgs e)
        {
            txt_Bilatu.Text = string.Empty;
            dtp_Eguna.Checked = false;

            dgv_ErreserbakIkusi.DataSource = _erreserbakOriginalak.ToList();
            EzarriKolumnak();
        }
    }
}
