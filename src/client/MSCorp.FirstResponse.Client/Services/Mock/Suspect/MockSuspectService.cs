using MSCorp.FirstResponse.Client.Data;
using MSCorp.FirstResponse.Client.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Cities
{
    public class MockSuspectService : ISuspectService
    {
        public async Task<IEnumerable<SuspectModel>> GetSuspectsAsync(string search)
        {
            await Task.Delay(500);
            return DataRepository.LoadSuspectData().AsEnumerable();
        }
    }
}
