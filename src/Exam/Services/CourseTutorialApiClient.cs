using ExamContract.CourseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exam.Services
{
    public class CourseTutorialApiClient : CourseTwoKeyApiClient<TutorialCourse>
    {
        public CourseTutorialApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "CoursesAPIConnection", "Tutorials", "Exam_CoursesApiKey")
        {
        }
    }
}
