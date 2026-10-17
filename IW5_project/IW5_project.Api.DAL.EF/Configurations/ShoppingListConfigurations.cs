using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IW5_project.Api.DAL.EF.Configurations
{
    public class ShoppingListItemConfiguration : IEntityTypeConfiguration<ShoppingListItemEntity>
    {
        public void Configure(EntityTypeBuilder<ShoppingListItemEntity> builder)
        {
            builder.HasIndex(l => new { l.shoppingListId, l.itemId }).IsUnique();

            builder.HasOne(l => l.item)
                .WithMany()
                .HasForeignKey(l => l.itemId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}