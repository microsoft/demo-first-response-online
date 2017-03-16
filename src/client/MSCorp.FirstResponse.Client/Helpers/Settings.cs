// Helpers/Settings.cs
using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace MSCorp.FirstResponse.Client.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string CurrentUserKey = "authenticated_user";

        private const string SelectedCityKey = "selected_city";

        private const string UserLatitudeKey = "user_latitude";

        private const string UserLongitudeKey = "user_longitude";

        private const string AmbulanceLatitudeKey = "ambulance_latitude";

        private const string AmbulanceLongitudeKey = "ambulance_longitude";

        private const string UseMockServiceKey = "use_mock_services_key";

        private const string ServiceEndpointKey = "service_endpoint_address";

        private const string PowerBIUrlKey = "power_bi_key";

        private const string UserSpeedKey = "user_speed";
        #endregion

        public static double AmbulanceLatitude
        {
            get
            {
                return AppSettings.GetValueOrDefault<double>(AmbulanceLatitudeKey, GlobalSetting.UserLatitude);
            }
            set
            {
                AppSettings.AddOrUpdateValue<double>(AmbulanceLatitudeKey, value);
            }
        }

        public static double AmbulanceLongitude
        {
            get
            {
                return AppSettings.GetValueOrDefault<double>(AmbulanceLongitudeKey, GlobalSetting.UserLongitude);
            }
            set
            {
                AppSettings.AddOrUpdateValue<double>(AmbulanceLongitudeKey, value);
            }
        }

        public static double UserLatitude
        {
            get
            {
                return AppSettings.GetValueOrDefault<double>(UserLatitudeKey, GlobalSetting.UserLatitude);
            }
            set
            {
                AppSettings.AddOrUpdateValue<double>(UserLatitudeKey, value);
            }
        }

        public static double UserLongitude
        {
            get
            {
                return AppSettings.GetValueOrDefault<double>(UserLongitudeKey, GlobalSetting.UserLongitude);
            }
            set
            {
                AppSettings.AddOrUpdateValue<double>(UserLongitudeKey, value);
            }
        }

        public static string CurrentUser
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(CurrentUserKey, default(string));
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(CurrentUserKey, value);
            }
        }


        public static int SelectedCity
        {
            get
            {
                if (!UseMockService && AppSettings.GetValueOrDefault<int>(SelectedCityKey) == GlobalSetting.DefaultMockCityId)
                {
                    AppSettings.AddOrUpdateValue<int>(SelectedCityKey, GlobalSetting.DefaultCityId);
                }

                return AppSettings.GetValueOrDefault<int>(SelectedCityKey, GlobalSetting.DefaultCityId);
            }
            set
            {
                AppSettings.AddOrUpdateValue<int>(SelectedCityKey, value);
            }
        }

        public static double UserSpeed
        {
            get
            {
                return AppSettings.GetValueOrDefault<double>(UserSpeedKey, GlobalSetting.MovementSpeed);
            }
            set
            {
                AppSettings.AddOrUpdateValue<double>(UserSpeedKey, value);
            }
        }

        public static bool UseMockService
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(UseMockServiceKey, GlobalSetting.UseMockServiceDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(UseMockServiceKey, value);
            }
        }
        
        public static string ServiceEndpoint
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(ServiceEndpointKey, GlobalSetting.ServiceEndpoint);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(ServiceEndpointKey, value);
            }
        }

        public static void RemoveCurrentUser()
        {
            AppSettings.Remove(CurrentUserKey);
        }
    }
}
