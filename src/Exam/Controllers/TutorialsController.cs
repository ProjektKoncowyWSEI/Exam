using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using ExamContract.TutorialModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Localization;
using Exam.Services;
using ExamContract.Auth;
using ExamContract;
using Helpers;

namespace Exam.Controllers
{
    public class TutorialsController : MyBaseController<Tutorial>
    {
        private readonly ILogger logger;
        public TutorialsController(IStringLocalizer<SharedResource> localizer, WebApiClient<Tutorial> service, ILogger<ExamsController> logger) : base(localizer, service)
        {
            ///this.uow = uow;
            this.logger = logger;
        }
        public override async Task<IActionResult> Index(int? parentId = null, int? questionId = null, string info = null, string warning = null, string error = null)
        {
            var currentRole = User.CurrentRoleEnum();
            var login = User.Identity.Name;
            switch (currentRole)
            {
                case RoleEnum.teacher:
                    ViewBag.Message = Localizer["Your tutorials"];
                    break;
                case RoleEnum.admin:
                    ViewBag.Message = Localizer["All tutorials"];
                    login = null;
                    break;
                default:
                    break;
            }
            ViewBag.Error = error;
            ViewBag.Warning = warning;
            ViewBag.Info = info;           
            ViewBag.TutorialId = parentId;
            bool onlyActive = Convert.ToBoolean(Request.Cookies[GlobalHelpers.ACTIVE]);
            ViewBag.OnlyActive = onlyActive;
            var model = await Service.GetListAsync(login, onlyActive);
            return View(model);
        }       
        public new async Task<IActionResult> Delete(int? id, int? parentId = null)
        {
            string message = Localizer["Tutorial can not be removed, you can deactivate!"];
            logger.LogWarning(message);
            return await Task.Run<ActionResult>(() =>
            {
                return RedirectToAction(nameof(Index), new { error = message });
            });
        }
        public IActionResult SetActive(bool active)
        {
            logger.LogInformation($"Tutorials - SetActive {active}");
            Response.Cookies.Append(GlobalHelpers.ACTIVE, active.ToString(), new Microsoft.AspNetCore.Http.CookieOptions());
            return RedirectToAction(nameof(Index));
        }
    }
}