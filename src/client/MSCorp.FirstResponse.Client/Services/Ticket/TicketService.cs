using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Ticket
{
    public class TicketService : ITicketService
    {
        private readonly IRequestProvider _requestProvider;

        public TicketService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<bool> AddTicketAsync(TicketModel ticket)
        {
            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = $"api/city/{Settings.SelectedCity}/tickets";

            string uri = builder.ToString();

            try
            {
                await _requestProvider.PostAsync<TicketModel>(uri, ticket);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error loading data in: {ex}");
                return false;
            }

            return true;
        }
    }
}
