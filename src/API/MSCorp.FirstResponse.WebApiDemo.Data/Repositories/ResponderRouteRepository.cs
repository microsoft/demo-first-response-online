using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Context;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Repositories
{
    public class ResponderRoutesRepository : IResponderRoutesRepository
    {
        private readonly FirstResponseContext _context;

        public ResponderRoutesRepository(FirstResponseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResponderRoute>> GetAllResponderRoutesAsync(int cityId)
        {
            var responderRoutes = await _context
                .Responders
                .Include(r => r.Route)
                .Include(r => r.Route.RoutePoints)
                .Where(r => r.CityId == cityId)
               .ToListAsync();

            return responderRoutes.Select( r => r.Route).Distinct();

        }

        public async Task<ResponderRoute> GetResponderRouteAsync(int id)
        {
            return await _context.ResponderRoutes
                .Include( rr => rr.RoutePoints)
                .FirstOrDefaultAsync(rr => rr.Id == id);
        }
    }

}

