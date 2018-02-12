using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Maps.Routes.GoogleApi
{
    public class DrivingRouterProvider
    {
        public static string GoogleMapsApiKey { get; set; }

        public async Task<IEnumerable<Geoposition>> GetRoutePositionsAsync(Geoposition from, Geoposition to)
        {
            // Origin of route
            string originQueryParam = $"origin={from.Latitude.ToString(CultureInfo.InvariantCulture)},{from.Longitude.ToString(CultureInfo.InvariantCulture)}";

            // Destination of route
            string destinationQueryParam = $"destination={to.Latitude.ToString(CultureInfo.InvariantCulture)},{to.Longitude.ToString(CultureInfo.InvariantCulture)}";

            // Sensor enabled
            string sensor = "sensor=false";

            // Auth
            string key = $"key={GoogleMapsApiKey}";

            RootRouteModel routeData = default(RootRouteModel);
            try
            {
                // Building the parameters to the web service
                string parameters = string.Join("&", new[] { originQueryParam, destinationQueryParam, sensor, key });

                UriBuilder uri = new UriBuilder("https://maps.googleapis.com/maps/api/directions/json");
                uri.Query = parameters;

                var requestProvider = new Data.Base.RequestProvider();
                routeData = await requestProvider.GetAsync<RootRouteModel>(uri.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error calling routes api from google: {ex}");
            }

            List<Geoposition> positions = new List<Geoposition>();

            if (routeData?.Routes?.Any() == true)
            {
                var route = routeData.Routes.FirstOrDefault();

                var polylinePoints = route.Polyline.Points;

                foreach (var step in route.Steps)
                {
                    var points = DecodePolyline(polylinePoints);

                    foreach (var point in points)
                    {
                        positions.Add(point);
                    }
                }
            }

            return positions;
        }

        /// <summary>
        /// More information: https://agileapp.co/xamarin-forms-maps-polyline-route-highlighted-google-api
        /// </summary>
        private static List<Geoposition> DecodePolyline(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                throw new ArgumentNullException("encodedPoints");

            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            List<Models.Geoposition> positions = new List<Models.Geoposition>();

            while (index < polylineChars.Length)
            {
                // Calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // Calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                positions.Add(new Models.Geoposition
                {
                    Latitude = Convert.ToDouble(currentLat) / 1E5,
                    Longitude = Convert.ToDouble(currentLng) / 1E5
                });
            }

            return (positions);
        }
    }
}
