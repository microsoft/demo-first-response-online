using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.Services.Authentication;
using MSCorp.FirstResponse.Client.Services.Cities;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using Plugin.Connectivity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private IAuthenticationService _authenticationService;

        private readonly ICitiesService _citiesService;

        private EventModel _city;

        public EventModel City { 
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                RaisePropertyChanged(() => City);
            }
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public LoginViewModel(IAuthenticationService authenticationService, ICitiesService citiesService)
        {
            _authenticationService = authenticationService;
            _citiesService = citiesService;
        }

        public async override Task InitializeAsync(object navigationData)
        {
            if (navigationData is EventModel)
            {
                City = navigationData as EventModel;
            }
            else
            {
                var events = await _citiesService.GetEventsAsync();

                City = events
                    .Where(q => q.CityId == Settings.SelectedCity)
                    .FirstOrDefault();

                if(City == null)
                {
                    City = _citiesService.GetDefaultEvent(Settings.SelectedCity);
                }
            }

            await base.InitializeAsync(navigationData);
        }

        public ICommand LoginCommand => new Command(LoginUser);
        public ICommand SelectCityCommand => new Command(SelectCity);

        private async void LoginUser()
        {
            IsBusy = true;

            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                if (CrossConnectivity.Current.IsConnected || ViewModelLocator.Instance.UseMockService)
                {
                    UserRole user = await _authenticationService.LoginAsync(UserName, Password);
                    if (user != null)
                    {
                        await NavigationService.NavigateToAsync<MainViewModel>(user);
                    }
                    else
                    {
                        await DialogService.ShowAlertAsync("Invalid credentials", "Login failure", "Try again");
                    }
                }
                else
                {
                    await DialogService.ShowAlertAsync("No internet connection available!", "Oops!", "Ok");
                }
            }

            IsBusy = false;
        }

        private void SelectCity()
        {
            NavigationService.NavigateToAsync<CitiesViewModel>();
        }
    }
}
