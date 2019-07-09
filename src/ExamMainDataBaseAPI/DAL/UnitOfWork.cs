using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamMainDataBaseAPI.DAL
{
    public class UnitOfWork
    {    
        public Repository<Exam> ExamRepo { get; private set; }
        public Repository<Question> QuestionRepo { get; private set; }
        public Repository<Answer> AnswersRepo { get; private set; }

        public UnitOfWork(ExamQuestionsDbContext context,Repository<Exam> examRepo, Repository<Question> questionRepo, Repository<Answer> answersRepo)
        {           
            this.ExamRepo = examRepo;
            this.QuestionRepo = questionRepo;
            this.AnswersRepo = answersRepo;
        }

        public async Task<Question> GetQuestionWithAnswer(int id)
        {
            var question = await QuestionRepo.GetAsync(id);
            question.Answers = await AnswersRepo.FindBy(a => a.QuestionId == id).ToListAsync();
            return question;
        }
        public async Task<Exam> GetExamwithQuestionsWithAnswers(int id)
        {
            var exam = await ExamRepo.GetAsync(id);
            var questions = await QuestionRepo.FindBy(a => a.ExamId == id).ToListAsync();
            foreach (var q in questions)
                q.Answers = await AnswersRepo.FindBy(a => a.QuestionId == q.Id).ToListAsync();
            return exam;
        }
        public async Task Clone(Exam item)
        {
            var newItem = new Exam
            {
                Id = 0,
                Active = true,
                DurationMinutes = item.DurationMinutes,
                Login = item.Login,
                MaxPoints = item.MaxPoints,
                MaxStart = item.MaxStart,
                MinStart = item.MinStart,
                Name = $"{item.Name}(1)",
                Questions = null,
                Users = null
            };
            var examId = await ExamRepo.AddAsync(newItem);
            foreach (var q in item.Questions)
            {
                var answers = q.Answers;
                q.ExamId = examId;
                q.Id = 0;
                q.Answers = null;
                var qId = await QuestionRepo.AddAsync(q);
                foreach (var a in answers)
                {
                    a.QuestionId = qId;
                    a.Id = 0;
                    await AnswersRepo.AddAsync(a);
                }
            }
            await ExamRepo.SaveChangesAsync();
        }
    }
}
