using System;

namespace MongoDb.Include.Conventions
{
    public class PluralCamelCaseCollectionNameConvention : CollectionNameConvention
    {
        public override string GetCollectionName(Type type) => type.Name.Pluralize().Camelize();
    }
}
