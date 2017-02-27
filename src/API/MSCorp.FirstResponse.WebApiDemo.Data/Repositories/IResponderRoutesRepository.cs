using System.Collections.Generic;
using System.Threading.Tasks;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Repositories
{
    public interface IResponderRoutesRepository
    {
        Task<ResponderRoute> GetResponderRouteAsync(int id);
        Task<IEnumerable<ResponderRoute>> GetAllResponderRoutesAsync(int cityId);
    }
}
