using MSCorp.FirstResponse.Client.Models;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MSCorp.FirstResponse.Client.Maps
{
    internal class IncidentsObserver
    {
        private readonly MapManager _mapManager;
        private IEnumerable<IncidentModel> _incidents;

        public IncidentsObserver(MapManager mapManager)
        {
            _mapManager = mapManager;
        }

        public void AttachIncidents(IEnumerable<IncidentModel> incidents)
        {
            //_mapManager.ResponderManager.StopResponderUpdater();
            _mapManager.PushpinManager.RemoveAllIncidents();

            // Remove previous incidents list instance handlers (if existing)
            INotifyCollectionChanged currentCollection = _incidents as INotifyCollectionChanged;

            if (currentCollection != null)
            {
                currentCollection.CollectionChanged -= OnIncidentsCollectionChanged;
            }

            _incidents = incidents;

            if (_incidents != null)
            {
                _mapManager.PushpinManager.AddIncidents(incidents);
            }

            // Add new incidents list instance handlers (if needed)
            if (currentCollection != null)
            {
                currentCollection.CollectionChanged -= OnIncidentsCollectionChanged;
            }

            //_mapManager.ResponderManager.StartResponderUpdater();
        }

        private void OnIncidentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                IEnumerable<IncidentModel> newIncidents = e.NewItems.OfType<IncidentModel>();
                _mapManager.PushpinManager.AddIncidents(newIncidents);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                IEnumerable<IncidentModel> removedIncidents = e.OldItems.OfType<IncidentModel>();
                _mapManager.PushpinManager.RemoveIncidents(removedIncidents);
            }
        }
    }
}
