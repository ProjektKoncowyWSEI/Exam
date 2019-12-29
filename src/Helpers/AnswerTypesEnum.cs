using System.ComponentModel.DataAnnotations;

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
