using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Windows;

namespace SolyankaGuide.Internals
{
    internal class GitHubAutoUpdate
    {

        public static async void Update()
        {
            var localFiles = GetLocalFiles(@"Assets/Data");
            if (localFiles == null) return;
            var githubFiles = await GetGitHubFolderContents("carefall", "SolyankaGuide", "Assets/Data");
            if (githubFiles == null || githubFiles.Count == 0) return;
            Dictionary<string, string> updateFiles = new();
            foreach (var item in githubFiles)
            {
                bool needDownload = false;
                var localFilePath = Path.Combine("Assets", item.Path!.Substring("Assets/".Length));
                if (!File.Exists(localFilePath))
                {
                    MessageBox.Show("Файл " + localFilePath + " не существует");
                    needDownload = true;
                }
                else
                {
                    string localSha = ComputeFileSha1(localFilePath);
                    string? fileSha = await GetGitHubFileShaAsync("carefall", "SolyankaGuide", "Assets/Data", "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");
                    if (fileSha == null) continue;
                    if (localSha != fileSha)
                    {
                        MessageBox.Show("Файлы " + localFilePath + " отличаются по sha: " + localSha + " " + fileSha);
                        needDownload = true;
                    }
                }
                if (needDownload)
                {
                    updateFiles.Add(item.Download_url!, localFilePath);
                }
            }
            if (updateFiles.Count > 0)
            {
                var result = MessageBox.Show("Найдена новая версия программы. Желаете установить?", "Автообновление", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (var item in updateFiles)
                    {
                        await DownloadFile(item.Key, item.Value);
                    }
                }
            }
        }

        private static async Task<string?> GetGitHubFileShaAsync(string owner, string repo, string path, string token)
        {
            string url = $"https://api.github.com/repos/{owner}/{repo}/contents/{path}";
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("SolyankaGuide");
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", token);
            }
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(json);
                string? sha = (string?)obj["sha"];
                return sha;
            }
            else
            {
                MessageBox.Show("Не удалось прочитать файл на GitHub. Обратитесь к разработчику гида.", "Автообновление", MessageBoxButton.OK);
                return null;
            }
        }

        public static async Task<List<GitHubContentItem>?> GetGitHubFolderContents(string owner, string repo, string path)
        {
            try
            {
                var url = $"https://api.github.com/repos/{owner}/{repo}/contents/{path}";
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("SolyankaGuide");
                var response = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<GitHubContentItem>>(response);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось получить доступ в репозиторий. Обратитесь к разработчику гида.", "Автообновление", MessageBoxButton.OK);
                return null;
            }
        }

        public static string ComputeFileSha1(string filePath)
        {
            using var sha1 = SHA1.Create();
            using var stream = File.OpenRead(filePath);
            var hash = sha1.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public static List<string>? GetLocalFiles(string folderPath)
        {
            try
            {
                return Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Select(f => f.Replace("\\", "/")).ToList();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка проверки файлов на диске.", "Автообновление", MessageBoxButton.OK);
                return null;
            }
        }

        public static async Task DownloadFile(string url, string destinationPath)
        {
            try
            {
                using var client = new HttpClient();
                var data = await client.GetByteArrayAsync(url);
                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath)!);
                await File.WriteAllBytesAsync(destinationPath, data);
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось загрузить и записать файл с GitHub.", "Автообновление", MessageBoxButton.OK);
            }
        }

        public class GitHubContentItem
        {
            public string? Name { get; set; }
            public string? Path { get; set; }
            public string? Sha { get; set; }
            public string? Type { get; set; }
            public string? Download_url { get; set; }
        }
    }
}
