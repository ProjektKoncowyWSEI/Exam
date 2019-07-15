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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly ILogger logger;

        public HomeController(IStringLocalizer<SharedResource> localizer, ILogger logger)
        {
            this.localizer = localizer;
            this.logger = logger;
        }
        public IActionResult Index()
        {           
            return View(nameof(Index));
        }     
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
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.Message = exceptionHandlerPathFeature.Error.ToString();
            ViewBag.InnerException = exceptionHandlerPathFeature.Error.InnerException;
            ViewBag.Path = exceptionHandlerPathFeature.Path;
            //logger.LogError($"{User.Identity.Name}{Environment.NewLine}{ViewBag.Message}{Environment.NewLine}{ViewBag.InnerException}{Environment.NewLine}{ViewBag.Path}");
            logger.LogError($"{ViewBag.Message} * {ViewBag.InnerException} * {ViewBag.Path}");
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
