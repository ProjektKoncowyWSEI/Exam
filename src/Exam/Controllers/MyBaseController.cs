using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Exam.Services;
using ExamContract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Helpers;
using Microsoft.Extensions.Logging;

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
        public async virtual Task<IActionResult> Index(int? parentId = null, int? questionId = null, string info = null, string warning = null, string error = null)
        {
            ViewBag.Error = error;
            ViewBag.Warning = warning;
            ViewBag.Info = info;
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

        public virtual IActionResult Create(int? parentId = null)
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

        public async virtual Task<IActionResult> Delete(int? id, int? parentId = null)
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
        public async virtual Task<IActionResult> DeleteConfirmed(int id, int? parentId = null)
        {
            var deleted = await Service.DeleteAsync(id);
            if (!deleted)
                return RedirectToAction(nameof(Index), new { error = Localizer["Item can not be deleted!"] });
            return RedirectToAction(nameof(Index));
        }
    }
}