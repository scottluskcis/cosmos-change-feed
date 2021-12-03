using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos.Fluent;

namespace CosmosChangeFeed.Shared.Context
{
    public interface IClientContext
    {

    }

    public sealed class CosmosClientContext : IClientContext
    {
    }
}
