﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Exam.TagHelpers;
using ExamTutorialsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{
    public class TutorialsController : Controller
    {
        private readonly ITutorialsRepo tutorialsRepo;

        public TutorialsController(ITutorialsRepo tutorialsRepo)
        {
            this.tutorialsRepo = tutorialsRepo;
        }
        // GET: Tutorials
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
                return View(null);
            }
        }

        // GET: Tutorials/Details/5
        public async Task <IActionResult> Details(int id)
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

        // GET: Tutorials/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tutorials/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(Tutorial tutorial)
        {
            if (tutorial.ImageFile == null || tutorial.ImageFile.Length == 0)
                return View(tutorial);
            await convertToBase64Async(tutorial);
            
            if (ModelState.IsValid)
            {
                //koment bo wywala mi błąd? 
               // await tutorial.Add(tutorial);
                return RedirectToAction(nameof(Index));
            }
            
            return View(tutorial);
        }

        private static async Task convertToBase64Async(Tutorial tutorial)
        {
            var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot/images",
                                    tutorial.ImageFile.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await tutorial.ImageFile.CopyToAsync(stream);
            }
            var byteArray = await System.IO.File.ReadAllBytesAsync(path);
            tutorial.Image = Convert.ToBase64String(byteArray);
        }

        // GET: Tutorials/Edit/5
        public async Task <IActionResult> Edit(int id)
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

        // POST: Tutorials/Edit/5
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
                    await convertToBase64Async(tutorial);
                    //await tutorial.Update(tutorial); - wywala tu błąd? 
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(tutorial);
        }

        // GET: Tutorials/Delete/5
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

        // POST: Tutorials/Delete/5
        [HttpPost]
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