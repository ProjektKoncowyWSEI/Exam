using Exam.Services;
using ExamContract.MainDbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using exam = ExamContract.MainDbModels.Exam;

namespace Exam.Data.UnitOfWork
{
    public sealed class Exams
    {
        public readonly WebApiClient<exam> ExamsRepo;
        public readonly WebApiClient<Question> QuestionsRepo;
        public readonly WebApiClient<Answer> AnswersRepo;
        public readonly WebApiClient<User> UsersRepo;
        private readonly IEmailSender emailSender;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly IHttpContextAccessor httpContext;
        public readonly ExamsQuestionsAnswersApiClient ExamsWithAllRepo;
        public readonly ExamApproachesApiClient ExamApproachesRepo;

        public Exams(WebApiClient<exam> exams, WebApiClient<Question> questions, WebApiClient<Answer> answers, ExamsQuestionsAnswersApiClient examsWithAll, WebApiClient<User> users, IEmailSender emailSender, IStringLocalizer<SharedResource> localizer, IHttpContextAccessor httpContext)
        {
            ExamsRepo = exams;
            QuestionsRepo = questions;
            AnswersRepo = answers;
            ExamsWithAllRepo = examsWithAll;
            UsersRepo = users;
            this.emailSender = emailSender;
            this.localizer = localizer;
            this.httpContext = httpContext;
            //ExamApproachesRepo = examApproachesRepo;
        }

        public Task<bool> StartExam(string login, string code)
        {
            throw new NotImplementedException();
        }

        public async Task<List<exam>> GetList(string login = null, bool? onlyActive = null)
        {
            List<exam> result = await ExamsRepo.GetListAsync(login, onlyActive);
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
        public async Task<bool> IsUserAssigned(string code, string login)
        {
            var exam = await GetExamByCode(code, true);
            var myExams = await GetMyExams(login, true);
            return exam != null && myExams != null && myExams.Any(a => a.ExamId == exam.Id);
        }
        public async Task<(string message, bool isUserAssigned)> CheckExam(exam exam)
        {
            var isUserAssigned = await IsUserAssigned(exam.Code, httpContext.HttpContext.User.Identity.Name);
            if (!exam.Active)
                return ($"Egzamin {exam.Code} nie jest aktywny, skontaktuj sie z wlaścicelem: {exam.Login}", isUserAssigned);            
            if (!isUserAssigned)
                return ("Nie jesteś zapisany na egzamin, możesz to zrobic klikając w przycisk.", isUserAssigned);           
            if (DateTime.Now < exam.MinStart || DateTime.Now > exam.MaxStart)
                return ($"Nie możesz teraz rozpocząć podejścia do egzaminu, możesz rozpocząć egzamin miedzy {exam.MinStart}, a {exam.MaxStart}", isUserAssigned);
            return (null, isUserAssigned);
        }
        public async Task<User> SignIntoExam(int id)
        {
            User created = null;
            User item = new User
            {
                ExamId = id,
                Login = httpContext.HttpContext.User.Identity.Name
            };
            var dbItem = (await UsersRepo.GetListAsync(item.Login)).Where(a => a.ExamId == item.ExamId).FirstOrDefault();
            if (dbItem != null)
            {
                dbItem.Active = true;
                await UsersRepo.UpdateAsync(dbItem);
            }
            else
                created = await UsersRepo.AddAsync(item);
            string examName = "";
            string message = "";
            if (created != null || dbItem != null)
            {
                var temp = await ExamsRepo.GetAsync(id);
                if (temp != null)
                {
                    string examUri = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}/StartExam/{temp.Code}";
                    examName = temp.Name;
                    message = localizer["Name {0}<br/>Code {1}<br/>When {2} - {3}<br/>Duration {4} min", temp.Name, temp.Code, temp.MinStart, temp.MaxStart, temp.DurationMinutes]
                        + $"<br/> Link: <a href='{examUri}'>{examUri}</a>";
                }
            }
            await emailSender.SendEmailAsync(item.Login, localizer["User sing up for exam {0}", examName], message);
            return created ?? dbItem;
        }
    }
}
