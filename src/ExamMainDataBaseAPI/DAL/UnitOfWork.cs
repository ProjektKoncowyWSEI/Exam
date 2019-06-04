using ExamMainDataBaseAPI.DAL.Interface;
using ExamMainDataBaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ExamQuestionsDbContext context;
        public IRepository<Questions> QuestionRepo { get; private set; }
        public IRepository<Answer> AnswersRepo { get; private set; }
        public IRepository<QuestionAnswer> QuestionAnswerRepo { get; private set; }

        public UnitOfWork(ExamQuestionsDbContext context, IRepository<Questions> questionRepo,
                IRepository<Answer> answersRepo, IRepository<QuestionAnswer> questionAnswerRepo) {
            this.context = context;
            this.QuestionRepo = questionRepo;
            this.AnswersRepo = answersRepo;
            this.QuestionAnswerRepo = questionAnswerRepo;
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
