using System;
using System.Globalization;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Converters
{
    public class EmptyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strValue = value as string;

            return string.IsNullOrEmpty(strValue) ? parameter : value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
