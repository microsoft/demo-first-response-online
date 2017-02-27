using Newtonsoft.Json;

namespace MSCorp.FirstResponse.Client.Droid.Models
{
    public class RouteStepModel
    {
        [JsonProperty("end_address")]
        public string EndAddress { get; set; }

        [JsonProperty("end_location")]
        public RouteLocationModel EndLocation { get; set; }

        [JsonProperty("start_address")]
        public string StartAddress { get; set; }

        [JsonProperty("start_location")]
        public RouteLocationModel StartLocation { get; set; }

        [JsonProperty("polyline")]
        public RoutePolylineModel Polyline { get; set; }
    }
}