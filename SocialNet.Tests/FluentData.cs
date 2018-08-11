using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNet.Data;
using SocialNet.Models;
using SocialNet.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Tests
{
    public class FluentData
    {
        private readonly DbContext _context;


        public class SampleEntity
        {
            [Key] public Guid Id { set; get; }
            public string StringField { set; get; }
            public int IntegerField { set; get; }
            public DateTime DateTimeField { set; get; }
        }
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
        public FluentData(DbContext context)
        {
            this._context = context;
        }
        public FluentData()
        {

        }
        public IEnumerable<T> GenerateSequence<T>(int count)
        {
            for (int i = 0; i < count; i++)
                yield return Generate<T>();
        }
        public async Task SaveAsync<T>(IEnumerable<T> sequence) where T : class
        {
            var memoryDb = new InMemoryDatabaseHelper();
            await _context.Set<T>().AddRangeAsync(sequence);
            await _context.SaveChangesAsync();
        }
        public T Generate<T>()
        {

            object instance = default(T);
            if (typeof(T) == typeof(Post)) instance = generatePost();
            else if (typeof(T) == typeof(ApplicationUser)) instance = generateUser();
            else if (typeof(T) == typeof(SampleEntity)) instance = generateSampleEntity();

            return (T)instance;
        }
        public async Task SaveAsync<T>(T instance) where T : class
        {
            _context.Set<T>().Add(instance);
            await _context.SaveChangesAsync();
        }
        public void Save<T>(T instance) where T : class
        {
            _context.Set<T>().Add(instance);
            _context.SaveChanges();
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
        };
        private SampleEntity generateSampleEntity() => new SampleEntity()
        {
            StringField = Randomness.RandomString(10),
            IntegerField = Randomness.SuperRandom.Next(),
            DateTimeField = Randomness.RandomDate(),
        };

    }
}
