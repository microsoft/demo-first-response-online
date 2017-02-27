using System.Linq;
using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Effects
{
    public static class ApplyNativeStyleEffect
    {
        public static readonly BindableProperty ApplyNativeStyleProperty =
            BindableProperty.CreateAttached("ApplyNativeStyle", typeof(bool), typeof(ApplyNativeStyleEffect), false,
                propertyChanged: OnApplyNativeStyleChanged);

        public static bool GetApplyNativeStyle(BindableObject view)
        {
            return (bool)view.GetValue(ApplyNativeStyleProperty);
        }

        public static void SetApplyNativeStyle(BindableObject view, bool value)
        {
            view.SetValue(ApplyNativeStyleProperty, value);
        }

        private static void OnApplyNativeStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as View;

            if (view == null)
            {
                return;
            }

            bool hasShadow = (bool)newValue;

            if (hasShadow)
            {
                view.Effects.Add(new NativeStyleEffect());
            }
            else
            {
                var entryLineColorEffectToRemove = view.Effects.FirstOrDefault(e => e is NativeStyleEffect);
                if (entryLineColorEffectToRemove != null)
                {
                    view.Effects.Remove(entryLineColorEffectToRemove);
                }
            }
        }

        class NativeStyleEffect : RoutingEffect
        {
            public NativeStyleEffect() : base("FirstResponse.NativeStyleEffect")
            {
            }
        }
    }
}
