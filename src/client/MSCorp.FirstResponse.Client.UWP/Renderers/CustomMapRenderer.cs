using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.UWP.Maps;
using MSCorp.FirstResponse.Client.UWP.Renderers;
using System;
using System.ComponentModel;
using Windows.UI.Xaml.Controls.Maps;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.UWP;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace MSCorp.FirstResponse.Client.UWP.Renderers
{
    public class CustomMapRenderer : MapRenderer
    {
        private MapControl _nativeMap;
        private CustomMap _customMap;

        private MapManager _mapManager;

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                _nativeMap = null;
                _mapManager = null;
            }

            if (e.NewElement != null)
            {
                _nativeMap = Control;
                _customMap = (CustomMap)Element;

                AddManagers();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("Renderer", StringComparison.CurrentCultureIgnoreCase))
            {
                _mapManager?.Initialize();
            }
            else
            {
                _mapManager?.HandleCustomMapPropertyChange(e);
            }
        }

        private void AddManagers()
        {
            var annotationManager = new PushpinManager(_nativeMap, _customMap);
            var routeManager = new RouteManager(_nativeMap, _customMap, annotationManager);
            var responderManager = new ResponderManager(_nativeMap, _customMap, routeManager, annotationManager);
            var heatMapManager = new HeatMapManager(_nativeMap, _customMap);

            _mapManager = new MapManager(_customMap, annotationManager, routeManager, responderManager, heatMapManager);
        }

        protected override void Dispose(bool disposing)
        {
            _mapManager?.ResponderManager.StopResponderUpdater();

            base.Dispose(disposing);
        }
    }
}
