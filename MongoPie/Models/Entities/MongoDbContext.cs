using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoPie.Models.Entities
{
    public class MongoDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mongopie.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MongoConnection>()
                .ToTable("mongoconnections")
                .Ignore(m => m.DatabaseKey)
                .HasKey(m => m.Id);
        }
        public DbSet<MongoConnection> mongoConnections { get; set; }
    }
}
