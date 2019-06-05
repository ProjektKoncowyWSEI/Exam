using System.Collections.Generic;

namespace Exam.Models
{
    public partial class Users
    {
        public Users()
        {
            Adverts = new HashSet<Adverts>();
        }

        public int UserId { get; set; }
        public string Nickname { get; set; }
        public string Pass { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Range(1, 100)]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        public int Age { get; set; }
        public string Email { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [Required]
        [StringLength(30)]
        public string PhoneNumber { get; set; }

        [NotMapped]
        public byte[] ImageColumn { get; set; }

        public ICollection<Adverts> Adverts { get; set; }
    }
}
