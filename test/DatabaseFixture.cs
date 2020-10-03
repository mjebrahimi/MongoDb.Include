using Mongo2Go;
using MongoDB.Driver;
using System;
using Xunit;

namespace MongoDb.Include.Tests
{
    public class DatabaseFixture : IDisposable
    {
        private readonly MongoDbRunner runner;
        public IMongoDatabase Database { get; }

        public DatabaseFixture()
        {
            if (runner == null)
            {
                runner = MongoDbRunner.Start();
                var client = new MongoClient(runner.ConnectionString);
                Database = client.GetDatabase("MongoDbIncludeTest");
            }
        }

        public void Dispose() => runner.Dispose();
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
