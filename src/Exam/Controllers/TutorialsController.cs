using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Exam.TagHelpers;
using ExamContract.TutorialModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Exam.Controllers
{
    public class TutorialsController : Controller
    {
        private readonly ITutorialsRepo tutorialsRepo;
        private readonly ILogger logger;

        public TutorialsController(ITutorialsRepo tutorialsRepo,ILogger logger)
        {
            this.tutorialsRepo = tutorialsRepo;
            this.logger = logger;
        }
      
        public async Task <IActionResult> Index( int pageId = 1, int? pageSizeLocal = 2)
        {
            try
            {
                var model = await tutorialsRepo.GetList(pageId, pageSizeLocal);
                ViewBag.PageInfo = new PageInfo
                {
                    CurrentPage = pageId,
                    ItemPerPage = pageSizeLocal ?? int.MaxValue,
                    TotalItems = tutorialsRepo.TotalItems
                };
                return View(model);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, System.Reflection.MethodBase.GetCurrentMethod().ToString());
                return View(null);
            }
        }
        
        public async Task <IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorials = await tutorialsRepo.Get((int)id);

            if (tutorials == null)
            {
                return NotFound();
            }

            return View(tutorials);
        }
      
        public ActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(Tutorial tutorial)
        {
            if (tutorial.Image == null || tutorial.Image.Length == 0)
               return View(tutorial);
            
            
            if (ModelState.IsValid)
            {
                 
                await tutorialsRepo.Add(tutorial);
                return RedirectToAction(nameof(Index));
            }
            
            return View(tutorial);
        }
           
        public async Task <IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorial = await tutorialsRepo.Get((int)id);
            if (tutorial == null)
            {
                return NotFound();
            }
            
            return View(tutorial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(int id, Tutorial tutorial)
        {
            if (id != tutorial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                 
                    await tutorialsRepo.Update(tutorial);  
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(tutorial);
        }

        public async Task <IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorial = await tutorialsRepo.Get((int)id);
            if (tutorial == null)
            {
                return NotFound();
            }

            return View(tutorial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> DeleteConfirmed(int id)
        {
            await tutorialsRepo.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteRow(int id)
        {
            try
            {
                await tutorialsRepo.Delete(id);
                return Content("OK");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
                //throw;
            }
        }
    }
}