using MSCorp.FirstResponse.Client.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MSCorp.FirstResponse.Client.Data
{
    public static class DataRepository
    {
        private static IList<IncidentModel> _incidentList;
        private static DeviceResponderUnit _userResponder;
        private static IList<RouteModel> _routes;
        private static IList<ResponderModel> _responderList;
        private static IList<UserRole> _userRoles;
        private static IList<SuspectModel> _suspectList;
        private static IList<Geoposition> _heatPointsList;

        public static IList<IncidentModel> LoadIncidentData()
        {
            return _incidentList ??
                   (_incidentList =
                       LoadData<IList<IncidentModel>>(GlobalSetting.IncidentJsonDataFile)
                           .OrderByDescending(x => x.Priority)
                           .ThenByDescending(x => x.ReceivedTime)
                           .ToList());
        }

        public static DeviceResponderUnit GetUser()
        {
            return _userResponder ?? (_userResponder = new DeviceResponderUnit
            {
                Id = 1,
                ResponderDepartment = DepartmentType.Responder,
                Status = ResponseStatus.Available
            });
        }

        public static IList<RouteModel> LoadRoutes()
        {
            return _routes ?? (_routes = LoadData<IList<RouteModel>>(GlobalSetting.ResponderRoutesJsonDataFile));
        }

        public static IList<Geoposition> LoadHeatData()
        {
            return _heatPointsList ?? (_heatPointsList = LoadData<IList<Geoposition>>(GlobalSetting.HeatPointsJsonDataFile));
        }

        public static IList<ResponderModel> LoadResponderData()
        {
            return _responderList ?? (_responderList = LoadData<IList<ResponderModel>>(GlobalSetting.ResponderJsonDataFile));
        }

        public static IList<UserRole> LoadUserRoles()
        {
            return _userRoles ?? (_userRoles = LoadData<IList<UserRole>>(GlobalSetting.UserRolesJsonDataFile));
        }

        public static IList<SuspectModel> LoadSuspectData()
        {
            return _suspectList ?? (_suspectList = LoadData<List<SuspectModel>>(GlobalSetting.SuspectJsonDataFile));
        }

        public static IList<EventModel> LoadEventsData()
        {
            return new List<EventModel>
            {
                new EventModel { CityId = 0, CityName = "Seattle", CityImage = "Seattle", EventDate = "", Latitude = 47.609093, Longitude = -122.015057, AmbulancePosition = new AmbulanceModel(){Latitude= GlobalSetting.AmbulanceLatitude, Longitude= GlobalSetting.AmbulanceLongitude}},
                new EventModel { CityId = 24, CityName = "Chicago", CityImage = "Chicago", EventDate = "January 19-20, 2017", Latitude = 41.880367, Longitude = -87.732912, AmbulancePosition = new AmbulanceModel(){Latitude = 41.880367, Longitude = -87.732912}},
                new EventModel { CityId = 25, CityName = "Johannesburg", CityImage = "Johannesburg", EventDate = "February 6-7, 2017", Latitude = -26.2041028, Longitude = 28.047305100000017, AmbulancePosition = new AmbulanceModel(){Latitude = -26.2041028, Longitude = 28.047305100000017}},
                new EventModel { CityId = 26, CityName = "Frankfurt", CityImage = "Frankfurt", EventDate = "February 9-10, 2017", Latitude = 50.1109221, Longitude = 8.682126700000026, AmbulancePosition = new AmbulanceModel(){Latitude = 50.1109221, Longitude = 8.682126700000026}},
                new EventModel { CityId = 27, CityName = "Washington, D.C", CityImage = "Washington", EventDate = "March 6-7, 2017", Latitude = 38.9071923, Longitude = -77.03687070000001, AmbulancePosition = new AmbulanceModel(){Latitude = 38.9071923, Longitude = -77.03687070000001}},
                new EventModel { CityId = 28, CityName = "Singapore", CityImage = "Singapore", EventDate = "March 13-14, 2017", Latitude = 1.35115, Longitude = 103.87268, AmbulancePosition = new AmbulanceModel(){Latitude = 1.35115, Longitude = 103.87268} },
                new EventModel { CityId = 29, CityName = "Bangalore", CityImage = "Bangalore", EventDate = "March 16-17, 2017", Latitude = 12.966763, Longitude = 77.587637, AmbulancePosition = new AmbulanceModel(){Latitude = 12.966763, Longitude = 77.587637} },
                new EventModel { CityId = 30, CityName = "Milan", CityImage = "Milan", EventDate = "March 20-21, 2017 ", Latitude = 45.468124, Longitude = 9.1826, AmbulancePosition = new AmbulanceModel(){Latitude = 45.468124, Longitude = 9.1826} },
                new EventModel { CityId = 31, CityName = "Amsterdam", CityImage = "Amsterdam", EventDate = "March 23-24, 2017", Latitude = 52.332933, Longitude = 4.885974, AmbulancePosition = new AmbulanceModel(){Latitude = 52.332933, Longitude = 4.885974} },
                new EventModel { CityId = 32, CityName = "Birmingham", CityImage = "Birmingham", EventDate = "March 27-28, 2017", Latitude = 52.4862, Longitude = -1.8904, AmbulancePosition = new AmbulanceModel(){Latitude = 52.4862, Longitude = -1.8904} },
                new EventModel { CityId = 33, CityName = "Copenhagen", CityImage = "Copenhagen", EventDate = "March 30-31, 2017", Latitude = 12.500027, Longitude = 55.675055, AmbulancePosition = new AmbulanceModel(){Latitude = 12.500027, Longitude = 55.675055} },
                new EventModel { CityId = 34, CityName = "Seoul", CityImage = "Seoul", EventDate = "April 27-28, 2017", Latitude = 37.566535, Longitude = 126.97796919999996, AmbulancePosition = new AmbulanceModel(){Latitude = 37.566535, Longitude = 126.97796919999996} },
            };
        }
        
        private static T LoadData<T>(string dataFileName)
        {
            var assembly = typeof(DataRepository).GetTypeInfo().Assembly;
            string preparedDataFileName = string.Format("MSCorp.FirstResponse.Client.{0}",
                dataFileName.Replace("/", "."));
            Stream stream = assembly.GetManifestResourceStream(preparedDataFileName);

            if(stream == null)
            {
                return default(T);
            }

            using (StreamReader sr = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
        }
    }
}