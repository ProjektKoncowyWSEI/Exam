using Exam.Services;
using ExamContract.ExamDTO;
using ExamContract.MainDbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public Exams(WebApiClient<exam> exams, WebApiClient<Question> questions, WebApiClient<Answer> answers, ExamsQuestionsAnswersApiClient examsWithAll, WebApiClient<User> users, IEmailSender emailSender, IStringLocalizer<SharedResource> localizer, IHttpContextAccessor httpContext, ExamApproachesApiClient examApproachesRepo)
        {
            ExamsRepo = exams;
            QuestionsRepo = questions;
            AnswersRepo = answers;
            ExamsWithAllRepo = examsWithAll;
            UsersRepo = users;
            this.emailSender = emailSender;
            this.localizer = localizer;
            this.httpContext = httpContext;
            ExamApproachesRepo = examApproachesRepo;
        }

        public async Task<(DateTime start, DateTime end)> StartExam(string login, string code)
        {
            var exam = await GetExamByCode(code, true);
            if (exam != null)
            {
                var savedExam = await ExamApproachesRepo.GetAsync(exam.Id, login);
                if (savedExam == null)
                {
                    ExamApproache item = new ExamApproache
                    {
                        ExamId = exam.Id,
                        Start = DateTime.Now,
                        End = DateTime.Now.AddMinutes(exam.DurationMinutes).AddSeconds(30),
                        Login = login
                    };
                    var result = await ExamApproachesRepo.AddAsync(item);
                    if (result != null)
                        return (result.Start, result.End);
                }
                else
                    return (savedExam.Start, savedExam.End);
            }
            return (new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
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
            var x = await ExamsWithAllRepo.AddAsync(item);
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
        public async Task<(string message, bool isUserAssigned, ExamApproacheResult examResult)> CheckExam(exam exam)
        {
            var isUserAssigned = await IsUserAssigned(exam.Code, httpContext.HttpContext.User.Identity.Name); 
            var examResult = await ExamApproachesRepo.GetResultAsync(exam.Id, httpContext.HttpContext.User.Identity.Name);
            if (examResult != null)
            {
                return (localizer["The exam has ended"], isUserAssigned, examResult);
            }                       
            if (!exam.Active)
                return (localizer["Exam {0} is not active, contact the owner: {1}", exam.Code, exam.Login], isUserAssigned, examResult);
            if (!isUserAssigned)
                return (localizer["You are not registered for the exam, you can do it by clicking the button."], isUserAssigned, examResult);
            if (DateTime.Now < exam.MinStart || DateTime.Now > exam.MaxStart)
                return (localizer["You can't start the exam right now, you can start the exam between {0} and {1}", exam.MinStart, exam.MaxStart], isUserAssigned, examResult);
            var savedExam = await ExamApproachesRepo.GetAsync(exam.Id, httpContext.HttpContext.User.Identity.Name);
            if (savedExam != null && savedExam.Start.AddMinutes(exam.DurationMinutes) < DateTime.Today)
                return (localizer["The exam time has passed"], isUserAssigned, examResult);
            return (null, isUserAssigned, examResult);
        }
        public async Task<bool> IsActive(exam exam)
        {
            var savedExam = await ExamApproachesRepo.GetAsync(exam.Id, httpContext.HttpContext.User.Identity.Name);
            return (savedExam != null && savedExam.Start.AddMinutes(exam.DurationMinutes) > DateTime.Today);
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
        public async Task<UserExamsDTO> GetUserExamsAsync(string login, bool onlyActive)
        {
            var model = new UserExamsDTO();
            model.MyExams = await GetMyExams(login, onlyActive);
            Thread.Sleep(50);
            model.AllExams = await GetList(null, true);
            return model;
        }
        public async Task<bool> FinishExam(ExamApproacheDTO exam)
        {
            var login = httpContext.HttpContext.User.Identity.Name;
            var model = new List<ExamApproacheResult>();
            var answers = await AnswersRepo.GetListAsync();
            int answerCount = 0;
            exam.Questions.ForEach(q => q.Answers.ForEach(a =>
            {
                answerCount++;
                model.Add(new ExamApproacheResult
                {
                    Login = login,
                    ExamId = exam.ExamId,
                    QuestionId = q.Id,
                    Points = answers.FirstOrDefault(aa => aa.Id == a.Id)?.Points ?? 0m,
                    AnswerId = a.Id,
                    Checked = a.Checked
                });
            }));
            var result = await ExamApproachesRepo.AddResultsAsync(model);
            return (result == answerCount);   
        }
        public async Task<(exam exam, List<ExamApproacheResult> examResults)> GetResultsForExam(int id)
        {
            var exam = await ExamsWithAllRepo.GetAsync(id);
            var examResults = await ExamApproachesRepo.GetResultsAsync(id);
            var examUsers = await UsersRepo.GetListAsync(id);
            if (examUsers.Count > examResults.Count)
            { 
                //Dodajemy uczestnków egzaminu którzy nie zakończyli egzaminu.
                var usersNotOnExam = examUsers
                    .Where(eu => !examResults.Any(m => m.Login == eu.Login))
                    .Select(eu => new ExamApproacheResult
                    {
                        Login = eu.Login,
                        Points = -1
                    });
                examResults.AddRange(usersNotOnExam);
            }
            return (exam, examResults);
        }
    }
}
