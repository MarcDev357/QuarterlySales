using Microsoft.AspNetCore.Identity;

namespace QuarterlySales.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string adminRole = "Admin";
            string username = "admin";
            string password = "Sesame";

            // Create Admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // Check if admin user exists
            var user = await userManager.FindByNameAsync(username);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = username
                };

                var result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                    return;
            }

            // Reload user from database
            user = await userManager.FindByNameAsync(username);

            // Ensure admin role assigned
            if (!await userManager.IsInRoleAsync(user, adminRole))
            {
                await userManager.AddToRoleAsync(user, adminRole);
            }
        }
    }
}