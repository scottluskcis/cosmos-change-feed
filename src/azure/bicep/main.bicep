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

module cosmosdb './cosmos/main.bicep' = {
  name: 'deployCosmos'
  params: {
    environment: environment
    location: location
    suffix: suffix
  }
}
