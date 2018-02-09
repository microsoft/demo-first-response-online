using Android.Gms.Maps;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Droid.Maps;
using MSCorp.FirstResponse.Client.Droid.Renderers;
using MSCorp.FirstResponse.Client.Maps;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MSCorp.FirstResponse.Client.Droid.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        private MapView _androidMapView;
        private CustomMap _customMap;
        private MapManager _mapManager;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _mapManager = null;
                _androidMapView = null;
            }

            if (e.NewElement != null)
            {
                _androidMapView = Control;
                _customMap = (CustomMap)Element;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            _mapManager?.HandleCustomMapPropertyChange(e);
        }

        protected override void OnMapReady(GoogleMap googleMap)
        {
            base.OnMapReady(googleMap);

            // Disable zoom buttons
            NativeMap.UiSettings.ZoomControlsEnabled = true;
            NativeMap.UiSettings.MapToolbarEnabled = false;

            AddManagers();

            _mapManager?.Initialize();
        }

        private void AddManagers()
        {
            var annotationManager = new MarkerManager(NativeMap, _customMap);
            var routeManager = new RouteManager(NativeMap, _customMap, annotationManager);
            var responderManager = new ResponderManager(NativeMap, _customMap, routeManager, annotationManager);
            var heatMapManager = new HeatMapManager(_androidMapView, NativeMap, _customMap);

            _mapManager = new MapManager(_customMap, annotationManager, routeManager, responderManager, heatMapManager);
        }

        protected override void Dispose(bool disposing)
        {
            _mapManager?.ResponderManager.StopResponderUpdater();

            base.Dispose(disposing);
        }
    }
}