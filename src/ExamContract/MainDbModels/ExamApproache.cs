using System;
using System.Collections.Generic;
using System.Text;

namespace ExamContract.MainDbModels
{
    public class ExamApproache
    {
        public int ExamId { get; set; }
        public string Login { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int DetailsId { get; set; }
    }
}
