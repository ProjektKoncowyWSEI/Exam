using System;
using System.Collections.Generic;
using System.Text;

namespace ExamContract.CourseModels
{
    public class Course : Entity
    {
        public Course()
        {
            Users = new HashSet<User>();
            ExamCourses = new HashSet<ExamCourse>();
            TutorialCourses = new HashSet<TutorialCourse>();
        }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<ExamCourse> ExamCourses { get; set; }
        public ICollection<TutorialCourse> TutorialCourses { get; set; }
    }
}
