﻿using ExamTutorialsAPI.DAL;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamTutorialsAPI.Models
{
    public class Tutorial : Entity
    {      
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public IFormFile ImageFile { get; set; }
        public string Description { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }


    }
}



