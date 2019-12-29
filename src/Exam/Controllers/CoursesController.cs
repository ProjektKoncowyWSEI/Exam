using System;
using System.Threading.Tasks;
using Exam.Data.UnitOfWork;
using Exam.Services;
using ExamContract;
using ExamContract.CourseModels;
using ExamContract.TutorialModels;
using ExamContract.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using exam = ExamContract.MainDbModels.Exam;
using examCourse = ExamContract.CourseModels.ExamCourse;
using Helpers;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin, RoleEnum.teacher)]
    public class CoursesController : Controller
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly WebApiClient<Course> service;
        private readonly WebApiClient<exam> exams;
        private readonly WebApiClient<Tutorial> tutorials;
        private readonly CourseTwoKeyApiClient<examCourse> examCourses;
        private readonly CourseTwoKeyApiClient<TutorialCourse> tutorialCourses;
        private readonly ILogger logger;
        private readonly Courses uow;

        public CoursesController(IStringLocalizer<SharedResource> localizer, WebApiClient<Course> service, WebApiClient<exam> exams, WebApiClient<Tutorial> tutorials, CourseTwoKeyApiClient<examCourse> examCourses, CourseTwoKeyApiClient<TutorialCourse> tutorialCourses, ILogger logger, Courses courses)
        {
            this.localizer = localizer;
            this.service = service;
            this.exams = exams;
            this.tutorials = tutorials;
            this.examCourses = examCourses;
            this.tutorialCourses = tutorialCourses;
            this.logger = logger;
            this.uow = courses;
        }
        public async Task<IActionResult> Index(int? courseId = null, string info = null, string warning = null, string error = null)
        {
            var currentRole = User.CurrentRoleEnum();
            var login = User.Identity.Name;
            switch (currentRole)
            {
                case RoleEnum.teacher:
                    ViewBag.Message = localizer["Your courses"];
                    break;
                case RoleEnum.admin:
                    ViewBag.Message = localizer["All courses"];
                    login = null;
                    break;
                default:
                    break;
            }
            ViewBag.Error = error;
            ViewBag.Warning = warning;
            ViewBag.Info = info;
            ViewBag.CourseId = courseId;
            bool onlyActive = Convert.ToBoolean(Request.Cookies[GlobalHelpers.ACTIVE]);
            ViewBag.OnlyActive = onlyActive;
            var model = await uow.GetListDTO(login, onlyActive);
            return View(model);
        }
        public virtual IActionResult Create(int? parentId = null)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Create(Course item)
        {
            if (ModelState.IsValid)
            {
                item.Login = HttpContext.User.Identity.Name;
                var x = await service.AddAsync(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }
        public async virtual Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await service.GetAsync((int)id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Edit(int id, Course item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                item.Login = HttpContext.User.Identity.Name;
                bool updated = await service.UpdateAsync(item);
                if (updated)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(item);
        }
        public async Task<IActionResult> Delete(int? id, int? parentId = null)
        {
            string message = localizer["Course can not be removed, you can deactivate!"];
            logger.LogWarning(message);
            return await Task.Run<ActionResult>(() =>
            {
                return RedirectToAction(nameof(Index), new { error = message });
            });
        }
        public IActionResult SetActive(bool active)
        {
            Response.Cookies.Append(GlobalHelpers.ACTIVE, active.ToString(), new Microsoft.AspNetCore.Http.CookieOptions());
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddExam(int parentId)
        {
            await fillViewBags(parentId, "Exam");
            return View();
        }
        public async Task<IActionResult> AddTutorial(int parentId)
        {
            await fillViewBags(parentId, "Tutorial");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExam(examCourse item)
        {
            if (ModelState.IsValid)
            {
                await examCourses.AddAsync(item);
                return RedirectToAction(nameof(Index), new { courseId = item.CourseId });
            }
            await fillViewBags(item.CourseId, "Exam");
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteExam(int courseId, int id)
        {
            if (await examCourses.DeleteAsync(courseId, id))
                return RedirectToAction(nameof(Index), new { courseId, info = localizer["Exam deleted"] });
            return RedirectToAction(nameof(Index), new { error = localizer["Cannot delete this exam"] });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTutorial(TutorialCourse item)
        {
            if (ModelState.IsValid)
            {
                var x = await tutorialCourses.AddAsync(item);
                return RedirectToAction(nameof(Index), new { courseId = item.CourseId });
            }
            await fillViewBags(item.CourseId, "Tutorial");
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTutorial(int courseId, int id)
        {
            if (await tutorialCourses.DeleteAsync(courseId, id))
                return RedirectToAction(nameof(Index), new { courseId, info = localizer["Tutorial deleted"] });
            return RedirectToAction(nameof(Index), new { error = localizer["Cannot delete this tutorial"] });
        }
        private async Task fillViewBags(int parentId, string type)
        {
            ViewBag.CourseId = parentId;
            if (type == "Exam")
                ViewBag.Exams = new SelectList(await exams.GetListAsync(onlyActive: true), "Id", "Name");
            if (type == "Tutorial")
                ViewBag.Tutorials = new SelectList(await tutorials.GetListAsync(onlyActive: true), "Id", "Name");
        }
    }
}