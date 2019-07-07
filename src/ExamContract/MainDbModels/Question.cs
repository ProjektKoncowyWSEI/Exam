using Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.MainDbModels
{
    public partial class Question : Entity
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }
        public int? ExamId { get; set; }
        [MaxLength(200)]
        public string Name { get; set; }
        public string Content { get; set; }
        public int AnswerType { get; set; }
        [NotMapped]
        public AnswerTypesEnum AnswerTypesEnum
        {
            get
            {
                return (Helpers.AnswerTypesEnum)AnswerType;
            }
            set
            {
                AnswerType = (int)value;
                answerTypesEnum = value;
            }
        }
        private AnswerTypesEnum answerTypesEnum;       

        public ICollection<Answer> Answers { get; set; }
    }
}
