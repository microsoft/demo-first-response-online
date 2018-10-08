using Newtonsoft.Json;

namespace MSCorp.FirstResponse.Client.Maps.Routes.GoogleApi
{
    public class RoutePolylineModel
    {
        [JsonProperty("points")]
        public string Points { get; set; }
    }
}