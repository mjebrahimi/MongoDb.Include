using MongoDB.Driver;
using System.Linq.Expressions;
using System.Linq;

namespace MongoDb.Include.Linq
{
    public class AggregateQueryProvider<TEntity> : IQueryProvider
    {
        private readonly IQueryProvider queryProvider;
        private readonly IAggregateQueryable<TEntity> aggregateQueryable;

        public IAggregateFluent<TEntity> AggregateFluent { get; set; }

        internal AggregateQueryProvider(IQueryable queryable, IAggregateQueryable<TEntity> aggregateQueryable, IAggregateFluent<TEntity> aggregateFluent)
        {
            queryable.NotNull(nameof(queryable));
            this.queryProvider = queryable.Provider.NotNull(nameof(queryable.Provider));
            this.aggregateQueryable = aggregateQueryable.NotNull(nameof(aggregateQueryable));
            AggregateFluent = aggregateFluent.NotNull(nameof(aggregateFluent));

            AddQueryToPipeline(queryable);
        }

        public IQueryable CreateQuery(Expression expression)
        {
            var query = queryProvider.CreateQuery(expression);
            AddQueryToPipeline(query);
            return aggregateQueryable;
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            var query = queryProvider.CreateQuery<TElement>(expression);
            AddQueryToPipeline(query);
            return (IQueryable<TElement>)aggregateQueryable;
        }

        public object Execute(Expression expression) => queryProvider.Execute(expression);

        public TResult Execute<TResult>(Expression expression) => queryProvider.Execute<TResult>(expression);

        private void AddQueryToPipeline(IQueryable query)
        {
            var stages = CommandExtractor.ExtractStages<TEntity>(query);

            foreach (var stage in stages)
                AggregateFluent = AggregateFluent.AppendStage(stage);
        }
    }
}
