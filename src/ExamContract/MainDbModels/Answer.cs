﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.MainDbModels
{
    public partial class Answer: Entity
    {
        [MaxLength(200)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Content")]
        public string Content { get; set; }
        [Display(Name = "Points")]
        public decimal Points { get; set; }        
        public int QuestionId { get; set; }
    }
}
