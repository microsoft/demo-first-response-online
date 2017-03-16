using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Droid.Adapters;
using MSCorp.FirstResponse.Client.Droid.Maps.Icons;
using MSCorp.FirstResponse.Client.Maps;
using MSCorp.FirstResponse.Client.Models;
using MSCorp.FirstResponse.Client.Helpers;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;

namespace MSCorp.FirstResponse.Client.Droid.Maps
{
    public class MarkerManager : AbstractPushpinManager
    {
        private readonly Dictionary<int, Marker> _incidentPushpinMappings;
        private readonly Dictionary<int, Marker> _responderPushpinMappings;
        private readonly Dictionary<int, Marker> _ticketPushpinMappings;
        private readonly MapInfoWindowAdapter _infoWindowAdapter;

        private Marker _userMarker;
        private GoogleMap _nativeMap;
        private bool _isUserInteractionEnabled;

        public Marker UserMarker
        {
            get
            {
                return _userMarker;
            }
        }

        public MarkerManager(GoogleMap nativeMap, CustomMap formsMap)
            : base(formsMap)
        {
            _nativeMap = nativeMap;
            _incidentPushpinMappings = new Dictionary<int, Marker>();
            _responderPushpinMappings = new Dictionary<int, Marker>();
            _ticketPushpinMappings = new Dictionary<int, Marker>();
            _isUserInteractionEnabled = true;

            _nativeMap.MarkerClick += OnMarkerClick;

            // Incident info panel
            _infoWindowAdapter = new MapInfoWindowAdapter();
            _nativeMap.SetInfoWindowAdapter(_infoWindowAdapter);
            _nativeMap.InfoWindowClick += InfoWindowClick;
            _nativeMap.InfoWindowClose += InfoWindowClose;
        }

        protected override void AddIncidentToMap(IncidentModel incident)
        {
            var incidentIcon = new IncidentIcon(incident);

            var markerOptions = incidentIcon.MarkerOptions;
            markerOptions.SetPosition(new LatLng(incident.Latitude, incident.Longitude));

            Marker marker = _nativeMap.AddMarker(markerOptions);
            _incidentPushpinMappings.Add(incident.Id, marker);
        }

        protected override void AddResponderToMap(ResponderModel responder)
        {
            responder.PropertyChanged += updateColor;

            var responderIcon = new ResponderIcon(responder);


            var markerOptions = responderIcon.MarkerOptions;
            markerOptions.SetPosition(new LatLng(responder.Latitude, responder.Longitude));

            Marker marker = _nativeMap.AddMarker(responderIcon.MarkerOptions);
            _responderPushpinMappings.Add(responder.Id, marker);
        }

        private void updateColor(object sender, PropertyChangedEventArgs e)
        {
            var responder = sender as ResponderModel;
            if (_responderPushpinMappings.ContainsKey(responder.Id))
            {
                Marker marker = GetMarkerForResponder(responder);
                _responderPushpinMappings.Remove(responder.Id);

                if (marker != null)
                {
                    var actualPosition = marker.Position;
                    marker.Remove();
                    var responderIcon = new ResponderIcon(responder);
                    var markerOptions = responderIcon.MarkerOptions;
                    markerOptions.SetPosition(new LatLng(actualPosition.Latitude, actualPosition.Longitude));

                    _nativeMap.AddMarker(responderIcon.MarkerOptions);
                }
            }
        }

        public override void AddUser()
        {
            var userIcon = new UserIcon();

            var markerOptions = userIcon.MarkerOptions;
            markerOptions.SetPosition(new LatLng(Settings.UserLatitude, Settings.UserLongitude));

            _userMarker = _nativeMap.AddMarker(markerOptions);
        }

        public override void ShowIncidentInformationPanel(IncidentModel incident)
        {
            // open the selected marker info windows
            if (_incidentPushpinMappings.ContainsKey(incident.Id))
            {
               var marker = _incidentPushpinMappings[incident.Id];
               if (!marker.IsInfoWindowShown)
                {
                    _infoWindowAdapter.CurrentIncident = incident;
                    marker.ShowInfoWindow();
                }
            }
        }

        public override void HideIncidentInformationPanel()
        {
            foreach(var marker in _incidentPushpinMappings.Values)
            {
                if (marker.IsInfoWindowShown)
                {
                    marker.HideInfoWindow();
                }
            }
        }

