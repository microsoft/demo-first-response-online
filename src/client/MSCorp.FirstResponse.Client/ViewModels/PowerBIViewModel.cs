using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.ViewModels.Base;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.ViewModels
{
    public class PowerBIViewModel : ViewModelBase
    {
        public string PowerBIUrl = $"{Settings.ServiceEndpoint}/PowerBi";

        public string PowerBIPageURL
        {
            get { return PowerBIUrl; }
        }

        public ICommand ClosePopupCommand => new Command(ClosePopup);

        private async void ClosePopup()
        {
            await PopupNavigation.PopAllAsync(true);
        }
    }
}
