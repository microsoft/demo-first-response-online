using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Extensions;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace MSCorp.FirstResponse.Client.Maps
{
    public class MapManager
    {
        private readonly IncidentsObserver _incidentsObserver;

        private bool _mapAlreadyCentered;

        public AbstractPushpinManager PushpinManager { get; private set; }

        public AbstractRouteManager RouteManager { get; private set; }

        public AbstractResponderManager ResponderManager { get; private set; }

        public AbstractHeatMapManager HeatMapManager { get; private set; }

        public CustomMap FormsMap { get; private set; }

        public MapManager(
            CustomMap formsMap, 
            AbstractPushpinManager pushpinManager, 
            AbstractRouteManager routeManager, 
            AbstractResponderManager responderManager,
            AbstractHeatMapManager heatMapManager)
        {
            FormsMap = formsMap;
            PushpinManager = pushpinManager;
            RouteManager = routeManager;
            ResponderManager = responderManager;
            HeatMapManager = heatMapManager;

            _mapAlreadyCentered = false;
            _incidentsObserver = new IncidentsObserver(this);
            CurrentUserStatus.Reset();
        }

        public void Initialize()
        {
            PushpinManager.IncidentSelected += PushpinSelected;
            PushpinManager.IncidentUnselected += PushpinUnselected;
            PushpinManager.NavigationRequested += NavigationRequested;
            ResponderManager.LoadTicketsRequest += LoadTicketsRequested;
            ResponderManager.UnloadTicketsRequest += UnloadTicketsRequested;
        }

        public Task SetCurrentIncident(IncidentModel incident)
        {
            return HandleIncidentSelection(incident);
        }

        public void HandleCustomMapPropertyChange(PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals(CustomMap.CurrentIncidentProperty.PropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                SetCurrentIncident(FormsMap.CurrentIncident);
            }
            else if (e.PropertyName.Equals(CustomMap.IncidentsProperty.PropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                _incidentsObserver.AttachIncidents(FormsMap.Incidents);

                if (!_mapAlreadyCentered && FormsMap.Incidents?.Any() == true)
                {
                    InitializeMapPosition();
                    _mapAlreadyCentered = true;
                }
            }
            else if (e.PropertyName.Equals(CustomMap.RespondersProperty.PropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                AttachResponders(FormsMap.Responders);
            }
            else if (e.PropertyName.Equals(CustomMap.RoutesProperty.PropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                PushpinManager.AddUser();
                ResponderManager.StartResponderUpdater();
            }
            else if (e.PropertyName.Equals(CustomMap.IsHeatMapVisibleProperty.PropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                HeatMapManager.UpdateVisibility();
            }
            else if (e.PropertyName.Equals(CustomMap.IsForceNavigationProperty.PropertyName, StringComparison.CurrentCultureIgnoreCase))
            {
                if (FormsMap.IsForceNavigation)
                {
                    ResponderManager.StartUserNavigation();
                }
            }
            
        }

        private void AttachResponders(IEnumerable<ResponderModel> responders)
        {
            PushpinManager.RemoveAllResponders();
            PushpinManager.AddResponders(responders);
        }

        private void InitializeMapPosition()
        {
            IEnumerable<IncidentModel> incidents = FormsMap?.Incidents;

            if (incidents?.Any() == false)
            {
                return;
            }

            var centerPosition = new Position(incidents.Average(x => x.Latitude),
                incidents.Average(x => x.Longitude));

            var minLongitude = incidents.Min(x => x.Longitude);
            var minLatitude = incidents.Min(x => x.Latitude);

            var maxLongitude = incidents.Max(x => x.Longitude);
            var maxLatitude = incidents.Max(x => x.Latitude);

            var distance = MapHelper.CalculateDistance(minLatitude, minLongitude,
                maxLatitude, maxLongitude, 'M') / 2;

            FormsMap.MoveToRegion(MapSpan.FromCenterAndRadius(centerPosition, Distance.FromMiles(distance)));
        }

        private async void PushpinSelected(object sender, IncidentModel incident)
        {
            if (FormsMap?.CurrentIncident != incident)
            {
                FormsMap.CurrentIncident = incident;
                await HandleIncidentSelection(incident);
            }
        }

        private async Task HandleIncidentSelection(IncidentModel incident)
        {

            if (CurrentUserStatus.CanNavigate)
            {
                RouteManager.ClearAllUserRoutes();
            }

            if (incident != null)
            {
                PushpinManager.ShowIncidentInformationPanel(incident);
                FormsMap.SetPosition(incident.GeoLocation);

                if (CurrentUserStatus.CanNavigate)
                {
                    await RouteManager.DrawRouteToIncident(incident);
                }
            }
            else
            {
                PushpinManager.HideIncidentInformationPanel();
                PushpinManager.SetInteraction(true);
                if (CurrentUserStatus.IsAttendingAnIncident) { 
                    ResponderManager.DismissCurrentUserIncident();
                }
            }
        }

        private void PushpinUnselected(object sender, IncidentModel incident)
        {
            FormsMap.CurrentIncident = null;
            if (!CurrentUserStatus.IsNavigating)
            {
                RouteManager.ClearAllUserRoutes();
            }
        }

        private void NavigationRequested(object sender, IncidentModel incident)
        {
            ResponderManager.StartUserNavigation();
        }

        private void LoadTicketsRequested(object sender, IEnumerable<TicketModel> tickets)
        {
            PushpinManager.LoadTickets(tickets);
        }

        private void UnloadTicketsRequested(object sender, EventArgs e)
        {
            PushpinManager.UnloadTickets();
        }
    }
}
