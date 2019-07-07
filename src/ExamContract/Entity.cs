using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExamContract
{
    public class Entity
    {
        public int Id { get; set; }
        [MaxLength(256)]
        [Display(Name = "Login")]
        public string Login { get; set; }
    }
}
