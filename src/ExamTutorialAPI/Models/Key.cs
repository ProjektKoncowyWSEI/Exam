using System;
using System.ComponentModel.DataAnnotations;

namespace ExamTutorialsAPI.Models
{ 
    
	public class Key
	{
        [Key]
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
