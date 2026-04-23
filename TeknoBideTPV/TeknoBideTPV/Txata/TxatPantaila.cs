using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using TeknoBideTPV.Zerbitzuak;

namespace TeknoBideTPV.Txata
{
    public partial class TxatPantaila : UserControl
    {
        private string erabiltzaileIzena = SesioZerbitzua.Izena + " txat-ean sartu da!";
        private TcpClient? erabiltzailea;
        private StreamReader? irakurlea;
        private StreamWriter? idazlea;
        private Thread? entzunHari;
        private volatile bool _konektatuta;
        private bool _konexioaHasita;
        private readonly object _bidaliakLock = new object();
        private readonly System.Collections.Generic.Dictionary<string, DateTime> _bidaliakBerriki = new System.Collections.Generic.Dictionary<string, DateTime>();
        private readonly TimeSpan _bikoizketaLeihoa = TimeSpan.FromSeconds(5);

        public TxatPantaila(string erabiltzaileIzena)
        {
            InitializeComponent();
            this.erabiltzaileIzena = erabiltzaileIzena;

            this.Resize += TxatPantaila_Resize;
            this.Disposed += (_, __) => GarbituKonexioa();
        }

        public TxatPantaila()
        {
            InitializeComponent();
            this.Resize += TxatPantaila_Resize;
            MezuIdazlea.KeyDown += MezuIdazlea_KeyDown;
            this.Disposed += (_, __) => GarbituKonexioa();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TPVEstiloa.ProfesionalizatuKontrolak(this);
            if (_konexioaHasita) return;
            _konexioaHasita = true;
            _ = KonexioaKargatuAsync();
        }

        private async Task KonexioaKargatuAsync()
        {
            try
            {
                var helbideak = new[]
                {
                    "127.0.0.1",
                    "localhost",
                    "192.168.1.112"
                };

                TcpClient? konektatua = null;
                foreach (var helbidea in helbideak)
                {
                    konektatua = await SaiatuKonektatzenAsync(helbidea, 5555, 2000);
                    if (konektatua != null) break;
                }

                if (konektatua == null)
                {
                    MessageBox.Show("Ezin izan da txat zerbitzarira konektatu (5555).", "Txata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                erabiltzailea = konektatua;
                var stream = erabiltzailea.GetStream();
                irakurlea = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
                idazlea = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true)
                {
                    AutoFlush = true
                };

                _konektatuta = true;
                idazlea.WriteLine(erabiltzaileIzena);

                entzunHari = new Thread(EntzunBuklea);
                entzunHari.IsBackground = true;
                entzunHari.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errorea zerbitzarira konektatzean: " + ex.Message, "Txata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async Task<TcpClient?> SaiatuKonektatzenAsync(string helbidea, int portua, int timeoutMs)
        {
            try
            {
                var client = new TcpClient();
                using var cts = new CancellationTokenSource(timeoutMs);
                await client.ConnectAsync(helbidea, portua, cts.Token);
                return client;
            }
            catch
            {
                return null;
            }
        }

        private void EntzunBuklea()
        {
            try
            {
                string lerroa;
                while (_konektatuta && irakurlea != null && (lerroa = irakurlea.ReadLine()) != null)
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
            msg = MezuaNormalizatu(msg);
            bool NireMezua = msg.StartsWith(SesioZerbitzua.Izena + ":", StringComparison.Ordinal);

            if (NireMezua && BidalitakoBikoizketaDa(msg))
                return;

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
                ? System.Drawing.ColorTranslator.FromHtml("#E0E7FF")
                : System.Drawing.ColorTranslator.FromHtml("#DBEAFE");

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

            if (!_konektatuta || idazlea == null)
            {
                MessageBox.Show("Ez zaude txatera konektatuta.", "Txata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var lerroa = MezuaNormalizatu(SesioZerbitzua.Izena + ": " + mezua);
                MarkatuBidalia(lerroa);
                idazlea.WriteLine(lerroa);
                idatziMezua(lerroa);
                MezuIdazlea.Text = "";
            }
            catch (Exception ex)
            {
                _konektatuta = false;
                MessageBox.Show("Ezin izan da mezua bidali: " + ex.Message, "Txata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string MezuaNormalizatu(string msg)
        {
            msg = msg.Trim();
            int idx = msg.IndexOf(':');
            if (idx < 0)
                return msg;

            string prefix = msg.Substring(0, idx + 1);
            string rest = msg.Substring(idx + 1).TrimStart();
            return rest.Length == 0 ? prefix : prefix + " " + rest;
        }

        private void MarkatuBidalia(string msg)
        {
            lock (_bidaliakLock)
            {
                GarbituBidaliak();
                _bidaliakBerriki[msg] = DateTime.UtcNow;
            }
        }

        private bool BidalitakoBikoizketaDa(string msg)
        {
            lock (_bidaliakLock)
            {
                GarbituBidaliak();
                if (_bidaliakBerriki.TryGetValue(msg, out var noiz))
                {
                    if (DateTime.UtcNow - noiz <= _bikoizketaLeihoa)
                    {
                        _bidaliakBerriki.Remove(msg);
                        return true;
                    }
                    _bidaliakBerriki.Remove(msg);
                }
                return false;
            }
        }

        private void GarbituBidaliak()
        {
            var orain = DateTime.UtcNow;
            var kentzeko = new System.Collections.Generic.List<string>();
            foreach (var kvp in _bidaliakBerriki)
            {
                if (orain - kvp.Value > _bikoizketaLeihoa)
                    kentzeko.Add(kvp.Key);
            }
            foreach (var k in kentzeko)
                _bidaliakBerriki.Remove(k);
        }

        private void MezuIdazlea_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !e.Shift)
            {
                e.SuppressKeyPress = true;
                BidaliBotoia.PerformClick();
            }
        }

        private void GarbituKonexioa()
        {
            try
            {
                _konektatuta = false;
                erabiltzailea?.Close();
                irakurlea?.Dispose();
                idazlea?.Dispose();
            }
            catch { }
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
