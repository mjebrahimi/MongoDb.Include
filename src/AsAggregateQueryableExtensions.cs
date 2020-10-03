using MongoDb.Include.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Reflection;

namespace MongoDb.Include
{
    public static class AsAggregateQueryableExtensions
    {
        public static IAggregateQueryable<TEntity> AsAggregateQueryable<TEntity>(this IMongoCollection<TEntity> collection, AggregateOptions options = null, IClientSessionHandle session = null)
        {
            collection.NotNull(nameof(collection));

            var aggregate = session == null ? collection.Aggregate(options) : collection.Aggregate(session, options);
            var queryable = collection.AsQueryable(options);

            return new AggregateQueryable<TEntity>(aggregate, queryable);
        }

        public static IAggregateQueryable<TEntity> AsAggregateQueryable<TEntity>(this IAggregateFluent<TEntity> aggregate)
        {
            aggregate.NotNull(nameof(aggregate));

            var field = aggregate.GetType().GetTypeInfo().GetField("_collection", BindingFlags.Instance | BindingFlags.NonPublic);

            if (!(field?.GetValue(aggregate) is IMongoCollection<TEntity> collection))
                throw new NotSupportedException("This type of IAggregateFluent<> dose not support converting to IAggregateQueryable<>");

            var queryable = collection.AsQueryable(aggregate.Options);

            return new AggregateQueryable<TEntity>(aggregate, queryable);
        }

        public static IAggregateQueryable<TEntity> AsAggregateQueryable<TEntity>(this IMongoQueryable<TEntity> queryable)
        {
            queryable.NotNull(nameof(queryable));

            var type = queryable.Provider.GetType().GetTypeInfo();
            var collectionField = type.GetField("_collection", BindingFlags.Instance | BindingFlags.NonPublic);
            var optionsField = type.GetField("_options", BindingFlags.Instance | BindingFlags.NonPublic);

            if (!(collectionField?.GetValue(queryable.Provider) is IMongoCollection<TEntity> collection))
                throw new NotSupportedException("This type of IMongoQueryable<> dose not support converting to IAggregateQueryable<>");
            if (!(optionsField?.GetValue(queryable.Provider) is AggregateOptions options))
                throw new NotSupportedException("This type of IMongoQueryable<> dose not support converting to IAggregateQueryable<>");

            var aggregate = collection.Aggregate(options);

            return new AggregateQueryable<TEntity>(aggregate, queryable);
        }
    }
}
