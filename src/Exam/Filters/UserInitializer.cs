using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam.Filters
{
    public class UserInitializer
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public UserInitializer(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }
        public async Task CreateRolesAsync()
        {
            foreach (var role in Enum.GetNames(typeof(RoleEnum)))
            {
                var roleExists = await roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
        public async Task CreateDefaultUser()
        {
            var email = configuration.GetValue<string>("DefaultUser");
            var pass = configuration.GetValue<string>("DefaultPassowrd");
            var user = new IdentityUser { UserName = email, Email = email };
            var userExists = await userManager.FindByNameAsync(email) != null;
            if (!userExists)
            {
                var result = await userManager.CreateAsync(user, pass);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, RoleEnum.admin.ToString());
                }
            }            
        }
    }
}
