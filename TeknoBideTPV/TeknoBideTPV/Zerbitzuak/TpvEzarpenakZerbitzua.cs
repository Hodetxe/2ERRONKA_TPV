using System;
using System.IO;
using System.Text.Json;

namespace TeknoBideTPV.Zerbitzuak
{
    public class TpvEzarpenak
    {
        public string? OdooBaseUrl { get; set; }
        public string? OdooToken { get; set; }
    }

    public static class TpvEzarpenakZerbitzua
    {
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true
        };

        public static string EzarpenakBidea()
        {
            var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(baseDir, "TeknoBideTPV", "tpv.settings.json");
        }

        public static TpvEzarpenak Kargatu()
        {
            try
            {
                var path = EzarpenakBidea();
                if (!File.Exists(path)) return new TpvEzarpenak();
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<TpvEzarpenak>(json, _jsonOptions) ?? new TpvEzarpenak();
            }
            catch
            {
                return new TpvEzarpenak();
            }
        }

        public static bool Gorde(TpvEzarpenak ezarpenak)
        {
            try
            {
                var path = EzarpenakBidea();
                var dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrWhiteSpace(dir))
                    Directory.CreateDirectory(dir);

                var json = JsonSerializer.Serialize(ezarpenak ?? new TpvEzarpenak(), _jsonOptions);
                File.WriteAllText(path, json);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
