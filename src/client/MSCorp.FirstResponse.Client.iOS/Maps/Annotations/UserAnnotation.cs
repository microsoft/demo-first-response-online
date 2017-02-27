using CoreLocation;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Annotations
{
    public class UserAnnotation : BaseAnnotation
    {
        public UserAnnotation(CLLocationCoordinate2D coordinate) : base(coordinate)
        {
        }
    }
}