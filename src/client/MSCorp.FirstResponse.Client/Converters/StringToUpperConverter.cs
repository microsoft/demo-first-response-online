using System;
using System.Globalization;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Converters
{
    public class StringToUpperConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var upperCase = string.Empty;

            if (value is Enum)
            {
                upperCase = Enum.GetName(value.GetType(), value as Enum);
            }
            else
            {
                upperCase = value?.ToString() ?? string.Empty;
            }

            return upperCase.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}