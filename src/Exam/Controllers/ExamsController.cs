using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Exam.Controllers
{
    [AuthorizeByRoles]
    public class ExamsController : Controller
    {
        private readonly IStringLocalizer<SharedResource> stringLocalizer;

        public ExamsController(IStringLocalizer<SharedResource> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
        }
        public IActionResult Index()
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
            return View();
        }
    }
}