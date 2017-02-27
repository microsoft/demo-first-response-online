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
    public class CustomMapRenderer : MapRenderer, IOnMapReadyCallback
    {
        private MapView _androidMapView;
        private GoogleMap _nativeMap;
        private CustomMap _customMap;
        private MapManager _mapManager;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _nativeMap = null;
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

            if (e.PropertyName.Equals("Renderer", StringComparison.CurrentCultureIgnoreCase))
            {
                _androidMapView.GetMapAsync(this);
            }
            else
            {
                _mapManager?.HandleCustomMapPropertyChange(e);
            }
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            _nativeMap = googleMap;

            // Disable zoom buttons
            _nativeMap.UiSettings.ZoomControlsEnabled = true;
            _nativeMap.UiSettings.MapToolbarEnabled = false;

            AddManagers();

            _mapManager?.Initialize();
        }

        private void AddManagers()
        {
            var annotationManager = new MarkerManager(_nativeMap, _customMap);
            var routeManager = new RouteManager(_nativeMap, _customMap, annotationManager);
            var responderManager = new ResponderManager(_nativeMap, _customMap, routeManager, annotationManager);
            var heatMapManager = new HeatMapManager(_androidMapView, _nativeMap, _customMap);

            _mapManager = new MapManager(_customMap, annotationManager, routeManager, responderManager, heatMapManager);
        }

        protected override void Dispose(bool disposing)
        {
            _mapManager?.ResponderManager.StopResponderUpdater();

            base.Dispose(disposing);
        }
    }
}