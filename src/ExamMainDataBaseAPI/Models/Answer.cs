using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamMainDataBaseAPI.Models
{
    public partial class Answer
    {
        public Answer()
        {
            QuestionAnswer = new HashSet<QuestionAnswer>();
        }
        
        public int Id { get; set; }
        public string Answer1 { get; set; }

        public virtual ICollection<QuestionAnswer> QuestionAnswer { get; set; }
    }
}
