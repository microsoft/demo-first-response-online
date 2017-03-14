using System;
using System.ComponentModel;
using MapKit;
using MSCorp.FirstResponse.Client.Models;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Annotations
{
    public class ResponderAnnotationView : MKAnnotationView
    {
        public const string CustomReuseIdentifier = nameof(ResponderAnnotationView);

        private ResponderIconView _iconView;

        private ResponderModel _responder;

        public ResponderModel Responder
        {
            get
            {
                return _responder;
            }

            set
            {
                _responder = value;
                UpdateData();
            }
        }

        public ResponderAnnotationView(IMKAnnotation annotation, ResponderModel responder)
            : base(annotation, CustomReuseIdentifier)
        {
            if (responder != null) {
                responder.PropertyChanged += UpdateColor;
            }

            Responder = responder;
        }

        private void UpdateColor(object sender, PropertyChangedEventArgs e)
        {
            if (_iconView != null)
            {
                _iconView.BackgroundColor = (sender as ResponderModel).StatusColor.ToUIColor();
                _iconView.LoadResponderData(Responder);
            }
        }

        public override void MovedToSuperview()
        {
            base.MovedToSuperview();

            _iconView = ResponderIconView.Create();

			var width = _iconView.Frame.Size.Width;
            _iconView.Frame = new CoreGraphics.CGRect(0, 0, width, width);
            _iconView.Center = new CoreGraphics.CGPoint
            {
                X = Frame.Size.Width / 2,
                Y = Frame.Size.Height / 2
            };

            _iconView.Layer.CornerRadius = width / 2;
            _iconView.Layer.BorderWidth = 2.0f;
            _iconView.Layer.BorderColor = UIColor.White.CGColor;

			UpdateData();
            AddSubview(_iconView);
        }

        private void UpdateData()
        {
            if (_iconView != null)
            {
                _iconView.BackgroundColor = Responder.StatusColor.ToUIColor();
                _iconView.LoadResponderData(Responder);
            }
        }
    }
}