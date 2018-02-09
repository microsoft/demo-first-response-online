using System.Collections.Generic;
using Newtonsoft.Json;

namespace MSCorp.FirstResponse.Client.Maps.Routes.GoogleApi
{
    public class RootRouteModel
    {
        [JsonProperty("routes")]
        public List<RouteModel> Routes { get; set; }

        public string Status { get; set; }
    }
}