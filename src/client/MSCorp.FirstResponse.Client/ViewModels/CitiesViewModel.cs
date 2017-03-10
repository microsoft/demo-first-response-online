using System.Threading.Tasks;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using MSCorp.FirstResponse.Client.Services.Cities;
using Xamarin.Forms;
using System.Windows.Input;
using MSCorp.FirstResponse.Client.Helpers;

namespace MSCorp.FirstResponse.Client.ViewModels
{
    public class CitiesViewModel : ViewModelBase
    {
        private ObservableCollection<EventModel> _cities;
        private ConfigViewModel _configViewModel;

        private ICitiesService _citiesService;

        public CitiesViewModel(ICitiesService citiesService)
        {
            _citiesService = citiesService;

            ConfigViewModel = new ConfigViewModel();
        }

        public ObservableCollection<EventModel> Cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                RaisePropertyChanged(() => Cities);
            }
        }

        public ConfigViewModel ConfigViewModel
        {
            get
            {
                return _configViewModel;
            }

            set
            {
                _configViewModel = value;
            }
        }

        public async override Task InitializeAsync(object navigationData)
        {
            Cities = await _citiesService.GetEventsAsync();
            await base.InitializeAsync(navigationData);
        }

        public ICommand CitySelectedCommand => new Command<EventModel>(CitySelected);

        private async void CitySelected(EventModel selectedEvent)
        {
            if (ViewModelLocator.Instance.UseMockService && (selectedEvent.CityId != GlobalSetting.DefaultMockCityId))
            {
                if (!await DialogService.ConfirmAsync("Your current selection will disable mock mode, Are you sure?", "Mock Enabled"))
                {
                    return;
                }
                else
                {
                    ViewModelLocator.Instance.UseMockService = false;
                }
            }

            Settings.SelectedCity = selectedEvent.CityId;
            Settings.UserLatitude = selectedEvent.Latitude;
            Settings.UserLongitude = selectedEvent.Longitude;
            Settings.AmbulanceLatitude = selectedEvent.AmbulancePosition.Latitude;
            Settings.AmbulanceLongitude = selectedEvent.AmbulancePosition.Longitude;
            await NavigationService.NavigateToAsync<LoginViewModel>(selectedEvent);
            await NavigationService.RemoveBackStackAsync();
        }
    }
}