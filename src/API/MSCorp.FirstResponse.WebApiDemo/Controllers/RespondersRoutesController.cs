using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using MSCorp.FirstResponse.WebApiDemo.Data.Repositories;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{
    [RoutePrefix("api/city/{city}/responder-routes")]
    public class ResponderRoutesController : ApiController
    {
        private readonly IResponderRoutesRepository _responderRoutesRepository;

        public ResponderRoutesController(IResponderRoutesRepository responderRoutesRepository)
        {
            _responderRoutesRepository = responderRoutesRepository;
        }

        // GET api/city/city/responder-routes
        [HttpGet, Route]
        public async Task<IEnumerable<ResponderRoute>> GetAllRespondersRoute(int city)
        {
            return await _responderRoutesRepository.GetAllResponderRoutesAsync(city);
        }

        //// GET api/city/city/responder-routes/1
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponderRoute> GetRespondersRoute(int id)
        {
            return await _responderRoutesRepository.GetResponderRouteAsync(id);
        }


    }
}
