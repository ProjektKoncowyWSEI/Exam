using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Services;
using ExamContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Helpers;

namespace Exam.Controllers
{
    public abstract class MyBaseController<T> : Controller where T : Entity
    {
        protected readonly WebApiClient<T> Service;
        protected readonly IStringLocalizer<SharedResource> Localizer;

        public MyBaseController(IStringLocalizer<SharedResource> localizer, WebApiClient<T> service)
        {
            Localizer = localizer;
            Service = service;
        }
        public async virtual Task<IActionResult> Index()
        {            
            return View(await Service.GetListAsync());
        }
        public async virtual Task<IActionResult> Details(int? id)
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

            return View(item);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Create(T item)
        {
            if (ModelState.IsValid)
            {
                item.Login = HttpContext.User.Identity.Name;
                await Service.AddAsync(item);
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

            var item = await Service.GetAsync((int)id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> Edit(int id, T item)
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
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(item);
        }

        public async virtual Task<IActionResult> Delete(int? id)
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

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async virtual Task<IActionResult> DeleteConfirmed(int id)
        {
            await Service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}