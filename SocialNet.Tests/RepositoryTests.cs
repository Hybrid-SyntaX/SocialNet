using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialNet.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNet.Tests
{
    public class RepositoryTests
    {
        private InMemoryDatabaseHelper inMemoryDatabaseHelper;
        private SqliteConnection connection;
        private DbContextOptions<TestDbContext> options;

        class TestDbContext : DbContext
        {
            public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
            {
            }
            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
                builder.Entity<FluentData.SampleEntity>();
            }
        }

        [SetUp]
        public void Setup()
        {
            // In-memory database only exists while the connection is open
            connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            options = new DbContextOptionsBuilder<TestDbContext>()
                .UseSqlite(connection)
                .Options;

            // Create the schema in the database
            using (var context = new TestDbContext(options))
            {
                context.Database.EnsureCreated();
            }
        }
        [TearDown]
        public void TearDown()
        {
            connection.Close();
        }


        [Test]
        public void Create_WithSampleEntity_StoresInDB()
        {
            var sample = default(FluentData.SampleEntity);
            using (var context = new TestDbContext(options))
            {
                //Arrange
                var repository = new Repository<FluentData.SampleEntity>(context);
                var fluentData = new FluentData(context);
                sample = fluentData.Generate<FluentData.SampleEntity>();

                //Act
                repository.Create(sample);
            }
            //Assert
            using (var context = new TestDbContext(options))
            {
                Assert.That(context.Set<FluentData.SampleEntity>().Count(), Is.EqualTo(1));
                FluentData.SampleEntity sampleEntity = context.Set<FluentData.SampleEntity>().Single();
                Assert.That(sampleEntity.Id, Is.Not.EqualTo(Guid.Empty.ToString()));
                AssertTwoSamplesAreEqual(sample, sampleEntity);
            }
        }

        [Test]
        public async Task CreateAsync_WithSampleEntity_StoresInDB()
        {
            var sample = default(FluentData.SampleEntity);
            using (var context = new TestDbContext(options))
            {
                //Arrange
                var repository = new Repository<FluentData.SampleEntity>(context);
                var fluentData = new FluentData(context);
                sample = fluentData.Generate<FluentData.SampleEntity>();

                //Act
                await repository.CreateAsync(sample);
            }
            //Assert
            using (var context = new TestDbContext(options))
            {
                Assert.That(context.Set<FluentData.SampleEntity>().Count(), Is.EqualTo(1));
                FluentData.SampleEntity sampleEntity = context.Set<FluentData.SampleEntity>().Single();
                Assert.That(sampleEntity.Id, Is.Not.EqualTo(Guid.Empty.ToString()));

                AssertTwoSamplesAreEqual(sample, sampleEntity);

            }
        }
        [Test]
        public async Task Read_WithSampleEntity_StoresInDB()
        {
            var sample = default(FluentData.SampleEntity);
            using (var context = new TestDbContext(options))
            {
                //Arrange
                var repository = new Repository<FluentData.SampleEntity>(context);
                var fluentData = new FluentData(context);
                sample = fluentData.Generate<FluentData.SampleEntity>();
                await fluentData.SaveAsync(sample);

                //Act
                var readSample = repository.Read(sample.Id);

                //Assert
                AssertTwoSamplesAreEqual(sample, readSample);
            }
        }
        [Test]
        public async Task ReadAsync_WithSampleEntity_StoresInDB()
        {
            var sample = default(FluentData.SampleEntity);
            using (var context = new TestDbContext(options))
            {
                //Arrange
                var repository = new Repository<FluentData.SampleEntity>(context);
                var fluentData = new FluentData(context);
                sample = fluentData.Generate<FluentData.SampleEntity>();
                await fluentData.SaveAsync(sample);

                //Act
                var readSample = await repository.ReadAsync(sample.Id);

                //Assert
                AssertTwoSamplesAreEqual(sample, readSample);
            }
        }

        [Test]
        public async Task ReadAll_ReturnsSequence()
        {
            var samples = default(IEnumerable<FluentData.SampleEntity>);
            using (var context = new TestDbContext(options))
            {
                //Arrange
                var repository = new Repository<FluentData.SampleEntity>(context);
                var fluentData = new FluentData(context);
                samples = fluentData.GenerateSequence<FluentData.SampleEntity>(5);
                await fluentData.SaveAsync(samples);

            }
            using (var context = new TestDbContext(options))
            {
                //Act
                var repository = new Repository<FluentData.SampleEntity>(context);
                var sequence = repository.ReadAll();

                //Assert
                Assert.That(sequence.Count(), Is.EqualTo(5));
            }
        }

        [Test]
        public  void Update_WithSampleEntity_UpdatesModelInDB()
        {
            var sample = default(FluentData.SampleEntity);
            using (var context = new TestDbContext(options))
            {
                //Arrange
                var repository = new Repository<FluentData.SampleEntity>(context);
                var fluentData = new FluentData(context);
                sample = fluentData.Generate<FluentData.SampleEntity>();
                fluentData.Save(sample);

                //Act
                sample.StringField = "NewString";
                repository.Update(sample);
            }
            //Assert
            using (var context = new TestDbContext(options))
            {
                Assert.That(context.Set<FluentData.SampleEntity>().Count(), Is.EqualTo(1));
                FluentData.SampleEntity sampleEntity = context.Set<FluentData.SampleEntity>().Single();
                Assert.That(sampleEntity.StringField, Is.EqualTo("NewString"));
                AssertTwoSamplesAreEqual(sample, sampleEntity);
            }
        }
        [Test]
        public async Task UpdateAsync_WithSampleEntity_UpdatesModelInDB()
        {
            var sample = default(FluentData.SampleEntity);
            using (var context = new TestDbContext(options))
            {
                //Arrange
                var repository = new Repository<FluentData.SampleEntity>(context);
                var fluentData = new FluentData(context);
                sample = fluentData.Generate<FluentData.SampleEntity>();
                await fluentData.SaveAsync(sample);

                //Act
                sample.StringField = "NewString";
                await repository.UpdateAsync(sample);
            }
            //Assert
            using (var context = new TestDbContext(options))
            {
                Assert.That(context.Set<FluentData.SampleEntity>().Count(), Is.EqualTo(1));
                FluentData.SampleEntity sampleEntity = context.Set<FluentData.SampleEntity>().Single();
                Assert.That(sampleEntity.StringField, Is.EqualTo("NewString"));
                AssertTwoSamplesAreEqual(sample, sampleEntity);
            }
        }
        //[Test]
        //public async Task ReadAllAsync_ReturnsSequence()
        //{
        //    var samples = default(IEnumerable<FluentData.SampleEntity>);
        //    using (var context = new TestDbContext(options))
        //    {
        //        //Arrange
        //        var repository = new Repository<FluentData.SampleEntity>(context);
        //        var fluentData = new FluentData(context);
        //        samples = fluentData.GenerateSequence<FluentData.SampleEntity>(5);
        //        await fluentData.SaveAsync(samples);

        //    }
        //    using (var context = new TestDbContext(options))
        //    {
        //        //Act
        //        var repository = new Repository<FluentData.SampleEntity>(context);
        //        var sequence = await repository.ReadAllAsync();

        //        //Assert
        //        Assert.That(sequence.Count(), Is.EqualTo(5));
        //    }
        //}

        private static void AssertTwoSamplesAreEqual(FluentData.SampleEntity sample1, FluentData.SampleEntity sample2)
        {
            Assert.That(sample2.Id, Is.Not.EqualTo(Guid.Empty.ToString()));
            Assert.That(sample2.StringField, Is.EqualTo(sample1.StringField));
            Assert.That(sample2.IntegerField, Is.EqualTo(sample1.IntegerField));
            Assert.That(sample2.DateTimeField, Is.EqualTo(sample1.DateTimeField));
        }
    }
}
