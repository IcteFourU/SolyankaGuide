using SolyankaGuide.Internals;
using System.Windows;

namespace SolyankaGuide
{
    public partial class App : Application {

        public static Action? RefreshUI;

        public App() : base() 
        {
            Application.Current.DispatcherUnhandledException += (s, e) =>
            {
                MessageBox.Show(e.Exception.ToString(), "Unhandled UI Exception");
                e.Handled = true;
            };
            bool shouldUpdate = GitHubAutoUpdate.Update().Result;
            if (shouldUpdate)
            {
                RefreshUI?.Invoke();
            }
        }
    }
}
