using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChalkboardChat.DAL.Data;

public class IdentitySeeder {
    public static async Task SeedAdminAsync(IServiceProvider services) {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        const string adminRole = "Admin";
        const string adminUser = "admin";
        const string adminPass = "Admin1234!";

        //ensure role exists
        if (!await roleManager.RoleExistsAsync(adminRole)) {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(adminRole));
            if (!roleResult.Succeeded)
                throw new InvalidOperationException($"failed to create role '{adminRole}': {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
        }

        var user = await userManager.FindByNameAsync(adminUser);

        //ensure user exists
        if (user == null) {
            user = new IdentityUser { UserName = adminUser };

            var userResult = await userManager.CreateAsync(user, adminPass);
            if (!userResult.Succeeded)
                throw new InvalidOperationException($"failed to create user '{adminUser}': {string.Join(", ", userResult.Errors.Select(e => e.Description))}");
        }

        //ensure user is in role
        if (!await userManager.IsInRoleAsync(user, adminRole)) {
            var addToRoleResult = await userManager.AddToRoleAsync(user, adminRole);
            if (!addToRoleResult.Succeeded)
                throw new InvalidOperationException($"failed to add user '{adminUser}' to role '{adminRole}': {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
        }
    }
}