using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ExamContract.TutorialModels
{
    public partial class User : Entity
    {        
        public int TutorialId { get; set; }
        [NotMapped]
        public Tutorial Tutorial { get; set; }
    }
}
