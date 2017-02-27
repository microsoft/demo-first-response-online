// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MSCorp.FirstResponse.WebApiDemo.Configuration;
using Microsoft.Azure.SqlDatabase.ElasticScale.ShardManagement;

namespace MSCorp.FirstResponse.WebApiDemo.Helpers
{
    internal static class ShardManagementUtils
    {
        /// <summary>
        /// Tries to get the ShardMapManager that is stored in the specified database.
        /// </summary>
        public static ShardMapManager TryGetShardMapManager(string shardMapManagerServerName, string shardMapManagerDatabaseName)
        {
            string shardMapManagerConnectionString =
                    ShardManagmentConfig.GetConnectionString(
                        ShardManagmentConfig.ShardMapManagerServerName,
                        ShardManagmentConfig.ShardMapDatabase);

            if (!SqlDatabaseUtils.DatabaseExists(shardMapManagerServerName, shardMapManagerDatabaseName))
            {
                // Shard Map Manager database has not yet been created
                return null;
            }

            ShardMapManager shardMapManager;
            bool smmExists = ShardMapManagerFactory.TryGetSqlShardMapManager(
                shardMapManagerConnectionString,
                ShardMapManagerLoadPolicy.Lazy,
                out shardMapManager);

            if (!smmExists)
            {
                // Shard Map Manager database exists, but Shard Map Manager has not been created
                return null;
            }

            return shardMapManager;
        }

        /// <summary>
        /// Creates a shard map manager in the database specified by the given connection string.
        /// </summary>
        public static ShardMapManager CreateOrGetShardMapManager(string shardMapManagerConnectionString)
        {
            // Get shard map manager database connection string
            // Try to get a reference to the Shard Map Manager in the Shard Map Manager database. If it doesn't already exist, then create it.
            ShardMapManager shardMapManager;
            bool shardMapManagerExists = ShardMapManagerFactory.TryGetSqlShardMapManager(
                shardMapManagerConnectionString,
                ShardMapManagerLoadPolicy.Lazy,
                out shardMapManager);

            if (!shardMapManagerExists)
            {
                shardMapManager = ShardMapManagerFactory.CreateSqlShardMapManager(shardMapManagerConnectionString);
            }

            return shardMapManager;
        }

        /// <summary>
        /// Creates a new Range Shard Map with the specified name, or gets the Range Shard Map if it already exists.
        /// </summary>
        public static RangeShardMap<T> CreateOrGetRangeShardMap<T>(ShardMapManager shardMapManager, string shardMapName)
        {
            // Try to get a reference to the Shard Map.
            RangeShardMap<T> shardMap;
            bool shardMapExists = shardMapManager.TryGetRangeShardMap(shardMapName, out shardMap);

            if (!shardMapExists)
            {
                // The Shard Map does not exist, so create it
                shardMap = shardMapManager.CreateRangeShardMap<T>(shardMapName);
            }

            return shardMap;
        }

        /// <summary>
        /// Adds Shards to the Shard Map, or returns them if they have already been added.
        /// </summary>
        public static Shard CreateOrGetShard(ShardMap shardMap, ShardLocation shardLocation)
        {
            // Try to get a reference to the Shard
            Shard shard;
            bool shardExists = shardMap.TryGetShard(shardLocation, out shard);

            if (!shardExists)
            {
                shard = shardMap.CreateShard(shardLocation);
            }
            return shard;
        }
    }
}
