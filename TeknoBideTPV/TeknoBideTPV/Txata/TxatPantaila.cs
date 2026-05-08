using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;
using TeknoBideTPV.Zerbitzuak;

namespace TeknoBideTPV.Txata
{
    public partial class TxatPantaila : UserControl
    {
        private string erabiltzaileIzena = SesioZerbitzua.Izena + " txat-ean sartu da!";
        private TcpClient? erabiltzailea;
        private BinaryReader? irakurlea;
        private BinaryWriter? idazlea;
        private AesGcm? aes;
        private Thread? entzunHari;
        private volatile bool _konektatuta;
        private bool _konexioaHasita;
        private readonly object _idazleaLock = new object();
        private readonly object _bidaliakLock = new object();
        private readonly Dictionary<string, DateTime> _bidaliakBerriki = new Dictionary<string, DateTime>();
        private readonly Dictionary<Guid, JasotzenDenFitxategia> _fitxategiak = new Dictionary<Guid, JasotzenDenFitxategia>();
        private readonly TimeSpan _bikoizketaLeihoa = TimeSpan.FromSeconds(5);
        private const int FitxategiBufferTamaina = 16 * 1024;
        private const long GehienezkoFitxategiTamaina = 100L * 1024L * 1024L;

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
                    "192.168.10.5",
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
                irakurlea = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);
                idazlea = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true);

                using RSA rsa = RSA.Create(2048);
                string pem = rsa.ExportSubjectPublicKeyInfoPem();
                BidaliPaketea(PaketeMota.Hello, Encoding.UTF8.GetBytes(pem));

                var keyPacket = IrakurriPaketea();
                if (keyPacket == null || keyPacket.Value.Mota != PaketeMota.Gakoa)
                    throw new IOException("Txat zerbitzariak ez du gakoa bidali.");

                byte[] aesKey = rsa.Decrypt(keyPacket.Value.Payload, RSAEncryptionPadding.OaepSHA256);
                aes = new AesGcm(aesKey, tagSizeInBytes: 16);

                _konektatuta = true;
                BidaliTestua(erabiltzaileIzena);

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
                while (_konektatuta && irakurlea != null)
                {
                    var paketea = IrakurriPaketea();
                    if (paketea == null) break;

                    switch (paketea.Value.Mota)
                    {
                        case PaketeMota.Testua:
                            if (aes != null)
                            {
                                string lerroa = DesenkriptatuTestua(aes, paketea.Value.Payload);
                                idatziMezua(lerroa);
                            }
                            break;

                        case PaketeMota.FitxategiHasiera:
                            KudeatuFitxategiHasiera(paketea.Value.Payload);
                            break;

                        case PaketeMota.FitxategiZatia:
                            KudeatuFitxategiZatia(paketea.Value.Payload);
                            break;

                        case PaketeMota.FitxategiAmaiera:
                            KudeatuFitxategiAmaiera(paketea.Value.Payload);
                            break;
                    }
                }
            }
            catch
            {
                _konektatuta = false;
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
            Color fondo = NireMezua
                ? System.Drawing.ColorTranslator.FromHtml("#E0E7FF")
                : System.Drawing.ColorTranslator.FromHtml("#DBEAFE");

            MezuPantaila.SelectionAlignment = NireMezua ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            MezuPantaila.SelectionBackColor = fondo;
            MezuPantaila.SelectedText = msg + Environment.NewLine + Environment.NewLine;
            MezuPantaila.SelectionBackColor = MezuPantaila.BackColor;
        }

        private void BidaliBotoia_Click(object sender, EventArgs e)
        {
            string mezua = MezuIdazlea.Text.Trim();
            if (mezua == "") return;

            if (!_konektatuta || idazlea == null || aes == null)
            {
                MessageBox.Show("Ez zaude txatera konektatuta.", "Txata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var lerroa = MezuaNormalizatu(SesioZerbitzua.Izena + ": " + mezua);
                BidaliTestua(lerroa);
                idatziMezua(lerroa);
                MezuIdazlea.Text = "";
            }
            catch (Exception ex)
            {
                _konektatuta = false;
                MessageBox.Show("Ezin izan da mezua bidali: " + ex.Message, "Txata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void FitxategiaBotoia_Click(object sender, EventArgs e)
        {
            if (!_konektatuta || idazlea == null || aes == null)
            {
                MessageBox.Show("Ez zaude txatera konektatuta.", "Txata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new OpenFileDialog
            {
                Title = "Aukeratu bidaltzeko fitxategia",
                Filter = "Fitxategi guztiak (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            try
            {
                FitxategiaBotoia.Enabled = false;
                await Task.Run(() => BidaliFitxategia(dialog.FileName));

                string izena = Path.GetFileName(dialog.FileName);
                idatziMezua($"{SesioZerbitzua.Izena}: [FITXATEGIA] {izena}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ezin izan da fitxategia bidali: " + ex.Message, "Txata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                FitxategiaBotoia.Enabled = true;
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
            var kentzeko = new List<string>();
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
                aes?.Dispose();
                aes = null;
                ItxiFitxategiIrekiak();
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
            MezuIdazlea.Size = new Size((int)(w * 0.55), (int)(h * 0.12));

            FitxategiaBotoia.Location = new Point((int)(w * 0.59), (int)(h * 0.84));
            FitxategiaBotoia.Size = new Size((int)(w * 0.18), (int)(h * 0.12));

            BidaliBotoia.Location = new Point((int)(w * 0.79), (int)(h * 0.84));
            BidaliBotoia.Size = new Size((int)(w * 0.19), (int)(h * 0.12));
        }

        private void TxatPantaila_Load_1(object sender, EventArgs e)
        {

        }

        private void MezuPantaila_TextChanged(object sender, EventArgs e)
        {

        }

        private void MezuPantaila_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int lineIndex = MezuPantaila.GetLineFromCharIndex(MezuPantaila.SelectionStart);
                if (lineIndex < 0 || lineIndex >= MezuPantaila.Lines.Length) return;

                string line = MezuPantaila.Lines[lineIndex];
                const string marker = "Ireki: ";
                int idx = line.IndexOf(marker, StringComparison.Ordinal);
                if (idx < 0) return;

                string bidea = line.Substring(idx + marker.Length).Trim();
                if (!File.Exists(bidea))
                {
                    MessageBox.Show("Fitxategia ez da aurkitu: " + bidea, "Txata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = bidea,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ezin izan da fitxategia ireki: " + ex.Message, "Txata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private enum PaketeMota : byte
        {
            Hello = 1,
            Gakoa = 2,
            Testua = 3,
            FitxategiHasiera = 4,
            FitxategiZatia = 5,
            FitxategiAmaiera = 6
        }

        private void BidaliTestua(string testua)
        {
            if (aes == null) throw new InvalidOperationException("AES gakoa ez dago prest.");
            byte[] payload = EnkriptatuTestua(aes, testua);
            BidaliPaketea(PaketeMota.Testua, payload);
        }

        private void BidaliPaketea(PaketeMota mota, byte[] payload)
        {
            if (idazlea == null) throw new InvalidOperationException("Txat idazlea ez dago prest.");
            lock (_idazleaLock)
            {
                idazlea.Write((byte)mota);
                idazlea.Write(payload.Length);
                idazlea.Write(payload);
                idazlea.Flush();
            }
        }

        private void BidaliFitxategia(string bidea)
        {
            if (!File.Exists(bidea))
                throw new FileNotFoundException("Fitxategia ez da aurkitu.", bidea);

            var info = new FileInfo(bidea);
            if (info.Length > GehienezkoFitxategiTamaina)
                throw new InvalidOperationException("Fitxategia handiegia da.");

            Guid id = Guid.NewGuid();
            byte[] startPayload = SortuFitxategiHasieraPayload(id, Path.GetFileName(bidea), info.Length);
            BidaliPaketea(PaketeMota.FitxategiHasiera, startPayload);

            byte[] buffer = new byte[FitxategiBufferTamaina];
            using FileStream fs = File.OpenRead(bidea);
            int read;
            while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                byte[] chunkPayload = new byte[16 + read];
                Buffer.BlockCopy(id.ToByteArray(), 0, chunkPayload, 0, 16);
                Buffer.BlockCopy(buffer, 0, chunkPayload, 16, read);
                BidaliPaketea(PaketeMota.FitxategiZatia, chunkPayload);
            }

            BidaliPaketea(PaketeMota.FitxategiAmaiera, id.ToByteArray());
        }

        private static byte[] SortuFitxategiHasieraPayload(Guid id, string izena, long tamaina)
        {
            byte[] nameBytes = Encoding.UTF8.GetBytes(izena);
            using MemoryStream ms = new MemoryStream();
            using BinaryWriter bw = new BinaryWriter(ms, Encoding.UTF8, leaveOpen: true);
            bw.Write(id.ToByteArray());
            bw.Write(nameBytes.Length);
            bw.Write(nameBytes);
            bw.Write(tamaina);
            bw.Flush();
            return ms.ToArray();
        }

        private (PaketeMota Mota, byte[] Payload)? IrakurriPaketea()
        {
            if (irakurlea == null) return null;

            int motaByte;
            try
            {
                motaByte = irakurlea.ReadByte();
            }
            catch (EndOfStreamException)
            {
                return null;
            }

            var mota = (PaketeMota)motaByte;
            int luzera = irakurlea.ReadInt32();
            if (luzera < 0 || luzera > 10 * 1024 * 1024)
                throw new InvalidDataException("Pakete luzera baliogabea.");

            byte[] payload = irakurlea.ReadBytes(luzera);
            return payload.Length == luzera ? (mota, payload) : null;
        }

        private static byte[] EnkriptatuTestua(AesGcm aes, string testua)
        {
            byte[] plaintext = Encoding.UTF8.GetBytes(testua);
            byte[] nonce = new byte[12];
            byte[] tag = new byte[16];
            byte[] ciphertext = new byte[plaintext.Length];
            RandomNumberGenerator.Fill(nonce);
            aes.Encrypt(nonce, plaintext, ciphertext, tag);

            byte[] payload = new byte[nonce.Length + tag.Length + ciphertext.Length];
            Buffer.BlockCopy(nonce, 0, payload, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, payload, nonce.Length, tag.Length);
            Buffer.BlockCopy(ciphertext, 0, payload, nonce.Length + tag.Length, ciphertext.Length);
            return payload;
        }

        private static string DesenkriptatuTestua(AesGcm aes, byte[] payload)
        {
            if (payload.Length < 28) throw new InvalidDataException("Payload laburregia.");

            byte[] nonce = new byte[12];
            byte[] tag = new byte[16];
            int cipherLen = payload.Length - nonce.Length - tag.Length;
            byte[] ciphertext = new byte[cipherLen];

            Buffer.BlockCopy(payload, 0, nonce, 0, nonce.Length);
            Buffer.BlockCopy(payload, nonce.Length, tag, 0, tag.Length);
            Buffer.BlockCopy(payload, nonce.Length + tag.Length, ciphertext, 0, ciphertext.Length);

            byte[] plaintext = new byte[cipherLen];
            aes.Decrypt(nonce, ciphertext, tag, plaintext);
            return Encoding.UTF8.GetString(plaintext);
        }

        private void KudeatuFitxategiHasiera(byte[] payload)
        {
            try
            {
                if (payload.Length < 16 + 4 + 8) return;

                var span = payload.AsSpan();
                Guid id = new Guid(span.Slice(0, 16));
                int offset = 16;
                int izenLen = BitConverter.ToInt32(span.Slice(offset, 4));
                offset += 4;
                if (izenLen < 0 || payload.Length < offset + izenLen + 8) return;

                string izena = Encoding.UTF8.GetString(span.Slice(offset, izenLen));
                offset += izenLen;
                long tamaina = BitConverter.ToInt64(span.Slice(offset, 8));

                string bidea = SortuDeskargaBidea(izena);
                var fs = new FileStream(bidea, FileMode.Create, FileAccess.Write, FileShare.Read);

                lock (_fitxategiak)
                {
                    if (_fitxategiak.TryGetValue(id, out var zaharra))
                    {
                        try { zaharra.Stream.Dispose(); } catch { }
                    }
                    _fitxategiak[id] = new JasotzenDenFitxategia(fs, Path.GetFileName(bidea), bidea, tamaina);
                }

                idatziMezua($"Fitxategia jasotzen: {Path.GetFileName(bidea)}");
            }
            catch (Exception ex)
            {
                idatziMezua("Ezin izan da fitxategia jasotzen hasi: " + ex.Message);
            }
        }

        private void KudeatuFitxategiZatia(byte[] payload)
        {
            try
            {
                if (payload.Length < 16) return;
                Guid id = new Guid(payload.AsSpan(0, 16));

                JasotzenDenFitxategia? fitxategia;
                lock (_fitxategiak)
                {
                    if (!_fitxategiak.TryGetValue(id, out fitxategia)) return;
                }

                fitxategia.Stream.Write(payload, 16, payload.Length - 16);
                fitxategia.Stream.Flush();
            }
            catch (Exception ex)
            {
                idatziMezua("Errorea fitxategi zatia jasotzean: " + ex.Message);
            }
        }

        private void KudeatuFitxategiAmaiera(byte[] payload)
        {
            try
            {
                if (payload.Length < 16) return;
                Guid id = new Guid(payload.AsSpan(0, 16));

                JasotzenDenFitxategia? fitxategia = null;
                lock (_fitxategiak)
                {
                    if (_fitxategiak.TryGetValue(id, out fitxategia))
                        _fitxategiak.Remove(id);
                }

                if (fitxategia == null) return;
                fitxategia.Stream.Dispose();
                idatziMezua($"Fitxategia jasota: {fitxategia.Izena}{Environment.NewLine}Ireki: {fitxategia.Bidea}");
            }
            catch (Exception ex)
            {
                idatziMezua("Errorea fitxategia bukatzean: " + ex.Message);
            }
        }

        private static string SortuDeskargaBidea(string izena)
        {
            string garbia = GarbituFitxategiIzena(izena);
            if (string.IsNullOrWhiteSpace(garbia))
                garbia = "fitxategia";

            string karpeta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads",
                "TeknoBideTPV-Txata");
            Directory.CreateDirectory(karpeta);

            string bidea = Path.Combine(karpeta, garbia);
            if (!File.Exists(bidea)) return bidea;

            string izenikGabe = Path.GetFileNameWithoutExtension(garbia);
            string luzapena = Path.GetExtension(garbia);
            return Path.Combine(karpeta, $"{izenikGabe}_{DateTime.Now:yyyyMMdd_HHmmssfff}{luzapena}");
        }

        private static string GarbituFitxategiIzena(string izena)
        {
            string garbia = Path.GetFileName(izena ?? string.Empty);
            foreach (char c in Path.GetInvalidFileNameChars())
                garbia = garbia.Replace(c, '_');
            return garbia;
        }

        private void ItxiFitxategiIrekiak()
        {
            lock (_fitxategiak)
            {
                foreach (var fitxategia in _fitxategiak.Values)
                {
                    try { fitxategia.Stream.Dispose(); } catch { }
                }
                _fitxategiak.Clear();
            }
        }

        private sealed class JasotzenDenFitxategia
        {
            public FileStream Stream { get; }
            public string Izena { get; }
            public string Bidea { get; }
            public long Tamaina { get; }

            public JasotzenDenFitxategia(FileStream stream, string izena, string bidea, long tamaina)
            {
                Stream = stream;
                Izena = izena;
                Bidea = bidea;
                Tamaina = tamaina;
            }
        }
    }
}
