using ExamContract;
using System.ComponentModel.DataAnnotations;

namespace ExamContract.TutorialModels
{
    public class Tutorial : Entity
    {       
        [MaxLength(100)]
        [Required]
        [Display(Name = "Tutorial name")]
        public string Name { get; set; }
        [Display(Name = "Image")]
        public byte[] Image { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}



