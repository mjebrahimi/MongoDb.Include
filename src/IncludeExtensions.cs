using MongoDb.Include.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MongoDb.Include.Tests")]
namespace MongoDb.Include
{
    public static class IncludeExtensions
    {
        public static IAggregateQueryableIncluded<TEntity, TProperty> Include<TEntity, TProperty>(
            this IAggregateQueryable<TEntity> query,
            Expression<Func<TEntity, TProperty>> navigationPath)
        {
            query.NotNull(nameof(query));
            navigationPath.NotNull(nameof(navigationPath));

            var aggregateQueryable = CastToAggregateQueryable(query);

            ResolveMemberInfoAndPredicate(navigationPath, out var navigationMember, out var predicate);
            navigationMember.EnsureMemberIsCustomType();

            ApplyLookup<TEntity, TEntity, TProperty>(aggregateQueryable, navigationMember, null,
                out var navigationElementName, out var asFieldName);

            ApplyPredicate<TEntity, TProperty>(aggregateQueryable, predicate, asFieldName);

            ApplyProjection<TEntity, TProperty>(aggregateQueryable, asFieldName, navigationElementName);

            return new AggregateQueryableIncluded<TEntity, TProperty>(aggregateQueryable, navigationElementName);
        }

        public static IAggregateQueryableIncluded<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IAggregateQueryableIncluded<TEntity, TPreviousProperty> query,
            Expression<Func<TPreviousProperty, TProperty>> navigationPath)
        {
            query.NotNull(nameof(query));
            navigationPath.NotNull(nameof(navigationPath));

            var aggregateQueryable = CastToAggregateQueryable(query);

            ResolveMemberInfoAndPredicate(navigationPath, out var navigationMember, out var predicate);
            navigationMember.EnsureMemberIsCustomType();

            ApplyLookup<TEntity, TPreviousProperty, TProperty>(aggregateQueryable, navigationMember, query.NavigationElementName,
                out var navigationElementName, out var asFieldName);

            ApplyPredicate<TEntity, TProperty>(aggregateQueryable, predicate, asFieldName);

            ApplyProjection<TEntity, TProperty>(aggregateQueryable, asFieldName, navigationElementName);

            return new AggregateQueryableIncluded<TEntity, TProperty>(aggregateQueryable, navigationElementName);
        }

