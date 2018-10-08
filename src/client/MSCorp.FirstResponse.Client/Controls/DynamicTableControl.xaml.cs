using MSCorp.FirstResponse.Client.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Controls
{
    public partial class DynamicTableControl : ContentView
    {
        private TapGestureRecognizer _addRowGesture;

        public DynamicTableControl()
        {
            InitializeComponent();

            _addRowGesture = new TapGestureRecognizer
            {
                Command = new Command(AddRowRequested)
            };

            AddRowImage.GestureRecognizers.Add(_addRowGesture);
        }

        private void AddRowRequested(object obj)
        {
            var rowHolder = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                }
            };

            // Create key entry
            var keyEntry = new Entry
            {
                Placeholder = "Enter key",
                Style = Resources["TableEntryStyle"] as Style
            };

            // Create value entry
            var valueEntry = new Entry
            {
                Placeholder = "Enter value",
                Style = Resources["TableEntryStyle"] as Style
            };

            // Create value entry
            var removeImage = new Image
            {
                Style = Resources["RemoveRowStyle"] as Style
            };

            var removeRowGesture = new TapGestureRecognizer
            {
                Command = new Command(RemoveRowRequested),
                CommandParameter = rowHolder
            };

            removeImage.GestureRecognizers.Add(removeRowGesture);

            rowHolder.Children.Add(keyEntry, 0, 0);
            rowHolder.Children.Add(valueEntry, 1, 0);
            rowHolder.Children.Add(removeImage, 2, 0);

            RowsContainer.Children.Add(rowHolder);

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    await Task.Delay(150);
                }

                if (keyEntry.IsFocused)
                {
                    keyEntry.Unfocus();
                }

                keyEntry.Focus();
            });
        }

        private void RemoveRowRequested(object obj)
        {
            var rowElement = obj as View;

            if (rowElement == null) return;

            RowsContainer.Children.Remove(rowElement);

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    await Task.Delay(150);
                }

                var lastEntry = RowsContainer.GetDescendants().OfType<Entry>().LastOrDefault();
                lastEntry?.Focus();
            });
        }
    }
}