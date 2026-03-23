using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using TeknoBideTPV.Zerbitzuak;

namespace TeknoBideTPV.Txata
{
    public partial class TxatPantaila : UserControl
    {
        private string erabiltzaileIzena = SesioZerbitzua.Izena + " txat-ean sartu da!";
        private TcpClient erabiltzailea;
        private StreamReader irakurlea;
        private StreamWriter idazlea;
        private Thread entzunHari;

        public TxatPantaila(string erabiltzaileIzena)
        {
            InitializeComponent();
            this.erabiltzaileIzena = erabiltzaileIzena;

            this.Resize += TxatPantaila_Resize;
        }

        public TxatPantaila()
        {
            InitializeComponent();
            this.Load += TxatPantaila_Load;

            this.Resize += TxatPantaila_Resize;
        }

        private void TxatPantaila_Load(object sender, EventArgs e)
        {
            konexioaKargatu();
        }

        private void konexioaKargatu()
        {
            try
            {
                erabiltzailea = new TcpClient("192.168.1.112", 5555);

                irakurlea = new StreamReader(erabiltzailea.GetStream());
                idazlea = new StreamWriter(erabiltzailea.GetStream());
                idazlea.AutoFlush = true;

                idazlea.WriteLine(erabiltzaileIzena);

                entzunHari = new Thread(entzunBuklea);
                entzunHari.IsBackground = true;
                entzunHari.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errorea zerbitzarira konektatzean: " + ex.Message);
            }
        }

        private void entzunBuklea()
        {
            try
            {
                string lerroa;
                while ((lerroa = irakurlea.ReadLine()) != null)
                {
                    idatziMezua(lerroa);
                }
            }
            catch
            {

            }
        }

        private void idatziMezua(string msg)
        {
            bool NireMezua = msg.StartsWith(SesioZerbitzua.Izena + ":");

            if (MezuPantaila.InvokeRequired)
            {
                MezuPantaila.Invoke(new Action(() =>
                    GehituRTFBorde(msg, NireMezua)
                ));
            }
            else
            {
                GehituRTFBorde(msg, NireMezua);
            }
        }

        private void GehituRTFBorde(string msg, bool NireMezua)
        {
            string alineacion = NireMezua ? @"\qr" : @"\ql";

            Color fondo = NireMezua
                ? ColorTranslator.FromHtml("#E65100")
                : ColorTranslator.FromHtml("#F57C00");

            string rtf = @"{\rtf1\ansi
{\pard" + alineacion + @" 
\box " + msg.Replace("\n", "\\line ") + @"\par}
}";

            MezuPantaila.SelectionBackColor = fondo;
            MezuPantaila.SelectedRtf = rtf;

            MezuPantaila.AppendText("\n");
        }

        private void BidaliBotoia_Click(object sender, EventArgs e)
        {
            string mezua = MezuIdazlea.Text.Trim();
            if (mezua == "") return;

            idazlea.WriteLine(SesioZerbitzua.Izena + ":  " + mezua);
            MezuIdazlea.Text = "";
        }


        private void TxatPantaila_Resize(object sender, EventArgs e)
        {
            int w = this.Width;
            int h = this.Height;

            MezuPantaila.Location = new Point((int)(w * 0.02), (int)(h * 0.02));
            MezuPantaila.Size = new Size((int)(w * 0.96), (int)(h * 0.80));

            MezuIdazlea.Location = new Point((int)(w * 0.02), (int)(h * 0.84));
            MezuIdazlea.Size = new Size((int)(w * 0.75), (int)(h * 0.12));

            BidaliBotoia.Location = new Point((int)(w * 0.79), (int)(h * 0.84));
            BidaliBotoia.Size = new Size((int)(w * 0.19), (int)(h * 0.12));
        }

        private void TxatPantaila_Load_1(object sender, EventArgs e)
        {

        }

        private void MezuPantaila_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
