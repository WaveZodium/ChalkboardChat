// Lokal variabel fö�r att anv�nda inbyggda funktioner f�r att bygga appen 
using ChalkboardChat.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//// Koppla till databas f�r autentisering och auktorisering (genom Identity) 
//builder.Services.AddDbContext<AuthDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthConnection"))); // R�dmarkering pga delprojekt 
//builder.Services.AddDefaultIdentity<IdentityUser>(options => // R�dmarkering pga delprojekt 
//{
//    // Definiera krav f�r l�senord
//    options.Password.RequireDigit = false; // Anger att vi inte kr�ver siffror
//    options.Password.RequireNonAlphanumeric = false; // Anger att vi inte kr�ver specialtecken
//    options.Password.RequireUppercase = false; // Anger att vi inte kr�ver versaler
//    options.Password.RequiredLength = 6; // Anger att vi kr�ver minst 6 tecken i l�senordet

//})
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<AuthDbContext>(); // R�dmarkering pga delprojekt 

// Definierar rollbaserad policy som kr�ver att anv�ndaren har rollen "Admin" f�r att f� �tkomst till vissa delar av applikationen
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// L�gger till autentisering p� alla sidor utom startsidan 
builder.Services.AddRazorPages(options =>
{
    // H�r s�ger vi: Vi kan n� membersidan n�r vi �r inloggade
    options.Conventions.AuthorizeFolder("/Member");
    // H�r s�ger vi: Vi kan n� adminsidan n�r vi �r inloggade OCH har rollen Admin
    options.Conventions.AuthorizeFolder("/Admin", "AdminOnly");
});

// L�gger till kakservice - omdirigerar anv�ndaren 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; // Om en anv�ndare inte �r inloggad och f�rs�ker n� en skyddad sida, omdirigeras de till /Login
    options.AccessDeniedPath = "/NoAccess"; // Om en inloggad anv�ndare f�rs�ker n� en sida som kr�ver en roll de inte har, omdirigeras de till /AccessDenied
});

// Lokal variabel f�r att bygga appen
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

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();


// UI - PRESENTATIONSLAGER (applikationens ansikte): 
// Razor Pages
// Controller (om vi anv�nder MVC)
// ViewModel-klasser (om vi vill f�ra samman flera olika objekt i en vy s� l�gger vi till detta bakom vyn) 
// Anropar services som finns i BLL 
// Modelstate

// Klient (UI) -> Request -> IService -> Service (BLL) -> 
// IRepository -> Repository (DAL) -> Databas
// Detta f�ljer MVC-m�nstret och Razor Pages

// Blazor <-> Server 
// Anv�nder Blazor n�r en vill arbeta med C# (och inte t ex JAVA) 
// Blazor Server: K�rs p� server. Live connection. SignalR (ist�llet f�r cshtml). 
// Blazor WebAssembly: K�rs p� klienten. KR�ver mer t�nk kring s�kerhet. cshtml-filer

// Ny branch f�r funktionalitet som ska implementeras
// REKOMMENDERAT att skriva kod p� separata ansvarsomr�den
// Regelbundna COMMITS
// Regelbundna PULL

// PUSH branch
// Skapa PR (pull request) p� GitHub (main <- branch) 
// VIKTIGT: St� p� MAIN och g�r en PULL 
// St� p� din branch : GIT MERGE MAIN (d� hamnar alla �ndringar fr�n main in i din branch
// Forts�tt arbeta 
