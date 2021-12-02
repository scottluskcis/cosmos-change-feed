# Setup

## Overview

Any setup necessary to support the **examples** in this repository is outlined here

## Tools

There are a few ways you can setup the resources needed for the examples in this repository:

* [Azure Portal](https://portal.azure.com/)
* [Bicep](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/overviewd)
* [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/)

I personally prefer to use **Bicep** or **Azure CLI** to provision Azure resources over using the **Azure Portal** directly. Anytime you can write the code to repeat this later, I highly recommend it.

If you want to use Bicep or Azure CLI follow the setup at one of the links below:

* [Get started with Bicep](https://docs.microsoft.com/en-us/azure/azure-resource-manager/bicep/overview#get-started)
* [Install Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

For purposes of this repository, any examples will use Bicep or Azure CLI

## Provision

In order to deploy the resources used in these examples follow the steps below. Descriptions of the resources are further below in this section that explain what resources are used and why.

### Deployment

1. Open powershell or bash
2. Navigate (i.e., _cd_) to the folder where this [README.md](README.md) is located
3. Connect to Azure

    ```bash
    az login
    ```

4. If it does not already exist, create a resource group that will contain these resources. Change the `--name` parameter if you would like to use a different name for your resource group

    ```bash
    az group create --name changefeed-scenarios-rg --location eastus
    ```

5. Check the [main.parameters.json](bicep/main.parameters.json) and modify any parameter values before proceeding
6. Run the command to deploy the [main.bicep](bicep/main.bicep) file

    ```bash
    az deployment group create \
    -g changefeed-scenarios-rg \
    -n ChangeFeedScenariosDeployment \
    -f bicep/main.bicep \
    --parameters @bicep/main.parameters.json
    ```

7. Disconect from Azure

    ```bash
    az logout
    ```

#### Script

As an alternative to walking through the steps above a step at a time you can simply run the script that exists in this repository [setup.azcli](setup.azcli) in bash or powershell

#### Cleanup

Optionally, when finished, you can delete the resource group and all resources if no longer needed by running the following command

```bash
az group delete --name changefeed-scenarios-rg
```

### Cosmos Database

The first thing and most important resource you will need for the examples in this repository is [Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/)
