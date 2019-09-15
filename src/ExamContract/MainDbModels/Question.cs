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
            Answers = new List<Answer>();
        }
        public int? ExamId { get; set; }
        [MaxLength(200)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Content")]
        public string Content { get; set; }
        [Display(Name = "Answer Type")]
        public int AnswerType { get; set; }
        [NotMapped]
        [Display(Name = "Answer Type")]
        public AnswerTypesEnum AnswerTypesEnum
        {
            get
            {
                return (Helpers.AnswerTypesEnum)AnswerType;
            }
            set
            {
                AnswerType = (int)value;           
            }
        }              

        public List<Answer> Answers { get; set; }
    }
}
