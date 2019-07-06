using Exam.Services;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin)]
    public class ExamsController : MyBaseController<ExamContract.MainDbModels.Exam>
    {   
        public ExamsController(IStringLocalizer<SharedResource> localizer, WebApiClient<ExamContract.MainDbModels.Exam> service) : base(localizer, service) {}
        public override async Task<IActionResult> Index()
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
            return await base.Index();
        }       
    }
}