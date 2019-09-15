using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ExamContract.MainDbModels
{
    public partial class Exam : Entity
    {
        public Exam()
        {
            Questions = new List<Question>();
            Users = new List<User>();
        }
        [Required(ErrorMessage = "The {0} field is required.")]
        [MaxLength(200)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Code")]
        [MaxLength(5)]
        public string Code { get; set; }
        [Display(Name = "Min start")]
        public DateTime MinStart { get; set; }
        [Display(Name = "Max start")]
        public DateTime MaxStart { get; set; }
        [Display(Name = "Duration (minutes)")]
        public int DurationMinutes { get; set; }
        [Display(Name = "Max points")]
        public decimal MaxPoints { get; set; }       
       
        public List<Question> Questions { get; set; }
        public List<User> Users { get; set; }    
        
        [NotMapped]
        public int ParentId { get; set; }
    }
}
