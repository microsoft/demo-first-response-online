using Newtonsoft.Json;

namespace MSCorp.FirstResponse.Client.Droid.Models
{
    public class RouteLocationModel
    {
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("lng")]
        public double Longitude { get; set; }
    }
}