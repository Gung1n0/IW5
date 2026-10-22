using IW5_project.Api.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IW5_project.Common.Models;

namespace IW5_project.Api.DAL.EF.Configurations
{
    public class ShoppingListItemConfiguration : IEntityTypeConfiguration<ShoppingListItemEntity>
    {
        public void Configure(EntityTypeBuilder<ShoppingListItemEntity> builder)
        {
            builder.HasIndex(l => new { l.ShoppingListId, l.ItemId }).IsUnique();

            builder.HasOne(l => l.Item)
                .WithMany()
                .HasForeignKey(l => l.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
    public class ShoppingListConfiguration : IEntityTypeConfiguration<ShoppingListEntity>
    {
        public void Configure(EntityTypeBuilder<ShoppingListEntity> builder)
        {
            builder.Property(sl => sl.Name).HasMaxLength(Constants.MAX_NAME_LENGTH);
            builder.Property(sl => sl.Store).HasMaxLength(Constants.MAX_NAME_LENGTH);
            builder.Property(sl => sl.Description).HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);

            builder.HasIndex(sl => sl.Name);

            builder.HasMany(sl => sl.Items)
                .WithOne(l => l.ShoppingList)
                .HasForeignKey(l => l.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
