using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.MainDbModels
{
    public partial class Answer
    {
        public Answer()
        {
            //QuestionAnswer = new HashSet<QuestionAnswer>();
        }
        
        public int Id { get; set; }
        public string answer { get; set; }

        //public virtual ICollection<QuestionAnswer> QuestionAnswer { get; set; }
    }
}
