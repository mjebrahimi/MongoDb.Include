using MongoDB.Bson.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MongoDb.Include
{
    internal static class Utilities
    {
        internal static T NotNull<T>(this T obj, string name, string message = null)
        {
            if (obj is null)
                throw new ArgumentNullException($"{name} : {typeof(T)}", message);
            return obj;
        }

        internal static MemberInfo GetMember<T, TPropType>(this Expression<Func<T, TPropType>> keySelector)
        {
            if (!(keySelector.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{keySelector}' refers to a method, not a property or field.");
            return member.Member;
        }

        internal static void EnsureMemberIsFieldOrProperty(this MemberInfo member)
        {
            if (!(member is PropertyInfo) && !(member is FieldInfo))
                throw new ArgumentException($"Expression '{member.Name}' does not refer a property or field.");
        }

        internal static Type GetUnderlyingType(this MemberInfo memberInfo)
        {
            return memberInfo.MemberType switch
            {
                MemberTypes.Event => ((EventInfo)memberInfo).EventHandlerType,
                MemberTypes.Field => ((FieldInfo)memberInfo).FieldType,
                MemberTypes.Method => ((MethodInfo)memberInfo).ReturnType,
                MemberTypes.Property => ((PropertyInfo)memberInfo).PropertyType,
                _ => throw new ArgumentException("Input MemberInfo must be of type EventInfo, FieldInfo, MethodInfo, or PropertyInfo")
            };
        }

        internal static void EnsureMemberIsCustomType(this MemberInfo memberInfo)
        {
            var type = memberInfo.GetUnderlyingType();
            var isEnumerable = type.IsEnumerable();
            if ((isEnumerable ? type.GetElementTypeOf() : type).IsCustomType() == false)
                throw new InvalidOperationException($"Member '{memberInfo.Name} is not a custom type.");
        }

        internal static string GetBsonElementName(this MemberInfo memberInfo)
        {
            var classMap = BsonClassMap.LookupClassMap(memberInfo.DeclaringType);
            return classMap.DeclaredMemberMaps.Single(p => p.MemberInfo == memberInfo).ElementName;
        }

        internal static MemberInfo GetPropertyOrField(this Type type, string name, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.GetProperty(name, bindingFlags) ?? (MemberInfo)typeInfo.GetField(name, bindingFlags);
        }

        internal static IEnumerable<MemberInfo> GetPropertyAndFields(this Type type, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.GetProperties(bindingFlags).Cast<MemberInfo>().Concat(typeInfo.GetFields(bindingFlags));
        }

        internal static bool IsInheritFrom<T>(this Type type)
        {
            return IsInheritFrom(type, typeof(T));
        }

        internal static bool IsInheritFrom(this Type type, Type parentType)
        {
            //the 'is' keyword do this too for values (new ChildClass() is ParentClass)
            return parentType.GetTypeInfo().IsAssignableFrom(type);
        }

        internal static bool IsEnumerable(this Type type)
        {
            return type != typeof(string) && type.IsInheritFrom<IEnumerable>();
        }

        internal static bool IsEnumerableOf<T>(this Type type)
        {
            return type.IsInheritFrom<IEnumerable<T>>();
        }

        internal static bool IsEnumerableOf(this Type type, Type ofType)
        {
            var enumerableType = typeof(IEnumerable<>).MakeGenericType(ofType);
            return type.IsInheritFrom(enumerableType);
        }

        internal static bool IsCustomType(this Type type)
        {
            //return type.Assembly.GetName().Name != "mscorlib";
            return type.IsCustomValueType() || type.IsCustomReferenceType();
        }

        internal static bool IsCustomValueType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsValueType && !typeInfo.IsPrimitive && type.Namespace?.StartsWith("System", StringComparison.Ordinal) == false;
        }

        internal static bool IsCustomReferenceType(this Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return !typeInfo.IsValueType && !typeInfo.IsPrimitive && type.Namespace?.StartsWith("System", StringComparison.Ordinal) == false;
        }

        internal static Type GetElementTypeOf(this Type type)
        {
            var typeInfo = type.GetTypeInfo();

            // Type is Array
            // short-circuit if you expect lots of arrays 
            if (type.IsArray)
                return type.GetElementType();

            // type is IEnumerable<T>;
            if (typeInfo.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return typeInfo.GetGenericArguments()[0];
            //TIPS: typeof(List<>).GetGenericArguments().Lenght == 1 but typeof(List<>).GenericTypeArguments == 0

            // type implements/extends IEnumerable<T>;
            return typeInfo.GetInterfaces().Where(t => t.GetTypeInfo().IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
              .Select(t => t.GenericTypeArguments[0]).FirstOrDefault();
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, BindingFlags binding, Type[] genericArgTypes, Type[] argTypes, Type returnType)
        {
            return type.GetTypeInfo().GetMethods(binding)
                .Where(p => p.Name == name && p.GetGenericArguments().Length == genericArgTypes.Length)
                .Select(p =>
                {
                    try { return p.MakeGenericMethod(genericArgTypes); }
                    catch { return null; }
                })
                .Single(p => p?.GetParameters().Select(pi => pi.ParameterType).SequenceEqual(argTypes) == true && p.ReturnType == returnType);
        }
    }
}
