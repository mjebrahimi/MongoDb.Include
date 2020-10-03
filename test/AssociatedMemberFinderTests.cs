using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace MongoDb.Include.Tests
{
    public class AssociatedMemberFinderTests
    {
        [Fact]
        public void ThrowException_When_HasBoth_ForeignKey_And_InverseProperty()
        {
            foreach (var type in new[] { typeof(Post2), typeof(Post22) })
            {
                var blogProperty = type.GetProperty("Blog");
                void action() => AssociatedMemberFinder.GetAssociatedMember(blogProperty);
                Assert.Throws<ForeignKeyMismatchException>(action);
            }
        }

        [Fact]
        public void ThrowException_When_ForeignKey_DoesNotExist()
        {
            foreach (var type in new[] { typeof(Post3), typeof(Post33) })
            {
                var blogProperty = type.GetProperty("Blog");
                void action() => AssociatedMemberFinder.GetAssociatedMember(blogProperty);
                Assert.Throws<ForeignKeyMismatchException>(action);
            }
        }

        [Fact]
        public void ThrowException_When_InverseProperty_DoesNotExist()
        {
            var blogProperty = typeof(Post4).GetProperty("Blog");
            void action() => AssociatedMemberFinder.GetAssociatedMember(blogProperty);
            Assert.Throws<ForeignKeyMismatchException>(action);
        }

        [Fact]
        public void ThrowException_When_NoMember_Refers_AsTheir_ForeignKey()
        {
            var blogProperty = typeof(Post5).GetProperty("Blog");
            void action() => AssociatedMemberFinder.GetAssociatedMember(blogProperty);
            Assert.Throws<ForeignKeyMismatchException>(action);
        }

        [Fact]
        public void ThrowException_When_MoreThanOneMember_Refers_AsTheir_ForeignKey()
        {
            var blogProperty = typeof(Post6).GetProperty("Blog");
            void action() => AssociatedMemberFinder.GetAssociatedMember(blogProperty);
            Assert.Throws<ForeignKeyMismatchException>(action);
        }

        [Fact]
        public void ThrowException_When_MoreThanOneMember_Refers_AsTheir_InverseProperty()
        {
            var blogProperty = typeof(Post7).GetProperty("Blog");
            void action() => AssociatedMemberFinder.GetAssociatedMember(blogProperty);
            Assert.Throws<ForeignKeyMismatchException>(action);
        }

        #region Nested Classes
        public class Blog
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Blog5
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int[] PostIds { get; set; }
        }

        public class Blog7
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int[] PostIds { get; set; }

            [InverseProperty("Blog")]
            public Post7 Post7 { get; set; }
            [InverseProperty("Blog")]
            public Post7 Post77 { get; set; }
        }

        public class Post2
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int BlogId { get; set; }

            [ForeignKey("BlogId")]
            [InverseProperty("PostIds")]
            public Blog Blog { get; set; }
        }

        public class Post22
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int BlogId { get; set; }

            [ForeignKey("BlogId")]
            [InverseProperty("PostIds")]
            public Blog Blog { get; set; }
        }

        public class Post3
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int BlogId { get; set; }

            [ForeignKey("NotExists")]
            public Blog Blog { get; set; }
        }
        public class Post33
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int BlogId { get; set; }

            [ForeignKey("NotExists")]
            public Blog Blog { get; set; }
        }

        public class Post4
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int BlogId { get; set; }

            [InverseProperty("NotExists")]
            public Blog Blog { get; set; }
        }

        public class Post5
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int BlogIdX { get; set; }
            public Blog5 Blog { get; set; }
        }

        public class Post6
        {
            public int Id { get; set; }
            [ForeignKey("Blog")]
            public string Name { get; set; }
            [ForeignKey("Blog")]
            public int BlogId { get; set; }
            public Blog Blog { get; set; }
        }

        public class Post7
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Blog7 Blog { get; set; }
        }
        #endregion
    }
}
