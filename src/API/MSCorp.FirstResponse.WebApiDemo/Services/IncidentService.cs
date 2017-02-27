using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using MSCorp.FirstResponse.WebApiDemo.Configuration;
using MSCorp.FirstResponse.WebApiDemo.Helpers;
using MSCorp.FirstResponse.WebApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MSCorp.FirstResponse.WebApiDemo.Services
{
    public class IncidentService
    {
        readonly Lazy<RangeShardMap<int>> _shardMap = new Lazy<RangeShardMap<int>>(GetShardMap);

        public IncidentModel GetIncident(int city, int incidentId)
        {
            string connectionString = ShardManagmentConfig.GetCredentialsConnectionString();
            Shard[] shards = _shardMap.Value.GetShards().ToArray();

            var query =
                IncidentQuery
                + $@"WHERE i.Id = {incidentId} AND i.CityId = {city};";

            var incidents = QueryHelper.ExecuteMultiShardQuery(connectionString, query, shards);
            var incident = incidents.FirstOrDefault(i => i.CityId == city && i.Id == incidentId);

            return incident;

        }

        public IEnumerable<IncidentModel> GetCityIncidents(int city)
        {
            string connectionString = ShardManagmentConfig.GetCredentialsConnectionString();
            Shard[] shards = _shardMap.Value.GetShards().ToArray();

            var query =
                IncidentQuery
                + $@"WHERE i.CityId = {city}";

            var incidents = QueryHelper.ExecuteMultiShardQuery(connectionString, query, shards);
            return incidents;
        }

        private static RangeShardMap<int> GetShardMap()
        {
            var shardMapManager = ShardManagementUtils.TryGetShardMapManager(
                ShardManagmentConfig.ShardMapManagerServerName
                , ShardManagmentConfig.ShardMapDatabase
            );
            return shardMapManager.GetRangeShardMap<int>(ShardManagmentConfig.ShardMapName);
        }

        private static string IncidentQuery => @"
                     SELECT 
                        i.Id, 
                        i.CityId, 
                        i.CallNumber, 
                        i.Phone, 
                        i.UnmaskedPhone, 
                        i.Title, 
                        i.ReceivedTime, 
                        i.Address, 
                        i.ReportingParty, 
                        i.UnmaskedReportingParty, 
                        i.Description, 
                        i.UpdateDescription, 
                        i.Longitude, 
                        i.Latitude,
                        i.IsHighPriority,
                        i.IncidentCategory,
                        i.SearchAreaId
                    FROM 
                        dbo.Incidents AS i
                ";


    }
}