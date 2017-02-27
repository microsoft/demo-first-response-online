using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Cities
{
    public interface ICitiesService
    {
        Task<ObservableCollection<EventModel>> GetEventsAsync();

        EventModel GetDefaultEvent(int cityId);
    }
}
