using Exam.Data.UnitOfWork;
using Exam.Services;
using ExamContract.TutorialModels;
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
    public class UserTutorialsController : Controller
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly Tutorials uow;
        private readonly ILogger logger;
        private readonly IEmailSender emailSender;

        public UserTutorialsController(IStringLocalizer<SharedResource> localizer, Tutorials uow, ILogger logger, IEmailSender emailSender)
        {
            this.localizer = localizer;
            this.uow = uow;
            this.logger = logger;
            this.emailSender = emailSender;
        }
        public async Task<IActionResult> Index(int? parentId = null, int? questionId = null, string info = null, string warning = null, string error = null)
        {
            var login = User.Identity.Name;
            ViewBag.Message = localizer["Join tutorial"];
            ViewBag.Error = error;
            ViewBag.Warning = warning;
            ViewBag.Info = info;
            ViewBag.TutorialId = parentId;           
            bool onlyActive = Convert.ToBoolean(Request.Cookies[GlobalHelpers.ACTIVE]);
            ViewBag.OnlyActive = onlyActive;
            //var model = new ExamContract.ExamDTO.UserTutorialsDTO
            //{
            //    MyTutorials = await uow.GetMyTutorials(login, onlyActive),
            //    AllTutorials = await uow.GetList(null, true)
            //};
            return View(await uow.GetUserTutorialsAsync(login, onlyActive));
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
                    User created = await uow.SignIntoTutorial(id);
                    return RedirectToAction(nameof(Index), new { info = localizer["User signed into tutorial"] });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                    return RedirectToAction(nameof(Index), new { error = ex.Message });
                }
            }
            return RedirectToAction(nameof(Index), new { info = localizer["Tutorial id is not valid"] });
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
                        throw new Exception(localizer["Tutorial is not active for user"]);
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
            return RedirectToAction(nameof(Index), new { info = localizer["Tutorial id is not valid"] });
        }
        public async Task<IActionResult> Tutorial(int id)
        {
            return View(await uow.TutorialRepo.GetAsync(id));
        }
    }
}