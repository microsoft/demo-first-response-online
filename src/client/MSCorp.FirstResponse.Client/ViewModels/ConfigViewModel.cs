using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Services.Cities;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.ViewModels
{
    public class ConfigViewModel : ViewModelBase
    {
        public bool UseMockService { get; set; } = ViewModelLocator.Instance.UseMockService;

        public string ServiceEndpoint { get; set; } = Settings.ServiceEndpoint;

        public double UserSpeed { get; set; } = Settings.UserSpeed;
        
        public ICommand SaveChangesCommand => new Command(SaveChanges);

        private async void SaveChanges()
        {
            ViewModelLocator.Instance.UseMockService = UseMockService;
            if (UseMockService)
            {
                // reset to mock values
                Settings.SelectedCity = GlobalSetting.DefaultMockCityId;
                Settings.UserLatitude = GlobalSetting.UserLatitude;
                Settings.UserLongitude = GlobalSetting.UserLongitude;
                Settings.AmbulanceLatitude = GlobalSetting.AmbulanceLatitude;
                Settings.AmbulanceLongitude = GlobalSetting.AmbulanceLongitude;
            }
            else
            {
                // load selected city from api
                ICitiesService citiesService = ViewModelLocator.Instance.Resolve<ICitiesService>();
                Settings.SelectedCity = Settings.SelectedCity == 0 ? GlobalSetting.DefaultCityId : Settings.SelectedCity;
                var city = (await citiesService.GetEventsAsync()).FirstOrDefault(q => q.CityId == Settings.SelectedCity);
                Settings.UserLatitude = city.Latitude;
                Settings.UserLongitude = city.Longitude;
                Settings.AmbulanceLatitude = city.AmbulancePosition.Latitude;
                Settings.AmbulanceLongitude = city.AmbulancePosition.Longitude;
            }

            Settings.ServiceEndpoint = ServiceEndpoint;
            Settings.UserSpeed = UserSpeed;
            await NavigationService.NavigateToAsync<LoginViewModel>();
            await NavigationService.RemoveBackStackAsync();
        }
    }
}
