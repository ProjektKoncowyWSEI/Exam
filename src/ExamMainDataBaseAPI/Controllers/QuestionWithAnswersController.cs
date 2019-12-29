using System.Threading.Tasks;
using ExamContract.Auth;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionWithAnswersController : ControllerBase
    {
        private UnitOfWork uow = null;

        public QuestionWithAnswersController(UnitOfWork uow)
        {
            this.uow = uow;
        }
        [HttpGet("{id}")]
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher, RoleEnum.student)]
        public async Task<Question> Get(int id)
        {
            return await uow.GetQuestionWithAnswer(id); 
        }
    }
}