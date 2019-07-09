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
        private readonly ExamsQuestionsAnswersApiClient examsWithAll;

        public Exams(WebApiClient<exam> exams, WebApiClient<Question> questions, WebApiClient<Answer> answers, ExamsQuestionsAnswersApiClient examsWithAll)
        {
            this.exams = exams;
            this.questions = questions;
            this.answers = answers;
            this.examsWithAll = examsWithAll;
        }
        public async Task<List<exam>> GetList(string login = null, bool? onlyActive = null)
        {
            List<exam> result = new List<exam>();
            result = await exams.GetListAsync(onlyActive);                  
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
        public async Task Clone(int id)
        {
            var item = await examsWithAll.GetAsync(id);
            await examsWithAll.AddAsync(item);                     
        }
    }
}
