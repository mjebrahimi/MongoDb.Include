using MongoDb.Include.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDb.Include
{
    /// <summary>
    /// Represents extension methods for IAggregateQueryable.
    /// </summary>
    public static class IAggregateQueryableExtensions
    {
        #region Exectuor
        /// <summary>
        /// Determines whether any element of a sequence satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence whose elements to test for a condition.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// true if any elements in the source sequence pass the test in the specified predicate; otherwise, false.
        /// </returns>
        public static Task<bool> AnyAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            predicate.NotNull(nameof(predicate));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Any, predicate);
            return aggregate.AnyAsync(cancellationToken);
        }

        /// <summary>
        /// Returns the first element of a sequence that satisfies a specified condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IAggregateQueryable{TSource}" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The first element in <paramref name="source" /> that passes the test in <paramref name="predicate" />.
        /// </returns>
        public static Task<TSource> FirstAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            predicate.NotNull(nameof(predicate));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.First, predicate);
            return aggregate.FirstAsync(cancellationToken);
        }

        /// <summary>
        /// Returns the first element of a sequence that satisfies a specified condition or a default value if no such element is found.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IAggregateQueryable{TSource}" /> to return an element from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// default(<typeparamref name="TSource" />) if <paramref name="source" /> is empty or if no element passes the test specified by <paramref name="predicate" />; otherwise, the first element in <paramref name="source" /> that passes the test specified by <paramref name="predicate" />.
        /// </returns>
        public static Task<TSource> FirstOrDefaultAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            predicate.NotNull(nameof(predicate));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.FirstOrDefault, predicate);
            return aggregate.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition, and throws an exception if more than one such element exists.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IAggregateQueryable{TSource}" /> to return a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The single element of the input sequence that satisfies the condition in <paramref name="predicate" />.
        /// </returns>
        public static Task<TSource> SingleAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            predicate.NotNull(nameof(predicate));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Single, predicate);
            return aggregate.SingleAsync(cancellationToken);
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists; this method throws an exception if more than one element satisfies the condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IAggregateQueryable{TSource}" /> to return a single element from.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The single element of the input sequence that satisfies the condition in <paramref name="predicate" />, or default(<typeparamref name="TSource" />) if no such element is found.
        /// </returns>
        public static Task<TSource> SingleOrDefaultAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            predicate.NotNull(nameof(predicate));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.SingleOrDefault, predicate);
            return aggregate.SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IAggregateQueryable{TSource}" /> that contains the elements to be counted.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The number of elements in the input sequence.
        /// </returns>
        public static async Task<int> CountAsync<TSource>(this IAggregateQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Count);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsInt32;
        }

        /// <summary>
        /// Returns the number of elements in the specified sequence that satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IAggregateQueryable{TSource}" /> that contains the elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The number of elements in the sequence that satisfies the condition in the predicate function.
        /// </returns>
        public static async Task<int> CountAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            predicate.NotNull(nameof(predicate));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Count, predicate);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsInt32;
        }

        /// <summary>
        /// Returns the number of elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IAggregateQueryable{TSource}" /> that contains the elements to be counted.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The number of elements in the input sequence.
        /// </returns>
        public static async Task<long> LongCountAsync<TSource>(this IAggregateQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Count);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsInt64;
        }

        /// <summary>
        /// Returns the number of elements in the specified sequence that satisfies a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IAggregateQueryable{TSource}" /> that contains the elements to be counted.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The number of elements in the sequence that satisfies the condition in the predicate function.
        /// </returns>
        public static async Task<long> LongCountAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, bool>> predicate, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            predicate.NotNull(nameof(predicate));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Count, predicate);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsInt64;
        }

        /// <summary>
        /// Returns the maximum value in a generic <see cref="IAggregateQueryable{TSource}" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The maximum value in the sequence.
        /// </returns>
        public static Task<TSource> MaxAsync<TSource>(this IAggregateQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        /// <summary>
        /// Invokes a projection function on each element of a generic <see cref="IAggregateQueryable{TSource}" /> and returns the maximum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the function represented by <paramref name="selector" />.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The maximum value in the sequence.
        /// </returns>
        public static Task<TResult> MaxAsync<TSource, TResult>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        /// <summary>
        /// Returns the minimum value in a generic <see cref="IAggregateQueryable{TSource}" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The minimum value in the sequence.
        /// </returns>
        public static Task<TSource> MinAsync<TSource>(this IAggregateQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        /// <summary>
        /// Invokes a projection function on each element of a generic <see cref="IAggregateQueryable{TSource}" /> and returns the minimum resulting value.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the function represented by <paramref name="selector" />.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum of.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The minimum value in the sequence.
        /// </returns>
        public static Task<TResult> MinAsync<TSource, TResult>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, TResult>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        #region AverageAsync
        /// <summary>
        /// Computes the average of a sequence of <see cref="Decimal"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The average of the values in the sequence.</returns>
        public static async Task<decimal> AverageAsync(this IAggregateQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDecimal;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="Nullable{Decimal}"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The average of the values in the sequence.</returns>
        public static async Task<decimal?> AverageAsync(this IAggregateQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDecimal;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="Double"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The average of the values in the sequence.</returns>
        public static async Task<double> AverageAsync(this IAggregateQueryable<double> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDouble;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="Nullable{Double}"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The average of the values in the sequence.</returns>
        public static async Task<double?> AverageAsync(this IAggregateQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDouble;
        }

        ///// <summary>
        ///// Computes the average of a sequence of <see cref="Single"/> values.
        ///// </summary>
        ///// <param name="source">A sequence of values to calculate the average of.</param>
        ///// <param name="cancellationToken">The cancellation token.</param>
        ///// <returns>The average of the values in the sequence.</returns>
        //public static Task<float> AverageAsync(this IAggregateQueryable<float> source, CancellationToken cancellationToken = default)
        //{
        //    source.NotNull(nameof(source));

        //    return IAsyncCursorSourceExtensions.AverageAsync(source, cancellationToken);
        //}

        ///// <summary>
        ///// Computes the average of a sequence of <see cref="Nullable{Single}"/> values.
        ///// </summary>
        ///// <param name="source">A sequence of values to calculate the average of.</param>
        ///// <param name="cancellationToken">The cancellation token.</param>
        ///// <returns>The average of the values in the sequence.</returns>
        //public static Task<float?> AverageAsync(this IAggregateQueryable<float?> source, CancellationToken cancellationToken = default)
        //{
        //    source.NotNull(nameof(source));

        //    return IAsyncCursorSourceExtensions.AverageAsync(source, cancellationToken);
        //}

        /// <summary>
        /// Computes the average of a sequence of <see cref="Int32"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The average of the values in the sequence.</returns>
        public static async Task<double> AverageAsync(this IAggregateQueryable<int> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDouble;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="Nullable{Int32}"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The average of the values in the sequence.</returns>
        public static async Task<double?> AverageAsync(this IAggregateQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDouble;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="Int64"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The average of the values in the sequence.</returns>
        public static async Task<double> AverageAsync(this IAggregateQueryable<long> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDouble;
        }

        /// <summary>
        /// Computes the average of a sequence of <see cref="Nullable{Int64}"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the average of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The average of the values in the sequence.</returns>
        public static async Task<double?> AverageAsync(this IAggregateQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDouble;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Decimal" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static async Task<decimal> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDecimal;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Nullable{Decimal}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static async Task<decimal?> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDecimal;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Double" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static async Task<double> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDouble;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Nullable{Double}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static async Task<double?> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDouble;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Single" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static Task<float> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Nullable{Single}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static Task<float?> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Int32" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static async Task<double> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDouble;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Nullable{Int32}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static async Task<double?> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDouble;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Int64" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static async Task<double> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDouble;
        }

        /// <summary>
        /// Computes the average of the sequence of <see cref="Nullable{Int64}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The average of the projected values.
        /// </returns>
        public static async Task<double?> AverageAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Average, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDouble;
        }
        #endregion

        #region SumAsync
        /// <summary>
        /// Computes the sum of a sequence of <see cref="Decimal"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static async Task<decimal> SumAsync(this IAggregateQueryable<decimal> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDecimal;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Decimal}"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static async Task<decimal?> SumAsync(this IAggregateQueryable<decimal?> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDecimal;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Double"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static async Task<double> SumAsync(this IAggregateQueryable<double> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDouble;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Double}"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static async Task<double?> SumAsync(this IAggregateQueryable<double?> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDouble;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Single"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static Task<float> SumAsync(this IAggregateQueryable<float> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Single}"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static Task<float?> SumAsync(this IAggregateQueryable<float?> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Int32"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static async Task<int> SumAsync(this IAggregateQueryable<int> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsInt32;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Int32}"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static async Task<int?> SumAsync(this IAggregateQueryable<int?> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableInt32;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Int64"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static async Task<long> SumAsync(this IAggregateQueryable<long> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsInt64;
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{Int64}"/> values.
        /// </summary>
        /// <param name="source">A sequence of values to calculate the sum of.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static async Task<long?> SumAsync(this IAggregateQueryable<long?> source, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableInt64;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Decimal" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static async Task<decimal> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, decimal>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDecimal;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Nullable{Decimal}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static async Task<decimal?> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, decimal?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDecimal;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Double" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static async Task<double> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, double>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsDouble;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Nullable{Double}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static async Task<double?> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, double?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableDouble;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Single" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static Task<float> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, float>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Nullable{Single}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static Task<float?> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, float?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            throw null;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Int32" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static async Task<int> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, int>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsInt32;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Nullable{Int32}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static async Task<int?> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, int?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableInt32;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Int64" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static async Task<long> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, long>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsInt64;
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Nullable{Int64}" /> values that is obtained
        /// by invoking a projection function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// The sum of the projected values.
        /// </returns>
        public static async Task<long?> SumAsync<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, long?>> selector, CancellationToken cancellationToken = default)
        {
            source.NotNull(nameof(source));
            selector.NotNull(nameof(selector));
            var aggregate = source.CastToAggregateQueryable();

            aggregate.ApplyExpression(Queryable.Sum, selector);
            return (await aggregate.GetBsonValueAsync(cancellationToken).ConfigureAwait(false)).AsNullableInt64;
        }
        #endregion

        #region Utilities
        private static AggregateQueryable<TSource> CastToAggregateQueryable<TSource>(this IAggregateQueryable<TSource> source)
        {
            if (source is not AggregateQueryable<TSource> aggregate)
                throw new NotSupportedException("This IAggregateQueryable<TEntity> is not supported.");
            return aggregate;
        }

        private static void ApplyExpression<TSource, TResult>(this AggregateQueryable<TSource> aggregate, Func<AggregateQueryable<TSource>, TResult> func)
        {
            var methodInfo = func.GetMethodInfo();
            var expression = Expression.Call(methodInfo, aggregate.Expression);
            aggregate.Provider.CreateQuery(expression);
        }

        private static void ApplyExpression<TSource, TParameter, TResult>(this AggregateQueryable<TSource> aggregate, Func<AggregateQueryable<TSource>, TParameter, TResult> func, TParameter parameter)
            where TParameter : Expression
        {
            var methodInfo = func.GetMethodInfo();
            var expression = Expression.Call(methodInfo, aggregate.Expression, Expression.Quote(parameter));
            aggregate.Provider.CreateQuery(expression);
        }

        private static async Task<BsonValue> GetBsonValueAsync<TSource>(this AggregateQueryable<TSource> aggregate, CancellationToken cancellationToken)
        {
            var result = await aggregate.AggregateFluent.As<BsonDocument>().SingleAsync(cancellationToken).ConfigureAwait(false);
            return result.GetValue("__result");
        }
        #endregion

        #endregion

        #region Non Executor
        /// <summary>
        /// Returns distinct elements from a sequence by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The <see cref="IMongoQueryable{TSource}" /> to remove duplicates from.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{TSource}" /> that contains distinct elements from <paramref name="source" />.
        /// </returns>
        public static IAggregateQueryable<TSource> Distinct<TSource>(this IAggregateQueryable<TSource> source)
        {
            return (IAggregateQueryable<TSource>)Queryable.Distinct(source);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by the function represented in keySelector.</typeparam>
        /// <param name="source">An <see cref="IMongoQueryable{TSource}" /> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{T}" /> that has a type argument of <see cref="IGrouping{TKey, TSource}"/>
        /// and where each <see cref="IGrouping{TKey, TSource}"/> object contains a sequence of objects
        /// and a key.
        /// </returns>
        public static IAggregateQueryable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return (IAggregateQueryable<IGrouping<TKey, TSource>>)Queryable.GroupBy(source, keySelector);
        }

        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function
        /// and creates a result value from each group and its key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by the function represented in keySelector.</typeparam>
        /// <typeparam name="TResult">The type of the result value returned by resultSelector.</typeparam>
        /// <param name="source">An <see cref="IMongoQueryable{TSource}" /> whose elements to group.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <param name="resultSelector">A function to create a result value from each group.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{T}" /> that has a type argument of TResult and where
        /// each element represents a projection over a group and its key.
        /// </returns>
        public static IAggregateQueryable<TResult> GroupBy<TSource, TKey, TResult>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, Expression<Func<TKey, IEnumerable<TSource>, TResult>> resultSelector)
        {
            return (IAggregateQueryable<TResult>)Queryable.GroupBy(source, keySelector, resultSelector);
        }

        /// <summary>
        /// Correlates the elements of two sequences based on key equality and groups the results.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to join.</param>
        /// <param name="inner">The sequence to join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from an element from the first sequence and a collection of matching elements from the second sequence.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{TResult}" /> that contains elements of type <typeparamref name="TResult" /> obtained by performing a grouped join on two sequences.
        /// </returns>
        public static IAggregateQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAggregateQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
        {
            return (IAggregateQueryable<TResult>)Queryable.GroupJoin(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        /// <summary>
        /// Correlates the elements of two sequences based on key equality and groups the results.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to join.</param>
        /// <param name="inner">The collection to join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from an element from the first sequence and a collection of matching elements from the second sequence.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{TResult}" /> that contains elements of type <typeparamref name="TResult" /> obtained by performing a grouped join on two sequences.
        /// </returns>
        public static IAggregateQueryable<TResult> GroupJoin<TOuter, TInner, TKey, TResult>(this IAggregateQueryable<TOuter> outer, IMongoCollection<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, IEnumerable<TInner>, TResult>> resultSelector)
        {
            return (IAggregateQueryable<TResult>)Queryable.GroupJoin(outer, inner.AsQueryable(), outerKeySelector, innerKeySelector, resultSelector);
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to join.</param>
        /// <param name="inner">The sequence to join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IQueryable`1" /> that has elements of type <typeparamref name="TResult" /> obtained by performing an inner join on two sequences.
        /// </returns>
        public static IAggregateQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAggregateQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return (IAggregateQueryable<TResult>)Queryable.Join(outer, inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        /// <summary>
        /// Correlates the elements of two sequences based on matching keys.
        /// </summary>
        /// <typeparam name="TOuter">The type of the elements of the first sequence.</typeparam>
        /// <typeparam name="TInner">The type of the elements of the second sequence.</typeparam>
        /// <typeparam name="TKey">The type of the keys returned by the key selector functions.</typeparam>
        /// <typeparam name="TResult">The type of the result elements.</typeparam>
        /// <param name="outer">The first sequence to join.</param>
        /// <param name="inner">The sequence to join to the first sequence.</param>
        /// <param name="outerKeySelector">A function to extract the join key from each element of the first sequence.</param>
        /// <param name="innerKeySelector">A function to extract the join key from each element of the second sequence.</param>
        /// <param name="resultSelector">A function to create a result element from two matching elements.</param>
        /// <returns>
        /// An <see cref="T:System.Linq.IQueryable`1" /> that has elements of type <typeparamref name="TResult" /> obtained by performing an inner join on two sequences.
        /// </returns>
        public static IAggregateQueryable<TResult> Join<TOuter, TInner, TKey, TResult>(this IAggregateQueryable<TOuter> outer, IMongoCollection<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return (IAggregateQueryable<TResult>)Queryable.Join(outer, inner.AsQueryable(), outerKeySelector, innerKeySelector, resultSelector);
        }

        /// <summary>
        /// Filters the elements of an <see cref="IMongoQueryable" /> based on a specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to filter the elements of the sequence on.</typeparam>
        /// <param name="source">An <see cref="IMongoQueryable" /> whose elements to filter.</param>
        /// <returns>
        /// A collection that contains the elements from <paramref name="source" /> that have type <typeparamref name="TResult" />.
        /// </returns>
        public static IAggregateQueryable<TResult> OfType<TResult>(this IMongoQueryable source)
        {
            return (IAggregateQueryable<TResult>)Queryable.OfType<TResult>(source);
        }

        /// <summary>
        /// Projects each element of a sequence into a new form by incorporating the
        /// element's index.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TResult"> The type of the value returned by the function represented by selector.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{TResult}"/> whose elements are the result of invoking a
        /// projection function on each element of source.
        /// </returns>
        public static IAggregateQueryable<TResult> Select<TSource, TResult>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return (IAggregateQueryable<TResult>)Queryable.Select(source, selector);
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{TResult}" /> and combines the resulting sequences into one sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the sequence returned by the function represented by <paramref name="selector" />.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A projection function to apply to each element.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{TResult}" /> whose elements are the result of invoking a one-to-many projection function on each element of the input sequence.
        /// </returns>
        public static IAggregateQueryable<TResult> SelectMany<TSource, TResult>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
        {
            return (IAggregateQueryable<TResult>)Queryable.SelectMany(source, selector);
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{TCollection}" /> and
        /// invokes a result selector function on each element therein. The resulting values from
        /// each intermediate sequence are combined into a single, one-dimensional sequence and returned.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TCollection">The type of the intermediate elements collected by the function represented by <paramref name="collectionSelector" />.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="collectionSelector">A projection function to apply to each element of the input sequence.</param>
        /// <param name="resultSelector">A projection function to apply to each element of each intermediate sequence.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{TResult}" /> whose elements are the result of invoking the one-to-many projection function <paramref name="collectionSelector" /> on each element of <paramref name="source" /> and then mapping each of those sequence elements and their corresponding <paramref name="source" /> element to a result element.
        /// </returns>
        public static IAggregateQueryable<TResult> SelectMany<TSource, TCollection, TResult>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            return (IAggregateQueryable<TResult>)Queryable.SelectMany(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the
        /// remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">An <see cref="IMongoQueryable{TSource}"/> to return elements from.</param>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{TSource}"/> that contains elements that occur after the
        /// specified index in the input sequence.
        /// </returns>
        public static IAggregateQueryable<TSource> Skip<TSource>(this IAggregateQueryable<TSource> source, int count)
        {
            return (IAggregateQueryable<TSource>)Queryable.Skip(source, count);
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{TSource}"/> that contains the specified number of elements
        /// from the start of source.
        /// </returns>
        public static IAggregateQueryable<TSource> Take<TSource>(this IAggregateQueryable<TSource> source, int count)
        {
            return (IAggregateQueryable<TSource>)Queryable.Take(source, count);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by the function that is represented by keySelector.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>
        /// An <see cref="IOrderedMongoQueryable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        public static IAggregateQueryable<TSource> OrderBy<TSource, TKey>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return (IAggregateQueryable<TSource>)Queryable.OrderBy(source, keySelector);
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by the function that is represented by keySelector.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>
        /// An <see cref="IOrderedMongoQueryable{TSource}"/> whose elements are sorted in descending order according to a key.
        /// </returns>
        public static IOrderedAggregateQueryable<TSource> OrderByDescending<TSource, TKey>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return (IOrderedAggregateQueryable<TSource>)Queryable.OrderByDescending(source, keySelector);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in ascending
        /// order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by the function that is represented by keySelector.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>
        /// An <see cref="IOrderedMongoQueryable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        public static IOrderedAggregateQueryable<TSource> ThenBy<TSource, TKey>(this IOrderedAggregateQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return (IOrderedAggregateQueryable<TSource>)Queryable.ThenBy(source, keySelector);
        }

        /// <summary>
        /// Performs a subsequent ordering of the elements in a sequence in descending
        /// order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by the function that is represented by keySelector.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="keySelector">A function to extract a key from an element.</param>
        /// <returns>
        /// An <see cref="IOrderedMongoQueryable{TSource}"/> whose elements are sorted in descending order according to a key.
        /// </returns>
        public static IOrderedAggregateQueryable<TSource> ThenByDescending<TSource, TKey>(this IOrderedAggregateQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            return (IOrderedAggregateQueryable<TSource>)Queryable.ThenByDescending(source, keySelector);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
        /// <param name="source">An <see cref="IMongoQueryable{TSource}"/> to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        /// An <see cref="IMongoQueryable{TSource}"/> that contains elements from the input sequence
        /// that satisfy the condition specified by predicate.
        /// </returns>
        public static IAggregateQueryable<TSource> Where<TSource>(this IAggregateQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return (IAggregateQueryable<TSource>)Queryable.Where(source, predicate);
        }
        #endregion
    }
}
