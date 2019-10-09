using ExamContract.MainDbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExamContract.ExamDTO
{
    public class ExamApproacheDTO
    {        
        public string Login { get; set; }
        public int ExamId { get; set; }
        public string Code { get; set; }
        public List<Question> Questions { get; set; }      
        public class Question
        {
            public int Id { get; set; }
            public List<Answer> Answers { get; set; }
            public class Answer
            {
                public int Id { get; set; }
                public bool Checked { get; set; }
            }
        }       
    }
}
