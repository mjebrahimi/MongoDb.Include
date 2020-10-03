//using MongoDB.Bson;
//using MongoDB.Bson.Serialization;
//using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Bson.Serialization.Conventions;
//using MongoDB.Bson.Serialization.Serializers;
//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace MongoDb.Include.Tests
//{
//    [Collection("Database collection")]
//    public class ThenIncludeSeed
//    {
//        public ThenIncludeSeed(DatabaseFixture databaseFixture)
//        {
//            var categories = databaseFixture.Database.GetCollection<ThenIncludeTests.Category>();
//            var subCategories = databaseFixture.Database.GetCollection<ThenIncludeTests.SubCategory>();
//            var posts = databaseFixture.Database.GetCollection<ThenIncludeTests.Post>();

//            var category1 = new ThenIncludeTests.Category
//            {
//                Id = ObjectId.GenerateNewId(),
//                Name = "Category1"
//            };
//            var category2 = new ThenIncludeTests.Category
//            {
//                Id = ObjectId.GenerateNewId(),
//                Name = "Category2"
//            };

//            var subCategory1 = new ThenIncludeTests.SubCategory
//            {
//                Id = ObjectId.GenerateNewId(),
//                Name = "SubCategory1",
//                CategoryId = category1.Id
//            };
//            var subCategory2 = new ThenIncludeTests.SubCategory
//            {
//                Id = ObjectId.GenerateNewId(),
//                Name = "SubCategory2",
//                CategoryId = category1.Id
//            };
//            var subCategory3 = new ThenIncludeTests.SubCategory
//            {
//                Id = ObjectId.GenerateNewId(),
//                Name = "SubCategory3"
//            };

//            var post1 = new ThenIncludeTests.Post
//            {
//                Id = ObjectId.GenerateNewId(),
//                Name = "Post1",
//                SubCategoryId = subCategory1.Id
//            };
//            var post2 = new ThenIncludeTests.Post
//            {
//                Id = ObjectId.GenerateNewId(),
//                Name = "Post2",
//                SubCategoryId = subCategory1.Id
//            };
//            var post3 = new ThenIncludeTests.Post
//            {
//                Id = ObjectId.GenerateNewId(),
//                Name = "Post3"
//            };

//            categories.InsertMany(new[] { category1, category2 });
//            subCategories.InsertMany(new[] { subCategory1, subCategory2, subCategory3 });
//            posts.InsertMany(new[] { post1, post2, post3 });

//        }
//    }

//    [Collection("Database collection")]
//    public class ThenIncludeTests : IClassFixture<ThenIncludeSeed>
//    {
//        private readonly IMongoCollection<Post> postCollection;
//        private readonly IMongoCollection<Category> categoryCollection;

//        public ThenIncludeTests(DatabaseFixture databaseFixture)
//        {
//            this.postCollection = databaseFixture.Database.GetCollection<Post>();
//            this.categoryCollection = databaseFixture.Database.GetCollection<Category>();
//        }

//        [Fact]
//        public void IncludeReference_ThenIncludeReference()
//        {
//            //Not Work Correctly
//            var posts = postCollection.AsAggregateQueryable().Include(p => p.SubCategory).ThenInclude(p => p.Category).ToList();

//            Assert.Equal(3, posts.Count);
//            Assert.NotNull(posts[0].SubCategory);
//            Assert.NotNull(posts[1].SubCategory);
//            Assert.Null(posts[2].SubCategory);
//            Assert.Equal("SubCategory1", posts[0].SubCategory.Name);
//            Assert.Equal("SubCategory1", posts[1].SubCategory.Name);
//            Assert.NotNull(posts[0].SubCategory.Category);
//            Assert.NotNull(posts[1].SubCategory.Category);
//            Assert.Equal("Category1", posts[0].SubCategory.Category.Name);
//            Assert.Equal("Category1", posts[1].SubCategory.Category.Name);
//        }

//        [Fact]
//        public void IncludeCollection_ThenIncludeCollection()
//        {
//            //Not Work Correctly
//            var categories = categoryCollection.AsAggregateQueryable().Include(p => p.SubCategories).ThenInclude(p => p.Posts).ToList();

//            Assert.Equal(2, categories.Count);
//            Assert.Equal(2, categories[0].SubCategories.Count);
//            Assert.Empty(categories[1].SubCategories);
//            Assert.Equal("SubCategory1", categories[0].SubCategories[0].Name);
//            Assert.Equal("SubCategory2", categories[0].SubCategories[1].Name);
//            Assert.Equal(2, categories[0].SubCategories[0].Posts.Count);
//            Assert.Empty(categories[0].SubCategories[1].Posts);
//            Assert.Equal("Post1", categories[0].SubCategories[0].Posts[0].Name);
//            Assert.Equal("Post2", categories[0].SubCategories[0].Posts[1].Name);
//        }

//        [Table("categories3")]
//        public class Category
//        {
//            [BsonId]
//            [BsonElement("CategoryId")]
//            public ObjectId Id { get; set; }

//            [BsonElement("CategoryName")]
//            public string Name { get; set; }

//            [InverseProperty("CategoryId")]
//            public IList<SubCategory> SubCategories { get; set; }
//        }

//        [Table("subcategories3")]
//        public class SubCategory
//        {
//            [BsonId]
//            [BsonElement("SubCategoryId")]
//            public ObjectId Id { get; set; }

//            [BsonElement("SubCategoryName")]
//            public string Name { get; set; }

//            [BsonElement("CategoryId_FK")]
//            public ObjectId CategoryId { get; set; }

//            public Category Category { get; set; }

//            [InverseProperty("SubCategoryId")]
//            public IList<Post> Posts { get; set; }
//        }

//        [Table("post3")]
//        public class Post
//        {
//            [BsonId]
//            [BsonElement("PostId")]
//            public ObjectId Id { get; set; }

//            [BsonElement("PostName")]
//            public string Name { get; set; }

//            [BsonElement("SubCategoryId_FK")]
//            public ObjectId SubCategoryId { get; set; }

//            public SubCategory SubCategory { get; set; }
//        }
//    }
//}
