using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChalkboardChat.UI.Context
{

    // Här startar Identity-systemet
    // ...som skapar automatiskt AspNetUsers och AspNetRoles
    // ...och tillhandahåller DbSet
    public class AuthDbContext : IdentityDbContext 
    {
        // KONSTRUKTOR 
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
    }
}
