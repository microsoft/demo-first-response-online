using MSCorp.FirstResponse.Client.Helpers;
using System;

namespace MSCorp.FirstResponse.Client.Models
{
    public class TicketModel : ExtendedBindableObject
    {
        private string _violation;
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Type { get; set; }
        public string Officer { get; set; }
        public DateTime DateCreated { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int? Speed { get; set; }
        public int? MaxSpeed { get; set; }
        public string Notes { get; set; }
        public DriverModel Driver { get; set; }
        public VehicleModel Vehicle { get; set; }
        public SearchAreaModel SearchArea { get; set; }
        public Geoposition Location => new Geoposition { Latitude = Latitude, Longitude = Longitude };
        public string Violation {
            get
            {
                return _violation;
            }
            set
            {
                _violation = value;
                RaisePropertyChanged(() => Violation);
            }
        }
    }
}