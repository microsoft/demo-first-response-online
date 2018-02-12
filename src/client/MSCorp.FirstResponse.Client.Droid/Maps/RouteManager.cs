using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Maps.Routes.GoogleApi;
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
        private readonly DrivingRouterProvider _routeProvider;

        public RouteManager(GoogleMap nativeMap, CustomMap formsMap, MarkerManager pushpinManager) 
            : base(formsMap, pushpinManager)
        {
            _nativeMap = nativeMap;
            _routeProvider = new DrivingRouterProvider();
        }

        public override async Task<IEnumerable<Geoposition>> CalculateRoute(Geoposition from, Geoposition to)
        {
            IEnumerable<Models.Geoposition> positions = await _routeProvider.GetRoutePositionsAsync(from, to);
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
    }
}