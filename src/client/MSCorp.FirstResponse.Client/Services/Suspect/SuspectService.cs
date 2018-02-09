using MSCorp.FirstResponse.Client.Data.Base;
using MSCorp.FirstResponse.Client.Helpers;
using MSCorp.FirstResponse.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // First try new API method (for updated endpoint)
            var suspects = await InnerGetPatientsAsSuspectsAsync(search);

            if (suspects == null)
            {
                // For old endpoint versions, use old operation
                suspects = await InnerGetSuspectsAsync(search);
            }

            return suspects;
        }

        // Maintain this method to support environments that are not updated to latest version
        private async Task<IEnumerable<SuspectModel>> InnerGetSuspectsAsync(string search)
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

        // New API operation to search patients provides values for Id property that are not present in suspects controller operations
        private async Task<IEnumerable<SuspectModel>> InnerGetPatientsAsSuspectsAsync(string search)
        {
            UriBuilder builder = new UriBuilder(Settings.ServiceEndpoint);
            builder.Path = $"patient/search/{search}";

            string uri = builder.ToString();

            try
            {
                var suspects = await _requestProvider.GetAsync<IEnumerable<PatientModel>>(uri);
                if (suspects != null)
                {
                    return suspects.Select(s => new SuspectModel
                    {
                        EyeColor = s.EyeColor,
                        HairColor = s.HairColor,
                        Id = s.Id,
                        Name = $"{s.FirstName} {s.LastName}",
                        Sex = s.Sex,
                        SkinColor = s.SkinColor,
                        SuspectSearchImage = s.SuspectSearchImage
                    });
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
