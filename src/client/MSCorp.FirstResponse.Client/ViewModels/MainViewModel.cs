using System.Threading.Tasks;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using MSCorp.FirstResponse.Client.Services.Responder;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using MSCorp.FirstResponse.Client.Services.Authentication;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Services.Incidents;
using MSCorp.FirstResponse.Client.Services.Heatmap;
using Plugin.Connectivity;

namespace MSCorp.FirstResponse.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private UserRole _selectedUser;
        private bool _heatMap;
        private bool _forceNavigation;
        private bool _incidentToggleButtonChecked;
        private bool _responderToggleButtonChecked;
        private IncidentListViewModel _incidentListViewModel;
        private ResponderListViewModel _responderListViewModel;
        private IncidentDetailViewModel _incidentDetailViewModel;
        private ObservableCollection<Geoposition> _heatData;

        private IResponderService _responderService;
        private IHeatmapService _heatmapService;

        public MainViewModel(
            IResponderService responderService, IAuthenticationService authenticationService,                   
            IIncidentsService incidentsService, IHeatmapService heatmapService)
        {
            _responderService = responderService;
            _heatmapService = heatmapService;

            IncidentToggleButtonChecked = true;
            ResponderToggleButtonChecked = false;

            _incidentListViewModel = new IncidentListViewModel(incidentsService);
            _responderListViewModel = new ResponderListViewModel(_responderService);
            _incidentDetailViewModel = new IncidentDetailViewModel();

            MessagingCenter.Subscribe<IncidentModel>(this, MessengerKeys.NavigateToCurrentIncident, (incident) => ForceNavigation = true);
        }

        public UserRole SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                RaisePropertyChanged(() => SelectedUser);
            }
        }

        public bool HeatMap
        {
            get
            {
                return _heatMap;
            }
            set
            {
                _heatMap = value;
                RaisePropertyChanged(() => HeatMap);
            }
        }

        public bool ForceNavigation
        {
            get
            {
                return _forceNavigation;
            }
            set
            {
                _forceNavigation = value;
                RaisePropertyChanged(() => ForceNavigation);
            }
        }

        public bool IncidentToggleButtonChecked
        {
            get
            {
                return _incidentToggleButtonChecked;
            }
            set
            {
                _incidentToggleButtonChecked = value;
                RaisePropertyChanged(() => IncidentToggleButtonChecked);
            }
        }

        public bool ResponderToggleButtonChecked
        {
            get
            {
                return _responderToggleButtonChecked;
            }
            set
            {
                _responderToggleButtonChecked = value;
                RaisePropertyChanged(() => ResponderToggleButtonChecked);
            }
        }

        public IncidentListViewModel IncidentListViewModel
        {
            get
            {
                return _incidentListViewModel;
            }
            set
            {
                _incidentListViewModel = value;
                RaisePropertyChanged(() => IncidentListViewModel);
            }
        }

        public ResponderListViewModel ResponderListViewModel
        {
            get
            {
                return _responderListViewModel;
            }
            set
            {
                _responderListViewModel = value;
                RaisePropertyChanged(() => ResponderListViewModel);
            }
        }

        public IncidentDetailViewModel IncidentDetailViewModel
        {
            get
            {
                return _incidentDetailViewModel;
            }
            set
            {
                _incidentDetailViewModel = value;
                RaisePropertyChanged(() => IncidentDetailViewModel);
            }
        }        

        public ObservableCollection<Geoposition> HeatData
        {
            get
            {
                return _heatData;
            }
            set
            {
                _heatData = value;
                RaisePropertyChanged(() => HeatData);
            }
        }

        public ICommand SelectorCommand => new Command<string>(Selector);
        public ICommand PowerBICommand => new Command(PowerBI);
        public ICommand LogoutCommand => new Command(LogoutAsync);

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            await _incidentListViewModel.InitializeAsync(null);
            await _responderListViewModel.InitializeAsync(null);

            if (navigationData is UserRole)
            {
                SelectedUser = navigationData as UserRole;
            }

            HeatData = await _heatmapService.GetHeatmapPointsAsync();

            IsBusy = false;
        }

        private void Selector(string parameter)
        {
            if(parameter.Equals("Incidents"))
            {
                IncidentToggleButtonChecked = true;
                ResponderToggleButtonChecked = false;
            }
            else
            {
                IncidentToggleButtonChecked = false;
                ResponderToggleButtonChecked = true;
            }
        }

        private async void PowerBI()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await NavigationService.NavigateToPopupAsync<PowerBIViewModel>(true);
            }
            else
            {
                await DialogService.ShowAlertAsync("No internet connection available!", "Oops!", "Ok");
            }
        }

        private async void LogoutAsync()
        {
            Settings.RemoveCurrentUser();
            await NavigationService.NavigateToAsync<LoginViewModel>();
            await NavigationService.RemoveBackStackAsync();
        }
    }
}