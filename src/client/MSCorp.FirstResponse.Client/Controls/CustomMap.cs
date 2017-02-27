using MSCorp.FirstResponse.Client.Common;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MSCorp.FirstResponse.Client.Controls
{
    public class CustomMap : Map
    {

        public CustomMap() {
            Initialize();
            
        }

        public event EventHandler<IncidentSelectedEventArgs> IncidentSelected;

        public static readonly BindableProperty IncidentsProperty =
            BindableProperty.Create("Incidents",
                typeof(IEnumerable<IncidentModel>), typeof(CustomMap), default(IEnumerable<IncidentModel>),
                BindingMode.TwoWay);

        public IEnumerable<IncidentModel> Incidents
        {
            get { return (IEnumerable<IncidentModel>)base.GetValue(IncidentsProperty); }
            set { base.SetValue(IncidentsProperty, value); }
        }

        public static readonly BindableProperty RespondersProperty =
            BindableProperty.Create("Responders", 
                typeof(IEnumerable<ResponderModel>), typeof(CustomMap), default(IEnumerable<ResponderModel>),
                BindingMode.TwoWay);

        public IEnumerable<ResponderModel> Responders
        {
            get { return (IEnumerable<ResponderModel>)base.GetValue(RespondersProperty); }
            set { base.SetValue(RespondersProperty, value); }
        }

        public static readonly BindableProperty RoutesProperty =
            BindableProperty.Create("Routes",
                typeof(IEnumerable<RouteModel>), typeof(CustomMap), default(IEnumerable<RouteModel>),
                BindingMode.TwoWay);

        public IEnumerable<RouteModel> Routes
        {
            get { return (IEnumerable<RouteModel>)base.GetValue(RoutesProperty); }
            set { base.SetValue(RoutesProperty, value); }
        }

        public static readonly BindableProperty LocationsProperty =
            BindableProperty.Create("Locations",
            typeof(IEnumerable<Geoposition>), typeof(CustomMap), new List<Geoposition>());

        public IEnumerable<Geoposition> Locations
        {
            get { return (IEnumerable<Geoposition>)base.GetValue(LocationsProperty); }
            set { base.SetValue(LocationsProperty, value); }
        }

        public static readonly BindableProperty IntensityProperty =
            BindableProperty.Create("Intensity",
                typeof(double), typeof(CustomMap), default(double),
                BindingMode.TwoWay);

        public double Intensity
        {
            get { return (double)base.GetValue(IntensityProperty); }
            set { base.SetValue(IntensityProperty, value); }
        }

        public static readonly BindableProperty RadiusProperty =
            BindableProperty.Create("Radius",
                typeof(double), typeof(CustomMap), default(double),
                BindingMode.TwoWay);

        public double Radius
        {
            get { return (double)base.GetValue(RadiusProperty); }
            set { base.SetValue(RadiusProperty, value); }
        }

        public static readonly BindableProperty IsHeatMapVisibleProperty =
            BindableProperty.Create("IsHeatMapVisible",
                typeof(bool), typeof(CustomMap), default(bool));

        public bool IsHeatMapVisible
        {
            get { return (bool)base.GetValue(IsHeatMapVisibleProperty); }
            set { base.SetValue(IsHeatMapVisibleProperty, value); }
        }

        public static readonly BindableProperty IsForceNavigationProperty =
            BindableProperty.Create("IsForceNavigation",
                typeof(bool), typeof(CustomMap), default(bool));

        public bool IsForceNavigation
        {
            get { return (bool)base.GetValue(IsForceNavigationProperty); }
            set { base.SetValue(IsForceNavigationProperty, value); }
        }
        

        public static readonly BindableProperty CurrentIncidentProperty =
            BindableProperty.Create("CurrentIncident",
                typeof(IncidentModel), typeof(CustomMap), null, propertyChanged: OnCurrentIncidentChanged);

        public IncidentModel CurrentIncident
        {
            get { return (IncidentModel)base.GetValue(CurrentIncidentProperty); }
            set { base.SetValue(CurrentIncidentProperty, value); }
        }

        public static readonly BindableProperty RouteColorProperty =
            BindableProperty.Create("RouteColor",
                typeof(Color), typeof(CustomMap), Color.Default);

        public Color RouteColor
        {
            get { return (Color)base.GetValue(RouteColorProperty); }
            set { base.SetValue(RouteColorProperty, value); }
        }

        public static readonly BindableProperty SearchPolygonColorProperty =
            BindableProperty.Create("SearchPolygonColor",
                typeof(Color), typeof(CustomMap), Color.Default);

        public Color SearchPolygonColor
        {
            get { return (Color)base.GetValue(SearchPolygonColorProperty); }
            set { base.SetValue(SearchPolygonColorProperty, value); }
        }

        public ICommand MapTypeCommand => new Command(ChangeMapType);

        private void Initialize()
        {
            // map initially centered on the user position with a configurable zoom level
            MoveToRegion(MapSpan.FromCenterAndRadius(
                            new Position(Settings.UserLatitude, Settings.UserLongitude),
                            Distance.FromMiles(GlobalSetting.ZoomLevel)));
        }

        private static void OnCurrentIncidentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = bindable as CustomMap;

            var handler = map?.IncidentSelected;

            if (handler != null && map.CurrentIncident != null)
            {
                handler(map, new IncidentSelectedEventArgs(map.CurrentIncident.Id));
            }
        }

        private void ChangeMapType()
        {
            if (MapType.Equals(MapType.Street))
            {
                MapType = MapType.Hybrid;
            }
            else
            {
                MapType = MapType.Street;
            }
        }
    }
}