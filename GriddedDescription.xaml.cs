using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SolyankaGuide
{
    public partial class GriddedDescription : UserControl
    {
        public GriddedDescription()
        {
            InitializeComponent();
        }

        private void Image_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            DescGrid.Clip = new RectangleGeometry(new Rect(0, 0, Image.ActualWidth, Image.ActualHeight), 10, 10);
            double ps = e.PreviousSize.Width;
            double ns = e.NewSize.Width;
            if (ps == 0) return;
            TileName.FontSize *= (ns / ps);
        }
    }
}
