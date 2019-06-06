using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExamTutorialsAPI.DAL
{
    public class Repository<T> : IDisposable where T : Entity
    {
        private readonly ExamTutorialsApiContext examTutorialsApiContext;
        private DbSet<T> dbSet = null;

        public Repository (ExamTutorialsApiContext examTutorialsApiContext)
        {
            this.examTutorialsApiContext = examTutorialsApiContext;
            dbSet = examTutorialsApiContext.Set<T>();
        }

        public async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }
        public async Task<List<T>> GetListAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }
        public async Task<int> AddAsync(T item)
        {
            try
            {
                await dbSet.AddAsync(item);
                return item.Id;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }
        public async Task<bool> UpdateAsync(T item)
        {
            try
            {
                await Task.Run(() => dbSet.Update(item));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var item = await GetAsync(id);
            if (item != null)
            {
                try
                {
                    await Task.Run(() => dbSet.Remove(item));
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return false;
        }
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Where(predicate).AsQueryable();
        }
        public async Task SaveChangesAsync()
        {
            await examTutorialsApiContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            examTutorialsApiContext.Dispose();
        }
    }

}

