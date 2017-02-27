using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Incidents
{
    public interface IIncidentsService
    {
        Task<ObservableCollection<IncidentModel>> GetIncidentsAsync();

        Task<SearchAreaModel> GetSearchAreaForIncidentAsync(int incidentId);
    }
}
