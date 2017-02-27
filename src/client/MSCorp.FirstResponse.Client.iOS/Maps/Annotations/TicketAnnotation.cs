using CoreLocation;
using MSCorp.FirstResponse.Client.Models;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Annotations
{
    public class TicketAnnotation : BaseAnnotation
    {
        public TicketModel Ticket { get; private set; }

        public TicketAnnotation(CLLocationCoordinate2D coordinate, TicketModel ticket) 
            : base(coordinate)
        {
            Ticket = ticket;
        }
    }
}