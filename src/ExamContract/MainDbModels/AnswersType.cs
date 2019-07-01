using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamContract.MainDbModels
{
    public partial class AnswersType
    {
        [Key]
        public string AnswerType { get; set; }
    }
}
