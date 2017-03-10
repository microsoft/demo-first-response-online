using ImageCircle.Forms.Plugin.UWP;
using MSCorp.FirstResponse.Client.UWP.Renderers;
using Plugin.Toasts.UWP;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new Client.App());
            Xamarin.FormsMaps.Init(GlobalSetting.BingMapsAPIKey);
            ImageCircleRenderer.Init();
            RoundedBoxViewRenderer.Init();
            DependencyService.Register<ToastNotification>();
            ToastNotification.Init();

            NativeCustomize();
        }

        private void NativeCustomize()
        {
            // PC Customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.BackgroundColor = (Windows.UI.Color)App.Current.Resources["NativeAccentColor"];
                    titleBar.ButtonBackgroundColor = (Windows.UI.Color)App.Current.Resources["NativeAccentColor"];
                }
            }

            // Launch in Window Mode
            var currentView = ApplicationView.GetForCurrentView();
            if (currentView.IsFullScreenMode)
            {
                currentView.ExitFullScreenMode();
            }
        }
    }
}