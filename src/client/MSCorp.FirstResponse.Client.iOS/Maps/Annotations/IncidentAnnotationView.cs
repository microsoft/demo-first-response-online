using System;
using CoreGraphics;
using MapKit;
using UIKit;
using MSCorp.FirstResponse.Client.Models;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Annotations
{
    public class IncidentAnnotationView : MKAnnotationView
    {
        public const string CustomReuseIdentifier = nameof(ResponderAnnotationView);

        public event EventHandler OnNavigationRequested;
        public event EventHandler OnClose;

        private MapIncidentInfoView _infoView;
        private IncidentModel _incident;

        public IncidentModel Incident
        {
            get
            {
                return _incident;
            }

            set
            {
                _incident = value;
                UpdateImage();
            }
        }

        public IncidentAnnotationView(IMKAnnotation annotation, IncidentModel incident)
            : base(annotation, CustomReuseIdentifier)
        {
            Incident = incident;
        }

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);

            if (selected)
            {
                _infoView = MapIncidentInfoView.Create();
                _infoView.UserInteractionEnabled = true;

				var desiredSize = _infoView.Frame.Size;
                var positionXOffset = -desiredSize.Width / 2 + Frame.Width / 2;

                var desiredPosition = new CGPoint(positionXOffset, -desiredSize.Height);
                _infoView.Frame = new CGRect(desiredPosition, desiredSize);
                _infoView.OnClose -= OnInfoViewClose;
                _infoView.OnClose += OnInfoViewClose;
                _infoView.OnNavigationRequested -= OnInfoViewNavigationRequest;
                _infoView.OnNavigationRequested += OnInfoViewNavigationRequest;

                _infoView.LoadIncidentData(Incident);

                AddSubview(_infoView);
            }
            else
            {
                _infoView.OnClose -= OnInfoViewClose;
                _infoView.OnNavigationRequested -= OnInfoViewNavigationRequest;
                _infoView.RemoveFromSuperview();
            }
        }

        public override bool PointInside(CGPoint point, UIEvent uievent)
        {
            return base.PointInside(point, uievent) || _infoView?.Frame.Contains(point) == true;
        }

        public override UIView HitTest(CGPoint point, UIEvent uievent)
        {
            var view = base.HitTest(point, uievent);

            if (view == null)
            {
                if (_infoView?.Frame.Contains(point) == true)
                {
                    view = _infoView;
                }
            }

            return view;
        }

        private void OnInfoViewNavigationRequest(object sender, EventArgs e)
        {
            var handler = OnNavigationRequested;

            handler?.Invoke(this, EventArgs.Empty);
        }

        private void OnInfoViewClose(object sender, EventArgs e)
        {
            var handler = OnClose;

            handler?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateImage()
        {
            switch (Incident.IncidentCategory)
            {
                case IncidentType.Alert:
                    Image = AnnotationImages.AlertImage;
                    break;
                case IncidentType.Animal:
                    Image = AnnotationImages.AnimalImage;
                    break;
                case IncidentType.Arrest:
                    Image = AnnotationImages.ArrestImage;
                    break;
                case IncidentType.Car:
                    Image = AnnotationImages.CarImage;
                    break;
                case IncidentType.Fire:
                    Image = AnnotationImages.FireImage;
                    break;
                case IncidentType.OfficerRequired:
                    Image = AnnotationImages.OfficerImage;
                    break;
                case IncidentType.Stranger:
                    Image = AnnotationImages.StrangerImage;
                    break;
                default:
                    break;
            }
        }
    }
}