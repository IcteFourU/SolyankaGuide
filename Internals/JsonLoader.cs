using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;

namespace SolyankaGuide.Internals
{
    internal class JsonLoader
    {
        public static Dictionary<string, List<Description>> FillFromJson()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "SolyankaGuide.Assets.Data.helpButtons.json";
                using Stream stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    var result = MessageBox.Show("Проверьте, что все файлы установлены и перезапустите приложение.", "Ошибка при загрузке файлов", MessageBoxButton.OK);
                    return null;
                }
                using StreamReader reader = new StreamReader(stream);
                string json = reader.ReadToEnd();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var tempDict = JsonSerializer.Deserialize<Dictionary<string, List<Description>>>(json, options);
                return tempDict;
            }
            catch (Exception ex)
            {
                var result = MessageBox.Show("Проверьте, что все файлы установлены и перезапустите приложение.", "Ошибка при загрузке файлов", MessageBoxButton.OK);
                return null;
            }
        }
    }
}
