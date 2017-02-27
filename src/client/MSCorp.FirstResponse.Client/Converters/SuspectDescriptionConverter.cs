using MSCorp.FirstResponse.Client.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Converters
{
    public class SuspectDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is SuspectModel)
            {
                var suspect = value as SuspectModel;
                return string.Format("{0} eyes, {1} hair", suspect.EyeColor, suspect.HairColor);
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
