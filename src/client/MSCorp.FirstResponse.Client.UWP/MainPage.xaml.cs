using ImageCircle.Forms.Plugin.UWP;
using MSCorp.FirstResponse.Client.UWP.Renderers;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.ViewManagement;

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
                    titleBar.BackgroundColor = (Color)App.Current.Resources["NativeAccentColor"];
                    titleBar.ButtonBackgroundColor = (Color)App.Current.Resources["NativeAccentColor"];
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