using Rg.Plugins.Popup.Pages;

namespace MSCorp.FirstResponse.Client.Views
{
    public partial class SuspectPopupView : PopupPage
    {
        public SuspectPopupView()
        {
            InitializeComponent();
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }

    }
}
