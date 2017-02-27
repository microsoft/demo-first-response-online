using System.Collections.Generic;

namespace MSCorp.FirstResponse.Client.Models
{
    public class RouteModel
    {
        public int Id { get; set; }
        public IList<Geoposition> RoutePoints { get; set; }
    }
}