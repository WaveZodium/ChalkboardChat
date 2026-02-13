using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChalkboardChat.DAL.Data;

public class IdentitySeeder {
    public static async Task SeedAdminAsync(IServiceProvider services) {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        const string adminRole = "Admin";
        const string adminUser = "admin";
        const string adminPass = "admin";

        if (!await roleManager.RoleExistsAsync(adminRole))
            await roleManager.CreateAsync(new IdentityRole(adminRole));

        var user = await userManager.FindByNameAsync(adminUser);

        if (user == null) {
            user = new IdentityUser { UserName = adminUser };
            await userManager.CreateAsync(user, adminPass);
            await userManager.AddToRoleAsync(user, adminRole);
        }
    }
}