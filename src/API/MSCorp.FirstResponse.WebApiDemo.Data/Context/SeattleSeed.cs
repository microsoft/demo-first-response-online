using System.Collections.Generic;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using MSCorp.FirstResponse.WebApiDemo.Data.Enums;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Context
{
    public static class SeattleSeed
    {
        public static ResponderRoute[] GetResponderRoutes()
        {
            var responderRoutes = new[]
            {
                new ResponderRoute { RoutePoints = new List<RoutePoint>
                {
                    new RoutePoint { Latitude = 47.588288, Longitude = -122.024835},
                    new RoutePoint { Latitude = 47.587962, Longitude = -122.013098},
                    new RoutePoint { Latitude = 47.602932, Longitude = -122.013862},
                    new RoutePoint { Latitude = 47.601701, Longitude = -122.035668},
                    new RoutePoint { Latitude = 47.588451, Longitude = -122.035609}
                }},
                  new ResponderRoute { RoutePoints = new List<RoutePoint>
                {
                    new RoutePoint { Latitude = 47.605526, Longitude = -122.036156},
                    new RoutePoint { Latitude = 47.606307, Longitude = -122.048988},
                    new RoutePoint { Latitude = 47.623685, Longitude = -122.051807},
                    new RoutePoint { Latitude = 47.620575, Longitude = -122.035692},
                    new RoutePoint { Latitude = 47.622022, Longitude = -122.024985}
                }}
            };

            return responderRoutes;
        }

        public static Responder[] GetResponders(City city, int routeId1, int routeId2)
        {
            var responders = new[]
            {
              new Responder { CityId = city.Id, RouteId = routeId1,Latitude = 47.594612, Longitude = -122.026981, ResponderDepartment = DepartmentType.Fire,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, RouteId = routeId2,Latitude = 47.578817, Longitude = -122.051171, ResponderDepartment = DepartmentType.Ambulance,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, RouteId = routeId1,Latitude = 47.587962, Longitude = -122.013098, ResponderDepartment = DepartmentType.Police,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, RouteId = routeId2,Latitude = 47.602932, Longitude = -122.013862, ResponderDepartment = DepartmentType.Ambulance,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, RouteId = routeId1,Latitude = 47.601701, Longitude = -122.013862, ResponderDepartment = DepartmentType.Fire,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, RouteId = routeId2,Latitude = 47.588451, Longitude = -122.035609, ResponderDepartment = DepartmentType.Police,Status = ResponseStatus.Available}
            };

            return responders;
        }
    }
}