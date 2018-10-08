using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Maps.Routes.GoogleApi;
using MSCorp.FirstResponse.Client.UWP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls.Maps;

namespace MSCorp.FirstResponse.Client.UWP.Maps
{
    public class RouteManager : AbstractRouteManager
    {
        private readonly MapControl _nativeMap;
        private MapPolyline _currentUserRoute;
        private readonly DrivingRouterProvider _routeProvider;

        public RouteManager(MapControl nativeMap, CustomMap formsMap, PushpinManager pushpinManager)
            : base(formsMap, pushpinManager)
        {
            _nativeMap = nativeMap;
            _routeProvider = new DrivingRouterProvider();
        }

        public override async Task<IEnumerable<Models.Geoposition>> CalculateRoute(Models.Geoposition from, Models.Geoposition to)
        {
            // Use PCL driving provider (that uses Google APIs), MapControl is crashing when trying to calculate routes
            // Maybe related to this? => https://social.msdn.microsoft.com/Forums/sqlserver/en-US/ebbd9020-728a-4342-b2a3-5fc44482c0f9/mapcontrol-crashes-inside-uwp-app-on-15063632?forum=bingmapswindows8
            IEnumerable<Models.Geoposition> positions = await _routeProvider.GetRoutePositionsAsync(from, to);
            return positions;
        }

        public override async void ClearAllUserRoutes()
        {
            var allRoutes = _nativeMap.MapElements?.OfType<MapPolyline>()
                                                   .ToArray();

            if (allRoutes?.Any() == true)
            {
                foreach (MapPolyline route in allRoutes)
                {
                    await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        _nativeMap.MapElements.Remove(route);
                    });
                }
            }

            _currentUserRoute = null;
        }

        protected override async void DrawRouteInMap(IEnumerable<Models.Geoposition> positions)
        {
            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
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
            });
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
