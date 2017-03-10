using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using MSCorp.FirstResponse.WebApiDemo.Data.Context;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;


namespace MSCorp.FirstResponse.WebApiDemo.Data.Repositories
{
    public class CitiesRepository : ICitiesRepository
    {
        private readonly FirstResponseContext _context;

        public CitiesRepository(FirstResponseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _context
                    .Cities
                    .Include(c => c.AmbulancePosition)
                    .ToListAsync();
        }

    }

}

