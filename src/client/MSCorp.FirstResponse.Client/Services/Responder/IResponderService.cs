using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Responder
{
    public interface IResponderService
    {
        Task<ObservableCollection<RouteModel>> GetRoutesAsync();

        Task<ObservableCollection<ResponderModel>> GetRespondersAsync();
    }
}