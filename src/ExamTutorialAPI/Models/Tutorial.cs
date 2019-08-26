using ExamTutorialsAPI.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ExamTutorialsAPI.Models
{
    [AllowAnonymous]
    public class Tutorial : Entity
    {
        //private readonly IStringLocalizer<SharedResource> localizer;

        //public Tutorial(IStringLocalizer<SharedResource> localizer)
        //{
        //    this.localizer = localizer;
        //}
    [MaxLength(100)]
        [Required]
        [Display(Name = "Tutorial name")]
        public string Name { get; set; }
        public byte[] Image { get; set; }

        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }


    }
}



