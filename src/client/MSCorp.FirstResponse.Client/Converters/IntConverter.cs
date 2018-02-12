using System;
using System.Globalization;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Converters
{
    public class IntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = value as int?;

            if (!v.HasValue) return null;

            return v == 0 ? null : v.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var strVal = value?.ToString();
            int v = 0;
            int.TryParse(strVal, out v);

            return v;
        }
    }
}
