using MSCorp.FirstResponse.Client.Maps.Routes.GoogleApi;
using MSCorp.FirstResponse.Client.Services.Navigation;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DrivingRouterProvider.GoogleMapsApiKey = GlobalSetting.GoogleMapsApiKey;

            if (Device.RuntimePlatform == Device.UWP)
            {
                InitNavigation();
            }
        }

        private Task InitNavigation()
        {
            var navigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (Device.RuntimePlatform != Device.UWP)
            {
                await InitNavigation();
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}