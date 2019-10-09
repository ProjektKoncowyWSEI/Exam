using ExamContract.TutorialModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamContract.ExamDTO
{
    public class UserTutorialsDTO
    {
        public UserTutorialsDTO()
        {
            MyTutorials = new List<User>();
            AllTutorials = new List<Tutorial>();
        }
        public List<User> MyTutorials { get; set; }
        public List<Tutorial> AllTutorials { get; set; }
    }
}
