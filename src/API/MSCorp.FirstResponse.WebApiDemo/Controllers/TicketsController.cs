using MSCorp.FirstResponse.WebApiDemo.Model;
using MSCorp.FirstResponse.WebApiDemo.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{
    [RoutePrefix("api/city/{city:int}/tickets")]
    public class TicketsController : ApiController
    {
        private readonly TicketService _ticketService;

        public TicketsController(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // POST api/city/city/tickets
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddTicket(int city, [FromBody] TicketModel ticket)
        {
            ticket.CityId = city;
            var document = await _ticketService.AddTicket(ticket);

            if (!string.IsNullOrEmpty(document.Id)) {
                return Ok();
            }

            return InternalServerError();
        }

    }
}
