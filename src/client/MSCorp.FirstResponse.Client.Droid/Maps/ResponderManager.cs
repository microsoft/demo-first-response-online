using System;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Maps.Routes;
using MSCorp.FirstResponse.Client.Models;
using Xamarin.Forms.Platform.Android;
using Android.App;

namespace MSCorp.FirstResponse.Client.Droid.Maps
{
    public class ResponderManager : AbstractResponderManager
    {
        private readonly GoogleMap _nativeMap;
        private Polygon _currentSearchPolygon;

        public ResponderManager(GoogleMap nativeMap, CustomMap formsMap, RouteManager routeManager, MarkerManager pushpinManager) 
            : base(formsMap, routeManager, pushpinManager)
        {
            _nativeMap = nativeMap;
        }

        public override void StartResponderUpdater()
        {
            var activity = Xamarin.Forms.Forms.Context as Activity;

            activity?.RunOnUiThread(async() =>
            {
                await InnerStartResponderUpdater();
            });
        }

        protected override void DrawSearchAreaPolygon(Geoposition[] polygonData)
        {
            Color color = FormsMap.SearchPolygonColor.ToAndroid();

            var polygonOptions = new PolygonOptions();
            polygonOptions.InvokeFillColor(color);
            polygonOptions.InvokeStrokeColor(color);
            polygonOptions.InvokeStrokeWidth(1.0f);

            foreach (var position in polygonData)
            {
                var nativeCoordinate = CoordinateConverter.ConvertToNative(position);
                polygonOptions.Add(nativeCoordinate);
            }

            _currentSearchPolygon = _nativeMap.AddPolygon(polygonOptions);
        }

        protected override void RemoveSearchAreaPolygon()
        {
            _currentSearchPolygon?.Remove();
        }

        protected override void UpdatePushpinPosition(Route e)
        {
            Marker marker = null;
            var androidManager = PushpinManager as MarkerManager;

            if (e is Route<UserRole>)
            {
                marker = androidManager.UserMarker;
            }
            else if (e is Route<ResponderModel>)
            {
                var responderRoute = e as Route<ResponderModel>;
                marker = androidManager.GetMarkerForResponder(responderRoute.Element);
            }

            if (marker != null)
            {
                LatLng newPosition = CoordinateConverter.ConvertToNative(e.CurrentPosition);
                marker.Position = newPosition;
            }
        }
    }
}