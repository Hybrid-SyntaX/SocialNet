using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using SocialNet.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNet.Tests
{
    public class InMemoryDatabaseHelper
    {
        private DbContextOptions<ApplicationDbContext> options;
        private SqliteConnection connection;

        public DbContextOptions<ApplicationDbContext> Options => options;
        public SqliteConnection Connection => connection;

        public InMemoryDatabaseHelper()
        {
            // In-memory database only exists while the connection is open
            connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();
            
            options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(Connection)
                .Options;

            // Create the schema in the database
            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureCreated();
            }
        }

        internal void Close() => connection.Close();

        //public void Dispose()
        //{
        //    Close();
        //}

        ~InMemoryDatabaseHelper()
        {
            connection.Close();
        }
    }
}
