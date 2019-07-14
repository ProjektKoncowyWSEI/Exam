﻿using Exam.Services;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using ExamContract.MainDbModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin, RoleEnum.teacher)]
    public class AnswersController : Controller
    {
        private readonly IStringLocalizer<SharedResource> Localizer;
        private readonly WebApiClient<Answer> Service;

        public AnswersController(IStringLocalizer<SharedResource> localizer, WebApiClient<Answer> service)
        {
            Localizer = localizer;
            Service = service;
        }

        public IActionResult Create(int? parentId, int? questionId)
        {
            ViewBag.ExamId = parentId;
            ViewBag.QuestionId = questionId;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Answer item)
        {
            ViewBag.ExamId = item.ExamId;
            ViewBag.QuestionId = item.QuestionId;
            if (ModelState.IsValid)
            {
                item.Login = HttpContext.User.Identity.Name;
                await Service.AddAsync(item);
                return RedirectToAction("Index", "Exams", new { parentId = item.ExamId, questionId = item.QuestionId});
            }
            return View(item);
        }
        public async virtual Task<IActionResult> Edit(int? id, int? parentId, int? questionId)
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
            ViewBag.ExamId = parentId;
            ViewBag.QuestionId = questionId;
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Answer item)
        {
            ViewBag.ExamId = item.ExamId;
            ViewBag.QuestionId = item.QuestionId;
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
                    return RedirectToAction("Index", "Exams", new { parentId = item.ExamId, questionId = item.QuestionId });
                }
            }
            return View(item);
        }
        public IActionResult Delete(int? id)
        {
            return RedirectToAction("Index", "Exams", new { error = Localizer["Questions can not be removed, you can deactivate!"] });
        }
    }
}