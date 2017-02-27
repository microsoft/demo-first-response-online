using MapKit;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.iOS.Maps;
using MSCorp.FirstResponse.Client.iOS.Renderers;
using MSCorp.FirstResponse.Client.Maps;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MSCorp.FirstResponse.Client.iOS.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        private MKMapView _iosMapView;
        private CustomMap _customMap;

        private MapDrawingManager _drawingManager;
        private MapManager _mapManager;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _iosMapView = null;
                _drawingManager = null;
                _mapManager = null;
            }

            if (e.NewElement != null)
            {
                _iosMapView = (MKMapView)Control;
                _customMap = (CustomMap)Element;
                _iosMapView.ZoomEnabled = true;

                AddManagers();
            }
        }

        private void AddManagers()
        {
            var annotationManager = new AnnotationManager(_iosMapView, _customMap);
            var routeManager = new RouteManager(_iosMapView, _customMap, annotationManager);
            var responderManager = new ResponderManager(_iosMapView, _customMap, routeManager, annotationManager);
            var heatMapManager = new HeatMapManager(_iosMapView, _customMap);

            _mapManager = new MapManager(_customMap, annotationManager, routeManager, responderManager, heatMapManager);
            _iosMapView.GetViewForAnnotation = annotationManager.GetViewForAnnotation;

            _drawingManager = new MapDrawingManager(_customMap);
            _iosMapView.OverlayRenderer = _drawingManager.GetOverlayRenderer;
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("Renderer", StringComparison.CurrentCultureIgnoreCase))
            {
                _mapManager?.Initialize();

                //InvokeOnMainThread(async () =>
                //{
                //    await _mapManager.ResponderManager.Initialize(_customMap.Responders.Select(m => m.Model), _customMap.Routes);
                //});
            }
            else
            {
                _mapManager?.HandleCustomMapPropertyChange(e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _mapManager?.ResponderManager.StopResponderUpdater();

            base.Dispose(disposing);
        }
    }
}
