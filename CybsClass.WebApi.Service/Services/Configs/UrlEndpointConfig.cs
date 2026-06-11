using CybsClass.Cybersource.Models.BaseData;
using System.Text.Json;

namespace CybsClass.WebApi.Service.Services.Configs
{
    public static class UrlEndpointConfig
    {
        private static List<UrlEndpointEntry> _entries = new();

        public static void Initialize(string jsonPath)
        {
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"[UrlEndpointConfig] urlEndpoints.json not found at {jsonPath}");
                return;
            }
            var json = File.ReadAllText(jsonPath);
            _entries = JsonSerializer.Deserialize<List<UrlEndpointEntry>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            Console.WriteLine($"[UrlEndpointConfig] Loaded {_entries.Count} endpoint definitions.");
        }

        public static string ResolveMethod(string resource)
        {
            foreach (var entry in _entries)
            {
                if (string.Equals(entry.Resource, resource, StringComparison.OrdinalIgnoreCase))
                    return entry.Method.ToUpperInvariant();
            }
            foreach (var entry in _entries)
            {
                if (MatchesTemplate(entry.Resource, resource))
                    return entry.Method.ToUpperInvariant();
            }
            return "POST";
        }

        private static bool MatchesTemplate(string template, string actual)
        {
            var tParts = template.Split('/');
            var aParts = actual.Split('/');
            if (tParts.Length != aParts.Length) return false;
            for (int i = 0; i < tParts.Length; i++)
            {
                if (tParts[i].StartsWith('{') && tParts[i].EndsWith('}')) continue;
                if (!string.Equals(tParts[i], aParts[i], StringComparison.OrdinalIgnoreCase)) return false;
            }
            return true;
        }
    }
}
