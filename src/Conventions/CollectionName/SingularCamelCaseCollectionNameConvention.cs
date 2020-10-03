using System;

namespace MongoDb.Include.Conventions
{
    public class SingularCamelCaseCollectionNameConvention : CollectionNameConvention
    {
        public override string GetCollectionName(Type type) => type.Name.Singularize(false).Camelize();
    }
}
