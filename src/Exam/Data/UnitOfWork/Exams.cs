using Exam.Services;
using ExamContract.MainDbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam = ExamContract.MainDbModels.Exam;

namespace Exam.Data.UnitOfWork
{
    public class Exams
    {
        public readonly WebApiClient<exam> ExamsRepo;
        public readonly WebApiClient<Question> QuestionsRepo;
        public readonly WebApiClient<Answer> AnswersRepo;
        public readonly WebApiClient<User> UsersRepo;
        public readonly ExamsQuestionsAnswersApiClient ExamsWithAllRepo;

        public Exams(WebApiClient<exam> exams, WebApiClient<Question> questions, WebApiClient<Answer> answers, ExamsQuestionsAnswersApiClient examsWithAll, WebApiClient<User> users)
        {
            ExamsRepo = exams;
            QuestionsRepo = questions;
            AnswersRepo = answers;
            ExamsWithAllRepo = examsWithAll;
            UsersRepo = users;
        }
        public async Task<List<exam>> GetList(string login = null, bool? onlyActive = null)
        {
            List<exam> result = new List<exam>();
            result = await ExamsRepo.GetListAsync(login, onlyActive);                  
            foreach (var e in result)
            {
                var exam = await ExamsWithAllRepo.GetAsync(e.Id);
                if (onlyActive == true)
                    e.Questions = exam.Questions.Where(x => x.Active).ToList();
                else
                    e.Questions = exam.Questions;
                
                foreach (var q in e.Questions)
                {                  
                    if (onlyActive == true)                   
                        q.Answers = q.Answers.Where(x => x.Active).ToList();                   
                    foreach (var a in q.Answers)                    
                        a.ExamId = e.Id;                    
                }
            }
            return result;
        }
        public async Task<List<User>> GetMyExams(string login, bool? onlyActive = null)
        {
            var myExams = await UsersRepo.GetListAsync(login, onlyActive);          
            myExams.ForEach(async a =>
            {
                a.Exam = await ExamsRepo.GetAsync(a.ExamId);
            });
            return myExams;
        }
        public async Task Clone(int id)
        {
            var item = await ExamsWithAllRepo.GetAsync(id);
            await ExamsWithAllRepo.AddAsync(item);                     
        }
    }
}
