using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamTutorialsAPI.Models
{
    public class Tutorial
    {
        public int ID { get; set; }
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public byte[] Image { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }


    }
}



