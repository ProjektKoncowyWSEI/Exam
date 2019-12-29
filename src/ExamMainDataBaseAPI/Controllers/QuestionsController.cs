using Microsoft.AspNetCore.Mvc;
using ExamMainDataBaseAPI.DAL;
using ExamContract.MainDbModels;

namespace ExamMainDataBaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : MyBaseController<Question>
    {
        public QuestionsController(Repository<Question> repo) : base(repo)
        {
        }
    }
}