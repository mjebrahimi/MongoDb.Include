using MongoDB.Bson.Serialization;
using System.Reflection;

namespace MongoDb.Include.Conventions
{
    /// <summary>
    /// Navigation property name + principal primary key property name (for example : Author Writer => WriterId)
    /// </summary>
    public class NavigationNameIdNameConvention : ForeignKeyNameConvention
    {
        public override string GetForeignKeyName(MemberInfo navigationMember)
        {
            var type = navigationMember.GetUnderlyingType();

            var isEnumerable = type.IsEnumerable();
            if (isEnumerable)
                type = type.GetElementTypeOf();

            var principalIdName = BsonClassMap.LookupClassMap(type).IdMemberMap.MemberName;

            var navigationName = navigationMember.Name;
            if (isEnumerable)
                return navigationName.Singularize(false) + principalIdName.Pluralize(false);

            return navigationName + principalIdName;
        }
    }
}
