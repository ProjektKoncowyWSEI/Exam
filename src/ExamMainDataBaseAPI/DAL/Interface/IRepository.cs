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
        Task <IEnumerable<TEntity>> GetAll();
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        Task<bool> RemoveAsync(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

    }
}
