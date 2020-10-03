using System.Linq;

namespace MongoDb.Include.Linq
{
    /// <summary>
    /// Provides functionality to evaluate queries against MongoDB wherein the type of the data is known.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of the data in the data source. This type parameter is covariant. That
    /// is, you can use either the type you specified or any type that is more derived.
    /// For more information about covariance and contravariance, see Covariance and
    /// Contravariance in Generics.
    /// </typeparam>
    public interface IAggregateQueryable<out TEntity> : IQueryable<TEntity>
    {
    }
}