        public override void UnloadTickets()
        {
            foreach(var marker in _ticketPushpinMappings.Values)
            {
                marker.Remove();
            }

            _ticketPushpinMappings.Clear();
        }

        public override Geoposition GetCurrentUserPosition()
        {
            return UserMarker != null
                ? CoordinateConverter.ConvertToAbstraction(UserMarker.Position)
                : default(Geoposition);
        }

        public override Geoposition GetResponderPosition(ResponderModel responder)
        {
            Marker marker = GetMarkerForResponder(responder);

            return marker != null
                ? CoordinateConverter.ConvertToAbstraction(marker.Position)
                : default(Geoposition);
        }

        public override void SetInteraction(bool active)
        {
            _isUserInteractionEnabled = active;
        }

        public Marker GetMarkerForResponder(ResponderModel responder)
        {
            return _responderPushpinMappings.ContainsKey(responder.Id)
                    ? _responderPushpinMappings[responder.Id]
                    : null;
        }

        public Marker GetMarkerForIncident(IncidentModel incident)
        {
            return _incidentPushpinMappings.ContainsKey(incident.Id)
                    ? _incidentPushpinMappings[incident.Id]
                    : null;
        }

        public override void RemoveAllIncidents()
        {
            List<Marker> allMarkers = _incidentPushpinMappings.Select(m => m.Value)
                                                              .ToList();
            _incidentPushpinMappings.Clear();

            foreach (Marker marker in allMarkers)
            {
                marker.Remove();
            }
        }

        public override void RemoveIncidents(IEnumerable<IncidentModel> removedIncidents)
        {
            List<KeyValuePair<int, Marker>> entriesToRemove = 
                _incidentPushpinMappings.Where(x => removedIncidents.Any(i => i.Id == x.Key))
                                        .ToList();

            foreach (KeyValuePair<int, Marker> entry in entriesToRemove)
            {
                entry.Value.Remove();
                _incidentPushpinMappings.Remove(entry.Key);
            }
        }

        public override void RemoveAllResponders()
        {
            List<Marker> allMarkers = _responderPushpinMappings.Select(m => m.Value)
                                                               .ToList();
            _responderPushpinMappings.Clear();

            foreach (Marker marker in allMarkers)
            {
                marker.Remove();
            }
        }

        public override void RemoveResponder(ResponderModel responder)
        {
            responder.PropertyChanged -= updateColor;

            if (_responderPushpinMappings.ContainsKey(responder.Id))
            {
                Marker marker = GetMarkerForResponder(responder);
                _responderPushpinMappings.Remove(responder.Id);

                if (marker != null)
                {
                    marker.Remove();
                }
            }
        }

        protected override void AddTicketToMap(TicketModel ticket)
        {
            var ticketIcon = new TicketIcon(ticket);

            var markerOptions = ticketIcon.MarkerOptions;
            markerOptions.SetPosition(new LatLng(ticket.Latitude, ticket.Longitude));

            Marker marker = _nativeMap.AddMarker(ticketIcon.MarkerOptions);
            _ticketPushpinMappings.Add(ticket.Id, marker);
        }

        private void OnMarkerClick(object sender, GoogleMap.MarkerClickEventArgs e)
        {
            var selectedMarker = e.Marker;

            if (selectedMarker == null || !_isUserInteractionEnabled)
            {
                // If user interaction is disabled, don't show info window
                e.Handled = true;
                return;
            }

            KeyValuePair<int, Marker> keyPair = GetIncidentMappedMarker(e.Marker);

            // Check if this is a incident marker
            if (keyPair.Equals(default(KeyValuePair<int, Marker>)))
            {
                return;
            }

            IncidentModel incident = FormsMap.Incidents.Where(i => i.Id == keyPair.Key)
                                                       .FirstOrDefault();

            if (incident != null)
            {
                OnIncidentSelected(incident);
            }

            _infoWindowAdapter.CurrentIncident = incident;

            // Mark as unhandled (to let info window appear)
            e.Handled = false;
        }

        private KeyValuePair<int, Marker> GetIncidentMappedMarker(Marker marker)
        {
            return _incidentPushpinMappings.FirstOrDefault(m => m.Value.Id == marker.Id);
        }

        private void InfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            OnNavigationRequested(FormsMap.CurrentIncident);
        }

        private void InfoWindowClose(object sender, GoogleMap.InfoWindowCloseEventArgs e)
        {
            _infoWindowAdapter.CurrentIncident = null;
            OnIncidentUnselected(FormsMap.CurrentIncident);
        }
    }
}