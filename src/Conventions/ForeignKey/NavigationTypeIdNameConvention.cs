using MongoDB.Bson.Serialization;
using System.Reflection;

namespace MongoDb.Include.Conventions
{
    /// <summary>
    /// Principal class name + principal primary key property name (for example : Author Writer => AuthorId)
    /// </summary>
    public class NavigationTypeIdNameConvention : ForeignKeyNameConvention
    {
        public override string GetForeignKeyName(MemberInfo navigationMember)
        {
            var type = navigationMember.GetUnderlyingType();

            var isEnumerable = type.IsEnumerable();
            if (isEnumerable)
                type = type.GetElementTypeOf();

            var principalIdName = BsonClassMap.LookupClassMap(type).IdMemberMap.MemberName;

            var typeName = type.Name;
            if (isEnumerable)
                return typeName.Singularize(false) + principalIdName.Pluralize(false);

            return typeName + principalIdName;
        }
    }
}
