// Lokal variabel för att använda inbyggda funktioner för att bygga appen 
using ChalkboardChat.BLL.Interfaces;
using ChalkboardChat.BLL.Services;
using ChalkboardChat.DAL;
using ChalkboardChat.DAL.Data;
using ChalkboardChat.DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Koppla till app-db (meddelanden)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));

// Koppla till databas för autentisering och auktorisering (genom Identity) 
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnection"))); // Rödmarkering pga delprojekt 

builder.Services.AddDefaultIdentity<IdentityUser>(options => // Rödmarkering pga delprojekt 
{
    // definiera krav för lösenord
    options.Password.RequireDigit = false; // anger att vi inte kräver siffror
    options.Password.RequireNonAlphanumeric = false; // anger att vi inte kräver specialtecken
    options.Password.RequireUppercase = false; // anger att vi inte kräver versaler
    options.Password.RequiredLength = 6; // anger att vi kräver minst 6 tecken i lösenordet
})
    .AddRoles<IdentityRole>() // aktiverar roller (RoleManager)
    .AddEntityFrameworkStores<AuthDbContext>(); // Rödmarkering pga delprojekt 

// Koppla ihop BLL/DAL med dependency injection
builder.Services.AddScoped<ChalkboardChat.DAL.Repositories.IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();

// definierar rollbaserad policy som kräver att användaren har rollen "Admin" för att få åtkomst till vissa delar av applikationen
builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

// Lägger till autentisering på alla sidor utom startsidan 
builder.Services.AddRazorPages(options =>
{
    // här säger vi: vi kan nå membersidan när vi är inloggade
    options.Conventions.AuthorizeFolder("/Member");
    // här säger vi: vi kan nå adminsidan när vi är inloggade OCH har rollen Admin
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
});

// Lägger till kakservice - omdirigerar användaren 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; // Om en användare inte är inloggad och försöker nå en skyddad sida, omdirigeras de till /Login
    options.AccessDeniedPath = "/NoAccess"; // Om en inloggad användare försöker nå en sida som kräver en roll de inte har, omdirigeras de till /AccessDenied
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnection")));

// koppla ihop bll-kontrakt med dal-implementation
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IMessageService, MessageService>();

// Lokal variabel för att bygga appen
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

// Seed the admin user and role.
using (var scope = app.Services.CreateScope()) {
    await IdentitySeeder.SeedAdminAsync(scope.ServiceProvider);
}

app.Run();


// UI - PRESENTATIONSLAGER (applikationens ansikte): 
// Razor Pages
// Controller (om vi använder MVC)
// ViewModel-klasser (om vi vill föra samman flera olika objekt i en vy så lägger vi till detta bakom vyn) 
// Anropar services som finns i BLL 
// Modelstate

// Klient (UI) -> Request -> IService -> Service (BLL) -> 
// IRepository -> Repository (DAL) -> Databas
// Detta följer MVC-mönstret och Razor Pages

// Blazor <-> Server 
// Använder Blazor när en vill arbeta med C# (och inte t ex JAVA) 
// Blazor Server: Körs på server. Live connection. SignalR (istället för cshtml). 
// Blazor WebAssembly: Körs på klienten. KRäver mer tänk kring säkerhet. cshtml-filer

// Ny branch för funktionalitet som ska implementeras
// REKOMMENDERAT att skriva kod på separata ansvarsområden
// Regelbundna COMMITS
// Regelbundna PULL

// PUSH branch
// Skapa PR (pull request) på GitHub (main <- branch) 
// VIKTIGT: Stå på MAIN och gör en PULL 
// Stå på din branch : GIT MERGE MAIN (då hamnar alla ändringar från main in i din branch
// Fortsätt arbeta 