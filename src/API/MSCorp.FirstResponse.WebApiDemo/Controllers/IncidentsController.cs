using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using MSCorp.FirstResponse.WebApiDemo.Configuration;
using MSCorp.FirstResponse.WebApiDemo.Constants;
using MSCorp.FirstResponse.WebApiDemo.Helpers;
using MSCorp.FirstResponse.WebApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MSCorp.FirstResponse.WebApiDemo.Controllers
{
    [RoutePrefix("api/incidents")]
    public class IncidentsController : ApiController
    {
        readonly Lazy<RangeShardMap<int>> _shardMap = new Lazy<RangeShardMap<int>>(GetShardMap);

        [HttpGet, Route("{cityId:int}")]
        public IEnumerable<IncidentModel> GetIncidents(int cityId)
        {
            string connectionString = ShardManagmentConfig.GetCredentialsConnectionString();
            Shard[] shards = _shardMap.Value.GetShards().ToArray();

            var query = GetIncidentsQuery(cityId);

            var incidents = QueryHelper.ExecuteMultiShardQuery(connectionString, query, shards);
            return incidents;
        }

        [HttpGet, Route("ambulance/{cityId:int}")]
        public IList<IncidentModel> GetAmbulanceIncidents(int cityId)
        {
            string connectionString = ShardManagmentConfig.GetCredentialsConnectionString();
            Shard shard = _shardMap.Value.GetMappingForKey((int)DepartmentType.Ambulance).Shard;

            return QueryHelper.ExecuteMultiShardQuery(connectionString, GetIncidentsQuery(cityId), shard);
        }

        [HttpGet, Route("police/{cityId:int}")]
        public IList<IncidentModel> GetPoliceIncidents(int cityId)
        {
            string connectionString = ShardManagmentConfig.GetCredentialsConnectionString();
            Shard shard = _shardMap.Value.GetMappingForKey((int)DepartmentType.Police).Shard;

            return QueryHelper.ExecuteMultiShardQuery(connectionString, GetIncidentsQuery(cityId), shard);
        }

        [HttpGet, Route("fire/{cityId:int}")]
        public IList<IncidentModel> GetFireIncidents(int cityId)
        {
            string connectionString = ShardManagmentConfig.GetCredentialsConnectionString();
            Shard shard = _shardMap.Value.GetMappingForKey((int)DepartmentType.Fire).Shard;

            return QueryHelper.ExecuteMultiShardQuery(connectionString, GetIncidentsQuery(cityId), shard);
        }

        private static RangeShardMap<int> GetShardMap()
        {
            var shardMapManager = ShardManagementUtils.TryGetShardMapManager(
                ShardManagmentConfig.ShardMapManagerServerName
                , ShardManagmentConfig.ShardMapDatabase
            );
            return shardMapManager.GetRangeShardMap<int>(ShardManagmentConfig.ShardMapName);
        }



        private static string GetIncidentsQuery(int cityId)
        {

            var query = $@"
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
                    Where i.CityId = {cityId} 
                ";

            return query;
        }

    }
}