namespace MSCorp.FirstResponse.WebApiDemo.Data.Entities
{
    public class RoutePoint
    {
        public int Id { get; set; }
        public int ResponderRouteId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
