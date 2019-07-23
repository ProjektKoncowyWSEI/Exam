using System;
using System.Collections.Generic;
using System.Text;

namespace ExamContract.CourseModels
{
    public class ExamCourse : ICourseTwoKey
    {
        public int CourseId { get; set; }
        public int Id { get; set; }       
    }
}
