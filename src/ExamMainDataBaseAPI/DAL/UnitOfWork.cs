using ExamContract.MainDbModels;
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
        public IRepository<Question> QuestionRepo { get; private set; }
        public IRepository<Answer> AnswersRepo { get; private set; }
        public IRepository<QuestionAnswer> QuestionAnswerRepo { get; private set; }

        public UnitOfWork(ExamQuestionsDbContext context, IRepository<Question> questionRepo,
                IRepository<Answer> answersRepo, IRepository<QuestionAnswer> questionAnswerRepo) {
            this.context = context;
            this.QuestionRepo = questionRepo;
            this.AnswersRepo = answersRepo;
            this.QuestionAnswerRepo = questionAnswerRepo;
        }

        public async Task<Question> GetQuestionWithAnswer(int id)
        {
            var question = await QuestionRepo.GetAsync(id);
            var answersQuestion = await QuestionAnswerRepo.FindBy(q => q.QuestionId == id).ToListAsync();
            var answers = new List<Answer>();

            foreach (var item in answersQuestion)
            {
                answers.Add(await AnswersRepo.GetAsync(item.AnswerId));
            }

            question.Answers = answers;
            return question;
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
