using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Behaviors
{
    public sealed class ItemTappedCommandListView
    {
        public static readonly BindableProperty ItemTappedCommandProperty =
            BindableProperty.CreateAttached(
                "ItemTappedCommand",
                typeof(ICommand),
                typeof(ItemTappedCommandListView),
                default(ICommand),
                BindingMode.OneWay,
                null,
                PropertyChanged);

        private static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listView = bindable as ListView;
            if (listView != null)
            {
                listView.ItemSelected -= ListViewOnItemSelected;
                listView.ItemSelected += ListViewOnItemSelected;
            }
        }

        private static void ListViewOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var list = sender as ListView;
            if (list != null && list.IsEnabled && !list.IsRefreshing)
            {
                var command = GetItemTappedCommand(list);
                if (command != null && command.CanExecute(list.SelectedItem))
                {
                    command.Execute(list.SelectedItem);
                }
            }
        }

        public static ICommand GetItemTappedCommand(BindableObject bindableObject)
        {
            return (ICommand)bindableObject.GetValue(ItemTappedCommandProperty);
        }

        public static void SetItemTappedCommand(BindableObject bindableObject, object value)
        {
            bindableObject.SetValue(ItemTappedCommandProperty, value);
        }
    }
}
