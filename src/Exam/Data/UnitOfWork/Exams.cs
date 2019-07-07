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
        private readonly QuestionWithAnswersApiClient questionWithAnswers;

        public Exams(WebApiClient<exam> exams, WebApiClient<Question> questions, WebApiClient<Answer> answers, QuestionWithAnswersApiClient questionWithAnswers)
        {
            this.exams = exams;
            this.questions = questions;
            this.answers = answers;
            this.questionWithAnswers = questionWithAnswers;
        }
        public async Task<List<exam>> GetList(string login = null)
        {
            List<exam> result = new List<exam>();
            result = await exams.GetListAsync();
            var questionList = await questions.GetListAsync();          
            foreach (var e in result)
            {                
                e.Questions = questionList.Where(x => x.ExamId == e.Id).ToList();
                foreach (var q in e.Questions)
                {
                    var qwa = await questionWithAnswers.GetAsync(q.Id);
                    q.Answers = qwa.Answers;
                }
            }
            return result;
        }
    }
}
