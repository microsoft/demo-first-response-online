using System;
using MSCorp.FirstResponse.Client.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MSCorp.FirstResponse.Client.Effects;
using CoreGraphics;
using System.ComponentModel;
using Foundation;

[assembly: ExportEffect(typeof(FormEntryNativeStyleEffect), "FormEntryNativeStyleEffect")]
namespace MSCorp.FirstResponse.Client.iOS.Effects
{
    public class FormEntryNativeStyleEffect : PlatformEffect
    {
        private UITextField control;

        protected override void OnAttached()
        {
            try
            {
                control = Control as UITextField;
                UpdateLineColor();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            control = null;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == ApplyFormEntryNativeStyleEffect.ApplyFormEntryNativeStyleProperty.PropertyName || args.PropertyName == "Height")
            {
                Initialize();
                UpdateLineColor();
            }
        }

        private void Initialize()
        {
            var picker = Element as Picker;

            if (picker != null)
            {
                Control.Bounds = new CGRect(0, 0, picker.Width, picker.Height);
                InsertDropDownIcon();
                InsertPlaceHolder();
            }
        }

        private void UpdateLineColor()
        {
            if (control == null)
                return;

            // background color
            control.BackgroundColor = UIColor.Clear;

            // border line
            control.BorderStyle = UITextBorderStyle.Line;
            control.Layer.BorderWidth = 2.0f;
            control.Layer.MasksToBounds = true;
            control.Layer.BorderColor = UIColor.FromRGB(38, 50, 56).CGColor; //<Color x:Key="TerciaryDarkGray">#263238</Color>

            // padding on the left
            UIView paddingView = new UIView(new CGRect(0, 0, 5, 20));
            control.LeftView = paddingView;
            control.LeftViewMode = UITextFieldViewMode.Always;
        }

        private void InsertDropDownIcon()
        {
            // dropdown image
            control.RightViewMode = UITextFieldViewMode.Always;
            var image = new UIImageView(new UIImage("ico_dropdown"));
            image.Frame = new CGRect(0, 0, image.Image.Size.Width + 30, image.Image.Size.Height);
            image.ContentMode = UIViewContentMode.ScaleAspectFit;
            control.RightView = image;

        }

        private void InsertPlaceHolder()
        {
            if (!string.IsNullOrWhiteSpace(ApplyFormEntryNativeStyleEffect.GetPlaceHolderText(Element))) {
                var placeHolder = new NSAttributedString(ApplyFormEntryNativeStyleEffect.GetPlaceHolderText(Element), new UIStringAttributes() { ForegroundColor = UIColor.FromRGB(38, 50, 56) });
                control.AttributedPlaceholder = placeHolder;
            }
        }
    }
}
