using IW5_project.Api.DAL.Common.Entities;
using IW5_project.API.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IW5_project.API.DAL.EF.Configurations
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
}