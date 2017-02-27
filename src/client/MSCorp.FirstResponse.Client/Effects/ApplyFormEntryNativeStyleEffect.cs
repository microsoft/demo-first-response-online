using System.Linq;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Effects
{
    public static class ApplyFormEntryNativeStyleEffect
    {
        public static readonly BindableProperty ApplyFormEntryNativeStyleProperty =
            BindableProperty.CreateAttached("ApplyFormEntryNativeStyle", typeof(bool), typeof(ApplyFormEntryNativeStyleEffect), false,
                propertyChanged: ApplyFormEntryNativeStyle);

        public static bool GetApplyNativeStyle(BindableObject view)
        {
            return (bool)view.GetValue(ApplyFormEntryNativeStyleProperty);
        }

        public static void SetApplyNativeStyle(BindableObject view, bool value)
        {
            view.SetValue(ApplyFormEntryNativeStyleProperty, value);
        }

        public static readonly BindableProperty PlaceHolderTextProperty =
            BindableProperty.CreateAttached("PlaceHolderText", typeof(string), typeof(ApplyFormEntryNativeStyleEffect), default(string));

        public static string GetPlaceHolderText(BindableObject view)
        {
            return (string)view.GetValue(PlaceHolderTextProperty);
        }

        public static void SetPlaceHolderText(BindableObject view, string value)
        {
            view.SetValue(PlaceHolderTextProperty, value);
        }

        private static void ApplyFormEntryNativeStyle(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;

            if (view == null)
            {
                return;
            }

            if ((bool)newValue)
            {
                view.Effects.Add(new FormEntryNativeStyleEffect());
            }
            else
            {
                var formEntryNativeStyleEffect = view.Effects.FirstOrDefault(e => e is FormEntryNativeStyleEffect);
                if (formEntryNativeStyleEffect != null)
                {
                    view.Effects.Remove(formEntryNativeStyleEffect);
                }
            }
        }
    }

    class FormEntryNativeStyleEffect : RoutingEffect
    {
        public FormEntryNativeStyleEffect() : base("FirstResponse.FormEntryNativeStyleEffect")
        {
        }
    }
}
