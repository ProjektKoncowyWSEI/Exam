﻿using ExamContract.MainDbModels;
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
        private DbSet<ExamApproache> dbSetExamApproache = null;
        private DbSet<ExamApproacheResult> dbSetExamApproacheResult = null;
        public ApproachesRepository(Context context)
        {
            this.context = context;
            dbSetExamApproache = context.Set<ExamApproache>();
            dbSetExamApproacheResult = context.Set<ExamApproacheResult>();
        }
        public async Task<ExamApproache> GetAsync(int examId, string login)
        {
            return await dbSetExamApproache.SingleOrDefaultAsync(a => a.ExamId == examId && a.Login == login);
        }
        public async Task<ExamApproacheResult> GetResultAsync(string login, int examId, int questionId, int answerId)
        {
            return await dbSetExamApproacheResult.SingleOrDefaultAsync(a => a.ExamId == examId && a.Login == login && a.QuestionId == questionId && a.AnswerId == answerId);
        }
        public async Task<List<ExamApproacheResult>> GetResultsGroupedAsync(int examId)
        {          
            var results = await Task.Run(() => dbSetExamApproacheResult
                               .Where(a => a.ExamId == examId && a.Checked)
                               .Select(a => new ExamApproacheResult
                               {
                                   Login = a.Login,
                                   ExamId = a.ExamId,
                                   Points = a.Points
                               }).ToListAsync());
            var grouped = results.GroupBy(a => new { a.ExamId, a.Login })
                .Select(a => new ExamApproacheResult
                {
                    Login = a.Key.Login,
                    ExamId = a.Key.ExamId,
                    Points = a.Sum(x => x.Points)
                }).ToList();
            return grouped;
        }
        public async Task<ExamApproacheResult> GetResultGroupedAsync(string login, int examId)
        {
            var approachExists = await dbSetExamApproacheResult.FirstOrDefaultAsync(a => a.ExamId == examId && a.Login == login);
            if (approachExists != null)
            {
                var points = await Task.Run(() => dbSetExamApproacheResult
                                .Where(a => a.ExamId == examId && a.Login == login && a.Checked)
                                .Select(a => a.Points)
                                .Sum(a => a));
                return new ExamApproacheResult
                {
                    Login = login,
                    ExamId = examId,
                    Points = points
                };
            }
            return null;     
        }
        public async Task<List<ExamApproache>> GetListAsync()
        {
            return await dbSetExamApproache.AsNoTracking().ToListAsync();
        }
        public async Task<List<ExamApproacheResult>> GetResultsListAsync()
        {
            return await dbSetExamApproacheResult.AsNoTracking().ToListAsync();
        }
        public async Task<List<ExamApproacheResult>> GetResultsListAsync(string login, int? examId)
        {
            return await dbSetExamApproacheResult.Where(a => a.Login == login && a.ExamId == examId).AsNoTracking().ToListAsync();
        }
        public async Task<List<ExamApproache>> GetListAsync(string login)
        {
            return await dbSetExamApproache.Where(a => a.Login == login).AsNoTracking().ToListAsync();
        }
        public async Task<List<ExamApproache>> GetListAsync(int examId)
        {
            return await dbSetExamApproache.Where(a => a.ExamId == examId).AsNoTracking().ToListAsync();
        }
        public async Task<DateTime> AddAsync(ExamApproache item)
        {
            try
            {
                await dbSetExamApproache.AddAsync(item);
                return item.Start;
            }
            catch (Exception ex)
            {
                return new DateTime(2000, 1, 1);
            }
        }
        public async Task<ExamApproacheResult> AddResultAsync(ExamApproacheResult item)
        {
            try
            {
                await dbSetExamApproacheResult.AddAsync(item);
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> UpdateAsync(ExamApproache item)
        {
            try
            {
                await Task.Run(() => dbSetExamApproache.Update(item));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public IQueryable<ExamApproache> FindBy(Expression<Func<ExamApproache, bool>> predicate)
        {
            return dbSetExamApproache.Where(predicate).AsQueryable();
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
