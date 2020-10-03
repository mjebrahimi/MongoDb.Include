using System;

namespace MongoDb.Include.Conventions
{
    public class SingularLowerCaseCollectionNameConvention : CollectionNameConvention
    {
        public override string GetCollectionName(Type type) => type.Name.Singularize(false).ToLowerInvariant();
    }
}
