using Exam.Services;
using ExamContract.ExamDTO;
using ExamContract.TutorialModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Exam.Data.UnitOfWork
{
    public sealed class Tutorials
    {
        public readonly WebApiClient<Tutorial> TutorialRepo;
        public readonly WebApiClient<User> UsersRepo;
        private readonly IEmailSender emailSender;
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly IHttpContextAccessor httpContext;
        public Tutorials(WebApiClient<Tutorial> tutorialRepo, WebApiClient<User> users, IEmailSender emailSender, IStringLocalizer<SharedResource> localizer, IHttpContextAccessor httpContext)
        {
            TutorialRepo = tutorialRepo;
            UsersRepo = users;
            this.emailSender = emailSender;
            this.localizer = localizer;
            this.httpContext = httpContext;        
        }      

        public async Task<List<Tutorial>> GetList(string login = null, bool? onlyActive = null)
        {
            List<Tutorial> result = await TutorialRepo.GetListAsync(login, onlyActive);          
            return result;
        }       
        public async Task<List<User>> GetMyTutorials(string login, bool? onlyActive = null)
        {
            var Tutorials = await UsersRepo.GetListAsync(login, onlyActive);
            Tutorials.ForEach(async a =>
            {
                a.Tutorial = await TutorialRepo.GetAsync(a.TutorialId);
            });
            return Tutorials;
        }         
        public async Task<User> SignIntoTutorial(int id)
        {
            User created = null;
            User item = new User
            {
                TutorialId = id,
                Login = httpContext.HttpContext.User.Identity.Name
            };
            var dbItem = (await UsersRepo.GetListAsync(item.Login)).Where(a => a.TutorialId == item.TutorialId).FirstOrDefault();
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
                var temp = await TutorialRepo.GetAsync(id);
                if (temp != null)
                {
                    string examUri = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}/UserTutorials/Tutorial/{temp.Id}";
                    examName = temp.Name;
                    message = localizer["Name: {0}", temp.Name]
                        + $"<br/> Link: <a href='{examUri}'>{examUri}</a>";
                }
            }
            await emailSender.SendEmailAsync(item.Login, localizer["User sing up for tutorial {0}", examName], message);
            return created ?? dbItem;
        }
        public async Task<UserTutorialsDTO> GetUserTutorialsAsync(string login, bool onlyActive)
        {
            var model = new UserTutorialsDTO();
            model.MyTutorials = await GetMyTutorials(login, onlyActive);
            Thread.Sleep(50);
            model.AllTutorials = await GetList(null, true);
            return model;
        }
    }
}
