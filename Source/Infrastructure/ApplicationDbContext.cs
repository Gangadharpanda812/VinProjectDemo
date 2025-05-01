using Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ApiLogs> ApiLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .HasIndex(v => v.Vin)
                //.HasDatabaseName("idx_vehicles_vin")
                .IsUnique(); // optional: if the index should be unique
        }
    }
}