using SolyankaGuide.Internals;
using System.Windows;

namespace SolyankaGuide
{
    public partial class App : Application {
        public App() : base() 
        {
            GitHubAutoUpdate.Update();
        }
    }
}
