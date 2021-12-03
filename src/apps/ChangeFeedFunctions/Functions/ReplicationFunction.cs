using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace CosmosChangeFeed.ChangeFeedFunctions.Functions
{
    public static class ReplicationFunction
    {
        [FunctionName("ReplicationFunction")]
        public static void Run(
            [CosmosDBTrigger(
                databaseName: "%CosmosDB:databaseName%",
                collectionName: "%CosmosDB:containers:replication%",
                ConnectionStringSetting = "%CosmosDB:connectionString%",
                LeaseCollectionName = "%CosmosDB:containers:leases%"
            )]
            IReadOnlyList<Document> input, 
            ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
