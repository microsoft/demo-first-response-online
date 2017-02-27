using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Views.Templates
{
    public partial class SuspectListItemTemplate : ContentView
    {

        public static readonly BindableProperty SelectableProperty =
            BindableProperty.Create("Selectable", typeof(bool), typeof(SuspectListItemTemplate), default(bool));

        public bool Selectable
        {
            get { return (bool)GetValue(SelectableProperty); }
            set { SetValue(SelectableProperty, value); }
        }

        public SuspectListItemTemplate()
        {
            InitializeComponent();
        }
    }
}
