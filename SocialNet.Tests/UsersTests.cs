using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialNet.Controllers;
using SocialNet.Data;
using SocialNet.Models;
using SocialNet.Tests;
using System;
using System.Linq;

namespace Tests
{
    public class UsersTests
    {
        private InMemoryDatabaseHelper inMemoryDatabaseHelper;
        [SetUp] public void Setup() => inMemoryDatabaseHelper = new InMemoryDatabaseHelper();
        [TearDown] public void TearDown() => inMemoryDatabaseHelper.Close();


        [Test]
        public void User_CanBeSavedInDB_UserIsStoredInDB()
        {
            // Run the test against one instance of the context
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
                Assert.AreEqual("example@example.com", context.Users.Single().UserName);
                Assert.That(context.Users.Single().Id, Is.EqualTo(userId));
            }
        }


        [Test]
        public void TestsAreWorking()
        {
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                Assert.AreEqual(0, context.Users.Count());
            }
        }

        [Test]
        public void UserAccesses_Register_Returns200()
        {
            //var controller = new AccountController;
        }
    }
}