@description('Location for all resources.')
param location string = resourceGroup().location

@allowed([
  'dev'
  'stage'
  'prod'
])
param environment string = 'dev'

@description('Suffix to append to resources')
param suffix string

var cosmosAccountName = 'cosmos-${environment}-${suffix}'
var cosmosDatabaseName = 'acme-webstore'

// Cosmos DB
module cosmosdb './cosmos/free-tier-database.bicep' = {
  name: 'deployCosmosDB'
  params: {
    accountName: cosmosAccountName
    location: location
    databaseName: cosmosDatabaseName
  }
}

module cartContainer './cosmos/container.bicep' = {
  name: 'deployCartContainer'
  params: {
    accountName: cosmosAccountName
    databaseName: cosmosDatabaseName
    containerName: 'cart'
    partitionKeyPath: '/cartId'
    throughputPolicy: 'Manual'
    manualProvisionedThroughput: 400
  }
}

module cartByItemContainer './cosmos/container.bicep' = {
  name: 'deployCartByItemContainer'
  params: {
    accountName: cosmosAccountName
    databaseName: cosmosDatabaseName
    containerName: 'cartByItem'
    partitionKeyPath: '/item'
    throughputPolicy: 'Manual'
    manualProvisionedThroughput: 400
  }
}

module leaseContainer './cosmos/container.bicep' = {
  name: 'deployLeaseContainer'
  params: {
    accountName: cosmosAccountName
    databaseName: cosmosDatabaseName
    containerName: 'lease'
    partitionKeyPath: '/id'
    throughputPolicy: 'Manual'
    manualProvisionedThroughput: 400
  }
}

