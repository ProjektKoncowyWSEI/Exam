using System;

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
