using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using MSCorp.FirstResponse.WebApiDemo.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{
    [RoutePrefix("api/cities")]
    public class CitiesController : ApiController
    {
        private readonly ICitiesRepository _citiesRepository;


        public CitiesController(ICitiesRepository citiesRepository)
        {

            _citiesRepository = citiesRepository;
        }

        [HttpGet, Route]
        public async Task<IEnumerable<City>> GetAllCities()
        {
            return await _citiesRepository.GetAllAsync();
        }
    }
}