using BingMapsRESTToolkit;
using MSCorp.FirstResponse.WebApiDemo.MapService;
using MSCorp.FirstResponse.WebApiDemo.Models;
using MSCorp.FirstResponse.WebApiDemo.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{
    public class SearchAreaController : ApiController
    {
        IncidentService _incidentService;

        public SearchAreaController(IncidentService incidentService)
        {
            _incidentService = incidentService;
        }

        [HttpGet]
        [Route("api/city/{city:int}/incidents/{incidentId}/search-area")]
        public SearchArea GetSearchAreas(int city, int incidentId)
        {
            var incident = _incidentService.GetIncident(city, incidentId);
            var searchArea = GetSearchArea(incident);
            return searchArea;
        }

        private static SearchArea GetSearchArea(IncidentModel incident)
        {
            var searchArea = new SearchArea
            {
                Polygon = GetPoligonPoints(incident)
            };

            return searchArea;
        }

        private static IList<PolygonPoint> GetPoligonPoints(IncidentModel incident)
        {
            int searchAreaRadius = 1500;
            var center = new Coordinate(incident.Latitude, incident.Longitude);
            var vertices = MapPointGenerator.GetVertices(center, searchAreaRadius);

            return vertices.Select(v => new PolygonPoint { Latitude = v.Latitude, Longitude = v.Longitude }).ToList();
        }
    }
}
