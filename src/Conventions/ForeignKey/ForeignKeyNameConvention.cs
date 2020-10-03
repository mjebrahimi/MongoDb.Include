using System.Reflection;

namespace MongoDb.Include.Conventions
{
    public abstract class ForeignKeyNameConvention
    {
        private static ForeignKeyNameConvention defaultConvention = NavigationNameIdName;

        /// <summary>
        /// Navigation property name + principal primary key property name (for example : Author Writer => WriterId)
        /// </summary>
        public static ForeignKeyNameConvention NavigationNameIdName => new NavigationNameIdNameConvention();

        /// <summary>
        /// Principal class name + principal primary key property name (for example : Author Writer => AuthorId)
        /// </summary>
        public static ForeignKeyNameConvention NavigationTypeIdName => new NavigationTypeIdNameConvention();

        /// <summary>
        /// Navigation property name + '_' + principal primary key property name (for example : Author Writer => WriterId)
        /// </summary>
        public static ForeignKeyNameConvention NavigationName_IdName => new NavigationName_IdNameConvention();

        /// <summary>
        /// Principal class name + '_' + principal primary key property name (for example : Author Writer => AuthorId)
        /// </summary>
        public static ForeignKeyNameConvention NavigationType_IdName => new NavigationType_IdNameConvention();

        public static void SetDefaultConvention(ForeignKeyNameConvention convention) => defaultConvention = convention;
        public static ForeignKeyNameConvention GetDefaultConvention() => defaultConvention;

        public abstract string GetForeignKeyName(MemberInfo navigationMember);
    }
}
