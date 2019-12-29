using System;
using System.ComponentModel.DataAnnotations;

namespace ExamContract.Auth
{
    public class Key
    {
        [Key]
        public string Name { get; set; }
        [MaxLength(20)]
        public string Role { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
