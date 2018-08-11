using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNet.Data.Interfaces
{
    public interface IRepository<TEntity>
    {
  
        void Create(TEntity entity);
        Task CreateAsync(TEntity entity);

        TEntity Read(object id);
        Task<TEntity> ReadAsync(object id);

        IQueryable<TEntity> ReadAll();
        //Task<IQueryable<TEntity>> ReadAllAsync();

        void Update(TEntity entity);
        Task UpdateAsync(TEntity entity);

        void Delete(TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
