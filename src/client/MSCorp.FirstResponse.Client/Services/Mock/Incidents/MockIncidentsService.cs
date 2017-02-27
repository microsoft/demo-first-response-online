using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System.Linq;
using MSCorp.FirstResponse.Client.Data;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Incidents
{
    public class MockIncidentsService : IIncidentsService
    {
        public async Task<ObservableCollection<IncidentModel>> GetIncidentsAsync()
        {
            await Task.Delay(500);

            return new ObservableCollection<IncidentModel>(DataRepository.LoadIncidentData());
        }

        public async Task<SearchAreaModel> GetSearchAreaForIncidentAsync(int incidentId)
        {
            await Task.Delay(500);

            IncidentModel selectedIncident = DataRepository.LoadIncidentData().Where(q => q.Id == incidentId).FirstOrDefault();

            return selectedIncident?.SearchArea;
        }
    }
}
