using MongoDb.Include.Conventions;
using MongoDB.Bson.Serialization;
using System;
using System.Linq.Expressions;
using Xunit;

namespace MongoDb.Include.Tests
{
    public class AuthorMappingFixture
    {
        public AuthorMappingFixture()
        {
            BsonClassMap.RegisterClassMap<Author>(config =>
            {
                config.AutoMap();
                config.MapIdProperty(p => p.AuthorId);
            });
        }
    }

    public class ForeignKeyFinderTests : IClassFixture<AuthorMappingFixture>
    {
        [Fact]
        public void NavigationNameIdName()
        {
            ForeignKeyNameConvention.SetDefaultConvention(ForeignKeyNameConvention.NavigationNameIdName);
            GetForeignKeyName<Book1>(p => p.Writer, p => p.WriterAuthorId);
        }

        [Fact]
        public void NavigationName_IdName()
        {
            ForeignKeyNameConvention.SetDefaultConvention(ForeignKeyNameConvention.NavigationName_IdName);
            GetForeignKeyName<Book2>(p => p.Writer, p => p.Writer_AuthorId);
        }

        [Fact]
        public void NavigationTypeIdName()
        {
            ForeignKeyNameConvention.SetDefaultConvention(ForeignKeyNameConvention.NavigationTypeIdName);
            GetForeignKeyName<Book3>(p => p.Writer, p => p.AuthorAuthorId);
        }

        [Fact]
        public void NavigationType_IdName()
        {
            ForeignKeyNameConvention.SetDefaultConvention(ForeignKeyNameConvention.NavigationType_IdName);
            GetForeignKeyName<Book4>(p => p.Writer, p => p.Author_AuthorId);
        }

        [Fact]
        public void CustomForeignKeyName()
        {
            ForeignKeyNameConvention.SetDefaultConvention(new CustomForeignKeyNameConvention());
            GetForeignKeyName<Book5>(p => p.Writer, p => p.WriterId);
        }

        [Fact]
        public void ReturnNull_When_ForeignKey_NotExist()
        {
            GetForeignKeyName<Book6>(p => p.Writer, null);
        }

        private void GetForeignKeyName<TEntity>(Expression<Func<TEntity, object>> navigation, Expression<Func<TEntity, int>> expected)
        {
            var navigationMember = navigation.GetMember();
            var foreignKey = ForeignKeyFinder.GetForeignKey(navigationMember);
            var expectedMember = expected?.GetMember();
            Assert.Equal(expectedMember, foreignKey);
        }

        #region Nested Classed
        public class Book1
        {
            public int WriterAuthorId { get; set; }
            public Author Writer { get; set; }
        }

        public class Book2
        {
            public int Writer_AuthorId { get; set; }
            public Author Writer { get; set; }
        }

        public class Book3
        {
            public int AuthorAuthorId { get; set; }
            public Author Writer { get; set; }
        }

        public class Book4
        {
            public int Author_AuthorId { get; set; }
            public Author Writer { get; set; }
        }

        public class Book5
        {
            public int WriterId { get; set; }
            public Author Writer { get; set; }
        }

        public class Book6
        {
            public Author Writer { get; set; }
        }

        public class CustomForeignKeyNameConvention : ForeignKeyNameConvention
        {
            public override string GetForeignKeyName(System.Reflection.MemberInfo navigationMember) => navigationMember.Name + "Id";
        }
        #endregion
    }

    public class Author
    {
        public int AuthorId { get; set; }
    }
}
