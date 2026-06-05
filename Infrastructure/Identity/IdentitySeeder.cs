using Core.Constants;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Support))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Support));
            }

            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
        }
    }
}
