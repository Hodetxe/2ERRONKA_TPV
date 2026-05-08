using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TeknoBideTPV.Zerbitzuak
{
    public class OdooZerbitzua
    {
        private const string DefaultOdooBaseUrl = "http://192.168.10.5:8069";
        private static readonly HttpClient _http = new HttpClient();

        private readonly string? _baseUrl;
        private readonly string? _token;

        public OdooZerbitzua(string? baseUrl = null, string? token = null)
        {
            var envBaseUrl = Environment.GetEnvironmentVariable("TPV_ODOO_BASE_URL");
            var ezarpenak = TpvEzarpenakZerbitzua.Kargatu();
            var gordetakoBaseUrl = NormalizatuBaseUrl(ezarpenak.OdooBaseUrl);
            _baseUrl = string.IsNullOrWhiteSpace(baseUrl)
                ? (
                    !string.IsNullOrWhiteSpace(envBaseUrl)
                        ? envBaseUrl
                        : (!string.IsNullOrWhiteSpace(gordetakoBaseUrl) ? gordetakoBaseUrl : DefaultOdooBaseUrl)
                  )
                : baseUrl;

            var envToken = Environment.GetEnvironmentVariable("TPV_ODOO_TOKEN");
            _token = string.IsNullOrWhiteSpace(token)
                ? (!string.IsNullOrWhiteSpace(envToken) ? envToken : ezarpenak.OdooToken)
                : token;
        }

        private static string? NormalizatuBaseUrl(string? baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl)) return null;

            var normalizatua = baseUrl.Trim().TrimEnd('/');
            return normalizatua.Equals("http://localhost:8069", StringComparison.OrdinalIgnoreCase) ||
                   normalizatua.Equals("http://127.0.0.1:8069", StringComparison.OrdinalIgnoreCase)
                ? null
                : normalizatua;
        }

        public bool KonfiguratutaDago => !string.IsNullOrWhiteSpace(_baseUrl);

        public async Task<OdooDeskontuKalkuluEmaitza> KalkulatuDeskontuaAsync(string kodea, double guztiraBruto)
        {
            kodea = (kodea ?? string.Empty).Trim();
            if (kodea.Length == 0)
                return OdooDeskontuKalkuluEmaitza.Baliogabea("Kodea hutsik dago.");

            if (guztiraBruto < 0)
                return OdooDeskontuKalkuluEmaitza.Baliogabea("Guztira ez da baliozkoa.");

            if (!KonfiguratutaDago)
                return OdooDeskontuKalkuluEmaitza.KonfiguratuGabea("Odoo ez dago konfiguratuta (TPV_ODOO_BASE_URL).");

            try
            {
                using var cts = new CancellationTokenSource(3500);
                var baseUrl = _baseUrl!.TrimEnd('/');

                var url = $"{baseUrl}/api/jatetxeko/deskontuak/kalkulatu";
                using var req = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = JsonContent.Create(new { kodea, guztira_bruto = guztiraBruto })
                };

                if (!string.IsNullOrWhiteSpace(_token))
                    req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                using var resp = await _http.SendAsync(req, cts.Token);
                var body = await resp.Content.ReadAsStringAsync(cts.Token);
                if (!resp.IsSuccessStatusCode)
                    return OdooDeskontuKalkuluEmaitza.Baliogabea($"Odoo errorea: {(int)resp.StatusCode}");

                return ParseatuKalkulua(body, kodea, guztiraBruto);
            }
            catch (Exception ex)
            {
                return OdooDeskontuKalkuluEmaitza.Errorea($"Errorea Odoo-ra konektatzean: {ex.Message}");
            }
        }

        public async Task<OdooDeskontuEmaitza> BalidatuDeskontuKodeaAsync(string kodea)
        {
            kodea = (kodea ?? string.Empty).Trim();
            if (kodea.Length == 0)
                return OdooDeskontuEmaitza.Baliogabea("Kodea hutsik dago.");

            if (!KonfiguratutaDago)
                return OdooDeskontuEmaitza.KonfiguratuGabea("Odoo ez dago konfiguratuta (TPV_ODOO_BASE_URL).");

            try
            {
                using var cts = new CancellationTokenSource(2500);
                var baseUrl = _baseUrl!.TrimEnd('/');

                var eskaerak = new[]
                {
                    $"{baseUrl}/api/deskontua?kodea={Uri.EscapeDataString(kodea)}",
                    $"{baseUrl}/api/discount?code={Uri.EscapeDataString(kodea)}"
                };

                foreach (var url in eskaerak)
                {
                    using var req = new HttpRequestMessage(HttpMethod.Get, url);
                    if (!string.IsNullOrWhiteSpace(_token))
                        req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                    using var resp = await _http.SendAsync(req, cts.Token);
                    if (!resp.IsSuccessStatusCode)
                        continue;

                    var body = await resp.Content.ReadAsStringAsync(cts.Token);
                    var emaitza = Parseatu(body);
                    if (emaitza.Ok)
                        return emaitza;
                }

                return OdooDeskontuEmaitza.Baliogabea("Kodea ez da existitzen edo ez da baliozkoa.");
            }
            catch (Exception ex)
            {
                return OdooDeskontuEmaitza.Errorea($"Errorea Odoo-ra konektatzean: {ex.Message}");
            }
        }

        private static OdooDeskontuEmaitza Parseatu(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
                return OdooDeskontuEmaitza.Baliogabea("Erantzuna hutsik dago.");

            try
            {
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("valid", out var validEl) && validEl.ValueKind == JsonValueKind.True)
                    {
                        if (root.TryGetProperty("percent", out var pEl) && pEl.TryGetDouble(out var p))
                            return OdooDeskontuEmaitza.Ona(p);

                        if (root.TryGetProperty("discountPercent", out var p2El) && p2El.TryGetDouble(out var p2))
                            return OdooDeskontuEmaitza.Ona(p2);
                    }

                    if (root.TryGetProperty("percent", out var percentEl) && percentEl.TryGetDouble(out var percent))
                    {
                        if (percent > 0)
                            return OdooDeskontuEmaitza.Ona(percent);
                    }
                }
            }
            catch
            {
            }

            return OdooDeskontuEmaitza.Baliogabea("Odoo erantzuna ez da ulertu.");
        }

        private static OdooDeskontuKalkuluEmaitza ParseatuKalkulua(string body, string kodea, double guztiraBruto)
        {
            if (string.IsNullOrWhiteSpace(body))
                return OdooDeskontuKalkuluEmaitza.Baliogabea("Erantzuna hutsik dago.");

            try
            {
                using var doc = JsonDocument.Parse(body);
                var root = doc.RootElement;
                if (root.ValueKind != JsonValueKind.Object)
                    return OdooDeskontuKalkuluEmaitza.Baliogabea("Odoo erantzuna ez da ulertu.");

                if (root.TryGetProperty("existitzen_da", out var existitzenDaEl) &&
                    existitzenDaEl.ValueKind == JsonValueKind.False)
                {
                    return OdooDeskontuKalkuluEmaitza.Baliogabea("Kodea ez da existitzen edo ez dago aktibo.");
                }

                var mota = root.TryGetProperty("mota", out var motaEl) ? motaEl.GetString() ?? string.Empty : string.Empty;
                var balioa = root.TryGetProperty("balioa", out var balioaEl) && balioaEl.TryGetDouble(out var b) ? b : 0.0;
                var deskontuKopurua = root.TryGetProperty("deskontu_kopurua", out var deskEl) && deskEl.TryGetDouble(out var d) ? d : 0.0;
                var guztira = root.TryGetProperty("guztira", out var guztiraEl) && guztiraEl.TryGetDouble(out var g) ? g : guztiraBruto;
                var bruto = root.TryGetProperty("guztira_bruto", out var brutoEl) && brutoEl.TryGetDouble(out var gb) ? gb : guztiraBruto;
                var kodeaErantzuna = root.TryGetProperty("kodea", out var kodeaEl) ? (kodeaEl.GetString() ?? kodea) : kodea;

                // Defensive clamp to avoid negative totals due to external rounding issues.
                if (deskontuKopurua < 0) deskontuKopurua = 0;
                if (guztira < 0) guztira = 0;
                if (deskontuKopurua > bruto) deskontuKopurua = bruto;

                return OdooDeskontuKalkuluEmaitza.Ona(
                    kodeaErantzuna,
                    mota,
                    balioa,
                    bruto,
                    deskontuKopurua,
                    guztira);
            }
            catch
            {
                return OdooDeskontuKalkuluEmaitza.Baliogabea("Odoo erantzuna ez da ulertu.");
            }
        }
    }

    public class OdooDeskontuEmaitza
    {
        public bool Ok { get; private set; }
        public bool KonfiguratuGabe { get; private set; }
        public string Mezua { get; private set; } = string.Empty;
        public double? Ehunekoa { get; private set; }

        public static OdooDeskontuEmaitza Ona(double ehunekoa)
        {
            if (ehunekoa < 0) ehunekoa = 0;
            if (ehunekoa > 100) ehunekoa = 100;

            return new OdooDeskontuEmaitza
            {
                Ok = true,
                Ehunekoa = ehunekoa,
                Mezua = $"Deskontua aurkituta: -{ehunekoa:0.##}%"
            };
        }

        public static OdooDeskontuEmaitza Baliogabea(string mezua) =>
            new OdooDeskontuEmaitza { Ok = false, KonfiguratuGabe = false, Mezua = mezua };

        public static OdooDeskontuEmaitza KonfiguratuGabea(string mezua) =>
            new OdooDeskontuEmaitza { Ok = false, KonfiguratuGabe = true, Mezua = mezua };

        public static OdooDeskontuEmaitza Errorea(string mezua) =>
            new OdooDeskontuEmaitza { Ok = false, KonfiguratuGabe = false, Mezua = mezua };
    }

    public class OdooDeskontuKalkuluEmaitza
    {
        public bool Ok { get; private set; }
        public bool KonfiguratuGabe { get; private set; }
        public string Mezua { get; private set; } = string.Empty;
        public string Kodea { get; private set; } = string.Empty;
        public string Mota { get; private set; } = string.Empty;
        public double Balioa { get; private set; }
        public double GuztiraBruto { get; private set; }
        public double DeskontuKopurua { get; private set; }
        public double Guztira { get; private set; }

        public static OdooDeskontuKalkuluEmaitza Ona(
            string kodea,
            string mota,
            double balioa,
            double guztiraBruto,
            double deskontuKopurua,
            double guztira)
        {
            var motaNorm = (mota ?? string.Empty).Trim().ToLowerInvariant();
            var mezua = motaNorm == "ehunekoa"
                ? $"Deskontua aplikatuta: -{balioa:0.##}%"
                : $"Deskontua aplikatuta: -{deskontuKopurua:0.00}€";

            return new OdooDeskontuKalkuluEmaitza
            {
                Ok = true,
                Kodea = kodea ?? string.Empty,
                Mota = motaNorm,
                Balioa = balioa,
                GuztiraBruto = guztiraBruto,
                DeskontuKopurua = deskontuKopurua,
                Guztira = guztira,
                Mezua = mezua
            };
        }

        public static OdooDeskontuKalkuluEmaitza Baliogabea(string mezua) =>
            new OdooDeskontuKalkuluEmaitza { Ok = false, KonfiguratuGabe = false, Mezua = mezua };

        public static OdooDeskontuKalkuluEmaitza KonfiguratuGabea(string mezua) =>
            new OdooDeskontuKalkuluEmaitza { Ok = false, KonfiguratuGabe = true, Mezua = mezua };

        public static OdooDeskontuKalkuluEmaitza Errorea(string mezua) =>
            new OdooDeskontuKalkuluEmaitza { Ok = false, KonfiguratuGabe = false, Mezua = mezua };
    }
}

