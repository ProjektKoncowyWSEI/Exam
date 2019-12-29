using ExamContract.MainDbModels;
using System.Collections.Generic;

namespace ExamContract.ExamDTO
{
    public class UserExamsDTO
    {
        public UserExamsDTO()
        {
            MyExams = new List<User>();
            AllExams = new List<Exam>();
        }
        public List<User> MyExams { get; set; }
        public List<Exam> AllExams { get; set; }
    }
}
