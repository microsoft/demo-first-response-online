using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Controls
{
    public partial class EntryWithIcon : Grid
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create("Text", typeof(string), typeof(EntryWithIcon));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty PlaceholderProperty =
            BindableProperty.Create("Placeholder", typeof(string), typeof(EntryWithIcon));

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create("Icon", typeof(ImageSource), typeof(EntryWithIcon));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create("IsPassword", typeof(bool), typeof(EntryWithIcon), false);

        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        public EntryWithIcon()
        {
            InitializeComponent();
        }
    }
}
