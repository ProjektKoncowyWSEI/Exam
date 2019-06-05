using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
    }
}
 