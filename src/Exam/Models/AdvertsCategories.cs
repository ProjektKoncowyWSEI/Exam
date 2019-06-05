using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Models
{
    public partial class AdvertsCategories
    {
        public int AdvertId { get; set; }
        public int CategoryId { get; set; }

        public Adverts Advert { get; set; }
        public Categories Category { get; set; }
    }
}
