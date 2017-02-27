using System;
using MapKit;
using MSCorp.FirstResponse.Client.Models;
using UIKit;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Annotations
{
    public class TicketAnnotationView : MKAnnotationView
    {
        public const string CustomReuseIdentifier = nameof(TicketAnnotationView);

        private TicketModel _ticket;

        public TicketModel Ticket
        {
            get
            {
                return _ticket;
            }

            set
            {
                _ticket = value;
                UpdateImage();
            }
        }

        public TicketAnnotationView(IMKAnnotation annotation, TicketModel ticketModel)
            : base(annotation, CustomReuseIdentifier)
        {
            Ticket = ticketModel;
        }

        private void UpdateImage()
        {
            // only traffic tickets are supported
            Image = AnnotationImages.CarImage;
        }
    }
}