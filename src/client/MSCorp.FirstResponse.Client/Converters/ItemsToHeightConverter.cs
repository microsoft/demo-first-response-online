using System;
using System.Globalization;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Converters
{
    public class ItemsToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                var itemHeight = System.Convert.ToInt32(parameter);
                return System.Convert.ToInt32(value) * itemHeight;
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
