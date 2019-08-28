using System;
using System.ComponentModel.DataAnnotations;

namespace ExamContract.TutorialModels
{    
	public class Key
	{
        [Key]
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
