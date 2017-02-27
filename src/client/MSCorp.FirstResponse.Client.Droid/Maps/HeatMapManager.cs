using Android.Gms.Maps;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Droid.Maps.Heat;
using MSCorp.FirstResponse.Client.Maps;
using System.Linq;

namespace MSCorp.FirstResponse.Client.Droid.Maps
{
    public class HeatMapManager : AbstractHeatMapManager
    {
        private HeatMapLayer _heatMapLayer;
        private MapView _nativeMap;
        private GoogleMap _googleMap;

        public HeatMapManager(MapView nativeMap, GoogleMap googleMap, CustomMap formsMap) 
            : base(formsMap)
        {
            _nativeMap = nativeMap;
            _googleMap = googleMap;
        }

        protected override void CreateHeatMapIfNeeded()
        {
            if (_heatMapLayer == null)
            {
                _heatMapLayer = new HeatMapLayer(Xamarin.Forms.Forms.Context, FormsMap)
                {
                    ParentMap = _googleMap,
                    Radius = FormsMap.Radius,
                    Intensity = FormsMap.Intensity,
                    Locations = FormsMap.Locations?.Select(CoordinateConverter.ConvertToNative)
                };

                FormsMap.PropertyChanged -= FormsMapPropertyChanged;
                FormsMap.PropertyChanged += FormsMapPropertyChanged;

                _nativeMap.AddView(_heatMapLayer);
            }
        }

        protected override void HideHeatMap()
        {
            _heatMapLayer.Visibility = Android.Views.ViewStates.Invisible;
        }

        protected override void ShowHeatMap()
        {
            _heatMapLayer.Visibility = Android.Views.ViewStates.Visible;
        }

        private void FormsMapPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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