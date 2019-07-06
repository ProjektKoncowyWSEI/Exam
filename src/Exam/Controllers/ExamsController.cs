using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Services;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using EXAM = ExamContract.MainDbModels.Exam;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin)]
    public class ExamsController : Controller
    {
        private readonly IStringLocalizer<SharedResource> stringLocalizer;
        private readonly ExamsApiClient service;       

        public ExamsController(IStringLocalizer<SharedResource> stringLocalizer, ExamsApiClient service)
        {
            this.stringLocalizer = stringLocalizer;
            this.service = service;          
        }
        public async Task<IActionResult> Index()
        {
            var currentRole = User.CurrentRoleEnum();
            switch (currentRole)
            {
                case RoleEnum.teacher:
                case RoleEnum.student:
                    ViewBag.Message = stringLocalizer["Your exams"];
                    break;                
                case RoleEnum.admin:
                    ViewBag.Message = stringLocalizer["All exams"];
                    break;
                default:
                    break;
            }
            return View(await service.GetListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await service.GetAsync((int)id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }
       
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EXAM exam)
        {
            if (ModelState.IsValid)
            {
                exam.Login = HttpContext.User.Identity.Name; 
                await service.AddAsync(exam);
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        // GET: Exams1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await service.GetAsync((int)id);
            if (exam == null)
            {
                return NotFound();
            }
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EXAM exam)
        {
            if (id != exam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                exam.Login = HttpContext.User.Identity.Name;
                bool updated = await service.UpdateAsync(exam);
                if (updated)
                {
                    return RedirectToAction(nameof(Index));
                }                
            }
            return View(exam);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await service.GetAsync((int)id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }
     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await service.DeleteAsync(id);            
            return RedirectToAction(nameof(Index));
        }
    }
}