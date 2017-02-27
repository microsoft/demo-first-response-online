using System.Collections.Generic;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Entities
{
    public class ResponderRoute
    {
        public int Id { get; set; }
        public ICollection<RoutePoint> RoutePoints { get; set; } = new List<RoutePoint>();
    }
}
