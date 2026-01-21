using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;

namespace SolyankaGuide.Internals
{
    internal static class JsonLoader
    {
        public static Category[]? FillCategories()
        {
            string? json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "/Assets/Data/categories.json");
            if (json == null) return null;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Category[]>(json, options);
        }

        public static Element[]? FillElements(string elementsPath)
        {
            string? json = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + $"/Assets/Data/{elementsPath}");
            if (json == null) return null;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Element[]>(json, options);
        }
    }
}
