using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class CoursesApiClient : WebApiClient<ExamContract.CourseModels.Course>
    {
        public CoursesApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "CoursesAPIConnection", "Courses")
        {
        }
    }
}
