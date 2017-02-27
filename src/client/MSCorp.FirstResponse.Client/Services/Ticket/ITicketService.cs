using MSCorp.FirstResponse.Client.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Ticket
{
    public interface ITicketService
    {
        Task<bool> AddTicketAsync(TicketModel ticket);
    }
}
