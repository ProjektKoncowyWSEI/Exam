using System;
using System.Collections.Generic;
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
        public decimal Points { get; set; }
    }
}
