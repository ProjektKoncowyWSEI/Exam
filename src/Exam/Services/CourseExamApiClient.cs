﻿using ExamContract.CourseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Exam.Services
{
    public class CourseExamApiClient : CourseTwoKeyApiClient<ExamCourse>
    {
        public CourseExamApiClient(ILogger logger, IConfiguration configuration) : base(logger, configuration, "CoursesAPIConnection", "Exams", "Exam_CoursesApiKey")
        {
        }
    }
}
