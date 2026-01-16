using System.Windows.Controls;

namespace SolyankaGuide
{
    public partial class TextContentControl : UserControl
    {
        public TextContentControl(string text)
        {
            InitializeComponent();
            ContentText.Text = text;
        }
    }
}
