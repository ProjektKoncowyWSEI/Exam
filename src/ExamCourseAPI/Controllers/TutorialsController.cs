using ExamContract.CourseModels;
using ExamCourseAPI.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ExamCourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutorialsController : MyBaseTwoKeyController<TutorialCourse>
    {
        public TutorialsController(TwoKeysRepository<TutorialCourse> repo) : base(repo)
        {

        }
    }
}