using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ExamContract.MainDbModels
{
    public class ExamApproacheResult
    {
        public string Login { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }
        public bool Checked { get; set; }
        [Display(Name = "Points")]
        public decimal Points { get; set; }       
    }
}
