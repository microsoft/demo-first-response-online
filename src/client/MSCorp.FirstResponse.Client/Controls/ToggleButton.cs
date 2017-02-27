using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Controls
{
    public class ToggleButton : ContentView
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ToggleButton), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(ToggleButton), null);

        public static readonly BindableProperty CheckedProperty =
            BindableProperty.Create("Checked", typeof(bool), typeof(ToggleButton), false, BindingMode.TwoWay,
                null, propertyChanged: OnCheckedChanged);

        public static readonly BindableProperty AnimateProperty =
            BindableProperty.Create("Animate", typeof(bool), typeof(ToggleButton), false);

        public static readonly BindableProperty CheckedImageProperty =
            BindableProperty.Create("CheckedImage", typeof(ImageSource), typeof(ToggleButton), null);

        public static readonly BindableProperty UnCheckedImageProperty =
            BindableProperty.Create("UnCheckedImage", typeof(ImageSource), typeof(ToggleButton), null);

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create("Text", typeof(string), typeof(ToggleButton), null);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create("TextColor", typeof(Color), typeof(ToggleButton), Color.White);

        private Color DefaultTextColor = Color.White;

        private ICommand ToggleCommand;
        private Image ToggleImage;
        private Label ToggleLabel;

        public ToggleButton()
        {
            Initialize();
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public bool Checked
        {
            get { return (bool)GetValue(CheckedProperty); }
            set { SetValue(CheckedProperty, value); }
        }

        public bool Animate
        {
            get { return (bool)GetValue(AnimateProperty); }
            set { SetValue(AnimateProperty, value); }
        }

        public ImageSource CheckedImage
        {
            get { return (ImageSource)GetValue(CheckedImageProperty); }
            set { SetValue(CheckedImageProperty, value); }
        }

        public ImageSource UnCheckedImage
        {
            get { return (ImageSource)GetValue(UnCheckedImageProperty); }
            set { SetValue(UnCheckedImageProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        public ICommand ToogleCommand
        {
            get
            {
                return ToggleCommand
                       ?? (ToggleCommand = new Command(() =>
                       {
                           if (Checked)
                           {
                               Checked = false;
                           }
                           else
                           {
                               Checked = true;
                           }

                           if (Command != null)
                           {
                               Command.Execute(CommandParameter);
                           }
                       }));
            }
        }

        private void Initialize()
        {
            ToggleImage = new Image();

            ToggleLabel = new Label
            {
                TextColor = TextColor != null ? TextColor : DefaultTextColor,
                FontSize = 10,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            Animate = true;

            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = ToogleCommand
            });

            ToggleImage.Source = UnCheckedImage;
            ToggleLabel.Text = Text;

            if (UnCheckedImage != null)
            {
                Content = ToggleImage;
            }
            else
            {
                Content = ToggleLabel;
            }
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();

            if (UnCheckedImage != null)
            {
                ToggleImage.Source = UnCheckedImage;
                Content = ToggleImage;
            }
            else
            {
                ToggleLabel.Text = Text;
                Content = ToggleLabel;
            }
        }

        private static async void OnCheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var toggleButton = (ToggleButton)bindable;

            if (Equals(newValue, null) && !Equals(oldValue, null))
                return;

            if (toggleButton.Checked)
            {
                toggleButton.ToggleImage.Source = toggleButton.CheckedImage;

                toggleButton.ToggleLabel.Text = toggleButton.Text;
                toggleButton.ToggleLabel.Opacity = 1.0;
            }
            else
            {
                toggleButton.ToggleImage.Source = toggleButton.UnCheckedImage;

                toggleButton.ToggleLabel.Text = toggleButton.Text;
                toggleButton.ToggleLabel.Opacity = 0.5;
            }


            if (toggleButton.ToggleImage.Source != null)
            {
                toggleButton.Content = toggleButton.ToggleImage;
            }
            else
            {
                toggleButton.Content = toggleButton.ToggleLabel;
            }

            if (toggleButton.Animate)
            {
                await toggleButton.ScaleTo(0.9, 50, Easing.Linear);
                await Task.Delay(100);
                await toggleButton.ScaleTo(1, 50, Easing.Linear);
            }
        }
    }
}