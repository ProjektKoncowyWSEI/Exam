using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL.Interface
{
   public interface IRepository<TEntity> where TEntity:class
    {
        Task<TEntity> GetAsync(int id);
        Task <IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task<bool> RemoveAsync(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

    }
}
