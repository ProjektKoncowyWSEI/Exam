using ExamContract.CourseModels;
using System.Collections.Generic;

namespace ExamContract.ExamDTO
{
    public class UserCoursesDTO
    {
        public UserCoursesDTO()
        {
            MyCourses = new List<User>();
            AllCourses = new List<Course>();
        }
        public List<User> MyCourses { get; set; }
        public List<Course> AllCourses { get; set; }
    }
}
