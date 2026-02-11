using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChalkboardChat.DAL.Data;
internal class AuthDbContext : IdentityDbContext<IdentityUser> {
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) {
        // Options innehåller connection string och inställningar för EF Core
        // så att EF vet hur den ska ansluta till databasen.
    }
}
