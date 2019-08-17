using ExamContract.CourseModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExamCourseAPI.DAL
{
    public class TwoKeysRepository<T> where T : class, ICourseTwoKey
    {
        private readonly Context context;
        private DbSet<T> dbSet = null;
        public TwoKeysRepository(Context context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }
        public async Task<T> GetAsync(int courseId, int id)
        {
            return await dbSet.SingleOrDefaultAsync(a => a.CourseId == courseId && a.Id == id);
        }
        public async Task<List<T>> GetListAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }
        public async Task<List<T>> GetListAsync(int courseId)
        {
            return await dbSet.Where(a => a.CourseId == courseId).AsNoTracking().ToListAsync();
        }
        public async Task<List<T>> GetListAsync(int? courseId, int id)
        {
            if (courseId != null)
                return await dbSet.Where(a => a.CourseId == courseId && a.Id == id).AsNoTracking().ToListAsync();
            return await dbSet.Where(a => a.Id == id).AsNoTracking().ToListAsync();
        }
        public async Task<T> AddAsync(T item)
        {
            try
            {
                var result = await dbSet.AddAsync(item);
                return item;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task<bool> UpdateAsync(T item)
        {
            try
            {
                await Task.Run(() => dbSet.Update(item));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteAsync(T item)
        {
            var dbItem = await GetAsync(item.CourseId, item.Id);
            if (dbItem != null)
            {
                try
                {
                    await Task.Run(() => dbSet.Remove(dbItem));
                    return true;
                }
                catch (Exception)
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
            await context.SaveChangesAsync();
        }
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
