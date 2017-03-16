using Foundation;
using MSCorp.FirstResponse.Client.Converters;
using MSCorp.FirstResponse.Client.Models;
using ObjCRuntime;
using System;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace MSCorp.FirstResponse.Client.iOS
{
    public partial class MapIncidentInfoView : UIView
    {
        public event EventHandler OnNavigationRequested;
        public event EventHandler OnClose;

        public MapIncidentInfoView (IntPtr handle) : base (handle)
        {
        }

        public static MapIncidentInfoView Create()
        {
            var arr = NSBundle.MainBundle.LoadNib("MapIncidentInfoView", null, null);
            var v = Runtime.GetNSObject<MapIncidentInfoView>(arr.ValueAt(0));

            return v;
        }

        public override void AwakeFromNib()
        {
            NavigateButton.AddGestureRecognizer(new UITapGestureRecognizer(OnNavigateTapped));
            HeaderCloseButton.AddGestureRecognizer(new UITapGestureRecognizer(OnCloseTapped));
        }

        public void LoadIncidentData(IncidentModel incident)
        {
            DescriptionLabel.Text = incident.Description;
            DescriptionLabel.LineBreakMode = UILineBreakMode.TailTruncation;
            DescriptionLabel.Lines = 4;
            LocationLabel.Text = incident.Address;
            HeaderTitleLabel.Text = incident.Title;

            Header.BackgroundColor = incident.IncidentColor.ToUIColor();

            var converter = new IconsImageConverter();
            var iconImagePath = (string)converter.Convert(incident.IncidentCategory, typeof(string), null, System.Globalization.CultureInfo.DefaultThreadCurrentUICulture);

            HeaderIconImage.Image = UIImage.FromBundle(iconImagePath);
        }

        private void OnCloseTapped()
        {
            var handler = OnClose;

            handler?.Invoke(this, EventArgs.Empty);
        }

        private void OnNavigateTapped()
        {
            var handler = OnNavigationRequested;

            handler?.Invoke(this, EventArgs.Empty);
        }
    }
}