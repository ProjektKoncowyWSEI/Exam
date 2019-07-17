using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Exam.Controllers
{
    public class ExamApproachesController : Controller
    {
        public IActionResult Index(string code)
        {
            ViewBag.Message = code;
            return View();
        }
    }
}