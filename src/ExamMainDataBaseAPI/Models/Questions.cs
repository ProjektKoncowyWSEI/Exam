using System;
using System.Collections.Generic;

namespace ExamMainDataBaseAPI.Models
{
    public partial class Questions
    {
        public Questions()
        {
            QuestionAnswer = new HashSet<QuestionAnswer>();
        }

        public int Id { get; set; }
        public string Question { get; set; }
        public byte[] Image { get; set; }
        public string AnswerType { get; set; }

        public virtual ICollection<QuestionAnswer> QuestionAnswer { get; set; }
    }
}
