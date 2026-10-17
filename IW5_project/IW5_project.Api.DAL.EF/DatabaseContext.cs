using IW5_project.Api.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace IW5_project.Api.DAL.EF
{
    public class DatabaseContext : DbContext
    {
        public DbSet<PlaceEntity> Places { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Rules setup for Users table
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Role).IsRequired().HasMaxLength(20);
            });
        }
    }
}
