using NUnit.Framework;
using SocialNet.Data;
using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Tests.Data
{
    class PostRepositoryTests
    {
        public void createUser()
        {
            string userId = null;
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                var user = new ApplicationUser
                {
                    UserName = "example@example.com",
                    Email = "example@example.com",
                    PasswordHash = "AQAAAAEAACcQAAAAEFh/coFuoWdVhkx5JuZPBAreeKiY0xJ1SzCvt60S/XZykPPgPjj+gNnF0WeM7YbL2w=="
                };
                context.Users.Add(user);
                context.SaveChanges();
                userId = user.Id;
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                Assert.AreEqual(1, context.Users.Count());
                Assert.AreNotEqual(2, context.Users.Count());
                Assert.AreEqual("example@example.com", context.Users.Single().UserName);
                Assert.That(context.Users.Single().Id, Is.EqualTo(userId));
            }
        }

        private InMemoryDatabaseHelper inMemoryDatabaseHelper;
        [SetUp] public void Setup() => inMemoryDatabaseHelper = new InMemoryDatabaseHelper();
        [TearDown] public void TearDown() => inMemoryDatabaseHelper.Close();


        [Test]
        public async Task CreatePostAsync_Called_StoresPostIndDB()
        {
            //Arrange
            //createUser();
            
            var fluentData = default(FluentData);
            var samplePost = default(Post);
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                fluentData = new FluentData(context);
                samplePost = fluentData.Generate<Post>();

                var user = fluentData.Generate<ApplicationUser>();
                await fluentData.SaveAsync(user);
            }

            //var samplePost =  await TestDatas.stubs[typeof(Post)]() as Post;

            //Act
            string postId = null;
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                var user = context.Users.Single();
                var postRepository = new PostRepository(context);
                var post = samplePost;

                post.OriginalPoster = user;

                await postRepository.CreatePostAsync(post);
                postId = post.Id.ToString();
            }

            //Assert
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                var post = context.Posts.Single();

                Assert.AreEqual(1, context.Posts.Count());
                Assert.AreEqual(samplePost.Title, post.Title);
                Assert.AreEqual(samplePost.Content, post.Content);
                Assert.That(post.Id.ToString(), Is.EqualTo(postId.ToString()));

                Assert.That(post.LastUpdated.Date, Is.EqualTo(DateTime.Now.Date));
                Assert.That(post.Created.Date, Is.EqualTo(DateTime.Now.Date));
                Assert.That(post.Created.Date, Is.EqualTo(post.LastUpdated.Date));
            }
        }
    }
}
