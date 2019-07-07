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
        public string Name { get; set; }
        public DateTime MinStart { get; set; }
        public DateTime MaxStart { get; set; }
        public int DurationMinutes { get; set; }
        public decimal MaxPoints { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
