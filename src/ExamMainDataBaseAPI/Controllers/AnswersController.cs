using ExamContract.MainDbModels;
using ExamMainDataBaseAPI.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : MyBaseController<Answer>
    {
        public AnswersController(Repository<Answer> repo) : base(repo)
        {
        }
    }
}