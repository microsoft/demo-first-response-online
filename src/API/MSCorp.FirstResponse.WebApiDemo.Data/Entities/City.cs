using System.Collections.Generic;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CityImage { get; set; }
        public string EventDate { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int PhoneLength { get; set; }
        public string PhoneMasked { get; set; }
        public string PhoneFormat { get; set; }
        public ICollection<Responder> Responders { get; set; } = new List<Responder>();
        public ICollection<HeatMapPoint> HeatMapPoints { get; set; } = new List<HeatMapPoint>();
        public AmbulancePosition AmbulancePosition { get; set; }
    }
}
