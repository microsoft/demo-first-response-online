using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.UWP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Controls.Maps;

namespace MSCorp.FirstResponse.Client.UWP.Maps
{
    public class RouteManager : AbstractRouteManager
    {
        private readonly MapControl _nativeMap;
        private MapPolyline _currentUserRoute;

        public RouteManager(MapControl nativeMap, CustomMap formsMap, PushpinManager pushpinManager)
            : base(formsMap, pushpinManager)
        {
            _nativeMap = nativeMap;
        }

        public override async Task<IEnumerable<Models.Geoposition>> CalculateRoute(Models.Geoposition from, Models.Geoposition to)
        {
            Geopoint nativeFrom = CoordinateConverter.ConvertToNative(from);
            Geopoint nativeTo = CoordinateConverter.ConvertToNative(to);

            MapRouteFinderResult routeResult = await MapRouteFinder.GetDrivingRouteAsync(
                nativeFrom,
                nativeTo,
                MapRouteOptimization.Time,
                MapRouteRestrictions.None);

            List<Models.Geoposition> result = new List<Models.Geoposition>();
            IReadOnlyList<BasicGeoposition> routePositions = routeResult?.Route?.Path?.Positions;

            if (routePositions?.Any() == true)
            {
                foreach (BasicGeoposition position in routePositions)
                {
                    result.Add(new Models.Geoposition
                    {
                        Latitude = position.Latitude,
                        Longitude = position.Longitude
                    });
                }
            }

            return result;
        }

        public override void ClearAllUserRoutes()
        {
            var allRoutes = _nativeMap.MapElements?.OfType<MapPolyline>()
                                                   .ToArray();

            if (allRoutes?.Any() == true)
            {
                foreach (MapPolyline route in allRoutes)
                {
                    _nativeMap.MapElements.Remove(route);
                }
            }

            _currentUserRoute = null;
        }

        protected override void DrawRouteInMap(IEnumerable<Models.Geoposition> positions)
        {
            var nativePositions = positions.Select(CoordinateConverter.ConvertToNative)
                                           .Select(p => p.Position)
                                           .ToArray();

            var polyline = new MapPolyline();
            polyline.StrokeColor = FormsMap.RouteColor.ToMediaColor();
            polyline.StrokeThickness = 8;
            polyline.Path = new Geopath(nativePositions);
            _nativeMap.MapElements.Add(polyline);

            _currentUserRoute = polyline;
        }

        public override IEnumerable<Models.Geoposition> GetCurrentUserRoute()
        {
            return _currentUserRoute?.Path?.Positions.Select(p => new Models.Geoposition
            {
                Latitude = p.Latitude,
                Longitude = p.Longitude
            }) ?? Enumerable.Empty<Models.Geoposition>();
        }
    }
}
