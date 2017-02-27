using Foundation;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.iOS.Extensions;
using MSCorp.FirstResponse.Client.iOS.Renderers;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedBoxView), typeof(RoundedBoxViewRenderer))]
namespace MSCorp.FirstResponse.Client.iOS.Renderers
{
    [Preserve(AllMembers = true)]
    public class RoundedBoxViewRenderer : BoxRenderer
    {
        public static void Init()
        {
            var temp = DateTime.Now;
        }

        private RoundedBoxView _formControl
        {
            get { return Element as RoundedBoxView; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
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