using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CosmosChangeFeed.ChangeFeedFunctions.Startup))]
namespace CosmosChangeFeed.ChangeFeedFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            throw new System.NotImplementedException();
        }
    }
}
