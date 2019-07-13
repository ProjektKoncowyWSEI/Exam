using Exam.Data.UnitOfWork;
using Exam.Services;
using ExamContract.MainDbModels;
using Helpers;
using Microsoft.AspNetCore.Authorization;
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

        public UserExamsController(IStringLocalizer<SharedResource> localizer, Exams uow, ILogger logger)
        {
            this.localizer = localizer;
            this.uow = uow;
            this.logger = logger;
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
                User item = new User
                {
                    ExamId = id,
                    Login = HttpContext.User.Identity.Name
                };
                try
                {
                    var dbItem = (await uow.Users.GetListAsync(item.Login)).Where(a => a.ExamId == item.ExamId).FirstOrDefault();
                    if (dbItem != null)
                    {
                        dbItem.Active = true;
                        await uow.Users.UpdateAsync(dbItem);
                    }
                    else
                        await uow.Users.AddAsync(item);
                    return RedirectToAction(nameof(Index), new { info = localizer["User signed into exam"] });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, item.Login);
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
                    var item = await uow.Users.GetAsync(id);
                    if (item == null)
                        throw new Exception(localizer["Exam is not active for user"]);
                    item.Active = false;
                    await uow.Users.UpdateAsync(item);
                    return RedirectToAction(nameof(Index), new { info = localizer["Deactivated"] });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "");
                    return RedirectToAction(nameof(Index), new { error = ex.Message });
                }
            }
            return RedirectToAction(nameof(Index), new { info = localizer["Exam id is not valid"] });
        }
    }
}