using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shell;

namespace SolyankaGuide
{
    public partial class TitleControl : UserControl
    {

        private readonly Window window;
        private double l, t, w, h;
        private bool maximized = false;

        public TitleControl()
        {
            InitializeComponent();
            window = App.Current.MainWindow;
            WindowChrome.SetIsHitTestVisibleInChrome(ButtonsPanel, true);
        }

        public void CloseButton(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        public void MinimizeButton(object sender, RoutedEventArgs e)
        {
            window.WindowState = WindowState.Minimized;
        }

        public void ResizeButton(object sender, RoutedEventArgs e)
        {
            if (!maximized)
            {
                l = window.Left;
                t = window.Top;
                w = window.Width;
                h = window.Height;
                var workArea = SystemParameters.WorkArea;
                window.Left = workArea.Left;
                window.Top = workArea.Top;
                window.Width = workArea.Width;
                window.Height = workArea.Height;
                maximized = true;
            } else
            {
                window.Left = l;
                window.Top = t;
                window.Height = h;
                window.Width = w;
                maximized = false;
            }
        }

        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && window.WindowState != WindowState.Minimized) window.DragMove();
        }
    }
}
