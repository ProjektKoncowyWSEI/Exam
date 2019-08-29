using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Data.UnitOfWork;
using Exam.Services;
using ExamContract.CourseModels;
using ExamContract.ExamDTO;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using exam = ExamContract.MainDbModels.Exam;
using examCourse = ExamContract.CourseModels.ExamCourse;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin, RoleEnum.teacher)]
    public class CoursesController : MyBaseController<ExamContract.CourseModels.Course>
    {
        private readonly WebApiClient<exam> exams;
        private readonly CourseTwoKeyApiClient<examCourse> examCourses;
        private readonly ILogger logger;
        private readonly Courses uow;

        public CoursesController(IStringLocalizer<SharedResource> localizer, WebApiClient<Course> service, WebApiClient<exam> exams, CourseTwoKeyApiClient<examCourse> examCourses, ILogger logger, Courses courses) : base(localizer, service)
        {
            this.exams = exams;
            this.examCourses = examCourses;
            this.logger = logger;
            this.uow = courses;
        }
        public override async Task<IActionResult> Index(int? parentId = null, int? questionId = null, string info = null, string warning = null, string error = null)
        {
            var currentRole = User.CurrentRoleEnum();
            var login = User.Identity.Name;
            switch (currentRole)
            {
                case RoleEnum.teacher:
                    ViewBag.Message = Localizer["Your courses"];
                    break;
                case RoleEnum.admin:
                    ViewBag.Message = Localizer["All courses"];
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
            var model = await uow.GetListDTO(login, onlyActive);
            return View(model);
        }
        public override async Task<IActionResult> Delete(int? id, int? parentId = null)
        {
            string message = Localizer["Course can not be removed, you can deactivate!"];
            logger.LogWarning(message);
            return RedirectToAction(nameof(Index), new { error = message });
        }
        public IActionResult SetActive(bool active)
        {
            Response.Cookies.Append(GlobalHelpers.ACTIVE, active.ToString(), new Microsoft.AspNetCore.Http.CookieOptions());
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddExam(int parentId)
        {
            await fillViewBags(parentId);
            return View();
        }       

        [HttpPost]
        public async Task<IActionResult> AddExam(examCourse item)
        {            
            if (ModelState.IsValid)
            {
                await examCourses.AddAsync(item);
                return RedirectToAction(nameof(Index));
            }
            await fillViewBags(item.CourseId);
            return View(item);
        }
        private async Task fillViewBags(int parentId)
        {
            ViewBag.CourseId = parentId;
            ViewBag.Exams = new SelectList(await exams.GetListAsync(onlyActive: true), "Id", "Name");
        }
    }
}