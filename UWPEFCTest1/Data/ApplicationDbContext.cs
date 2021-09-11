using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPEFCTest1.Models;

namespace UWPEFCTest1.Data
{
    class ApplicationDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public ApplicationDbContext() : base()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(@"Data Source=myAukro.sqlite");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
