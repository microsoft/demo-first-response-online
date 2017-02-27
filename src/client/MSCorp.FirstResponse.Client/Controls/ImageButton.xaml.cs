using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Controls
{
    public partial class ImageButton : Grid
    {
        public static readonly BindableProperty ImageButtonCommandProperty =
            BindableProperty.Create("ImageButtonCommand", typeof(ICommand), typeof(ImageButton), default(ICommand));


        public ICommand ImageButtonCommand
        {
            get { return (ICommand)GetValue(ImageButtonCommandProperty); }
            set { SetValue(ImageButtonCommandProperty, value); }
        }

        public static readonly BindableProperty ImageButtonTextProperty =
            BindableProperty.Create("ImageButtonText", typeof(string), typeof(ImageButton), default(string));

        public string ImageButtonText
        {
            get { return (string)GetValue(ImageButtonTextProperty); }
            set { SetValue(ImageButtonTextProperty, value); }
        }

        public ImageButton()
        {
            InitializeComponent();
        }
    }
}
