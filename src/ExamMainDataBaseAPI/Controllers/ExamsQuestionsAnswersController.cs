using System.Threading.Tasks;
using ExamContract.Auth;
using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamsQuestionsAnswersController : ControllerBase
    {
        private UnitOfWork uow = null;

        public ExamsQuestionsAnswersController(UnitOfWork uow)
        {
            this.uow = uow;
        }
        [HttpGet("{id}")]
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher, RoleEnum.student)]
        public async Task<Exam> GetById(int id)
        {
            return await uow.GetExamwithQuestionsWithAnswers(id);
        }
        [HttpGet("{code}")]
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher, RoleEnum.student)]
        public async Task<Exam> GetByCode(string code)
        {
            return await uow.GetExamwithQuestionsWithAnswers(code);
        }
        [HttpPost]
        [KeyAuthorize(RoleEnum.admin, RoleEnum.teacher)]
        public async Task<ActionResult> Post(Exam item)
        {           
            await uow.Clone(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
    }
}