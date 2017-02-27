using MSCorp.FirstResponse.Client.Models;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Ticket.Mock
{
    public class MockTicketService : ITicketService
    {
        public async Task<bool> AddTicketAsync(TicketModel ticket)
        {
            await Task.Delay(500);
            return true;
        }
    }
}
