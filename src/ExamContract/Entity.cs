using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract
{
    public class Entity
    {
        public Entity()
        {
            Active = true;
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(256)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
