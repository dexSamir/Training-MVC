﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Training.Core.Entities;
using Training.Core.Enums;

namespace Training.BL.Extensions;

public static class SeedExtension
{
    public async static void UseUserSeed(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!roleManager.Roles.Any())
            {
                foreach (Roles role in Enum.GetValues(typeof(Roles)))
                    await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }

            if (!userManager.Users.Any(x => x.NormalizedUserName == "ADMIN"))
            {
                User admin = new User
                {
                    Fullname = "admin",
                    UserName = "admin",
                    Email = "admin@gmail.com",
                };

                await userManager.CreateAsync(admin, "admin253admin");

                await userManager.AddToRoleAsync(admin, nameof(Roles.Admin));
            }
        }
    }
}