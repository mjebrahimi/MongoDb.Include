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
    public class IncludeCollectionSeed
    {
        public IncludeCollectionSeed(DatabaseFixture databaseFixture)
        {
            var blogs = databaseFixture.Database.GetCollection<IncludeCollectionTests.Blog>();
            var posts = databaseFixture.Database.GetCollection<IncludeCollectionTests.Post>();

            var blog1 = new IncludeCollectionTests.Blog
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Blog1"
            };
            var blog2 = new IncludeCollectionTests.Blog
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Blog2"
            };
            var blog3 = new IncludeCollectionTests.Blog
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Blog2"
            };

            var post1 = new IncludeCollectionTests.Post
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Post1",
            };
            var post2 = new IncludeCollectionTests.Post
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Post2",
            };
            var post3 = new IncludeCollectionTests.Post
            {
                Id = ObjectId.GenerateNewId(),
                Name = "Post3",
            };

            blog1.PostIds = new[] { post1.Id, post2.Id };
            post1.BlogIds = new[] { blog1.Id, blog2.Id };

            blogs.InsertMany(new[] { blog1, blog2, blog3 });
            posts.InsertMany(new[] { post1, post2, post3 });
        }
    }

    [Collection("Database collection")]
    public class IncludeCollectionTests : IClassFixture<IncludeCollectionSeed>
    {
        private readonly IMongoCollection<Post> postCollection;
        private readonly IMongoCollection<Blog> blogCollection;

        public IncludeCollectionTests(DatabaseFixture databaseFixture)
        {
            this.postCollection = databaseFixture.Database.GetCollection<Post>();
            this.blogCollection = databaseFixture.Database.GetCollection<Blog>();

            #region Test Syntax
            //#region Include all
            //postCollection.AsAggregateQueryable().Include(p => p.Blog);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSSS);
            //#endregion

            //#region Include all ThenInclude Post
            //postCollection.AsAggregateQueryable().Include(p => p.Blog).ThenInclude(p => p.Post);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSS).ThenInclude(p => p.Post);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSS).ThenInclude(p => p.Post);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSSS).ThenInclude(p => p.Post);
            //#endregion

            //#region Include all ThenInclude PostSS
            //postCollection.AsAggregateQueryable().Include(p => p.Blog).ThenInclude(p => p.PostSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSS).ThenInclude(p => p.PostSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSS).ThenInclude(p => p.PostSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSSS).ThenInclude(p => p.PostSS);
            //#endregion

            //#region Include all ThenInclude PostSSS
            //postCollection.AsAggregateQueryable().Include(p => p.Blog).ThenInclude(p => p.PostSSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSS).ThenInclude(p => p.PostSSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSS).ThenInclude(p => p.PostSSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSSS).ThenInclude(p => p.PostSSS);
            //#endregion

            //#region Include all ThenInclude PostSSSS
            //postCollection.AsAggregateQueryable().Include(p => p.Blog).ThenInclude(p => p.PostSSSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSS).ThenInclude(p => p.PostSSSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSS).ThenInclude(p => p.PostSSSS);
            //postCollection.AsAggregateQueryable().Include(p => p.BlogSSSS).ThenInclude(p => p.PostSSSS);
            #endregion
        }

        [Fact]
        public void IncludePosts_By_ForeignKey()
        {
            var blogs = blogCollection.AsAggregateQueryable().Include(p => p.Posts).ToList();

            Assert.Equal(3, blogs.Count);
            Assert.Equal(2, blogs[0].Posts.Count);
            Assert.Empty(blogs[1].Posts);
            Assert.Empty(blogs[2].Posts);

            Assert.Equal(new[] { "Post1", "Post2" }, blogs[0].Posts.Select(p => p.Name));
        }

        [Fact]
        public void IncludeBlogs_By_ForeignKey()
        {
            var posts = postCollection.AsAggregateQueryable().Include(p => p.Blogs).ToList();

            Assert.Equal(3, posts.Count);
            Assert.Equal(2, posts[0].Blogs.Count);
            Assert.Empty(posts[1].Blogs);
            Assert.Empty(posts[2].Blogs);

            Assert.Equal(new[] { "Blog1", "Blog2" }, posts[0].Blogs.Select(p => p.Name));
        }

        [Fact]
        public void IncludePosts_By_InverseProperty()
        {
            var blogs = blogCollection.AsAggregateQueryable().Include(p => p.PostsInverse).ToList();

            Assert.Equal(3, blogs.Count);
            Assert.Single(blogs[0].PostsInverse);
            Assert.Single(blogs[1].PostsInverse);
            Assert.Empty(blogs[2].PostsInverse);

            Assert.Equal(new[] { "Post1" }, blogs[0].PostsInverse.Select(p => p.Name));
            Assert.Equal(new[] { "Post1" }, blogs[1].PostsInverse.Select(p => p.Name));
        }

        [Fact]
        public void IncludeBlogs_By_InverseProperty()
        {
            var posts = postCollection.AsAggregateQueryable().Include(p => p.BlogsInverse).ToList();

            Assert.Equal(3, posts.Count);
            Assert.Single(posts[0].BlogsInverse);
            Assert.Single(posts[1].BlogsInverse);
            Assert.Empty(posts[2].BlogsInverse);

            Assert.Equal(new[] { "Blog1" }, posts[0].BlogsInverse.Select(p => p.Name));
            Assert.Equal(new[] { "Blog1" }, posts[1].BlogsInverse.Select(p => p.Name));
        }

        [Table("blogs2")]
        public class Blog
        {
            [BsonId]
            [BsonElement("BlogId")]
            public ObjectId Id { get; set; }

            [BsonElement("BlogName")]
            public string Name { get; set; }

            [BsonElement("PostIds_FK")]
            public ICollection<ObjectId> PostIds { get; set; }

            [ForeignKey("PostIds")]
            public ICollection<Post> Posts { get; set; }

            [InverseProperty("BlogIds")]
            public ICollection<Post> PostsInverse { get; set; }
        }

        [Table("posts2")]
        public class Post
        {
            [BsonId]
            [BsonElement("PostId")]
            public ObjectId Id { get; set; }

            [BsonElement("PostName")]
            public string Name { get; set; }

            [BsonElement("BlogIds_FK")]
            public ICollection<ObjectId> BlogIds { get; set; }

            [ForeignKey("BlogIds")]
            public ICollection<Blog> Blogs { get; set; }

            [InverseProperty("PostIds")]
            public ICollection<Blog> BlogsInverse { get; set; }
        }
    }
}
