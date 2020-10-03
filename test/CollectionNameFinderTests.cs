using MongoDb.Include.Conventions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace MongoDb.Include.Tests
{
    public class CollectionNameFinderTests
    {
        public CollectionNameFinderTests()
        {
            var field = typeof(CollectionNameFinder).GetField("dictionary", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            dynamic dictionary = field.GetValue(null);
            dictionary.Clear();
        }

        [Fact]
        public void PluralCamelCase()
        {
            CollectionNameConvention.SetDefaultConvention(CollectionNameConvention.PluralCamelCase);
            var collectioName = CollectionNameFinder.GetCollectionName<CategoryPerson>();

            Assert.Equal("categoryPeople", collectioName);
        }

        [Fact]
        public void PluralLowerCase()
        {
            CollectionNameConvention.SetDefaultConvention(CollectionNameConvention.PluralLowerCase);
            var collectioName = CollectionNameFinder.GetCollectionName<CategoryPerson>();

            Assert.Equal("categorypeople", collectioName);
        }

        [Fact]
        public void SingularCamelCase()
        {
            CollectionNameConvention.SetDefaultConvention(CollectionNameConvention.SingularCamelCase);
            var collectioName = CollectionNameFinder.GetCollectionName<CategoryPeople>();

            Assert.Equal("categoryPerson", collectioName);
        }

        [Fact]
        public void SingularLowerCase()
        {
            CollectionNameConvention.SetDefaultConvention(CollectionNameConvention.SingularLowerCase);
            var collectioName = CollectionNameFinder.GetCollectionName<CategoryPeople>();

            Assert.Equal("categoryperson", collectioName);
        }

        [Fact]
        public void CustomCollectioName()
        {
            CollectionNameConvention.SetDefaultConvention(new CustomCollectioNameConvention());
            var collectioName = CollectionNameFinder.GetCollectionName<CategoryPeople>();

            Assert.Equal("CategoryPeople_Collection", collectioName);
        }

        [Fact]
        public void FromTableAttribute()
        {
            var collectioName = CollectionNameFinder.GetCollectionName<BlaBlaBla1>();

            Assert.Equal("CustomName1", collectioName);
        }

        #region Nested Classe
        public class CategoryPerson
        {
        }

        public class CategoryPeople
        {
        }

        [Table("CustomName1")]
        public class BlaBlaBla1
        {
        }

        public class CustomCollectioNameConvention : CollectionNameConvention
        {
            public override string GetCollectionName(Type type) => type.Name + "_Collection";
        }
        #endregion
    }
}
