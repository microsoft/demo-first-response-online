using Xamarin.Forms;

namespace MSCorp.FirstResponse.Client.Controls
{
    public class RoundedBoxView : BoxView
    {
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create("CornerRadius", 
                typeof(double), typeof(RoundedBoxView), default(double));

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create("BorderThickness",
                typeof(int), typeof(RoundedBoxView), 0);

        public int BorderThickness
        {
            get { return (int)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public static readonly BindableProperty RoundedBackgroudColorProperty =  
            BindableProperty.Create("RoundedBackgroudColor",
                typeof(Color), typeof(RoundedBoxView), Color.White);

        public Color RoundedBackgroudColor
        {
            get { return (Color)GetValue(RoundedBackgroudColorProperty); }
            set { SetValue(RoundedBackgroudColorProperty, value); }
        }
    }
}