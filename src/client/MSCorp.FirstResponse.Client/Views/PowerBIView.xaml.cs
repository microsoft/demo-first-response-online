using MSCorp.FirstResponse.Client.ViewModels;
using Rg.Plugins.Popup.Pages;

namespace MSCorp.FirstResponse.Client.Views
{
    public partial class PowerBIView : PopupPage
    {
        public PowerBIView()
        {
            InitializeComponent();
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}