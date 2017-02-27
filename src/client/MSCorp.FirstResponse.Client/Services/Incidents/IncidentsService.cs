using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSCorp.FirstResponse.Client.Helpers;

namespace MSCorp.FirstResponse.Client.Services.Incidents
{
    public class IncidentsService : IIncidentsService
    {
        private readonly IRequestProvider _requestProvider;

        public IncidentsService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<ObservableCollection<IncidentModel>> GetIncidentsAsync()
        {

            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = $"api/city/{Settings.SelectedCity}/incidents";

            string uri = builder.ToString();

            try
            {
                IEnumerable<IncidentModel> incidents = 
                    await _requestProvider.GetAsync<IEnumerable<IncidentModel>>(uri);

                if (incidents != null)
                {
                    return incidents.OrderByDescending(q => q.IsHighPriority).ToObservableCollection();
                }
                else
                {
                    return new ObservableCollection<IncidentModel>();
                }
            }
            catch
            {
                return new ObservableCollection<IncidentModel>();
            }
        }

        public async Task<SearchAreaModel> GetSearchAreaForIncidentAsync(int incidentId)
        {
            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = $"api/city/{Settings.SelectedCity}/incidents/{incidentId}/search-area";

            string uri = builder.ToString();

            try
            {
                SearchAreaModel searchArea = await _requestProvider.GetAsync<SearchAreaModel>(uri);

                if (searchArea != null)
                {
                    return new SearchAreaModel()
                    {
                        Polygon = searchArea.Polygon.ToPolygonConvexHull().ToArray(),
                        //Tickets = jsonSearchArea.Tickets
                    };
                }

                return default(SearchAreaModel);
            }
            catch
            {
                return default(SearchAreaModel);
            }
        }

        class JsonSearchArea {
            public SearchAreaModel SearchArea;

            public TicketModel[] Tickets;
        }
    }
}
