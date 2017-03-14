using System.Collections.Generic;
using BingMapsRESTToolkit;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using MSCorp.FirstResponse.WebApiDemo.Data.Enums;
using MSCorp.FirstResponse.WebApiDemo.MapService;
using System.Linq;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Context
{
    public static class Seed
    {
        private static int routeRadius = 2000;

        public static User[] GetUsers()
        {
            var users = new[]
            {
                new User{RoleName = "Supervisor", Name = "Evan Dodds",UserName = "edodds",UserRoleImage = "Adambig.jpg",PasswordHash = "edodds"},
                new User{RoleName = "Attending Officer", Name = "John Clarkson",UserName = "jclarkson",UserRoleImage = "Benbig.jpg",PasswordHash = "jclarkson"},
                new User{RoleName = "Officer", Name = "Ben Martens",UserName = "bmartens",UserRoleImage = "Charliebig.jpg",PasswordHash = "bmartens"},
                new User{RoleName = "Attending Officer", Name = "Scott Montgomery",UserName = "smontgomery",UserRoleImage = "smontgomery.jpg",PasswordHash = "smontgomery"}
            };

            return users;
        }

        public static IEnumerable<HeatMapPoint> GetHeatMap(City city)
        {
            var heatMap = new List<HeatMapPoint>();

            for (int i = 0; i < 100; i++)
            {
                var point = MapPointGenerator.GetPoint(new Coordinate { Latitude = city.Latitude, Longitude = city.Longitude });
                heatMap.Add(new HeatMapPoint
                {
                    CityId = city.Id,
                    Longitude = point.Longitude,
                    Latitude = point.Latitude
                });
            }

            return heatMap;
        }

        public static AmbulancePosition GetAmbulancePosition(City city)
        {
            var original = new Coordinate(city.Latitude, city.Longitude);
            var point = MapPointGenerator.GetPoint(original, 250);
            var ambulancePosition = new AmbulancePosition
            {
                CityId = city.Id,
                Latitude = point.Latitude,
                Longitude = point.Longitude
            };
            return ambulancePosition;
        }

        public static Responder[] GetResponders(City city)
        {
            var original = new Coordinate(city.Latitude, city.Longitude);

            var responders = new[]
            {
              new Responder { CityId = city.Id, ResponderDepartment = DepartmentType.Fire,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, ResponderDepartment = DepartmentType.Ambulance,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, ResponderDepartment = DepartmentType.Police,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, ResponderDepartment = DepartmentType.Ambulance,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, ResponderDepartment = DepartmentType.Fire,Status = ResponseStatus.Available},
              new Responder { CityId = city.Id, ResponderDepartment = DepartmentType.Police,Status = ResponseStatus.Available}
            };

            foreach (var res in responders)
            {
                var point = MapPointGenerator.GetPoint(original);
                res.Longitude = point.Longitude;
                res.Latitude = point.Latitude;
                res.Route = GetResponderRoute(point);
            }

            return responders;
        }

        //Id    Name
        //24	Chicago
        //25	Johannesburg
        //26	Frankfurt
        //27	Washington, DC
        //28	Shingapore
        //29	Bangalore
        //30	Milan
        //31	Amsterdam
        //32	Birminghan
        //33	Copenhagen
        //34	Seoul
        //35	Seattle

        public static City[] GetCities()
        {
            var cities = new[]
            {
                new City
                {
                    Id = 24,
                    Name = "Chicago", Latitude = 41.880367, Longitude = -87.732912,
                    CityImage = "Chicago", EventDate = "January 19-20, 2017",
                    PhoneFormat = "(312) ###-####", PhoneLength = 7, PhoneMasked = "(312) XXX-XXXX"
                },
                new City
                {
                    Id = 25,
                    Name = "Johannesburg", Latitude = -26.2041028, Longitude = 28.047305100000017,
                    CityImage = "Johannesburg", EventDate = "February 6-7, 2017",
                    PhoneFormat = "(11) ###-####", PhoneLength = 7, PhoneMasked = "(11) XXX-XXXX"
                },
                new City
                {
                    Id = 26,
                    Name = "Frankfurt", Latitude = 50.1109221, Longitude = 8.682126700000026,
                    CityImage = "Frankfurt", EventDate = "February 9-10, 2017",
                    PhoneFormat = "(69) ###-###", PhoneLength = 6, PhoneMasked = "(69) XXX-XXX"
                },
                new City
                {
                    Id = 27,
                    Name = "Washington, DC", Latitude = 38.9071923, Longitude = -77.03687070000001,
                    CityImage = "Washington", EventDate = "March 6-7, 2017",
                    PhoneFormat = "(202) ###-####", PhoneLength = 7, PhoneMasked = "(202) XXX-XXXX"
                },
                new City
                {
                    Id = 28,
                    Name = "Shingapore", Latitude = 1.35115, Longitude = 103.87268,
                    CityImage = "Singapore", EventDate = "March 13-14, 2017",
                    PhoneFormat = "####-####", PhoneLength = 8, PhoneMasked = "XXXX-XXXX"
                },
                new City
                {
                    Id = 29,
                    Name = "Bangalore", Latitude = 12.966763, Longitude = 77.587637,
                    CityImage = "Bangalore", EventDate = "March 16-17, 2017",
                    PhoneFormat = "(80) ####-####", PhoneLength = 7, PhoneMasked = "(80) XXXX-XXXX"

                },
                new City
                {
                    Id = 30,
                    Name = "Milan", Latitude = 45.468124, Longitude = 9.182600,
                    CityImage = "Milan", EventDate = "March 20-21, 2017",
                    PhoneFormat = "(911) ####-####", PhoneLength = 8, PhoneMasked = "(911) XXXX-XXXX"
                },
                new City
                {
                    Id = 31,
                    Name = "Amsterdam", Latitude = 52.332933, Longitude = 4.885974,
                    CityImage = "Amsterdam", EventDate = "March 23-24, 2017",
                    PhoneFormat = "(85) ###-####", PhoneLength = 7, PhoneMasked = "(85) XXX-XXXX"
                },
                new City
                {
                    Id = 32,
                    Name = "Birminghan", Latitude = 52.4862, Longitude = -1.8904,
                    CityImage = "Birmingham", EventDate = "March 27-28, 2017",
                    PhoneFormat = "(871) ###-####", PhoneLength = 7, PhoneMasked = "(871) XXX-XXXX"
                },
                new City
                {
                    Id = 33,
                    Name = "Copenhagen", Latitude = 55.675055, Longitude = 12.500027,
                    CityImage = "Copenhagen", EventDate = "March 30-31, 2017",
                    PhoneFormat = "(78) ###-###", PhoneLength = 6, PhoneMasked = "(78) XXX-XXX"
                },
                new City
                {
                    Id = 34,
                    Name = "Seoul", Latitude = 37.566535, Longitude = 126.97796919999996,
                    CityImage = "Seoul", EventDate = "April 27-28, 2017",
                    PhoneFormat = "(2) ####-####", PhoneLength = 8, PhoneMasked = "(2) XXXX-XXXX"
                },
                new City {
                    Id = 35,
                    Name = "Seattle",
                    Latitude = 47.588400,
                    Longitude = -122.035594,
                    CityImage = "Seattle",
                    PhoneMasked = "(555) XXX-XXXX",
                    PhoneFormat = "(555) ###-####",
                    PhoneLength = 7,
                    EventDate = ""
                }
            };

            cities.ToList().ForEach(city =>
            {
                var ambulancePosition = Seed.GetAmbulancePosition(city);
                city.AmbulancePosition = ambulancePosition;
            });

            return cities;
        }

        public static ResponderRoute GetResponderRoute(Coordinate startPoint)
        {
            var endPoint = MapPointGenerator.GetPoint(startPoint, routeRadius);
            var routePoints = RouteGenerator.GetRoute(startPoint, endPoint);
            var responderRoute = new ResponderRoute();

            if (routePoints != null)
            {
                foreach (var r in routePoints)
                {
                    responderRoute.RoutePoints.Add(new RoutePoint
                    {
                        Latitude = r[0],
                        Longitude = r[1]
                    });
                }
            }

            return responderRoute;
        }
    }
}