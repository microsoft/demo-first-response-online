using System;
using System.Globalization;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Converters
{
    public class CitiesImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string cityName = (string)value;
            cityName = cityName.Replace(" ", string.Empty);

            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                return string.Format("Assets/Cities/city_{0}.jpg", cityName);

            if (Device.OS == TargetPlatform.iOS)
                return string.Format("cities/city_{0}.jpg", cityName);

            return string.Format("city_{0}.jpg", cityName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
