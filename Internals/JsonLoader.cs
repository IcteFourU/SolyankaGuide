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
            string? json = LoadResource("SolyankaGuide.Assets.Data.categories.json");
            if (json == null) return null;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Category[]>(json, options);
        }

        public static Element[]? FillElements(string elementsPath)
        {
            string? json = LoadResource("SolyankaGuide.Assets.Data." + elementsPath);
            if (json == null) return null;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Element[]>(json, options);
        }

        private static string? LoadResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                var result = MessageBox.Show("Проверьте, что все файлы установлены и перезапустите приложение.", "Ошибка при чтении файда", MessageBoxButton.OK);
                return null;
            }
            using StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();
            return json;
        }
    }
}
