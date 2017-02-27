using System;
using MSCorp.FirstResponse.Client.iOS.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("FirstResponse")]
[assembly: ExportEffect(typeof(NativeStyleEffect), "NativeStyleEffect")]
namespace MSCorp.FirstResponse.Client.iOS.Effects
{
    public class NativeStyleEffect : PlatformEffect
    {
        private UITextField control;

        protected override void OnAttached()
        {
            try
            {
                control = Control as UITextField;
                control.BorderStyle = UITextBorderStyle.None;
                control.TintColor = control.TextColor;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}