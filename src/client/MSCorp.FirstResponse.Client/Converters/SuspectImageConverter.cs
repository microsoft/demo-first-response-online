using System;
using System.Globalization;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Converters
{
    public class SuspectImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string suspectName = (string)value;
            suspectName = suspectName.Replace(" ", string.Empty);

            if (Device.RuntimePlatform == Device.WinPhone || Device.RuntimePlatform == Device.UWP)
                return string.Format("Assets/Suspect/{0}", suspectName);

            if (Device.RuntimePlatform == Device.iOS)
                return string.Format("suspects/suspect_{0}", suspectName);

            return string.Format("suspect_{0}", suspectName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
