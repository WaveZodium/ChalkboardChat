using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ChalkboardChat.DAL.Models;

namespace ChalkboardChat.DAL.Data;

public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }

    public DbSet<MessageModel> Messages { get; set; }
}
