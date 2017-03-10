using MSCorp.FirstResponse.Client.Common;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.UWP.Controls;
using MSCorp.FirstResponse.Client.UWP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Maps;

namespace MSCorp.FirstResponse.Client.UWP.Maps
{
    public class PushpinManager : AbstractPushpinManager
    {
        private readonly MapControl _nativeMap;
        private readonly MapItemsControl _mapItems;
        private readonly Dictionary<MapIcon, int> _pushpinMappings;

        public PushpinManager(MapControl nativeMap, CustomMap formsMap) 
            : base(formsMap)
        {
            _nativeMap = nativeMap;
            _mapItems = new MapItemsControl();
            _nativeMap.Children.Add(_mapItems);
            _pushpinMappings = new Dictionary<MapIcon, int>();

            _nativeMap.MapElementClick += MapElementClick;
        }

        public override void AddUser()
        {
            var geoposition = new Models.Geoposition
            {
                Latitude = Settings.UserLatitude,
                Longitude = Settings.UserLongitude
            };

            var userIcon = new UserIcon();

            _mapItems.Items.Add(userIcon);
            SetMapIconPosition(userIcon, geoposition, new Point(0.5, 0.5));
        }

        protected override async void AddIncidentToMap(IncidentModel incident)
        {
            var geoLocation = CoordinateConverter.ConvertToNative(incident.GeoLocation);

            var mapIcon = new MapIcon();
            mapIcon.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
            mapIcon.Location = geoLocation;
            mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon.ZIndex = 1000;

            var iconImageUri = default(Uri);

            switch (incident.IncidentCategory)
            {
                case IncidentType.Alert:
                    iconImageUri = new Uri("ms-appx:///Assets/Pins/pin_alert.png");
                    break;
                case IncidentType.Animal:
                    iconImageUri = new Uri("ms-appx:///Assets/Pins/pin_animal.png");
                    break;
                case IncidentType.Arrest:
                    iconImageUri = new Uri("ms-appx:///Assets/Pins/pin_arrest.png");
                    break;
                case IncidentType.Car:
                    iconImageUri = new Uri("ms-appx:///Assets/Pins/pin_car.png");
                    break;
                case IncidentType.Fire:
                    iconImageUri = new Uri("ms-appx:///Assets/Pins/pin_siren.png");
                    break;
                case IncidentType.OfficerRequired:
                    iconImageUri = new Uri("ms-appx:///Assets/Pins/pin_officer.png");
                    break;
                case IncidentType.Stranger:
                    iconImageUri = new Uri("ms-appx:///Assets/Pins/pin_stranger.png");
                    break;
                default:
                    iconImageUri = new Uri("ms-appx:///Assets/Pins/pin_car.png");
                    break;
            }

            RandomAccessStreamReference stream = RandomAccessStreamReference.CreateFromUri(iconImageUri);
            mapIcon.Image = await stream.ScaleTo(40, 58);

            _nativeMap.MapElements.Add(mapIcon);
            _pushpinMappings.Add(mapIcon, incident.Id);
        }

        protected override void AddResponderToMap(ResponderModel responder)
        {
            var responderIcon = new ResponderIcon(responder);

            _mapItems.Items.Add(responderIcon);
            SetMapIconPosition(responderIcon, responder.GeoLocation, new Point(0.5, 0.5));
        }

        protected override void AddTicketToMap(TicketModel ticket)
        {
            var ticketIcon = new TicketIcon(ticket);

            _mapItems.Items.Add(ticketIcon);
            SetMapIconPosition(ticketIcon, ticket.Location, new Point(0.5, 0.5));
        }

        public override void UnloadTickets()
        {
            var allIcons = _mapItems.Items.OfType<TicketIcon>()
                                                   .ToList();
            RemoveIcons(allIcons);
        }

        public override void ShowIncidentInformationPanel(IncidentModel incident)
        {
            DismissCurrentIncidentPanel();

            var panel = new IncidentInfoIcon(incident, true);
            panel.IncidentIconExit += OnIncidentPanelClosed;
            panel.IncidentIconNavigate += OnIncidentNavigationRequest;
            panel.Margin = new Thickness(0, 0, 0, 60);

            _mapItems.Items.Add(panel);
            SetMapIconPosition(panel, incident.GeoLocation, new Point(0.5, 1));
        }

        public override void HideIncidentInformationPanel()
        {
            DismissCurrentIncidentPanel();
        }

