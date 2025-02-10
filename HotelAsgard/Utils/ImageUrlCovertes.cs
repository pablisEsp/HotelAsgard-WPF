using System.Globalization;
using System.Windows.Data;

namespace HotelAsgard.Utils
{
    public class ImageUrlConverter : IValueConverter
    {
        private const string BaseUrl = "http://localhost:3000"; // URL base del servidor

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrEmpty(imagePath))
            {
                return BaseUrl + imagePath;
            }
            return "pack://application:,,,/Images/default.png"; // Imagen por defecto si no hay URL
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}