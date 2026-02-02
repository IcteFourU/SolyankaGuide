using SolyankaGuide.Internals;
using System.Windows;
using System.Windows.Shell;

namespace SolyankaGuide
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BGImage.Source = ImageLoader.LoadImage("background.jpg");
        }
    }
}