using System.Windows.Input;
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

        public static readonly BindableProperty NewTicketCommandProperty =
            BindableProperty.Create("NewTicketCommand", typeof(ICommand), typeof(SuspectListItemTemplate), default(ICommand));

        public ICommand NewTicketCommand
        {
            get { return (ICommand)GetValue(NewTicketCommandProperty); }
            set { SetValue(NewTicketCommandProperty, value); }
        }

        public static readonly BindableProperty NewEpcrCommandProperty =
            BindableProperty.Create("NewEpcrCommand", typeof(ICommand), typeof(SuspectListItemTemplate), default(ICommand));

        public ICommand NewEpcrCommand
        {
            get { return (ICommand)GetValue(NewEpcrCommandProperty); }
            set { SetValue(NewEpcrCommandProperty, value); }
        }

        public SuspectListItemTemplate()
        {
            InitializeComponent();
        }
    }
}
