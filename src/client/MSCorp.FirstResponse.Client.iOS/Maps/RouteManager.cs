using CoreLocation;
using MapKit;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.iOS.Maps.Annotations;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.iOS.Maps
{
    public class RouteManager : AbstractRouteManager
    {
        private readonly MKMapView _nativeMap;

        private MKPolyline _currentUserRoute;

        public RouteManager(MKMapView nativeMap, CustomMap formsMap, AnnotationManager annotationManager)
            : base(formsMap, annotationManager)
        {
            _nativeMap = nativeMap;
        }

        public override void ClearAllUserRoutes()
        {
            var allRoutes = _nativeMap.Overlays?.Where(o => o.GetType() != typeof(MKPolygon))
                                                .ToArray();

            if (allRoutes?.Any() == true)
            {
                _nativeMap.RemoveOverlays(allRoutes);
            }

            _currentUserRoute = null;
        }

        public override IEnumerable<Geoposition> GetCurrentUserRoute()
        {
            var positions = _currentUserRoute?.Points.Select(r =>
            {
                var coord = MKMapPoint.ToCoordinate(r);
                return CoordinateConverter.ConvertToAbstraction(coord);
            });

            return positions;
        }

        public override async Task<IEnumerable<Geoposition>> CalculateRoute(Geoposition from, Geoposition to)
        {
            IEnumerable<Geoposition> result = Enumerable.Empty<Geoposition>();

            var nativeFrom = CoordinateConverter.ConvertToNative(from);
            var nativeTo = CoordinateConverter.ConvertToNative(to);

			var userPlaceMark = new MKPlacemark(nativeFrom, new Foundation.NSDictionary());
			var incidencePlaceMark = new MKPlacemark(nativeTo, new Foundation.NSDictionary());

            var sourceItem = new MKMapItem(userPlaceMark);
            var destItem = new MKMapItem(incidencePlaceMark);

            var request = new MKDirectionsRequest
            {
                Source = sourceItem,
                Destination = destItem,
                TransportType = MKDirectionsTransportType.Automobile
            };

            var directions = new MKDirections(request);

            MKPolyline polyRoute = null;

            directions.CalculateDirections((response, error) =>
            {
                if (error != null)
                {
                    System.Diagnostics.Debug.WriteLine(error.LocalizedDescription);
                }
                else
                {
                    if (response.Routes.Any())
                    {
                        var firstRoute = response.Routes.First();
                        polyRoute = firstRoute.Polyline;
                    }
                }
            });

            do
            {
                await Task.Delay(100);
            }
            while (directions.Calculating);

            if (polyRoute != null)
            {
                result = polyRoute.Points.Select(s =>
                {
                    CLLocationCoordinate2D coordinate = MKMapPoint.ToCoordinate(s);
                    return CoordinateConverter.ConvertToAbstraction(coordinate);
                });
            }

            return result;
        }

        protected override void DrawRouteInMap(IEnumerable<Geoposition> positions)
        {
            var nativeCoordinates = positions.Select(p => CoordinateConverter.ConvertToNative(p));
            _currentUserRoute = MKPolyline.FromCoordinates(nativeCoordinates.ToArray());

            _nativeMap.AddOverlay(_currentUserRoute);
        }
    }
}