using MSCorp.FirstResponse.Client.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Cities
{
    public interface ISuspectService
    {
        Task<IEnumerable<SuspectModel>> GetSuspectsAsync(string search);
    }
}
