using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Droid.Models;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;

namespace MSCorp.FirstResponse.Client.Droid.Maps
{
    public class RouteManager : AbstractRouteManager
    {
        private readonly GoogleMap _nativeMap;
        private Polyline _currentUserRoute;

        public RouteManager(GoogleMap nativeMap, CustomMap formsMap, MarkerManager pushpinManager) 
            : base(formsMap, pushpinManager)
        {
            _nativeMap = nativeMap;
        }

        public override async Task<IEnumerable<Geoposition>> CalculateRoute(Geoposition from, Geoposition to)
        {
            PolylineOptions polyline = await RequestRoutePolyline(from, to);

            var positions = polyline?.Points.Select(CoordinateConverter.ConvertToAbstraction) ?? Enumerable.Empty<Geoposition>();

            return positions;
        }

        public override void ClearAllUserRoutes()
        {
            _currentUserRoute?.Remove();
            _currentUserRoute = null;
        }

        public override IEnumerable<Geoposition> GetCurrentUserRoute()
        {
            return _currentUserRoute?.Points.Select(CoordinateConverter.ConvertToAbstraction)
                ?? Enumerable.Empty<Geoposition>();
        }

        protected override void DrawRouteInMap(IEnumerable<Geoposition> positions)
        {
            var polyLine = new PolylineOptions();
            
            foreach(var position in positions)
            {
                LatLng nativePosition = CoordinateConverter.ConvertToNative(position);
                polyLine.Add(nativePosition);
            }

            polyLine.InvokeColor(FormsMap.RouteColor.ToAndroid());

            _currentUserRoute = _nativeMap.AddPolyline(polyLine);
        }

        private async Task<PolylineOptions> RequestRoutePolyline(Geoposition from, Geoposition to)
        {
            // Origin of route
            string originQueryParam = $"origin={from.Latitude.ToString(CultureInfo.InvariantCulture)},{from.Longitude.ToString(CultureInfo.InvariantCulture)}";

            // Destination of route
            string destinationQueryParam = $"destination={to.Latitude.ToString(CultureInfo.InvariantCulture)},{to.Longitude.ToString(CultureInfo.InvariantCulture)}";

            // Sensor enabled
            string sensor = "sensor=false";

            // Auth
            string key = "key=AIzaSyDDKvhUqz1fnEImpiC8zrflraQdgo8PaMo";

            RootRouteModel routeData = default(RootRouteModel);
            try
            {
                // Building the parameters to the web service
                string parameters = string.Join("&", new[] { originQueryParam, destinationQueryParam, sensor, key });

                UriBuilder uri = new UriBuilder("https://maps.googleapis.com/maps/api/directions/json");
                uri.Query = parameters;

                var requestProvider = new RequestProvider();
                routeData = await requestProvider.GetAsync<RootRouteModel>(uri.ToString());
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error calling routes api from google: {ex}");
            }

            PolylineOptions polylineOptions = new PolylineOptions();

            if (routeData?.Routes?.Any() == true)
            {
                var route = routeData.Routes.FirstOrDefault();

                var polylinePoints = route.Polyline.Points;

                foreach (var step in route.Steps)
                {
                    var points = DecodePolyline(polylinePoints);

                    foreach (var point in points)
                    {
                        polylineOptions.Add(point);
                    }
                }
            }

            return polylineOptions;
        }

        /// <summary>
        /// More information: https://agileapp.co/xamarin-forms-maps-polyline-route-highlighted-google-api
        /// </summary>
        public static List<LatLng> DecodePolyline(string encodedPoints)
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

            List<LatLng> polylinesPosition = new List<LatLng>();

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

                polylinesPosition.Add(new LatLng(Convert.ToDouble(currentLat) / 1E5, Convert.ToDouble(currentLng) / 1E5));
            }

            return (polylinesPosition);
        }
    }
}