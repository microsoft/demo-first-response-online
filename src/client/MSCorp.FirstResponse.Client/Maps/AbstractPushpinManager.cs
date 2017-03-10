using MSCorp.FirstResponse.Client.Controls;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Generic;

namespace MSCorp.FirstResponse.Client.Maps
{
    public abstract class AbstractPushpinManager
    {
        public event EventHandler<IncidentModel> IncidentSelected;

        public event EventHandler<IncidentModel> IncidentUnselected;

        public event EventHandler<IncidentModel> NavigationRequested;

        protected readonly CustomMap FormsMap;

        protected AbstractPushpinManager(CustomMap formsMap)
        {
            FormsMap = formsMap;
        }

        public void AddIncidents(IEnumerable<IncidentModel> incidents)
        {
            foreach (var incident in incidents)
            {
                AddIncidentToMap(incident);
            }
        }

        public abstract void RemoveAllIncidents();

        public abstract void RemoveIncidents(IEnumerable<IncidentModel> removedIncidents);

        public void AddResponders(IEnumerable<ResponderModel> responders)
        {
            if(responders == null)
            {
                return;
            }

            foreach (var responder in responders)
            {
                AddResponderToMap(responder);
            }
        }

        public abstract void RemoveAllResponders();

        public abstract void RemoveResponder(ResponderModel responder);

        public void LoadTickets(IEnumerable<TicketModel> tickets)
        {
            foreach (var ticket in tickets)
            {
                AddTicketToMap(ticket);
            }
        }

        public abstract void AddUser();

        public abstract void UnloadTickets();

        public abstract void ShowIncidentInformationPanel(IncidentModel incident);

        public abstract void HideIncidentInformationPanel();

        public abstract Geoposition GetCurrentUserPosition();

        public abstract Geoposition GetResponderPosition(ResponderModel responder);

        public abstract void SetInteraction(bool active);

        protected abstract void AddIncidentToMap(IncidentModel incident);

        protected abstract void AddResponderToMap(ResponderModel responder);

        protected abstract void AddTicketToMap(TicketModel ticket);
        
        protected void OnIncidentSelected(IncidentModel incident)
        {
            if (CurrentUserStatus.IsAttendingAnIncident)
            {
                return;
            }

            if (incident != null)
            {
                FormsMap.CurrentIncident = incident;

                var handler = IncidentSelected;
                handler?.Invoke(this, incident);
            }
        }

        protected void OnIncidentUnselected(IncidentModel incident)
        {
            if (CurrentUserStatus.IsAttendingAnIncident)
            {
                return;
            }

            var handler = IncidentUnselected;

            handler?.Invoke(this, incident);
        }

        protected void OnNavigationRequested(IncidentModel incident)
        {
            var handler = NavigationRequested;

            handler?.Invoke(this, incident);
        }
    }
}
