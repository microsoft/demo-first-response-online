using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSCorp.FirstResponse.Client.Services.Cities
{
    public class SuspectService : ISuspectService
    {
        private readonly IRequestProvider _requestProvider;
        public SuspectService(IRequestProvider requestProvider)
        {
            _requestProvider = requestProvider;
        }

        public async Task<IEnumerable<SuspectModel>> GetSuspectsAsync(string search)
        {
            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = "api/person/suspects";
            builder.Query = "searchText=" + search;

            string uri = builder.ToString();

            try
            {
                var suspects = await _requestProvider.GetAsync<IEnumerable<SuspectModel>>(uri);
                if (suspects != null)
                {
                    return suspects;
                }
                else
                {
                    return default(IEnumerable<SuspectModel>);
                }
            }
            catch
            {
                return default(IEnumerable<SuspectModel>);
            }
        }
    }
}
