using MSCorp.FirstResponse.WebApiDemo.Models;
using MSCorp.FirstResponse.WebApiDemo.Services;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{
    [RoutePrefix("api/city/{city:int}/incidents")]
    public class CityIncidentsController : ApiController
    {
        IncidentService _incidentService;

        public CityIncidentsController(IncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        [HttpGet, Route]
        public IEnumerable<IncidentModel> GetIncidents(int city)
        {
            var incidents = _incidentService.GetCityIncidents(city).OrderBy(i => i.Address); // .OrderBy(i => i.ReceivedTime);
            return incidents;            
        }

        [HttpGet]
        [Route("{id:int}")]
        public IncidentModel GetIncident(int city, int id)
        {
            var incident = _incidentService.GetIncident(city, id);
            return incident;
        }
    }
}