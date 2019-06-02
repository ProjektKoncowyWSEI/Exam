using ExamMainDataBaseAPI.DAL.Interface;
using ExamMainDataBaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity:class 
    {
        protected DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }
        public async Task<bool> Add(TEntity entity)
        {
            try
            {
                await context.Set<TEntity>().AddAsync(entity);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().AddRange(entities);
        }
        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate).AsQueryable();
        }
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            try
            {
                await Task.Run(() => context.Update<TEntity>(entity));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<TEntity> GetAsync(int id)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }
        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            
            if (entity != null)
            {
                try
                {
                    await Task.Run(() => context.Set<TEntity>().Remove(entity));
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            context.Set<TEntity>().RemoveRange(entities);
        }
    }
}
