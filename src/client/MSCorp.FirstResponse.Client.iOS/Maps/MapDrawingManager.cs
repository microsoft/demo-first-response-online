using MapKit;
using MSCorp.FirstResponse.Client.Controls;
using Xamarin.Forms.Platform.iOS;

namespace MSCorp.FirstResponse.Client.iOS.Maps
{
    public class MapDrawingManager
    {
        private readonly CustomMap _formsMap;

        public MapDrawingManager(CustomMap formsMap)
        {
            _formsMap = formsMap;
        }

        public MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlay)
        {
            if (overlay is MKPolyline)
            {
                var polylineRenderer = new MKPolylineRenderer(overlay as MKPolyline);

                polylineRenderer.FillColor = _formsMap.RouteColor.ToUIColor();
                polylineRenderer.StrokeColor = _formsMap.RouteColor.ToUIColor();
                polylineRenderer.LineWidth = 8;

                return polylineRenderer;
            }
            else if (overlay is MKPolygon)
            {
                var polygonRenderer = new MKPolygonRenderer(overlay as MKPolygon);

                polygonRenderer.FillColor = _formsMap.SearchPolygonColor.ToUIColor();
                polygonRenderer.StrokeColor = Xamarin.Forms.Color.Transparent.ToUIColor();

                return polygonRenderer;
            }

            return null;
        }
    }
}