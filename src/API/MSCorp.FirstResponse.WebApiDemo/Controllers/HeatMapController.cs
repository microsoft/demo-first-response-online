using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using MSCorp.FirstResponse.WebApiDemo.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{
    [RoutePrefix("api/city/{city:int}/heatmap")]
    public class HeatMapController : ApiController
    {
        private readonly IHeatMapPointsRepository _heatMapPointsRepository;

        public HeatMapController(IHeatMapPointsRepository heatMapPointsRepository)
        {
            _heatMapPointsRepository = heatMapPointsRepository;
        }

        // GET api/city/city/heatmap
        [HttpGet, Route]
        public async Task<IEnumerable<HeatMapPoint>> GetHeatMap(int city)
        {
            return await _heatMapPointsRepository.GetAllByCityAsync(city);
        }


    }
}