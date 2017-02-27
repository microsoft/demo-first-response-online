using Rg.Plugins.Popup.Pages;

namespace MSCorp.FirstResponse.Client.Views
{
    public partial class NewTicketPopupView : PopupPage
    {
        public NewTicketPopupView()
        {
            InitializeComponent();
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}
