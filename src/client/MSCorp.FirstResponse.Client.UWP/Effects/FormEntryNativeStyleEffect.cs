using MSCorp.FirstResponse.Client.Effects;
using MSCorp.FirstResponse.Client.UWP.Effects;
using System;
using System.ComponentModel;
using System.Diagnostics;
using UI = Windows.UI;
using Xaml = Windows.UI.Xaml;
using Media = Windows.UI.Xaml.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Windows.UI.Xaml.Controls;

[assembly: ExportEffect(typeof(FormEntryNativeStyleEffect), "FormEntryNativeStyleEffect")]
namespace MSCorp.FirstResponse.Client.UWP.Effects
{
    public class FormEntryNativeStyleEffect : PlatformEffect
    {

        public object LineColorEffect { get; private set; }

        protected override void OnAttached()
        {
            try
            {
                if (Control is FormsComboBox)
                {
                    FormsComboBox control = Control as FormsComboBox;

                    // native picker style
                    control.Style = (Xaml.Style)Xaml.Application.Current.Resources["ComboBoxStyle"] as Xaml.Style;
                    control.ItemContainerStyle = Xaml.Application.Current.Resources["ComboBoxItemStyle"] as Xaml.Style;
                    if (!string.IsNullOrWhiteSpace(ApplyFormEntryNativeStyleEffect.GetPlaceHolderText(Element)))
                    {
                        control.PlaceholderText = ApplyFormEntryNativeStyleEffect.GetPlaceHolderText(Element);
                    }
                }
                else if (Control is TextBox)
                {
                    TextBox control = Control as TextBox;
                    control.Style = (Xaml.Style)Xaml.Application.Current.Resources["TextBoxStyle"] as Xaml.Style;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Cannot set property on attached control. Error: ", ex.Message);
            }
        }

        protected override void OnDetached()
        {
        }
    }
}
