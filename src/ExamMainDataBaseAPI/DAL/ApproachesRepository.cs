using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL
{
    public class ApproachesRepository : IDisposable 
    { 
        private readonly Context context;
        private DbSet<ExamApproache> dbSet = null;
        public ApproachesRepository(Context context)
        {
            this.context = context;
            dbSet = context.Set<ExamApproache>();
        }
        public async Task<ExamApproache> GetAsync(int examId, string login)
        {
            return await dbSet.SingleOrDefaultAsync(a=>a.ExamId == examId && a.Login == login);
        }
        public async Task<List<ExamApproache>> GetListAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }
        public async Task<List<ExamApproache>> GetListAsync(string login)
        {
            return await dbSet.Where(a=>a.Login == login).AsNoTracking().ToListAsync();
        }
        public async Task<List<ExamApproache>> GetListAsync(int examId)
        {
            return await dbSet.Where(a => a.ExamId == examId).AsNoTracking().ToListAsync();
        }
        public async Task<DateTime> AddAsync(ExamApproache item)
        {
            try
            {
                await dbSet.AddAsync(item);
                return item.Start;
            }
            catch (Exception ex)
            {
                return new DateTime(2000,1,1);
            }
        }
        public async Task<bool> UpdateAsync(ExamApproache item)
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
        public IQueryable<ExamApproache> FindBy(Expression<Func<ExamApproache, bool>> predicate)
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