        public override Models.Geoposition GetCurrentUserPosition()
        {
            var itemsControl = _nativeMap.Children.OfType<MapItemsControl>()
                                                  .FirstOrDefault();

            var userIcon = itemsControl?.Items.OfType<UserIcon>()
                                              .FirstOrDefault();

            return GetIconPosition(userIcon);
        }

        public override Models.Geoposition GetResponderPosition(ResponderModel responder)
        {
            var itemsControl = _nativeMap.Children.OfType<MapItemsControl>()
                                                  .FirstOrDefault();

            var responderIcon = itemsControl?.Items.OfType<ResponderIcon>()
                                                   .Where(icon => icon.Responder.Id == responder.Id)
                                                   .FirstOrDefault();

            return GetIconPosition(responderIcon);
        }

        public override void SetInteraction(bool active)
        {
            // For UWP we leave this empty
        }

        public void DismissCurrentIncidentPanel()
        {
            var panel = _mapItems.Items.OfType<IncidentInfoIcon>()
                                       .FirstOrDefault();

            if (panel != null)
            {
                panel.IncidentIconExit -= OnIncidentPanelClosed;
                panel.IncidentIconNavigate -= OnIncidentNavigationRequest;
                _mapItems.Items.Remove(panel);
            }
        }

        private Models.Geoposition GetIconPosition(DependencyObject icon)
        {
            if (icon == null)
            {
                return default(Models.Geoposition);
            }

            Geopoint geoLocation = MapControl.GetLocation(icon);

            return CoordinateConverter.ConvertToAbstraction(geoLocation);
        }

        private void SetMapIconPosition(DependencyObject icon, Models.Geoposition geoLocation, Point anchorPoint)
        {
            var nativeCoordinate = CoordinateConverter.ConvertToNative(geoLocation);

            MapControl.SetLocation(icon, nativeCoordinate);
            MapControl.SetNormalizedAnchorPoint(icon, anchorPoint);
        }
        
        private void MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {
            MapIcon icon = args.MapElements.OfType<MapIcon>()
                                           .FirstOrDefault();

            if (icon != null && _pushpinMappings.ContainsKey(icon))
            {
                int incidentId = _pushpinMappings[icon];
                IncidentModel incident = FormsMap.Incidents
                                                 .Where(i => i.Id == incidentId)
                                                 .FirstOrDefault();

                if (incident != null)
                {
                    OnIncidentSelected(incident);
                }
            }
        }

        private void OnIncidentPanelClosed(object sender, IncidentSelectedEventArgs e)
        {
            FormsMap.CurrentIncident = null;
            DismissCurrentIncidentPanel();
        }

        private void OnIncidentNavigationRequest(object sender, IncidentSelectedEventArgs e)
        {
            var incident = FormsMap.Incidents.FirstOrDefault(i => i.Id == e.IncidentId);

            if (incident != null)
            {
                OnNavigationRequested(incident);
            }
        }

        public override void RemoveAllIncidents()
        {
            RemoveIncidentsIcons(_pushpinMappings.Keys.ToList());
        }

        public override void RemoveIncidents(IEnumerable<IncidentModel> removedIncidents)
        {
            var iconsToRemove = _pushpinMappings.Where(x => removedIncidents.Any(i => i.Id == x.Value))
                                                .Select(x => x.Key)
                                                .ToList();

            RemoveIncidentsIcons(iconsToRemove);
        }

        public override void RemoveAllResponders()
        {
            var allResponderIcons = _mapItems.Items.OfType<ResponderIcon>()
                                                   .ToList();
            RemoveIcons(allResponderIcons);
        }

        public override void RemoveResponder(ResponderModel responder)
        {
            var responderIcon = _mapItems.Items.OfType<ResponderIcon>().FirstOrDefault(icon => icon.Responder.Id == responder.Id);
            if (responderIcon != null)
            {
                _mapItems?.Items?.Remove(responderIcon);
            }
        }

        private void RemoveIncidentsIcons(List<MapIcon> icons)
        {
            if (icons != null)
            {
                foreach (var icon in icons)
                {
                    _nativeMap?.MapElements?.Remove(icon);
                    _pushpinMappings.Remove(icon);
                }
            }
        }

        private void RemoveIcons(IEnumerable<DependencyObject> icons)
        {
            if (icons != null)
            {
                foreach (var icon in icons)
                {
                    _mapItems?.Items?.Remove(icon);
                }
            }
        }
    }
}
