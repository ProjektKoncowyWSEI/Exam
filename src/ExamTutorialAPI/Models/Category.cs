using ExamTutorialsAPI.DAL;
using System;
using System.Collections.Generic;

namespace ExamTutorialsAPI.Models
{
    public class Category: Entity
    {       
       public int Id { get; set;  }
        public string Name { get; set; }

        public virtual ICollection <Tutorial> Tutorials { get; set; }

    }
}
