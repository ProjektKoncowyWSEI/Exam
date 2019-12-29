using System.ComponentModel.DataAnnotations;

namespace ExamContract.Auth
{
    public enum RoleEnum
    {
        [Display(Name = "Lack")]
        lack = 0,
        [Display(Name = "Student")]
        student = 8,
        [Display(Name = "Teacher")]
        teacher = 16,
        [Display(Name = "Admin")]
        admin = 64        
    }
}
