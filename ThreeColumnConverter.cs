using System.Globalization;
using System.Windows.Data;

namespace SolyankaGuide
{
    public class ThreeColumnConverter : IValueConverter
    {
        public double Margin { get; set; } = 15;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double totalWidth)
            {
                return (totalWidth / 3) - Margin;
            }
            return 100.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}