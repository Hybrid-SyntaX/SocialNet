using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNet.Data;
using SocialNet.Models;
using SocialNet.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Tests
{
    public class FluentData
    {
        private readonly ApplicationDbContext _context;

        //public class FluentData
        //{
        //    public async Task<FluentData> Generate<T>(out T instance)
        //    {
        //        instance = await TestDatas.GenerateAsync<T>();
        //        return this;
        //    }
        //}
        //private static Dictionary<Type, Func<bool, Task<object>>> typesObjects = new Dictionary<Type, Func<bool, Task<object>>>()
        //{
        //    [typeof(Post)] = async (createRecord) =>
        //    {
        //        return await generatePostAsync(createRecord);
        //    },
        //    [typeof(ApplicationUser)] = async (createRecord) =>
        //    {
        //        return await generateUserAsync(createRecord);
        //    }
        //};
        public FluentData(ApplicationDbContext context)
        {
            this._context = context;
        }

        public T Generate<T>()
        {

            object instance = default(T);
            if (typeof(T) == typeof(Post)) instance = generatePost();
            else if (typeof(T) == typeof(ApplicationUser)) instance = generateUser();

            return (T)instance;
        }
        public async Task SaveAsync<T>(T instance)
        {
            if (typeof(T) == typeof(Post))
            {
                var memoryDb = new InMemoryDatabaseHelper();
                PostRepository postRepository = new PostRepository(_context);
                await postRepository.CreatePostAsync(instance as Post);
            }
            else if (typeof(T) == typeof(ApplicationUser))
            {
                _context.Users.Add(instance as ApplicationUser);
                await _context.SaveChangesAsync();
            }
        }

        private Post generatePost() => new Post()
        {
            Title = Randomness.RandomAlphaspacename(10),
            Content = Randomness.RandomAlphaspacename(30),
            Score = Randomness.SuperRandom.Next(),
        };

        private ApplicationUser generateUser() => new ApplicationUser()
        {
            UserName = Randomness.RandomAlphanumericName(10),
            Email = Randomness.Email(10),
            PasswordHash = Randomness.RandomString(50),
            //PasswordHash = "AQAAAAEAACcQAAAAEFh/coFuoWdVhkx5JuZPBAreeKiY0xJ1SzCvt60S/XZykPPgPjj+gNnF0WeM7YbL2w=="
        };


    }
}
