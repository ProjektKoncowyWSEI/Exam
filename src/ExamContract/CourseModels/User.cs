using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ExamContract.CourseModels
{
    public partial class User : Entity
    {        
        public int CourseId { get; set; }
        [NotMapped]
        public Course Course { get; set; }
    }
}
