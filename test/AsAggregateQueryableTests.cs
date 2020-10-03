using MongoDb.Include.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace MongoDb.Include.Tests
{
    [Collection("Database collection")]
    public class AsAggregateQueryableTests
    {
        private readonly IMongoCollection<Post> collection;

        public AsAggregateQueryableTests(DatabaseFixture databaseFixture)
        {
            this.collection = databaseFixture.Database.GetCollection<Post>();
            //TODO:
            var a = nameof(IAsyncCursorSourceExtensions.ToListAsync);
        }

        [Fact]
        public void Queryable_To_AggregateQueryable_CheckStages()
        {
            var aggregate = collection.Aggregate()
                .Match(p => p.Title.Contains("test"))
                .SortBy(p => p.Id)
                .Skip(10)
                .Limit(5);

            var aggregateQueryable = collection.AsQueryable()
                .Where(p => p.Title.Contains("test"))
                .OrderBy(p => p.Id)
                .Skip(10)
                .Take(5)
                .AsAggregateQueryable();

            CompareStages(aggregate, aggregateQueryable);
        }

        [Fact]
        public void Collection_To_AggregateQueryable_CheckStages()
        {
            var aggregate = collection.Aggregate()
                .Match(p => p.Title.Contains("test"))
                .SortBy(p => p.Id)
                .Skip(10)
                .Limit(5);

            var aggregateQueryable = collection.AsAggregateQueryable()
                .Where(p => p.Title.Contains("test"))
                .OrderBy(p => p.Id)
                .Skip(10)
                .Take(5);

            CompareStages(aggregate, aggregateQueryable);
        }

        [Fact]
        public void Aggregate_To_AggregateQueryable_CheckStages()
        {
            var aggregate = collection.Aggregate()
                .Match(p => p.Title.Contains("test"))
                .SortBy(p => p.Id)
                .Skip(10)
                .Limit(5);

            var aggregateQueryable = collection.Aggregate()
                .Match(p => p.Title.Contains("test"))
                .SortBy(p => p.Id)
                .AsAggregateQueryable()
                .Skip(10)
                .Take(5);

            CompareStages(aggregate, aggregateQueryable);
        }

        private void CompareStages(IAggregateFluent<Post> aggregate, IAggregateQueryable<Post> aggregateQueryable)
        {
            var leftList = aggregate.Stages;
            var rightList = ((AggregateQueryable<Post>)aggregateQueryable).AggregateFluent.Stages;

            Assert.Equal(leftList.Count, rightList.Count);

            for (int i = 0; i < leftList.Count; i++)
            {
                var left = leftList[i];
                var right = rightList[i];

                Assert.Equal(left.InputType, right.InputType);
                Assert.Equal(left.OutputType, right.OutputType);
                Assert.Equal(left.ToString(), right.ToString());
            }
        }

        [Table("post1")]
        public class Post
        {
            public ObjectId Id { get; set; }
            public string Title { get; set; }
        }
    }
}
