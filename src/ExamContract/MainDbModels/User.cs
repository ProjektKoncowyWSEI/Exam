using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExamContract.MainDbModels
{
    public partial class User : Entity
    {
        [Display(Name = "Points")]
        public decimal Points { get; set; }
    }
}
