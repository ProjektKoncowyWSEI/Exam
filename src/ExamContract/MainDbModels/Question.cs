using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.MainDbModels
{
    public partial class Question : Entity
    {
        public Question()
        {
            //QuestionAnswer = new HashSet<QuestionAnswer>();
        }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public string AnswerType { get; set; }

        //public virtual ICollection<QuestionAnswer> QuestionAnswer { get; set; }
        [NotMapped]
        public ICollection<Answer> Answers { get; set; }
    }
}
