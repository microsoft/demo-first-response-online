using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Xaml = Windows.UI.Xaml;
using MSCorp.FirstResponse.Client.UWP.Effects;

[assembly: ResolutionGroupName("FirstResponse")]
[assembly: ExportEffect(typeof(NativeStyleEffect), "NativeStyleEffect")]
namespace MSCorp.FirstResponse.Client.UWP.Effects
{
    public class NativeStyleEffect : PlatformEffect
    {
        TextBox control;

        protected override void OnAttached()
        {
            try
            {
                control = Control as TextBox;

                var style = Xaml.Application.Current.Resources["FormTextBoxStyle"] as Xaml.Style;
                control.Style = style;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
            control = null;
        }
    }
}
