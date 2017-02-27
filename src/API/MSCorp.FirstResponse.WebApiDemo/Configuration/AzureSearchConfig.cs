using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Spatial;

namespace MSCorp.FirstResponse.WebApiDemo.Configuration
{
    /// <summary>
    /// Config for azure search
    /// </summary>
    public static class AzureSearchConfig
    {
        /// <summary>
        /// The search index name
        /// </summary>
        public static string SearchIndexName => ConfigurationManager.AppSettings["SearchIndexName"];

        /// <summary>
        /// The search service name
        /// </summary>
        public static string SearchServiceName => ConfigurationManager.AppSettings["SearchServiceName"];

        /// <summary>
        /// The search service api key
        /// </summary>
        public static string SearchServiceApiKey => ConfigurationManager.AppSettings["SearchServiceApiKey"];

        /// <summary>
        /// The facets that are available on the search index
        /// </summary>
        public static IList<string> SearchIndexFacets => ConfigurationManager.AppSettings["SearchIndexFacets"].Split(',');

        /// <summary>
        /// The position to be scored against by the scoring profile
        /// </summary>
        public static GeographyPosition SearchGeographyPoint => new GeographyPosition(Double.Parse(ConfigurationManager.AppSettings["SearchLatitude"]), Double.Parse(ConfigurationManager.AppSettings["SearchLongitude"]));

        /// <summary>
        /// The name of the reference point for the geo scoring profile
        /// </summary>
        public static string GeoScoringParameterName => ConfigurationManager.AppSettings["GeoScoringParameterName"];

        /// <summary>
        /// The name of the scoring profile for geo scoring
        /// </summary>
        public static string ScoringProfileName => ConfigurationManager.AppSettings["ScoringProfileName"];
    }
}