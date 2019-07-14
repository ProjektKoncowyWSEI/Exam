using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExamContract.MainDbModels
{
    public partial class Exam : Entity
    {
        public Exam()
        {
            Questions = new HashSet<Question>();
            Users = new HashSet<User>();
        }
        [Required]
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
       
        public ICollection<Question> Questions { get; set; }
        public ICollection<User> Users { get; set; }
        public override string ToString()
        {
            return $"Name: {Name} ** Code: {Code} ** {MinStart} - {MaxStart} ** Time: {DurationMinutes} min";
        }
    }
}
