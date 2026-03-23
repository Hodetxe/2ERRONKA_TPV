using System;
using System.Drawing;
using System.Windows.Forms;
using TeknoBideTPV.Zerbitzuak;

namespace TeknoBideTPV.UI
{
    public partial class LoginForm : Form
    {
        private bool _hasierakoaGordeta = false;

        public LoginForm()
        {
            InitializeComponent();

            this.BackColor = TPVEstiloa.Koloreak.Background;
            
            btn_Sartu.BackColor = TPVEstiloa.Koloreak.Primary;
            btn_Sartu.ForeColor = TPVEstiloa.Koloreak.White;
            btn_Sartu.FlatStyle = FlatStyle.Flat;
            btn_Sartu.FlatAppearance.BorderSize = 0;
            
            lbl_Erabiltzailea.ForeColor = TPVEstiloa.Koloreak.Text;
            lbl_Pasahitza.ForeColor = TPVEstiloa.Koloreak.Text;

            txt_Erabiltzailea.BorderStyle = BorderStyle.FixedSingle;
            txt_Pasahitza.BorderStyle = BorderStyle.FixedSingle;

            this.WindowState = FormWindowState.Maximized;

            this.Shown += LoginForm_Shown;
            this.Resize += LoginForm_Resize;

            this.MaximizeBox = false;
            img_Logoa.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void LoginForm_Shown(object sender, EventArgs e)
        {
            if (!_hasierakoaGordeta)
            {
                TPVEstiloa.PantailarenEskalatuaHasi(this);
                _hasierakoaGordeta = true;
            }

            BirarraztuKontrolak();
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

            int imgMargin = 50;
            int halfWidth = this.ClientSize.Width / 2;
            
            img_Logoa.Location = new Point(halfWidth + imgMargin, imgMargin);
            img_Logoa.Size = new Size(halfWidth - (imgMargin * 2), this.ClientSize.Height - (imgMargin * 2));

            int axisX = (int)(halfWidth * 0.60); 
            int verticalGap = 40;
            int controlHeight = txt_Erabiltzailea.Height; 
            
            int maxInputWidth = 400;
            int availableWidth = halfWidth - axisX - 40; 
            int inputWidth = Math.Min(maxInputWidth, availableWidth);
            
            if (inputWidth < 150) inputWidth = 150; 
            
            txt_Erabiltzailea.Width = inputWidth;
            txt_Pasahitza.Width = inputWidth;
            btn_Sartu.Width = inputWidth;

            int totalBlockHeight = 
                lbl_Erabiltzailea.Height + 
                verticalGap + 
                lbl_Pasahitza.Height + 
                verticalGap + 
                btn_Sartu.Height; 
            
            int row1Height = Math.Max(lbl_Erabiltzailea.Height, txt_Erabiltzailea.Height);
            int row2Height = Math.Max(lbl_Pasahitza.Height, txt_Pasahitza.Height);
            int row3Height = btn_Sartu.Height;

            int totalContentHeight = row1Height + verticalGap + row2Height + verticalGap + row3Height;
            int startY = (this.ClientSize.Height - totalContentHeight) / 2;

            lbl_Erabiltzailea.Location = new Point(axisX - lbl_Erabiltzailea.Width - 20, startY + (row1Height - lbl_Erabiltzailea.Height) / 2);
            txt_Erabiltzailea.Location = new Point(axisX, startY + (row1Height - txt_Erabiltzailea.Height) / 2);

            int row2Y = startY + row1Height + verticalGap;
            lbl_Pasahitza.Location = new Point(axisX - lbl_Pasahitza.Width - 20, row2Y + (row2Height - lbl_Pasahitza.Height) / 2);
            txt_Pasahitza.Location = new Point(axisX, row2Y + (row2Height - txt_Pasahitza.Height) / 2);

            int row3Y = row2Y + row2Height + verticalGap + 20;
            btn_Sartu.Location = new Point(axisX, row3Y);
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
    }
}
