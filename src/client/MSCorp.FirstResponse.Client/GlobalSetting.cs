using System;

namespace MSCorp.FirstResponse.Client
{
    public class GlobalSetting
    {
        public const string IncidentJsonDataFile = "Data/IncidentData.json";
        public const string ResponderJsonDataFile = "Data/ResponderData.json";
        public const string TicketJsonDataFile = "Data/TicketData.json";
        public const string ResponderRoutesJsonDataFile = "Data/ResponderRoutes.json";
        public const string UserRolesJsonDataFile = "Data/UserRoles.json";
        public const string SuspectJsonDataFile = "Data/SuspectData.json";
        public const string HeatPointsJsonDataFile = "Data/HeatData.json";

		public const string ServiceEndpoint = "http://localhost:50002/";

        public const bool UseMockServiceDefault = false;
        public const int DefaultMockCityId= 0;
        public const int DefaultCityId = 35;
        public const double UserLatitude = 47.609093;
        public const double UserLongitude = -122.015057;
        public const double AmbulanceLatitude = 47.603350;
        public const double AmbulanceLongitude = -122.019949;

        public const string BingMapsAPIKey = "40mgB3YEUPMAewdi02Hm~HM0JyrqUSuQqAz9WoBg0Iw~Ar-hXz2S6oaN-ZJRM3VzCSS1lpPhUlNmParkTwh6zuaG7AcSb9j2N0Oe2wRpr5qz";

        public static TimeSpan TimeToResolve { get; } = TimeSpan.FromSeconds(30);

        public static readonly TimeSpan UpdateInterval = TimeSpan.FromMilliseconds(10);

        public const double ZoomLevel = 14.1;
        public const double IncidentZoomLevel = 16;
        public const double MovementSpeed = 200;
    }
}