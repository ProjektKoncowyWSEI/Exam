using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.MainDbModels
{
    public partial class Answer: Entity
    {
        [MaxLength(200)]
        public string Name { get; set; }
        public string Content { get; set; }
        public decimal Points { get; set; }
        public int QuestionId { get; set; }
    }
}
