namespace MongoDb.Include.Linq
{
    /// <summary>
    /// Represents the result of a included operation.
    /// </summary>
    /// <typeparam name="TEntity">The type of the content of the data TEntity.</typeparam>
    /// <typeparam name="TProperty">The type of the content of the data TProperty.</typeparam>
    public interface IAggregateQueryableIncluded<out TEntity, out TProperty> : IAggregateQueryable<TEntity>
    {
        string NavigationElementName { get; }
    }

    /// <summary>
    /// Represents the result of a included operation.
    /// </summary>
    /// <typeparam name="TEntity">The type of the content of the data TEntity.</typeparam>
    /// <typeparam name="TProperty">The type of the content of the data TProperty.</typeparam>
    internal class AggregateQueryableIncluded<TEntity, TProperty> : AggregateQueryable<TEntity>, IAggregateQueryableIncluded<TEntity, TProperty>
    {
        public string NavigationElementName { get; }

        public AggregateQueryableIncluded(AggregateQueryable<TEntity> aggregateQueryable, string navigationElementName)
            : base(aggregateQueryable)
        {
            NavigationElementName = navigationElementName.NotNull(nameof(navigationElementName));
        }
    }
}
