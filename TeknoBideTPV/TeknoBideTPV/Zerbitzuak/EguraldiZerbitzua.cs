using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TeknoBideTPV.Zerbitzuak
{
    public class EguraldiZerbitzua
    {
        private static readonly HttpClient _http = new HttpClient();
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        private EguraldiEmaitza? _cache;
        private DateTime _cacheNoiz;

        public string Iturria { get; }
        public TimeSpan CacheDenbora { get; }

        public EguraldiZerbitzua(string? iturria = null, TimeSpan? cacheDenbora = null)
        {
            Iturria = string.IsNullOrWhiteSpace(iturria)
                ? (Environment.GetEnvironmentVariable("TPV_EGURALDI_XML") ?? DefaultIturria())
                : iturria;

            CacheDenbora = cacheDenbora ?? TimeSpan.FromMinutes(60);
        }

        public async Task<EguraldiaInfo?> LortuAsync()
        {
            var emaitza = await LortuEmaitzaAsync();
            return emaitza?.Info;
        }

        public async Task<EguraldiEmaitza?> LortuEmaitzaAsync()
        {
            return await LortuEmaitzaAsync(false);
        }

        public async Task<EguraldiEmaitza?> LortuEmaitzaAsync(bool behartu)
        {
            await _lock.WaitAsync();
            try
            {
                if (!behartu && _cache != null && DateTime.UtcNow - _cacheNoiz < CacheDenbora)
                    return _cache;

                var emaitza = await IrakurriEtaParseatuAsync(Iturria);
                _cache = emaitza;
                _cacheNoiz = DateTime.UtcNow;
                return emaitza;
            }
            finally
            {
                _lock.Release();
            }
        }

        private static string DefaultIturria()
        {
            return "https://www.aemet.es/xml/municipios/localidad_20069.xml";
        }

        private static async Task<EguraldiEmaitza?> IrakurriEtaParseatuAsync(string iturria)
        {
            string xml;

            if (Uri.TryCreate(iturria, UriKind.Absolute, out var uri) &&
                (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                using var cts = new CancellationTokenSource(2000);
                xml = await _http.GetStringAsync(uri, cts.Token);
            }
            else
            {
                if (!File.Exists(iturria))
                    return null;

                xml = await File.ReadAllTextAsync(iturria);
            }

            if (string.IsNullOrWhiteSpace(xml))
                return null;

            var doc = XDocument.Parse(xml);
            var root = doc.Root;
            if (root == null)
                return null;

            string? temp = LortuAemetTenperatura(root)
                ?? BilatuBalioa(root, "tenperatura", "temperatura", "temperature", "temp");

            string? egoera = LortuAemetEgoera(root)
                ?? BilatuBalioa(root, "egoera", "estado", "condition", "weather", "tiempo");

            string? haizea = LortuAemetHaizea(root)
                ?? BilatuBalioa(root, "haizea", "viento", "wind");

            var info = new EguraldiaInfo
            {
                Tenperatura = temp?.Trim(),
                Egoera = NormalizatuEgoera(egoera?.Trim()),
                Haizea = haizea?.Trim()
            };

            var astea = LortuAemetAstea(root);
            if (astea.Count > 0)
            {
                info.Tenperatura = astea[0].Tenperatura;
                info.Egoera = astea[0].Egoera;
                info.Haizea = astea[0].Haizea;
            }

            return new EguraldiEmaitza
            {
                Iturria = iturria,
                Xml = xml,
                Info = info,
                Astea = astea
            };
        }

        private static List<EguraldiEgunInfo> LortuAemetAstea(XElement root)
        {
            var emaitza = new List<EguraldiEgunInfo>();

            XElement? pred = null;
            foreach (var el in root.Descendants())
            {
                if (string.Equals(el.Name.LocalName, "prediccion", StringComparison.OrdinalIgnoreCase))
                {
                    pred = el;
                    break;
                }
            }

            if (pred == null)
                return emaitza;

            foreach (var dia in pred.Elements())
            {
                if (!string.Equals(dia.Name.LocalName, "dia", StringComparison.OrdinalIgnoreCase))
                    continue;

                var fechaAttr = dia.Attribute("fecha")?.Value;
                if (!DateTime.TryParse(fechaAttr, out var data))
                    continue;

                var min = BilatuBalioa(dia, "minima");
                var max = BilatuBalioa(dia, "maxima");

                string? tenp = null;
                if (!string.IsNullOrWhiteSpace(min) || !string.IsNullOrWhiteSpace(max))
                {
                    if (string.IsNullOrWhiteSpace(max)) tenp = min?.Trim();
                    else if (string.IsNullOrWhiteSpace(min)) tenp = max?.Trim();
                    else tenp = $"{min.Trim()}-{max.Trim()}";
                }

                string? egoera = null;
                foreach (var ec in dia.Descendants())
                {
                    if (!string.Equals(ec.Name.LocalName, "estado_cielo", StringComparison.OrdinalIgnoreCase))
                        continue;

                    var desc = ec.Attribute("descripcion")?.Value;
                    if (!string.IsNullOrWhiteSpace(desc))
                    {
                        egoera = desc;
                        break;
                    }
                }

                string? norabidea = null;
                string? abiadura = null;
                foreach (var v in dia.Descendants())
                {
                    if (!string.Equals(v.Name.LocalName, "viento", StringComparison.OrdinalIgnoreCase))
                        continue;

                    norabidea = BilatuBalioa(v, "direccion");
                    abiadura = BilatuBalioa(v, "velocidad");
                    if (!string.IsNullOrWhiteSpace(norabidea) || !string.IsNullOrWhiteSpace(abiadura))
                        break;
                }

                string? haizea = null;
                if (!string.IsNullOrWhiteSpace(norabidea) && !string.IsNullOrWhiteSpace(abiadura))
                    haizea = $"{norabidea.Trim()} {abiadura.Trim()}";
                else if (!string.IsNullOrWhiteSpace(norabidea))
                    haizea = norabidea.Trim();
                else if (!string.IsNullOrWhiteSpace(abiadura))
                    haizea = abiadura.Trim();

                emaitza.Add(new EguraldiEgunInfo
                {
                    Data = data.Date,
                    Tenperatura = string.IsNullOrWhiteSpace(tenp) ? null : $"{tenp}°C",
                    Egoera = NormalizatuEgoera(egoera?.Trim()),
                    Haizea = haizea
                });

                if (emaitza.Count >= 7)
                    break;
            }

            return emaitza;
        }

        private static string? LortuAemetTenperatura(XElement root)
        {
            var max = BilatuBalioa(root, "maxima");
            var min = BilatuBalioa(root, "minima");
            if (!string.IsNullOrWhiteSpace(max) || !string.IsNullOrWhiteSpace(min))
            {
                if (string.IsNullOrWhiteSpace(max)) return min?.Trim();
                if (string.IsNullOrWhiteSpace(min)) return max?.Trim();
                return $"{min.Trim()}-{max.Trim()}";
            }

            return null;
        }

        private static string? LortuAemetEgoera(XElement root)
        {
            foreach (var el in root.Descendants())
            {
                if (!string.Equals(el.Name.LocalName, "estado_cielo", StringComparison.OrdinalIgnoreCase))
                    continue;

                var attr = el.Attribute("descripcion");
                if (attr != null && !string.IsNullOrWhiteSpace(attr.Value))
                    return attr.Value;

                if (!string.IsNullOrWhiteSpace(el.Value))
                    return el.Value;
            }

            return null;
        }

        private static string? LortuAemetHaizea(XElement root)
        {
            var norabidea = BilatuBalioa(root, "direccion");
            var abiadura = BilatuBalioa(root, "velocidad");

            if (string.IsNullOrWhiteSpace(norabidea) && string.IsNullOrWhiteSpace(abiadura))
                return null;

            norabidea = norabidea?.Trim();
            abiadura = abiadura?.Trim();

            if (!string.IsNullOrWhiteSpace(norabidea) && !string.IsNullOrWhiteSpace(abiadura))
                return $"{norabidea} {abiadura}";

            return !string.IsNullOrWhiteSpace(norabidea) ? norabidea : abiadura;
        }

        private static string? BilatuBalioa(XElement root, params string[] izenak)
        {
            foreach (var izena in izenak)
            {
                foreach (var el in root.Descendants())
                {
                    if (string.Equals(el.Name.LocalName, izena, StringComparison.OrdinalIgnoreCase))
                    {
                        var v = el.Value;
                        if (!string.IsNullOrWhiteSpace(v))
                            return v;
                    }
                }

                foreach (var attr in root.Descendants().Attributes())
                {
                    if (string.Equals(attr.Name.LocalName, izena, StringComparison.OrdinalIgnoreCase))
                    {
                        var v = attr.Value;
                        if (!string.IsNullOrWhiteSpace(v))
                            return v;
                    }
                }
            }

            return null;
        }

        private static string? NormalizatuEgoera(string? egoera)
        {
            if (string.IsNullOrWhiteSpace(egoera))
                return egoera;

            var e = egoera.Trim().ToLowerInvariant();
            return e switch
            {
                "sol" => "Eguzkia",
                "sun" => "Eguzkia",
                "lluvia" => "Euria",
                "rain" => "Euria",
                "nublado" => "Hodeitsu",
                "cloudy" => "Hodeitsu",
                "despejado" => "Garbi",
                "clear" => "Garbi",
                _ => egoera
            };
        }
    }

    public class EguraldiEmaitza
    {
        public string Iturria { get; set; } = string.Empty;
        public string Xml { get; set; } = string.Empty;
        public EguraldiaInfo Info { get; set; } = new EguraldiaInfo();
        public List<EguraldiEgunInfo> Astea { get; set; } = new List<EguraldiEgunInfo>();
    }

    public class EguraldiEgunInfo
    {
        public DateTime Data { get; set; }
        public string? Tenperatura { get; set; }
        public string? Egoera { get; set; }
        public string? Haizea { get; set; }
    }

    public class EguraldiaInfo
    {
        public string? Tenperatura { get; set; }
        public string? Egoera { get; set; }
        public string? Haizea { get; set; }

        public override string ToString()
        {
            string t = string.IsNullOrWhiteSpace(Tenperatura) ? "--" : Tenperatura;
            string e = string.IsNullOrWhiteSpace(Egoera) ? "--" : Egoera;
            string h = string.IsNullOrWhiteSpace(Haizea) ? "--" : Haizea;
            return $"Eguraldia: {t}°C | {e} | Haizea {h}";
        }
    }
}
