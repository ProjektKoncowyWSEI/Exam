namespace Exam.Models
{
    public partial class Locations
    {
        public Locations()
        {
            Adverts = new HashSet<Adverts>();
        }

        public int LocationId { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        [Display(Name = "Release Date")]
        public string Country { get; set; }
        public string Region { get; set; }
        public string County { get; set; }
        public string City { get; set; }

        public ICollection<Adverts> Adverts { get; set; }
    }
}
