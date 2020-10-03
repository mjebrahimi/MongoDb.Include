using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace MongoDb.Include
{
    internal static class AssociatedMemberFinder
    {
        public static readonly ConcurrentDictionary<MemberInfo, MemberInfo> dictionary = new ConcurrentDictionary<MemberInfo, MemberInfo>();

        internal static MemberInfo GetAssociatedMember(MemberInfo memberInfo)
        {
            return dictionary.GetOrAdd(memberInfo, _ =>
            {
                memberInfo.EnsureMemberIsFieldOrProperty();
                memberInfo.EnsureMemberIsCustomType();

                var findedMember = FindByItsAttribute(memberInfo);
                if (findedMember != null)
                    return findedMember;

                var foreignKeyMember = FindForeignKeyMember(memberInfo);
                if (foreignKeyMember != null)
                    return foreignKeyMember;

                var inversePropertyMember = FindInversePropertyMember(memberInfo);
                if (inversePropertyMember != null)
                    return inversePropertyMember;

                //Find by Convention
                var conventionMember = ForeignKeyFinder.GetForeignKey(memberInfo);
                if (conventionMember != null)
                    return conventionMember;

                throw new ForeignKeyMismatchException($"No property or field refers to '{memberInfo.DeclaringType.Name}.{memberInfo.Name}' as their foreign key.");
            });
        }

        private static MemberInfo FindByItsAttribute(MemberInfo memberInfo)
        {
            var foreignKey = memberInfo.GetCustomAttribute<ForeignKeyAttribute>();
            var inverseProperty = memberInfo.GetCustomAttribute<InversePropertyAttribute>();

            if (foreignKey != null && inverseProperty != null)
                throw new ForeignKeyMismatchException($"'{memberInfo.DeclaringType.Name}.{memberInfo.Name}' has both [ForeignKey] and [InverseProperty].");

            //Find by ForeignKey attribute
            if (foreignKey != null)
            {
                var foreignKeyMember = memberInfo.DeclaringType.GetPropertyOrField(foreignKey.Name);

                if (foreignKeyMember == null)
                    throw new ForeignKeyMismatchException($"Foreign key '{memberInfo.DeclaringType.Name}.{foreignKey.Name}' does not exist.");

                return foreignKeyMember;
            }

            //Find by InverseProperty attribute
            if (inverseProperty != null)
            {
                var inversePropertyType = memberInfo.GetUnderlyingType();

                var isEnumerable = inversePropertyType.IsEnumerable();
                if (isEnumerable)
                    inversePropertyType = inversePropertyType.GetElementTypeOf();

                var inversePropertyMember = inversePropertyType.GetPropertyOrField(inverseProperty.Property);

                if (inversePropertyMember == null)
                    throw new ForeignKeyMismatchException($"Inverse property '{inversePropertyType.Name}.{inverseProperty.Property}' does not exist.");

                return inversePropertyMember;
            }

            return null; //Has no attribute
        }

        private static MemberInfo FindForeignKeyMember(MemberInfo memberInfo)
        {
            var declaringMembers = memberInfo.DeclaringType.GetPropertyAndFields();

            var relatedMembers = declaringMembers
                .Where(p => p.GetCustomAttribute<ForeignKeyAttribute>()?.Name == memberInfo.Name).ToList();

            if (relatedMembers.Count > 1)
                throw new ForeignKeyMismatchException($"More than one property or field refers to '{memberInfo.DeclaringType.Name}.{memberInfo.Name}' as their foreign key.");

            return relatedMembers.SingleOrDefault();
        }

        private static MemberInfo FindInversePropertyMember(MemberInfo memberInfo)
        {
            var propertyType = memberInfo.GetUnderlyingType();
            var propertyTypeMembers = propertyType.GetPropertyAndFields();

            var relatedMembers = propertyTypeMembers
                .Where(p => p.GetCustomAttribute<InversePropertyAttribute>()?.Property == memberInfo.Name).ToList();

            if (relatedMembers.Count > 1)
                throw new ForeignKeyMismatchException($"More than one property or field of type '{propertyType.Name}' refers to '{memberInfo.DeclaringType.Name}.{memberInfo.Name}' as their inverse property.");

            return relatedMembers.SingleOrDefault();
        }
    }
}
