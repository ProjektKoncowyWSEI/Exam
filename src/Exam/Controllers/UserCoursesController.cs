using Exam.Data.UnitOfWork;
using ExamContract.CourseModels;
using ExamContract.Auth;
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
    public class UserCoursesController : Controller
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly Courses uow;
        private readonly ILogger logger;
        private readonly IEmailSender emailSender;

        public UserCoursesController(IStringLocalizer<SharedResource> localizer, Courses uow, ILogger logger, IEmailSender emailSender)
        {
            this.localizer = localizer;
            this.uow = uow;
            this.logger = logger;
            this.emailSender = emailSender;
        }
        public async Task<IActionResult> Index(int? courseId = null, string info = null, string warning = null, string error = null)
        {
            var login = User.Identity.Name;
            ViewBag.Message = localizer["Join course"];
            ViewBag.Error = error;
            ViewBag.Warning = warning;
            ViewBag.Info = info;
            ViewBag.CourseId = courseId;           
            bool onlyActive = Convert.ToBoolean(Request.Cookies[GlobalHelpers.ACTIVE]);
            ViewBag.OnlyActive = onlyActive;            
            return View(await uow.GetUserCoursesAsync(login, onlyActive));
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
                item.CourseId = id;
                //logger.LogInformation($"item.ExamId = id;");
                item.Login = HttpContext.User.Identity.Name;
                //logger.LogInformation($"item.Login = HttpContext.User.Identity.Name;");
                User created = null;
                try
                {
                    var dbItem = (await uow.UsersRepo.GetListAsync(item.Login)).Where(a => a.CourseId == item.CourseId).FirstOrDefault();
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
                        var temp = await uow.CourseRepo.GetAsync(id);
                        if (temp != null)
                        {
                            string examUri = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/UserCourses/Course/{temp.Id}";
                            examName = temp.Name;
                            message = localizer["Name {0}, Login {1}", temp.Name, temp.Login]
                                + $"<br/> Link: <a href='{examUri}'>{examUri}</a>";
                        }
                    }
                    await emailSender.SendEmailAsync(item.Login, localizer["User sing up for course {0}", examName], message); 
                    return RedirectToAction(nameof(Index), new { info = localizer["User signed into course"] });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                    return RedirectToAction(nameof(Index), new { error = ex.Message });
                }
            }
            return RedirectToAction(nameof(Index), new { info = localizer["Course id is not valid"] });
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
                        throw new Exception(localizer["Course is not active for user"]);
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
            return RedirectToAction(nameof(Index), new { info = localizer["Course id is not valid"] });
        }
        public async Task<IActionResult> Course(int id)
        {
            return View(await uow.GetDTO(id));
        }
       
    }
}