using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Models;

namespace WarehouseManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Warehouse
            modelBuilder.Entity<Warehouse>()
                .HasMany(w => w.Shelves)
                .WithOne(s => s.Warehouse)
                .HasForeignKey(s => s.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Shelf
            modelBuilder.Entity<Shelf>()
                .HasMany(s => s.Locations)
                .WithOne(l => l.Shelf)
                .HasForeignKey(l => l.ShelfId)
                .OnDelete(DeleteBehavior.Cascade);

            // Location
            modelBuilder.Entity<Location>()
                .HasMany(l => l.Items)
                .WithOne(i => i.Location)
                .HasForeignKey(i => i.LocationId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        
    }
}
