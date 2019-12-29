using ExamContract.CourseModels;
using ExamCourseAPI.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ExamCourseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : MyBaseController<Course>
    {
        public CoursesController(Repository<Course> repo) : base(repo)
        {
        }
    }
}