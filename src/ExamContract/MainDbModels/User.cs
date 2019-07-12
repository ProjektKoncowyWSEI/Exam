using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ExamContract.MainDbModels
{
    public partial class User : Entity
    {
        public int ExamId { get; set; }      
    }
}
