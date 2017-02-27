using CoreLocation;
using MapKit;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.iOS.Maps.Annotations;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Maps.Routes;
using MSCorp.FirstResponse.Client.Models;
using System.Linq;
using System;

namespace MSCorp.FirstResponse.Client.iOS.Maps
{
    public class ResponderManager : AbstractResponderManager
    {
        private readonly MKMapView _nativeMap;
        private MKPolygon _currentSearchPolygon;

        public ResponderManager(MKMapView nativeMap, CustomMap formsMap, RouteManager routeManager, AnnotationManager annotationManager)
            : base(formsMap, routeManager, annotationManager)
        {
            _nativeMap = nativeMap;
        }

        public override void StartResponderUpdater()
        {
            _nativeMap?.InvokeOnMainThread(async () =>
            {
                try
                {
                    await InnerStartResponderUpdater();
                }
                catch (OperationCanceledException)
                {
                    System.Diagnostics.Debug.WriteLine("ResponderManager.StartResponderUpdater cancelled");
                }
            });
        }

        protected override void DrawSearchAreaPolygon(Geoposition[] polygonData)
        {
            var polygonNativeCoordinates = polygonData.Select(CoordinateConverter.ConvertToNative);
            _currentSearchPolygon = MKPolygon.FromCoordinates(polygonNativeCoordinates.ToArray());
            _nativeMap.AddOverlay(_currentSearchPolygon);
        }

        protected override void UpdatePushpinPosition(Route e)
        {
            MKAnnotation annotation = null;

            if (e is Route<ResponderModel>)
            {
                var responderRoute = e as Route<ResponderModel>;

                annotation = _nativeMap.Annotations
                                       .OfType<ResponderAnnotation>()
                                       .FirstOrDefault(a => a.Responder.Id == responderRoute.Element?.Id);
            }
            else if (e is Route<UserRole>)
            {
                annotation = _nativeMap.Annotations
                                       .OfType<UserAnnotation>()
                                       .FirstOrDefault();
            }

            if (annotation != null)
            {
                var newCoordinate = new CLLocationCoordinate2D
                {
                    Latitude = e.CurrentPosition.Latitude,
                    Longitude = e.CurrentPosition.Longitude
                };

                annotation.SetCoordinate(newCoordinate);
            }
        }

        protected override void RemoveSearchAreaPolygon()
        {
            if (_currentSearchPolygon != null)
            {
                _nativeMap.RemoveOverlay(_currentSearchPolygon);
            }
        }
    }
}