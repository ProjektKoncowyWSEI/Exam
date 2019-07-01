using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamContract.MainDbModels
{
    public partial class Answer: Entity
    {
        public string Name { get; set; }
    }
}
