﻿using CosmosChangeFeed.Shared;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CosmosChangeFeed.IntegrationFunctions.Startup))]
namespace CosmosChangeFeed.IntegrationFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddShared();
        }
    }
}
