using Exam.Services;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using ExamContract.MainDbModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin)]
    public class QuestionsController : MyBaseController<Question>
    {   
        public QuestionsController(IStringLocalizer<SharedResource> localizer, WebApiClient<Question> service) : base(localizer, service) {}


        public override async Task<IActionResult> Index(int? parentId = null, string info = null, string warning = null, string error = null)
        {
            ViewBag.ExamId = parentId;
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
            if (ModelState.IsValid)
            {
                item.Login = HttpContext.User.Identity.Name;
                await Service.AddAsync(item);
                return RedirectToAction(nameof(Index), "Exams", new { parentId = item.ExamId });
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Edit(int id, Question item)
        {
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
                    return RedirectToAction(nameof(Index), "Exams", new { parentId = item.ExamId });
                }
            }
            return View(item);
        }
        public override async Task<IActionResult> Delete(int? id, int? parentId = null)
        {
            if (id == null)
            {
                return NotFound();
            }
          
            var item = await Service.GetAsync((int)id);
            if (item == null)
            {
                return NotFound();
            }
            item.ExamId = parentId;
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> DeleteConfirmed(int id, int? parentId = null)
        {
            var deleted = await Service.DeleteAsync(id);
            if (!deleted)
                return RedirectToAction(nameof(Index), new { error = Localizer["Item can not be deleted!"] });
            return RedirectToAction(nameof(Index), "Exams", new { parentId });
        }
    }
}