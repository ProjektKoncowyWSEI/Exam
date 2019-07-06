using Exam.Services;
using Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using ExamContract.MainDbModels;

namespace Exam.Controllers
{
    [AuthorizeByRoles(RoleEnum.admin)]
    public class QuestionsController : MyBaseController<Question>
    {   
        public QuestionsController(IStringLocalizer<SharedResource> localizer, WebApiClient<Question> service) : base(localizer, service) {}        
    }
}