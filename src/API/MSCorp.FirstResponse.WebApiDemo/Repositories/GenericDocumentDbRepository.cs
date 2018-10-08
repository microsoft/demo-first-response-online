namespace MSCorp.FirstResponse.WebApiDemo.Repositories
{
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class GenericDocumentDbRepository<T> where T : class
    {
        private DocumentClient _client;
        private readonly string _databaseId;
        private readonly string _collectionId;

        public GenericDocumentDbRepository(string databaseName, string collectionName)
        {
            _databaseId = databaseName;
            _collectionId = collectionName;

            string endpoint = ConfigurationManager.AppSettings["DocumentDbUri"];
            string authKey = ConfigurationManager.AppSettings["DocumentDbAuthKey"];

            _client = new DocumentClient(new Uri(endpoint), authKey);
        }

        public async Task CreateDbIfNotExists()
        {
            await _client.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseId });
        }

        public async Task CreateCollectionIfNotExists()
        {
            await _client.CreateDocumentCollectionIfNotExistsAsync(
                   UriFactory.CreateDatabaseUri(_databaseId),
                   new DocumentCollection { Id = _collectionId },
                   new RequestOptions { OfferThroughput = 1000 });
        }


        public async Task<T> GetItemAsync(string id) 
        {
            Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
            return (T)(dynamic)document;
        }


        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = -1 })
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);
        }

        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
        }
    }
}
