using Exam.Services;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using ExamContract.MainDbModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin, RoleEnum.teacher)]
    public class QuestionsController : MyBaseController<Question>
    {
        private readonly ILogger logger;
        public QuestionsController(IStringLocalizer<SharedResource> localizer, WebApiClient<Question> service , ILogger logger) : base(localizer, service) { this.logger = logger; }
        

        public override async Task<IActionResult> Index(int? parentId = null, int? questionId = null, string info = null, string warning = null, string error = null)
        {
            ViewBag.ExamId = parentId;
            ViewBag.QuestionId = questionId;
            return await base.Index();
        }

        public override IActionResult Create(int? parentId = null)
        {
            ViewBag.ExamId = parentId;
            return base.Create();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Create(Question item)
        {
            ViewBag.ExamId = item.ExamId;           
            if (ModelState.IsValid)
            {
                item.Login = HttpContext.User.Identity.Name;
                await Service.AddAsync(item);
                logger.LogInformation($"User create question : id {item.Id} , examId : {item.ExamId} , content : {item.Content} , active ? : {item.Active}");
                return RedirectToAction(nameof(Index), "Exams", new { parentId = item.ExamId });
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Edit(int id, Question item)
        {
            ViewBag.ExamId = item.ExamId;          
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                item.Login = HttpContext.User.Identity.Name;
                bool updated = await Service.UpdateAsync(item);
                if (updated)
                {
                    logger.LogInformation($"User update question : id {item.Id} , examId : {item.ExamId} , content : {item.Content} , active ? : {item.Active}");
                    return RedirectToAction(nameof(Index), "Exams", new { parentId = item.ExamId });
                }
            }
            return View(item);
        }
        public override async Task<IActionResult> Delete(int? id, int? parentId = null)
        {
            logger.LogWarning(Localizer["Questions can not be removed, you can deactivate!"]);
            return RedirectToAction(nameof(Index), new { error = Localizer["Questions can not be removed, you can deactivate!"] });
        }     
    }
}