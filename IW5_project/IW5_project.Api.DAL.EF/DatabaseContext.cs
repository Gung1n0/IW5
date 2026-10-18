using IW5_project.Api.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace IW5_project.Api.DAL.EF
{
    public class DatabaseContext : DbContext
    {
        public DbSet<PlaceEntity> Places { get; set; } = null!;
        public DbSet<ShoppingListEntity> ShoppingLists { get; set; } = null!;
        public DbSet<ShoppingListItemEntity> ShoppingListItems { get; set; } = null!;
        public DbSet<ItemEntity> Items { get; set; } = null!;
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
