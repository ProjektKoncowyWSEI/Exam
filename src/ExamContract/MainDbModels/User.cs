using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.MainDbModels
{
    public partial class User : Entity
    {
        public int ExamId { get; set; }   
        [NotMapped]
        public Exam Exam { get; set; }
    }
}
