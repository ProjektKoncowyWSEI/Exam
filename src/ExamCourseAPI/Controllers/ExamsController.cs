using ExamContract.CourseModels;
using ExamCourseAPI.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ExamCourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : MyBaseTwoKeyController<ExamCourse>
    {
        public ExamsController(TwoKeysRepository<ExamCourse> repo) : base(repo)
        {

        }
    }
}