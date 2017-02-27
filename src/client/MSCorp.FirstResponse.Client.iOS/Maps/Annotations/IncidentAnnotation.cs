using CoreLocation;
using MSCorp.FirstResponse.Client.Models;

namespace MSCorp.FirstResponse.Client.iOS.Maps.Annotations
{
    public class IncidentAnnotation : BaseAnnotation
    {
        public IncidentModel Incident { get; private set; }

        public IncidentAnnotation(CLLocationCoordinate2D coordinate, IncidentModel incident)
            : base(coordinate)
        {
            Incident = incident;
        }
    }
}