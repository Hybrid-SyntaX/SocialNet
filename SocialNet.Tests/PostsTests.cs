﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SocialNet.Controllers;
using SocialNet.Data;
using SocialNet.Data.Interfaces;
using SocialNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Tests
{
    public class PostsTests
    {
        private InMemoryDatabaseHelper inMemoryDatabaseHelper;
        [SetUp] public void Setup() => inMemoryDatabaseHelper = new InMemoryDatabaseHelper();
        [TearDown] public void TearDown() => inMemoryDatabaseHelper.Close();


        [Test]
        public async Task Index_ReturnsAViewResult_WithAList()
        {
            //Arrange
            createUser();
            for (int i = 0; i < 2; i++)
                using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
                {
                    var user = context.Users.Single();
                    var postRepository = new PostRepository(context);
                    var post = new Post
                    {
                        Title = "post",
                        Content = "post",
                        OriginalPoster = user
                    };
                    await postRepository.CreatePostAsync(post);
                }


            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                // Act
                var postsController = new PostsController(context,
                    new Mock<IUserRepository>().Object,
                    //new PostRepository(context)
                    new Repository<Post>(context)
                    );
                var result = await postsController.Index();

                // Assert
                //IsTypeOf
                Assert.IsInstanceOf<ViewResult>(result);
                var viewResult = result as ViewResult;

                //IsAssignableFrom
                Assert.IsInstanceOf<IList<Post>>(viewResult.ViewData.Model);
                var model = viewResult.ViewData.Model;
                Assert.That(model, Is.InstanceOf<IList<Post>>());
                var posts = model as IList<Post>;

                Assert.AreEqual(2, posts.Count());
            }



        }


        [Test]
        public async Task PostController_Details_WithId_ReturnsPost()
        {
            ApplicationUser user = null;
            Post post = null;
            //Arrange
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                FluentData fluentData = new FluentData(context);

                //create user
                user = fluentData.Generate<ApplicationUser>();
                await fluentData.SaveAsync(user);
                post = fluentData.Generate<Post>();

                //create post
                post.OriginalPoster = user;
                await fluentData.SaveAsync(post);
            }

            //Act
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                //Create UserRepo mock
                var userRepoMock = new Mock<IUserRepository>();
                userRepoMock.Setup(repo => repo.GetUserAsync(ClaimsPrincipal.Current))
                    .Returns(Task.FromResult(user));

                var postsController = new PostsController(context,
                    userRepoMock.Object,
                    new Repository<Post>(context));

                var result = await postsController.Details(post.Id);

                //Assert
                Assert.IsInstanceOf<ViewResult>(result);
                var viewResult = result as ViewResult;

                //IsAssignableFrom
                Assert.IsInstanceOf<Post>(viewResult.ViewData.Model);
                var model = viewResult.ViewData.Model;
                Assert.That(model, Is.InstanceOf<Post>());
                var readPost = model as Post;

                Assert.AreEqual(readPost.Id, post.Id);
                Assert.That(readPost.Title, Is.EqualTo(post.Title));
            }

        }

        [Test]
        public async Task PostsController_CreatesPost()
        {
            // Arrange
            createUser();

            // Act
            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                // Act
                var user = context.Users.Single();
                var userRepoMock = new Mock<IUserRepository>();

                userRepoMock.Setup(repo => repo.GetUserAsync(ClaimsPrincipal.Current))
                    .Returns(Task.FromResult(user));

                var postsController = new PostsController(context,
                    userRepoMock.Object,
                    new Repository<Post>(context)
                    );

                Assert.That((await userRepoMock.Object.GetUserAsync(ClaimsPrincipal.Current)).Id, Is.EqualTo(user.Id));
                var result = await postsController.Create(new Post()
                {
                    Title = "title",
                    Content = "content",
                    OriginalPoster = user
                });


                // Assert
                Assert.IsInstanceOf<RedirectToActionResult>(result);
                var redirectToActionResult = result as RedirectToActionResult;

                Assert.Null(redirectToActionResult.ControllerName);

                Assert.AreEqual("Index", redirectToActionResult.ActionName);

            }

            using (var context = new ApplicationDbContext(inMemoryDatabaseHelper.Options))
            {
                Assert.That(context.Posts.Count(), Is.EqualTo(1));

            }

        }

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
    }
}
