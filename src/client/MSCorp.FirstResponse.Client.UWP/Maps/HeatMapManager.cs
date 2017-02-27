using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.UWP.Maps.Heat;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace MSCorp.FirstResponse.Client.UWP.Maps
{
    public class HeatMapManager : AbstractHeatMapManager
    {
        private readonly MapControl _nativeMap;
        private HeatMapLayer _heatMapLayer;

        public HeatMapManager(MapControl nativeMap, CustomMap formsMap) 
            : base(formsMap)
        {
            _nativeMap = nativeMap;
        }

        protected override void CreateHeatMapIfNeeded()
        {
            if (_heatMapLayer == null)
            {
                IEnumerable<BasicGeoposition> positions = FormsMap.Locations?.Select(pos => new BasicGeoposition
                {
                    Latitude = pos.Latitude,
                    Longitude = pos.Longitude
                });

                Geopath polygonPath = new Geopath(positions);

                _heatMapLayer = new HeatMapLayer(FormsMap)
                {
                    ParentMap = _nativeMap,
                    Locations = polygonPath,
                    Radius = FormsMap.Radius,
                    Intensity = FormsMap.Intensity,
                    Visibility = Visibility.Collapsed
                };

                _nativeMap.Children.Add(_heatMapLayer);
            }
        }

        protected override void HideHeatMap()
        {
            _heatMapLayer.Visibility = Visibility.Collapsed;
        }

        protected override void ShowHeatMap()
        {
            _heatMapLayer.Visibility = Visibility.Visible;
        }
    }
}
