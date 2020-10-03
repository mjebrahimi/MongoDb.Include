using System.Linq;

namespace MongoDb.Include.Linq
{
    /// <summary>
    /// Represents the result of a sorting operation.
    /// </summary>
    /// <typeparam name="TEntity">The type of the content of the data source.</typeparam>
    public interface IOrderedAggregateQueryable<out TEntity> : IOrderedQueryable<TEntity>, IAggregateQueryable<TEntity>
    {
    }

    ///// <summary>
    ///// Represents the result of a sorting operation.
    ///// </summary>
    ///// <typeparam name="TEntity">The type of the content of the data source.</typeparam>
    //internal class OrderedAggregateQueryable<out TEntity> : AggregateQueryableCombination<TEntity>, IOrderedAggregateQueryable<TEntity>
    //{
    //    internal OrderedAggregateQueryable(IAggregateQueryable<TEntity> aggregateQueryable)
    //        : base(aggregateQueryable)
    //    {
    //    }
    //}
}
