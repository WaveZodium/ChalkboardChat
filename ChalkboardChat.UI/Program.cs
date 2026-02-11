using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using ChalkboardChat.DAL.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add EF Core DbContext for the application data.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("AppConnection")
));

// Add EF Core DbContext for authentication and identity management.
builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("AuthConnection")
));

// Add Identity services to the container.
builder.Services.AddIdentity<IdentityUser, IdentityRole>(
        options => {
            // Configure password requirements
            // This setting is for demonstration purposes only.
            // In a production application, you should use stronger password requirements.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
            options.Password.RequiredUniqueChars = 0;
        })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

// Configure authentication and authorization services.
builder.Services.AddAuthorization(options => {
    // Policy är ett namn på en regel som kan innehålla olika krav.
    // I det här fallet skapar vi en policy som kräver att användaren har rollen "Admin".
    options.AddPolicy(
        "AdminOnly",
        policy => policy.RequireRole("Admin"));
});

// Configure authorization for Razor Pages.
builder.Services.AddRazorPages(options => {
    options.Conventions.AuthorizeFolder("/Member"); // Alla sidor i /Member kräver att användaren är inloggad
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly"); // Alla sidor i /Admin kräver att användaren har rollen "Admin"
});

builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Login";
    options.LogoutPath = "/Logout";
    options.AccessDeniedPath = "/NoAccess";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
