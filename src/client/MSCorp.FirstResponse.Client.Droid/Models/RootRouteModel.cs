using System.Collections.Generic;
using Newtonsoft.Json;

namespace MSCorp.FirstResponse.Client.Droid.Models
{
    public class RootRouteModel
    {
        [JsonProperty("routes")]
        public List<RouteModel> Routes { get; set; }

        public string Status { get; set; }
    }
}