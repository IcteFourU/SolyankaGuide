using System.Globalization;
using System.Windows.Data;

namespace SolyankaGuide
{
    public class ThreeColumnConverter : IValueConverter
    {
        public double Margin { get; set; } = 30;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double totalWidth)
            {
                return ((totalWidth - Margin) / 3);
            }
            return 100.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}