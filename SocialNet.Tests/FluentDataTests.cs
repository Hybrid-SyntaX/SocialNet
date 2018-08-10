using NUnit.Framework;
using SocialNet.Data;
using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Tests
{
    public class FluentDataTests
    {
        private InMemoryDatabaseHelper inMemoryDatabaseHelper;
        [SetUp] public void Setup() => inMemoryDatabaseHelper = new InMemoryDatabaseHelper();
        [TearDown] public void TearDown() => inMemoryDatabaseHelper.Close();

        [Test]
        public void Generate_WithPost_ReturnsPost()
        {
            //Arrange
            var fluentData = default(FluentData);
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                fluentData = new FluentData(context);
            }
            //Act
            var post = fluentData.Generate<Post>();

            //Assert
            Assert.That(post, Is.Not.Null);
            Assert.That(post.Title, Is.Not.Null);
            Assert.That(post.Content, Is.Not.Null);
        }

        [Test]
        public void Generate_WithApplicationUser_ReturnsApplicatoinUser()
        {
            //Arrange
            var fluentData = default(FluentData);
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                fluentData = new FluentData(context);
            }
            //Act
            var user = fluentData.Generate<ApplicationUser>();

            //Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user.UserName, Is.Not.Null);
            Assert.That(user.Email, Is.Not.Null);
            Assert.That(user.PasswordHash, Is.Not.Null);
        }

        [Test]
        public async Task Savesync_WithApplicationUser_StoresApplicationInDB()
        {

            //Arrange

            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                var fluentData = new FluentData(context);
                var user = fluentData.Generate<ApplicationUser>();
                //Act
                await fluentData.SaveAsync(user);
            }


            //Assert
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                Assert.That(context.Users.Count(), Is.EqualTo(1));
                var user = context.Users.Single();
                Assert.That(user.Id.ToString(), Is.Not.EqualTo(Guid.Empty.ToString()));
                Assert.That(user.UserName, Is.Not.Null);
                Assert.That(user.Email, Is.Not.Null);
                Assert.That(user.PasswordHash, Is.Not.Null);

            }
        }


        [Test]
        public async Task SaveAsync_WithPost_StoresPostInDB()
        {

            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                //Arrange
                var fluentData = new FluentData(context);
                var user = fluentData.Generate<ApplicationUser>();
                await fluentData.SaveAsync(user);


                //Act
                var post = fluentData.Generate<Post>();
                post.OriginalPoster = context.Users.Single();
                await fluentData.SaveAsync(post);
            }


            //Assert

            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                Assert.That(context.Posts.Count(), Is.EqualTo(1));

            }
        }
    }
}
