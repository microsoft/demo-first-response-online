using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using MSCorp.FirstResponse.WebApiDemo.Configuration;
using MSCorp.FirstResponse.WebApiDemo.Helpers;
using System.Collections.Generic;
using MSCorp.FirstResponse.WebApiDemo.Models;
using System;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{
    /// <summary>
    /// Azure search endpoint for showcasing phonetic search.
    /// </summary>
    [AllowAnonymous, RoutePrefix("api/person")]
    public class PersonController : ApiController
    {
        private readonly Random _random = new Random();
        
        /// <summary>
        /// Performs a search on an Azure Search index, the values searched for are set in the web config. Returns the result set from the search.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("search")]
        public async Task<DocumentSearchResult> Search([FromUri]string searchText)
        {
            var credentials = new SearchCredentials(AzureSearchConfig.SearchServiceApiKey);
            SearchIndexClient client = new SearchIndexClient(AzureSearchConfig.SearchServiceName
                , AzureSearchConfig.SearchIndexName, credentials);

            var callersGeoLocation = LocationHelper.GetCallerLocation();

            var scoringParam = new ScoringParameter(
                AzureSearchConfig.GeoScoringParameterName,
                new string[] { callersGeoLocation.Longitude.ToString(), callersGeoLocation.Latitude.ToString()
                });

            var parameters = new SearchParameters
            {
                SearchMode = SearchMode.All,
                Facets = AzureSearchConfig.SearchIndexFacets,
                ScoringProfile = AzureSearchConfig.ScoringProfileName,
                ScoringParameters = new[] { scoringParam }
            };

            DocumentSearchResult searchResult = await client.Documents.SearchAsync(searchText, parameters);
            return searchResult;
        }

        /// <summary>
        /// Performs a search on an Azure Search index, the values searched for are set in the web config. Returns the result set from the search.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("suspects")]
        public async Task<IEnumerable<PersonModel>> Suspects([FromUri]string searchText)
        {
            var credentials = new SearchCredentials(AzureSearchConfig.SearchServiceApiKey);
            SearchIndexClient client = new SearchIndexClient(AzureSearchConfig.SearchServiceName
                , AzureSearchConfig.SearchIndexName, credentials);

            var callersGeoLocation = LocationHelper.GetCallerLocation();

            var scoringParam = new ScoringParameter(
                AzureSearchConfig.GeoScoringParameterName,
                new string[] { callersGeoLocation.Longitude.ToString(), callersGeoLocation.Latitude.ToString()
                });

            var parameters = new SearchParameters
            {
                SearchMode = SearchMode.All,
                Facets = AzureSearchConfig.SearchIndexFacets,
                ScoringProfile = AzureSearchConfig.ScoringProfileName,
                ScoringParameters = new[] { scoringParam },
                Top = 10,
                Select = new[] { "FirstName", "LastName", "EyeColor", "HairColor", "Sex", "SuspectSearchImage" }
            };

            DocumentSearchResult searchResult = await client.Documents.SearchAsync(searchText, parameters);
            
            var persons = searchResult.Results.Select(d =>
            {
                return new PersonModel
                {
                    Name = d.Document["FirstName"].ToString() + d.Document["LastName"].ToString(),
                    EyeColor = d.Document["EyeColor"].ToString(),
                    HairColor = d.Document["HairColor"].ToString(),
                    Sex = d.Document["Sex"].ToString(),
                    SuspectSearchImage = d.Document["SuspectSearchImage"].ToString()
                };
            });

            return persons;
        }
    }
}
