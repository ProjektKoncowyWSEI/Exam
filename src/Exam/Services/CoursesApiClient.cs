using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exam.Services
{
    public class CoursesApiClient : WebApiClient<ExamContract.CourseModels.Course>
    {
        public CoursesApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "CoursesAPIConnection", "Courses", "Exam_CoursesApiKey")
        {
        }
    }
}
