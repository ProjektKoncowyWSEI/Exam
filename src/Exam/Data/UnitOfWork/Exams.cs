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
                setActive(onlyActive, e, exam);
            }
            return result;
        }

        private static void setActive(bool? onlyActive, exam output, exam input)
        {
            if (onlyActive == true)
                output.Questions = input.Questions.Where(x => x.Active).ToList();
            else
                output.Questions = input.Questions;

            foreach (var q in output.Questions)
            {
                if (onlyActive == true)
                    q.Answers = q.Answers.Where(x => x.Active).ToList();
                foreach (var a in q.Answers)
                    a.ExamId = output.Id;
            }
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
        public async Task<exam> GetExamByCode(string code, bool? onlyActive = null)
        {            
            var myExam = await ExamsWithAllRepo.GetByCodeAsync(code);
            if (onlyActive != null)           
                setActive(onlyActive, myExam, myExam);
            return myExam;
        }
    }
}
