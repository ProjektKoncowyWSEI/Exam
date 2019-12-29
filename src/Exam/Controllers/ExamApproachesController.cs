using System.Threading.Tasks;
using Exam.Data.UnitOfWork;
using ExamContract.ExamDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Exam.Controllers
{
    [AuthorizeByRoles]
    public class ExamApproachesController : Controller
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly Exams uow;

        public ExamApproachesController(IStringLocalizer<SharedResource> localizer, Exams uow)
        {
            this.localizer = localizer;
            this.uow = uow;
        }

        public async Task<IActionResult> Index(string code, string info = null, string warning = null, string error = null, bool? notAssinged = null)
        {
            var model = await uow.GetExamByCode(code, true);
            ViewBag.InfoTimeout = 10000;
            if (error != null || notAssinged != null)
            {
                ViewBag.NotAssigned = notAssinged;
                ViewBag.Error = error;
                ViewBag.NotAvailable = true;
                return View(model);
            }
            var (message, isUserAssigned, examResult) = await uow.CheckExam(model);
            model.ExamApproacheResult = examResult;
            if (model.ExamApproacheResult != null)
            {
                ViewBag.Info = message;
            }
            else
            {
                ViewBag.Warning = warning ?? message;
                ViewBag.NotAssigned = !isUserAssigned;
                ViewBag.Info = info;
                ViewBag.Error = error;
                ViewBag.NotAvailable = ViewBag.Warning != null;
                ViewBag.IsActive = await uow.IsActive(model);
            }
            return View(model);
        }
        public async Task<IActionResult> Start(string code)
        {
            var model = await uow.GetExamByCode(code, true);
            var (message, isUserAssigned, examResult) = await uow.CheckExam(model);
            if (message != null || !isUserAssigned)
                return RedirectToAction(nameof(Index), new { code, error = message, notAssigned = !isUserAssigned });
            var (start, end) = await uow.StartExam(HttpContext.User.Identity.Name, code);
            ViewBag.StartDate = start;
            ViewBag.EndDate = end.ToString("yyyy-MM-dd HH:mm");
            return View("Exam", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> SignIntoExam(string code, int id)
        {
            var exam = await uow.SignIntoExam(id);
            if (exam != null)
                return RedirectToAction(nameof(Index), new { code, info = localizer["You are sing up for exam"] });
            return RedirectToAction(nameof(Index), new { code, error = localizer["You are not sing up for exam"] });
        }
        [HttpPost]
        public async virtual Task<JsonResult> FinishExam(ExamApproacheDTO exam)
        {
            var result = await uow.FinishExam(exam);
            if (result)
                return Json(new object[] { true, localizer["The exam has ended"] });
            return Json(new object[] { false, localizer["Error while ending exam"] });
        }
    }
}