        public static IAggregateQueryableIncluded<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IAggregateQueryableIncluded<TEntity, IEnumerable<TPreviousProperty>> query,
            Expression<Func<TPreviousProperty, TProperty>> navigationPath)
        {
            query.NotNull(nameof(query));
            navigationPath.NotNull(nameof(navigationPath));

            var aggregateQueryable = CastToAggregateQueryable(query);

            ResolveMemberInfoAndPredicate(navigationPath, out var navigationMember, out var predicate);
            navigationMember.EnsureMemberIsCustomType();

            ApplyLookup<TEntity, TPreviousProperty, TProperty>(aggregateQueryable, navigationMember, query.NavigationElementName,
                out var navigationElementName, out var asFieldName);

            ApplyPredicate<TEntity, TProperty>(aggregateQueryable, predicate, asFieldName);

            ApplyProjection<TEntity, TProperty>(aggregateQueryable, asFieldName, navigationElementName);

            return new AggregateQueryableIncluded<TEntity, TProperty>(aggregateQueryable, navigationElementName);
        }

        #region Utilities
        private static AggregateQueryable<TEntity> CastToAggregateQueryable<TEntity>(IAggregateQueryable<TEntity> query)
        {
            if (query is not AggregateQueryable<TEntity> aggregateQueryable)
                throw new NotSupportedException("This IAggregateQueryable<TEntity> is not supported.");

            return aggregateQueryable;
        }

        private static void ResolveMemberInfoAndPredicate<TEntity, TForeign>(Expression<Func<TEntity, TForeign>> expression, out MemberInfo memberInfo, out Expression predicate)
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                throw new NotSupportedException("Include filter does not supported yet.");

                //if (methodCall.Arguments.Count != 2 ||
                //    methodCall.Arguments[0] is not MemberExpression memberExpression ||
                //    methodCall.Arguments[1] is not LambdaExpression lambdaExpression ||
                //    lambdaExpression.ReturnType != typeof(bool))
                //{
                //    throw new InvalidOperationException("Invalid lambda expression.");
                //}

                //predicate = lambdaExpression;
                //memberInfo = memberExpression.Member;
            }
            else
            {
                predicate = null;
                memberInfo = expression.GetMember();
            }

            memberInfo.EnsureMemberIsFieldOrProperty();
        }

        private static void ApplyLookup<TEntity, TDeclaring, TProperty>(AggregateQueryable<TEntity> aggregateQueryable,
            MemberInfo navigationMember, string previousNavigationName, out string navigationElementName, out string asFieldName)
        {
            var declaringType = typeof(TDeclaring);
            var propertyType = typeof(TProperty);
            var isEnumerable = propertyType.IsEnumerable();
            if (isEnumerable)
                propertyType = propertyType.GetElementTypeOf();

            var associatedMember = AssociatedMemberFinder.GetAssociatedMember(navigationMember);
            var associatedElementName = associatedMember.GetBsonElementName();

            var elementName = navigationMember.Name;
            navigationElementName = AppendPreviousNavigationName(elementName);
            asFieldName = AppendPreviousNavigationName(isEnumerable ? elementName : $"__{elementName}__");

            var collectioName = CollectionNameFinder.GetCollectionName(propertyType);
            string localField;
            string foreignField;

            var isInside = associatedMember.DeclaringType == declaringType;
            if (isInside)
            {
                localField = AppendPreviousNavigationName(associatedElementName);
                foreignField = BsonClassMap.LookupClassMap(propertyType).IdMemberMap.ElementName;
            }
            else
            {
                localField = BsonClassMap.LookupClassMap(declaringType).IdMemberMap.ElementName;
                foreignField = AppendPreviousNavigationName(associatedElementName);
            }

            aggregateQueryable.AggregateFluent = aggregateQueryable.AggregateFluent
                .Lookup<TProperty, TDeclaring>(collectioName, localField, foreignField, asFieldName)
                .As<TEntity>();

            string AppendPreviousNavigationName(string input)
            {
                if (previousNavigationName == null)
                    return input;
                return previousNavigationName + "." + input;
            }
        }

        private static void ApplyPredicate<TEntity, TForeign>(AggregateQueryable<TEntity> aggregateQueryable, Expression predicate, string asFieldName)
        {
            if (predicate == null)
                return;

            var foreignType = typeof(TForeign);
            var isEnumerable = typeof(TForeign).IsEnumerable();
            if (isEnumerable)
                foreignType = foreignType.GetElementTypeOf();

            //lamb = Expression.Lambda(methodCall.Arguments[0], Expression.Parameter(typeof(TEntity), "p"));
            var filterDefinitionBuilder = new FilterDefinitionBuilder<TEntity>();
            var param1Type = typeof(FieldDefinition<TEntity>);
            var param2Type = typeof(FilterDefinition<>).MakeGenericType(foreignType);

            var method = filterDefinitionBuilder.GetType()
                .GetGenericMethod(nameof(filterDefinitionBuilder.ElemMatch), BindingFlags.Public | BindingFlags.Instance,
                new[] { foreignType }, new[] { param1Type, param2Type }, typeof(FilterDefinition<TEntity>));

            var expressionFilterDefinition = Activator.CreateInstance(typeof(ExpressionFilterDefinition<>).MakeGenericType(foreignType), predicate);
            var asField = new StringFieldDefinition<TEntity>(asFieldName);

            var elmMatch = (FilterDefinition<TEntity>)method.Invoke(filterDefinitionBuilder, new object[] { asField, expressionFilterDefinition });

            //Filter ($match) elements of TForeign using predicate 
            aggregateQueryable.AggregateFluent = aggregateQueryable.AggregateFluent.Match(elmMatch);
        }

        private static void ApplyProjection<TEntity, TProperty>(AggregateQueryable<TEntity> aggregateQueryable, string asFieldName, string navigationElementName)
        {
            var isEnumerable = typeof(TProperty).IsEnumerable();
            if (!isEnumerable)
            {
                //Set first element as navigation field
                PipelineStageDefinition<TEntity, TEntity> addFields = "{ \"$addFields\": { \"" + navigationElementName + "\": { \"$arrayElemAt\": [\"$" + asFieldName + "\", 0] } } }";
                //Exclude temp @as field
                PipelineStageDefinition<TEntity, TEntity> project = "{ \"$project\" : { \"" + asFieldName + "\" : 0 } }";

                aggregateQueryable.AggregateFluent = aggregateQueryable.AggregateFluent
                    .AppendStage(addFields)
                    .AppendStage(project);
            }
        }
        #endregion

        #region JoinExpression
        //public static IMongoQueryable<TEntity> Include<TEntity, TForeign>(this IMongoQueryable<TEntity> queryable, Expression<Func<TEntity, TForeign>> navigationField)
        //{
        //    var assembly = typeof(IMongoQueryable).Assembly;

        //    var collectionExpressionType = assembly.GetType("MongoDB.Driver.Linq.Expressions.CollectionExpression");
        //    var fieldExpressionType = assembly.GetType("MongoDB.Driver.Linq.Expressions.FieldExpression");
        //    var documentExpressionType = assembly.GetType("MongoDB.Driver.Linq.Expressions.DocumentExpression");
        //    var joinExpressionType = assembly.GetType("MongoDB.Driver.Linq.Expressions.JoinExpression");

        //    var sourceNamespace = new CollectionNamespace("MyDatabase", "posts");
        //    var joinedNamespace = new CollectionNamespace("MyDatabase", "categories");

        //    var sourceSerializer = BsonSerializer.LookupSerializer<TEntity>();
        //    var joinedSerializer = BsonSerializer.LookupSerializer<TForeign>();

        //    var sourceCollectionExpression = (Expression)Activator.CreateInstance(collectionExpressionType, sourceNamespace, sourceSerializer);
        //    var joinedCollectionExpression = (Expression)Activator.CreateInstance(collectionExpressionType, joinedNamespace, joinedSerializer);

        //    var sourceDocumentExpression = (Expression)Activator.CreateInstance(documentExpressionType, sourceSerializer);
        //    var joinedDocumentExpression = (Expression)Activator.CreateInstance(documentExpressionType, joinedSerializer);

        //    var sourcePropertyInfo = typeof(TEntity).GetProperty("CategoryId", BindingFlags.Instance | BindingFlags.Public);
        //    var joinedPropertyInfo = typeof(TForeign).GetProperty("Id", BindingFlags.Instance | BindingFlags.Public);

        //    var sourcePropertyExpression = MemberExpression.Property(sourceDocumentExpression, sourcePropertyInfo);  //Expression.Property(sourceDocumentExpression, sourcePropertyInfo)
        //    var joinedPropertyExpression = MemberExpression.Property(joinedDocumentExpression, joinedPropertyInfo);  //Expression.Property(joinedDocumentExpression, joinedPropertyInfo)

        //    var fieldSerializer = new ObjectIdSerializer();// new ObjectIdSerializer(BsonType.ObjectId);

        //    var sourceKeySelectorFieldExpression = (Expression)Activator.CreateInstance(fieldExpressionType, sourceDocumentExpression, "CategoryId", fieldSerializer, sourcePropertyExpression);
        //    var joinedKeySelectorFieldExpression = (Expression)Activator.CreateInstance(fieldExpressionType, joinedDocumentExpression, "_id", fieldSerializer, joinedPropertyExpression);

        //    var queryableType = typeof(IQueryable<TEntity>);
        //    var joinExpression = (Expression)Activator.CreateInstance(joinExpressionType,
        //        queryableType,
        //        sourceCollectionExpression,
        //        joinedCollectionExpression,
        //        sourceKeySelectorFieldExpression,
        //        joinedKeySelectorFieldExpression,
        //        "category");

        //    var query = queryable.Provider.CreateQuery<TEntity>(joinExpression);
        //    return (IMongoQueryable<TEntity>)query;
        //}
        #endregion
    }
}
