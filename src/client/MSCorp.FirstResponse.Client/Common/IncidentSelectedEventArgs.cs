using System;

namespace MSCorp.FirstResponse.Client.Common
{
    public class IncidentSelectedEventArgs : EventArgs
    {
        public IncidentSelectedEventArgs(int incidentId, bool showDetails = true, bool showTickets = false)
        {
            IncidentId = incidentId;
            ShowDetails = showDetails;
            ShowTickets = showTickets;
        }

        public int IncidentId { get; }
        public bool ShowDetails { get; set; }
        public bool ShowTickets { get; set; }
    }
}