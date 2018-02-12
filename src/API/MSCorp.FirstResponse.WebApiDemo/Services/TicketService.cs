using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using MSCorp.FirstResponse.WebApiDemo.Data.Entities;
using Newtonsoft.Json;
using Microsoft.Azure.Documents;
using System.Threading.Tasks;
using MSCorp.FirstResponse.WebApiDemo.Model;

namespace MSCorp.FirstResponse.WebApiDemo.Services
{
    public class TicketService
    {
        private readonly string EndpointUri = ConfigurationManager.AppSettings["DocumentDbUri"];
        private readonly string PrimaryKey = ConfigurationManager.AppSettings["DocumentDbAuthKey"];

        private DocumentClient _client;

        public TicketService()
        {
            _client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);
        }

        public async Task<Document> AddTicket(TicketModel ticket)
        {
            string databaseName = "FirstResponse";
            string collectionName = "Tickets";
            
            ResourceResponse<Document> result = await _client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),
                ticket);

            return result.Resource;
        }
    }
}