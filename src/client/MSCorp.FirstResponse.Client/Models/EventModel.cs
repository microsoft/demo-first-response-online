using Newtonsoft.Json;

namespace MSCorp.FirstResponse.Client.Models
{
    public class EventModel
    {
        [JsonProperty("id")]
        public int CityId { get; set; }
        [JsonProperty("name")]
        public string CityName { get; set; }
        public string CityImage { get; set; } = "";
        public string EventDate { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Coordinates => $"{Latitude}, {Longitude}";
        public AmbulanceModel AmbulancePosition { get; set; }
    }
}
