using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IW5_project.Api.DAL.EF.Configurations;

public class PlaceConfiguration : IEntityTypeConfiguration<PlaceEntity>
{
    

    public void Configure(EntityTypeBuilder<PlaceEntity> builder)
    {
        builder.Property(p => p.name).HasMaxLength(Constants.MAX_NAME_LENGTH);
        builder.Property(p => p.description).HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);
        builder.Property(p => p.imageUrl).HasMaxLength(Constants.MAX_IMAGE_URL_LENGTH);

        builder.HasIndex(p => p.name);

        // implement when polozky is finished
        //builder.HasMany(p => p.items)
        //    .WithOne(i => i.Place)
        //    .HasForeignKey(i => i.PlaceId)
        //    .OnDelete(DeleteBehavior.Restrict);
    }
}