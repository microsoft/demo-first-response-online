using System.Collections.Generic;
using BingMapsRESTToolkit;

namespace MSCorp.FirstResponse.WebApiDemo.MapService
{
    public class RouteGenerator
    {
        private static string _key = "Aj8J8KEAFCZQ6LYzF5h2M01FT7bzptBGVP8ku8y80bB8i8xfeOtT8d - a3JVm3xJe";

        public static double[][] GetRoute(Coordinate start, Coordinate end)
        {

            SimpleWaypoint originPoint = new SimpleWaypoint
            {
                Coordinate = start
            };

            SimpleWaypoint endPoint = new SimpleWaypoint
            {
                Coordinate = end
            };

            var r = ServiceManager.GetResponseAsync(new RouteRequest()
            {
                BingMapsKey = _key,
                RouteOptions = new RouteOptions
                {
                    RouteAttributes = new List<RouteAttributeType>
                    {
                        RouteAttributeType.RoutePath
                    }
                },
                Waypoints = new List<SimpleWaypoint>() { originPoint, endPoint }

            }).GetAwaiter().GetResult();

            if (r != null && r.ResourceSets != null &&
               r.ResourceSets.Length > 0 &&
               r.ResourceSets[0].Resources != null &&
               r.ResourceSets[0].Resources.Length > 0)
            {
                var res = (Route) r.ResourceSets[0].Resources[0];
                return res.RoutePath.Line.Coordinates;
            }

            return null;
        }
    }
}
