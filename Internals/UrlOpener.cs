using System.Diagnostics;

namespace SolyankaGuide.Internals
{
    internal class UrlOpener
    {
        public static void OpenUrl(string url)
        {
            var psi = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
