# Overview

If you are new to change feed be sure to check out the Microsoft Documentation: [Change feed in Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/change-feed) and [Features of change feed](https://docs.microsoft.com/en-us/azure/cosmos-db/change-feed#features-of-change-feed). Below are some notable points that come from the documentation.

**Change feed overview**:

* A persistent record of changes to a container in the order they occur
* Works by listening to an Azure Cosmos container for any changes
* Outputs the sorted list of documents that were changed in the order in which they were modified
* The persisted changes can be processed asynchronously and incrementally, and the output can be distributed across one or more consumers for parallel processing.

**Features of change feed**:

* Change feed is available for each logical partition key within a container.
* Change feed can be distributed across one or more consumers for parallel processing but currently implementation only takes advantage of one (i.e., via Azure Function).
* Current version of change feed does not support filtering, in other words all inserts and updates are sent thru change feed.
* Current version of change feed supports Inserts and Updates but does not track hard deletes.
* You can track deletes by using a "soft-delete" flag in conjunction with a TTL time to live
* When a change (insert/update) occurs it will appear only once in the change feed, clients consuming change feed manage checkpoint logic.
* Only the most recent change for an item will be in the change feed log. Change feed is not an audit log or a way to track history.

## Examples

The examples folder contains various examples of scenarios you may encounter where change feed could be beneficial

* [Replication](./examples/Replication/README.md): Replicating containers for multiple partition keys
* [Denormalization](./examples/Denormalization/README.md): Denormalizing a document data model across containers
* [Notifications](./examples/Notifications/README.md): Triggering API calls for an event-driven architecture
* [Materialized Views](./examples/MaterializedViews/README.md): Real time stream processing and materialized view patterns
* [Data Movement](./examples/DataMovement/README.md): Moving or archiving data to secondary data stores
