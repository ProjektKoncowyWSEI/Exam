using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Exam.Filters;
using Helpers;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using System.Threading;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<SharedResource> localizer;

        public HomeController(IStringLocalizer<SharedResource> localizer)
        {
            this.localizer = localizer;
        }
        public IActionResult Index()
        {
            ViewBag.X = localizer["Name"];
            return View();
        }
        [AuthorizeByRoles(RoleEnum.student)]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );       
           
            return LocalRedirect(returnUrl);
        }
    }
}
