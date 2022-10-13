using birthreg.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace birthreg.Data
{
    public class SeedData
    {
        public async static Task Initialize(UserManager<User> userManager)
        {

            if (await userManager.FindByEmailAsync("admin@birthreg.com") == null)
            {
                User user = new User();
                user.UserName = "admin@birthreg.com";
                user.Email = "admin@birthreg.com";
                user.FirstName = "Admin";
                user.LastName = "Admin";


                IdentityResult result = await userManager.CreateAsync(user, "Admin@123");
                Debug.WriteLine(result.Succeeded);
            }


        }
    }
}
