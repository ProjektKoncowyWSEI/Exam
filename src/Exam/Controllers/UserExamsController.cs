using Exam.Data.UnitOfWork;
using Exam.Services;
using ExamContract.MainDbModels;
using ExamContract.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Helpers;

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
            return View(await uow.GetUserExamsAsync(login, onlyActive));
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
                try
                {
                    User created = await uow.SignIntoExam(id);
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