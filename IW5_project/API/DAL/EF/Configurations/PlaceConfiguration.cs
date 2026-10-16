using IW5_project.API.DAL.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IW5_project.API.DAL.EF.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<PlaceEntity>
{
    const int MAX_NAME_LENGTH = 100;
    const int MAX_DESCRIPTION_LENGTH = 500;
    const int MAX_IMAGE_URL_LENGTH = 2048;

    public void Configure(EntityTypeBuilder<PlaceEntity> builder)
    {
        builder.Property(p => p.name).HasMaxLength(MAX_NAME_LENGTH);
        builder.Property(p => p.description).HasMaxLength(MAX_DESCRIPTION_LENGTH);
        builder.Property(p => p.imageUrl).HasMaxLength(MAX_IMAGE_URL_LENGTH);

        builder.HasIndex(p => p.name);

        // implement when polozky is finished
        //builder.HasMany(p => p.items)
        //    .WithOne(i => i.Place)
        //    .HasForeignKey(i => i.PlaceId)
        //    .OnDelete(DeleteBehavior.Restrict);
    }
}