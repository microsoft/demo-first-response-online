using System.Security.Principal;
using Microsoft.Spatial;
using MSCorp.FirstResponse.WebApiDemo.Configuration;

namespace MSCorp.FirstResponse.WebApiDemo.Helpers
{
    /// <summary>
    /// The helper method simulates a service or repository that provides the geolocation of a caller to be used by
    /// an azure search query's scoring profile.
    /// </summary>
    public static class LocationHelper
    {
        /// <summary>
        /// This method simulates the dispatcher providing the caller's home location
        /// </summary>
        /// <returns></returns>
        public static GeographyPosition GetCallerLocation()
        {
            //Stub to retun the identity of the current principal.
            return AzureSearchConfig.SearchGeographyPoint;
        }
    }
}