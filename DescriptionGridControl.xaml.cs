using SolyankaGuide.Internals;
using System.Windows;
using System.Windows.Controls;

namespace SolyankaGuide
{
    public partial class DescriptionGridControl : UserControl
    {

        internal static event Action<Description>? ShowDescription;

        public DescriptionGridControl()
        {
            InitializeComponent();
            GuideControl.ShowGrid += ShowGrid;
        }

        private void ShowGrid(Element element)
        {
            Descriptions.Children.Clear();
            foreach(Description desc in element.Descriptions!)
            {
                Descriptions.Children.Add(BuildSubButtonUI(desc));
            }
        }

        private GriddedDescription BuildSubButtonUI(Description desc)
        {
            GriddedDescription gd = new();
            gd.Image.Source = IconStorage.GetById(desc.IconId);
            gd.Name.Text = desc.Name;
            gd.MouseDown += (s, e) => OpenDescription(desc);
            return gd;
        }

        private void OpenDescription(Description desc)
        {
            Visibility = Visibility.Hidden;
            ShowDescription?.Invoke(desc);
        }
    }
}
