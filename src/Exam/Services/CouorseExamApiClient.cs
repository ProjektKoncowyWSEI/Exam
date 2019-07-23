using ExamContract.CourseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Services
{
    public class CouorseExamApiClient : CourseTwoKeyApiClient<ExamCourse>
    {
        public CouorseExamApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "CoursesAPIConnection", "Exams")
        {
        }
    }
}
