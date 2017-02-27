using MSCorp.FirstResponse.Client.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace MSCorp.FirstResponse.Client.iOS.Extensions
{
    public static class UIViewExtensions
    {
        public static void InitializeFrom(this UIView nativeControl, RoundedBoxView formsControl)
        {
            if (nativeControl == null || formsControl == null)
                return;

            float size = (float)formsControl.WidthRequest;
            nativeControl.Bounds = new CoreGraphics.CGRect(0, 0, size, size);
            nativeControl.Layer.CornerRadius = size / 2;            
            nativeControl.Layer.MasksToBounds = true;

            nativeControl.UpdateColor(formsControl.RoundedBackgroudColor, formsControl.BorderThickness);  
        }

        public static void UpdateFrom(this UIView nativeControl, RoundedBoxView formsControl,
          string propertyChanged)
        {
            if (nativeControl == null || formsControl == null)
                return;

            if (propertyChanged == RoundedBoxView.CornerRadiusProperty.PropertyName)
            {
                nativeControl.Layer.CornerRadius = (float)formsControl.CornerRadius;
            }

            if (propertyChanged == RoundedBoxView.RoundedBackgroudColorProperty.PropertyName)
            {
                nativeControl.UpdateColor(formsControl.RoundedBackgroudColor, formsControl.BorderThickness);
            }

            if (propertyChanged == RoundedBoxView.BorderThicknessProperty.PropertyName)
            {
                nativeControl.UpdateColor(formsControl.RoundedBackgroudColor, formsControl.BorderThickness);
            }
        }

        public static void UpdateColor(this UIView nativeControl, Color color, int thickness)
        {
            nativeControl.Layer.BackgroundColor = color.ToCGColor();
            nativeControl.Layer.BorderColor = color.ToCGColor();
            nativeControl.Layer.BorderWidth = thickness;
        }
    }
}