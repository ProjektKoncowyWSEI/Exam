using ExamContract.TutorialModels;
using ExamTutorialsAPI.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ExamTutorialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : MyBaseController<User>
    {
        public UsersController(Repository<User> repo) : base(repo)
        {
        }
    }
}