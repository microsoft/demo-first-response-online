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

            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
                return string.Format("Assets/Suspect/{0}", suspectName);

            if (Device.OS == TargetPlatform.iOS)
                return string.Format("suspects/suspect_{0}", suspectName);

            return string.Format("suspect_{0}", suspectName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
