using Newtonsoft.Json;

namespace MSCorp.FirstResponse.Client.Droid.Models
{
    public class RoutePolylineModel
    {
        [JsonProperty("points")]
        public string Points { get; set; }
    }
}