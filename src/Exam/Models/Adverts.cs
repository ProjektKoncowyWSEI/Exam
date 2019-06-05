using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public partial class Adverts
    {
        public Adverts()
        {
            AdvertsCategories = new HashSet<AdvertsCategories>();
        }

        public int AdvertId { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Title { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public string Content { get; set; }
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }

        public Locations Location { get; set; }
        public Users User { get; set; }
        public ICollection<AdvertsCategories> AdvertsCategories { get; set; }
    }
}
