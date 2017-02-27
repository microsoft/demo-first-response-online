using Xamarin.Forms;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Droid.Renderers;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;
using MSCorp.FirstResponse.Client.Droid.Extensions;

[assembly: ExportRenderer(typeof(RoundedBoxView), typeof(RoundedBoxViewRenderer))]
namespace MSCorp.FirstResponse.Client.Droid.Renderers
{
    public class RoundedBoxViewRenderer
        : ViewRenderer<RoundedBoxView, Android.Views.View>
    {
        public static void Init()
        {
        }

        private RoundedBoxView _formControl
        {
            get { return Element; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<RoundedBoxView> e)
        {
            base.OnElementChanged(e);

            this.InitializeFrom(_formControl);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            this.UpdateFrom(_formControl, e.PropertyName);
        }
    }
}