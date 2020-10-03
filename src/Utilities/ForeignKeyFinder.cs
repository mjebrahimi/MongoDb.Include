using MongoDb.Include.Conventions;
using System.Collections.Concurrent;
using System.Reflection;

namespace MongoDb.Include
{
    internal static class ForeignKeyFinder
    {
        private static readonly ConcurrentDictionary<MemberInfo, MemberInfo> dictionary = new ConcurrentDictionary<MemberInfo, MemberInfo>();

        internal static MemberInfo GetForeignKey(MemberInfo memberInfo)
        {
            return dictionary.GetOrAdd(memberInfo, _ =>
            {
                memberInfo.EnsureMemberIsFieldOrProperty();
                memberInfo.EnsureMemberIsCustomType();

                var foreignKeyName = ForeignKeyNameConvention.GetDefaultConvention().GetForeignKeyName(memberInfo);
                return foreignKeyName == null ? null : memberInfo.DeclaringType.GetPropertyOrField(foreignKeyName);
            });
        }
    }
}
