using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Heatmap
{
    public interface IHeatmapService
    {
        Task<ObservableCollection<Geoposition>> GetHeatmapPointsAsync();
    }
}
