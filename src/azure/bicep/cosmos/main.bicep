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

// ---------------------------------------------------------------------
// Variables
// ---------------------------------------------------------------------
var cosmosAccountName = 'cosmos-${environment}-${suffix}'
var cosmosDatabaseName = 'acme-webstore'

// ---------------------------------------------------------------------
// Cosmos DB
// ---------------------------------------------------------------------
module cosmosdb './free-tier-database.bicep' = {
  name: 'deployCosmosDB'
  params: {
    accountName: cosmosAccountName
    location: location
    databaseName: cosmosDatabaseName
  }
}

// ---------------------------------------------------------------------
// Containers
// ---------------------------------------------------------------------
module cartContainer './container.bicep' = {
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

module cartByItemContainer './container.bicep' = {
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

module productContainer './container.bicep' = {
  name: 'deployProductContainer'
  params: {
    accountName: cosmosAccountName
    databaseName: cosmosDatabaseName
    containerName: 'product'
    partitionKeyPath: '/categoryId'
    throughputPolicy: 'Manual'
    manualProvisionedThroughput: 400
  }
}

module productMetaContainer './container.bicep' = {
  name: 'deployProductMetaContainer'
  params: {
    accountName: cosmosAccountName
    databaseName: cosmosDatabaseName
    containerName: 'productMeta'
    partitionKeyPath: '/type'
    throughputPolicy: 'Manual'
    manualProvisionedThroughput: 400
  }
}

module leasesContainer './container.bicep' = {
  name: 'deployLeasesContainer'
  params: {
    accountName: cosmosAccountName
    databaseName: cosmosDatabaseName
    containerName: 'leases'
    partitionKeyPath: '/id'
    throughputPolicy: 'Manual'
    manualProvisionedThroughput: 400
  }
}

