using ExamContract.CourseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exam.Services
{
    public class UsersCoursesApiClient : WebApiClient<User>
    {
        public UsersCoursesApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "CoursesAPIConnection", "Users", "Exam_CoursesApiKey")
        {
        }
    }
}
