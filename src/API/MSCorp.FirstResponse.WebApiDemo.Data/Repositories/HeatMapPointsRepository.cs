using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Context;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Repositories
{
    public class HeatMapPointsRepository : IHeatMapPointsRepository
    {
        private readonly FirstResponseContext _context;

        public HeatMapPointsRepository(FirstResponseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HeatMapPoint>> GetAllByCityAsync(int cityId)
        {
            return await _context
                .HeatMapPoints
                .Where(r => r.CityId == cityId)
                .ToListAsync();
        }
    }

}

