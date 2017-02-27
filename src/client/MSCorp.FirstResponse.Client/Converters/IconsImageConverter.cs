using MSCorp.FirstResponse.Client.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Converters
{
    public class IconsImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IncidentType type = (IncidentType)value;

            string iconName;

            switch (type)
            {
                case IncidentType.Alert:
                    iconName = "icon_alert";
                    break;
                case IncidentType.Animal:
                    iconName = "icon_animal";
                    break;
                case IncidentType.Arrest:
                    iconName = "icon_arrest";
                    break;
                case IncidentType.Car:
                    iconName = "icon_car";
                    break;
                case IncidentType.Fire:
                    iconName = "icon_fire";
                    break;
                case IncidentType.OfficerRequired:
                    iconName = "icon_officer";
                    break;
                case IncidentType.Stranger:
                    iconName = "icon_stranger";
                    break;
                default:
                    iconName = "icon_other";
                    break;
            }

            var iconPath = iconName;

            if (Device.OS == TargetPlatform.WinPhone || Device.OS == TargetPlatform.Windows)
            {
                iconPath = string.Format("Assets/Icons/{0}.png", iconName);
            }
            else if(Device.OS == TargetPlatform.iOS)
            {
                iconPath = string.Format("icons/{0}", iconName);
            }

            return iconPath;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}