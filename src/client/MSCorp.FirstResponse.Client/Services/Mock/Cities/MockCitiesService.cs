using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MSCorp.FirstResponse.Client.Data;
using MSCorp.FirstResponse.Client.Extensions;
using System.Linq;

namespace MSCorp.FirstResponse.Client.Services.Cities
{
    public class MockCitiesService : ICitiesService
    {
        public ObservableCollection<EventModel> Cities = DataRepository.LoadEventsData().ToObservableCollection();

        public async Task<ObservableCollection<EventModel>> GetEventsAsync() 
        {
            await Task.Delay(500);

            return Cities;
        }

        public EventModel GetDefaultEvent(int cityId)
        {
            var events = Cities;

            return events.FirstOrDefault(q => q.CityId == cityId);
        }
    }
}