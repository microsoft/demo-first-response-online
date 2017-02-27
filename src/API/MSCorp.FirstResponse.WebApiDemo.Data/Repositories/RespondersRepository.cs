using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Context;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Data.Repositories
{
    public class RespondersRepository : IRespondersRepository
    {
        private readonly FirstResponseContext _context;

        public RespondersRepository(FirstResponseContext context)
        {
            _context = context;
        }

        public IEnumerable<Responder> GetAll()
        {
            return _context.Responders.AsEnumerable();
        }

        public async Task<IEnumerable<Responder>> GetAllByCityAsync(int cityId)
        {
            return await _context
                .Responders
                .Include(r => r.Route)
                .Where(r => r.CityId == cityId)
                .ToListAsync();
        }
    }

}

