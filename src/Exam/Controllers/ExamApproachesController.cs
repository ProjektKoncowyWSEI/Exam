using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Data.UnitOfWork;
using ExamContract.ExamDTO;
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
            var (message, isUserAssigned) = await uow.CheckExam(model);
            ViewBag.Warning = warning ?? message;
            ViewBag.NotAssigned = !isUserAssigned;
            ViewBag.Info = info;                               
            ViewBag.Error = error;                     
            ViewBag.NotAvailable = ViewBag.Warning != null;
            ViewBag.IsActive = await uow.IsActive(model);
            return View(model);
        }
        public async Task<IActionResult> Start(string code)
        {
            var model = await uow.GetExamByCode(code, true);
            var (message, isUserAssigned) = await uow.CheckExam(model);
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
                return RedirectToAction(nameof(Index), new { code, info = "Jesteś zapisany na egzamin" });
            return RedirectToAction(nameof(Index), new { code, error = "Nie jesteś zapisany na egzamin" });
        }
        [HttpPost]        
        public async virtual Task<JsonResult> FinishExam(ExamApproacheDTO exam)
        {
            var result = await uow.FinishExam(exam);
            if (result != null)
                return Json("Zakończono egzamin");
            return Json("Nie udało się zakończyć egzaminu");            
        }       
    }
}