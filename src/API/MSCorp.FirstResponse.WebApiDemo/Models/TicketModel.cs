using System;

namespace MSCorp.FirstResponse.WebApiDemo.Model
{
    public class TicketModel
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Type { get; set; }
        public string Violation { get; set; }
        public string Officer { get; set; }
        public DateTime DateCreated { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Speed { get; set; }
        public int MaxSpeed { get; set; }
        public string Notes { get; set; }
        public int DriverId { get; set; }
        public int VehicleId { get; set; }
    }
}
