using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChalkboardChat.DAL.Data;

public class AuthDbContext : IdentityDbContext<IdentityUser> {
    // The constructor takes DbContextOptions and passes it to the base class constructor
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
}
