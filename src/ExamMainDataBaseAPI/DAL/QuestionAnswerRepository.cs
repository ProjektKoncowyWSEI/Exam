using ExamContract;
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
    public class QuestionAnswerRepository
    {
        private readonly ExamQuestionsDbContext context;
        private DbSet<QuestionAnswer> dbSet = null;

        public QuestionAnswerRepository(ExamQuestionsDbContext context)
        {
            this.context = context;
            dbSet = context.Set<QuestionAnswer>();
        }
        public async Task<QuestionAnswer> GetAsync(int questionId, int answerId)
        {
            return await dbSet.Where(a => a.QuestionId == questionId && a.AnswerId == answerId).FirstOrDefaultAsync();
        }
        public async Task<List<QuestionAnswer>> GetListAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }
        public async Task<bool> AddAsync(QuestionAnswer item)
        {
            try
            {
                await dbSet.AddAsync(item);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<bool> UpdateAsync(QuestionAnswer item)
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
        public async Task<bool> DeleteAsync(int questionId, int answerId)
        {
            var item = await GetAsync(questionId, answerId);
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
        public IQueryable<QuestionAnswer> FindBy(Expression<Func<QuestionAnswer, bool>> predicate)
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
