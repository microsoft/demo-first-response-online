using MSCorp.FirstResponse.Client.Animations;
using MSCorp.FirstResponse.Client.Extensions;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Views.Templates
{
    public partial class IncidentListItemTemplate : ContentView
    {
        public IncidentListItemTemplate()
        {
            InitializeComponent();

            this.BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(HighPriorityTitle.Text) && HighPriorityTitle.IsVisible)
            {
                var highPriorityAnimation = this.Resources["HighPriorityAnimation"] as StoryBoard;

                if (highPriorityAnimation != null)
                {
                    HighPriorityPoint.Animate(highPriorityAnimation);
                }
            }
        }
    }
}