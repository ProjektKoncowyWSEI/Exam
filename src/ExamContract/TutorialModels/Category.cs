using ExamContract;
using System;
using System.Collections.Generic;

namespace ExamContract.TutorialModels
{
    public class Category : Entity
    {  
        public string Name { get; set; }

        public virtual ICollection <Tutorial> Tutorials { get; set; }

    }
}
