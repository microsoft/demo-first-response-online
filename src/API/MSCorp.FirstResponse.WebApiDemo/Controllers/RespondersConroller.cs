using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using MSCorp.FirstResponse.WebApiDemo.Data.Repositories;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{
    [RoutePrefix("api/city/{city:int}/responders")]
    public class RespondersController : ApiController
    {
        private readonly IRespondersRepository _respondersRepository;

        public RespondersController(IRespondersRepository respondersRepository)
        {
            _respondersRepository = respondersRepository;
        }

        // GET api/city/city/responders
        [HttpGet, Route]
        public async Task<IEnumerable<Responder>> GetResponders(int city)
        {
            return await _respondersRepository.GetAllByCityAsync(city);
        }

    }
}
