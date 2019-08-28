using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    [AuthorizeByRoles]
    public class ExamApproachesController : Controller
    {
        private readonly Exams uow;

        public ExamApproachesController(Exams uow)
        {
            this.uow = uow;
        }
      
        public async Task<IActionResult> Index(string code)
        {
            var model = await uow.GetExamByCode(code, true);
            return View(model);
        }
        public async Task<IActionResult> Start(string code)
        {
            uow.StartExam(HttpContext.User.Identity.Name, code);
            return RedirectToAction(nameof(Index), new { code });
        }
    }
}