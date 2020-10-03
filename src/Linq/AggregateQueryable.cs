using MongoDB.Driver.Linq;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace MongoDb.Include.Linq
{
    /// <inheritdoc />
    internal class AggregateQueryable<TEntity> : IAggregateQueryable<TEntity>, IOrderedAggregateQueryable<TEntity>, IAsyncCursorSource<TEntity>
    {
        internal AggregateQueryable(AggregateQueryable<TEntity> aggregateQueryable)
        {
            aggregateQueryable.NotNull(nameof(aggregateQueryable));
            aggregateQueryable.aggregateQueryProvider.NotNull(nameof(aggregateQueryable.aggregateQueryProvider));
            aggregateQueryable.ElementType.NotNull(nameof(aggregateQueryable.ElementType));
            aggregateQueryable.Expression.NotNull(nameof(aggregateQueryable.Expression));

            aggregateQueryProvider = aggregateQueryable.aggregateQueryProvider;
            ElementType = aggregateQueryable.ElementType;
            Expression = aggregateQueryable.Expression;
        }

        internal AggregateQueryable(IAggregateFluent<TEntity> aggregate, IMongoQueryable<TEntity> queryable)
        {
            aggregate.NotNull(nameof(aggregate));
            queryable.NotNull(nameof(queryable));
            queryable.ElementType.NotNull(nameof(queryable.ElementType));
            queryable.Expression.NotNull(nameof(queryable.Expression));

            aggregateQueryProvider = new AggregateQueryProvider<TEntity>(queryable, this, aggregate);
            ElementType = queryable.ElementType;
            Expression = queryable.Expression;
        }

        private readonly AggregateQueryProvider<TEntity> aggregateQueryProvider;

        internal IAggregateFluent<TEntity> AggregateFluent
        {
            get => aggregateQueryProvider.AggregateFluent;
            set => aggregateQueryProvider.AggregateFluent = value;
        }

        /// <inheritdoc />
        public Type ElementType { get; }

        /// <inheritdoc />
        public Expression Expression { get; }

        /// <inheritdoc />
        public IQueryProvider Provider => aggregateQueryProvider;

        /// <inheritdoc />
        public IEnumerator<TEntity> GetEnumerator() => aggregateQueryProvider.AggregateFluent.ToEnumerable().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => aggregateQueryProvider.AggregateFluent.ToEnumerable().GetEnumerator();

        /// <inheritdoc />
        public IAsyncCursor<TEntity> ToCursor(CancellationToken cancellationToken = default) => aggregateQueryProvider.AggregateFluent.ToCursor(cancellationToken);

        /// <inheritdoc />
        public Task<IAsyncCursor<TEntity>> ToCursorAsync(CancellationToken cancellationToken = default) => aggregateQueryProvider.AggregateFluent.ToCursorAsync(cancellationToken);
    }
}
