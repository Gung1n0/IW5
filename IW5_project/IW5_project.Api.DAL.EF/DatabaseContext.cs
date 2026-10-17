using IW5_project.API.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace IW5_project.API.DAL.EF
{
    public class DatabaseContext : DbContext
    {
        public DbSet<PlaceEntity> Places { get; set; } = null!;

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
