using MongoDB.Bson.Serialization;
using System.Reflection;

namespace MongoDb.Include.Conventions
{
    /// <summary>
    /// Navigation property name + '_' + principal primary key property name (for example : Author Writer => WriterId)
    /// </summary>
    public class NavigationName_IdNameConvention : ForeignKeyNameConvention
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
                return navigationName.Singularize(false) + '_' + principalIdName.Pluralize(false);

            return navigationName + '_' + principalIdName;
        }
    }
}
