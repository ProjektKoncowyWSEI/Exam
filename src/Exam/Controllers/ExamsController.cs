﻿using Exam.Data.UnitOfWork;
using Exam.Services;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin, RoleEnum.teacher)]
    public class ExamsController : MyBaseController<ExamContract.MainDbModels.Exam>
    {
        private readonly Exams uow;
        public ExamsController(IStringLocalizer<SharedResource> localizer, WebApiClient<ExamContract.MainDbModels.Exam> service, Exams uow) : base(localizer, service)
        {
            this.uow = uow;
        }
        public override async Task<IActionResult> Index(int? parentId = null, int? questionId = null, string info = null, string warning = null, string error = null)
        {
            var currentRole = User.CurrentRoleEnum();
            switch (currentRole)
            {
                case RoleEnum.teacher:
                case RoleEnum.student:
                    ViewBag.Message = Localizer["Your exams"];
                    break;                
                case RoleEnum.admin:
                    ViewBag.Message = Localizer["All exams"];
                    break;
                default:
                    break;
            }
            ViewBag.Error = error;
            ViewBag.Warning = warning;
            ViewBag.Info = info;
            ViewBag.ExamId = parentId;
            ViewBag.QuestionId = questionId;
            return View(await uow.GetList());
        }
        public override async Task<IActionResult> Delete(int? id, int? parentId = null)
        {
            return RedirectToAction(nameof(Index), new { error = Localizer["Exams can not be removed, you can deactivate!"] });          
        }
    }
}