
# CFP Library Example

## Overview

To run `CfpLibraryHost` you will need to have a `launchSettings.json` file that exists in the `Properties` folder. You can set these values by going to the `Debug` `Properties` of the `csproj` itself or by adding this file and updating the values

```json
{
  "profiles": {
    "CfpLibraryHost": {
      "commandName": "Project",
      "commandLineArgs": "your_processor_name",
      "environmentVariables": {
        "CosmosAccountKey": "your_account_key",
        "CosmosAccountName": "your_account_name"
      }
    }
  }
}
```

## References

* [Building Event-driven Microservices with the Azure Cosmos DB Change Feed by Leonard Lobel](https://app.pluralsight.com/library/courses/building-event-driven-microservices-azure-cosmos-db-change-feed/exercise-files)
