using CozyCafe.Models.Domain.ForUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CozyCafe.Application.Initialization
{
    /// <summary>
    /// (UA) Клас для початкової ініціалізації бази даних. 
    /// Використовується для створення базових ролей (Admin, User) та 
    /// додавання адміністратора за замовчуванням.
    /// 
    /// (EN) Class for database initialization. 
    /// Used to create default roles (Admin, User) and 
    /// add a default administrator account.
    /// </summary>
    public class DbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminEmail = "Admin@cozycafe.com";
            var adminPassword = "Admin123!"; 

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin"
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }

    }
}
