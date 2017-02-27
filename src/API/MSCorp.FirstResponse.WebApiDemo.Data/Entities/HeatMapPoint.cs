namespace MSCorp.FirstResponse.WebApiDemo.Data.Entities
{
    public class HeatMapPoint
    {
        public int Id { get; set; }
        public  int CityId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
