using Exam.Data.UnitOfWork;
using Exam.Services;
using ExamContract.MainDbModels;
using Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin, RoleEnum.teacher, RoleEnum.student)]
    public class UserExamsController : Controller
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly Exams uow;
        private readonly ILogger logger;
        private readonly IEmailSender emailSender;

        public UserExamsController(IStringLocalizer<SharedResource> localizer, Exams uow, ILogger logger, IEmailSender emailSender)
        {
            this.localizer = localizer;
            this.uow = uow;
            this.logger = logger;
            this.emailSender = emailSender;
        }
        public async Task<IActionResult> Index(int? parentId = null, int? questionId = null, string info = null, string warning = null, string error = null)
        {
            var login = User.Identity.Name;
            ViewBag.Message = localizer["Join exam"];
            ViewBag.Error = error;
            ViewBag.Warning = warning;
            ViewBag.Info = info;
            ViewBag.ExamId = parentId;
            ViewBag.QuestionId = questionId;
            bool onlyActive = Convert.ToBoolean(Request.Cookies[GlobalHelpers.ACTIVE]);
            ViewBag.OnlyActive = onlyActive;
            var model = new ExamContract.ExamDTO.UserExamsDTO();
            model.MyExams = await uow.GetMyExams(login, onlyActive);
            model.AllExams = await uow.GetList(null, true);
            return View(model);
        }
        public IActionResult SetActive(bool active)
        {
            Response.Cookies.Append(GlobalHelpers.ACTIVE, active.ToString(), new Microsoft.AspNetCore.Http.CookieOptions());
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Create(int id)
        {            
            if (id > 0)
            {
                logger.LogInformation($"Create, {id}");
                User item = new User();
                //logger.LogInformation($"User item = new User();");
                item.ExamId = id;
                //logger.LogInformation($"item.ExamId = id;");
                item.Login = HttpContext.User.Identity.Name;
                //logger.LogInformation($"item.Login = HttpContext.User.Identity.Name;");
                User created = null;
                try
                {
                    var dbItem = (await uow.UsersRepo.GetListAsync(item.Login)).Where(a => a.ExamId == item.ExamId).FirstOrDefault();
                    if (dbItem != null)
                    {
                        dbItem.Active = true;
                        await uow.UsersRepo.UpdateAsync(dbItem);
                    }
                    else
                        created = await uow.UsersRepo.AddAsync(item);
                    string examName = "";
                    string message = "";
                    if (created != null || dbItem != null)
                    {
                        var temp = await uow.ExamsRepo.GetAsync(id);
                        if (temp != null)
                        {
                            string examUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/StartExam/{temp.Code}";
                            examName = temp.Name;
                            message = localizer["Name {0}<br/>Code {1}<br/>When {2} - {3}<br/>Duration {4} min", temp.Name, temp.Code, temp.MinStart, temp.MaxStart, temp.DurationMinutes]
                                + $"<br/> Link: <a href='{examUri}'>{examUri}</a>";
                        }
                    }
                    await emailSender.SendEmailAsync(item.Login, localizer["User sing up for exam {0}", examName], message); //TODO Dodać link do egzaminu
                    return RedirectToAction(nameof(Index), new { info = localizer["User signed into exam"] });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                    return RedirectToAction(nameof(Index), new { error = ex.Message });
                }
            }
            return RedirectToAction(nameof(Index), new { info = localizer["Exam id is not valid"] });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Edit(int id)
        {
            if (id > 0)
            {
                try
                {
                    var item = await uow.UsersRepo.GetAsync(id);
                    if (item == null)
                        throw new Exception(localizer["Exam is not active for user"]);
                    item.Active = false;
                    await uow.UsersRepo.UpdateAsync(item);
                    return RedirectToAction(nameof(Index), new { info = localizer["Deactivated"] });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                    return RedirectToAction(nameof(Index), new { error = ex.Message });
                }
            }
            return RedirectToAction(nameof(Index), new { info = localizer["Exam id is not valid"] });
        }
    }
}