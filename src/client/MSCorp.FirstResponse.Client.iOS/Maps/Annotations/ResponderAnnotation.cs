using CoreLocation;
using MSCorp.FirstResponse.Client.Models;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Annotations
{
    public class ResponderAnnotation : BaseAnnotation
    {
        public ResponderModel Responder { get; private set; }

        public ResponderAnnotation(CLLocationCoordinate2D coordinate, ResponderModel responder)
            : base(coordinate)
        {
            Responder = responder;
        }
    }
}