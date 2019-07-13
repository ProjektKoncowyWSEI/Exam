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
        private readonly WebApiClient<exam> exams;
        private readonly WebApiClient<Question> questions;
        private readonly WebApiClient<Answer> answers;
        public readonly WebApiClient<User> Users;
        private readonly ExamsQuestionsAnswersApiClient examsWithAll;

        public Exams(WebApiClient<exam> exams, WebApiClient<Question> questions, WebApiClient<Answer> answers, ExamsQuestionsAnswersApiClient examsWithAll, WebApiClient<User> users)
        {
            this.exams = exams;
            this.questions = questions;
            this.answers = answers;
            this.examsWithAll = examsWithAll;
            Users = users;
        }
        public async Task<List<exam>> GetList(string login = null, bool? onlyActive = null)
        {
            List<exam> result = new List<exam>();
            result = await exams.GetListAsync(login, onlyActive);                  
            foreach (var e in result)
            {
                var exam = await examsWithAll.GetAsync(e.Id);
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
            var myExams = await Users.GetListAsync(login, onlyActive);          
            myExams.ForEach(async a =>
            {
                a.Exam = await exams.GetAsync(a.ExamId);
            });
            return myExams;
        }
        public async Task Clone(int id)
        {
            var item = await examsWithAll.GetAsync(id);
            await examsWithAll.AddAsync(item);                     
        }
    }
}
