using System;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CfpLibraryHost
{
    // TODO: cleanup this class, this is only for quick demo purposes
    class Program
    {
        private static CosmosClient Client { get; set; }
        
        private static readonly int maxRetryCount = 5;
        private static readonly int pollIntervalSeconds = 5;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromSeconds(5);
        private static readonly string DefaultProcessorName = $"{nameof(CfpLibraryHost)}Processor"; // different processor name creates new entry in leases 
        private static readonly DateTime StartTime = DateTime.MinValue.ToUniversalTime(); // to start from very beginning set min value

        private static string separator = "===========================================================================";
        private static string batchSeparator = "---------------------------------------------------------------------------";

        static async Task Main(string[] args)
        {

            var processorName = args.Length > 0 ? args[0] : string.Empty;
            if (string.IsNullOrEmpty(processorName))
                processorName = DefaultProcessorName;

            Console.WriteLine($"{nameof(CfpLibraryHost)} app started {DateTime.Now}");

            // establish a connection to the database
            Console.WriteLine("establishing connection to cosmos");
            var cosmosAccountName = Environment.GetEnvironmentVariable("CosmosAccountName");
            if (string.IsNullOrEmpty(cosmosAccountName))
                throw new InvalidOperationException("unable to find environment variable CosmosAccountName");

            var cosmosAccountKey = Environment.GetEnvironmentVariable("CosmosAccountKey");
            if (string.IsNullOrEmpty(cosmosAccountKey))
                throw new InvalidOperationException("unable to find environment variable CosmosAccountKey");

            Client = new CosmosClient($"AccountEndpoint=https://{cosmosAccountName}.documents.azure.com:443/;AccountKey={cosmosAccountKey};");

            // get a reference to the containers
            var database = Client.GetDatabase("acme-webstore");
            var cartContainer = database.GetContainer("cart");
            var leaseContainer = database.GetContainer("lease");

            // connect to the CFP 
            Console.WriteLine($"creating change feed processor '{processorName}'");
            var cfp = cartContainer.GetChangeFeedProcessorBuilder<dynamic>(processorName, ProcessChanges)
                .WithLeaseContainer(leaseContainer)
                .WithPollInterval(new TimeSpan(0, 0, 0, pollIntervalSeconds))
                .WithInstanceName($"{processorName} Instance")
                .WithStartTime(StartTime) 
                .Build();
            
            int currentRetry = 0;

            // simple retry pattern
            for (;;)
            {
                try
                {
                    Console.WriteLine($"starting change feed processor '{processorName}'...");
                    await cfp.StartAsync();

                    Console.WriteLine(separator);
                    Console.WriteLine($"Started change feed processor {DateTime.Now} - press any key to stop");
                    Console.WriteLine(separator);
                    Console.ReadKey(true);
                    Console.WriteLine(separator);

                    Console.WriteLine("stopping change feed processor...");
                    await cfp.StopAsync();
                    Console.WriteLine($"Stopped change feed processor {DateTime.Now}");

                    break;
                }
                catch (Exception ex)
                {

                    currentRetry++;

                    // Check if the exception thrown was a transient exception
                    // based on the logic in the error detection strategy.
                    // Determine whether to retry the operation, as well as how
                    // long to wait, based on the retry strategy.
                    if (currentRetry > maxRetryCount || !IsTransient(ex))
                    {
                        Console.WriteLine($"error occurred at {DateTime.Now} and retry count has been reached, application will now terminate");
                        // If this isn't a transient error or we shouldn't retry,
                        // rethrow the exception.
                        throw;
                    }

                    Console.WriteLine($"error occurred at {DateTime.Now}, current retry count: {currentRetry}, retrying connection after delay {RetryDelay}");
                }

                // Wait to retry the operation.
                // Consider calculating an exponential delay here and
                // using a strategy best suited for the operation and fault.
                await Task.Delay(RetryDelay);
            }
        }

        private static bool IsTransient(Exception exception)
        {
            return true;
        }

        static Task ProcessChanges(IReadOnlyCollection<dynamic> docs, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(ProcessChanges)} called - {docs.Count} {nameof(docs)} to process");

            // write any code to respond to changes being made
            foreach (var doc in docs)
            {
                Console.WriteLine($"Document {doc.id} has changed!, CFP processed at {DateTime.Now}");
                Console.WriteLine(doc.ToString());
            }

            Console.WriteLine(batchSeparator);

            return Task.CompletedTask;
        }
    }
}
