using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Helpers
{
    public enum AnswerTypesEnum
    {
        [Display(Name = "Single")]
        single = 8,
        [Display(Name = "Multiple")]
        multiple = 16             
    }
}
