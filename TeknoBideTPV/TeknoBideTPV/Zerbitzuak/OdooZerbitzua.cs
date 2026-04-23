using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace TeknoBideTPV.Zerbitzuak
{
    public class OdooZerbitzua
    {
        private static readonly HttpClient _http = new HttpClient();

        private readonly string? _baseUrl;
        private readonly string? _token;

        public OdooZerbitzua(string? baseUrl = null, string? token = null)
        {
            _baseUrl = string.IsNullOrWhiteSpace(baseUrl)
                ? Environment.GetEnvironmentVariable("TPV_ODOO_BASE_URL")
                : baseUrl;

            _token = string.IsNullOrWhiteSpace(token)
                ? Environment.GetEnvironmentVariable("TPV_ODOO_TOKEN")
                : token;
        }

        public bool KonfiguratutaDago => !string.IsNullOrWhiteSpace(_baseUrl);

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
}

