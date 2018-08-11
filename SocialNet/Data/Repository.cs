using Microsoft.EntityFrameworkCore;
using SocialNet.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace SocialNet.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;

        public Repository(DbContext context)
        {
            this._context = context;
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public async Task CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public T Read(object id)
            => _context.Set<T>().SingleOrDefault((u) => getKeyValue(u, getKeyName()).ToString() == id.ToString());


        public async Task<T> ReadAsync(object id)
            => await _context.Set<T>().SingleAsync((u) => getKeyValue(u, getKeyName()).ToString() == id.ToString());

        public IQueryable<T> ReadAll()
            => _context.Set<T>().AsQueryable();

        //public async Task<IQueryable<T>> ReadAllAsync()
        //{

        //    return await _context.Set<T>().(e => e.Equals(null));
        //}

        public void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        // Entity Framework Core
        private string getKeyName()
            => _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();

        private object getKeyValue(T entity, string keyName)
            => entity.GetType().GetProperty(keyName).GetValue(entity, null);
    }

}
