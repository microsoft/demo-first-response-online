using System;
using System.Collections.Generic;
using Microsoft.Azure.SqlDatabase.ElasticScale.Query;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;
using MSCorp.FirstResponse.WebApiDemo.Constants;
using MSCorp.FirstResponse.WebApiDemo.Models;


namespace MSCorp.FirstResponse.WebApiDemo.Helpers
{
    public static class QueryHelper
    {

        /// <summary>
        /// Executes a sql command on an elastic scale DB over all shards available in the provided shard map and returns incidents.
        /// </summary>
        public static IList<IncidentModel> ExecuteMultiShardQuery(string credentialsConnectionString, string commandText, params Shard[] shards)
        {
            if (shards == null)
            {
                throw new ArgumentNullException(nameof(shards));
            }
            if (credentialsConnectionString == null)
            {
                throw new ArgumentNullException(nameof(credentialsConnectionString));
            }
            if (commandText == null)
            {
                throw new ArgumentNullException(nameof(commandText));
            }

            // Get the shards to connect to
            List<IncidentModel> result = new List<IncidentModel>();
            // Create the multi-shard connection
            using (MultiShardConnection conn = new MultiShardConnection(shards, credentialsConnectionString))
            {
                // Create a simple command
                using (MultiShardCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;

                    // Append a column with the shard name where the row came from
                    cmd.ExecutionOptions = MultiShardExecutionOptions.IncludeShardNameColumn;

                    // Allow for partial results in case some shards do not respond in time
                    cmd.ExecutionPolicy = MultiShardExecutionPolicy.PartialResults;

                    // Allow the entire command to take up to 30 seconds
                    cmd.CommandTimeout = 30;

                    // Execute the command. 
                    // We do not need to specify retry logic because MultiShardDataReader will internally retry until the CommandTimeout expires.
                    using (MultiShardDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetFieldValue<int>(0);
                            var cityId = reader.GetFieldValue<int>(1);
                            var callNumber = reader.GetFieldValue<string>(2);
                            var phone = reader.GetFieldValue<string>(3);
                            var unmaskedPhone = reader.GetFieldValue<string>(4);
                            var title = reader.GetFieldValue<string>(5);
                            var receivedTime = reader.GetFieldValue<DateTime>(6);
                            var address = reader.GetFieldValue<string>(7);
                            var reportingParty = reader.GetFieldValue<string>(8);
                            var unmaskedReportingParty = reader.GetFieldValue<string>(9);
                            var description = reader.GetFieldValue<string>(10);
                            var updateDescription = reader.GetFieldValue<string>(11);
                            var longitude = reader.GetFieldValue<double>(12);
                            var latitude = reader.GetFieldValue<double>(13);
                            var isHighPriority = reader.GetFieldValue<bool>(14);
                            var incidentCategory = reader.GetFieldValue<IncidentType>(15);

                            var incident = new IncidentModel
                            {
                                Id = id,
                                CityId = cityId,
                                CallNumber = callNumber,
                                Phone = phone,
                                UnmaskedPhone = unmaskedPhone,
                                Title = title,
                                ReceivedTime = receivedTime,
                                Address = address,
                                ReportingParty = reportingParty,
                                UnmaskedReportingParty = unmaskedReportingParty,
                                Description = description,
                                UpdateDescription = updateDescription,
                                Longitude = longitude,
                                Latitude = latitude,
                                IsHighPriority = isHighPriority,
                                IncidentCategory = incidentCategory,
                                SearchAreaId = null
                            };

                            result.Add(incident);
                        }
                    }
                }
            }
            return result;
        }

        private static string ExtractDatabaseName(string shardLocationString)
        {
            string[] pattern = { "[", "DataSource=", "Database=", "]" };
            string[] matches = shardLocationString.Split(pattern, StringSplitOptions.RemoveEmptyEntries);
            return matches[1];
        }
    }
}