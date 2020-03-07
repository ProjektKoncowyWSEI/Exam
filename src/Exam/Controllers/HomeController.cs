using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Exam.Data.UnitOfWork;
using System.Threading.Tasks;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStringLocalizer<SharedResource> localizer;
        private readonly ILogger logger;
        private readonly ApiStrarter apiStrarter;

        public HomeController(IStringLocalizer<SharedResource> localizer, ILogger logger, ApiStrarter apiStrarter)
        {
            this.localizer = localizer;
            this.logger = logger;
            this.apiStrarter = apiStrarter;
        }
        public async Task<IActionResult> Index()
        {
            await apiStrarter.WakeUp();
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
            ViewBag.Message = exceptionHandlerPathFeature.Error.Message.ToString();
            //ViewBag.InnerException = exceptionHandlerPathFeature.Error.InnerException;
            ViewBag.Path = exceptionHandlerPathFeature.Path;            
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
