using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.CourseModels
{
    public partial class User : Entity
    {        
        public int CourseId { get; set; }
        [NotMapped]
        public Course Course { get; set; }
    }
}
