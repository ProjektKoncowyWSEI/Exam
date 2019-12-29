using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.TutorialModels
{
    public partial class User : Entity
    {        
        public int TutorialId { get; set; }
        [NotMapped]
        public Tutorial Tutorial { get; set; }
    }
}
