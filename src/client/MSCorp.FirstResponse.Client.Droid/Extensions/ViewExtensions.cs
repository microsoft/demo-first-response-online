using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using MSCorp.FirstResponse.Client.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace MSCorp.FirstResponse.Client.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static void InitializeFrom(this Android.Views.View nativeControl, RoundedBoxView formsControl)
        {
            if (nativeControl == null || formsControl == null)
                return;

            var background = new GradientDrawable();

            background.SetColor(formsControl.RoundedBackgroudColor.ToAndroid());

            if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBean)
            {
                nativeControl.Background = background;
            }
            else
            {
                nativeControl.SetBackgroundDrawable(background);
            }

            nativeControl.UpdateCornerRadius(formsControl.CornerRadius);
            nativeControl.UpdateBorder(formsControl.RoundedBackgroudColor, formsControl.BorderThickness);
            nativeControl.UpdateSize(Convert.ToInt32(formsControl.WidthRequest), Convert.ToInt32(formsControl.HeightRequest));
        }

        public static void UpdateFrom(this Android.Views.View nativeControl, RoundedBoxView formsControl,
          string propertyChanged)
        {
            if (nativeControl == null || formsControl == null)
                return;

            if (propertyChanged == RoundedBoxView.CornerRadiusProperty.PropertyName)
            {
                nativeControl.UpdateCornerRadius(formsControl.CornerRadius);
            }
            if (propertyChanged == VisualElement.BackgroundColorProperty.PropertyName)
            {
                var background = nativeControl.Background as GradientDrawable;

                if (background != null)
                {
                    background.SetColor(formsControl.BackgroundColor.ToAndroid());
                }
            }

            if (propertyChanged == RoundedBoxView.RoundedBackgroudColorProperty.PropertyName)
            {
                var background = nativeControl.Background as GradientDrawable;

                if (background != null)
                {
                    background.SetColor(formsControl.RoundedBackgroudColor.ToAndroid());
                }
            }
        }

        public static void UpdateBorder(this Android.Views.View nativeControl, Xamarin.Forms.Color color, int thickness)
        {
            var backgroundGradient = nativeControl.Background as GradientDrawable;

            if (backgroundGradient != null)
            {
                var relativeBorderThickness = thickness * 3;
                backgroundGradient.SetStroke(relativeBorderThickness, color.ToAndroid());
            }
        }

        public static void UpdateCornerRadius(this Android.Views.View nativeControl, double cornerRadius)
        {
            var backgroundGradient = nativeControl.Background as GradientDrawable;

            if (backgroundGradient != null)
            {
                var relativeCornerRadius = (float)(cornerRadius * 3.7);
                backgroundGradient.SetCornerRadius(relativeCornerRadius);
            }
        }

        public static void UpdateSize(this Android.Views.View nativeControl, int height, int width)
        {
            var backgroundGradient = nativeControl.Background as GradientDrawable;

            if (backgroundGradient != null)
            {
                backgroundGradient.SetSize(width, height);
            }
        }

        public static Bitmap AsBitmap(this Android.Views.View view, Context context, int width, int height)
        {
            DisplayMetrics displayMetrics = new DisplayMetrics();
            view.Measure(displayMetrics.WidthPixels, displayMetrics.HeightPixels);
            view.Layout(0, 0, displayMetrics.WidthPixels, displayMetrics.HeightPixels);

            view.BuildDrawingCache();
            Bitmap bitmap = Bitmap.CreateBitmap(displayMetrics, width, height, Bitmap.Config.Argb8888);

            Canvas canvas = new Canvas(bitmap);
            view.Draw(canvas);

            return bitmap;
        }
    }
}