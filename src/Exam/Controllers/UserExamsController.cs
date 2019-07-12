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
            var currentRole = User.CurrentRoleEnum();
            var login = User.Identity.Name;
            switch (currentRole)
            {
                case RoleEnum.teacher:
                case RoleEnum.student:
                    ViewBag.Message = localizer["Your exams"];
                    break;
                case RoleEnum.admin:
                    ViewBag.Message = localizer["All exams"];
                    login = null;
                    break;
                default:
                    break;
            }
            ViewBag.Error = error;
            ViewBag.Warning = warning;
            ViewBag.Info = info;
            ViewBag.ExamId = parentId;
            ViewBag.QuestionId = questionId;
            bool onlyActive = Convert.ToBoolean(Request.Cookies[GlobalHelpers.ACTIVE]);
            ViewBag.OnlyActive = onlyActive;
            return View(await uow.GetMyExams(login, onlyActive));
        }
        public IActionResult SetActive(bool active)
        {
            Response.Cookies.Append(GlobalHelpers.ACTIVE, active.ToString(), new Microsoft.AspNetCore.Http.CookieOptions());
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Create(User item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    item.Login = HttpContext.User.Identity.Name;
                    if ((await uow.Users.GetListAsync(item.Login)).FirstOrDefault() != null)
                        throw new Exception(localizer["User has exam on list"]);
                        await uow.Users.AddAsync(item);
                    return RedirectToAction(nameof(Index), new { info = localizer["Added"] });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, item.Login);
                    return RedirectToAction(nameof(Index), new { info = ex.Message });
                }
            }
            return RedirectToAction(nameof(Index), new { info = localizer["Not added"] });
        }
    }
}