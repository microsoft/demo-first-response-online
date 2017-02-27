using MSCorp.FirstResponse.Client.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace MSCorp.FirstResponse.Client.UWP.Extensions
{
    public static class BorderExtensions
    {
        public static void InitializeFrom(this Border nativeControl, RoundedBoxView formsControl)
        {
            if (nativeControl == null || formsControl == null)
                return;

            nativeControl.Height = formsControl.HeightRequest;
            nativeControl.Width = formsControl.WidthRequest;
            nativeControl.UpdateCornerRadius(formsControl.CornerRadius);
            nativeControl.UpdateBorderColor(formsControl.RoundedBackgroudColor);

            var rectangle = new Rectangle();

            rectangle.InitializeFrom(formsControl);

            nativeControl.Child = rectangle;
        }

        public static void UpdateFrom(this Border nativeControl, RoundedBoxView formsControl,
          string propertyChanged)
        {
            if (nativeControl == null || formsControl == null)
                return;

            if (propertyChanged == RoundedBoxView.CornerRadiusProperty.PropertyName)
            {
                nativeControl.UpdateCornerRadius(formsControl.CornerRadius);

                var rect = nativeControl.Child as Rectangle;

                if (rect != null)
                {
                    rect.UpdateCornerRadius(formsControl.CornerRadius);
                }

            }

            if (propertyChanged == RoundedBoxView.RoundedBackgroudColorProperty.PropertyName)
            {
                nativeControl.Background = formsControl.RoundedBackgroudColor.ToBrush();
            }

            if (propertyChanged == RoundedBoxView.BorderThicknessProperty.PropertyName)
            {
                var rect = nativeControl.Child as Rectangle;

                if (rect != null)
                {
                    rect.UpdateBorderThickness(formsControl.BorderThickness, formsControl.HeightRequest, formsControl.WidthRequest);
                }
            }
        }

        private static void UpdateCornerRadius(this Border nativeControl, double cornerRadius)
        {
            var relativeBorderCornerRadius = cornerRadius * 1.25;

            nativeControl.CornerRadius = new CornerRadius(relativeBorderCornerRadius);
        }

        public static void UpdateBorderColor(this Border nativeControl, Xamarin.Forms.Color backgroundColor)
        {
            nativeControl.Background = backgroundColor.ToBrush();
        }
    }
}