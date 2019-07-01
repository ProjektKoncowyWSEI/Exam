using System;
using System.Collections.Generic;

namespace ExamContract.MainDbModels
{
    public partial class QuestionAnswer
    {
        public int QuestionId { get; set; }
        public int AnswerId { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Questions Question { get; set; }
    }
}
