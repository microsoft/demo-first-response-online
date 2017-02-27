using MSCorp.FirstResponse.Client.Data;
using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MSCorp.FirstResponse.Client.Extensions;

namespace MSCorp.FirstResponse.Client.Services.Heatmap
{
    public class MockHeatmapService : IHeatmapService
    {
      
        public async Task<ObservableCollection<Geoposition>> GetHeatmapPointsAsync()
        {
            await Task.Delay(500);
            return DataRepository.LoadHeatData().ToObservableCollection();
        }
    }
}
