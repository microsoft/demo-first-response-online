using System.Windows.Input;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Views
{
    public partial class ResponderListPaneView : ContentView
    {
        public static readonly BindableProperty SelectedIncidentCommandProperty =
            BindableProperty.Create("SelectedIncidentCommand",
                typeof(ICommand), typeof(ResponderListPaneView), default(ICommand),
                BindingMode.TwoWay);

        public ICommand SelectedIncidentCommand
        {
            get { return (ICommand)base.GetValue(SelectedIncidentCommandProperty); }
            set { base.SetValue(SelectedIncidentCommandProperty, value); }
        }

        public ResponderListPaneView()
        {
            InitializeComponent();
        }
    }
}
