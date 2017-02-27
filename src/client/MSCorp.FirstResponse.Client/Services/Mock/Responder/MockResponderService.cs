using MSCorp.FirstResponse.Client.Data;
using MSCorp.FirstResponse.Client.Extensions;
using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Responder
{
    public class MockResponderService : IResponderService
    {
        public async Task<ObservableCollection<ResponderModel>> GetRespondersAsync()
        {
            await Task.Delay(500);

            return DataRepository.LoadResponderData().ToObservableCollection();
        }

        public async Task<ObservableCollection<RouteModel>> GetRoutesAsync()
        {
            await Task.Delay(500);
            var routes = DataRepository.LoadRoutes();

            return routes.ToObservableCollection();
        }
    }
}