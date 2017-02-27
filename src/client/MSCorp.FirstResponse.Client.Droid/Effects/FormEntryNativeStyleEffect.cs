using System;
using Android.Widget;
using Xamarin.Forms;
using MSCorp.FirstResponse.Client.Droid.Effects;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using MSCorp.FirstResponse.Client.Effects;

[assembly: ResolutionGroupName("FirstResponse")]
[assembly: ExportEffect(typeof(FormEntryNativeStyleEffect), "FormEntryNativeStyleEffect")]
namespace MSCorp.FirstResponse.Client.Droid.Effects
{
    public class FormEntryNativeStyleEffect : PlatformEffect
    {
        EditText control;

        protected override void OnAttached()
        {
            try
            {
                control = Control as EditText;
                control.SetBackgroundResource(Resource.Drawable.CustomPicker);
                control.SetPadding(10,0,10,0);
                control.SetHintTextColor(global::Android.Graphics.Color.Rgb(38, 50, 56));
                control.Gravity = GravityFlags.CenterVertical;
                control.SetForegroundGravity(GravityFlags.CenterVertical);
                control.SetTextColor(global::Android.Graphics.Color.White);
                if (!string.IsNullOrWhiteSpace(ApplyFormEntryNativeStyleEffect.GetPlaceHolderText(Element)))
                {
                    control.Hint = ApplyFormEntryNativeStyleEffect.GetPlaceHolderText(Element);
                }
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
    }
}