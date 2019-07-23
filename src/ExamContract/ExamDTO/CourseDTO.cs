using ExamContract.CourseModels;
using ExamContract.MainDbModels;
using System;
using System.Collections.Generic;
using System.Text;
using User = ExamContract.CourseModels.User;

namespace ExamContract.ExamDTO
{
    public class CourseDTO
    {
        public CourseDTO()
        {
            Course = new Course();
            Exams = new HashSet<Exam>();
            Users = new HashSet<User>();
        }
        public Course Course { get; set; }
        public ICollection<Exam> Exams { get; set; }
        public ICollection<User> Users { get; set; }
        //public List<Tutorial> Tutorials { get; set; }
    }
}
