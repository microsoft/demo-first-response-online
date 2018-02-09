using System;
using System.Globalization;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Converters
{
    public class FloatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as float?;

            if (!v.HasValue) return null;

            return v == 0 ? null : v.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strVal = value?.ToString();
            float v = 0f;
            float.TryParse(strVal, out v);

            return v;
        }
    }
}
