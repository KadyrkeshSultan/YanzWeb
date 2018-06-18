using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Yanz.DAL.Entities;

namespace Yanz.Web.Services
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "sultan.kadyrkesh@yandex.ua";
            string password = "1307mama1307";
            if(await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));
            if(await roleManager.FindByNameAsync("teacher") == null)
                await roleManager.CreateAsync(new IdentityRole("teacher"));
            if (await roleManager.FindByNameAsync("student") == null)
                await roleManager.CreateAsync(new IdentityRole("student"));
            if (await roleManager.FindByNameAsync("other") == null)
                await roleManager.CreateAsync(new IdentityRole("other"));

            if(await userManager.FindByNameAsync(adminEmail) == null)
            {
                AppUser admin = new AppUser { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                    await userManager.ConfirmEmailAsync(admin, await userManager.GenerateEmailConfirmationTokenAsync(admin));
                }
            }
        }
    }
}
