using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Xunit;

namespace MongoDb.Include.Tests
{
    [Collection("Database collection")]
    public class IncludeReferenceSeed
    {
        public IncludeReferenceSeed(DatabaseFixture databaseFixture)
        {
            var blogs = databaseFixture.Database.GetCollection<IncludeReferenceTests.Blog>();
            var posts = databaseFixture.Database.GetCollection<IncludeReferenceTests.Post>();

            var blog1 = new IncludeReferenceTests.Blog
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Blog1"
            };
            var blog2 = new IncludeReferenceTests.Blog
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Blog2"
            };
            var blog3 = new IncludeReferenceTests.Blog
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Blog3"
            };

            var post1 = new IncludeReferenceTests.Post
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Post1"
            };
            var post2 = new IncludeReferenceTests.Post
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Post2"
            };
            var post3 = new IncludeReferenceTests.Post
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Post3",
            };
            var post4 = new IncludeReferenceTests.Post
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Post4"
            };
            var post5 = new IncludeReferenceTests.Post
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Post5"
            };

            post1.BlogId = blog1.Id;
            post2.BlogId = blog1.Id;

            blog2.PostIds = new[] { post3.Id, post4.Id };

            blogs.InsertMany(new[] { blog1, blog2, blog3 });
            posts.InsertMany(new[] { post1, post2, post3, post4, post5 });
        }
    }

    [Collection("Database collection")]
    public class IncludeReferenceTests : IClassFixture<IncludeReferenceSeed>
    {
        private readonly IMongoCollection<Post> collection;

        public IncludeReferenceTests(DatabaseFixture databaseFixture)
        {
            this.collection = databaseFixture.Database.GetCollection<Post>();
        }

        [Fact]
        public void Include_By_ForeignKey()
        {
            var posts = collection.AsAggregateQueryable().Include(p => p.Blog).ToList();

            Assert.Equal(5, posts.Count);
            Assert.Equal("Blog1", posts[0].Blog.Name);
            Assert.Equal("Blog1", posts[1].Blog.Name);
            Assert.Null(posts[2].Blog);
            Assert.Null(posts[2].Blog);
            Assert.Null(posts[4].Blog);
        }

        [Fact]
        public void Include_By_InverseProperty()
        {
            var posts = collection.AsAggregateQueryable().Include(p => p.BlogInverse).ToList();

            Assert.Equal(5, posts.Count);
            Assert.Null(posts[0].BlogInverse);
            Assert.Null(posts[1].BlogInverse);
            Assert.Equal("Blog2", posts[2].BlogInverse.Name);
            Assert.Equal("Blog2", posts[3].BlogInverse.Name);
            Assert.Null(posts[4].BlogInverse);
        }

        [Table("blogs1")]
        public class Blog
        {
            [BsonId]
            [BsonElement("BlogId")]
            public ObjectId Id { get; set; }

            [BsonElement("BlogName")]
            public string Name { get; set; }

            [BsonElement("Post_List")]
            public ICollection<ObjectId> PostIds { get; set; }
        }

        [Table("posts1")]
        public class Post
        {
            [BsonId]
            [BsonElement("PostId")]
            public ObjectId Id { get; set; }

            [BsonElement("PostName")]
            public string Name { get; set; }

            [BsonElement("BlogId_FK")]
            public ObjectId BlogId { get; set; }

            [ForeignKey("BlogId")]
            public Blog Blog { get; set; }

            [InverseProperty("PostIds")]
            public Blog BlogInverse { get; set; }
        }
    }
}
