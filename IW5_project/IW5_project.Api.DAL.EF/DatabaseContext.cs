using IW5_project.IW5_project.Api.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace IW5_project.IW5_project.Api.DAL.EF
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
