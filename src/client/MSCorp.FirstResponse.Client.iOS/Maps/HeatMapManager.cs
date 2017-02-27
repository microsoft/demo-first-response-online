using MapKit;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.iOS.Maps.Heat;
using MSCorp.FirstResponse.Client.Maps;
using System.ComponentModel;
using System.Linq;

namespace MSCorp.FirstResponse.Client.iOS.Maps
{
    public class HeatMapManager : AbstractHeatMapManager
    {
        private readonly MKMapView _nativeMap;
        private HeatMapLayer _heatMapLayer;

        public HeatMapManager(MKMapView nativeMap, CustomMap formsMap) : base(formsMap)
        {
            _nativeMap = nativeMap;
        }

        protected override void CreateHeatMapIfNeeded()
        {
            if (_heatMapLayer == null)
            {
                _heatMapLayer = new HeatMapLayer(FormsMap)
                {
                    ParentMap = _nativeMap,
                    Radius = FormsMap.Radius,
                    Intensity = FormsMap.Intensity,
                    Locations = FormsMap.Locations?.Select(CoordinateConverter.ConvertToNative)
                };

                FormsMap.PropertyChanged -= FormsMapPropertyChanged;
                FormsMap.PropertyChanged += FormsMapPropertyChanged;

                _heatMapLayer.Frame = new CoreGraphics.CGRect(0, 0, _nativeMap.Frame.Width, _nativeMap.Frame.Height);
                _nativeMap.AddSubview(_heatMapLayer);
            }
        }

        protected override void HideHeatMap()
        {
            _heatMapLayer.Hidden = true;
        }

        protected override void ShowHeatMap()
        {
            _heatMapLayer.Hidden = false;
        }

        private void FormsMapPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(CustomMap.LocationsProperty.PropertyName))
            {
                if (_heatMapLayer != null)
                {
                    _heatMapLayer.Locations = FormsMap.Locations?.Select(CoordinateConverter.ConvertToNative);
                }
            }
        }
    }
}