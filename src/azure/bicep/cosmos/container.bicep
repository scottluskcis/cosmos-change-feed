@description('Cosmos DB account name')
param accountName string

@description('The name for the Core (SQL) database')
param databaseName string

@description('The name for the container')
param containerName string

@description('The partition key for the container')
param partitionKeyPath string = '/partitionKey'

@description('The throughput policy for the container')
@allowed([
  'Manual'
  'Autoscale'
])
param throughputPolicy string = 'Autoscale'

@description('Throughput value when using Manual Throughput Policy for the container')
@minValue(400)
@maxValue(1000000)
param manualProvisionedThroughput int = 400

@description('Maximum throughput when using Autoscale Throughput Policy for the container')
@minValue(4000)
@maxValue(1000000)
param autoscaleMaxThroughput int = 4000

var throughput_Policy = {
  Manual: {
    Throughput: manualProvisionedThroughput
  }
  Autoscale: {
    autoscaleSettings: {
      maxThroughput: autoscaleMaxThroughput
    }
  }
}

resource cosmosContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2021-04-15' = {
  name: '${accountName}/${databaseName}/${containerName}'
  properties: {
    resource: {
      id: containerName
      partitionKey: {
        paths: [
          partitionKeyPath
        ]
        kind: 'Hash'
      }
      indexingPolicy: {
        indexingMode: 'consistent'
        includedPaths: [
          {
            path: '/*'
          }
        ]
      }
      //defaultTtl: 86400
    }
    options: throughput_Policy[throughputPolicy]
  }
}
