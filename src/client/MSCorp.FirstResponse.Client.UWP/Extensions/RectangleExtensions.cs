using MSCorp.FirstResponse.Client.Controls;
using Windows.UI.Xaml.Shapes;

namespace MSCorp.FirstResponse.Client.UWP.Extensions
{
    public static class RectangleExtensions
    {
        public static void InitializeFrom(this Rectangle nativeControl, RoundedBoxView formsControl)
        {
            if (nativeControl == null || formsControl == null)
                return;

            nativeControl.UpdateCornerRadius(formsControl.CornerRadius);
            nativeControl.UpdateBorderThickness(formsControl.BorderThickness, formsControl.HeightRequest, formsControl.WidthRequest);
        }

        public static void UpdateCornerRadius(this Rectangle nativeControl, double cornerRadius)
        {
            var relativeBorderCornerRadius = cornerRadius * 1.25;

            nativeControl.RadiusX = relativeBorderCornerRadius;
            nativeControl.RadiusY = relativeBorderCornerRadius;
        }

        public static void UpdateBorderThickness(this Rectangle nativeControl, int borderThickness, double formsControlHeightRequest, double formsControlWidthRequest)
        {
            var relativeBorderThickness = borderThickness * 1.7;

            var rectHeight = formsControlHeightRequest - relativeBorderThickness;
            var rectWidth = formsControlWidthRequest - relativeBorderThickness;

            nativeControl.Height = rectHeight;
            nativeControl.Width = rectWidth;
        }
    }
}