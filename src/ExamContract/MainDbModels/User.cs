using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExamContract.MainDbModels
{
    public partial class User
    {
        [Key]
        public string Login { get; set; }
    }
}
